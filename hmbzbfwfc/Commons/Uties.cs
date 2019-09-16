using hmbzbfwfc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace hmbzbfwfc.Commons
{
    public static class Uties
    {
        public static string getrandom(int length)
        {
            string s = "100000";
            string e = "999999";
            Random ran2 = new Random();
            int myint2 = ran2.Next(int.Parse(s.Substring(0, length)), int.Parse(e.Substring(0, length)));
            string ranstr = Convert.ToString(myint2);
            return ranstr;
        }
        /////////////////////////////////////////////////
        public static string getPostStr(HttpContextBase context)
        {
            Int32 intLen = Convert.ToInt32(context.Request.InputStream.Length);
            byte[] b = new byte[intLen];
            context.Request.InputStream.Read(b, 0, intLen);
            return System.Text.Encoding.UTF8.GetString(b);
        }
        #region 生成随机字母或数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
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
        /// <summary>
        /// 根据日期和随机码生成订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyMMddHHmmss");//yyyyMMddHHmmssms
            return num + Number(2, true).ToString();

        }

        /// <summary>
        /// 将实体类转为XML字符串
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeXml<T>(T obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sw.ToString();
            }
        }
        /// <summary>
        /// 反序列化XML为实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static T DESerializerXML<T>(string strXML) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 此方法只能使用枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetEnum<T>()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                T it = (T)item;
                dic.Add(item.ToString(), (int)item);
            }
            return dic;
        }
       /* public static Dictionary<string, int> GetPowerType(TravelEnum.PowerType lt)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            switch (lt)
            {
                case TravelEnum.PowerType.业务员:
                    break;
                case TravelEnum.PowerType.店长:
                    dic.Add(TravelEnum.PowerType.业务员.ToString(), (int)TravelEnum.PowerType.业务员);
                    dic.Add(TravelEnum.PowerType.用户.ToString(), (int)TravelEnum.PowerType.用户);
                    dic.Add(TravelEnum.PowerType.收银.ToString(), (int)TravelEnum.PowerType.收银);
                    break;
                case TravelEnum.PowerType.总财务:
                    break;
                case TravelEnum.PowerType.收银:
                    break;
                case TravelEnum.PowerType.用户:
                    break;
                case TravelEnum.PowerType.超级管理员:
                    dic = GetEnum<TravelEnum.PowerType>();
                    break;
            }
            return dic;
        }*/

        private static int Next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;//这里用uint作为生成的随机数  
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }
        #endregion

        public static void EntityCopy<T>(T objold, T objnew) where T : class
        {
            Type myType = objold.GetType(),
                myType2 = objnew.GetType();
            PropertyInfo currobj = null;
            if (myType == myType2)
            {
                PropertyInfo[] myProperties = myType.GetProperties();
                for (int i = 0; i < myProperties.Length; i++)
                {
                    currobj = objold.GetType().GetProperties()[i];
                    currobj.SetValue(objnew, currobj.GetValue(objold, null), null);
                }
            }
        }
        public static Dictionary<string, object> GetEntitDictionary<T>(T model, string noStr = null,bool IsNull=false)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in model.GetType().GetProperties())
            {
                if (!string.IsNullOrEmpty(noStr))
                {
                    if (noStr.IndexOf(item.Name) > -1)
                        continue;
                }
                if (IsNull && item.GetValue(model) == null)
                {
                    continue;
                }
                if (item.PropertyType == typeof(string))
                    dic.Add(item.Name, NoHTML(HttpUtility.UrlDecode(item.GetValue(model) == null ? "" : item.GetValue(model).ToString())));
                else
                    dic.Add(item.Name, item.GetValue(model));
            }
            return dic;
        }

        public static string GetParams(this Dictionary<string, object> dic)
        {
            List<string> list = new List<string>();
            foreach (var item in dic)
            {
                list.Add("{item.Key}={item.Value}");
            }
            return string.Join("&", list);
        }
        public static Dictionary<string, string> GetDic(string str)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string s in str.Split('&'))
            {
                string[] strItem = s.Split('=');
                if (strItem.Length == 2)
                {
                    if (string.IsNullOrEmpty(strItem[1]))
                        dic.Add(strItem[0], null);
                    else
                        dic.Add(strItem[0], HttpContext.Current.Server.UrlDecode(strItem[1]));
                }

            }
            return dic;
        }

        #region URL请求数据
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param = "", Dictionary<string, string> dic = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }


        /// <summary>
        /// HTTP POST方式请求数据 contenttype=application/json
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPostJ(string url, string param, Dictionary<string, string> dic = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// 执行URL获取页面内容
        /// </summary>
        public static string UrlExecute(string urlPath)
        {
            if (string.IsNullOrEmpty(urlPath))
            {
                return "error";
            }
            StringWriter sw = new StringWriter();
            try
            {
                HttpContext.Current.Server.Execute(urlPath, sw);
                return sw.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }
        #endregion
        public static string CMD5(string strSource, string sEncode = "UTF-8")
        {
            //new
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //获取密文字节数组
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.GetEncoding(sEncode).GetBytes(strSource));

            //转换成字符串，并取9到25位
            //string strResult = BitConverter.ToString(bytResult, 4, 8); 
            //转换成字符串，32位

            string strResult = BitConverter.ToString(bytResult);

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉
            strResult = strResult.Replace("-", "");

            return strResult.ToLower();
        }

        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(System.DateTime time)
        {
            long ts = ConvertDateTimeToInt(time) / 1000;
            return ts.ToString();
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        /// <summary>
        /// 将时间戳转成时间格式
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime IntToDateTime(long timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds(timestamp);
        }
        ///   <summary>   
        ///    去除HTML标记   
        ///   </summary>   
        ///   <param    name="NoHTML">包括HTML的源码   </param>   
        ///   <returns>已经去除后的文字</returns>   
        public static string NoHTML(string Htmlstring)
        {
            if (System.Web.HttpContext.Current == null)
                return Htmlstring;
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }


        #region apose创建数据表
        public static DataTable SetTable<T>(List<T> listModel, string tableName)
        {
            DataTable dt = new DataTable();
            bool t = true;
            foreach (var model in listModel)
            {
                var dic = GetEntitDictionary(model);
                List<string> list = new List<string>();
                list.AddRange(dic.Keys);
                if (t)
                {
                    dt = CreateDataTable(tableName, list.ToArray());
                    t = false;
                }
                DataRow dr = dt.NewRow();
                object[] obj = dic.Values.ToArray();
                SetDataTable(dr, dic.Values.ToArray());
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable CreateDataTable(string tableName, string[] tableClumes)
        {
            DataTable dataTable = new DataTable(tableName);
            foreach (string item in tableClumes)
            {
                dataTable.Columns.Add(new DataColumn(item));
            }
            return dataTable;
        }
        public static void SetDataTable(DataRow row, object[] obj)
        {
            int i = 0;
            foreach (var item in obj)
            {
                row[i] = item;
                i++;
            }
        }
        #endregion

        public static List<string> GetDiscount(string discount)
        {
            List<string> list = new List<string>();
            if (discount == null)
            {
                list.Add("0");
                list.Add("1000");
                list.Add("0");
                return list;
            }
            var dis = discount.Trim('|').Split('|');
            if (dis.Length != 3)
            {
                list.Add("0");
                list.Add("1000");
                list.Add("0");
                return list;
            }
            list = dis.ToList();
            return list;

        }

        public static string XMLToJson(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            return jsonText;
        }
        

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encode">编码方式</param>
        /// <param name="result">解密字符串</param>
        /// <returns></returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// 16进制转换算法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String strTo16(string str)
        {
            //创建MD5对像
            MD5 md5 = MD5.Create();
            //将字符串转换成数组
            byte[] ba = Encoding.Default.GetBytes(str);
            //将数组加密 成  加密数组
            byte[] md55 = md5.ComputeHash(ba);
            //将加密数组编译成字符串
            // return Encoding.Default.GetString(md55);
            string STR = "";
            //便利数组中元素转化成字符并拼接
            for (int I = 0; I < md55.Length; I++)
            {
                //X 表是10进制,X2表示16进制

                STR += md55[I].ToString("x2");

            }
            return STR;
        }
    }
}