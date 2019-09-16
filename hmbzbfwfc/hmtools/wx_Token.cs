using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using marr.BusinessRule;
using marr.DataLayer;
using System.Net;
using System.IO;
using LitJson;
using System.Text;
using UI.WXPay;
using marr.BusinessRule.Entity;
using UI.hmtools;

namespace UI
{
    //获取access_token
    public class wx_Token
    {
        public string get_token()
        {
            string token = "";
            //从数据库调用access
            using (brwxaccess bracc = new brwxaccess())
            {
                wx_accessToken accToken = bracc.Retrtktype("","token");
                if (accToken != null)//判断是否到期
                {
                    token = accToken.access_token;
                    //过期时间
                    DateTime dt = (DateTime)accToken.in_date.Value.AddSeconds((int)accToken.expires_in - 200);
                    //当前时间
                    if (dt < DateTime.Now)
                    {
                        //时间过期了
                        token = _token();
                    }
                }
                else
                {
                    token = _token();
                }

            }
            return token;
        }

        public string _token()
        {
            string appid = WxPayConfig.APPID;
            string secret = WxPayConfig.APPSECRET;
            string strip = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strip);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("GBK"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            //context.Response.Write(retString);
            //  String input = "{\"access_token\":\"KIkVqsOBVZkb7Od-Gd5hvSgHj5pd6ruNaipSQGcmYMXxzErkOUlhCNjt1l27iUjtKPOxzLlxgFaZVEaVVI7_t6QkGgdZsLkZBFPpP7Tl438\",\"expires_in\":7200}";
            string tokem = "";
            try
            {
                JsonData jd = JsonMapper.ToObject(retString);
                tokem = (string)jd["access_token"];
                int expires_in = (int)jd["expires_in"];
                //如果不存在,则新建一条空数据

                //如果不存在,则新建一条空数据
                string sql1 = "insert into wx_accessToken(unitcode,wxid,tokentype) select '" + "" + "','" + "" + "', 'token' where not exists(select * from wx_accessToken where wxid='" + "" + "' and    tokentype='token')";
                DbHelperSQL.ExecuteSql(sql1);

                //存入token和expires_in
                string sql = "update wx_accessToken set access_token='" + tokem + "',expires_in=" + expires_in + ",in_date='" + DateTime.Now + "' where wxid='" + "" + "' and   tokentype='token' ";
                DbHelperSQL.ExecuteSql(sql);
            }
            catch
            {

            }
            return tokem;
        }


        public string get_token(Wxcode inwxcode)
        { 
            string token = "";
            //从数据库调用access
            using (brwxaccess bracc = new brwxaccess())
            {
                wx_accessToken accToken = bracc.Retrtktype(inwxcode.WxId,"token");
                if (accToken != null)//判断是否到期
                {
                    token = accToken.access_token;
                    //过期时间
                    DateTime dt = (DateTime)accToken.in_date.Value.AddSeconds((int)accToken.expires_in - 200);
                    //当前时间
                    if (dt < DateTime.Now)
                    {
                        //时间过期了
                        token = _token(inwxcode);
                    }
                }
                else
                {
                    token = _token(inwxcode);
                }

            }
            return token;
        }

        public string _token(Wxcode inwxcode)
        {
            string appid = inwxcode.AppId;
            string secret = inwxcode.AppSecret;


            Log.Error("token", "secret:" + secret);
            Log.Error("token", "appid:" + appid);


            string strip = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strip);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("GBK"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            //context.Response.Write(retString);
            //  String input = "{\"access_token\":\"KIkVqsOBVZkb7Od-Gd5hvSgHj5pd6ruNaipSQGcmYMXxzErkOUlhCNjt1l27iUjtKPOxzLlxgFaZVEaVVI7_t6QkGgdZsLkZBFPpP7Tl438\",\"expires_in\":7200}";
            string tokem = "";
            try
            {
                Log.Error("token", retString);

                JsonData jd = JsonMapper.ToObject(retString);
                tokem = (string)jd["access_token"];
                int expires_in = (int)jd["expires_in"];

                //如果不存在,则新建一条空数据
                string sql1 = "insert into wx_accessToken(unitcode,wxid,tokentype) select '" + inwxcode.unitcode + "','" + inwxcode.WxId + "', 'token' where not exists(select * from wx_accessToken where wxid='" + inwxcode.WxId + "' and    tokentype='token')";
                DbHelperSQL.ExecuteSql(sql1);

                //存入token和expires_in
                string sql = "update wx_accessToken set access_token='" + tokem + "',expires_in=" + expires_in + ",in_date='" + DateTime.Now + "' where wxid='" + inwxcode.WxId + "' and   tokentype='token' ";
                DbHelperSQL.ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                Log.Error("token", ex.Message);
            }
            return tokem;
        }

    }
}
