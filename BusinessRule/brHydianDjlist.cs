using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;
using marr.BusinessRule.Entity;
using System.Data.SqlClient;

namespace marr.BusinessRule
{
    public class brHydianDjlist : IBrObject<hy_diandjjl>
    {
        [Serializable] 
        public static class FieldType
        {
            public const string None = null;
            public const string hy_no = "hy_no";
            public const string hy_cn = "hy_cn";
            public const string in_date = "in_date";
            public const string Jp_jf = "Jp_jf";
            public const string jp_id = "jp_id";
            public const string jp_name = "jp_name";
            public const string linktell = "linktell";
            public const string linkaddr = "linkaddr";
            public const string status = "status";

        }
        [Serializable] 
        public class QueryContext
        {
            public DateTime? datef { get; set; }
            public DateTime? datet { get; set; }
            public string hy_cn { get; set; }
            public string hy_no { get; set; }

            public int jp_id { get; set; }
            public string jp_name { get; set; }
            public int status { get; set; }
            public string  unitcode { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-1);
                datet = DateTime.Now;
                
            }
        }

        public brHydianDjlist()
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

        void IBrObject<hy_diandjjl>.CheckAction(hy_diandjjl entity, ActionEnum action) { }

        private IQueryable<hy_diandjjl> GetByFilter(IQueryable<hy_diandjjl> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.hy_no:
                    return q.Where(x => x.hy_no == filter.SearchText);

                case FieldType.hy_cn:
                    return q.Where(x => x.hy_cn == filter.SearchText);

                case FieldType.in_date:
                    if (filter.SearchText == null || filter.SearchText.Length == 0)
                    {
                        string[] d = filter.SearchText.Split(';');
                        return q.Where(x => x.in_date >= Convert.ToDateTime(d[0]) && x.in_date <= Convert.ToDateTime(d[1]).AddDays(1).AddSeconds(-1));
                    }
                    else
                        return q;
                case FieldType.jp_id:
                    return q.Where(x => x.jp_id == Convert.ToInt32(filter.SearchText));
                case FieldType.status:
                    if (filter.SearchText == null || filter.SearchText.Length == 0)
                        return q;
                    else
                        return q.Where(x => x.status == Convert.ToInt32(filter.SearchText));


                default:
                    return q;
            }
        }

        private IQueryable<hy_diandjjl> GetBySorter(IQueryable<hy_diandjjl> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.hy_no:
                    return sorter.ASC ? q.OrderBy(x => x.hy_no) : q.OrderByDescending(x => x.hy_no);

                case FieldType.hy_cn:
                    return sorter.ASC ? q.OrderBy(x => x.hy_cn) : q.OrderByDescending(x => x.hy_cn);

                case FieldType.in_date:
                    return sorter.ASC ? q.OrderBy(x => x.in_date) : q.OrderByDescending(x => x.in_date);
                case FieldType.jp_id:
                    return sorter.ASC ? q.OrderBy(x => x.jp_id) : q.OrderByDescending(x => x.jp_id);
                 case FieldType.jp_name:
                   return sorter.ASC ? q.OrderBy(x => x.jp_name) : q.OrderByDescending(x => x.jp_name);
                case FieldType.status:
                    return sorter.ASC ? q.OrderBy(x => x.status) : q.OrderByDescending(x => x.status);
                case FieldType.Jp_jf:
                    return sorter.ASC ? q.OrderBy(x => x.jf) : q.OrderByDescending(x => x.jf);

                default:
                    return q.OrderByDescending(x => x.in_date);
            }
        }



        private IQueryable<hy_diandjjl> GetAll(QueryContext context)
        {
            IQueryable<hy_diandjjl> ret = db.hy_diandjjl;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.in_date >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.in_date <= context.datet.Value.AddDays(1));

            if (context.hy_no != null && context.hy_no.Trim().Length > 0)
                ret = ret.Where(x => x.hy_no == context.hy_no);

            if (context.hy_cn != null && context.hy_cn.Trim().Length > 0)
                ret = ret.Where(x => x.hy_cn == context.hy_cn);

            if (context.jp_id != 0)
                ret = ret.Where(x => x.jp_id == context.jp_id);

            if (context.jp_name != null && context.jp_name.Length > 0)
                ret = ret.Where(x => x.jp_name.Contains(context.jp_name));
            if (context.status != 0)
                ret = ret.Where(x => x.status == context.status);

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<hy_diandjjl> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public IEnumerable<hy_diandjjl> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.hy_diandjjl, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.hy_diandjjl, filter).Count();
        }

        public hy_diandjjl Retrieve(int inID)
        {
            return this.db.hy_diandjjl.FirstOrDefault(x => x.fid == inID);
        }
        public IEnumerable<hy_diandjjl> Retrieve(string inCode)
        {
            IQueryable<hy_diandjjl> ret = db.hy_diandjjl;
            ret = ret.Where(x => x.hy_no == inCode);
            return ret.OrderByDescending(x => x.in_date);
        }
        public hy_diandjjl Retrieve(int inJPID, string inhy_no)
        {
            return this.db.hy_diandjjl.FirstOrDefault(x => x.jp_id == inJPID && x.hy_no == inhy_no);
        }

        public IEnumerable<hy_diandjjl> Retrieve(string inCode,string inunitcode)
        {
            IQueryable<hy_diandjjl> ret = db.hy_diandjjl;
            ret = ret.Where(x => x.hy_no == inCode);
            return ret.OrderByDescending(x => x.in_date);
        }
        public void Insert(hy_diandjjl entity)
        {
            if (db != null)
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


                        db.hy_diandjjl.InsertOnSubmit(entity);
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
            else
                throw new ObjectDisposedException("db");
        }

        public void Update(hy_diandjjl entity)
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
                    hy_diandjjl dbdata = db.hy_diandjjl.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<hy_diandjjl>(entity, dbdata);
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
                    hy_diandjjl dbdata = db.hy_diandjjl.FirstOrDefault(x => x.fid == inID);
                    if (dbdata != null)
                        db.hy_diandjjl.DeleteOnSubmit(dbdata);
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

        public void Delete(hy_diandjjl entity)
        {
            this.Delete(entity.fid);
        }

        public hy_diandjjl NewEntity()
        {
            return new hy_diandjjl();
        }
    }
}
