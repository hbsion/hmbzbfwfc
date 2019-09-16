using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brCustomer : IBrObject<customer>
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

        public brCustomer()
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

        void IBrObject<customer>.CheckAction(customer entity, ActionEnum action) { }

        private IQueryable<customer> GetByFilter(IQueryable<customer> q, Filter filter)
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

        private IQueryable<customer> GetBySorter(IQueryable<customer> q, Sorter sorter)
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

        public IEnumerable<customer> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.customer, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.customer, filter).Count();
        }

        public customer Retrieve(string inCode)
        {
            return db.customer.FirstOrDefault(x => x.cu_no == inCode);
        }

        public customer Retrieve(string inCode, string inunitcode)
        {
            return db.customer.FirstOrDefault(x => x.cu_no == inCode && x.unitcode == inunitcode);
        }


        public IEnumerable<customer> QueryS(string incu_no, string inscode, string inmtype, string inunitcode)  //返回
        {

            IQueryable<customer> ret = db.customer;

            if (inunitcode != null && inunitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

            if (incu_no != null && incu_no.Trim().Length > 0)
                ret = ret.Where(x => x.xtcu_no == incu_no);


            if (inscode != null && inscode.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.Contains(inunitcode) || x.cu_name.Contains(inunitcode) || x.addr.Contains(inunitcode));


            return ret.OrderBy(x => x.cu_no);
        }


        public bool addrbool(string cu_no, string inCode, string inunitcode)
        {
            customer cust = db.customer.FirstOrDefault(x => x.addr.Contains(inCode) && x.cu_no != cu_no&&x.unitcode==inunitcode);
            if (cust == null)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }

        public customer Retrieve(int inCode)
        {
            return db.customer.FirstOrDefault(x => x.fid == inCode);
        }

        private IQueryable<customer> GetAll(QueryContext context)
        {
            IQueryable<customer> ret = db.customer;

            if (context.cu_no != null && context.cu_no.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.StartsWith(context.cu_no));

  
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

            if (context.xtcu_no != null && context.xtcu_no.Length>0)
                ret = ret.Where(x => x.xtcu_no == context.xtcu_no);

 
            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.cu_no.Contains(context.seartext) || x.cu_name.Contains(context.seartext) || x.province.Contains(context.seartext) || x.city.Contains(context.seartext));


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<customer> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<customer> Query()  //返回全部
        {

            IQueryable<customer> ret = db.customer;

            return ret.OrderBy(x => x.cu_no);
        }

        public IEnumerable<customer> Query(string inCu_no)  //返回下级客户
        {

            IQueryable<customer> ret = db.customer;

            if (inCu_no == null || inCu_no.Length == 0)
                ret = ret.Where(x => x.xtcu_no == "");
            else
                ret = ret.Where(x => x.xtcu_no == inCu_no);

            ret = ret.OrderBy(x => x.cu_no);
            return ret;
        }

        public IEnumerable<customer> Query(string inCu_no, string inunitcode)  //返回下级客户
        {

            IQueryable<customer> ret = db.customer;

            if (inCu_no == null || inCu_no.Length == 0)
                ret = ret.Where(x => x.xtcu_no == "");
            else
                ret = ret.Where(x => x.xtcu_no == inCu_no);

            if (inunitcode != null && inunitcode.Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

            ret = ret.OrderBy(x => x.cu_no);
            return ret;
        }


        public IEnumerable<customer> Query(string inCu_no, string inloca, string inunitcode)  //返回下级客户
        {

            IQueryable<customer> ret = db.customer;

            if (inCu_no == null || inCu_no.Length == 0)
                ret = ret.Where(x => x.xtcu_no == "");
            else
                ret = ret.Where(x => x.xtcu_no == inCu_no);

            if (inloca != null && inloca.Length > 0)
                ret = ret.Where(x => x.province == inloca);

  

            ret = ret.OrderBy(x => x.cu_no);
            return ret;
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(customer entity)
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
                    if (db.customer.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.customer.InsertOnSubmit(entity);
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

        public void Update(customer entity)
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
                    customer dbdata = db.customer.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<customer>(entity, dbdata);
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
                    customer dbdata = db.customer.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.customer.DeleteOnSubmit(dbdata);
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

        public void Delete(customer entity)
        {
            this.Delete(entity.fid);
        }

        public customer NewEntity()
        {
            return new customer();
        }


        public bool CheckLogin(string inUserID, string inPassword, string inunitcode, out customer user)
        {

            string passwd = CMD5(inPassword);


            if (db != null)
            {

                user = db.customer.FirstOrDefault(u => u.cu_no == inUserID && u.passwd == passwd );

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

                    customer User = db.customer.FirstOrDefault(b => b.unitcode == inunitcode && b.cu_no == inUserbm);
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

        public static string CMD5(string strSource, string sEncode = "UTF-8")
        {
            //new
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //获取密文字节数组
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.GetEncoding(sEncode).GetBytes(strSource));

            //转换成字符串，并取9到25位
            //string strResult = BitConverter.ToString(bytResult, 4, 8); 
            //转换成字符串，32位

            string strResult = BitConverter.ToString(bytResult);

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉
            strResult = strResult.Replace("-", "");

            return strResult.ToLower();
        }

    }
}
