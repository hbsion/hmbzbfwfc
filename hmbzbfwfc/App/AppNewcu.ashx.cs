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
using UI.hmtools;


namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppNewcu : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
            context.Response.Cache.SetNoStore();
            try
            {
                //Log.Debug("newcu", "aaaa");



                string xtcu_no = context.Request["xtcu_no"];

                string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);
                string password = context.Request["password"];
                string cu_name = context.Request["cu_name"];

                string addr = context.Request["addr"];
                string remark = context.Request["remark"];
                string mcode = context.Request["mcode"];


                string Wxid = context.Server.UrlDecode(context.Request["Wxid"]);
                string mySfz = context.Server.UrlDecode(context.Request["mySfz"]);

                string province = context.Request["province"];
                string city = context.Request["city"];

                string phone = context.Server.UrlDecode(context.Request["phone"]);
                string qq = context.Request["qq"];
                string taoid = context.Request["taoid"];
                string shitidian = context.Request["shitidian"];

                string icon = context.Request["icon"];

                string salloca = "";

                string zfb = context.Request["zfb"];
                string khh = context.Request["khh"];
                string yhzh = context.Request["yhzh"];

                string type = context.Request["type"];    //0 为自己注册，1为上级建立

                if (type == null)
                    type = "1";

                if (xtcu_no == null)
                    xtcu_no = "";


                string xtunitcode = "";
                string gclass = "";
                string xtName = "";
                string mybrand = "";

                string checkyn = "N";


                if (phone == null || phone.Length  <6)
                {
                    context.Response.Write("0|手机号码有误，请输入大于6位的手机号码!");
                    context.Response.End();
                }

                //if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[1]\d{10}$"))
                //{
                //    context.Response.Write("0|请输入正确的手机号码");
                //    context.Response.End();
                //}




                if (type == "0")   //注册
                {
                    //
                }
                else              //新增
                {

                    int myint = 0;
                    string myclass = "";


                     checkyn = "Y";


                    using (brCustomer br = new brCustomer())
                    {


                

                        customer mycust = br.Retrieve(cu_no);
                        if (mycust != null)
                        {

                            context.Response.Write("0|该账号已经存在，请重输");
                            context.Response.End();
                        }
                        else
                        {

                            customer xtCust = br.Retrieve(xtcu_no);
                            if (xtCust != null)
                            {
                                xtName = xtCust.cu_name;
                            }
                            string res_date = DateTime.Now.ToString();
                            string strsql = "insert into customer(cu_no,cu_name,passwd,xtcu_no,unitcode,addr,remark,phone,province,city,gclass,Wxid,mySfz,cutype,checkyn,taoid,shitidian,salloca,zfb,khh,yhzh,QQ,in_date,myHead,xtName) values('" + phone + "','" + cu_name + "','" + password.Trim() + "','" + xtcu_no + "','" + xtunitcode + "','" + addr + "','" + remark + "','" + phone + "','" + province + "','" + city + "','" + gclass + "','" + Wxid + "','" + mySfz + "','" + mybrand + "','" + checkyn + "','" + taoid + "','" + shitidian + "','" + salloca + "','" + zfb + "','" + khh + "','" + yhzh + "','" + qq + "','" + res_date + "','" + icon + "','" + xtName + "')";


                            Log.Debug("newcu", strsql);

                            int ii = DbHelperSQL.ExecuteSql(strsql);

                            if (ii > 0)
                            {
                                context.Response.Write("0|OK|" + xtunitcode);

                            }
                            else
                                context.Response.Write("0|注册失败，请检查输入的资料");

                            if (type == "0")   //注册
                            {

                                strsql = "update mbr_sell set ovyn='Y' where snno='" + mcode + "'";
                                ii = DbHelperSQL.ExecuteSql(strsql);
                            }


                        }


                    }
                }
            }




            catch (Exception ex)
            {

                Log.Error("appnew", ex.Message);

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
