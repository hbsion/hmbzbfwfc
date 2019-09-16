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
    public class brwxaccess : IBrObject<wx_accessToken>
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

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brwxaccess()
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

        void IBrObject<wx_accessToken>.CheckAction(wx_accessToken entity, ActionEnum action) { }

        private IQueryable<wx_accessToken> GetByFilter(IQueryable<wx_accessToken> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.WxId:
                    return q.Where(x => x.WxId.StartsWith(filter.SearchText));

                //case FieldType.WxNo:
                //    return q.Where(x => x.WxNo.StartsWith(filter.SearchText));
                //case FieldType.WxName:
                //    return q.Where(x => x.WxName.Contains(filter.SearchText));
                default:
                    return q;
            }
        }

        private IQueryable<wx_accessToken> GetBySorter(IQueryable<wx_accessToken> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.WxId:
                    return sorter.ASC ? q.OrderBy(x => x.WxId) : q.OrderByDescending(x => x.WxId);

                //case FieldType.WxNo:
                //    return sorter.ASC ? q.OrderBy(x => x.WxNo) : q.OrderByDescending(x => x.WxNo);
                //case FieldType.WxName:
                //    return sorter.ASC ? q.OrderBy(x => x.WxName) : q.OrderByDescending(x => x.WxName);

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<wx_accessToken> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.wx_accessToken, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.wx_accessToken, filter).Count();
        }

        public wx_accessToken Retrieve(string inCode)
        {
            return db.wx_accessToken.FirstOrDefault(x => x.unitcode == inCode);
        }


        public wx_accessToken Retrtktype(string inwxid, string inCode)
        {
            return db.wx_accessToken.FirstOrDefault(x => x.tokentype == inCode && x.WxId == inwxid);
        }

        //public wx_accessToken Retunit(string inCode)
        //{
        //    return db.wx_accessToken.FirstOrDefault(x => x.unitcode == inCode);
        //}

        public wx_accessToken Retrieve(int inCode)
        {
            return db.wx_accessToken.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<wx_accessToken> GetAll(QueryContext context)
        {
            IQueryable<wx_accessToken> ret = db.wx_accessToken;

            if (context.WxId != null && context.WxId.Trim().Length > 0)
                ret = ret.Where(x => x.WxId.StartsWith(context.WxId));
            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode.StartsWith(context.unitcode));

            //if (context.WxNo != null && context.WxNo.Trim().Length > 0)
            //    ret = ret.Where(x => x.WxNo.StartsWith(context.WxNo));

            //if (context.WxName != null && context.WxName.Trim().Length > 0)
            //    ret = ret.Where(x => x.WxName.Contains(context.WxName));

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<wx_accessToken> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }




        public IEnumerable<wx_accessToken> Query( string inWxId)
        {

            IQueryable<wx_accessToken> ret = db.wx_accessToken;


            ret = ret.Where(x =>  x.WxId == inWxId);

            return ret.OrderBy(x => x.fid);
        }

        public IEnumerable<wx_accessToken> Query()  //返回全部
        {

            IQueryable<wx_accessToken> ret = db.wx_accessToken;

            return ret.OrderBy(x => x.WxId);
        }


        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(wx_accessToken entity)
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
                    if (db.wx_accessToken.FirstOrDefault(x => x.fid == entity.fid ) == null)
                    {
                        db.wx_accessToken.InsertOnSubmit(entity);
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

        public void Update(wx_accessToken entity)
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
                    wx_accessToken dbdata = db.wx_accessToken.FirstOrDefault(x => x.unitcode == entity.unitcode);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<wx_accessToken>(entity, dbdata);
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
                    wx_accessToken dbdata = db.wx_accessToken.FirstOrDefault(x => x.fid== inCode);
                    if (dbdata != null)
                        db.wx_accessToken.DeleteOnSubmit(dbdata);
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

        public void Delete(wx_accessToken entity)
        {
            this.Delete(entity.fid);
        }

        public wx_accessToken NewEntity()
        {
            return new wx_accessToken();
        }
    }
}
