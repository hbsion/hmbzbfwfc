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
    public class brCuShipScan : IBrObject<sal_cuship>
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
            public string unitcode { get; set; }
            public string st_no { get; set; }

            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-1);
                datet = DateTime.Now;
            }


        }


        public brCuShipScan()
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

        void IBrObject<sal_cuship>.CheckAction(sal_cuship entity, ActionEnum action) { }

        private IQueryable<sal_cuship> GetByFilter(IQueryable<sal_cuship> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.bmbm:
                    return q.Where(x => x.bsnno.StartsWith(filter.SearchText));
                case FieldType.mcmc:
                    return q.Where(x => x.cu_name.Contains(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode==filter.SearchText);
                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        private IQueryable<sal_cuship> GetBySorter(IQueryable<sal_cuship> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.bmbm:
                    return sorter.ASC ? q.OrderBy(x => x.bsnno) : q.OrderByDescending(x => x.bsnno);
                case FieldType.mcmc:
                    return sorter.ASC ? q.OrderBy(x => x.cu_name) : q.OrderByDescending(x => x.cu_name);

                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        public IEnumerable<sal_cuship> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.sal_cuship, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.sal_cuship, filter).Count();
        }

        public sal_cuship Retrieve(string inCode)
        {
            return db.sal_cuship.FirstOrDefault(x => x.bsnno == inCode);
        }

        public sal_cuship Retrieve(string inCode,string inCu_no)
        {
            return db.sal_cuship.FirstOrDefault(x => x.bsnno == inCode && x.cu_no == inCu_no);
        }

        public sal_cuship resale(Barcode inBrcode, string incu_no)  //否有销售
        {
            return db.sal_cuship.FirstOrDefault(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.xtcu_no == incu_no);
        }

        public sal_cuship Retrieve(string inCode, string inCode2,int op)
        {
            return db.sal_cuship.FirstOrDefault(x => x.bsnno == inCode || x.bsnno == inCode2 );
        }

        public sal_cuship Retrieve(Barcode inBrcode,string incu_no, string inunitcode)
        {
            return db.sal_cuship.FirstOrDefault(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.cu_no == incu_no && x.unitcode == inunitcode);
        }

        public sal_cuship Retrieve(string inCode, string inCu_no, string inunitcode)
        {
            return db.sal_cuship.FirstOrDefault(x => x.bsnno == inCode && x.xtcu_no == inCu_no && x.unitcode == inunitcode);
        }

        public sal_cuship RetrieveB(Barcode inBrcode, string incu_no)  //上级出库
        {
            return db.sal_cuship.FirstOrDefault(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.cu_no == incu_no );
        }

        public sal_cuship RetrieveB(Barcode inBrcode,  DateTime? indate)  //扫描枪出库
        {
            if (indate == null)
                return db.sal_cuship.FirstOrDefault(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode));
            else
                return db.sal_cuship.FirstOrDefault(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.ship_date > indate);
        }
        public sal_cuship RetrieveB(Barcode inBrcode, string inunitcode, DateTime? indate)  //扫描枪出库
        {
            if (indate == null)
                return db.sal_cuship.FirstOrDefault(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.unitcode == inunitcode);
            else
                return db.sal_cuship.FirstOrDefault(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.unitcode == inunitcode && x.ship_date > indate);
        }
        public sal_cuship RetrieveRE(Barcode inBrcode, string inunitcode)  //扫描枪退货用
        {
            IQueryable<sal_cuship> ret = db.sal_cuship.Where(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode));
            return ret.OrderByDescending(x => x.fid).FirstOrDefault();
        }
        public IEnumerable<sal_cuship> Query(Barcode inBrcode, string inCu_no, string inunitcode)
        {

            IQueryable<sal_cuship> ret = db.sal_cuship;


            ret = ret.Where(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.unitcode == inunitcode);

            if (inCu_no != null && inCu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no == inCu_no);

            return ret.OrderBy(x => x.fid);
        }


        public IEnumerable<sal_cuship> Query(string inCode, string inCu_no)
        {
 
                IQueryable<sal_cuship> ret = db.sal_cuship;


                ret = ret.Where(x => x.bsnno == inCode);

                if (inCu_no!=null && inCu_no.Trim().Length > 0)
                    ret = ret.Where(x => x.cu_no == inCu_no);

                return ret.OrderByDescending(x=>x.fid);
         }


        public IEnumerable<sal_cuship> Query(string inCode, string inCu_no,string inunitcode)
        {

            IQueryable<sal_cuship> ret = db.sal_cuship;


            ret = ret.Where(x => x.bsnno == inCode);

            if (inCu_no != null && inCu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no == inCu_no);

            ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderByDescending(x => x.fid);
        }


        public IEnumerable<sal_cuship> Query(string inCode, string inCode2, string inCu_no, string inunitcode)
        {

            IQueryable<sal_cuship> ret = db.sal_cuship;


            ret = ret.Where(x => x.bsnno == inCode || x.bsnno == inCode2 );

            if (inCu_no != null && inCu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no == inCu_no);

            ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderByDescending(x => x.fid);
        }


        public void Insert(sal_cuship entity)
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
                    if (db.sal_cuship.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.sal_cuship.InsertOnSubmit(entity);
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

        public void Update(sal_cuship entity)
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
                    sal_cuship dbdata = db.sal_cuship.FirstOrDefault(x => x.fid == entity.fid );
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<sal_cuship>(entity, dbdata);
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
                    sal_cuship dbdata = db.sal_cuship.FirstOrDefault(x => x.fid == inFid);
                    if (dbdata != null)
                        db.sal_cuship.DeleteOnSubmit(dbdata);
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

        public void Delete(sal_cuship entity)
        {
            this.Delete(entity.fid);
        }

        public sal_cuship NewEntity()
        {
            return new sal_cuship();
        }

  

        private IQueryable<sal_cuship> GetAll(QueryContext context)
        {
            IQueryable<sal_cuship> ret = db.sal_cuship;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.ship_date >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.ship_date <= context.datet);
            if (context.bsnno != null && context.bsnno.Trim().Length > 0)
                ret = ret.Where(x => x.bsnno.StartsWith(context.bsnno.Trim()));
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


            if (context.xtcu_no != null && context.xtcu_no.Trim().Length > 0)
                ret = ret.Where(x => x.xtcu_no == context.xtcu_no);

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);

            if (context.st_no != null && context.st_no.Trim().Length > 0)
                ret = ret.Where(x => x.st_no == context.st_no);

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

       //     ret = ret.Distinct();

            return ret;
        }

        public IEnumerable<sal_cuship> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }


        public sal_cuship RetrieveRE(Barcode inBrcode, string incu_no, string inunitcode)  //返回上级最后一条出库记录
        {
            IQueryable<sal_cuship> ret = db.sal_cuship.Where(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.unitcode == inunitcode && x.cu_no == incu_no);
            return ret.OrderByDescending(x => x.fid).FirstOrDefault();
        }

        public sal_cuship RetrieveXt(Barcode inBrcode, string xtcu_no, string inunitcode)  //返回最后一条出库记录
        {
            IQueryable<sal_cuship> ret = db.sal_cuship.Where(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode) && x.unitcode == inunitcode && x.xtcu_no == xtcu_no);
            return ret.OrderByDescending(x => x.fid).FirstOrDefault();
        }
        public sal_cuship RetrieveXt(Barcode inBrcode)  //返回最后一条出库记录
        {
            IQueryable<sal_cuship> ret = db.sal_cuship.Where(x => (x.bsnno == inBrcode.Code || x.bsnno == inBrcode.Tcode || x.bsnno == inBrcode.Ucode));
            return ret.OrderByDescending(x => x.fid).FirstOrDefault();
        }
        //根据出货单号查询相对应的出货记录
        public sal_cuship getShipRecord(string ship_no)
        {
            return db.sal_cuship.Where(x => x.ship_no == ship_no).FirstOrDefault();
        }
    }
}
