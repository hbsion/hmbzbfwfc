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
    public class brHbjuan: IBrObject<hob_juan>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string unitcode = "unitcode";

  
        }
        [Serializable]
        public class QueryContext
        {
            public string unitcode { get; set; }
            public string brand { get; set; }
     
 

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brHbjuan()
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

        void IBrObject<hob_juan>.CheckAction(hob_juan entity, ActionEnum action) { }

        private IQueryable<hob_juan> GetByFilter(IQueryable<hob_juan> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode.StartsWith(filter.SearchText));
          

                default:
                    return q;
            }
        }

        private IQueryable<hob_juan> GetBySorter(IQueryable<hob_juan> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.unitcode:
                    return sorter.ASC ? q.OrderBy(x => x.unitcode) : q.OrderByDescending(x => x.unitcode);
 

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<hob_juan> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.hob_juan, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.hob_juan, filter).Count();
        }

        public hob_juan Retrieve(string inCode)
        {
            return db.hob_juan.FirstOrDefault(x => x.dian_no == inCode);
        }

        public hob_juan Retrievename(string inCode,string dian_no)
        {
            return db.hob_juan.FirstOrDefault(x => x.jpname == inCode && x.dian_no==dian_no);
        }

        public hob_juan Retrieve(int inCode)
        {
            return db.hob_juan.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<hob_juan> GetAll(QueryContext context)
        {
            IQueryable<hob_juan> ret = db.hob_juan;

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode.StartsWith(context.unitcode));
  

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<hob_juan> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<hob_juan> Query()  //返回全部
        {

            IQueryable<hob_juan> ret = db.hob_juan;

            return ret.OrderBy(x => x.unitcode);
        }

        //public IEnumerable<hob_juan> Query(string inCode,string inunitcode )  //分类
        //{

        //    IQueryable<hob_juan> ret = db.hob_juan;
        //    ret = ret.Where(x => x.bmtype== inCode && x.unitcode == inunitcode );

        //    return ret.OrderBy(x => x.unitcode);
        //}




        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(hob_juan entity)
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
                    if (db.hob_juan.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.hob_juan.InsertOnSubmit(entity);
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

        public void Update(hob_juan entity)
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
                    hob_juan dbdata = db.hob_juan.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<hob_juan>(entity, dbdata);
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
                    hob_juan dbdata = db.hob_juan.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.hob_juan.DeleteOnSubmit(dbdata);
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

        public void Delete(hob_juan entity)
        {
            this.Delete(entity.fid);
        }

        public hob_juan NewEntity()
        {
            return new hob_juan();
        }
    }
}
