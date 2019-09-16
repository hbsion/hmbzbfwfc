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
    public class brQclot : IBrObject<inv_qclot>
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
            public string ship_no { get; set; }
            public string cu_no { get; set; }
            public string cu_name { get; set; }
            public string pname { get; set; }
            public string p_no { get; set; }
            public Sorter sorter { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public string xtcu_no { get; set; }
            public int fid { get; set; }
            public string type { get; set; }

            public string mtype { get; set; }

            public string seartext { get; set; }

            public string unitcode  { get; set; }


      


        }


        public brQclot()
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

        void IBrObject<inv_qclot>.CheckAction(inv_qclot entity, ActionEnum action) { }

        private IQueryable<inv_qclot> GetByFilter(IQueryable<inv_qclot> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.bmbm:
                    return q.Where(x => x.ship_no.StartsWith(filter.SearchText));
                case FieldType.mcmc:
                    return q.Where(x => x.cu_name.Contains(filter.SearchText));
                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }


        private IQueryable<inv_qclot> GetBySorter(IQueryable<inv_qclot> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.bmbm:
                    return sorter.ASC ? q.OrderBy(x => x.ship_no) : q.OrderByDescending(x => x.ship_no);
                case FieldType.mcmc:
                    return sorter.ASC ? q.OrderBy(x => x.cu_name) : q.OrderByDescending(x => x.cu_name);

                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        public IEnumerable<inv_qclot> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.inv_qclot, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.inv_qclot, filter).Count();
        }

        public inv_qclot Retrieve(string inCode)
        {

            return db.inv_qclot.FirstOrDefault(x => x.ship_no == inCode);

        }

        public inv_qclot Retrieve(int inCode)
        {

            return db.inv_qclot.FirstOrDefault(x => x.fid == inCode);

        }


        public inv_qclot Retrieve2(string inCode)
        {

           return db.inv_qclot.FirstOrDefault(x => x.ship_no == inCode);

        }

        public inv_qclot Retrieve(string inCode, string inCu_no)
        {
            return db.inv_qclot.FirstOrDefault(x => x.ship_no == inCode && x.unitcode == inCu_no);
        }




        public inv_qclot Retrieve(string inCode, string inCode2, int op)
        {
            return db.inv_qclot.FirstOrDefault(x => x.ship_no == inCode || x.ship_no == inCode2);
        }

        public IEnumerable<inv_qclot> Query(string inCode)
        {

            IQueryable<inv_qclot> ret = db.inv_qclot;


            ret = ret.Where(x => x.ship_no == inCode);


            return ret.OrderByDescending(x => x.fid);
        }


        public IEnumerable<inv_qclot> Querylot(string inCode)
        {

            IQueryable<inv_qclot> ret = db.inv_qclot;


            ret = ret.Where(x => x.lot_no == inCode);


            return ret.OrderByDescending(x => x.fid);
        }
 


        public void Insert(inv_qclot entity)
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
                    if (db.inv_qclot.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.inv_qclot.InsertOnSubmit(entity);

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

        public void Update(inv_qclot entity)
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
                    inv_qclot dbdata = db.inv_qclot.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<inv_qclot>(entity, dbdata);
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
                    inv_qclot dbdata = db.inv_qclot.FirstOrDefault(x => x.fid == inFid);
                    if (dbdata != null)
                        db.inv_qclot.DeleteOnSubmit(dbdata);
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

        public void Delete(inv_qclot entity)
        {
            this.Delete(entity.fid);
        }

        public inv_qclot NewEntity()
        {
            return new inv_qclot();
        }


        private IQueryable<inv_qclot> GetAll(QueryContext context)
        {
            IQueryable<inv_qclot> ret = db.inv_qclot;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.ship_date >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.ship_date <= context.datet);

            if (context.ship_no != null && context.ship_no.Trim().Length > 0)
                ret = ret.Where(x => x.ship_no==context.ship_no.Trim());

            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.ship_no.Contains(context.ship_no.Trim()) || x.p_no.Contains(context.ship_no.Trim()) || x.pname.Contains(context.ship_no.Trim()));


            if (context.cu_name != null && context.cu_name.Trim().Length > 0)
                ret = ret.Where(x => x.cu_name.StartsWith(context.cu_name.Trim()));
           
            if (context.pname != null && context.pname.Trim().Length > 0)
                ret = ret.Where(x => x.pname.StartsWith(context.pname.Trim()));
           
            if (context.p_no != null && context.p_no.Trim().Length > 0)
                ret = ret.Where(x => x.p_no.StartsWith(context.p_no.Trim()));
          
            if (context.cu_no != null && context.cu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.StartsWith(context.cu_no.Trim()));

   
            if (context.type != null && context.type.Trim().Length > 0)
                ret = ret.Where(x => x.type.StartsWith(context.type.Trim()));

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);


            if (context.mtype != null && context.mtype.Trim().Length > 0)
                ret = ret.Where(x => x.mtype==context.mtype.Trim());



            switch (context.sorter.Field)
            {
                case "ship_no":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.ship_no) : ret.OrderByDescending(x => x.ship_no);
                    break;
                case "p_no":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.p_no) : ret.OrderByDescending(x => x.p_no);
                    break;
                case "pname":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.pname) : ret.OrderByDescending(x => x.pname);
                    break;
                case "ship_date":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.ship_date) : ret.OrderByDescending(x => x.ship_date);
                    break;
                case "loca":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.loca) : ret.OrderByDescending(x => x.loca);
                    break;
                case "cu_name":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.cu_name) : ret.OrderByDescending(x => x.cu_name);
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


            return ret;
        }

        public IEnumerable<inv_qclot> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }




    }
}
