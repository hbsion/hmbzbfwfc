using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using marr.BusinessRule;
using marr.DataLayer;
using System.Net;
using System.IO;
using System.Text;
using LitJson;
using marr.BusinessRule.Entity;
using UI.WXPay;

namespace UI
{
    //获取jsapi_ticket
    public class wx_ticket
    {
        public string get_ticket()
        {
            string token = "";
            //从数据库调用access
            using (brwxaccess bracc = new brwxaccess())
            {
                wx_accessToken accToken = bracc.Retrieve("ticket");
                if (accToken != null)//判断是否到期
                {
                    token = accToken.access_token;
                    //过期时间
                    DateTime dt = (DateTime)accToken.in_date.Value.AddSeconds((int)accToken.expires_in - 200);
                    //当前时间
                    if (dt < DateTime.Now)
                    {
                        //时间过期了
                        token = _ticket();
                    }
                }
                else
                {
                    token = _ticket();
                }

            }
            return token;
        }


        public string get_ticket(Wxcode inwxcode)
        {
            string token = "";
            //从数据库调用access
            using (brwxaccess bracc = new brwxaccess())
            {
                wx_accessToken accToken = bracc.Retrtktype(inwxcode.WxId,"ticket");
                if (accToken != null)//判断是否到期
                {
                    token = accToken.access_token;
                    //过期时间
                    DateTime dt = (DateTime)accToken.in_date.Value.AddSeconds((int)accToken.expires_in - 200);
                    //当前时间
                    if (dt < DateTime.Now)
                    {
                        //时间过期了
                        token = _ticket(inwxcode);
                    }
                }
                else
                {
                    token = _ticket(inwxcode);
                }

            }
            return token;
        }

        public string _ticket()
        {
    
            wx_Token wxToken = new wx_Token();//获取access_token
            wxToken._token(); //重新获取ticket前 重新获取token
            string strip = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token="+wxToken.get_token()+"&type=jsapi ";
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
                tokem = (string)jd["ticket"];
                int expires_in = (int)jd["expires_in"];

                string sql1 = "insert into wx_accessToken(unitcode) select 'ticket' where not exists(select * from wx_accessToken where unitcode='ticket')";
                DbHelperSQL.ExecuteSql(sql1);

                string sql = "update wx_accessToken set access_token='" + tokem + "',expires_in=" + expires_in + ",in_date='" + DateTime.Now + "' where unitcode='ticket'";
                DbHelperSQL.ExecuteSql(sql);
            }
            catch
            {

            }
            return tokem;
        }

        public string _ticket(Wxcode inwxcode)
        {

            wx_Token wxToken = new wx_Token();//获取access_token
            wxToken._token(inwxcode); //重新获取ticket前 重新获取token
            string strip = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + wxToken.get_token(inwxcode) + "&type=jsapi ";
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
                tokem = (string)jd["ticket"];
                int expires_in = (int)jd["expires_in"];

                string sql1 = "insert into wx_accessToken(unitcode,wxid,tokentype) select '" + inwxcode.unitcode + "','" + inwxcode.WxId + "', 'ticket' where not  exists(select * from wx_accessToken where wxid='" + inwxcode.WxId + "' and    tokentype='ticket')";  
                DbHelperSQL.ExecuteSql(sql1);

                string sql = "update wx_accessToken set access_token='" + tokem + "',expires_in=" + expires_in + ",in_date='" + DateTime.Now +  "' where wxid='" + inwxcode.WxId + "' and   tokentype='ticket' " ;
                DbHelperSQL.ExecuteSql(sql);





            }
            catch
            {

            }
            return tokem;
        }

    }
}
