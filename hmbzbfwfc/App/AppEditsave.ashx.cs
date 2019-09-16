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
    public class AppEditsave : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
 
            string addr = context.Request["addr"];
            //   string phone = context.Request["phone"];
            string mobile = context.Request["mobile"];

            string myHead = "";



            String path = context.Server.MapPath(".");

            string mypath = path + "\\cuimg\\" + cu_no;

            //  if (File.Exists(mypath + "\\headimg.jpg"))

            myHead = "/cuimg/" + cu_no + "/headimg.jpg";

            if (cu_no == null )
            {
                context.Response.Write("请输入账号及密码等内容!");
                context.Response.End();
            }



            using (brCustomer br = new brCustomer())
            {
                customer mycust = br.Retrieve(cu_no);

                if (mycust != null)
                {
                    mycust.addr = addr;
            //        mycust.mobile = mobile;
             //       mycust.myHead = myHead;
                    br.Update(mycust);

                    context.Response.Write("OK");

                }
                else
                {
                    context.Response.Write("注册失败，请检查输入的资料！");
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
