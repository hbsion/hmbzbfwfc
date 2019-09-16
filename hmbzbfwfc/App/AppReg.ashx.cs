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
using System.IO;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppReg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

      

            string cu_no = context.Request["cu_no"];
            string password = context.Request["password"];
            string cu_name = context.Request["cu_name"];

            string addr = context.Request["addr"];
            string remark = context.Request["remark"];
            string moble = context.Request["moble"];
            string xtcu_no = context.Request["xtcu_no"];
    
            string province = context.Request["province"];
            string city = context.Request["city"];



            if (cu_no == null || password == null || cu_no.Length == 0 || password.Length == 0 )
            {
                context.Response.Write("0|请输入账号及密码等内容!");
                context.Response.End();
            }

            if (xtcu_no == null)
                xtcu_no = "";


            using (brCustomer br = new brCustomer())
            {
                cu_no=cu_no.Trim() ;
                customer mycust = br.Retrieve(cu_no);
                if (mycust != null)
                {
                    context.Response.Write("0|该账号已经存在，请重输!！");
                    context.Response.End();
                }


                string mycutype ="" ;
   

                if (cu_no.Length == 3)
                {
                    mycutype = "官方";
                }

                if (cu_no.Length == 6)
                {
                    mycutype = "总代";
                }

                if (cu_no.Length == 9)
                {
                    mycutype = "一级";
                }

                if (cu_no.Length == 12)
                {
                    mycutype = "二级";
                }

                if (cu_no.Length == 15)
                {
                    mycutype = "特约";
                }

                string strsql = "insert into customer(cu_no,cu_name,passwd,xtcu_no,unitcode,addr,remark,phone,cutype,checkyn,province,city) values('" + cu_no + "','" + cu_name + "','" + password + "','" + xtcu_no + "','" + "" + "','" + addr + "','" + remark + "','" + moble + "','" + mycutype + "','" + "Y" + "','" + province + "','" + city + "')";


                int ii = DbHelperSQL.ExecuteSql(strsql);


                if (ii > 0)
                {
                    context.Response.Write("0|OK|" );

                }
                else
                    context.Response.Write("0|注册失败，请检查输入的资料！");




            }


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
