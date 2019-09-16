using marr.BusinessRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hmbzbfwfc.App
{
    /// <summary>
    /// AppVender 的摘要说明
    /// </summary>
    public class AppVender : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string scode = context.Request["scode"];
            string cu_no = context.Request["cu_no"];


            string strjason = "[";



            using (brVender br = new brVender())
            {
                int i = 0;
                foreach (var item in br.Querys(scode))
                {
                    if (i > 0)
                    {
                        strjason += ",";
                    }
                    strjason += "{\"id\":\"" + item.cu_no.Trim() + "\",\"title\":\"" + item.cu_name.Trim() + "\"}";

                    i++;
                }
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