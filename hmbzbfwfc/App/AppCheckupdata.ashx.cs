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
using loginuser = marr.BusinessRule.Entity.User;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.SessionState;
using marr.BusinessRule.Entity;


namespace UI
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppCheckupdata : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
                 context.Response.ContentType = "text/plain; charset=utf-8";

              string aaaa = context.Request["type"];

            if (aaaa == null || aaaa.Length == 0)
                aaaa = "1";

            if (aaaa == "0" || aaaa == "3")
            {
            
                context.Response.Write("{" + "\"updateUrl\":\"" + "itms-services://?action=download-manifest&amp;url=https://www.hmbrand.com/tq.plist" + "\"," + "\"remoteVersion\":\"" + "2.3" + "\"" + "}");
                context.Response.End();
             }
            else
            {
                context.Response.Write("{" + "\"updateUrl\":\"" + "http://tq.hmaws.com/tq.apk" + "\"," + "\"remoteVersion\":\"" + "2.5" + "\"" + "}");

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
