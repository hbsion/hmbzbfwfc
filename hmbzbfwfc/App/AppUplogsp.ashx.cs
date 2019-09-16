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
using System.Text;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppUplogsp : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];


            string scode = context.Server.UrlDecode(context.Request["scode"]);

         

            string txtitem = context.Request["item"];

            Log.Info("cuNo|scode|item", cu_no + "|" + scode + "|" + txtitem);

            if (cu_no == null)
                cu_no = "";

            if (txtitem == null || txtitem == "")//单页显示
            {
                string strjason = "[";

                using (brSalPack br = new brSalPack())
                {
                    int i = 0;
                    foreach (var item in br.QueryS(cu_no, scode))
                    {
                        if (i > 0)
                        {
                            strjason += ",";
                        }

                        string strstus = "未处理";
                        if (item.status == 1)
                        {
                            strstus = "正在处理...";
                        }

                        if (item.status == 9)
                        {
                            strstus = "处理完成!";
                        }


                        strjason += "{\"title\":\"" + item.uplog.Replace("\r\n","") + "\",\"id\":\"" + Convert.ToDateTime(item.pack_date).ToString("yy-MM-dd HH:mm") + "\",\"status\":\"" + strstus + "\",\"fid\":\"" + item.fid + "\"}";
                        i++;
                    }
                }
                strjason += "]";

                context.Response.Write(strjason);
                context.Response.End();
            }
            else//分页显示
            {
                int INTitem = 0;
                try
                {
                    INTitem = int.Parse(txtitem);
                }
                catch { }

                using (brSalPack br = new brSalPack())
                {
                    brSalPack.QueryContext QueryContext = new brSalPack.QueryContext();
                    QueryContext.PageIndex = INTitem;
                    QueryContext.PageSize = 15;

         
                     QueryContext.cu_no = cu_no;


                     QueryContext.seartext = scode;



                    int maxItemCount = br.Count(QueryContext);
                    if (maxItemCount == 0)
                    {
                        context.Response.Write("");
                        context.Response.End();
                    }
                    int maxPageIndex = (maxItemCount - 1) / QueryContext.PageSize;
                    if (maxPageIndex < 0) maxPageIndex = 0;
                    if (QueryContext.PageIndex < 0) QueryContext.PageIndex = 0;
                    if (QueryContext.PageIndex > maxPageIndex)
                    {
                        context.Response.Write("");
                        context.Response.End();
                        QueryContext.PageIndex = maxPageIndex;
                    }
                    string strjason = "[";


                    int i = 0;
                    foreach (var item in br.Query(QueryContext))
                    {
                        if (i > 0)
                        {
                            strjason += ",";
                        }

                        string strstus = "未处理";
                        if (item.status == 1)
                        {
                            strstus = "正在处理...";
                        }

                        if (item.status == 9)
                        {
                            strstus = "处理完成!";
                        }


                        strjason += "{\"title\":\"" + item.uplog.Replace("\r\n", "") + "\",\"id\":\"" + Convert.ToDateTime(item.pack_date).ToString("yy-MM-dd HH:mm") + "\",\"status\":\"" + strstus + "\",\"fid\":\"" + item.fid + "\"}";
                       
                        i++;
                    }

                    strjason += "]";

                    context.Response.Write(strjason);
                    context.Response.End();
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
