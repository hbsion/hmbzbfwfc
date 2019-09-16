using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using marr.BusinessRule;
using brclass = marr.BusinessRule.brCorder;
using entity = marr.DataLayer.sal_corder;
using brcontext = marr.BusinessRule.brCorder.QueryContext;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppCustomer_edit : IHttpHandler
    {
        //代发货上级经销商浏览
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string opid = context.Request["opid"];
            string chestr = context.Request["chestr"];



                string[] asnn = chestr.Split('|');


                for (int s = 0; s <= asnn.Length - 1; s++)
                {
                    if (opid == "0")  //删除
                    {

                     int tt = DbHelperSQL.ExecuteSql("delete   customer  where fid='" + asnn[s] + "'" );

                    }

                    else
                    {
                        int tt2 = DbHelperSQL.ExecuteSql("update customer set checkyn='Y'    where fid='" + asnn[s] + "'");

                      }

                }

              context.Response.Write("OK");


      
  
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
