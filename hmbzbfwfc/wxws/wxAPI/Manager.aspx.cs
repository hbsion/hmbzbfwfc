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
using System.Collections.Generic;
using System.Web.Script.Serialization;
using hmbzbfwfc.Commons;


namespace UI.wxws.wxAPI
{
    public partial class Manager : System.Web.UI.Page
    {

        public class OrderItem
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string action = Request["action"];//功能操作类型
                if (action == "login")
                {   //登录用户名可以手机号/微信号/代理商帐号
                    string username = Request["username"];
                    string password = Request["password"];


                    if (username == null || username == "" )
                    {
                        Response.Write(Mes("请输入登录帐号!"));
                        Response.End();
                    }
                    if (password == null || password == "")
                    {
                        Response.Write(Mes("请输入登录密码!"));
                        Response.End();
                    }

                    string mypasswd = Uties.CMD5(password);

                    using (brCustomer br = new brCustomer())
                    {
                        customer en = br.Retrieve(username);
                        if (en != null)
                        {
                            if (en.passwd.Trim() == mypasswd)
                            {
   
                                string myright = "1";

                                string cu_no = en.cu_no;
                                Session["login_account"] = cu_no;
                                string success = "{\"success\":true,\"Message\":\"\",\"cu_no\":\"" + cu_no + "\",\"myright\":\"" + myright + "\"}";
                                Response.Write(success);
                                Response.End();
                            }
                            else
                            {
                                Response.Write(Mes("密码错误!"));
                                Response.End();
                            }
                        }
                        else
                        {
                            Response.Write(Mes("该帐号尚未注册!"));
                            Response.End();
                        }
                    }

                }
                //加载资料
                if (action == "list")
                {//登录主页面Agent_Index.aspx
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string loginAccount = Request["cu_no"];
                    using (brCustomer br = new brCustomer())
                    {
                        customer ob = br.Retrieve(loginAccount);

                        if (ob != null)
                        {

                            int level = 0;

                            Session["login_account"] = ob.cu_no;
                            Hashtable ht = new Hashtable();
                            ht.Add("success", true);
                            ht.Add("Message", "ok");
                            ht.Add("no", ob.cu_no);
                            ht.Add("name", ob.cu_name);
                            ht.Add("wxid", "");
                            ht.Add("phone", ob.phone);
                            ht.Add("gclass","");
                            ht.Add("level", level);
                            ht.Add("province", ob.province);
                            ht.Add("city", ob.city);
                            ht.Add("addr", ob.addr);
                            //string success = "{\"success\":true,\"Message\":\"\",\"no\":\"" + ob.cu_no + "\",\"name\":\"" + ob.cu_name + "\",\"wxid\":\"" + ob.WxId + "\"}";
                            Response.Write(jss.Serialize(ht));
                            Response.End();
                        }
                    }
                }
                //修改密码
                if (action == "modifypwd")
                {
                    if (Session["login_account"] == null)
                    {
                        Response.Write(Mes("未登录!"));
                        Response.End();
                    }
                    string cu_no = Session["login_account"].ToString();
                    string oldpwd = Request["OldPassWord"].ToString();
                    string newpws = Request["NewPassWord"].ToString();
                    if (oldpwd == "" || newpws == "")
                    {
                        Response.Write(Mes("修改密码不可为空"));
                        Response.End();
                    }
                    if (oldpwd != newpws)
                    {
                        Response.Write(Mes("两次输入的密码不一致"));
                        Response.End();
                    }
                    //if(o)
                    using (brCustomer br = new brCustomer())
                    {

                        customer ob = br.Retrieve(cu_no);
                        if (ob != null)
                        {
                            ob.passwd = newpws;
                            br.Update(ob);
                            string success = "{\"success\":true,\"Message\":\"密码修改成功!\"}";
                            Response.Write(success);
                            Response.End();
                        }
                    }
                }
                //加载品牌,发
                if (action == "agentBrandList" || action == "shareurl_getlevel")
                {
                    string brandcode = "";//品牌
                    string status = "停用";
                    string cu_no = Request["cu_no"];
                    using (brCustomer br = new brCustomer())
                    {
                        customer ob = br.Retrieve(cu_no);
                        if (ob != null)
                        {
                            brandcode = ob.cutype;                                status = "正常";
                        }
                        if (brandcode.Length > 0)
                        {
                            string info = "[";
                            int ii = 0;

                            info += "]";
                            string success = "{\"success\":true,\"Message\":\"\",\"content\":" + info + ",\"agentid\":\"A" + ob.fid.ToString() + "\",\"gclass\":\"" + "" + "\",\"status\":\"" + status + "\"}";
                            Response.Write(success);
                            Response.End();
                        }
                        else
                        {
                            Response.Write(Mes("您暂无授权的品牌商标!"));
                            Response.End();
                        }
                    }
                }

