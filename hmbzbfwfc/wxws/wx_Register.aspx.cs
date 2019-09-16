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
using UI.hmtools;
using marr.BusinessRule;
using marr.DataLayer;

namespace UI.wxws
{
    public partial class wx_Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                if (Session["login_account"] == null)
                {
                    txtxtcu_no.Value = "";
                }
                else
                {
                    string cu_no = Session["login_account"].ToString();
                    txtxtcu_no.Value = cu_no;
                }

 


            }
        }
    }
}
