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
using UI.hmtools;

namespace UI.wxws
{
    public partial class wx_shipScan : System.Web.UI.Page
    {
        string xtcu_no = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["login_account"] != null && Session["login_account"].ToString().Trim() != null)
                {
                    xtcu_no = Session["login_account"].ToString();
                }

                wx_JsApi _wxJsApi = new wx_JsApi();
                wx_JsApi.Api _api = _wxJsApi.get_JsApi(Request.Url.ToString());
                string apiList = "'scanQRCode'";
                string Config = "wx.config({debug:false,appId:'" + _api.AppId + "',timestamp:" + _api.timestamp + ",nonceStr:'" + _api.noncestr + "',signature:'" + _api.jsapi_ticket + "',jsApiList:[" + apiList + "]});";
                this.myConfig.InnerHtml = "<script>" + Config + "</script>";
            }


            /*
             备注：
             * 1、选择订单回调，访问服务器加载产品
             * 2、全局变量“hoid",程序锁；ajax开始改为1，其他ajax不能运行。运行完改成0，其他程序才能运行
             * 3、  ordership  notOrdership  ；样式关键字，选择订单隐藏"notOrdership" 块；普通发货显示
             * 4、有订单号，选择订单产品；其他东西都不需要
             
             
             */
        }
    }
}
