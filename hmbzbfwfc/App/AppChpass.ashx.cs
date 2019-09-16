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
    public class AppChpass : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string gadmin = context.Request["gadmin"];
     
            string opass = context.Request["opass"];
            string npass = context.Request["npass"];

            if (cu_no == null || opass == null || cu_no.Length == 0 || opass.Length == 0)
            {
                context.Response.Write("请输入内容!");
                context.Response.End();
            }


            if (cu_no.Length==0)  //总部
           {

               using (brCzygl br = new brCzygl())
               {
                    string  mystring="";

         
                   if (br.ChangPassWd(gadmin, opass, npass, out mystring))
                   {
                       context.Response.Write("1|密码修改成功");

                   }
                   else
                   {
                       context.Response.Write("0|" + mystring);
                   }

               }

            }

            else
            {

                using (brCustomer br = new brCustomer())
                {
                    customer mycust = br.Retrieve(cu_no);

                    if (mycust != null)
                    {
                        if (opass == mycust.passwd)
                        {
                            mycust.passwd = npass;
                            br.Update(mycust);
                            context.Response.Write("OK");
                        }
                        else
                        {
                            context.Response.Write("旧密码错误！");
                        }


                    }

                    else
                    {
                        context.Response.Write("账号错误！");
                    }
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
