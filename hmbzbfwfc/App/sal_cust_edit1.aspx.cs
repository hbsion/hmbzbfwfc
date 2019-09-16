using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using entity = marr.DataLayer.customer;
using brclass = marr.BusinessRule.brCustomer;
using marr.BusinessRule;
using marr.DataLayer;
using Telerik.Web.UI;
using System.Linq;

namespace UI
{
    public partial class sal_cust_edit2 : System.Web.UI.Page
    {
        protected string DefaultTitle = ConfigurationSettings.AppSettings["SystemName"].ToString();             //获取系统名称
        protected string UploadFileTypes = ConfigurationSettings.AppSettings["UploadFileType"].ToString();       //获取允许上传的格式
        protected string UploadSavePath = ConfigurationSettings.AppSettings["UploadSavePath"].ToString();       //获取附件保存根目录,如upfiles/


        private void ShowData(entity en)
        {
            if (en != null)
            {

                this.txtcu_no.Text = en.cu_no;
                this.txtcu_name.Text = en.cu_name;

                this.hid_province.Value = en.province  ;
                this.hid_city.Value = en.city ;

                this.province.Items.Insert(0, en.province);

                this.province.Value = en.province  ;

                this.City.Items.Insert(0, en.city);

                this.City.Value = en.city;


                this.txtaddr.Text = en.addr;
                this.txtphone.Text = en.phone;

                txtcutype.Text = en.cutype;

         
                this.txtremark.Text = en.remark;
          //      this.txtpasswd.Text = en.passwd;



                this.txtcu_no.Enabled = false;

                txtcutype.Enabled = false;

   
            }
            else
            {
            
   

            }

        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveData()
        {
            entity en ;
            using (brclass br = new brclass())
            {
                if (Request["fid"] != null && Request["fid"].Length > 0)
                {
                    en = br.Retrieve(int.Parse(Request["fid"]));
                }
                else
                {
                    en = new entity();
                    //新增重复判断

              

                }


                en.cu_no = this.txtcu_no.Text;
                en.cu_name = this.txtcu_name.Text;
     



                en.province = this.hid_province.Value.Trim();
                en.city = this.hid_city.Value.Trim();

                en.addr = this.txtaddr.Text;
                en.phone = this.txtphone.Text;
    
         
                en.remark = this.txtremark.Text;
                if (this.txtpasswd.Text.Trim().Length > 0)
                    en.passwd = this.txtpasswd.Text;

                en.unitcode = "";

           //     en.xtcu_no = this.User.cu_no;

                //string cubrand = "";
                //for (int i = 0; i < brand.Items.Count; i++)
                //{
                //    if (brand.Items[i].Selected)
                //    {
                //        cubrand += brand.Items[i].Value;
                //    }
                //}
                //en.cutype = cubrand;
             
   


                if (Request["fid"] != null && Request["fid"].Length > 0)
                    br.Update(en);
                else
                    br.Insert(en);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                if (Request["fid"] != null && Request["fid"].Length > 0 )
                {

                    using (brclass br = new brclass())
                    {
                        this.ShowData(br.Retrieve(int.Parse(Request["fid"])));
                    }
                }
                else
                {
                    this.ErrorMsg("参数错误", false);
                }


            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveData();
                this.ErrorMsg("", true);
            }
            catch (Exception ex)
            {
                this.ErrorMsg(ex.Message, false);
            }
        }

        private void ErrorMsg(string ErrMsg, bool OkCan)
        {
            if (!OkCan)
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", " <script>alert('" + ErrMsg + "');</script>");
            else
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", " <script>alert('操作成功\\n" + ErrMsg + "');</script>");
        }


        public void JsTip(string msg)
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('" + msg + "');</script>");
        }


    }
}
