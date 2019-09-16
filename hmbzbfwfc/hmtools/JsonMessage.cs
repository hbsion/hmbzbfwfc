using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Data;

namespace UI.hmtools
{

    /*   调用
          using (brCustomer br = new brCustomer())
            {
               // IEnumerable<customer>
                    string html = new JsonMessage { Success = true, Data = br.Query("1001", "").ToList() }.ToString();
                    //marr.DataLayer
                string html2 = new JsonMessage { Success = true, Data = br.Query("1001", "").FirstOrDefault() }.ToString();
            }
     * 
       string sql = "select * from customer";
            DataTable dt = DbHelperSQL.ExecuteTable(sql);
            context.Response.Write(new JsonMessage { Success = true, Data = dt, Message = "" }.ToString());
            context.Response.End();
     */
    public class JsonMessage
    {
        public  bool Success { get; set; }
        public  object Data { get; set; }
        public  string Message { get; set; }
        public string remark { get; set; }


        public int? pqty { get; set; }
        public string cu_no { get; set; }
        public string cu_name { get; set; }
        public string p_no { get; set; }
        public string pname { get; set; }
        public string snno { get; set; }
        public decimal? amount { get; set; }
        public override string ToString()
        {
         /*   Hashtable hs = new Hashtable();
          PropertyInfo[] propertyInfo = this.GetType().GetProperties();

          for (int i = 0; i < propertyInfo.Length; i++)
          {
              object objectValue = propertyInfo[i].GetGetMethod().Invoke(this, null);

              if (objectValue == null)
              {

                  continue;

              }
              hs.Add(propertyInfo[i].Name, objectValue);
          }
            JavaScriptSerializer jss = new JavaScriptSerializer();

            return jss.Serialize(hs);*/
           // json js = new json();
            return ToJson(this);
        }



        /*json类，大量修改过
         
         
         */



        /// <summary> 

        /// 对象转换为Json字符串 

        /// </summary> 

        /// <param name="jsonObject">对象</param> 

        /// <returns>Json字符串</returns> 

        public string ToJson(object jsonObject)
        {

            try
            {

                StringBuilder jsonString = new StringBuilder();

                jsonString.Append("{");

                PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();

                for (int i = 0; i < propertyInfo.Length; i++)
                {

                    object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);

                    if (objectValue == null)
                    {

                        continue;

                    }

                    StringBuilder value = new StringBuilder();

                    if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                    {

                        value.Append("\"" + objectValue.ToString() + "\"");

                    }
                    else if (objectValue is bool)
                    {

                        value.Append(objectValue.ToString().ToLower());

                    }
                
                    else if (objectValue is DataSet)
                    {

                        value.Append(ToJson((DataSet)objectValue));

                    }
                    else if (objectValue is DataTable)
                    {

                        value.Append(ToJson((DataTable)objectValue));

                    }
                    else if (objectValue is string)
                    {

                        value.Append("\"" + String2Json(objectValue.ToString()) + "\"");

                    }

                    else if (objectValue is IEnumerable)
                    {

                        value.Append(ToJson((IEnumerable)objectValue));

                    }
                    else if (objectValue.GetType().ToString().Contains("marr.DataLayer"))
                    {

                        value.Append(ToJson((object)objectValue));

                    }
                    else
                    {

                        value.Append("\"" + objectValue.ToString() + "\"");

                    }

                   // value.Append(objectValue.GetType().ToString());

                    

                    jsonString.Append("\"" + propertyInfo[i].Name + "\":" + value.ToString() + ","); ;

                }

                return jsonString.ToString().TrimEnd(',') + "}";

            }

            catch (Exception ex)
            {

                throw ex;

            }

        }

        /// <summary>

        /// List转成json 

        /// </summary>

        /// <typeparam name="T"></typeparam>

        /// <param name="jsonName"></param>

        /// <param name="list"></param>

        /// <returns></returns>

