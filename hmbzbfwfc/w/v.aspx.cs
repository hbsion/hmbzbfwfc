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
using entity = marr.DataLayer.xt_webnote;
using brclass = marr.BusinessRule.brWebnote;

namespace UI
{
    public partial class v : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string sys_no = this.Request["b"];
                using (brclass br = new brclass())
                {
                    xt_webnote webnote = br.Retrieve(sys_no);
                    if (webnote != null)
                    {
                        fwresult2.InnerHtml = webnote.sys_note;
                        Page.Title = webnote.sys_title;
                        myheader.InnerHtml = webnote.sys_title;
                    }
                    else
                        fwresult2.InnerHtml = "";
                }
            }

        }



    }
}
