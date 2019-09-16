using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using System.Data;

namespace marr.BusinessRule
{
    [Serializable]
    public class brInvHunheb : IBrObject<inv_hunheb>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string hhlot_no = "hhlot_no";


  
        }

        [Serializable]
        public class QueryContext
        {
            public string hhlot_no { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public string seartext { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brInvHunheb()
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

        void IBrObject<inv_hunheb>.CheckAction(inv_hunheb entity, ActionEnum action) { }

        private IQueryable<inv_hunheb> GetByFilter(IQueryable<inv_hunheb> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.hhlot_no:
                    return q.Where(x => x.hhlot_no.StartsWith(filter.SearchText));
      
                default:
                    return q;
            }
        }

        private IQueryable<inv_hunheb> GetBySorter(IQueryable<inv_hunheb> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.hhlot_no:
                    return sorter.ASC ? q.OrderBy(x => x.hhlot_no) : q.OrderByDescending(x => x.hhlot_no);

                default:
                    return q.OrderBy(x => x.hhlot_no);
            }
        }

        public IEnumerable<inv_hunheb> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.inv_hunheb, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.inv_hunheb, filter).Count();
        }

        public inv_hunheb Retrieve(string inCode)
        {
            return db.inv_hunheb.FirstOrDefault(x => x.hhlot_no == inCode);
        }


        public inv_hunheb Retrieve(string inCode,string inyjlot_no)
        {
            return db.inv_hunheb.FirstOrDefault(x => x.hhlot_no == inCode && x.yjlot_no == inyjlot_no);
        }



        public inv_hunheb Retrieve(int inCode)
        {
            return db.inv_hunheb.FirstOrDefault(x => x.fid == inCode);
        }



        private IQueryable<inv_hunheb> GetAll(QueryContext context)
        {
            IQueryable<inv_hunheb> ret = db.inv_hunheb;

            if (context.hhlot_no != null && context.hhlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.hhlot_no==context.hhlot_no);

        
            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.hhlot_no.Contains(context.seartext)  );


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<inv_hunheb> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<inv_hunheb> Query()  //返回全部产品
        {

            IQueryable<inv_hunheb> ret = db.inv_hunheb;

            return ret.OrderBy(x => x.hhlot_no);
        }


        public IEnumerable<inv_hunheb> Query(string inunitcode)  //返回xxx产品
        {

            IQueryable<inv_hunheb> ret = db.inv_hunheb;


            return ret.OrderBy(x => x.hhlot_no);
        }

        public IEnumerable<inv_hunheb> Query(string inhhlot_no, string inyjlot_no) 
        {

            IQueryable<inv_hunheb> ret = db.inv_hunheb;

            if (inhhlot_no != null && inhhlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.hhlot_no == inhhlot_no);

            if (inyjlot_no != null && inyjlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.yjlot_no == inyjlot_no);


            return ret.OrderBy(x => x.yjlot_no);

        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(inv_hunheb entity)
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
                    if (db.inv_hunheb.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.inv_hunheb.InsertOnSubmit(entity);
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

        public void Update(inv_hunheb entity)
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
                    inv_hunheb dbdata = db.inv_hunheb.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<inv_hunheb>(entity, dbdata);
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
                    inv_hunheb dbdata = db.inv_hunheb.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.inv_hunheb.DeleteOnSubmit(dbdata);
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

        public void Delete(inv_hunheb entity)
        {
            this.Delete(entity.fid);
        }

        public inv_hunheb NewEntity()
        {
            return new inv_hunheb();
        }
    }

}
