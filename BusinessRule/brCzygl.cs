using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brCzygl : IBrObject<Gy_Czygl>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string Czybm = "Czybm";
            public const string Czymc = "Czymc";
            public const string xtcu_no = "xtcu_no";
            public const string unitcode = "unitcode";

        }

        public class QueryContext
        {
            public string Czybm { get; set; }
            public string Czymc { get; set; }
            public string sysrole { get; set; }
            public string unitcode { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }
        public brCzygl()
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

        void IBrObject<Gy_Czygl>.CheckAction(Gy_Czygl entity, ActionEnum action) { }

        private IQueryable<Gy_Czygl> GetByFilter(IQueryable<Gy_Czygl> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.Czybm:
                    return q.Where(x => x.czybm.StartsWith(filter.SearchText));
                case FieldType.Czymc:
                    return q.Where(x => x.czymc.Contains(filter.SearchText));

                case FieldType.unitcode:
                    return q.Where(x => x.unitcode==(filter.SearchText));


          

                default:
                    return q;
            }
        }

        private IQueryable<Gy_Czygl> GetBySorter(IQueryable<Gy_Czygl> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.Czybm:
                    return sorter.ASC ? q.OrderBy(x => x.czybm) : q.OrderByDescending(x => x.czybm);
                case FieldType.Czymc:
                    return sorter.ASC ? q.OrderBy(x => x.czymc) : q.OrderByDescending(x => x.czymc);
                case FieldType.unitcode:
                    return sorter.ASC ? q.OrderBy(x => x.unitcode) : q.OrderByDescending(x => x.unitcode);

                default:
                    return q.OrderBy(x => x.fid);
            }
        }

        public IEnumerable<Gy_Czygl> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.Gy_Czygl, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.Gy_Czygl, filter).Count();
        }


        public Gy_Czygl Retrieve(string inCode)
        {
            return db.Gy_Czygl.FirstOrDefault(x => x.czybm == inCode);
        }

        public Gy_Czygl Retrieve(int infid)
        {
            return db.Gy_Czygl.FirstOrDefault(x => x.fid == infid);
        }


         public Gy_Czygl Retrieve(string  inzh,string inmm)
        {
            string passwd = CMD5(inmm);

            return db.Gy_Czygl.FirstOrDefault(x => x.czybm == inzh && x.czmm == passwd);
        }




         public bool CheckLogin(string inUserID, string inPassword, out Gy_Czygl user)
         {

             string passwd = CMD5(inPassword);

             if (db != null)
             {
                 user = db.Gy_Czygl.FirstOrDefault(u => u.czybm == inUserID && u.czmm == passwd);

                 return user != null;
             }
             else
                 throw new ObjectDisposedException("db");
         }



         public bool ChangPassWd(string inUserID, string inopass, string innpass, out string outstr)
         {
              
             if (inopass==null ||  innpass==null || inopass.Length==0 || innpass.Length==0)
             {

                 outstr = "请输入正新旧密码！";
                 return false;
             }


             string opasswd = CMD5(inopass);
             string npasswd = CMD5(innpass);
             outstr = "";

             Gy_Czygl user = db.Gy_Czygl.FirstOrDefault(u => u.czybm == inUserID && u.czmm == opasswd);
             if (user == null)
             {
                 outstr = "旧秘密不正确！";
                 return false;
             }
             else
             {
                 user.czmm = npasswd;
                 Update(user);

                 return true;

             }


         }


        private IQueryable<Gy_Czygl> GetAll(QueryContext context)
        {
            IQueryable<Gy_Czygl> ret = db.Gy_Czygl;

            if (context.Czybm != null && context.Czybm.Trim().Length > 0)
                ret = ret.Where(x => x.czybm.StartsWith(context.Czybm));

            if (context.Czymc != null && context.Czymc.Trim().Length > 0)
                ret = ret.Where(x => x.czymc.Contains(context.Czymc));

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode ==context.unitcode);

            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<Gy_Czygl> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<Gy_Czygl> Query()  //返回全部
        {

            IQueryable<Gy_Czygl> ret = db.Gy_Czygl;

            return ret.OrderBy(x => x.czybm);
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(Gy_Czygl entity)
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
                    if (db.Gy_Czygl.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.Gy_Czygl.InsertOnSubmit(entity);
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

        public void Update(Gy_Czygl entity)
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
                    Gy_Czygl dbdata = db.Gy_Czygl.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<Gy_Czygl>(entity, dbdata);
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
                    Gy_Czygl dbdata = db.Gy_Czygl.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.Gy_Czygl.DeleteOnSubmit(dbdata);
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

        public void Delete(Gy_Czygl entity)
        {
            this.Delete(entity.fid);
        }

        public Gy_Czygl NewEntity()
        {
            return new Gy_Czygl();
        }

        public static string CMD5(string strSource, string sEncode = "UTF-8")
        {

            if (strSource == null || strSource.Length == 0)
                return "";


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
