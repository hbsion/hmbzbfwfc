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
    public class brImages : IBrObject<xt_images>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string img_name = "img_name";
            public const string imgtype = "imgtype";
            public const string remark = "remark";

        }

        [Serializable]
        public class QueryContext
        {
            public string img_name { get; set; }
            public string unitcode { get; set; }

            public string imgtype { get; set; }
            public string remark { get; set; }

            public int tcode { get; set; }



            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brImages()
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

        void IBrObject<xt_images>.CheckAction(xt_images entity, ActionEnum action) { }

        private IQueryable<xt_images> GetByFilter(IQueryable<xt_images> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.img_name:
                    return q.Where(x => x.img_name.StartsWith(filter.SearchText));

                case FieldType.imgtype:
                    return q.Where(x => x.imgtype.StartsWith(filter.SearchText));
                case FieldType.remark:
                    return q.Where(x => x.remark.Contains(filter.SearchText));
                default:
                    return q;
            }
        }

        private IQueryable<xt_images> GetBySorter(IQueryable<xt_images> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.img_name:
                    return sorter.ASC ? q.OrderBy(x => x.img_name) : q.OrderByDescending(x => x.img_name);

                case FieldType.imgtype:
                    return sorter.ASC ? q.OrderBy(x => x.imgtype) : q.OrderByDescending(x => x.imgtype);
                case FieldType.remark:
                    return sorter.ASC ? q.OrderBy(x => x.remark) : q.OrderByDescending(x => x.remark);

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<xt_images> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.xt_images, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.xt_images, filter).Count();
        }



        public xt_images Retrieve(string inCode)
        {
            return db.xt_images.FirstOrDefault(x => x.img_name == inCode );
        }

        public xt_images Retrieve(int inCode)
        {
            return db.xt_images.FirstOrDefault(x => x.fid == inCode);
        }


        private IQueryable<xt_images> GetAll(QueryContext context)
        {
            IQueryable<xt_images> ret = db.xt_images;

            if (context.img_name != null && context.img_name.Trim().Length > 0)
                ret = ret.Where(x => x.img_name.StartsWith(context.img_name));


            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode==context.unitcode);
       


            if (context.imgtype != null && context.imgtype.Trim().Length > 0)
                ret = ret.Where(x => x.imgtype.StartsWith(context.imgtype));

            if (context.remark != null && context.remark.Trim().Length > 0)
                ret = ret.Where(x => x.remark.Contains(context.remark));

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<xt_images> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }



        public IEnumerable<xt_images> Query()  //返回全部
        {

            IQueryable<xt_images> ret = db.xt_images;

            return ret.OrderBy(x => x.sindex);
        }

        public IEnumerable<xt_images> Query(string inunitcode)
        {

            IQueryable<xt_images> ret = db.xt_images;


            ret = ret.Where(x => x.unitcode == inunitcode);

     
            return ret.OrderBy(x => x.sindex);
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(xt_images entity)
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
                    if (db.xt_images.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.xt_images.InsertOnSubmit(entity);
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

        public void Update(xt_images entity)
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
                    xt_images dbdata = db.xt_images.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<xt_images>(entity, dbdata);
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
                    xt_images dbdata = db.xt_images.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.xt_images.DeleteOnSubmit(dbdata);
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

        public void Delete(xt_images entity)
        {
            this.Delete(entity.fid);
        }

        public xt_images NewEntity()
        {
            return new xt_images();
        }
    }
}
