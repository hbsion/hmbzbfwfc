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

namespace UI.wxws.wxAPI
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class queryAPI : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8";
            context.Response.Cache.SetNoStore();

           
            string action = context.Request["action"];
            string cu_no = context.Request["cu_no"];//经销商编号，如有值，则是代理商，若为null,则是总部直属经销商
            string searchText = context.Request["searchText"];//搜索文本
            string Page = context.Request["page"];//页数0,1,2,3,4
            string type = context.Request["type"];//审核状态:0:未审核,1已审核

            if (type == null)
                type = "1";
            if (cu_no == null)
                cu_no = "";

            using (brCustomer oblist = new brCustomer())
            {
                if (Page == null || Page == "")//单页显示
                {
                    string json = "[";
                    int num = 0;
                    foreach (var item in oblist.Query(cu_no, searchText, ""))
                    {
                       
                        if (num > 0)
                        { json += ","; }
                        string bjson = "[";

                        bjson += "]";
                        json += "{\"title\":\"" + item.cu_name.Trim() + "\",\"gclass\":\"" +"" + "\",\"id\":\"" + item.cu_no.Trim() + "\",\"wxid\":\"" + "" + "\",\"phone\":\"" + item.phone.Trim() + "\",\"addr\":\"" + item.province + item.city + item.addr.Trim() + "\",\"fid\":\"" + item.fid + "\",\"authCode\":\"A" + item.fid + "\",\"brands\":" + bjson + "}";
                        num++;
                    }
                    json += "]";
                    string success = "{\"success\":true,\"Messages\":\"\",\"lists\":" + json + "}";
                    context.Response.Write(success);
                    context.Response.End();
                }
                else
                {//分页显示
                    int mypage = 0;
                    try {
                        mypage = int.Parse(Page);
                    }
                    catch { }
                    brCustomer.QueryContext Querycontext = new brCustomer.QueryContext();
                    Querycontext.PageIndex = mypage;//第几页
                    Querycontext.PageSize = 5;

                    if (cu_no.Length == 0)
                        Querycontext.xtcu_no = "";
                    else
                        Querycontext.xtcu_no = cu_no;
           

                    int maxItemCount = oblist.Count(Querycontext);//记录总数
                    if (maxItemCount == 0)
                    {
                        context.Response.Write("{\"success\":false,\"Messages\":\"没有记录数据\"}");
                        context.Response.End();
                    }
                    int maxPageIndex = (maxItemCount - 1) / Querycontext.PageSize;//总页数
                    if (maxPageIndex < 0)
                        maxPageIndex = 0;
                    if (Querycontext.PageIndex < 0)
                        Querycontext.PageIndex = 0;
                    if (Querycontext.PageIndex > maxPageIndex)
                    {
                        context.Response.Write("{\"success\":false,\"Messages\":\"没有更多数据了!\"}");
                        context.Response.End();
                        //Querycontext.PageIndex = maxPageIndex;
                    }

                    Querycontext.seartext = searchText;//搜索文本

                    string json = "[";
                    int num = 0;
                    foreach(var item in oblist.Query(Querycontext))
                    {
                        
                        if (num > 0)
                            json += ",";
                        string bjson = "[";

                        bjson += "]";

                        json += "{\"title\":\"" + item.cu_name.Trim() + "\",\"gclass\":\"" + "" + "\",\"id\":\"" + item.cu_no.Trim() + "\",\"wxid\":\"" + "" + "\",\"phone\":\"" + item.phone + "\",\"addr\":\"" + item.addr + "\",\"fid\":\"" + item.fid + "\",\"authCode\":\"A" + item.fid + "\",\"brands\":" + bjson + "}";
                        num++;
                    }
                    json += "]";
                    string success = "{\"success\":true,\"Messages\":\"\",\"lists\":"+json+"}";
                    context.Response.Write(success);
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
