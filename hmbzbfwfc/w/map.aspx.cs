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
using UI.hmtools;


namespace UI
{

    public partial class map : System.Web.UI.Page
    {
        public string xtgimges;
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string lat = this.Request["lat"];
                string lng = this.Request["lng"];

                txtlongitude.Value = lng;
                txtlatitude.Value = lat;

 
            }

        }




    }
}
