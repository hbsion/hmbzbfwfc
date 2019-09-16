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
    public partial class AdminshipList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                wx_JsApi _wxJsApi = new wx_JsApi();
                wx_JsApi.Api _api = _wxJsApi.get_JsApi(Request.Url.ToString());
                string apiList = "'checkJsApi', 'onMenuShareTimeline',        'onMenuShareAppMessage',        'onMenuShareQQ',        'onMenuShareWeibo',        'hideMenuItems',        'showMenuItems',        'hideAllNonBaseMenuItem',        'showAllNonBaseMenuItem',        'translateVoice',        'startRecord',        'stopRecord',        'onRecordEnd',        'playVoice',        'pauseVoice',        'stopVoice',        'uploadVoice',        'downloadVoice',        'chooseImage',        'previewImage',        'uploadImage',        'downloadImage',        'getNetworkType',        'openLocation',        'getLocation',        'hideOptionMenu',        'showOptionMenu',        'closeWindow',        'scanQRCode', 'chooseWXPay','openProductSpecificView', 'addCard','chooseCard','openCard'";
                string Config = "wx.config({debug:false,appId:'" + _api.AppId + "',timestamp:" + _api.timestamp + ",nonceStr:'" + _api.noncestr + "',signature:'" + _api.jsapi_ticket + "',jsApiList:[" + apiList + "]});";
                this.myConfig.InnerHtml = "<script>" + Config + "</script>";
            }
        }
    }
}
