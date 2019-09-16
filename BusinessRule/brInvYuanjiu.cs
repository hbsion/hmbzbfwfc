using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using System.Data;

namespace marr.BusinessRule
{
    [Serializable]
    public class brInvYuanjiu : IBrObject<inv_yuanjiu>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string yjlot_no = "yjlot_no";
            public const string gllot_no = "gllot_no";
            public const string gklot_no = "gklot_no";

  
        }

        [Serializable]
        public class QueryContext
        {
            public string yjlot_no { get; set; }
            public string gllot_no { get; set; }
            public string gklot_no { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public string seartext { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brInvYuanjiu()
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

        void IBrObject<inv_yuanjiu>.CheckAction(inv_yuanjiu entity, ActionEnum action) { }

        private IQueryable<inv_yuanjiu> GetByFilter(IQueryable<inv_yuanjiu> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.yjlot_no:
                    return q.Where(x => x.yjlot_no.StartsWith(filter.SearchText));
                case FieldType.gklot_no:
                    return q.Where(x => x.gklot_no.Equals(filter.SearchText));
               
                case FieldType.gllot_no:
                    return q.Where(x => x.gllot_no.Contains(filter.SearchText));

                default:
                    return q;
            }
        }

        private IQueryable<inv_yuanjiu> GetBySorter(IQueryable<inv_yuanjiu> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.yjlot_no:
                    return sorter.ASC ? q.OrderBy(x => x.yjlot_no) : q.OrderByDescending(x => x.yjlot_no);
                case FieldType.gklot_no:
                    return sorter.ASC ? q.OrderBy(x => x.gklot_no) : q.OrderByDescending(x => x.gklot_no);
                case FieldType.gllot_no:
                    return sorter.ASC ? q.OrderBy(x => x.gllot_no) : q.OrderByDescending(x => x.gllot_no);

                default:
                    return q.OrderBy(x => x.yjlot_no);
            }
        }

        public IEnumerable<inv_yuanjiu> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.inv_yuanjiu, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.inv_yuanjiu, filter).Count();
        }

        public inv_yuanjiu Retrieve(string inCode)
        {
            return db.inv_yuanjiu.FirstOrDefault(x => x.yjlot_no == inCode);
        }



        public inv_yuanjiu Retrieve(int inCode)
        {
            return db.inv_yuanjiu.FirstOrDefault(x => x.fid == inCode);
        }



        private IQueryable<inv_yuanjiu> GetAll(QueryContext context)
        {
            IQueryable<inv_yuanjiu> ret = db.inv_yuanjiu;

            if (context.yjlot_no != null && context.yjlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.yjlot_no.StartsWith(context.yjlot_no));

            if (context.gklot_no != null && context.gklot_no.Trim().Length > 0)
                ret = ret.Where(x => x.gklot_no.Contains(context.gklot_no));

            if (context.gllot_no != null && context.gllot_no.Trim().Length > 0)
                ret = ret.Where(x => x.gllot_no.Contains(context.gllot_no));


        
            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.yjlot_no.Contains(context.seartext) || x.gllot_no.Contains(context.seartext) || x.gklot_no.Contains(context.seartext) );


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<inv_yuanjiu> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<inv_yuanjiu> Query()  //返回全部产品
        {

            IQueryable<inv_yuanjiu> ret = db.inv_yuanjiu;

            return ret.OrderBy(x => x.gklot_no);
        }


        public IEnumerable<inv_yuanjiu> Query(string inunitcode)  //返回xxx产品
        {

            IQueryable<inv_yuanjiu> ret = db.inv_yuanjiu;


            return ret.OrderBy(x => x.gklot_no);
        }

        public IEnumerable<inv_yuanjiu> Query(string inyjlot_no, string inunitcode)  //返回xxx产品
        {

            IQueryable<inv_yuanjiu> ret = db.inv_yuanjiu;

            if (inyjlot_no != null && inyjlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.yjlot_no == inyjlot_no);



            return ret.OrderBy(x => x.ship_date);
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(inv_yuanjiu entity)
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
                    if (db.inv_yuanjiu.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.inv_yuanjiu.InsertOnSubmit(entity);
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

        public void Update(inv_yuanjiu entity)
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
                    inv_yuanjiu dbdata = db.inv_yuanjiu.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<inv_yuanjiu>(entity, dbdata);
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
                    inv_yuanjiu dbdata = db.inv_yuanjiu.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.inv_yuanjiu.DeleteOnSubmit(dbdata);
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

        public void Delete(inv_yuanjiu entity)
        {
            this.Delete(entity.fid);
        }

        public inv_yuanjiu NewEntity()
        {
            return new inv_yuanjiu();
        }
    }

}
