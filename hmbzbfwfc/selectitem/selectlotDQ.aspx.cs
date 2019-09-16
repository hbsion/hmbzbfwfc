
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using marr.BusinessRule;
using brclass = marr.BusinessRule.brInvDaqu;
using entity = marr.DataLayer.inv_daqu;
using brcontext = marr.BusinessRule.brInvDaqu.QueryContext;
using Telerik.Web.UI;


namespace UI
{
    public partial class selectlotDQ : System.Web.UI.Page
    {


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.GetQueryContext();
            this.BindData();
        }
        private brcontext QueryContext
        {
            get
            {
                brcontext ret = Session["QueryContext_fw_prod_list"] as brcontext;
                if (ret == null)
                {
                    ret = new brcontext();
                    Session["QueryContext_fw_prod_list"] = ret;
                }
                return ret;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;


            if (!Page.IsPostBack)
            {
   

                Session["QueryContext_fw_prod_list"] = new brclass.QueryContext()
                {
                    PageIndex = 0,
                    PageSize = 10
                    
                };
                this.BindData();



       
 



            }

        }

        public void BindData()
        {
            using (brInvDaqu br = new brInvDaqu())
            {
                this.grdList.PageSize = this.QueryContext.PageSize;
                this.grdList.VirtualItemCount = br.Count(this.QueryContext);
                int maxPageIndex = (this.grdList.VirtualItemCount - 1) / this.QueryContext.PageSize;
                if (maxPageIndex < 0) maxPageIndex = 0;
                if (this.QueryContext.PageIndex > maxPageIndex) this.QueryContext.PageIndex = maxPageIndex;
                this.grdList.CurrentPageIndex = this.QueryContext.PageIndex;

                this.grdList.DataSource = br.Query(this.QueryContext);
                this.grdList.MasterTableView.CurrentPageIndex = this.QueryContext.PageIndex;

                this.grdList.DataBind();

            }
        }




        private void GetQueryContext()
        {

            this.QueryContext.seartext = this.txtSearchText.Value; 

        }



        protected void grdList_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {

            this.QueryContext.PageIndex = e.NewPageIndex;

            this.BindData();


        }

        protected void grdList_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            if (e.NewSortOrder == Telerik.Web.UI.GridSortOrder.None)
                this.QueryContext.sorter = new Sorter() { Field = e.SortExpression };
            else
                this.QueryContext.sorter = new Sorter() { Field = e.SortExpression, ASC = e.NewSortOrder == Telerik.Web.UI.GridSortOrder.Ascending };

            this.BindData();
        }

        protected void grdList_ItemDataBound(object sender, GridItemEventArgs e)
        {

            Label lbl = (Label)e.Item.FindControl("Label2");

            if (e.Item is GridDataItem)
            {
                    GridDataItem dataItem = e.Item as GridDataItem;

 
                    lbl.Text = "<input type=\"radio\" name=\"radio\"  value=\"{id:'" + dataItem["dqlot_no"].Text  + "'}\">";

            }

    

        }

    }
}
