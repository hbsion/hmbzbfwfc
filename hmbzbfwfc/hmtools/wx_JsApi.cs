using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using marr.BusinessRule;
using marr.DataLayer;
using UI.WXPay;
using marr.BusinessRule.Entity;
using UI.WXPay;

namespace UI
{
    public class wx_JsApi
    {
        //
        [Serializable] 
        public class Api
        {
            public int timestamp { get; set; }//时间
            public string jsapi_ticket { get; set; }//签名
            public string noncestr { get; set; }//js签名随机字符串
            public string AppId { get; set; }
        }
        public Api get_JsApi(string Url)
        {
            Api _api = new Api();
            string _ticket = "";

            //获取jsapi_ticket
            wx_ticket _wx_ticket = new wx_ticket();
            _ticket = _wx_ticket.get_ticket();

            int date = ConvertDateTimeInt();//时间戳
            string ran = GenerateCheckCode(16);//随机数
            string string1 = "jsapi_ticket=" + _ticket + "&noncestr=" + ran + "&timestamp=" + date + "&url=" + Url;

            string1 = EncryptToSHA1(string1);


            _api.AppId = WxPayConfig.APPID;
            _api.jsapi_ticket = string1.ToLower();
            _api.noncestr = ran;
            _api.timestamp = date;
            return _api;
        }


        public Api get_JsApi(string Url, Wxcode inwxcode)
        {
            Api _api = new Api();
            string _ticket = "";

            //获取jsapi_ticket
            wx_ticket _wx_ticket = new wx_ticket();

            _ticket = _wx_ticket.get_ticket(inwxcode);

            int date = ConvertDateTimeInt();//时间戳
            string ran = GenerateCheckCode(16);//随机数
            string string1 = "jsapi_ticket=" + _ticket + "&noncestr=" + ran + "&timestamp=" + date + "&url=" + Url;

            string1 = EncryptToSHA1(string1);


            _api.AppId = inwxcode.AppId;

            _api.jsapi_ticket = string1.ToLower();
            _api.noncestr = ran;
            _api.timestamp = date;
            return _api;
        }


        //时间戳
        private int ConvertDateTimeInt()
        {
            DateTime time = DateTime.Now;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        // 生成的字母字符串
        private string GenerateCheckCode(int codeCount)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        //  SHA1加密
        public string EncryptToSHA1(string str)
        {

            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");

        }
    }
}
