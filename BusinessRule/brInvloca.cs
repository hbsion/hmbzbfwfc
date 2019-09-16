using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using System.Data;

namespace marr.BusinessRule
{
    [Serializable]
    public class brInvloca : IBrObject<inv_loca>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string lc_no = "lc_no";
            public const string locaname = "locaname";
            public const string in_date = "in_date";
            public const string linkman = "linkman";
            public const string unitcode = "unitcode";

        }
        [Serializable]
        public class QueryContext
        {
            public string lc_no { get; set; }
            public string locaname { get; set; }
            public string in_date { get; set; }
            public string unitcode { get; set; }
            public string linkman { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brInvloca()
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

        void IBrObject<inv_loca>.CheckAction(inv_loca entity, ActionEnum action) { }

        private IQueryable<inv_loca> GetByFilter(IQueryable<inv_loca> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.lc_no:
                    return q.Where(x => x.lc_no.StartsWith(filter.SearchText));
                case FieldType.locaname:
                    return q.Where(x => x.locaname.Contains(filter.SearchText));
                case FieldType.in_date:
                    return q.Where(x => x.in_date.Equals(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode ==filter.SearchText);
                default:
                    return q;
            }
        }

        private IQueryable<inv_loca> GetBySorter(IQueryable<inv_loca> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.lc_no:
                    return sorter.ASC ? q.OrderBy(x => x.lc_no) : q.OrderByDescending(x => x.lc_no);
                case FieldType.locaname:
                    return sorter.ASC ? q.OrderBy(x => x.locaname) : q.OrderByDescending(x => x.locaname);
                case FieldType.in_date:
                    return sorter.ASC ? q.OrderBy(x => x.in_date) : q.OrderByDescending(x => x.in_date);
            
                default:
                    return q.OrderBy(x => x.lc_no);
            }
        }

        public IEnumerable<inv_loca> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.inv_loca, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.inv_loca, filter).Count();
        }

        public inv_loca Retrieve(string inCode)
        {
            return db.inv_loca.FirstOrDefault(x => x.lc_no == inCode);
        }

        public inv_loca Retrieve(string inCode, string inunitcode)
        {
            return db.inv_loca.FirstOrDefault(x => x.lc_no == inCode && x.unitcode==inunitcode);


        }
        public inv_loca Retrieve(int inCode)
        {
            return db.inv_loca.FirstOrDefault(x => x.fid == inCode);
        }

        public inv_loca Rallpart(string inunitcode)
        {
            return db.inv_loca.FirstOrDefault(x =>  x.unitcode==inunitcode);
        }

        private IQueryable<inv_loca> GetAll(QueryContext context)
        {
            IQueryable<inv_loca> ret = db.inv_loca;

            if (context.lc_no != null && context.lc_no.Trim().Length > 0)
                ret = ret.Where(x => x.lc_no.StartsWith(context.lc_no));
            if (context.locaname != null && context.locaname.Trim().Length > 0)
                ret = ret.Where(x => x.locaname.Contains(context.locaname));
            if (context.in_date != null && context.in_date.Trim().Length > 0)
                ret = ret.Where(x => x.in_date.Equals(context.in_date));

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);           
            
             ret = GetBySorter(ret, context.sorter);

              return ret;
        }

        public IEnumerable<inv_loca> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<inv_loca> Query()  //返回全部产品
        {

            IQueryable<inv_loca> ret = db.inv_loca;

            return ret.OrderBy(x => x.lc_no);
        }


        public IEnumerable<inv_loca> Query(string inunitcode)  //返回xxx库别
        {

            IQueryable<inv_loca> ret = db.inv_loca;

            if (inunitcode != null && inunitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

             return ret.OrderBy(x => x.lc_no);
        }


        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(inv_loca entity)
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
                    if (db.inv_loca.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.inv_loca.InsertOnSubmit(entity);
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

        public void Update(inv_loca entity)
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
                    inv_loca dbdata = db.inv_loca.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<inv_loca>(entity, dbdata);
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

        public void Delete(int inCode)
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
                    inv_loca dbdata = db.inv_loca.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.inv_loca.DeleteOnSubmit(dbdata);
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

        public void Delete(inv_loca entity)
        {
            this.Delete(entity.fid);
        }

        public inv_loca NewEntity()
        {
            return new inv_loca();
        }
    }
}
