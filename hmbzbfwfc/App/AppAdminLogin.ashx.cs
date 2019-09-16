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
    public class AppAdminLogin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
    
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string password = context.Request["password"];

            //if (cu_no == null)
            //    cu_no = "11111";

            //context.Response.Write(cu_no);
            //context.Response.End();

            if (cu_no == null || password == null || cu_no.Length == 0 || password.Length == 0)
            {
                context.Response.Write("0|请输入账号和密码");
                context.Response.End();
            }


            using (brCzygl br = new brCzygl())
            {
                Gy_Czygl user = null;
                if (br.CheckLogin(cu_no, password, out user))
                {
                    string myright="00000";
                    if (user.deptname!=null &&  user.deptname.Length>=5)
                    {
                        myright=  user.deptname;
                    }

                    context.Response.Write("1|"+ myright);

                }
                else
                {
                    context.Response.Write("0|用户名/密码错误");
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
