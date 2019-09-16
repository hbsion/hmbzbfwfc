using hmbzbfwfc.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Net.Http;
using System.Web.Http;
using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;

namespace hmbzbfwfc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {



            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //错误日志
            string filePath = Server.MapPath("/Log/");
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    //判断一下队列中是否有数据
                    if (LogAttribute.execptionQueue.Count > 0)
                    {
                        //出队
                        Exception ex = LogAttribute.execptionQueue.Dequeue();
                        if (ex != null)
                        {
                            //将异常信息写到日志文件中
                            string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                            File.AppendAllText(filePath + fileName + ".txt", ex.ToString() + "\r\n----------------------------" + DateTime.Now + "------------------------------\r\n", System.Text.Encoding.UTF8);
                        }
                        else
                        {
                            //如果队列中没有数据，休息5秒钟
                            Thread.Sleep(5000);
                        }
                    }
                    else
                    {
                        //如果队列中没有数据，休息
                        Thread.Sleep(5000);
                    }
                }
            });

        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            //测试数据库

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            ;
            try
            {

                var model = dbHelpers.Gy_Czygl.FirstOrDefault(db => db.fid == 0);

            }
            catch(Exception ex)
            {
                Response.Write("<font color=red>无法打开数据库，请正确配置数据库服务器。</font>" + ex.Message);
                Response.End();
            }

            //MyConn.Close();


        }


    }
}
