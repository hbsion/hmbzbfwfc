﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using System.Data;

namespace marr.BusinessRule
{
    [Serializable]
    public class brInvmark : IBrObject<inv_mark>
    {
        public static class FieldType
        {
            public const string None = null;
            public const string lot_no = "lot_no";
            public const string p_no = "p_no";
            public const string pname = "pname";
            public const string unitcode = "unitcode";

        }

        [Serializable]
        public class QueryContext
        {
            public string lot_no { get; set; }
            public string p_no { get; set; }
            public string pname { get; set; }
            public string unitcode { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public string seartext { get; set; }

            public Sorter sorter { get; set; }

            public QueryContext()
            {

            }
        }

        public brInvmark()
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

        void IBrObject<inv_mark>.CheckAction(inv_mark entity, ActionEnum action) { }

        private IQueryable<inv_mark> GetByFilter(IQueryable<inv_mark> q, Filter filter)
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

                default:
                    return q;
            }
        }

        private IQueryable<inv_mark> GetBySorter(IQueryable<inv_mark> q, Sorter sorter)
        {
            switch (sorter.Field)
            {
                case FieldType.lot_no:
                    return sorter.ASC ? q.OrderBy(x => x.p_no) : q.OrderByDescending(x => x.p_no);
                case FieldType.p_no:
                    return sorter.ASC ? q.OrderBy(x => x.pname) : q.OrderByDescending(x => x.pname);
                case FieldType.pname:
                    return sorter.ASC ? q.OrderBy(x => x.type) : q.OrderByDescending(x => x.type);

                default:
                    return q.OrderBy(x => x.p_no);
            }
        }

        public IEnumerable<inv_mark> Query(Filter filter, Sorter sorter, int pageIndex, int pageSize)
        {
            return GetBySorter(GetByFilter(db.inv_mark, filter), sorter).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int Count(Filter filter)
        {
            return GetByFilter(db.inv_mark, filter).Count();
        }

        public inv_mark Retrieve(string inCode)
        {
            return db.inv_mark.FirstOrDefault(x => x.lot_no == inCode);
        }

        public inv_mark Retrieve(string inCode, string inunitcode)
        {
            return db.inv_mark.FirstOrDefault(x => x.lot_no == inCode && x.unitcode == inunitcode);
        }


        public inv_mark Retrieve(int inCode)
        {
            return db.inv_mark.FirstOrDefault(x => x.fid == inCode);
        }

        public inv_mark Rallpart(string inunitcode)
        {
            return db.inv_mark.FirstOrDefault(x => x.unitcode == inunitcode);
        }

        private IQueryable<inv_mark> GetAll(QueryContext context)
        {
            IQueryable<inv_mark> ret = db.inv_mark;

            if (context.lot_no != null && context.lot_no.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no.StartsWith(context.lot_no));
            if (context.p_no != null && context.p_no.Trim().Length > 0)
                ret = ret.Where(x => x.p_no.Contains(context.p_no));
            if (context.pname != null && context.pname.Trim().Length > 0)
                ret = ret.Where(x => x.pname.Contains(context.pname));

            if (context.unitcode != null && context.unitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == context.unitcode);
        
            if (context.seartext != null && context.seartext.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no.Contains(context.seartext) || x.pname.Contains(context.seartext) || x.p_no.Contains(context.seartext) || x.type.Contains(context.seartext));


            ret = GetBySorter(ret, context.sorter);

            return ret;
        }

        public IEnumerable<inv_mark> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }


        public IEnumerable<inv_mark> Query()  //返回全部产品
        {

            IQueryable<inv_mark> ret = db.inv_mark;

            return ret.OrderBy(x => x.p_no);
        }


        public IEnumerable<inv_mark> Query(string inunitcode)  //返回xxx产品
        {

            IQueryable<inv_mark> ret = db.inv_mark;

            if (inunitcode != null && inunitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderBy(x => x.p_no);
        }

        public IEnumerable<inv_mark> Query(string inlot_no, string inunitcode)  //返回xxx产品
        {

            IQueryable<inv_mark> ret = db.inv_mark;

            if (inlot_no != null && inlot_no.Trim().Length > 0)
                ret = ret.Where(x => x.lot_no == inlot_no);

            if (inunitcode != null && inunitcode.Trim().Length > 0)
                ret = ret.Where(x => x.unitcode == inunitcode);

            return ret.OrderBy(x => x.ship_date);
        }



        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }

        public void Insert(inv_mark entity)
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
                    if (db.inv_mark.FirstOrDefault(x => x.fid == entity.fid) == null)
                    {
                        db.inv_mark.InsertOnSubmit(entity);
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

        public void Update(inv_mark entity)
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
                    inv_mark dbdata = db.inv_mark.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<inv_mark>(entity, dbdata);
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
                    inv_mark dbdata = db.inv_mark.FirstOrDefault(x => x.fid == inCode);
                    if (dbdata != null)
                        db.inv_mark.DeleteOnSubmit(dbdata);
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

        public void Delete(inv_mark entity)
        {
            this.Delete(entity.fid);
        }

        public inv_mark NewEntity()
        {
            return new inv_mark();
        }
    }

}
