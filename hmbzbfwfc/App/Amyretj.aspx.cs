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
    public partial class Amyretj : System.Web.UI.Page
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



            string xtp_no = cboprod.SelectedValue;



           this.awards.DataSource = DbHelperSQL.Query("exec psal_myretj  '" + cu_no + "','" + xtp_no + "','" + txtdatef.SelectedDate.Value.ToString() + "','" + txtdatet.SelectedDate.Value.ToString() + "' "); 
           this.awards.DataBind();


        }
    }
}
