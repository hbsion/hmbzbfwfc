using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using marr.BusinessRule;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppXtuser : IHttpHandler
    {

 
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
            context.Response.Cache.SetNoStore();
            string scode = context.Request["scode"];
            string cu_no = context.Request["cu_no"];
   




            string strjason = "[";


            using (brCzygl br = new brCzygl())
            {
                int i = 0;
                foreach (var item in br.Query())
                {
                    if (i > 0)
                    {
                        strjason += ",";
                    }
                    strjason += "{\"id\":\"" + item.czybm.ToLower().Trim() + "\",\"title\":\"" + item.czmm.Trim() + "\"}";

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
