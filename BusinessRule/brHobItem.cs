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
    public class brHobItem : IBrObject<hob_item>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string itme_no = "itme_no";
            public const string itme_name = "itme_name";
            public const string mqtyf = "mqtyf";
            public const string mqtyt = "mqtyt";
            public const string in_date = "in_date";
            public const string en_date = "en_date";
            public const string itemtype = "itemtype";
            public const string hmlv = "pnhmlvame";
            public const string remark = "remark";
            public const string unitcode = "unitcode";

        }

        [Serializable] 
        public class QueryContext
        {
            public string itme_no { get; set; }
            public string itme_name { get; set; }
            public DateTime? in_date { get; set; }
            public DateTime? en_date { get; set; }
            public decimal mqtypf { get; set; }
            public decimal mqtypt { get; set; }
            public decimal hmlv { get; set; }
            public string itemtype { get; set; }
            public string unitcode { get; set; }
            public string remark { get; set; }

            public string seartext { get; set; }

            public string province { get; set; }


            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brHobItem()
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

        void IBrObject<hob_item>.CheckAction(hob_item entity, ActionEnum action) { }

        private IQueryable<hob_item> GetByFilter(IQueryable<hob_item> q, Filter filter)
        {
            switch (filter.Field)
            {
                case FieldType.itme_no:
                    return q.Where(x => x.itme_no.StartsWith(filter.SearchText));

                case FieldType.itme_name:
                    return q.Where(x => x.itme_name.StartsWith(filter.SearchText));
                case FieldType.unitcode:
                    return q.Where(x => x.unitcode == filter.SearchText);
                case FieldType.itemtype:
                    return q.Where(x => x.itemtype == filter.SearchText);
                case FieldType.remark:
                    return q.Where(x => x.remark == filter.SearchText);
                default:
                    return q;
            }
        }

        private IQueryable<hob_item> GetBySorter(IQueryable<hob_item> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.itme_no:
                    return sorter.ASC ? q.OrderBy(x => x.itme_no) : q.OrderByDescending(x => x.itme_no);
                case FieldType.itme_name:
                    return sorter.ASC ? q.OrderBy(x => x.itme_name) : q.OrderByDescending(x => x.itme_name);
                case FieldType.hmlv:
                    return sorter.ASC ? q.OrderBy(x => x.hmlv) : q.OrderByDescending(x => x.hmlv);
                case FieldType.in_date:
                    return sorter.ASC ? q.OrderBy(x => x.in_date) : q.OrderByDescending(x => x.in_date);
                case FieldType.en_date:
                    return sorter.ASC ? q.OrderBy(x => x.en_date) : q.OrderByDescending(x => x.en_date);
                case FieldType.remark:
                    return sorter.ASC ? q.OrderBy(x => x.remark) : q.OrderByDescending(x => x.remark);
                case FieldType.itemtype:
                    return sorter.ASC ? q.OrderBy(x => x.itemtype) : q.OrderByDescending(x => x.itemtype);
                default:
                    return q.OrderBy(x => x.fid);
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="sorter">排序条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">此页的记录数量</param>
        /// <returns>符合条件的结果集</returns>
        public IEnumerable<hob_item> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.hob_item, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }
        /// <summary>
        /// 求符合筛选条件记录的和
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>int</returns>
        public int Count(Filter filter)
        {
            return GetByFilter(db.hob_item, filter).Count();
        }

        public hob_item Retrieve(string inCode)
        {
            return db.hob_item.FirstOrDefault(x => x.itme_no == inCode);
        }
        /// <summary>
        /// 检索记录
        /// </summary>
        /// <param name="inCode"></param>
        /// <param name="incu_no"></param>
        /// <returns>一条记录</returns>
        public hob_item Retrieve(string inCode, string incu_no)
        {
            return db.hob_item.FirstOrDefault(x => x.itme_no == inCode && x.unitcode == incu_no);
        }

        public hob_item Retrieve(int infid)
        {
            return db.hob_item.FirstOrDefault(x => x.fid == infid);
        }


        public hob_item Retrieve(string initem_no, DateTime indatetime)
        {
            return db.hob_item.FirstOrDefault(x => x.itme_no == initem_no && x.in_date <= indatetime && x.en_date >= indatetime);
        }


        public bool CheckRe(string no)
        {
            bool re = false;
            int i = db.hob_item.Count(x => x.itme_no == no);
            if (i > 0)
            {
                return re = true;
            }
            else
            {
                re = false;
            }
            return re;
        }

        public hob_item recx(string inCode)
        {
            return db.hob_item.FirstOrDefault(x => (x.itme_no == inCode || x.remark == inCode || x.itme_name == inCode || x.itemtype == inCode));
        }

        //public hob_item recx(string inCode, string incutype)
        //{
        //    return db.hob_item.FirstOrDefault(x => (x.p_no == inCode || x.cu_name==inCode || x.cu_no==inCode )  && x.pname.Contains(incutype));
        //}

        public hob_item rewx(string inCode)
        {
            return db.hob_item.FirstOrDefault(x => x.itme_no == inCode);
        }

        //public hob_item rewx(string inCode,string incutype)
        //{
        //    return db.hob_item.FirstOrDefault(x => x.WxId == inCode && x.cutype == incutype);
        //}

        /// <summary>
        /// 检索全部数据
        /// </summary>
        /// <param name="context">参数</param>
        /// <returns>结果集</returns>
        private IQueryable<hob_item> GetAll(QueryContext context)
        {
            IQueryable<hob_item> ret = db.hob_item;

            if (context.in_date.HasValue)
                ret = ret.Where(x=>x.in_date>=context.in_date);
            if (context.en_date.HasValue)
                ret = ret.Where(x => x.in_date < context.en_date.Value.AddDays(1));
            if (context.itme_no != null && context.itme_no.Trim().Length > 0)
                ret = ret.Where(x => x.itme_no.StartsWith(context.itme_no));

            if (context.itme_name != null && context.itme_name.Trim().Length > 0)
                ret = ret.Where(x => x.itme_name.StartsWith(context.itme_name));

            if (context.itemtype != null && context.itemtype.Trim().Length > 0)
                ret = ret.Where(x => x.itemtype.Contains(context.itemtype));

            if (context.remark != null && context.remark.Trim().Length > 0)
                ret = ret.Where(x => x.remark.Contains(context.remark));
            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode.Contains(context.unitcode));
            if (context.hmlv != 0 && context.hmlv> 0)
                ret = ret.Where(x => x.hmlv.Value==context.hmlv);
            ret = GetBySorter(ret, context.sorter);
            return ret;
        }

        /// <summary>
        /// 加载列表时显示全部记录分页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<hob_item> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<hob_item> Query()  //返回全部
        {

            IQueryable<hob_item> ret = db.hob_item;

            return ret.OrderBy(x => x.itme_no);
        }

        public IEnumerable<hob_item> Querybylv()  //返回按机率由大往下的
        {

            IQueryable<hob_item> ret = db.hob_item;

            return ret.OrderBy(x => x.hmlv);
        }

        public IEnumerable<hob_item> Querybylv(string inunitcode)  //返回按机率由大往下的
        {

            IQueryable<hob_item> ret = db.hob_item.Where(x => x.dian_no == inunitcode && x.in_date <= System.DateTime.Now && x.en_date >= System.DateTime.Now);

            return ret.OrderBy(x => x.hmlv);
        }


        public IEnumerable<hob_item> Querybylv(string initems,string inunitcode)  //返回按机率由大往下的
        {

            IQueryable<hob_item> ret = db.hob_item.Where(x => initems.IndexOf(x.itme_no.Trim()) >= 0 && x.in_date <= System.DateTime.Now && x.en_date >= System.DateTime.Now);

            return ret.OrderBy(x => x.hmlv);
        }

        public hob_bfwjf rehbfwjf(string insnno)
        {
             return db.hob_bfwjf.FirstOrDefault(x => x.snnof.CompareTo(insnno) <= 0 && x.snnot.CompareTo(insnno) >= 0);
          
           
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(hob_item entity)
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
                    if (db.hob_item.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.hob_item.InsertOnSubmit(entity);
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

        public void Update(hob_item entity)
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
                    hob_item dbdata = db.hob_item.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<hob_item>(entity, dbdata);
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
                    hob_item dbdata = db.hob_item.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.hob_item.DeleteOnSubmit(dbdata);
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

        public void Delete(hob_item entity)
        {
            this.Delete(entity.fid);
        }

        public hob_item NewEntity()
        {
            return new hob_item();
        }

    


    }
}
