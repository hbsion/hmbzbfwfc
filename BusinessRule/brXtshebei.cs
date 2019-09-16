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
    public class brXtshebei : IBrObject<xt_shebei>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string op_no = "op_no";
            public const string op_name = "op_name";
            public const string remark = "remark";
            public const string unitcode = "unitcode";
        }

        [Serializable]
        public class QueryContext
        {
            public string op_no { get; set; }
            public string op_name { get; set; }
            public string remark { get; set; }
            public string unitcode { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brXtshebei()
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

        void IBrObject<xt_shebei>.CheckAction(xt_shebei entity, ActionEnum action) { }

        private IQueryable<xt_shebei> GetByFilter(IQueryable<xt_shebei> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.op_no:
                    return q.Where(x => x.op_no.StartsWith(filter.SearchText));
                case FieldType.op_name:
                    return q.Where(x => x.op_name.Contains(filter.SearchText));
                case FieldType.remark:
                    return q.Where(x => x.remark.Contains(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode==filter.SearchText);

                default:
                    return q;
            }
        }

        private IQueryable<xt_shebei> GetBySorter(IQueryable<xt_shebei> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.op_no:
                    return sorter.ASC ? q.OrderBy(x => x.op_no) : q.OrderByDescending(x => x.op_no);
                case FieldType.op_name:
                    return sorter.ASC ? q.OrderBy(x => x.op_name) : q.OrderByDescending(x => x.op_name);
                case FieldType.remark:
                    return sorter.ASC ? q.OrderBy(x => x.remark) : q.OrderByDescending(x => x.remark);

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<xt_shebei> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.xt_shebei, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.xt_shebei, filter).Count();
        }

        public xt_shebei Retrieve(string inCode)
        {
            return db.xt_shebei.FirstOrDefault(x => x.op_no == inCode);
        }

        public xt_shebei Retrieve(int inCode)
        {
            return db.xt_shebei.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<xt_shebei> GetAll(QueryContext context)
        {
            IQueryable<xt_shebei> ret = db.xt_shebei;

            if (context.op_no != null && context.op_no.Trim().Length > 0)
                ret = ret.Where(x => x.op_no.StartsWith(context.op_no));
            if (context.op_name != null && context.op_name.Trim().Length > 0)
                ret = ret.Where(x => x.op_name.Contains(context.op_name));
            if (context.remark != null && context.remark.Trim().Length > 0)
                ret = ret.Where(x => x.remark.Contains(context.remark));
            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode );

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<xt_shebei> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<xt_shebei> Query()  //返回全部
        {

            IQueryable<xt_shebei> ret = db.xt_shebei;

            return ret.OrderBy(x => x.op_no);
        }

        public IEnumerable<xt_shebei> Query(string inCode,string inunitcode )  //备注
        {

            IQueryable<xt_shebei> ret = db.xt_shebei;
            ret = ret.Where(x => x.remark== inCode && x.unitcode == inunitcode );

            return ret.OrderBy(x => x.op_no);
        }

        public IEnumerable<xt_shebei> Query( string inunitcode)  //备注
        {

            IQueryable<xt_shebei> ret = db.xt_shebei;
            ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderBy(x => x.op_no);
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(xt_shebei entity)
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
                    if (db.xt_shebei.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.xt_shebei.InsertOnSubmit(entity);
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

        public void Update(xt_shebei entity)
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
                    xt_shebei dbdata = db.xt_shebei.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<xt_shebei>(entity, dbdata);
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
                    xt_shebei dbdata = db.xt_shebei.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.xt_shebei.DeleteOnSubmit(dbdata);
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

        public void Delete(xt_shebei entity)
        {
            this.Delete(entity.fid);
        }

        public xt_shebei NewEntity()
        {
            return new xt_shebei();
        }
    }
}
