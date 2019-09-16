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
    public partial class Acustlist : System.Web.UI.Page
    {
        protected int i = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                string cu_no =  Session["xtcu_no"].ToString();

                if (cu_no == null)
                    cu_no = "--";


                using (brCustomer br = new brCustomer())
                {

                    this.cbocustomer.DataSource = br.Query(cu_no,"");
                    this.cbocustomer.DataTextField = "cu_name";
                    this.cbocustomer.DataValueField = "cu_no";
                    this.cbocustomer.DataBind();
                    this.cbocustomer.Items.Insert(0, new RadComboBoxItem("本经销商", cu_no));
                    this.cbocustomer.Items.Insert(0, new RadComboBoxItem("全部", ""));


                }


                using (brProduct br = new brProduct())
                {
                    this.cboprod.DataSource = br.Query("");
                    this.cboprod.DataTextField = "pname";
                    this.cboprod.DataValueField = "p_no";

                    this.cboprod.DataBind();
                    this.cboprod.Items.Insert(0, new RadComboBoxItem("全部", ""));
                }
 
            }
        }







        protected void Button3_Click(object sender, EventArgs e)
        {
            string cu_no = Session["xtcu_no"].ToString();

            if (cu_no == null)
                cu_no = "--";


            string scode = txtscode.Text.Trim();

            string xtcu_no = cbocustomer.SelectedValue;
            string xtp_no = cboprod.SelectedValue;

            string strfindstr = " where a.p_no=b.p_no and  a.stqty>0 and a.cu_no=c.cu_no and (c.cu_no='" + cu_no  + "' or c.xtcu_no='" + cu_no + "') ";

            if (xtcu_no.Length>0 )
            {
                strfindstr= strfindstr + " and  a.cu_no='" + xtcu_no + "'"  ;
            }

            if (xtp_no.Length > 0)
            {
                strfindstr = strfindstr + " and  a.p_no='" + xtp_no + "'";
            }

            if (scode.Length > 0)
            {
                strfindstr = strfindstr + "  and  (a.p_no like '%" + scode + "%' or b.pname like '%" + scode + "%' or c.cu_no like '%" + scode + "%' or c.cu_name like '%" + scode + "%')";

            }


            string strsql = " select a.*,b.pname,c.cu_name,c.xtcu_no  from sal_custqty  a,inv_part b,customer c " + strfindstr;


           this.awards.DataSource = DbHelperSQL.Query(strsql);
           this.awards.DataBind();


        }
    }
}
