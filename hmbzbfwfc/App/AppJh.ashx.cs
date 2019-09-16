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
    public class AppJh : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string snno = context.Request["snno"];
            string p_no = context.Request["p_no"];

            if (p_no == null)
                p_no = "";

            string p_name = "";

            using (brProduct br = new brProduct())
            {
                Inv_Part myinvpart = br.Retrieve(p_no);
                if (myinvpart != null)
                {
                    p_name = myinvpart.pname;

                }


            }
        


            if (cu_no == null || snno == null || cu_no.Length == 0 || snno.Length == 0)
            {
                context.Response.Write("请输入账号及条码内容!");
                context.Response.End();
            }

            int t = snno.LastIndexOf("?");
            string mysnno = snno;

            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);


            using (brCustomer br = new brCustomer())
            {
                customer mycust = br.Retrieve(cu_no);
                if  (mycust!=null)
                {
                    if (DbHelperSQL.chkrepeat("sal_jh", "bsnno", mysnno))
                    {
                        context.Response.Write("该编号已经激活!！");
                        context.Response.End();
                    }



                    string strsql = "insert into sal_jh(bsnno,cu_no,cu_name,esnno,p_no,pname) values('" + mysnno + "','" + cu_no + "','" + mycust.cu_name + "','" + mysnno + "','" + p_no + "','" + p_name +"')";
                       
                    int ii = DbHelperSQL.ExecuteSql(strsql);

                    context.Response.Write("OK");

                }

                else
                {
                    context.Response.Write("账号错误！");
                }


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
