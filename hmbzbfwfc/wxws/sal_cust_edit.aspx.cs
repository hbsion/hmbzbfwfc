using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using marr.BusinessRule;
using marr.DataLayer;

namespace UI.wxws
{
    public partial class sal_cust_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login_account"] != null)
            {
                string cu_no = Session["login_account"].ToString();
                if (cu_no != null && cu_no != "")
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>localStorage.hmcu_no='" + cu_no + "'</script>");
                }
            }
            else
            {
                JsTip("请登录", "wxLogin.aspx");
            }
            string xtcu_no = Request["id"];
            if (xtcu_no != null && xtcu_no != "") show(xtcu_no);
            else
            {
                Response.Redirect("wx_CustomerLevelList.aspx");
            }
        }
        protected void show(string cu_no)
        {
            string ss = "";
            using (brCustomer br = new brCustomer())
            {
                string xtcu_no = "";
                if   (Session["login_account"] !=null)
                {
                    xtcu_no = Session["login_account"].ToString();
                }

                 //下级等级
                customer en = br.Retrieve(cu_no);
                //加载等级
  
                if (en != null)
                {
                    ss += "<div class=\"dmain\"></div>";
                    ss += myDiv("账号","<span class='cname'>"+en.cu_no+"</span>");
                    ss += myDiv("姓名", en.cu_name);

                    ss += myDiv("手机号", en.phone);

                     ss += myDiv("省份", en.province);
                    ss += myDiv("城市", en.city);
                    ss += myDiv("收货地址", en.addr);
    
                    myD.InnerHtml = ss;
                }
            }
        }
        public string myDiv(string title, string s)
        {
            return "<div class='dmain'><div class='dL'>" + title + "：</div><div class=' dR'>" + s + "</div><div class='c'></div></div>";
        }
        protected void JsTip(string msg)
        {
            JsTip(msg, "");
        }

        protected void JsTip(string msg, string url)
        {
            if (string.IsNullOrEmpty(url))
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('" + msg + "');</script>");
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('" + msg + "');location.href='" + url + "';</script>");
        }
    }
}