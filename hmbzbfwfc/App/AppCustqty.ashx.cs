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

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppCustqty : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string scode = context.Request["scode"];

            if (cu_no == null)
                cu_no = "";

            if (scode == null)
                scode = "";

            string strjason="[" ;

            string strsql = " select a.*,b.pname from sal_custqty  a,inv_part b where a.p_no=b.p_no and  a.stqty>0 and  a.cu_no='" + cu_no +  "' and  (a.p_no like '%" + scode + "%' or b.pname like '%" + scode + "%')";

              DataSet ds = new DataSet();
  
              ds = DbHelperSQL.Query(strsql);


            DataTable dt=ds.Tables[0];
            //遍历行
            int i = 0;
            foreach(DataRow dr in dt.Rows){
 

                if (i > 0)
                {
                    strjason += ",";
                }
                strjason += "{\"title\":\"" + dr["pname"].ToString() + "\",\"id\":\"" + dr["p_no"].ToString() + "\",\"cu_no\":\"" + "" + "\",\"stqty\":\"" + dr["stqty"].ToString() + "\"}";

                i++;
            }




            strjason += "]";

            context.Response.Write(strjason);


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
