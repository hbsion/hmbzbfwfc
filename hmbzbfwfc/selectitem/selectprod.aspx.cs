
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
using brclass = marr.BusinessRule.brProduct;
using entity = marr.DataLayer.Inv_Part;
using brcontext = marr.BusinessRule.brProduct.QueryContext;
using Telerik.Web.UI;


namespace UI
{
    public partial class selectprod : System.Web.UI.Page
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



            //this.navigation.IParent = this;
            //if (!string.IsNullOrEmpty(base.Request["type"]))
            //{
            //    this.type = base.Request["type"];
            //}
            //if (!string.IsNullOrEmpty(base.Request["kind"]))
            //{
            //    this.kind = base.Request["kind"];
            //}
            //else
            //{
            //    base.Response.Write("参数传入错误！");
            //    base.Response.End();
            //}
            //if (!this.Page.IsPostBack)
            //{
            //    this.BindData();
            //}

 



            }

        }

        public void BindData()
        {
            using (brProduct br = new brProduct())
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
           // this.QueryContext.ProdNO = this.txtProdNO.Text;
          //  this.QueryContext.ProdName = this.txtProdName.Text;

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
          //      e.Item.Attributes.Add("onmouseout", "this.className=\"ItemStyle\"");
           //     e.Item.Attributes.Add("onmouseover", "this.className=\"trmouseover\"");
           //     e.Item.Attributes.CssStyle.Value = "background-color: #CCCCCC; font-weight: bold;color:blue";

      //          e.Item.Cells[3].Attributes.CssStyle.Value = "padding-left:20px;";

                GridDataItem dataItem = e.Item as GridDataItem;

            //    lbl.Text = "<input type=\"radio\" name=\"radio\" onclick=\"javascript:doOk()\" value=\"{id:'" + dataItem["p_no"].Text + "',name:'" + dataItem["pname"].Text + "'}\">";

                    lbl.Text = "<input type=\"radio\" name=\"radio\"  value=\"{id:'" + dataItem["p_no"].Text + "',name:'" + dataItem["pname"].Text + "',type:'" + dataItem["type"].Text + "'}\">";

            }

    

        }

    }
}
