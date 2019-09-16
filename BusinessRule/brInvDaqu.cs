using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using System.Data;

namespace marr.BusinessRule
{
    [Serializable]
    public class brInvDaqu : IBrObject<inv_daqu>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string dqlot_no = "dqlot_no";
            public const string wdlot_no = "wdlot_no";
            public const string dmlot_no = "dmlot_no";

  
        }

        [Serializable]
        public class QueryContext
        {
            public string dqlot_no { get; set; }
            public string wdlot_no { get; set; }
            public string dmlot_no { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public string seartext { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brInvDaqu()
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

        void IBrObject<inv_daqu>.CheckAction(inv_daqu entity, ActionEnum action) { }

        private IQueryable<inv_daqu> GetByFilter(IQueryable<inv_daqu> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.dqlot_no:
                    return q.Where(x => x.dqlot_no.StartsWith(filter.SearchText));
                case FieldType.dmlot_no:
                    return q.Where(x => x.dmlot_no.Equals(filter.SearchText));
               
                case FieldType.wdlot_no:
                    return q.Where(x => x.wdlot_no.Contains(filter.SearchText));

                default:
                    return q;
            }
        }

        private IQueryable<inv_daqu> GetBySorter(IQueryable<inv_daqu> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.dqlot_no:
                    return sorter.ASC ? q.OrderBy(x => x.dqlot_no) : q.OrderByDescending(x => x.dqlot_no);
                case FieldType.dmlot_no:
                    return sorter.ASC ? q.OrderBy(x => x.dmlot_no) : q.OrderByDescending(x => x.dmlot_no);
                case FieldType.wdlot_no:
                    return sorter.ASC ? q.OrderBy(x => x.wdlot_no) : q.OrderByDescending(x => x.wdlot_no);

                default:
                    return q.OrderBy(x => x.dqlot_no);
            }
        }

        public IEnumerable<inv_daqu> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.inv_daqu, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.inv_daqu, filter).Count();
        }

        public inv_daqu Retrieve(string inCode)
        {
            return db.inv_daqu.FirstOrDefault(x => x.dqlot_no == inCode);
        }



        public inv_daqu Retrieve(int inCode)
        {
            return db.inv_daqu.FirstOrDefault(x => x.fid == inCode);
        }



        private IQueryable<inv_daqu> GetAll(QueryContext context)
        {
            IQueryable<inv_daqu> ret = db.inv_daqu;

            if (context.dqlot_no != null && context.dqlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.dqlot_no.StartsWith(context.dqlot_no));

            if (context.dmlot_no != null && context.dmlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.dmlot_no.Contains(context.dmlot_no));

            if (context.wdlot_no != null && context.wdlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.wdlot_no.Contains(context.wdlot_no));


        
            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.dqlot_no.Contains(context.seartext) || x.wdlot_no.Contains(context.seartext) || x.dmlot_no.Contains(context.seartext) );


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<inv_daqu> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<inv_daqu> Query()  //返回全部产品
        {

            IQueryable<inv_daqu> ret = db.inv_daqu;

            return ret.OrderBy(x => x.dmlot_no);
        }


        public IEnumerable<inv_daqu> Query(string inunitcode)  //返回xxx产品
        {

            IQueryable<inv_daqu> ret = db.inv_daqu;


            return ret.OrderBy(x => x.dmlot_no);
        }

        public IEnumerable<inv_daqu> Query(string indqlot_no, string inunitcode)  //返回xxx产品
        {

            IQueryable<inv_daqu> ret = db.inv_daqu;

            if (indqlot_no != null && indqlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.dqlot_no == indqlot_no);



            return ret.OrderBy(x => x.ship_date);
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(inv_daqu entity)
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
                    if (db.inv_daqu.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.inv_daqu.InsertOnSubmit(entity);
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

        public void Update(inv_daqu entity)
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
                    inv_daqu dbdata = db.inv_daqu.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<inv_daqu>(entity, dbdata);
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
                    inv_daqu dbdata = db.inv_daqu.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.inv_daqu.DeleteOnSubmit(dbdata);
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

        public void Delete(inv_daqu entity)
        {
            this.Delete(entity.fid);
        }

        public inv_daqu NewEntity()
        {
            return new inv_daqu();
        }
    }

}
