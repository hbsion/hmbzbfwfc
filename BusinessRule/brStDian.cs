using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brStDian : IBrObject<st_dian>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string dian_no = "dian_no";
            public const string dian_name = "dian_name";
            public const string xtdian_no = "xtdian_no";
            public const string lc_no = "lc_no";
            public const string addr = "addr";
            public const string phone = "phone";
            public const string link_man = "link_man";
            public const string unitcode = "unitcode";

        }

        public class QueryContext
        {
            public string dian_no { get; set; }
            public string dian_name { get; set; }
            public string xtdian_no { get; set; }
            public string lc_no { get; set; }
            public string addr { get; set; }
            public string phone { get; set; }
            public string link_man { get; set; }
            public string unitcode { get; set; }

            public string province { get; set; }
            public string seartext { get; set; }

            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brStDian()
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

        void IBrObject<st_dian>.CheckAction(st_dian entity, ActionEnum action) { }

        private IQueryable<st_dian> GetByFilter(IQueryable<st_dian> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.dian_no:
                    return q.Where(x => x.dian_no.StartsWith(filter.SearchText));



                case FieldType.dian_name:
                    return q.Where(x => x.dian_name.Contains(filter.SearchText));

                case FieldType.addr:
                    return q.Where(x => x.addr.Contains(filter.SearchText));

                case FieldType.phone:
                    return q.Where(x => x.phone.Contains(filter.SearchText));

                case FieldType.link_man:
                    return q.Where(x => x.link_man.Contains(filter.SearchText));


                case FieldType.unitcode:
                    return q.Where(x => x.unitcode == filter.SearchText);

                default:
                    return q;
            }
        }

        private IQueryable<st_dian> GetBySorter(IQueryable<st_dian> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.dian_no:
                    return sorter.ASC ? q.OrderBy(x => x.dian_no) : q.OrderByDescending(x => x.dian_no);


                case FieldType.dian_name:
                    return sorter.ASC ? q.OrderBy(x => x.dian_name) : q.OrderByDescending(x => x.dian_name);


                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<st_dian> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.st_dian, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.st_dian, filter).Count();
        }

        public st_dian Retrieve(string inCode)
        {
            return db.st_dian.FirstOrDefault(x => x.dian_no == inCode);
        }

        public st_dian Retrieve(string inCode, string inunitcode)
        {
            return db.st_dian.FirstOrDefault(x => x.dian_no == inCode && x.unitcode == inunitcode);
        }


        public IEnumerable<st_dian> QueryS(string indian_no, string inscode, string inmtype, string inunitcode)  //返回
        {

            IQueryable<st_dian> ret = db.st_dian;

            if (inunitcode != null && inunitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);


            if (inscode != null && inscode.Trim().Length > 0)
                ret = ret.Where(x => x.dian_no.Contains(inunitcode) || x.dian_name.Contains(inunitcode) || x.addr.Contains(inunitcode));


            return ret.OrderBy(x => x.dian_no);
        }




        public st_dian Retrieve(int inCode)
        {
            return db.st_dian.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<st_dian> GetAll(QueryContext context)
        {
            IQueryable<st_dian> ret = db.st_dian;

            if (context.dian_no != null && context.dian_no.Trim().Length > 0)
                ret = ret.Where(x => x.dian_no.StartsWith(context.dian_no));



            if (context.dian_name != null && context.dian_name.Trim().Length > 0)
                ret = ret.Where(x => x.dian_name.Contains(context.dian_name));

            if (context.addr != null && context.addr.Trim().Length > 0)
                ret = ret.Where(x => x.addr.Contains(context.addr));

            if (context.phone != null && context.phone.Trim().Length > 0)
                ret = ret.Where(x => x.phone.Contains(context.phone));

            if (context.link_man != null && context.link_man.Trim().Length > 0)
                ret = ret.Where(x => x.link_man.Contains(context.link_man));

            if (context.province != null && context.province.Trim().Length > 0)
                ret = ret.Where(x => x.province.Contains(context.province));


            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);

            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.dian_no.Contains(context.seartext) || x.dian_name.Contains(context.seartext) || x.province.Contains(context.seartext) || x.city.Contains(context.seartext));


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<st_dian> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<st_dian> Query()  //返回全部
        {

            IQueryable<st_dian> ret = db.st_dian;

            return ret.OrderBy(x => x.dian_no);
        }






        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(st_dian entity)
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
                    if (db.st_dian.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.st_dian.InsertOnSubmit(entity);
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

        public void Update(st_dian entity)
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
                    st_dian dbdata = db.st_dian.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<st_dian>(entity, dbdata);
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
                    st_dian dbdata = db.st_dian.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.st_dian.DeleteOnSubmit(dbdata);
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

        public void Delete(st_dian entity)
        {
            this.Delete(entity.fid);
        }

        public st_dian NewEntity()
        {
            return new st_dian();
        }


        public bool CheckLogin(string inUserID, string inPassword, string inunitcode, out st_dian user)
        {
            if (db != null)
            {

                user = db.st_dian.FirstOrDefault(u => u.dian_no == inUserID && u.passwd == inPassword && u.unitcode == inunitcode);

                return user != null;
            }
            else
                throw new ObjectDisposedException("db");
        }

        public void ChangPasswd(string inunitcode, string inUserbm, string inNewPwd)
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

                    st_dian User = db.st_dian.FirstOrDefault(b => b.unitcode == inunitcode && b.dian_no == inUserbm);
                    User.passwd = inNewPwd;

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

    }
}
