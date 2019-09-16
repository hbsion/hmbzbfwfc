using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using marr.DataLayer;
using marr.BusinessRule.Entity;
using System.Collections;
using System.Xml.Linq;
using marr.BusinessRule;
using Telerik.Web.UI;

namespace UI
{
    public partial class Acusalelist : System.Web.UI.Page
    {
        protected int i = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                string cu_no =  Session["xtcu_no"].ToString();

                if (cu_no == null)
                    cu_no = "--";



                using (brProduct br = new brProduct())
                {
                    this.cboprod.DataSource = br.Query("");
                    this.cboprod.DataTextField = "pname";
                    this.cboprod.DataValueField = "p_no";

                    this.cboprod.DataBind();
                    this.cboprod.Items.Insert(0, new RadComboBoxItem("全部", ""));
                }

                txtdatef.SelectedDate = DateTime.Now.AddDays(-30);
                this.txtdatet.SelectedDate = DateTime.Now;
 
            }
        }







        protected void Button3_Click(object sender, EventArgs e)
        {
            string cu_no = Session["xtcu_no"].ToString();

            if (cu_no == null)
                cu_no = "--";


            string scode = txtscode.Text.Trim();


            string xtp_no = cboprod.SelectedValue;

            string strfindstr = " where  a.xtcu_no='" + cu_no  + "' ";



            if (xtp_no.Length > 0)
            {
                strfindstr = strfindstr + " and  a.p_no='" + xtp_no + "'";
            }

            strfindstr = strfindstr + " and  a.ship_date >='" + txtdatef.SelectedDate.Value.ToString() + "'";

            strfindstr = strfindstr + " and  a.ship_date <'" + txtdatet.SelectedDate.Value.AddDays(1).ToString() + "'";


            if (scode.Length > 0)
            {
                strfindstr = strfindstr + "  and  (a.p_no like '%" + scode + "%' or a.pname like '%" + scode + "%' or a.chetype like '%" + scode + "%' or a.cu_name like '%" + scode + "%' or a.chetype like '%" + scode  + "%')";

            }


            string strsql = " select a.*    from sal_cusale  a  " + strfindstr;


           this.awards.DataSource = DbHelperSQL.Query(strsql);
           this.awards.DataBind();


        }
    }
}
