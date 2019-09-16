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
    public class brhydian : IBrObject<hy_dian>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string hy_no = "hy_no";
            public const string hy_cn = "hy_cn";
            public const string mobile = "mobile";
            public const string password = "password";
            public const string email = "email";
            public const string jf = "jf";
            public const string historyjf = "historyjf";
            public const string realname = "realname";
            public const string cu_no = "cu_no";
            public const string cu_name = "cu_name";
            public const string bankaccount = "bankaccount";
            public const string khh = "khh";
            public const string in_date = "in_date";
            public const string loca = "loca";
            public const string statu = "statu";
            public const string remarks = "remarks";
            public const string birthday = "birthday";
            public const string postcode = "postcode";
            public const string address = "address";
            public const string sex = "sex";
            public const string city = "city";

        }
        [Serializable]
        public class QueryContext
        {
            public string hy_no { get; set; }
            public string hy_cn { get; set; }
            public string mobile { get; set; }
            public string password { get; set; }
            public string email { get; set; }
            public string jf { get; set; }
            public string historyjf { get; set; }
            public string realname { get; set; }
            public string cu_no { get; set; }
            public string cu_name { get; set; }
            public string bankaccount { get; set; }
            public string khh { get; set; }
            public string in_date { get; set; }
            public string loca { get; set; }
            public int  statu { get; set; }
            public string remarks { get; set; }
            public string birthday { get; set; }
            public string postcode { get; set; }
            public string address { get; set; }
            public string sex { get; set; }
            public string city { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brhydian()
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

        void IBrObject<hy_dian>.CheckAction(hy_dian entity, ActionEnum action) { }
        public bool CheckLogin(string inUserID, string inPassword, out hy_dian user)
        {
            if (db != null)
            {

                user = db.hy_dian.FirstOrDefault(u => u.hy_no == inUserID && u.password == inPassword);

                return user != null;
            }
            else
                throw new ObjectDisposedException("db");
        }
        private IQueryable<hy_dian> GetByFilter(IQueryable<hy_dian> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.hy_no:
                    if (filter.SearchText != null && filter.SearchText.Trim().Length > 0)
                        return q.Where(x => x.hy_no == filter.SearchText || x.email == filter.SearchText);
                    else
                        return q;
                case FieldType.email:
                    return q.Where(x => x.email.StartsWith(filter.SearchText));
                case FieldType.mobile:
                    return q.Where(x => x.mobile.StartsWith(filter.SearchText));
                case FieldType.hy_cn:
                    return q.Where(x => x.hy_cn.StartsWith(filter.SearchText));
                case FieldType.password:
                    return q.Where(x => x.password.Contains(filter.SearchText));


                case FieldType.realname:
                    return q.Where(x => x.realname.Contains(filter.SearchText));
                case FieldType.cu_no:
                    return q.Where(x => x.cu_no.Contains(filter.SearchText));
                case FieldType.cu_name:
                    return q.Where(x => x.cu_name.Contains(filter.SearchText));

                case FieldType.khh:
                    return q.Where(x => x.khh.Contains(filter.SearchText));

                case FieldType.loca:
                    return q.Where(x => x.loca.Contains(filter.SearchText));

                case FieldType.remarks:
                    return q.Where(x => x.remarks.Contains(filter.SearchText));

                case FieldType.postcode:
                    return q.Where(x => x.postcode.Contains(filter.SearchText));
                case FieldType.address:
                    return q.Where(x => x.address.Contains(filter.SearchText));
                case FieldType.sex:
                    return q.Where(x => x.sex.Contains(filter.SearchText));

                default:
                    return q;
            }
        }

        private IQueryable<hy_dian> GetBySorter(IQueryable<hy_dian> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.hy_no:
                    return sorter.ASC ? q.OrderBy(x => x.hy_no) : q.OrderByDescending(x => x.hy_no);
                case FieldType.email:
                    return sorter.ASC ? q.OrderBy(x => x.email) : q.OrderByDescending(x => x.email);
                case FieldType.mobile:
                    return sorter.ASC ? q.OrderBy(x => x.mobile) : q.OrderByDescending(x => x.mobile);
                case FieldType.password:
                    return sorter.ASC ? q.OrderBy(x => x.password) : q.OrderByDescending(x => x.password);
                case FieldType.hy_cn:
                    return sorter.ASC ? q.OrderBy(x => x.hy_cn) : q.OrderByDescending(x => x.hy_cn);
                case FieldType.jf:
                    return sorter.ASC ? q.OrderBy(x => x.jf) : q.OrderByDescending(x => x.jf);
                case FieldType.historyjf:
                    return sorter.ASC ? q.OrderBy(x => x.historyjf) : q.OrderByDescending(x => x.historyjf);
                case FieldType.in_date:
                    return sorter.ASC ? q.OrderBy(x => x.in_date) : q.OrderByDescending(x => x.in_date);
                case FieldType.loca:
                    return sorter.ASC ? q.OrderBy(x => x.loca) : q.OrderByDescending(x => x.loca);
                case FieldType.khh:
                    return sorter.ASC ? q.OrderBy(x => x.khh) : q.OrderByDescending(x => x.khh);

                case FieldType.postcode:
                    return sorter.ASC ? q.OrderBy(x => x.postcode) : q.OrderByDescending(x => x.postcode);
                case FieldType.realname:
                    return sorter.ASC ? q.OrderBy(x => x.realname) : q.OrderByDescending(x => x.realname);
                case FieldType.remarks:
                    return sorter.ASC ? q.OrderBy(x => x.remarks) : q.OrderByDescending(x => x.remarks);
                case FieldType.statu:
                    return sorter.ASC ? q.OrderBy(x => x.statu) : q.OrderByDescending(x => x.statu);
                case FieldType.sex:
                    return sorter.ASC ? q.OrderBy(x => x.sex) : q.OrderByDescending(x => x.sex);
                case FieldType.address:
                    return sorter.ASC ? q.OrderBy(x => x.address) : q.OrderByDescending(x => x.address);
                case FieldType.bankaccount:
                    return sorter.ASC ? q.OrderBy(x => x.BankAccount) : q.OrderByDescending(x => x.BankAccount);
                case FieldType.birthday:
                    return sorter.ASC ? q.OrderBy(x => x.birthday) : q.OrderByDescending(x => x.birthday);
                case FieldType.cu_name:
                    return sorter.ASC ? q.OrderBy(x => x.cu_name) : q.OrderByDescending(x => x.cu_name);
                case FieldType.cu_no:
                    return sorter.ASC ? q.OrderBy(x => x.cu_no) : q.OrderByDescending(x => x.cu_no);
                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<hy_dian> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.hy_dian, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.hy_dian, filter).Count();
        }


        public hy_dian Retrieve(string inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.hy_no == inCode);
        }

        public hy_dian Retrieve(int inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.fid == inCode);
        }


        public hy_dian wx_Retrieve(string inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.wx_id == inCode);
        }

        public string[] GetAreaInfo(string Usn)
        {
            string[] info = new string[2];
            hy_dian man = db.hy_dian.FirstOrDefault(n => n.hy_no.Equals(Usn));
            info[0] = man.city; info[1] = man.province;
            return info;
        }

        public hy_dian Retrieve1(string inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.password == inCode);
        }

        private IQueryable<hy_dian> GetAll(QueryContext context)
        {
            IQueryable<hy_dian> ret = db.hy_dian;

            if (context.hy_no != null && context.hy_no.Trim().Length > 0)
                ret = ret.Where(x => x.hy_no.StartsWith(context.hy_no));

            if (context.email != null)
                ret = ret.Where(x => x.email == context.email);

            if (context.mobile != null && context.mobile.Trim().Length > 0)
                ret = ret.Where(x => x.mobile.StartsWith(context.mobile));

            if (context.password != null && context.password.Trim().Length > 0)
                ret = ret.Where(x => x.password == context.password);

            if (context.address != null && context.address.Trim().Length > 0)
                ret = ret.Where(x => x.address.StartsWith(context.address));

            if (context.statu != null && context.statu >= 0)
                ret = ret.Where(x => x.statu == context.statu);
            if (context.historyjf != null) {
                ret = ret.Where(x => x.historyjf >=Convert.ToDecimal(context.historyjf));
            }

            if (context.city != null && context.city.Trim().Length > 0)
                ret = ret.Where(x => x.city.Contains(context.city) || x.province.Contains(context.city));

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<hy_dian> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }



        public IEnumerable<hy_dian> GetSubItems(string inemail)
        {
            return db.hy_dian.Where(x => x.email == inemail);
        }

        public IEnumerable<hy_dian> Query()  //返回全部
        {

            IQueryable<hy_dian> ret = db.hy_dian;

            return ret.OrderBy(x => x.hy_no);
        }
        public hy_dian Retrieve2(string inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.province == inCode);
        }
        public hy_dian  ljjf(int inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.historyjf >= inCode);

            
            
        }
        public hy_dian Retrieve3(string inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.city == inCode);
        }
        public hy_dian Retrieve4(string inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.cu_name == inCode);
        }

        public hy_dian Retrievetel(string inCode)
        {
            return db.hy_dian.FirstOrDefault(x => x.mobile == inCode);
        }


        public bool Checkfwmm(string inUserID, string inPassword)
        {
            if (db != null)
            {

                hy_dian user = db.hy_dian.FirstOrDefault(u => u.hy_no == inUserID && u.password == inPassword);
                return user != null;

            }
            else
                throw new ObjectDisposedException("db");
        }





        public bool ChangJf(string inUserbm, decimal? injf, bool addyn, out hy_dian user)
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

                    user = db.hy_dian.FirstOrDefault(b => b.hy_no == inUserbm);
                    if (addyn)
                        user.jf += injf;
                    else
                        user.jf -= injf;

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    return user != null;

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


        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(hy_dian entity)
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
                    if (db.hy_dian.FirstOrDefault(x => x.hy_no == entity.hy_no) == null)
                    {
                        db.hy_dian.InsertOnSubmit(entity);//添加数据
                        db.SubmitChanges();//保存数据
                    }
                    db.Transaction.Commit();//事务结束
                }
                catch
                {
                    if (db.Transaction != null) db.Transaction.Rollback();//回滚事务
                    throw;
                }
                finally
                {
                    db.Transaction = null;
                    if (closeConn) db.Connection.Close();
                }
            }
        }

        public void Update(hy_dian entity)
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
                    hy_dian dbdata = db.hy_dian.FirstOrDefault(x => x.hy_no == entity.hy_no);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<hy_dian>(entity, dbdata);
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

        public void Delete(string inCode)
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
                    hy_dian dbdata = db.hy_dian.FirstOrDefault(x => x.hy_no == inCode);
                    if (dbdata != null)
                        db.hy_dian.DeleteOnSubmit(dbdata);
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

        public void Delete(hy_dian entity)
        {
            this.Delete(entity.mobile);
        }

        public hy_dian NewEntity()
        {
            return new hy_dian();
        }

    }
}
