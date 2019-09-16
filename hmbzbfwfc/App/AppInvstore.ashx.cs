using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hmbzbfwfc.App
{
    /// <summary>
    /// AppInvstore 的摘要说明
    /// </summary>
    public class AppInvstore : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string scode = context.Request["scode"];
            string cu_no = context.Request["cu_no"];


            string strjason = "[";



            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<Inv_Store> ret = db.Inv_Store;
            if (scode != null && scode.Trim().Length > 0)
                ret = ret.Where(x => x.st_name.Contains(scode) || x.st_no.Contains(scode));

            var list = ret.OrderBy(x => x.fid);

            int i = 0;
            foreach (var item in list)
            {
                if (i > 0)
                {
                    strjason += ",";
                }
                strjason += "{\"id\":\"" + item.st_no.Trim() + "\",\"title\":\"" + item.st_name.Trim() + "\"}";

                i++;
            }


            strjason += "]";

            context.Response.Write(strjason);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}