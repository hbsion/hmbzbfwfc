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
    public class brVender : IBrObject<vender>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string cu_no = "cu_no";
            public const string cu_name = "cu_name";
            public const string xtcu_no = "xtcu_no";
            public const string lc_no = "lc_no";
            public const string addr = "addr";
            public const string phone = "phone";
            public const string link_man = "link_man";
            public const string unitcode = "unitcode";

        }

        [Serializable]
        public class QueryContext
        {
            public string cu_no { get; set; }
            public string cu_name { get; set; }
            public string xtcu_no { get; set; }
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

        public brVender()
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

        void IBrObject<vender>.CheckAction(vender entity, ActionEnum action) { }

        public IEnumerable<vender> Querys(string incode)
        {

            IQueryable<vender> ret = db.vender;
            if (incode != null && incode.Trim().Length > 0)
                ret = ret.Where(x => x.cu_name.Contains(incode) || x.cu_no.Contains(incode));


            return ret.OrderBy(x => x.fid);
        }
        private IQueryable<vender> GetByFilter(IQueryable<vender> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.cu_no:
                    return q.Where(x => x.cu_no.StartsWith(filter.SearchText));

                case FieldType.lc_no:
                    return q.Where(x => x.lc_no.StartsWith(filter.SearchText));

                case FieldType.cu_name:
                    return q.Where(x => x.cu_name.Contains(filter.SearchText));

                case FieldType.addr:
                    return q.Where(x => x.addr.Contains(filter.SearchText));

                case FieldType.phone:
                    return q.Where(x => x.phone.Contains(filter.SearchText));

                case FieldType.link_man:
                    return q.Where(x => x.link_man.Contains(filter.SearchText));

                case FieldType.xtcu_no:
                    return q.Where(x => x.xtcu_no == filter.SearchText);

                case FieldType.unitcode:
                    return q.Where(x => x.unitcode == filter.SearchText);

                default:
                    return q;
            }
        }

        private IQueryable<vender> GetBySorter(IQueryable<vender> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.cu_no:
                    return sorter.ASC ? q.OrderBy(x => x.cu_no) : q.OrderByDescending(x => x.cu_no);
                case FieldType.lc_no:
                    return sorter.ASC ? q.OrderBy(x => x.lc_no) : q.OrderByDescending(x => x.lc_no);


                case FieldType.cu_name:
                    return sorter.ASC ? q.OrderBy(x => x.cu_name) : q.OrderByDescending(x => x.cu_name);


                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<vender> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.vender, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.vender, filter).Count();
        }

        public vender Retrieve(string inCode)
        {
            return db.vender.FirstOrDefault(x => x.cu_no == inCode);
        }


        public vender Retrieve(int inCode)
        {
            return db.vender.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<vender> GetAll(QueryContext context)
        {
            IQueryable<vender> ret = db.vender;

            if (context.cu_no != null && context.cu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.StartsWith(context.cu_no));

            if (context.lc_no != null && context.lc_no.Trim().Length > 0)
                ret = ret.Where(x => x.lc_no.StartsWith(context.lc_no));

            if (context.cu_name != null && context.cu_name.Trim().Length > 0)
                ret = ret.Where(x => x.cu_name.Contains(context.cu_name));

            if (context.addr != null && context.addr.Trim().Length > 0)
                ret = ret.Where(x => x.addr.Contains(context.addr));

            if (context.phone != null && context.phone.Trim().Length > 0)
                ret = ret.Where(x => x.phone.Contains(context.phone));

            if (context.link_man != null && context.link_man.Trim().Length > 0)
                ret = ret.Where(x => x.link_man.Contains(context.link_man));

            if (context.province != null && context.province.Trim().Length > 0)
                ret = ret.Where(x => x.province.Contains(context.province));      

            if (context.xtcu_no != null )
                ret = ret.Where(x => x.xtcu_no == context.xtcu_no);

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);

            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.Contains(context.seartext) || x.cu_name.Contains(context.seartext) || x.province.Contains(context.seartext) || x.city.Contains(context.seartext));   


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<vender> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<vender> Query()  //返回全部
        {

            IQueryable<vender> ret = db.vender;

            return ret.OrderBy(x => x.cu_no);
        }

        public IEnumerable<vender> Query(string inCu_no)  //返回下级供应商
        {

            IQueryable<vender> ret = db.vender;

            if (inCu_no == null || inCu_no.Length == 0)
                ret = ret.Where(x => x.xtcu_no == "");
            else
                ret = ret.Where(x => x.xtcu_no == inCu_no);

            ret = ret.OrderBy(x => x.cu_no);
            return ret;
        }

        public IEnumerable<vender> Query(string inCu_no, string inunitcode)  //返回下级供应商
        {

            IQueryable<vender> ret = db.vender;

            if (inCu_no == null || inCu_no.Length == 0)
                ret = ret.Where(x => x.xtcu_no == "");
            else
                ret = ret.Where(x => x.xtcu_no == inCu_no);

            if (inunitcode != null && inunitcode.Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

            ret = ret.OrderBy(x => x.cu_no);
            return ret;
        }


        public IEnumerable<vender> Query(string inCu_no, string inloca,string inunitcode)  //返回下级供应商
        {

            IQueryable<vender> ret = db.vender;

            if (inCu_no == null || inCu_no.Length == 0)
                ret = ret.Where(x => x.xtcu_no == "");
            else
                ret = ret.Where(x => x.xtcu_no == inCu_no);

            if (inloca != null && inloca.Length > 0)
                ret = ret.Where(x => x.province == inloca);

            if (inunitcode != null && inunitcode.Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

            ret = ret.OrderBy(x => x.cu_no);
            return ret;
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(vender entity)
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
                    if (db.vender.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.vender.InsertOnSubmit(entity);
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

        public void Update(vender entity)
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
                    vender dbdata = db.vender.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<vender>(entity, dbdata);
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
                    vender dbdata = db.vender.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.vender.DeleteOnSubmit(dbdata);
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

        public void Delete(vender entity)
        {
            this.Delete(entity.fid);
        }

        public vender NewEntity()
        {
            return new vender();
        }


        public bool CheckLogin(string inUserID, string inPassword,string inunitcode, out vender user)
        {
            if (db != null)
            {

                user = db.vender.FirstOrDefault(u => u.cu_no == inUserID && u.passwd == inPassword && u.unitcode==inunitcode);

                return user != null;
            }
            else
                throw new ObjectDisposedException("db");
        }

        public void ChangPasswd(string inunitcode,string inUserbm, string inNewPwd)
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

                    vender User = db.vender.FirstOrDefault(b => b.unitcode == inunitcode && b.cu_no == inUserbm);
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
