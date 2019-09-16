using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;


namespace UI.WXPay
{
    [Serializable]
    public class Wxcode
    {
        public Wxcode(wx_gid wxgid)
        {
            this.WxId = wxgid.WxId;
            this.WxName = wxgid.WxName;
            this.AppId = wxgid.AppId;
            this.AppSecret = wxgid.AppSecret;
            this.mchid = wxgid.mchid;
            this.mkey = wxgid.mkey;
            this.msslpath = wxgid.msslpath;
            this.msslpasswd = wxgid.msslpasswd;
            this.unitcode = wxgid.unitcode;
        }

        
        public Wxcode()
        {
            this.WxId = "";
            this.WxName = "";
            this.AppId = WxPayConfig.APPID;
            this.AppSecret = WxPayConfig.APPSECRET;
            this.mchid = WxPayConfig.MCHID;
            this.mkey = WxPayConfig.KEY;
            this.msslpath = WxPayConfig.SSLCERT_PATH;
            this.msslpasswd = WxPayConfig.SSLCERT_PASSWORD;
            this.unitcode = "";
;
        }

        public string WxId { get; set; }
        public string WxName { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string mchid { get; set; }
        public string mkey { get; set; }
        public string msslpath { get; set; }
        public string msslpasswd { get; set; }

        public string unitcode { get; set; }
    }
}
