using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using System.Data;

namespace marr.BusinessRule
{
    [Serializable]
    public class brCorder : IBrObject<sal_corder>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string po_no = "po_no";
            public const string p_no = "p_no";
            public const string pname = "pname";

            public const string unitcode = "unitcode";
            public const string cu_no = "pname";
            public const string cu_name = "cu_name";

        }
        [Serializable]
        public class QueryContext
        {
            public DateTime? datef { get; set; }
            public DateTime? datet { get; set; }

            public string po_no { get; set; }
            public string p_no { get; set; }
            public string pname { get; set; }
            public string unitcode { get; set; }
            public string cu_no { get; set; }
            public string cu_name { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-1);
                datet = DateTime.Now;
            }
        }

        public brCorder()
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

        void IBrObject<sal_corder>.CheckAction(sal_corder entity, ActionEnum action) { }

        private IQueryable<sal_corder> GetByFilter(IQueryable<sal_corder> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.po_no:
                    return q.Where(x => x.po_no.StartsWith(filter.SearchText));
                case FieldType.p_no:
                    return q.Where(x => x.p_no.Equals(filter.SearchText));
                case FieldType.pname:
                    return q.Where(x => x.pname.Contains(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode == filter.SearchText);

                default:
                    return q;
            }
        }

        private IQueryable<sal_corder> GetBySorter(IQueryable<sal_corder> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.po_no:
                    return sorter.ASC ? q.OrderBy(x => x.p_no) : q.OrderByDescending(x => x.p_no);
                case FieldType.p_no:
                    return sorter.ASC ? q.OrderBy(x => x.pname) : q.OrderByDescending(x => x.pname);
                case FieldType.pname:
                    return sorter.ASC ? q.OrderBy(x => x.type) : q.OrderByDescending(x => x.type);

                default:
                    return q.OrderBy(x => x.p_no);
            }
        }

        public IEnumerable<sal_corder> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.sal_corder, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.sal_corder, filter).Count();
        }

        public sal_corder Retrieve(string inCode)
        {
            return db.sal_corder.FirstOrDefault(x => x.po_no == inCode);
        }

        public sal_corder Retrieve(string inCode, string inunitcode)
        {
            return db.sal_corder.FirstOrDefault(x => x.po_no == inCode && x.unitcode == inunitcode);
        }


        public sal_corder Retrieve(int inCode)
        {
            return db.sal_corder.FirstOrDefault(x => x.fid == inCode);
        }

        public sal_corder Rallpart(string inunitcode)
        {
            return db.sal_corder.FirstOrDefault(x => x.unitcode == inunitcode);
        }

        private IQueryable<sal_corder> GetAll(QueryContext context)
        {
            IQueryable<sal_corder> ret = db.sal_corder;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.po_date >= context.datef);

            if (context.datet.HasValue)
                ret = ret.Where(x => x.po_date <= context.datet);

            if (context.po_no != null && context.po_no.Trim().Length > 0)
                ret = ret.Where(x => x.po_no.StartsWith(context.po_no));
            if (context.p_no != null && context.p_no.Trim().Length > 0)
                ret = ret.Where(x => x.p_no.Contains(context.p_no));
            if (context.pname != null && context.pname.Trim().Length > 0)
                ret = ret.Where(x => x.pname.Contains(context.pname));

            if (context.cu_no != null && context.cu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.Contains(context.cu_no));

            if (context.cu_name != null && context.cu_name.Trim().Length > 0)
                ret = ret.Where(x => x.cu_name.Contains(context.cu_name));

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<sal_corder> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<sal_corder> Query()  //返回全部产品
        {

            IQueryable<sal_corder> ret = db.sal_corder;

            return ret.OrderBy(x => x.p_no);
        }


        public IEnumerable<sal_corder> Query(string inunitcode)  //返回xxx产品
        {

            IQueryable<sal_corder> ret = db.sal_corder;

            if (inunitcode != null && inunitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderBy(x => x.p_no);
        }


        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(sal_corder entity)
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
                    if (db.sal_corder.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.sal_corder.InsertOnSubmit(entity);
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

        public void Update(sal_corder entity)
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
                    sal_corder dbdata = db.sal_corder.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<sal_corder>(entity, dbdata);
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
                    sal_corder dbdata = db.sal_corder.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.sal_corder.DeleteOnSubmit(dbdata);
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

        public void Delete(sal_corder entity)
        {
            this.Delete(entity.fid);
        }

        public sal_corder NewEntity()
        {
            return new sal_corder();
        }
    }

}
