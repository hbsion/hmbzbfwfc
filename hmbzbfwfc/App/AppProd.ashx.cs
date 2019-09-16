using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using marr.BusinessRule;
using marr.DataLayer;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppProd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string scode = context.Request["scode"];
            string cu_no = context.Request["cu_no"];
      
  
            string strjason = "[";

            //using (brCustomer br1 = new brCustomer())
            //{
            //    customer mycust = br1.Retrieve(cu_no);
            //    if (mycust == null)
            //    {
            //        context.Response.Write("该账号不存在!！");
            //        context.Response.End();
            //    }
            //    if (mycust.cutype != null && mycust.cutype != "")
            //    {
            //        string[] t = mycust.cutype.Split('|');
            //        if (t.Length >= 3)
            //        {
            //            s1 = t[0];
            //            s2 = t[1];
            //            s3 = t[2];
            //        }
            //    }
            //}

            using (brProduct br = new brProduct())
            {
                int i = 0;
                foreach (var item in br.Querys(scode))
                {
                    if (i > 0)
                    {
                        strjason += ",";
                    }
                    string type = "";
                    if (!string.IsNullOrEmpty(item.type))
                        type ="_"+ item.type;
                    strjason += "{\"id\":\"" + item.p_no.Trim() + "\",\"title\":\"" + item.pname.Trim()+type.Trim() + "\"}";

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
