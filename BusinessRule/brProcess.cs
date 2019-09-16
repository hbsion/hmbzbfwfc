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
    public class brProcess : IBrObject<xt_process>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string pc_no = "pc_no";
            public const string pc_name = "pc_name";
            public const string remark = "remark";
            public const string unitcode = "unitcode";
        }

        [Serializable]
        public class QueryContext
        {
            public string pc_no { get; set; }
            public string pc_name { get; set; }
            public string remark { get; set; }
            public string unitcode { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brProcess()
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

        void IBrObject<xt_process>.CheckAction(xt_process entity, ActionEnum action) { }

        private IQueryable<xt_process> GetByFilter(IQueryable<xt_process> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.pc_no:
                    return q.Where(x => x.pc_no.StartsWith(filter.SearchText));
                case FieldType.pc_name:
                    return q.Where(x => x.pc_name.Contains(filter.SearchText));
                case FieldType.remark:
                    return q.Where(x => x.remark.Contains(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode==filter.SearchText);

                default:
                    return q;
            }
        }

        private IQueryable<xt_process> GetBySorter(IQueryable<xt_process> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.pc_no:
                    return sorter.ASC ? q.OrderBy(x => x.pc_no) : q.OrderByDescending(x => x.pc_no);
                case FieldType.pc_name:
                    return sorter.ASC ? q.OrderBy(x => x.pc_name) : q.OrderByDescending(x => x.pc_name);
                case FieldType.remark:
                    return sorter.ASC ? q.OrderBy(x => x.remark) : q.OrderByDescending(x => x.remark);

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<xt_process> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.xt_process, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.xt_process, filter).Count();
        }

        public xt_process Retrieve(string inCode)
        {
            return db.xt_process.FirstOrDefault(x => x.pc_no == inCode);
        }

        public xt_process Retrieve(int inCode)
        {
            return db.xt_process.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<xt_process> GetAll(QueryContext context)
        {
            IQueryable<xt_process> ret = db.xt_process;

            if (context.pc_no != null && context.pc_no.Trim().Length > 0)
                ret = ret.Where(x => x.pc_no.StartsWith(context.pc_no));
            if (context.pc_name != null && context.pc_name.Trim().Length > 0)
                ret = ret.Where(x => x.pc_name.Contains(context.pc_name));
            if (context.remark != null && context.remark.Trim().Length > 0)
                ret = ret.Where(x => x.remark.Contains(context.remark));
            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode );

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<xt_process> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<xt_process> Query()  //返回全部
        {

            IQueryable<xt_process> ret = db.xt_process;

            return ret.OrderBy(x => x.pc_no);
        }

        public IEnumerable<xt_process> Query(string inCode,string inunitcode )  //备注
        {

            IQueryable<xt_process> ret = db.xt_process;
            ret = ret.Where(x => x.remark== inCode && x.unitcode == inunitcode );

            return ret.OrderBy(x => x.pc_no);
        }

        public IEnumerable<xt_process> Query( string inunitcode)  //备注
        {

            IQueryable<xt_process> ret = db.xt_process;
            ret = ret.Where( x => x.unitcode == inunitcode);

            return ret.OrderBy(x => x.pc_no);
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(xt_process entity)
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
                    if (db.xt_process.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.xt_process.InsertOnSubmit(entity);
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

        public void Update(xt_process entity)
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
                    xt_process dbdata = db.xt_process.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<xt_process>(entity, dbdata);
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
                    xt_process dbdata = db.xt_process.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.xt_process.DeleteOnSubmit(dbdata);
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

        public void Delete(xt_process entity)
        {
            this.Delete(entity.fid);
        }

        public xt_process NewEntity()
        {
            return new xt_process();
        }
    }
}
