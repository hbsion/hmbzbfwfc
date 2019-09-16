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
using marr.BusinessRule;
using marr.DataLayer;
using marr.BusinessRule.Entity;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UI.WXPay;

namespace UI.w
{

    public partial class fw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if   (Session["fwcode"] !=null)
                {
                    txt_fw_code.Value = Session["fwcode"].ToString();
                }



            }



        }







        public string get_urlencode(string postusl)
        {
            return HttpUtility.UrlEncode(postusl);
        }

       


    }
}
