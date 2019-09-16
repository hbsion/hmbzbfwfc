using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brfwcust : IBrObject<fw_cust>
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

            public string seartext { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brfwcust()
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

        void IBrObject<fw_cust>.CheckAction(fw_cust entity, ActionEnum action) { }

        private IQueryable<fw_cust> GetByFilter(IQueryable<fw_cust> q, Filter filter)
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

        private IQueryable<fw_cust> GetBySorter(IQueryable<fw_cust> q, Sorter sorter)
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

        public IEnumerable<fw_cust> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.fw_cust, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.fw_cust, filter).Count();
        }

        public fw_cust Retrieve(string inCode)
        {
            return db.fw_cust.FirstOrDefault(x => x.UnitCode == inCode);
        }

        public fw_cust Retrieve(int inCode)
        {
            return db.fw_cust.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<fw_cust> GetAll(QueryContext context)
        {
            IQueryable<fw_cust> ret = db.fw_cust;

            if (context.UnitName != null && context.UnitName.Trim().Length > 0)
                ret = ret.Where(x => x.UnitName.StartsWith(context.UnitName));
            if (context.UnitCode != null && context.UnitCode.Trim().Length > 0)
                ret = ret.Where(x => x.UnitCode == context.UnitCode);


            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.UnitName.Contains(context.seartext) || x.UnitCode.Contains(context.seartext));

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<fw_cust> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<fw_cust> Query()  //返回全部
        {

            IQueryable<fw_cust> ret = db.fw_cust;


            return ret.OrderBy(x => x.UnitName);
        }

        public IEnumerable<fw_cust> Query(string inUnitCode)
        {

            IQueryable<fw_cust> ret = db.fw_cust;
            ret = ret.Where(x => x.UnitCode == inUnitCode);

            return ret.OrderBy(x => x.UnitName);
        }




        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }


        public void Insert(fw_cust entity)
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
                    if (db.fw_cust.FirstOrDefault(x => x.UnitCode == entity.UnitCode) == null)
                    {
                        db.fw_cust.InsertOnSubmit(entity);
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

        public void Update(fw_cust entity)
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
                    fw_cust dbdata = db.fw_cust.FirstOrDefault(x => x.UnitCode == entity.UnitCode);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<fw_cust>(entity, dbdata);
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
                    fw_cust dbdata = db.fw_cust.FirstOrDefault(x => x.UnitCode == inCode);
                    if (dbdata != null)
                        db.fw_cust.DeleteOnSubmit(dbdata);
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

        public void Delete(fw_cust entity)
        {
            this.Delete(entity.UnitCode);
        }


        public fw_cust NewEntity()
        {
            return new fw_cust();
        }

        public bool CheckLogin(string inUserID, string inPassword, out fw_cust user)
        {
            if (db != null)
            {

                user = db.fw_cust.FirstOrDefault(u => u.UnitCode== inUserID && u.password == inPassword);

                return user != null;
            }
            else
                throw new ObjectDisposedException("db");
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

                    fw_cust User = db.fw_cust.FirstOrDefault(b => b.UnitCode == inUserbm);
                    User.password = inNewPwd;

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

        public fw_cust Retrieve(string inCode, string inPasswd)
        {
            return db.fw_cust.FirstOrDefault(x => x.UnitCode == inCode && x.password == inPasswd);
        }
    }
}
