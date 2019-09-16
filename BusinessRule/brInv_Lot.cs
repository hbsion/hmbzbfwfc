using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using System.Data;

namespace marr.BusinessRule
{
    [Serializable]
    public class brInv_Lot : IBrObject<Inv_Lot>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string lot_no = "lot_no";
            public const string p_no = "p_no";
            public const string pname = "pname";
            public const string makeare = "makeare";
            public const string unitcode = "unitcode";

        }

        [Serializable]
        public class QueryContext
        {
            public string lot_no { get; set; }
            public string p_no { get; set; }
            public string pname { get; set; }
            public string unitcode { get; set; }
            public string makeare { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public string seartext { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brInv_Lot()
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

        void IBrObject<Inv_Lot>.CheckAction(Inv_Lot entity, ActionEnum action) { }

        private IQueryable<Inv_Lot> GetByFilter(IQueryable<Inv_Lot> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.lot_no:
                    return q.Where(x => x.lot_no.StartsWith(filter.SearchText));
                case FieldType.p_no:
                    return q.Where(x => x.p_no.Equals(filter.SearchText));
                case FieldType.pname:
                    return q.Where(x => x.pname.Contains(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode == filter.SearchText);
                case FieldType.makeare:
                    return q.Where(x => x.makeare == filter.SearchText);
                default:
                    return q;
            }
        }

        private IQueryable<Inv_Lot> GetBySorter(IQueryable<Inv_Lot> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.lot_no:
                    return sorter.ASC ? q.OrderBy(x => x.p_no) : q.OrderByDescending(x => x.p_no);
                case FieldType.p_no:
                    return sorter.ASC ? q.OrderBy(x => x.pname) : q.OrderByDescending(x => x.pname);
                case FieldType.pname:
                    return sorter.ASC ? q.OrderBy(x => x.type) : q.OrderByDescending(x => x.type);
                case FieldType.makeare:
                    return sorter.ASC ? q.OrderBy(x => x.makeare) : q.OrderByDescending(x => x.makeare);
                default:
                    return q.OrderBy(x => x.p_no);
            }
        }

        public IEnumerable<Inv_Lot> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.Inv_Lot, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.Inv_Lot, filter).Count();
        }

        public Inv_Lot Retrieve(string inCode)
        {
            return db.Inv_Lot.FirstOrDefault(x => x.lot_no == inCode);
        }

        public Inv_Lot Retrieve(string inCode, string inunitcode)
        {
            return db.Inv_Lot.FirstOrDefault(x => x.lot_no == inCode && x.unitcode == inunitcode);
        }


        public IEnumerable<Inv_Lot> Querys(string scode)  //返回全部产品
        {

            IQueryable<Inv_Lot> ret = db.Inv_Lot;

            if (scode != null && scode.Trim() != "")
                ret = ret.Where(x => x.p_no.Contains(scode) || x.pname.Contains(scode) || x.lot_no.Contains(scode) );


            return ret.OrderBy(x => x.p_no);
        }

        public IEnumerable<inv_lotb> Querylotb(string  inlot_no)  //返回混合批次
        {

            IQueryable<inv_lotb> ret = db.inv_lotb;

            if (inlot_no != null && inlot_no.Trim() != "")
                ret = ret.Where(x => x.lot_no == inlot_no);


            return ret.OrderBy(x => x.hhlot_no);
        }



        public Inv_Lot Retrieve(int inCode)
        {
            return db.Inv_Lot.FirstOrDefault(x => x.fid == inCode);
        }

        public Inv_Lot Rallpart(string inunitcode)
        {
            return db.Inv_Lot.FirstOrDefault(x => x.unitcode == inunitcode);
        }

        private IQueryable<Inv_Lot> GetAll(QueryContext context)
        {
            IQueryable<Inv_Lot> ret = db.Inv_Lot;

            if (context.lot_no != null && context.lot_no.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no.StartsWith(context.lot_no));
            if (context.p_no != null && context.p_no.Trim().Length > 0)
                ret = ret.Where(x => x.p_no.Contains(context.p_no));
            if (context.pname != null && context.pname.Trim().Length > 0)
                ret = ret.Where(x => x.pname.Contains(context.pname));
            if (context.makeare != null && context.makeare.Trim().Length > 0)
                ret = ret.Where(x => x.makeare.Contains(context.makeare));
            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);
        
            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no.Contains(context.seartext) || x.pname.Contains(context.seartext) || x.p_no.Contains(context.seartext) || x.type.Contains(context.seartext));


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<Inv_Lot> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<Inv_Lot> Query()  //返回全部产品
        {

            IQueryable<Inv_Lot> ret = db.Inv_Lot;

            return ret.OrderBy(x => x.p_no);
        }


        public IEnumerable<Inv_Lot> Query(string inunitcode)  //返回xxx产品
        {

            IQueryable<Inv_Lot> ret = db.Inv_Lot;

            if (inunitcode != null && inunitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderBy(x => x.p_no);
        }


        public IEnumerable<Inv_Lot> Query(string inp_no,string inlot_no)  //返回xxx产品
        {

            IQueryable<Inv_Lot> ret = db.Inv_Lot;

            if (inp_no != null && inp_no.Trim().Length > 0)
                ret = ret.Where(x => x.p_no == inp_no);


            if (inlot_no != null && inlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no == inlot_no);

            return ret.OrderBy(x => x.p_no);
        }


        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(Inv_Lot entity)
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
                    if (db.Inv_Lot.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.Inv_Lot.InsertOnSubmit(entity);
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

        public void Update(Inv_Lot entity)
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
                    Inv_Lot dbdata = db.Inv_Lot.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<Inv_Lot>(entity, dbdata);
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
                    Inv_Lot dbdata = db.Inv_Lot.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.Inv_Lot.DeleteOnSubmit(dbdata);
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

        public void Delete(Inv_Lot entity)
        {
            this.Delete(entity.fid);
        }

        public Inv_Lot NewEntity()
        {
            return new Inv_Lot();
        }
    }

}
