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
    public class brHbyrerout : IBrObject<hob_yuerout>
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
   
            public string unitcode { get; set; }

            public Sorter sorter { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }


            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-1);
                datet = DateTime.Now;
            }


        }


        public brHbyrerout()
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

        void IBrObject<hob_yuerout>.CheckAction(hob_yuerout entity, ActionEnum action) { }

        private IQueryable<hob_yuerout> GetByFilter(IQueryable<hob_yuerout> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.bmbm:
                    return q.Where(x => x.bill_no.StartsWith(filter.SearchText));
                case FieldType.mcmc:
                    return q.Where(x => x.czybm.Contains(filter.SearchText));
                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        private IQueryable<hob_yuerout> GetBySorter(IQueryable<hob_yuerout> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.bmbm:
                    return sorter.ASC ? q.OrderBy(x => x.bill_no) : q.OrderByDescending(x => x.bill_no);
                case FieldType.mcmc:
                    return sorter.ASC ? q.OrderBy(x => x.czybm) : q.OrderByDescending(x => x.czybm);

                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        public IEnumerable<hob_yuerout> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.hob_yuerout, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.hob_yuerout, filter).Count();
        }


        public int Record(string inCode)
        {

            return db.hob_yuerout.GroupBy(x => x.fwcode == inCode).Count();
        }

        public hob_yuerout Retrieve(string inunitcode)
        {

            return db.hob_yuerout.FirstOrDefault(x => x.unitcode == inunitcode);

        }

        public hob_yuerout Retrieve(string inCode, string inunitcode)
        {
            return db.hob_yuerout.FirstOrDefault(x => x.fwcode == inCode );
        }

        public hob_yuerout Retrievekey(string inCode, string inunitcode)
        {
            return db.hob_yuerout.FirstOrDefault(x => x.keycode == inCode && x.unitcode == inunitcode);
        }

        public hob_yuerout Retrlast(string inwxid)
        {

            return db.hob_yuerout.FirstOrDefault(x => x.wx_id == inwxid);

        }



        public IEnumerable<hob_yuerout> Query(string inCode, string inCu_no)
        {

            IQueryable<hob_yuerout> ret = db.hob_yuerout;


            ret = ret.Where(x => x.bill_no == inCode);

            if (inCu_no != null && inCu_no.Trim().Length > 0)
                ret = ret.Where(x => x.czybm == inCu_no);

            return ret.OrderByDescending(x => x.fid);
        }




        public IEnumerable<hob_yuerout> rewxid(string inwx_id)
        {
            IQueryable<hob_yuerout> ret = db.hob_yuerout.Where(x=>x.wx_id==inwx_id);
 
            return ret.OrderByDescending(x => x.in_date);
        }


        public void Insert(hob_yuerout entity)
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
                    if (db.hob_yuerout.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.hob_yuerout.InsertOnSubmit(entity);

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

        public void Update(hob_yuerout entity)
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
                    hob_yuerout dbdata = db.hob_yuerout.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<hob_yuerout>(entity, dbdata);
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
                    hob_yuerout dbdata = db.hob_yuerout.FirstOrDefault(x => x.fid == inFid);
                    if (dbdata != null)
                        db.hob_yuerout.DeleteOnSubmit(dbdata);
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

        public void Delete(hob_yuerout entity)
        {
            this.Delete(entity.fid);
        }

        public hob_yuerout NewEntity()
        {
            return new hob_yuerout();
        }


        private IQueryable<hob_yuerout> GetAll(QueryContext context)
        {
            IQueryable<hob_yuerout> ret = db.hob_yuerout;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.in_date >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.in_date <= context.datet.Value.AddDays(1));

          
            //if (context.unitcode != null && context.unitcode.Trim().Length > 0)
            //    ret = ret.Where(x => x.unitcode == context.unitcode);

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => context.unitcode.IndexOf(x.unitcode) >= 0 && x.unitcode.Trim().Length == 4);

   


            switch (context.sorter.Field)
            {
                case "bill_no":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.bill_no) : ret.OrderByDescending(x => x.bill_no);
                    break;
 
                case "in_date":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.in_date) : ret.OrderByDescending(x => x.in_date);
                    break;
                   case "czybm":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.czybm) : ret.OrderByDescending(x => x.czybm);
                    break;

                case "mqty":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.mqty) : ret.OrderByDescending(x => x.mqty);
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

        public IEnumerable<hob_yuerout> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }


        public int Count(string fwcode,string inunitcode)
        {
            return db.hob_yuerout.Where(x => x.fwcode==fwcode &&  x.unitcode == inunitcode).Count();

        }

        public hob_yuerout RetrieveD(string inCode, string inunitcode)
        {
            return db.hob_yuerout.FirstOrDefault(x => x.wx_id == inCode && x.unitcode == inunitcode && (System.DateTime.Now - Convert.ToDateTime(x.in_date)).TotalDays < 1);
        }



        public decimal sumqty(string inunitcode)
        {
            return db.hob_yuerout.Where(x => x.unitcode == inunitcode).Sum(x => (decimal?)x.mqty).GetValueOrDefault();
        }


    }
}
