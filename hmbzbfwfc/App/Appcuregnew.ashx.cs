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
using System.IO;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Appcuregnew : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string xtcu_no = context.Request["xtcu_no"];


            if (xtcu_no == null)
                xtcu_no = "";

            xtcu_no = xtcu_no.Trim();

            using (brCustomer br = new brCustomer())
            {
                string mycu_no = br.regmaxitem(xtcu_no);

                context.Response.Write("{" + "\"newcu_no\":\"" + mycu_no + "\"" + "}");

                context.Response.End();

            }


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
