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
    public class AppGetcust : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];

            using (brCustomer br = new brCustomer())
            {
                customer mycust = br.Retrieve(cu_no);

                if (mycust != null)
                {
                    context.Response.Write("{" + "\"addr\":\"" + mycust.addr + "\"," + "\"mobile\":\"" + "" + "\"" + "}");
                }
                else
                {
                     context.Response.Write("");
                }

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
