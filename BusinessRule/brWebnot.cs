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
    public class brWebnote : IBrObject<xt_webnote>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string sys_no = "sys_no";
            public const string sys_note = "sys_note";
            public const string unitcode = "unitcode";
            public const string remark = "remark";

        }

        [Serializable]
        public class QueryContext
        {
            public string sys_no { get; set; }
            public string sys_note { get; set; }
            public string remark { get; set; }
            public string unitcode { get; set; }
            public int tcode { get; set; }



            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brWebnote()
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

        void IBrObject<xt_webnote>.CheckAction(xt_webnote entity, ActionEnum action) { }

        private IQueryable<xt_webnote> GetByFilter(IQueryable<xt_webnote> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.sys_no:
                    return q.Where(x => x.sys_no.StartsWith(filter.SearchText));

                case FieldType.sys_note:
                    return q.Where(x => x.sys_note.StartsWith(filter.SearchText));
                case FieldType.remark:
                    return q.Where(x => x.remark.Contains(filter.SearchText));
                default:
                    return q;
            }
        }

        private IQueryable<xt_webnote> GetBySorter(IQueryable<xt_webnote> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.sys_no:
                    return sorter.ASC ? q.OrderBy(x => x.sys_no) : q.OrderByDescending(x => x.sys_no);

                case FieldType.sys_note:
                    return sorter.ASC ? q.OrderBy(x => x.sys_note) : q.OrderByDescending(x => x.sys_note);
                case FieldType.remark:
                    return sorter.ASC ? q.OrderBy(x => x.remark) : q.OrderByDescending(x => x.remark);

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<xt_webnote> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.xt_webnote, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.xt_webnote, filter).Count();
        }

        public xt_webnote Retrieve(string inCode,string inunitcode)
        {
            return db.xt_webnote.FirstOrDefault(x => x.sys_no == inCode && x.unitcode==inunitcode );
        }

        public xt_webnote Retrieve(string inCode)
        {
            return db.xt_webnote.FirstOrDefault(x => x.sys_no == inCode );
        }

        public xt_webnote Retrieve(int inCode)
        {
            return db.xt_webnote.FirstOrDefault(x => x.fid == inCode);
        }


        private IQueryable<xt_webnote> GetAll(QueryContext context)
        {
            IQueryable<xt_webnote> ret = db.xt_webnote;

            if (context.sys_no != null && context.sys_no.Trim().Length > 0)
                ret = ret.Where(x => x.sys_no.StartsWith(context.sys_no));

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.sys_note);

            if (context.tcode > 0 )
                ret = ret.Where(x => x.tcode == context.tcode);

            if (context.sys_note != null && context.sys_note.Trim().Length > 0)
                ret = ret.Where(x => x.sys_note.StartsWith(context.sys_note));

            if (context.remark != null && context.remark.Trim().Length > 0)
                ret = ret.Where(x => x.remark.Contains(context.remark));

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<xt_webnote> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public IEnumerable<xt_webnote> Query(string inCode, string inunitcode)
        {

            IQueryable<xt_webnote> ret = db.xt_webnote;

            ret = ret.Where(x => x.unitcode == inunitcode);
            ret = ret.Where(x => x.sys_no == inCode);

            return ret.OrderBy(x => x.fid);
        }

        public IEnumerable<xt_webnote> Query()  //返回全部
        {

            IQueryable<xt_webnote> ret = db.xt_webnote;

            return ret.OrderBy(x => x.sys_no);
        }


        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(xt_webnote entity)
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
                    if (db.xt_webnote.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.xt_webnote.InsertOnSubmit(entity);
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

        public void Update(xt_webnote entity)
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
                    xt_webnote dbdata = db.xt_webnote.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<xt_webnote>(entity, dbdata);
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
                    xt_webnote dbdata = db.xt_webnote.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.xt_webnote.DeleteOnSubmit(dbdata);
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

        public void Delete(xt_webnote entity)
        {
            this.Delete(entity.fid);
        }

        public xt_webnote NewEntity()
        {
            return new xt_webnote();
        }
    }
}
