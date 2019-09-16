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
    public class appCustomerall : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];


            string scode = context.Server.UrlDecode(context.Request["scode"]);

         

            string txtitem = context.Request["item"];

            string mytype = context.Request["type"];   //分类，0：未审核，1，已经审核

            if (mytype == null)
                mytype = "1";


            if (cu_no == null)
                cu_no = "";

            if (txtitem == null || txtitem == "")//单页显示
            {
                string strjason = "[";

                using (brCustomer br = new brCustomer())
                {
                    int i = 0;
                    foreach (var item in br.QueryS("-1", scode, mytype,""))
                    {
                        if (i > 0)
                        {
                            strjason += ",";
                        }
                        strjason += "{\"title\":\"" + item.cu_name.Trim() + "\",\"id\":\"" + item.cu_no.Trim() + "\",\"wxid\":\"" + ""  + "\",\"phone\":\"" + item.phone.Trim() + "\",\"addr\":\"" +   item.addr.Trim() + "\",\"fid\":\"" + item.fid + "\"}";
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

                using (brCustomer br = new brCustomer())
                {
                    brCustomer.QueryContext QueryContext = new brCustomer.QueryContext();
                    QueryContext.PageIndex = INTitem;
                    QueryContext.PageSize = 15;
             
                    if  (cu_no.Length==0)
                        QueryContext.xtcu_no = "-1" ;  //全部
                    else
                        QueryContext.xtcu_no = cu_no;


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
                        strjason += "{\"title\":\"" + item.cu_name.Trim() + "\",\"id\":\"" + item.cu_no.Trim() + "\",\"wxid\":\"" + ""  + "\",\"phone\":\"" + item.phone.Trim() + "\",\"addr\":\"" + item.addr.Trim() + "\",\"fid\":\"" + item.fid + "\"}";
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

        private void WriteLog(string strMemo)
        {

            string filename = "e:/WEBHOME/logs/log.txt";
            if (!System.IO.Directory.Exists("e:/WEBHOME/logs/"))
                System.IO.Directory.CreateDirectory("e:/WEBHOME/logs/");

            System.IO.StreamWriter sr = null;
            try
            {
                if (!System.IO.File.Exists(filename))
                {
                    sr = System.IO.File.CreateText(filename);
                }
                else
                {
                    sr = System.IO.File.AppendText(filename);
                }
                sr.WriteLine(strMemo);
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
    }
}
