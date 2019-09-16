using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using System.Data;

namespace marr.BusinessRule
{
    [Serializable]
    public class brInvLotb : IBrObject<inv_lotb>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string lot_no = "lot_no";


  
        }

        [Serializable]
        public class QueryContext
        {
            public string lot_no { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public string seartext { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brInvLotb()
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

        void IBrObject<inv_lotb>.CheckAction(inv_lotb entity, ActionEnum action) { }

        private IQueryable<inv_lotb> GetByFilter(IQueryable<inv_lotb> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.lot_no:
                    return q.Where(x => x.lot_no.StartsWith(filter.SearchText));
      
                default:
                    return q;
            }
        }

        private IQueryable<inv_lotb> GetBySorter(IQueryable<inv_lotb> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.lot_no:
                    return sorter.ASC ? q.OrderBy(x => x.lot_no) : q.OrderByDescending(x => x.lot_no);

                default:
                    return q.OrderBy(x => x.lot_no);
            }
        }

        public IEnumerable<inv_lotb> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.inv_lotb, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.inv_lotb, filter).Count();
        }

        public inv_lotb Retrieve(string inCode)
        {
            return db.inv_lotb.FirstOrDefault(x => x.lot_no == inCode);
        }


        public inv_lotb Retrieve(string inCode,string inhhlot_no)
        {
            return db.inv_lotb.FirstOrDefault(x => x.lot_no == inCode && x.hhlot_no == inhhlot_no);
        }



        public inv_lotb Retrieve(int inCode)
        {
            return db.inv_lotb.FirstOrDefault(x => x.fid == inCode);
        }



        private IQueryable<inv_lotb> GetAll(QueryContext context)
        {
            IQueryable<inv_lotb> ret = db.inv_lotb;

            if (context.lot_no != null && context.lot_no.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no.StartsWith(context.lot_no));



        
            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no.Contains(context.seartext)  );


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<inv_lotb> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<inv_lotb> Query()  //返回全部产品
        {

            IQueryable<inv_lotb> ret = db.inv_lotb;

            return ret.OrderBy(x => x.lot_no);
        }


        public IEnumerable<inv_lotb> Query(string inunitcode)  //返回xxx产品
        {

            IQueryable<inv_lotb> ret = db.inv_lotb;


            return ret.OrderBy(x => x.lot_no);
        }

        public IEnumerable<inv_lotb> Query(string inlot_no, string inhhlot_no) 
        {

            IQueryable<inv_lotb> ret = db.inv_lotb;

            if (inlot_no != null && inlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no == inlot_no);

            if (inhhlot_no != null && inhhlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.hhlot_no == inhhlot_no);


            return ret.OrderBy(x => x.hhlot_no);

        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(inv_lotb entity)
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
                    if (db.inv_lotb.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.inv_lotb.InsertOnSubmit(entity);
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

        public void Update(inv_lotb entity)
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
                    inv_lotb dbdata = db.inv_lotb.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<inv_lotb>(entity, dbdata);
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
                    inv_lotb dbdata = db.inv_lotb.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.inv_lotb.DeleteOnSubmit(dbdata);
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

        public void Delete(inv_lotb entity)
        {
            this.Delete(entity.fid);
        }

        public inv_lotb NewEntity()
        {
            return new inv_lotb();
        }
    }

}
