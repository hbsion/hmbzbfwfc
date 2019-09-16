using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    [Serializable]
    public class brInvMpart : IBrObject<Inv_Mpart>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string ProdNO = "p_no";
            public const string ProdName = "pname";
            public const string Prodtype = "type";
            public const string unitcode = "unitcode";

        }

        [Serializable]
        public class QueryContext
        {
            public string ProdNO { get; set; }
            public string ProdName { get; set; }
            public string Prodtype { get; set; }
            public string unitcode { get; set; }

            public string seartext { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brInvMpart()
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

        void IBrObject<Inv_Mpart>.CheckAction(Inv_Mpart entity, ActionEnum action) { }

        private IQueryable<Inv_Mpart> GetByFilter(IQueryable<Inv_Mpart> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.ProdNO:
                    return q.Where(x => x.p_no.StartsWith(filter.SearchText));
                case FieldType.ProdName:
                    return q.Where(x => x.pname.Contains(filter.SearchText));
                case FieldType.Prodtype:
                    return q.Where(x => x.type.Contains(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode ==filter.SearchText);
                default:
                    return q;
            }
        }

        private IQueryable<Inv_Mpart> GetBySorter(IQueryable<Inv_Mpart> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.ProdNO:
                    return sorter.ASC ? q.OrderBy(x => x.p_no) : q.OrderByDescending(x => x.p_no);
                case FieldType.ProdName:
                    return sorter.ASC ? q.OrderBy(x => x.pname) : q.OrderByDescending(x => x.pname);
                case FieldType.Prodtype:
                    return sorter.ASC ? q.OrderBy(x => x.type) : q.OrderByDescending(x => x.type);
            
                default:
                    return q.OrderBy(x => x.p_no);
            }
        }

        public IEnumerable<Inv_Mpart> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.Inv_Mpart, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.Inv_Mpart, filter).Count();
        }

        public Inv_Mpart Retrieve(string inCode)
        {
            return db.Inv_Mpart.FirstOrDefault(x => x.p_no == inCode);
        }

        public Inv_Mpart Retrieve(string inCode,string inunitcode)
        {
            return db.Inv_Mpart.FirstOrDefault(x => x.p_no == inCode && x.unitcode == inunitcode);
        }


        public Inv_Mpart Retrieve(int inCode )
        {
            return db.Inv_Mpart.FirstOrDefault(x => x.fid == inCode );
        }

        public Inv_Mpart Rallpart(string inunitcode)
        {
            return db.Inv_Mpart.FirstOrDefault(x =>  x.unitcode==inunitcode);
        }

        private IQueryable<Inv_Mpart> GetAll(QueryContext context)
        {
            IQueryable<Inv_Mpart> ret = db.Inv_Mpart;

            if (context.ProdNO != null && context.ProdNO.Trim().Length > 0)
                ret = ret.Where(x => x.p_no.StartsWith(context.ProdNO));
            if (context.ProdName != null && context.ProdName.Trim().Length > 0)
                ret = ret.Where(x => x.pname.Contains(context.ProdName));
            if (context.Prodtype != null && context.Prodtype.Trim().Length > 0)
                ret = ret.Where(x => x.type.Contains(context.Prodtype));

            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.pname.Contains(context.seartext) || x.p_no.Contains(context.seartext) || x.type.Contains(context.seartext));


            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);           
            
             ret = GetBySorter(ret, context.sorter);

              return ret;
        }

        public IEnumerable<Inv_Mpart> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<Inv_Mpart> Query()  //返回全部原物料
        {

            IQueryable<Inv_Mpart> ret = db.Inv_Mpart;

            return ret.OrderBy(x => x.p_no);
        }


        public IEnumerable<Inv_Mpart> Query(string inunitcode)  //返回xxx原物料
        {

            IQueryable<Inv_Mpart> ret = db.Inv_Mpart;

            if (inunitcode != null && inunitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);     

            return ret.OrderBy(x => x.p_no);
        }


        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(Inv_Mpart entity)
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
                    if (db.Inv_Mpart.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.Inv_Mpart.InsertOnSubmit(entity);
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

        public void Update(Inv_Mpart entity)
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
                    Inv_Mpart dbdata = db.Inv_Mpart.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<Inv_Mpart>(entity, dbdata);
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
                    Inv_Mpart dbdata = db.Inv_Mpart.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.Inv_Mpart.DeleteOnSubmit(dbdata);
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

        public void Delete(Inv_Mpart entity)
        {
            this.Delete(entity.fid);
        }

        public Inv_Mpart NewEntity()
        {
            return new Inv_Mpart();
        }
    }
}
