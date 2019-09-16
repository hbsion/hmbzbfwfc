using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using marr.BusinessRule;
using marr.DataLayer;

namespace UI
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PTDselect : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (context.Request["type"] == "province")
            {
                context.Response.Write(select1());
            }
            else if (context.Request["type"] == "city")
            {
                context.Response.Write(select2(context.Request["provinceID"]));
            }
            else if (context.Request["type"] == "district")
            {
                context.Response.Write(select3(context.Request["cityID"]));
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string select1()
        {

            string sql = "select * from T_Province ";

            DataSet ds = new DataSet();

            ds = DbHelperSQL.Query(sql);


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                sb.Append("{");
                sb.Append("\"ProvinceID\":\"" + item[0] + "\",\"ProvinceName\":\"" + item[1] + "\"");
                sb.Append("},");
            }
            if  (sb.Length>1)
             sb.Remove(sb.Length - 1, 1);

            sb.Append("]");
            return sb.ToString();

        }


        public string select2(string id)
        {

            string sql = "select * from T_City where ProID='" + id + "'" ; 

            DataSet ds = new DataSet();

            ds = DbHelperSQL.Query(sql);


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                sb.Append("{");
                sb.Append("\"CityID\":\"" + item[0] + "\",\"CityName\":\"" + item[1] + "\"");
                sb.Append("},");
            }
            if (sb.Length > 1)
            sb.Remove(sb.Length - 1, 1);

            sb.Append("]");
            return sb.ToString();
        }

        public string select3(string id)
        {


            string sql = "select * from T_District where CityID='" + id + "'" ;

            DataSet ds = new DataSet();

            ds = DbHelperSQL.Query(sql);


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                sb.Append("{");
                sb.Append("\"DistrictID\":\"" + item[0] + "\",\"DistrictName\":\"" + item[1] + "\"");
                sb.Append("},");
            }
            if (sb.Length > 1)
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }


    }
}
