using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;
using marr.BusinessRule.Entity;

namespace marr.BusinessRule
{
    [Serializable]
    public class brUplogs : IBrObject<xt_uplogs>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string bmbm = "bmbm";
            public const string mcmc = "mcmc";
        }
        [Serializable]
        public class QueryContext
        {
            public DateTime? datef { get; set; }
            public DateTime? datet { get; set; }

            public Sorter sorter { get; set; }
            public string cu_no { get; set; }
            public int status { get; set; }

            public string seartext { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-2);
                datet = DateTime.Now;
            }


        }


        public brUplogs()
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

        void IBrObject<xt_uplogs>.CheckAction(xt_uplogs entity, ActionEnum action) { }

        private IQueryable<xt_uplogs> GetByFilter(IQueryable<xt_uplogs> q, Filter filter)
        {
  
                    return q.OrderByDescending(x => x.fid);
          
        }

        private IQueryable<xt_uplogs> GetBySorter(IQueryable<xt_uplogs> q, Sorter sorter)
        {
   
                    return q.OrderByDescending(x => x.fid);
            
        }

        public IEnumerable<xt_uplogs> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.xt_uplogs, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.xt_uplogs, filter).Count();
        }




        public IEnumerable<xt_uplogs> QueryS(string inCu_no, string inscode)  //返回下级客户
        {

            IQueryable<xt_uplogs> ret = db.xt_uplogs;

            //if (inCu_no == null || inCu_no.Length == 0)
            //    ret = ret.Where(x => x.cu_no == "");
            //else
            //{
            //    if (inCu_no != "-1")
            //        ret = ret.Where(x => x.cu_no == inCu_no);
            //}


            if (inscode != null && inscode.Length >= 0)
                ret = ret.Where(x => x.cu_no.Contains(inscode) || x.filename.Contains(inscode)) ;

            ret = ret.OrderByDescending(x => x.fid);

            return ret;
        }




        public IEnumerable<xt_uplogs> Query(string inCode, string inCu_no)
        {

            IQueryable<xt_uplogs> ret = db.xt_uplogs;



            return ret.OrderByDescending(x => x.fid);
        }



        public void Insert(xt_uplogs entity)
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
                    if (db.xt_uplogs.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.xt_uplogs.InsertOnSubmit(entity);

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

        public void Update(xt_uplogs entity)
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
                    xt_uplogs dbdata = db.xt_uplogs.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<xt_uplogs>(entity, dbdata);
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

        public void Delete(int inFid)
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
                    xt_uplogs dbdata = db.xt_uplogs.FirstOrDefault(x => x.fid == inFid);
                    if (dbdata != null)
                        db.xt_uplogs.DeleteOnSubmit(dbdata);
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

        public void Delete(xt_uplogs entity)
        {
            this.Delete(entity.fid);
        }

        public xt_uplogs NewEntity()
        {
            return new xt_uplogs();
        }


        private IQueryable<xt_uplogs> GetAll(QueryContext context)
        {
            IQueryable<xt_uplogs> ret = db.xt_uplogs;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.in_date >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.in_date <= context.datet);

            if (context.cu_no != null && context.cu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.StartsWith(context.cu_no));

            //if (context.status != 10)
            //    ret = ret.Where(x => x.status == context.status);


            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.StartsWith(context.seartext));

             ret = ret.OrderByDescending(x => x.fid);
          


            //     ret = ret.Distinct();

            return ret;
        }


       
        public IEnumerable<xt_uplogs> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }


        public IEnumerable<xt_uplogs> QueryN()
        {

            IQueryable<xt_uplogs> ret = db.xt_uplogs.Where(x => (x.status == 0 ) && (System.DateTime.Now - Convert.ToDateTime(x.in_date)).TotalMinutes > 1);

            return ret.OrderByDescending(x => x.fid);
        }

        public int reCount()
        {
            return db.xt_uplogs.Where(x => x.status == 0 && (System.DateTime.Now - Convert.ToDateTime(x.in_date)).TotalMinutes > 1).Count();
        }



    }
}
