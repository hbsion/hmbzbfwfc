using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brDfwcust : IBrObject<dhm_cust>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string UnitName = "UnitName";
            public const string UnitCode = "UnitCode";
        }

        public class QueryContext
        {
            public string UnitName { get; set; }
            public string UnitCode { get; set; }

            public string sunitcode { get; set; }

            public string seartext { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brDfwcust()
        {
            db = new dbDataContext(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
        }

        private dbDataContext db;

        public void Dispose()
        {
            db.Dispose();
            db = null;
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        void IBrObject<dhm_cust>.CheckAction(dhm_cust entity, ActionEnum action) { }

        private IQueryable<dhm_cust> GetByFilter(IQueryable<dhm_cust> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.UnitName:
                    return q.Where(x => x.UnitName.StartsWith(filter.SearchText));
                case FieldType.UnitCode:
                    return q.Where(x => x.UnitCode == filter.SearchText);

                default:
                    return q;
            }
        }

        private IQueryable<dhm_cust> GetBySorter(IQueryable<dhm_cust> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.UnitName:
                    return sorter.ASC ? q.OrderBy(x => x.UnitName) : q.OrderByDescending(x => x.UnitName);
                case FieldType.UnitCode:
                    return sorter.ASC ? q.OrderBy(x => x.UnitCode) : q.OrderByDescending(x => x.UnitCode);

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<dhm_cust> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.dhm_cust, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.dhm_cust, filter).Count();
        }

        public dhm_cust Retrieve(string inCode)
        {
            return db.dhm_cust.FirstOrDefault(x => x.UnitCode == inCode);
        }

        public dhm_cust Retrieve(int inCode)
        {
            return db.dhm_cust.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<dhm_cust> GetAll(QueryContext context)
        {
            IQueryable<dhm_cust> ret = db.dhm_cust;

            if (context.UnitName != null && context.UnitName.Trim().Length > 0)
                ret = ret.Where(x => x.UnitName.StartsWith(context.UnitName));

            if (context.UnitCode != null && context.UnitCode.Trim().Length > 0)
                ret = ret.Where(x => x.UnitCode == context.UnitCode);

            if (context.sunitcode != null && context.sunitcode.Trim().Length > 0)
                ret = ret.Where(x => context.sunitcode.IndexOf(x.UnitCode) >= 0 && x.UnitCode.Trim().Length == 4);


            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.UnitName.Contains(context.seartext) || x.UnitCode.Contains(context.seartext));

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<dhm_cust> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<dhm_cust> Query()  //返回全部
        {

            IQueryable<dhm_cust> ret = db.dhm_cust;
            return ret.OrderBy(x => x.UnitName);
        }

        public IEnumerable<dhm_cust> Query(string inUnitCode)
        {

            IQueryable<dhm_cust> ret = db.dhm_cust;
            ret = ret.Where(x => x.UnitCode == inUnitCode);

            return ret.OrderBy(x => x.UnitName);
        }




        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }


        public void Insert(dhm_cust entity)
        {
            lock (db)
            {
                bool closeConn = false;
                if (db.Connection.State != ConnectionState.Open)
                {
                    db.Connection.Open();
                    closeConn = true;
                }

                try
                {
                    db.Transaction = db.Connection.BeginTransaction();
                    if (db.dhm_cust.FirstOrDefault(x => x.UnitCode == entity.UnitCode) == null)
                    {
                        db.dhm_cust.InsertOnSubmit(entity);
                        db.SubmitChanges();
                    }
                    db.Transaction.Commit();
                }
                catch
                {
                    if (db.Transaction != null) db.Transaction.Rollback();
                    throw;
                }
                finally
                {
                    db.Transaction = null;
                    if (closeConn) db.Connection.Close();
                }
            }
        }

        public void Update(dhm_cust entity)
        {
            lock (db)
            {
                bool closeConn = false;
                if (db.Connection.State != ConnectionState.Open)
                {
                    db.Connection.Open();
                    closeConn = true;
                }

                try
                {
                    db.Transaction = db.Connection.BeginTransaction();
                    dhm_cust dbdata = db.dhm_cust.FirstOrDefault(x => x.UnitCode == entity.UnitCode);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<dhm_cust>(entity, dbdata);
                    else
                        throw new Exception("更新失败，该记录可能已被其他用户删除。");
                    db.SubmitChanges();

                    db.Transaction.Commit();
                }
                catch
                {
                    if (db.Transaction != null) db.Transaction.Rollback();
                    throw;
                }
                finally
                {
                    db.Transaction = null;
                    if (closeConn) db.Connection.Close();
                }
            }
        }


        public void Delete(string inCode)
        {
            lock (db)
            {
                bool closeConn = false;
                if (db.Connection.State != ConnectionState.Open)
                {
                    db.Connection.Open();
                    closeConn = true;
                }

                try
                {
                    db.Transaction = db.Connection.BeginTransaction();
                    dhm_cust dbdata = db.dhm_cust.FirstOrDefault(x => x.UnitCode == inCode);
                    if (dbdata != null)
                        db.dhm_cust.DeleteOnSubmit(dbdata);
                    else
                        throw new Exception("删除失败，该记录可能已被其他用户删除。");
                    db.SubmitChanges();

                    db.Transaction.Commit();
                }
                catch
                {
                    if (db.Transaction != null) db.Transaction.Rollback();
                    throw;
                }
                finally
                {
                    db.Transaction = null;
                    if (closeConn) db.Connection.Close();
                }
            }
        }

        public void Delete(dhm_cust entity)
        {
            this.Delete(entity.UnitCode);
        }


        public dhm_cust NewEntity()
        {
            return new dhm_cust();
        }


        public void ChangPasswd(string inUserbm, string inNewPwd)
        {
            lock (db)
            {
                bool closeConn = false;
                if (db.Connection.State != ConnectionState.Open)
                {
                    db.Connection.Open();
                    closeConn = true;
                }

                try
                {
                    db.Transaction = db.Connection.BeginTransaction();

                    dhm_cust User = db.dhm_cust.FirstOrDefault(b => b.UnitCode == inUserbm);
    
                    db.SubmitChanges();
                    db.Transaction.Commit();


                }
                catch
                {
                    if (db.Transaction != null) db.Transaction.Rollback();
                    throw;
                }
                finally
                {
                    db.Transaction = null;
                    if (closeConn) db.Connection.Close();
                }
            }
        }

    }
}
