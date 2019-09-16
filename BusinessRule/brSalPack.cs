using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brSalPack : IBrObject<sal_packa>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string pack_no = "pack_no";
            public const string pfilename = "pfilename";


        }

        public class QueryContext
        {
            public string pack_no { get; set; }
            public string pfilename { get; set; }
            public DateTime? datef { get; set; }
            public DateTime? datet { get; set; }
            public string cu_no { get; set; }

            public string seartext { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }
        public brSalPack()
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

        void IBrObject<sal_packa>.CheckAction(sal_packa entity, ActionEnum action) { }

        private IQueryable<sal_packa> GetByFilter(IQueryable<sal_packa> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.pack_no:
                    return q.Where(x => x.pack_no.StartsWith(filter.SearchText));
                case FieldType.pfilename:
                    return q.Where(x => x.pfilename.Contains(filter.SearchText));



          

                default:
                    return q;
            }
        }

        private IQueryable<sal_packa> GetBySorter(IQueryable<sal_packa> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.pack_no:
                    return sorter.ASC ? q.OrderBy(x => x.pack_no) : q.OrderByDescending(x => x.pack_no);
                case FieldType.pfilename:
                    return sorter.ASC ? q.OrderBy(x => x.pfilename) : q.OrderByDescending(x => x.pfilename);


                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<sal_packa> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.sal_packa, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.sal_packa, filter).Count();
        }


        public sal_packa Retrieve(string inCode)
        {
            return db.sal_packa.FirstOrDefault(x => x.pack_no == inCode);
        }

        public sal_packb Retrieveb(string inCode)
        {
            return db.sal_packb.FirstOrDefault(x => x.snno == inCode);
        }
   

        public sal_packa Retrieve(int infid)
        {
            return db.sal_packa.FirstOrDefault(x => x.fid == infid);
        }


        public IEnumerable<sal_packa> QueryS(string inCu_no, string inscode)   
        {

            IQueryable<sal_packa> ret = db.sal_packa.Where(x => x.type == "P" || x.type == "p"); ;

            if (inCu_no != null && inCu_no.Length > 0)
            {
                ret = ret.Where(x => x.cu_no == inCu_no);
            }


            if (inscode != null && inscode.Length >= 0)
                ret = ret.Where(x => x.cu_no.Contains(inscode) || x.pfilename.Contains(inscode));

            ret = ret.OrderByDescending(x => x.fid);

            return ret;
        }


        private IQueryable<sal_packa> GetAll(QueryContext context)
        {
            IQueryable<sal_packa> ret = db.sal_packa;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.pack_date >= context.datef);

            if (context.datet.HasValue)
                ret = ret.Where(x => x.pack_date < context.datet.Value.AddDays(1));


            if (context.pack_no != null && context.pack_no.Trim().Length > 0)
                ret = ret.Where(x => x.pack_no.StartsWith(context.pack_no));


            if (context.cu_no != null && context.cu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no==context.cu_no);


            if (context.pfilename != null && context.pfilename.Trim().Length > 0)
                ret = ret.Where(x => x.pfilename.Contains(context.pfilename));

            if (context.seartext != null && context.seartext.Length >= 0)
                ret = ret.Where(x => x.cu_no.Contains(context.seartext) || x.pfilename.Contains(context.seartext));



            ret = ret.OrderByDescending(x => x.fid);

      
            return ret;
        }

        public IEnumerable<sal_packa> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<sal_packa> Query()  //返回全部
        {

            IQueryable<sal_packa> ret = db.sal_packa;

            return ret.OrderBy(x => x.pack_no);
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(sal_packa entity)
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
                    if (db.sal_packa.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.sal_packa.InsertOnSubmit(entity);
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

        public void Update(sal_packa entity)
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
                    sal_packa dbdata = db.sal_packa.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<sal_packa>(entity, dbdata);
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
                    sal_packa dbdata = db.sal_packa.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.sal_packa.DeleteOnSubmit(dbdata);
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

        public void Delete(sal_packa entity)
        {
            this.Delete(entity.fid);
        }

        public sal_packa NewEntity()
        {
            return new sal_packa();
        }
    }
}
