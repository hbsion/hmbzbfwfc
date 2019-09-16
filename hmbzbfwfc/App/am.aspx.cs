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
    public partial class am : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string xtcu_no = Request["xtcu_no"];
                if (xtcu_no != null && xtcu_no.Length > 0)
                {

                    Session["xtcu_no"] = xtcu_no;
                    this.xtcu_no.Value = xtcu_no;
                }
            }

        }
    }
}
