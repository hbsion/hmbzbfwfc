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
    public class brWxgid : IBrObject<wx_gid>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string WxId = "WxId";
            public const string WxNo = "WxNo";
            public const string WxName = "WxName";
            public const string unitcode = "unitcode";
        }

        [Serializable]
        public class QueryContext
        {
            public string WxId { get; set; }
            public string WxNo { get; set; }
            public string WxName { get; set; }
            public string unitcode { get; set; }
            public string sunitcode { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brWxgid()
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

        void IBrObject<wx_gid>.CheckAction(wx_gid entity, ActionEnum action) { }

        private IQueryable<wx_gid> GetByFilter(IQueryable<wx_gid> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.WxId:
                    return q.Where(x => x.WxId.StartsWith(filter.SearchText));

                case FieldType.WxNo:
                    return q.Where(x => x.WxNo.StartsWith(filter.SearchText));
                case FieldType.WxName:
                    return q.Where(x => x.WxName.Contains(filter.SearchText));
                default:
                    return q;
            }
        }

        private IQueryable<wx_gid> GetBySorter(IQueryable<wx_gid> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.WxId:
                    return sorter.ASC ? q.OrderBy(x => x.WxId) : q.OrderByDescending(x => x.WxId);

                case FieldType.WxNo:
                    return sorter.ASC ? q.OrderBy(x => x.WxNo) : q.OrderByDescending(x => x.WxNo);
                case FieldType.WxName:
                    return sorter.ASC ? q.OrderBy(x => x.WxName) : q.OrderByDescending(x => x.WxName);

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<wx_gid> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.wx_gid, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.wx_gid, filter).Count();
        }

        public wx_gid Retrieve(string inCode)
        {
            return db.wx_gid.FirstOrDefault(x => x.WxId == inCode);
        }

        public wx_gid Retunit(string inCode)
        {
            return db.wx_gid.FirstOrDefault(x => x.unitcode == inCode);
        }

        public wx_gid Retgid(string inCode)
        {
            return db.wx_gid.FirstOrDefault(x => x.WxId == inCode);
        }

        public wx_gid Retrieve(int inCode)
        {
            return db.wx_gid.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<wx_gid> GetAll(QueryContext context)
        {
            IQueryable<wx_gid> ret = db.wx_gid;

            if (context.WxId != null && context.WxId.Trim().Length > 0)
                ret = ret.Where(x => x.WxId.StartsWith(context.WxId));
            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode.StartsWith(context.unitcode));

            if (context.WxNo != null && context.WxNo.Trim().Length > 0)
                ret = ret.Where(x => x.WxNo.StartsWith(context.WxNo));

            if (context.WxName != null && context.WxName.Trim().Length > 0)
                ret = ret.Where(x => x.WxName.Contains(context.WxName));

            if (context.sunitcode != null && context.sunitcode.Trim().Length > 0)
                ret = ret.Where(x => context.sunitcode.IndexOf(x.unitcode) >= 0 && x.unitcode.Trim().Length == 4);

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<wx_gid> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }




        public IEnumerable<wx_gid> Query( string inWxId)
        {

            IQueryable<wx_gid> ret = db.wx_gid;


            ret = ret.Where(x =>  x.WxId == inWxId);

            return ret.OrderBy(x => x.fid);
        }

        public IEnumerable<wx_gid> Query()  //返回全部
        {

            IQueryable<wx_gid> ret = db.wx_gid;

            return ret.OrderBy(x => x.WxId);
        }


        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(wx_gid entity)
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
                    if (db.wx_gid.FirstOrDefault(x => x.fid == entity.fid ) == null)
                    {
                        db.wx_gid.InsertOnSubmit(entity);
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

        public void Update(wx_gid entity)
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
                    wx_gid dbdata = db.wx_gid.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<wx_gid>(entity, dbdata);
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
                    wx_gid dbdata = db.wx_gid.FirstOrDefault(x => x.fid== inCode);
                    if (dbdata != null)
                        db.wx_gid.DeleteOnSubmit(dbdata);
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

        public void Delete(wx_gid entity)
        {
            this.Delete(entity.fid);
        }

        public wx_gid NewEntity()
        {
            return new wx_gid();
        }
    }
}
