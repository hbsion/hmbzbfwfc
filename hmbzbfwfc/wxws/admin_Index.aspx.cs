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

namespace UI.wxws
{
    public partial class admin_Index:System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Getcookie();
            }
      
        }


        private void Getcookie()      //获取账号密码
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
            {
                HttpCookie cookie = request.Cookies["adminInfo"];
                if (cookie != null)
                {
                    string user = cookie["adminName"];
                    string pwd = cookie["adminPwd"];
                    if (user != null && user != "")
                    {
                        // txtuser.Value = user.Trim();
                    }

                    if (user != null && pwd != null && user != "" && pwd != "")
                    {
                        using (brCzygl br = new brCzygl())
                        {
                            Gy_Czygl en = null;
                            if (br.CheckLogin(user,pwd,out en))
                            {
                                    //登录成功
                                    Session["AdminUser_ID"] = en.czybm;
                                    Session["login_account"] ="";
                                    ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>localStorage.xtuser_id ='" + en.czybm + "';localStorage.hmcu_no='';location.href='Admin_Index.aspx'</script>");
                            }
                            else
                            {//
                                Response.Redirect("AdminLogin.aspx");
                            }
                        }
                    }
                    else
                    {//
                        Response.Redirect("AdminLogin.aspx");
                    }
                }
            }
        }
    }
}
