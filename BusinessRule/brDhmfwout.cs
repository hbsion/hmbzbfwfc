using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brDhmfwout: IDisposable 
    {
        public static class FieldType
        {
            public const string None = null;
            public const string UnitCode = "UnitCode";
            public const string Querystatu = "Querystatu";

        }


        public class QueryContext
        {
            public DateTime? datef { get; set; }
            public DateTime? datet { get; set; }
            public string UnitCode { get; set; }

            public  int mqty { get; set; }
         
            public Sorter sorter { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public int fid { get; set; }


            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-1);
                datet = DateTime.Now;
            }


        }


        public brDhmfwout()
        {
            db = new dbDataContext(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
        }

        private dbDataContext db;

        public void Dispose()
        {
            db.Dispose();
            db = null;
        }



        private IQueryable<dhm_fwout> GetByFilter(IQueryable<dhm_fwout> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.UnitCode:
                    return q.Where(x => x.UnitCode.StartsWith(filter.SearchText));

                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        private IQueryable<dhm_fwout> GetBySorter(IQueryable<dhm_fwout> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.UnitCode:
                    return sorter.ASC ? q.OrderBy(x => x.UnitCode) : q.OrderByDescending(x => x.UnitCode);

                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        public IEnumerable<dhm_fwout> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.dhm_fwout, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.dhm_fwout, filter).Count();
        }

        public dhm_fwout Retrieve(string inCode)
        {
            return db.dhm_fwout.FirstOrDefault(x => x.UnitCode == inCode);
        }

        public dhm_fwout Retrieve(string inCode,long myk)
        {
            return db.dhm_fwout.FirstOrDefault(x => x.UnitCode == inCode && x.myBegin<= myk && (x.myBegin+ x.SellCount) > myk );
        }



        public dhm_fwout Retrieve(int  inCode, long myk)
        {
            return db.dhm_fwout.FirstOrDefault(x => x.codeLen == inCode && x.myBegin <= myk && (x.myBegin + x.SellCount) > myk);
        }


        public dhm_fwout Retrieve(string inCode, string inCode2, int op)
        {
            return db.dhm_fwout.FirstOrDefault(x => x.UnitCode == inCode || x.UnitCode == inCode2);
        }

        public IEnumerable<dhm_fwout> Query(string inCode, string inCu_no)
        {

            IQueryable<dhm_fwout> ret = db.dhm_fwout;


            ret = ret.Where(x => x.UnitCode == inCode);

            return ret.OrderByDescending(x => x.fid);
        }


        public IEnumerable<dhm_fwout> Query(string inCode, string inCode1, string inCode2, string inCu_no)
        {

            IQueryable<dhm_fwout> ret = db.dhm_fwout;


            ret = ret.Where(x => x.UnitCode == inCode || x.UnitCode == inCode1 || x.UnitCode == inCode2);



            return ret.OrderByDescending(x => x.fid);
        }



        public void Insert(dhm_fwout entity)
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
                    if (db.dhm_fwout.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.dhm_fwout.InsertOnSubmit(entity);
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

        public void Update(dhm_fwout entity)
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
                    dhm_fwout dbdata = db.dhm_fwout.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<dhm_fwout>(entity, dbdata);
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
                    dhm_fwout dbdata = db.dhm_fwout.FirstOrDefault(x => x.fid == inFid);
                    if (dbdata != null)
                        db.dhm_fwout.DeleteOnSubmit(dbdata);
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

        public void Delete(dhm_fwout entity)
        {
            this.Delete(entity.fid);
        }

        public dhm_fwout NewEntity()
        {
            return new dhm_fwout();
        }



        private IQueryable<dhm_fwout> GetAll(QueryContext context)
        {
            IQueryable<dhm_fwout> ret = db.dhm_fwout.Where(x=>x.UnitCode !="") ;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.SellDateTime >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.SellDateTime <= context.datet);
            if (context.UnitCode != null && context.UnitCode.Trim().Length > 0)
                ret = ret.Where(x => x.UnitCode.StartsWith(context.UnitCode.Trim()));




            switch (context.sorter.Field)
            {
                case "UnitCode":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.UnitCode) : ret.OrderByDescending(x => x.UnitCode);
                    break;

                case "SellDateTime":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.SellDateTime) : ret.OrderByDescending(x => x.SellDateTime);
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

        public IEnumerable<dhm_fwout> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }





    }
}