                //单号查询
                if (action == "wxQuery")
                {
                    string no = Request["code"];
                    using (brCuShipScan brCuShip = new brCuShipScan())
                    {
                        sal_cuship obj = brCuShip.getShipRecord(no);
                        if (obj == null)
                        {
                            Response.Write(Mes("出货单号不存在!"));
                            Response.End();
                        }
                        string success = "{\"success\":true,\"Message\":\"ok\",\"Code\":\"" + obj.ship_no + "\",\"CusName\":\"" + obj.cu_name + "\",\"Qty\":\"" + obj.mqty + "\",\"WorkDate\":\"" + obj.ship_date.Value.ToString("yyyy-MM-dd") + "\"}";
                        Response.Write(success);
                        Response.End();
                    }
                }
                //退出系统
                if (action == "loginOut")
                {
                    if (Session["login_account"] != null)
                    {
                        Session.Remove("login_account");
                        HttpResponse response = HttpContext.Current.Response;
                        HttpCookie cookie = response.Cookies["userInfo"];
                        cookie.Values.Remove("userName");
                        cookie.Values.Remove("userPwd");
                        cookie.Values.Remove("check");
                        //ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>localStorage[\"hmcu_no\"]=\"\"</script>");
                        Response.Write("OK");
                        Response.End();
                        //Response.Redirect("wxLogin.aspx");
                        //GC.Collect();
                    }
                }


                if (action == "level2")
                {//按等级获取下级代理商列表
                    string brandId = Request["brandId"];
                    string gclass = Server.UrlDecode(Request["gclass"]);
                    string cu_no = Session["login_account"].ToString();
                    string searchText = Request["searchText"];
                    if (searchText == null)
                        searchText = "";
                    string json = "[";
                    using (brCustomer brcus = new brCustomer())
                    {
                        int n = 0;

                        //foreach (var item in brcus.QueryLevel(brandId, cu_no, gclass, searchText))
                        //{
                        //    if (n > 0)
                        //        json += ",";
                        //    json += "{\"cu_no\":\"" + item.cu_no + "\",\"cu_name\":\"" + item.cu_name + "\",\"gclass\":\"" + item.gclass + "\",\"wxId\":\"" + item.WxId + "\",\"phone\":\"" + item.phone + "\",\"AuthCode\":\"A" + item.fid.ToString() + "\"}";
                        //    n++;
                        //}
                        json += "]";
                    }
                    string success = "{\"success\":true,\"Message\":\"\",\"customers\":" + json + "}";
                    Response.Write(success);
                    Response.End();
                }

            }
        }

      
        /// <summary>
        /// 设置前端Cookie，保存账号密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        public void SetCookies(string userName, string userPwd)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                HttpCookie cookie = response.Cookies["userInfo"];
                if (cookie != null)
                {
                    cookie.Values.Set("userName", userName);
                    cookie.Values.Set("userPwd", userPwd);
                    cookie.Values.Set("check", "1");
                    cookie.Expires = DateTime.Now.AddDays(365);
                    response.SetCookie(cookie);
                }
            }
        }
        public string Msg(string msg)
        {
            string msg1 = "{\"success\":true,\"Message\":\"" + msg + "\"}";
            return msg1;
        }
        #region 错误通用回复
        /// <summary>
        /// 
        /// </summary> 
        public string Mes(string s)
        {
            // string callback = Request["callback"];
            return "{\"success\":false,\"Message\":\"" + s + "\"}";
        }
        #endregion
    }
}
