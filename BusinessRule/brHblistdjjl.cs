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
    public class brHblistdjj : IBrObject<hob_listdjjl>
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
   
            public string dian_no { get; set; }

            public Sorter sorter { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }


            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-1);
                datet = DateTime.Now;
            }


        }


        public brHblistdjj()
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

        void IBrObject<hob_listdjjl>.CheckAction(hob_listdjjl entity, ActionEnum action) { }

        private IQueryable<hob_listdjjl> GetByFilter(IQueryable<hob_listdjjl> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.bmbm:
                    return q.Where(x => x.dian_no.StartsWith(filter.SearchText));
                case FieldType.mcmc:
                    return q.Where(x => x.dian_name.Contains(filter.SearchText));
                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        private IQueryable<hob_listdjjl> GetBySorter(IQueryable<hob_listdjjl> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.bmbm:
                    return sorter.ASC ? q.OrderBy(x => x.dian_no) : q.OrderByDescending(x => x.dian_no);
                case FieldType.mcmc:
                    return sorter.ASC ? q.OrderBy(x => x.dian_name) : q.OrderByDescending(x => x.dian_name);

                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        public IEnumerable<hob_listdjjl> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.hob_listdjjl, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.hob_listdjjl, filter).Count();
        }


        public int Record(string inCode)
        {

            return db.hob_listdjjl.GroupBy(x => x.fwcode == inCode).Count();
        }

        public hob_listdjjl Retrieve(string indian_no)
        {

            return db.hob_listdjjl.FirstOrDefault(x => x.dian_no == indian_no);

        }

        public hob_listdjjl Retrieve(string inCode, string indian_no)
        {
            return db.hob_listdjjl.FirstOrDefault(x => x.fwcode == inCode && x.dian_no == indian_no);
        }



        public hob_listdjjl Retrlast(string inwxid)
        {

            return db.hob_listdjjl.FirstOrDefault(x => x.wx_id == inwxid);

        }



        public IEnumerable<hob_listdjjl> Query(string inCode, string inCu_no)
        {

            IQueryable<hob_listdjjl> ret = db.hob_listdjjl;


            ret = ret.Where(x => x.dian_no == inCode);



            return ret.OrderByDescending(x => x.fid);
        }




        public IEnumerable<hob_listdjjl> rewxid(string inwx_id)
        {
            IQueryable<hob_listdjjl> ret = db.hob_listdjjl.Where(x=>x.wx_id==inwx_id);
 
            return ret.OrderByDescending(x => x.in_date);
        }


        public void Insert(hob_listdjjl entity)
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
                    if (db.hob_listdjjl.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.hob_listdjjl.InsertOnSubmit(entity);

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

        public void Update(hob_listdjjl entity)
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
                    hob_listdjjl dbdata = db.hob_listdjjl.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<hob_listdjjl>(entity, dbdata);
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
                    hob_listdjjl dbdata = db.hob_listdjjl.FirstOrDefault(x => x.fid == inFid);
                    if (dbdata != null)
                        db.hob_listdjjl.DeleteOnSubmit(dbdata);
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

        public void Delete(hob_listdjjl entity)
        {
            this.Delete(entity.fid);
        }

        public hob_listdjjl NewEntity()
        {
            return new hob_listdjjl();
        }


        private IQueryable<hob_listdjjl> GetAll(QueryContext context)
        {
            IQueryable<hob_listdjjl> ret = db.hob_listdjjl;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.in_date >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.in_date <= context.datet.Value.AddDays(1));



            if (context.dian_no != null && context.dian_no.Trim().Length > 0)
                ret = ret.Where(x => context.dian_no.IndexOf(x.dian_no) >= 0 && x.dian_no.Trim().Length == 4);

   


            switch (context.sorter.Field)
            {

 
                case "in_date":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.in_date) : ret.OrderByDescending(x => x.in_date);
                    break;


                case "fid":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.fid) : ret.OrderByDescending(x => x.fid);
                    break;

                default:
                    ret = ret.OrderByDescending(x => x.fid);
                    break;

            }

            //     ret = ret.Distinct();

            return ret;
        }

        public IEnumerable<hob_listdjjl> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }


        public int Count(string fwcode,string indian_no)
        {
            return db.hob_listdjjl.Where(x => x.fwcode==fwcode &&  x.dian_no == indian_no).Count();

        }

        public hob_listdjjl RetrieveD(string inCode, string indian_no)
        {
            return db.hob_listdjjl.FirstOrDefault(x => x.wx_id == inCode && x.dian_no == indian_no && (System.DateTime.Now - Convert.ToDateTime(x.in_date)).TotalDays < 1);
        }



    }
}
