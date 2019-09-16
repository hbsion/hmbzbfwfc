using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using marr.DataLayer;
using marr.BusinessRule;
using System.Web.Script.Serialization;
using UI.WXPay;
using UI.hmtools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using marr.BusinessRule.Entity;

namespace UI
{
    public class AppId_code
    {
        public string appid_code(string code)
        {
            if (code == null || code == "")
                return "";

            string appid = WxPayConfig.APPID ;
            string secret = WxPayConfig.APPSECRET;


            return GetAccessToken(appid, secret, code);
        }

        public string appid_code(string code, Wxcode inwxcode)
        {
            if (code == null || code == "")
                return "";

            string appid = inwxcode.AppId;
            string secret = inwxcode.AppSecret;

            Log.Debug("hreg", appid);
   
            return GetAccessToken(appid, secret, code);
        }


        /// <summary>
        /// 获取access_token  oauth方式
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken(string m_appid, string m_secret,string m_code)
        {

            string m_AcessTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid="+m_appid+"&secret="+m_secret+"&code="+m_code+"&grant_type=authorization_code";

            
            WebClient webClient = new WebClient();
            Byte[] bytes = webClient.DownloadData(m_AcessTokenUrl);
            string result = Encoding.GetEncoding("utf-8").GetString(bytes);

            Log.Debug("hreg", result);
   


            string[] temp = result.Split(',');
            if (temp.Length <= 3)
                return "";//刷新appid


            string[] tp = temp[3].Split(':');
      
            return tp[1].ToString().Replace('"', ' ').Trim().ToString();

        }


        /// <summary>
        /// 获取用户基本信息（包括UnionID机制）
        /// </summary>
        /// <returns></returns>
        public class UnionID
        {
            public string subscribe { get; set; }
            public string openid { get; set; }
            public string nickname { get; set; }
            public string sex { get; set; }
            public string language { get; set; }
            public string city { get; set; }
            public string province { get; set; }
            public string country { get; set; }
            public string headimgurl { get; set; }
            public string subscribe_time { get; set; }
            public string unionid { get; set; }
            public string remark { get; set; }
            public string groupid { get; set; }
            public UnionID()
            {
                nickname = "";
            }
        }

        /// <summary>
        /// 获取GetUserInfoUnion  获取用户基本信息(UnionID机制)
        /// </summary>
        /// <returns></returns>

        public UnionID GetUserInfoUnion(string openid)
        {
            UnionID unionid = new UnionID() { openid = openid };

            wx_Token wx = new wx_Token();

            string m_AcessTokenUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + wx._token() + "&openid=" + openid + "&lang=zh_CN";
            WebClient webClient = new WebClient();
            Byte[] bytes = webClient.DownloadData(m_AcessTokenUrl);
            string result = Encoding.GetEncoding("utf-8").GetString(bytes);


            if (result.Length > 50)
            {
                try
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    UnionID ef = jss.Deserialize<UnionID>(result);
                    if (ef != null)
                    {
                        return ef;
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("UnionID", ex.Message);
                }
            }
            return unionid;
        }

        /// <summary>
        /// 获取GetUserInfoauth  授权页面获取用户信息
        /// </summary>
        /// <returns></returns>
        public  string GetUserInfoauth(string m_appid, string m_secret, string incode)
        {


            string m_AcessTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?grant_type=authorization_code";



            WebClient webClient = new WebClient();
            Byte[] bytes = webClient.DownloadData(string.Format("{0}&appid={1}&secret={2}&code={3}", m_AcessTokenUrl, m_appid, m_secret, incode));
            string result = Encoding.GetEncoding("utf-8").GetString(bytes);


            string[] temp = result.Split(',');
            string[] tp = temp[0].Split(':');

    
            string access_token = tp[1].ToString().Replace('"', ' ').Trim().ToString();

            if (temp.Length <= 3)
                return "##";


            string[] tp2 = temp[3].Split(':');

            string myopenid = tp2[1].ToString().Replace('"', ' ').Trim().ToString();




            //获取用户信息

            string userinfoUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + myopenid + "&lang=zh_CN";

      
            WebClient webClient1 = new WebClient();
            Byte[] bytes1 = webClient1.DownloadData(userinfoUrl);
            string result1 = Encoding.GetEncoding("utf-8").GetString(bytes1);


       

            JObject ja = JObject.Parse(result1);

            if (result1.IndexOf("errcode") > 0)  //如果出错
            {
                return "##" + ja["errcode"].Value<string>();
            }
            else
            {
                string restr = "";

                if (result1.Length > 150)
                    restr = ja["openid"].Value<string>() + "|" + ja["nickname"].Value<string>() + "|" + ja["sex"].Value<string>() + "|" + ja["city"].Value<string>() + "|" + ja["country"].Value<string>() + "|" + ja["province"].Value<string>() + "|" + ja["headimgurl"].Value<string>() + "|";
                else
                    restr = ja["openid"].Value<string>() + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|";

                return restr;
            }





        }

 
    }
}
