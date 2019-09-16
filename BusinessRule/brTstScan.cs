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
    public class brTstScan : IBrObject<sal_tst>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string bmbm = "bmbm";
            public const string mcmc = "mcmc";
            public const string unitcode = "unitcode";

        }

        [Serializable] 
        public class QueryContext
        {
            public DateTime? datef { get; set; }
            public DateTime? datet { get; set; }
            public string bsnno { get; set; }
            public string pname { get; set; }
            public string p_no { get; set; }
            public Sorter sorter { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public int fid { get; set; }
            public string type { get; set; }
            public string unitcode { get; set; }
            public string ost_no { get; set; }
            public string nst_no { get; set; }

            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-1);
                datet = DateTime.Now;
            }


        }


        public brTstScan()
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

        void IBrObject<sal_tst>.CheckAction(sal_tst entity, ActionEnum action) { }

        private IQueryable<sal_tst> GetByFilter(IQueryable<sal_tst> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.bmbm:
                    return q.Where(x => x.bsnno.StartsWith(filter.SearchText));
                case FieldType.mcmc:
                    return q.Where(x => x.ost_no.Contains(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode==filter.SearchText);
                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        private IQueryable<sal_tst> GetBySorter(IQueryable<sal_tst> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.bmbm:
                    return sorter.ASC ? q.OrderBy(x => x.bsnno) : q.OrderByDescending(x => x.bsnno);
                case FieldType.mcmc:
                    return sorter.ASC ? q.OrderBy(x => x.ost_no) : q.OrderByDescending(x => x.ost_no);

                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        public IEnumerable<sal_tst> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.sal_tst, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.sal_tst, filter).Count();
        }

        public sal_tst Retrieve(string inCode)
        {
            return db.sal_tst.FirstOrDefault(x => x.bsnno == inCode);
        }


        public sal_tst Retrievetst(string inCode,string innst_no,string inost_no)
        {
            return db.sal_tst.FirstOrDefault(x => x.bsnno == inCode && x.nst_no == innst_no && x.ost_no == inost_no);
        }
 

        public sal_tst Retrieve(string inCode, string inCode2,int op)
        {
            return db.sal_tst.FirstOrDefault(x => x.bsnno == inCode || x.bsnno == inCode2 );
        }

        public sal_tst Retrieve(string inCode, string inCu_no, string inunitcode)
        {
            return db.sal_tst.FirstOrDefault(x => x.bsnno == inCode     && x.unitcode == inunitcode);
        }


        public IEnumerable<sal_tst> Query(string inCode, string inCu_no)
        {
 
                IQueryable<sal_tst> ret = db.sal_tst;


                ret = ret.Where(x => x.bsnno == inCode);

 
                return ret.OrderByDescending(x=>x.fid);
         }

        public IEnumerable<sal_tst> Query(string inCode)
        {

            IQueryable<sal_tst> ret = db.sal_tst;

            return ret.OrderByDescending(x => x.fid);
        }

        public IEnumerable<sal_tst> Query(string inCode,DateTime? indate )
        {

            IQueryable<sal_tst> ret = db.sal_tst.Where(x=>x.bsnno==inCode);

            if (indate != null)
            {
                ret = ret.Where(x => x.ship_date > indate);
            }

            return ret.OrderByDescending(x => x.fid);
        }


        public IEnumerable<sal_tst> Query(string inCode, string inCu_no,string inunitcode)
        {

            IQueryable<sal_tst> ret = db.sal_tst;


            ret = ret.Where(x => x.bsnno == inCode);

 
            ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderByDescending(x => x.fid);
        }


        public IEnumerable<sal_tst> Query(Barcode inBrcode, string inCu_no, string inunitcode)
        {

            IQueryable<sal_tst> ret = db.sal_tst;


            ret = ret.Where(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.unitcode == inunitcode);


            return ret.OrderByDescending(x => x.fid);
        }


        public IEnumerable<sal_tst> Query(string inCode, string inCode2, string inCu_no, string inunitcode)
        {

            IQueryable<sal_tst> ret = db.sal_tst;


            ret = ret.Where(x => x.bsnno == inCode || x.bsnno == inCode2 );

 

            ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderByDescending(x => x.fid);
        }


        public void Insert(sal_tst entity)
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
                    if (db.sal_tst.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.sal_tst.InsertOnSubmit(entity);
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

        public void Update(sal_tst entity)
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
                    sal_tst dbdata = db.sal_tst.FirstOrDefault(x => x.fid == entity.fid );
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<sal_tst>(entity, dbdata);
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
                    sal_tst dbdata = db.sal_tst.FirstOrDefault(x => x.fid == inFid);
                    if (dbdata != null)
                        db.sal_tst.DeleteOnSubmit(dbdata);
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

        public void Delete(sal_tst entity)
        {
            this.Delete(entity.fid);
        }

        public sal_tst NewEntity()
        {
            return new sal_tst();
        }

  

        private IQueryable<sal_tst> GetAll(QueryContext context)
        {
            IQueryable<sal_tst> ret = db.sal_tst;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.ship_date >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.ship_date <= context.datet.Value.AddDays(1));
            if (context.bsnno != null && context.bsnno.Trim().Length > 0)
                ret = ret.Where(x => x.bsnno.StartsWith(context.bsnno.Trim()));
            if (context.pname != null && context.pname.Trim().Length > 0)
                ret = ret.Where(x => x.pname.StartsWith(context.pname.Trim()));
            if (context.p_no != null && context.p_no.Trim().Length > 0)
                ret = ret.Where(x => x.p_no.StartsWith(context.p_no.Trim()));

            if (context.type != null && context.type.Trim().Length > 0)
                ret = ret.Where(x => x.type.StartsWith(context.type.Trim()));


            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);

            if (context.ost_no != null && context.ost_no.Trim().Length > 0)
                ret = ret.Where(x => x.ost_no == context.ost_no);

            if (context.nst_no != null && context.nst_no.Trim().Length > 0)
                ret = ret.Where(x => x.nst_no == context.nst_no);


            switch (context.sorter.Field)
            {
                case "bsnno":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.bsnno) : ret.OrderByDescending(x => x.bsnno);
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
                case "ost_no":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.ost_no) : ret.OrderByDescending(x => x.ost_no);
                    break;
                case "nst_no":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.nst_no) : ret.OrderByDescending(x => x.nst_no);
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

        public IEnumerable<sal_tst> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }




     
    }
}
