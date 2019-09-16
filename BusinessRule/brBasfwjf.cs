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
    public class brBasfwjf : IBrObject<BasFwjf>
    {
        [Serializable]
        public static class FieldType
        {
            public const string None = null;
            public const string ProdNO = "p_no";
            public const string ProdName = "pname";
            public const string unitcode = "unitcode";

            public const string snnof = "snnof";
            public const string snnot = "snnot";
            public const string jf = "jf";
            public const string remark = "remark";

        }

        [Serializable] 
        public class QueryContext
        {
            public string ProdNO { get; set; }
            public string ProdName { get; set; }
            public string unitcode { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brBasfwjf()
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

        void IBrObject<BasFwjf>.CheckAction(BasFwjf entity, ActionEnum action) { }

        private IQueryable<BasFwjf> GetByFilter(IQueryable<BasFwjf> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.ProdNO:
                    return q.Where(x => x.p_no.StartsWith(filter.SearchText));
                case FieldType.ProdName:
                    return q.Where(x => x.pname.Contains(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode.Contains(filter.SearchText));

                default:
                    return q;
            }
        }

        private IQueryable<BasFwjf> GetBySorter(IQueryable<BasFwjf> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.ProdNO:
                    return sorter.ASC ? q.OrderBy(x => x.p_no) : q.OrderByDescending(x => x.p_no);
                case FieldType.ProdName:
                    return sorter.ASC ? q.OrderBy(x => x.pname) : q.OrderByDescending(x => x.pname);
                case FieldType.snnof:
                    return sorter.ASC ? q.OrderBy(x => x.snnof) : q.OrderByDescending(x => x.snnof);
                case FieldType.snnot:
                    return sorter.ASC ? q.OrderBy(x => x.snnot) : q.OrderByDescending(x => x.snnot);
                case FieldType.remark:
                    return sorter.ASC ? q.OrderBy(x => x.remark) : q.OrderByDescending(x => x.remark);
                case FieldType.jf:
                    return sorter.ASC ? q.OrderBy(x => x.jf) : q.OrderByDescending(x => x.jf);
                default:
                    return q.OrderBy(x => x.unitcode);
            }
        }

        public IEnumerable<BasFwjf> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.BasFwjf, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.BasFwjf, filter).Count();
        }

        public BasFwjf Retrieve(int inID)
        {
            return db.BasFwjf.FirstOrDefault(x => x.fid == inID);
        }

        public BasFwjf Retrieve(string incode,string inunitcode)
        {
            return db.BasFwjf.FirstOrDefault(x => x.snnof.CompareTo(incode) <= 0 && x.snnot.CompareTo(incode) >= 0 && x.unitcode == inunitcode);
        }

        public BasFwjf RetrieveP(long incode, string inunitcode)
        {
            return db.BasFwjf.FirstOrDefault(x => Convert.ToInt64(x.snnof) <= incode && Convert.ToInt64(x.snnot) >= incode && x.unitcode == inunitcode );
        }

        public BasFwjf RetrieveP(string incode, string inunitcode)
        {
            return db.BasFwjf.FirstOrDefault(x => x.snnof.CompareTo(incode) <= 0 && x.snnot.CompareTo(incode) >= 0 && x.unitcode == inunitcode);
        }


        private IQueryable<BasFwjf> GetAll(QueryContext context)
        {
            IQueryable<BasFwjf> ret = db.BasFwjf;

            if (context.ProdNO != null && context.ProdNO.Trim().Length > 0)
                ret = ret.Where(x => x.p_no.StartsWith(context.ProdNO));
            if (context.ProdName != null && context.ProdName.Trim().Length > 0)
                ret = ret.Where(x => x.pname.Contains(context.ProdName));
            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode==context.unitcode);



            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<BasFwjf> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<BasFwjf> Query()
        {
            IQueryable<BasFwjf> ret = db.BasFwjf;

            return ret ;
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(BasFwjf entity)
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
                    if (db.BasFwjf.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.BasFwjf.InsertOnSubmit(entity);
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

        public void Update(BasFwjf entity)
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
                    BasFwjf dbdata = db.BasFwjf.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<BasFwjf>(entity, dbdata);
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

        public void Delete(int inID)
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
                    BasFwjf dbdata = db.BasFwjf.FirstOrDefault(x => x.fid == inID);
                    if (dbdata != null)
                        db.BasFwjf.DeleteOnSubmit(dbdata);
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

        public void Delete(BasFwjf entity)
        {
            this.Delete(entity.fid);
        }

        public BasFwjf NewEntity()
        {
            return new BasFwjf();
        }
    }
}
