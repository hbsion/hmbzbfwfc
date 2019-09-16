using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    [Serializable] 
    public class brHydianJflist : IDisposable
    {
        public class Entity
        {
            public int fid { get; set; }
            public string hy_no { get; set; }
            public string UserName { get; set; }
            public string p_no { get; set; }
            public string pname { get; set; }
            public string fwcode { get; set; }
            public Decimal?  jf { get; set; }
            public DateTime? in_date { get; set; }
            public string ComTrueName { get; set; }
            public string remark { get; set; }
            public string hy_cn { get; set; }

        }

        [Serializable] 
        public class QueryContext
        {
            public DateTime? datef { get; set; }
            public DateTime? datet { get; set; }
            public string UserName { get; set; }
            public string FwCode { get; set; }
            public string hy_no { get; set; }
            public string hy_cn { get; set; }
            public Sorter sorter { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }

            public QueryContext()
            {
                datef = DateTime.Now.AddMonths(-1);
                datet = DateTime.Now;
;
            }
        }


        public brHydianJflist()
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

        private IQueryable<hy_dianjflist> GetAll(QueryContext context)
        {

            IQueryable<hy_dianjflist> ret = db.hy_dianjflist;

            if (context.datef.HasValue)
                ret = ret.Where(x => x.in_date >= context.datef);
            if (context.datet.HasValue)
                ret = ret.Where(x => x.in_date <= context.datet.Value.AddDays(1));

            //if (context.UserName != null && context.UserName.Trim().Length > 0)
            //    ret = ret.Where(x => x.UserName == context.UserName);

            if (context.hy_no != null && context.hy_no.Trim().Length > 0)
               ret = ret.Where(x => x.hy_no == context.hy_no);

            if (context.FwCode != null && context.FwCode.Trim().Length > 0)
              ret = ret.Where(x => x.fwcode.StartsWith(context.FwCode.Trim()));


            switch (context.sorter.Field)
            {
                case "hy_no":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.hy_no) : ret.OrderByDescending(x => x.hy_no);
                    break;
                case "hy_cn":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.hy_cn) : ret.OrderByDescending(x => x.hy_cn);
                    break;

                //case "UserName":
                //    ret = context.sorter.ASC ? ret.OrderBy(x => x.UserName) : ret.OrderByDescending(x => x.UserName);
                //    break;
                case "pname":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.pname) : ret.OrderByDescending(x => x.pname);
                    break;
                case "fwcode":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.fwcode) : ret.OrderByDescending(x => x.fwcode);
                    break;
                case "jf":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.jf) : ret.OrderByDescending(x => x.jf);
                    break;
                case "in_date":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.in_date) : ret.OrderByDescending(x => x.in_date);
                    break;
                //case "ComTrueName":
                //    ret = context.sorter.ASC ? ret.OrderBy(x => x.ComTrueName) : ret.OrderByDescending(x => x.ComTrueName);
                //    break;
                case "remark":
                    ret = context.sorter.ASC ? ret.OrderBy(x => x.remark) : ret.OrderByDescending(x => x.remark);
                    break;

            }
          //  ret = ret.Distinct();

            return ret;
        }

        public IEnumerable<hy_dianjflist> Query(QueryContext context)
        {
            return Utils.ExecLTS(this.GetAll(context).Skip(context.PageIndex * context.PageSize).Take(context.PageSize));
        }

        public int Count(QueryContext context)
        {
            return this.GetAll(context).Count();
        }
        public hy_dianjflist Retrieve(string inCode)
        {
            return db.hy_dianjflist.FirstOrDefault(x => x.fwcode == inCode);
        }

        public hy_dianjflist RetrieveJH(string inCode)  
        {
            return db.hy_dianjflist.FirstOrDefault(x => x.fwcode == inCode );
        } 
        
    }
}