        public string ListToJson<T>(IList<T> list, string jsonName)
        {

            StringBuilder Json = new StringBuilder();

            if (string.IsNullOrEmpty(jsonName))

                jsonName = list[0].GetType().Name;

            Json.Append("{\"" + jsonName + "\":[");

            if (list.Count > 0)
            {

                for (int i = 0; i < list.Count; i++)
                {

                    T obj = Activator.CreateInstance<T>();

                    PropertyInfo[] pi = obj.GetType().GetProperties();

                    Json.Append("{");

                    for (int j = 0; j < pi.Length; j++)
                    {

                        Type type = pi[j].GetValue(list[i], null).GetType();

                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));



                        if (j < pi.Length - 1)
                        {

                            Json.Append(",");

                        }

                    }

                    Json.Append("}");

                    if (i < list.Count - 1)
                    {

                        Json.Append(",");

                    }

                }

            }

            Json.Append("]}");

            return Json.ToString();

        }



        /// <summary>

        /// List转成json 

        /// </summary>

        /// <typeparam name="T"></typeparam>

        /// <param name="list"></param>

        /// <returns></returns>

        public string ListToJson<T>(IList<T> list)
        {

            object obj = list[0];

            return ListToJson<T>(list, obj.GetType().Name);

        }

        
        /// 对象集合转换Json 

        /// </summary> 

        /// <param name="array">集合对象</param> 

        /// <returns>Json字符串</returns> 

        public string ToJson(IEnumerable array)
        {

            string jsonString = "[";

            foreach (object item in array)
            {

                jsonString += ToJson(item) + ",";

            }

            if (jsonString.Length > 2)
                jsonString = jsonString.Substring(0, jsonString.Length - 1);

              return jsonString + "]";

        }
        /// <summary> 

        /// DataSet转换为Json 

        /// </summary> 

        /// <param name="dataSet">DataSet对象</param> 

        /// <returns>Json字符串</returns> 

        public string ToJson(DataSet dataSet)
        {

            string jsonString = "{";

            foreach (DataTable table in dataSet.Tables)
            {

                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";

            }

            jsonString = jsonString.TrimEnd(',');

            return jsonString + "}";

        }

        /// <summary> 

        /// Datatable转换为Json 

        /// </summary> 

        /// <param name="table">Datatable对象</param> 

        /// <returns>Json字符串</returns> 

        public string ToJson(DataTable dt)
        {

            StringBuilder jsonString = new StringBuilder();

            jsonString.Append("[");

            DataRowCollection drc = dt.Rows;

            for (int i = 0; i < drc.Count; i++)
            {

                jsonString.Append("{");

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    string strKey = dt.Columns[j].ColumnName;

                    string strValue = drc[i][j].ToString();

                    Type type = dt.Columns[j].DataType;

                    jsonString.Append("\"" + strKey + "\":");

                    strValue = StringFormat(strValue, type);

                    if (j < dt.Columns.Count - 1)
                    {

                        jsonString.Append(strValue + ",");

                    }

                    else
                    {

                        jsonString.Append(strValue);

                    }

                }

                jsonString.Append("},");

            }
            if (jsonString.Length > 5)
                jsonString.Remove(jsonString.Length - 1, 1);

            jsonString.Append("]");

            return jsonString.ToString();

        }

        /// <summary>

        /// 过滤特殊字符

        /// </summary>

        /// <param name="s"></param>

        /// <returns></returns>

        public string String2Json(String s)
        {
            if (s == null) return "";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {

                char c = s.ToCharArray()[i];

                switch (c)
                {
                   // case ' ': break;
                    case '\"':

                        sb.Append("\\\""); break;

                    case '\\':

                        sb.Append("\\\\"); break;

                    case '/':

                        sb.Append("\\/"); break;

                    case '\b':

                        sb.Append("\\b"); break;

                    case '\f':

                        sb.Append("\\f"); break;

                    case '\n':

                        sb.Append("\\n"); break;

                    case '\r':

                        sb.Append("\\r"); break;

                    case '\t':

                        sb.Append("\\t"); break;

                    default:

                        sb.Append(c); break;

                }

            }

            return sb.ToString();

        }



        /// <summary>

        /// 格式化字符型、日期型、布尔型

        /// </summary>

        /// <param name="str"></param>

        /// <param name="type"></param>

        /// <returns></returns>

        private string StringFormat(string str, Type type)
        {

            if (type != typeof(string) && string.IsNullOrEmpty(str))
            {

                str = "\"" + str + "\"";

            }

            else if (type == typeof(string))
            {

                str = String2Json(str);

                str = "\"" + str + "\"";

            }
            else if (type == typeof(int))
            {

                str = "\"" + str + "\"";

            }

            else if (type == typeof(DateTime))
            {

                str = "\"" + str.Split(' ')[0] + "\"";

            }

            else if (type == typeof(bool))
            {

                str = str.ToLower();

            }



            return str;

        }
    }
}
