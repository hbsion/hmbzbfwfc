using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;


namespace marr.BusinessRule
{
    [Serializable]
    public class brPackb : IBrObject<sal_packb>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string snno = "snno";
            public const string usnno = "usnno";

        }

        [Serializable]
        public class QueryContext
        {
            public string snno { get; set; }
            public string usnno { get; set; }
            public string bmtype { get; set; }
            public string unitcode { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brPackb()
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

        void IBrObject<sal_packb>.CheckAction(sal_packb entity, ActionEnum action) { }

        private IQueryable<sal_packb> GetByFilter(IQueryable<sal_packb> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.snno:
                    return q.Where(x => x.snno.StartsWith(filter.SearchText));
                case FieldType.usnno:
                    return q.Where(x => x.usnno.Contains(filter.SearchText));
 

                default:
                    return q;
            }
        }

        private IQueryable<sal_packb> GetBySorter(IQueryable<sal_packb> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.snno:
                    return sorter.ASC ? q.OrderBy(x => x.snno) : q.OrderByDescending(x => x.snno);
                case FieldType.usnno:
                    return sorter.ASC ? q.OrderBy(x => x.usnno) : q.OrderByDescending(x => x.usnno);


                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<sal_packb> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.sal_packb, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.sal_packb, filter).Count();
        }

        public sal_packb Retrieve(string inCode)
        {
            return db.sal_packb.FirstOrDefault(x => x.snno == inCode);
        }

        public sal_packb Retrieves(string inCode)
        {
            return db.sal_packb.FirstOrDefault(x => x.snno == inCode || x.usnno==inCode);
        }

        public sal_packb Retucode(string inCode)
        {
            return db.sal_packb.FirstOrDefault(x => x.usnno == inCode);
        }
        public IQueryable<sal_packb> Retucodes(string inCode)
        {
            return db.sal_packb.Where(x => x.usnno == inCode);
        }
        public sal_packb Retrieve(int inCode)
        {
            return db.sal_packb.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<sal_packb> GetAll(QueryContext context)
        {
            IQueryable<sal_packb> ret = db.sal_packb;

            if (context.snno != null && context.snno.Trim().Length > 0)
                ret = ret.Where(x => x.snno.StartsWith(context.snno));
            if (context.usnno != null && context.usnno.Trim().Length > 0)
                ret = ret.Where(x => x.usnno.Contains(context.usnno));


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<sal_packb> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<sal_packb> Query()  //返回全部
        {

            IQueryable<sal_packb> ret = db.sal_packb;

            return ret.OrderBy(x => x.snno);
        }



        public int ReInt(string inCode)  //返回箱数
        {
            return db.sal_packb.Count(x => x.usnno == inCode);
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(sal_packb entity)
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
                    if (db.sal_packb.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.sal_packb.InsertOnSubmit(entity);
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

        public void Update(sal_packb entity)
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
                    sal_packb dbdata = db.sal_packb.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<sal_packb>(entity, dbdata);
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
                    sal_packb dbdata = db.sal_packb.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.sal_packb.DeleteOnSubmit(dbdata);
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

        public void Delete(sal_packb entity)
        {
            this.Delete(entity.fid);
        }

        public sal_packb NewEntity()
        {
            return new sal_packb();
        }
    }
}
