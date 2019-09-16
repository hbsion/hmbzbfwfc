using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brBasoneto : IBrObject<bas_oneto>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string fucode = "fucode";
            public const string sendmess = "sendmess";

        }

        public class QueryContext
        {
            public string fucode { get; set; }
            public string sendmess { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brBasoneto()
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

        void IBrObject<bas_oneto>.CheckAction(bas_oneto entity, ActionEnum action) { }

        private IQueryable<bas_oneto> GetByFilter(IQueryable<bas_oneto> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.fucode:
                    return q.Where(x => x.fucode.StartsWith(filter.SearchText));
                case FieldType.sendmess:
                    return q.Where(x => x.sendmess.Contains(filter.SearchText));

                default:
                    return q;
            }
        }

        private IQueryable<bas_oneto> GetBySorter(IQueryable<bas_oneto> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.fucode:
                    return sorter.ASC ? q.OrderBy(x => x.fucode) : q.OrderByDescending(x => x.fucode);
                case FieldType.sendmess:
                    return sorter.ASC ? q.OrderBy(x => x.sendmess) : q.OrderByDescending(x => x.sendmess);
                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<bas_oneto> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.bas_oneto, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.bas_oneto, filter).Count();
        }

        public bas_oneto Retrieve(string inCode)
        {
            return db.bas_oneto.FirstOrDefault(x => x.fucode == inCode);
        }

        public bas_oneto Retrieve(int inCode)
        {
            return db.bas_oneto.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<bas_oneto> GetAll(QueryContext context)
        {
            IQueryable<bas_oneto> ret = db.bas_oneto;

            if (context.fucode != null && context.fucode.Trim().Length > 0)
                ret = ret.Where(x => x.fucode.StartsWith(context.fucode));
            if (context.sendmess != null && context.sendmess.Trim().Length > 0)
                ret = ret.Where(x => x.sendmess.Contains(context.sendmess));

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<bas_oneto> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<bas_oneto> Query()  //返回全部
        {

            IQueryable<bas_oneto> ret = db.bas_oneto;


            return ret.OrderBy(x => x.fucode);
        }



        public IEnumerable<bas_oneto> FindAllbas_oneto()
        {
            return db.bas_oneto;
        }



        public  string  resendmess(string inCode)
        {
            bas_oneto basenoe = db.bas_oneto.FirstOrDefault(x => x.fucode == inCode);

            if (basenoe != null)
                return basenoe.sendmess;
            else
                return "";

        }




        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(bas_oneto entity)
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
                    if (db.bas_oneto.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.bas_oneto.InsertOnSubmit(entity);
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

        public void Update(bas_oneto entity)
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
                    bas_oneto dbdata = db.bas_oneto.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<bas_oneto>(entity, dbdata);
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
                    bas_oneto dbdata = db.bas_oneto.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.bas_oneto.DeleteOnSubmit(dbdata);
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

        public void Delete(bas_oneto entity)
        {
            this.Delete(entity.fid);
        }

        public bas_oneto NewEntity()
        {
            return new bas_oneto();
        }


    }
}
