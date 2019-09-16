using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace UI.app
{
    public partial class AppCheckupdata : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/plain; charset=utf-8";

            string aaaa = Request["type"];

            if (aaaa == null || aaaa.Length == 0)
                aaaa = "1";

            if (aaaa == "0" || aaaa == "3")
            {

               Response.Write("{" + "\"updateUrl\":\"" + "itms-services://?action=download-manifest&amp;url=https://www.hmbrand.com/tq.plist" + "\"," + "\"remoteVersion\":\"" + "2.0" + "\"" + "}");

            }
            else
            {
                Response.Write("{" + "\"updateUrl\":\"" + "http://www.hmbrand.com/tq.apk" + "\"," + "\"remoteVersion\":\"" + "4.2" + "\"" + "}");


            }
        }
    }
}
