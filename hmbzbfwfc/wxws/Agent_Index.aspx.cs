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
    public partial class Agent_Index : System.Web.UI.Page
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
                HttpCookie cookie = request.Cookies["userInfo"];
                if (cookie != null)
                {
                    string user = cookie["userName"];
                    string pwd = cookie["userPwd"];
                    if (user != null && user != "")
                    {
                        // txtuser.Value = user.Trim();
                    }
                    if (user != null && pwd != null && user != "" && pwd != "")
                    {
                        using (brCustomer br = new brCustomer())
                        {
                            customer en = br.Retrieve(user.Trim());
                            if (en != null)
                            {
                                if (en.passwd.Trim() == pwd.Trim())
                                {
             
                                    //登录成功
                                    Session["login_account"] = en.cu_no;
                                    ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>localStorage.hmcu_no ='" + en.cu_no + "';location.href='Agent_Index.aspx'</script>");
                                }
                            }
                        }
                    }
                }
            }
        }
        #region 错误通用回复
        public string Mes(string s)
        {
            // string callback = Request["callback"];
            return "{\"success\":false,\"Message\":\"" + s + "\"}";
        }
        #endregion
    }
}
