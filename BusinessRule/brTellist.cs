using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brTellist : IBrObject<tellist>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string FWCode = "FWCode";
            public const string Querystatu = "Querystatu";

        }

        public class EntityByQuerystatu
        {
            public string Querystatu { get; set; }
            public int? sumqty { get; set; }

        }

        public class EntityByqutype
        {
            public string qutype { get; set; }
            public int? sumqty { get; set; }

        }


        public class EntityByremark
        {
            public string remark { get; set; }
            public int? sumqty { get; set; }

        }

        public class EntityBycallerid
        {
            public string callerid { get; set; }
            public int? sumqty { get; set; }

        }

        public class EntityByFWCode
        {
            public string FWCode { get; set; }
            public int? sumqty { get; set; }

        }

        public class QueryContext
        {
            public DateTime? datef { get; set; }
            public DateTime? datet { get; set; }
            public string FWCode { get; set; }
            public string Querystatu { get; set; }
            public string callerid { get; set; }
            public string remark { get; set; }
            public string qutype { get; set; }
            public string unitcode { get; set; 
            }
            public string loca { get; set; }

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


        public brTellist()
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

        void IBrObject<tellist>.CheckAction(tellist entity, ActionEnum action) { }

        private IQueryable<tellist> GetByFilter(IQueryable<tellist> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.FWCode:
                    return q.Where(x => x.FWCode.StartsWith(filter.SearchText));
                case FieldType.Querystatu:
                    return q.Where(x => x.Querystatu.Contains(filter.SearchText));
                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        private IQueryable<tellist> GetBySorter(IQueryable<tellist> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.FWCode:
                    return sorter.ASC ? q.OrderBy(x => x.FWCode) : q.OrderByDescending(x => x.FWCode);
                case FieldType.Querystatu:
                    return sorter.ASC ? q.OrderBy(x => x.Querystatu) : q.OrderByDescending(x => x.Querystatu);

                default:
                    return q.OrderByDescending(x => x.fid);
            }
        }

        public IEnumerable<tellist> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.tellist, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.tellist, filter).Count();
        }

        public tellist Retrieve(string inCode)
        {
            return db.tellist.FirstOrDefault(x => x.FWCode == inCode && (System.DateTime.Now - Convert.ToDateTime(x.QueryDate)).TotalSeconds > 15);
        }

        public tellist RetrieveB(string inCode)
        {
            return db.tellist.FirstOrDefault(x => x.FWCode == inCode && (System.DateTime.Now - Convert.ToDateTime(x.QueryDate)).TotalSeconds < 15);
        }

        public int refwCount(string inCode)
        {
            return db.tellist.Where(x => x.FWCode == inCode && (System.DateTime.Now - Convert.ToDateTime(x.QueryDate)).TotalSeconds > 15).Count() + 1;
        }


        public IEnumerable<tellist> Query()
        {

            IQueryable<tellist> ret = db.tellist;


            ret = ret.Where(x => x.upyn != "Y");

            return ret.OrderBy(x => x.fid);
        }


        public tellist Retrieve(string inCode, string inCode2, int op)
        {
            return db.tellist.FirstOrDefault(x => x.FWCode == inCode || x.FWCode == inCode2);
        }

        public IEnumerable<tellist> Query(string inCode, string inCu_no)
        {

            IQueryable<tellist> ret = db.tellist;


            ret = ret.Where(x => x.FWCode == inCode);

            return ret.OrderBy(x => x.fid);
        }


        public IEnumerable<tellist> Query(string inCode, string inCode1, string inCode2, string inCu_no)
        {

            IQueryable<tellist> ret = db.tellist;


            ret = ret.Where(x => x.FWCode == inCode || x.FWCode == inCode1 || x.FWCode == inCode2);



            return ret.OrderByDescending(x => x.fid);
        }


        public void Insert(tellist entity)
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
                    if (db.tellist.FirstOrDefault(x => x.FWCode == entity.FWCode) == null)
                    {
                        db.tellist.InsertOnSubmit(entity);
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

        public void Update(tellist entity)
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
                    tellist dbdata = db.tellist.FirstOrDefault(x => x.FWCode == entity.FWCode);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<tellist>(entity, dbdata);
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
                    tellist dbdata = db.tellist.FirstOrDefault(x => x.fid == inFid);
                    if (dbdata != null)
                        db.tellist.DeleteOnSubmit(dbdata);
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

        public void Delete(tellist entity)
        {
            this.Delete(entity.fid);
        }

        public tellist NewEntity()
        {
            return new tellist();
        }



        private IQueryable<tellist> GetAll(QueryContext context)
        {
            IQueryable<tellist> ret = db.tellist.Where(x=>x.FWCode !="") ;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.QueryDate >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.QueryDate < context.datet.Value.AddDays(1)   );
            if (context.FWCode != null && context.FWCode.Trim().Length > 0)
                ret = ret.Where(x => x.FWCode.StartsWith(context.FWCode.Trim()));
            if (context.Querystatu != null && context.Querystatu.Trim().Length > 0)
                ret = ret.Where(x => x.Querystatu.StartsWith(context.Querystatu.Trim()));
            if (context.callerid != null && context.callerid.Trim().Length > 0)
                ret = ret.Where(x => x.callerid.StartsWith(context.callerid.Trim()));

            if (context.remark != null && context.remark.Trim().Length > 0)
                ret = ret.Where(x => x.remark.StartsWith(context.remark.Trim()));

            if (context.qutype != null && context.qutype.Trim().Length > 0)
                ret = ret.Where(x => x.qutype.StartsWith(context.qutype.Trim()));


            if (context.loca != null && context.loca.Trim().Length > 0)
                ret = ret.Where(x => x.remark.StartsWith(context.loca.Trim()));

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => context.unitcode.IndexOf(x.unitcode)>=0 &&  x.unitcode.Trim().Length==4  );

            switch (context.sorter.Field)
            {
                case "FWCode":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.FWCode) : ret.OrderByDescending(x => x.FWCode);
                    break;
                case "remark":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.remark) : ret.OrderByDescending(x => x.remark);
                    break;
                case "callerid":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.callerid) : ret.OrderByDescending(x => x.callerid);
                    break;
                case "QueryDate":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.QueryDate) : ret.OrderByDescending(x => x.QueryDate);
                    break;
                case "qutype":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.qutype) : ret.OrderByDescending(x => x.qutype);
                    break;

                case "Querystatu":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.Querystatu) : ret.OrderByDescending(x => x.Querystatu);
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

        public IEnumerable<tellist> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public IEnumerable<EntityByQuerystatu> QuerybyQuerystatu(QueryContext context)
        {

            IQueryable<EntityByQuerystatu> q = this.GetAll(context).GroupBy(x => x.Querystatu).Select(x => new EntityByQuerystatu() { Querystatu = x.Key, sumqty = x.Count() });

            switch (context.sorter.Field)
            {
                case "Querystatu":
                    q = context.sorter.ASC ? q.OrderBy(x => x.Querystatu) : q.OrderByDescending(x => x.Querystatu);
                    break;
                case "sumqty":
                    q = context.sorter.ASC ? q.OrderBy(x => x.sumqty) : q.OrderByDescending(x => x.sumqty);
                    break;

                default:
                    q = q.OrderByDescending(x => x.sumqty);
                    break;
            }

            return Utils.ExecLTS(q.Skip(context.PageIndex * context.PageSize).Take(context.PageSize));

        }


        public IEnumerable<EntityByqutype> Querybyqutype(QueryContext context)
        {
            IQueryable<EntityByqutype> q = this.GetAll(context).GroupBy(x => x.qutype).Select(x => new EntityByqutype() { qutype = x.Key, sumqty = x.Count() });

            switch (context.sorter.Field)
            {
                case "qutype":
                    q = context.sorter.ASC ? q.OrderBy(x => x.qutype) : q.OrderByDescending(x => x.qutype);
                    break;
                case "sumqty":
                    q = context.sorter.ASC ? q.OrderBy(x => x.sumqty) : q.OrderByDescending(x => x.sumqty);
                    break;

                default:
                    q = q.OrderByDescending(x => x.sumqty);
                    break;
            }

            return Utils.ExecLTS(q.Skip(context.PageIndex * context.PageSize).Take(context.PageSize));

        }


        public IEnumerable<EntityByremark> Querybyremark(QueryContext context)
        {
            IQueryable<EntityByremark> q = this.GetAll(context).GroupBy(x => x.remark).Select(x => new EntityByremark() { remark = x.Key, sumqty = x.Count() });

            switch (context.sorter.Field)
            {
                case "remark":
                    q = context.sorter.ASC ? q.OrderBy(x => x.remark) : q.OrderByDescending(x => x.remark);
                    break;
                case "sumqty":
                    q = context.sorter.ASC ? q.OrderBy(x => x.sumqty) : q.OrderByDescending(x => x.sumqty);
                    break;

                default:
                    q = q.OrderByDescending(x => x.sumqty);
                    break;
            }

            return Utils.ExecLTS(q.Skip(context.PageIndex * context.PageSize).Take(context.PageSize));

        }


        public IEnumerable<EntityBycallerid> Querybycallerid(QueryContext context)
        {
            IQueryable<EntityBycallerid> q = this.GetAll(context).GroupBy(x => x.callerid).Select(x => new EntityBycallerid() { callerid = x.Key, sumqty = x.Count() });


            q = q.Where(x => x.sumqty > 1);

            switch (context.sorter.Field)
            {
                case "callerid":
                    q = context.sorter.ASC ? q.OrderBy(x => x.callerid) : q.OrderByDescending(x => x.callerid);
                    break;
                case "sumqty":
                    q = context.sorter.ASC ? q.OrderBy(x => x.sumqty) : q.OrderByDescending(x => x.sumqty);
                    break;

                default:
                    q = q.OrderByDescending(x => x.sumqty);
                    break;
            }

            return Utils.ExecLTS(q.Skip(context.PageIndex * context.PageSize).Take(context.PageSize));

        }



        public IEnumerable<EntityByFWCode> QuerybyFWCode(QueryContext context)
        {
            IQueryable<EntityByFWCode> q = this.GetAll(context).GroupBy(x => x.FWCode).Select(x => new EntityByFWCode() { FWCode = x.Key, sumqty = x.Count() });
            q = q.Where(x => x.sumqty > 1);
            switch (context.sorter.Field)
            {
                case "FWCode":
                    q = context.sorter.ASC ? q.OrderBy(x => x.FWCode) : q.OrderByDescending(x => x.FWCode);
                    break;
                case "sumqty":
                    q = context.sorter.ASC ? q.OrderBy(x => x.sumqty) : q.OrderByDescending(x => x.sumqty);
                    break;

                default:
                    q = q.OrderByDescending(x => x.sumqty);
                    break;
            }

            return Utils.ExecLTS(q.Skip(context.PageIndex * context.PageSize).Take(context.PageSize));

        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }



    }
}
