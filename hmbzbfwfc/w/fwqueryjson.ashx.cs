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
using loginuser = marr.BusinessRule.Entity.User;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.SessionState;
using marr.BusinessRule.Entity;

namespace UI
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class fwqueryjson : IHttpHandler, IRequiresSessionState
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
    

            string fwcode = context.Request["fwcode"];   //防伪码
            string address = context.Request["address"];

            string lat = context.Request["lat"];
            string lng = context.Request["lng"];

            if (lat == null)
                lat = "";

            if (lng == null)
                lng = "";

            if (address == null)
                address = "";


            if (fwcode == null)
                fwcode = "";

            string unitcode="--";
            string fcxaddress = "";
            string fcxlat = "";
            string fcxlng = "";


   

            address = address.Replace("查询地址：", "");

            using (brBarcode br = new brBarcode())
            {

                Fwcode myfwcode = null;
           

                if  (context.Session["fwtype"]!=null &&   context.Session["fwtype"].ToString()=="2")  //类型  防伪码查
                {
                      myfwcode = br.refwcode(fwcode.Trim());

                }
                else
                {
                    myfwcode = br.refccode(fwcode.Trim());
                }

                if  (myfwcode == null)
                {
                    return;
                }
        
                Boolean cxre = true;   //是否记录

                unitcode = myfwcode.unitcode;
   

                if (cxre == true)
                {
                    string IP = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
  

                    using (brTellist brt = new brTellist())
                    {
                        tellist telist = brt.RetrieveB(fwcode.Trim());
                        if (telist == null)
                        {

                            string sql = "Insert into tellist(UnitCode,FWCode,Querystatu,callerid,latitude,longitude,loca,qutype)  Values('";
                            sql = sql + unitcode + "','";
                            sql = sql + fwcode.Trim() + "','";
                            sql = sql + GetcxDesc(myfwcode.findresut) + "','";
                            sql = sql + IP + "','";
                            sql = sql + lat + "','";
                            sql = sql + lng + "','";
                            sql = sql + address + "','3')";

                            int ii = DbHelperSQL.ExecuteSql(sql);

                        }
                    }
                                


  
                }



               string   cxState=myfwcode.findresut.ToString() ;
 

               string  callback =context.Request["callback"];


               string llist = "[";

               using (brTellist brtel = new brTellist())
               {

                   int intnum = 0;

                   foreach (var item in brtel.Query(fwcode.Trim(), ""))
                   {

                       if (intnum > 0)
                       {
                           llist += ",";
                       }
                       else //第一条
                       {
                           if  (item.loca!=null)
                           fcxaddress = item.loca;

                           if (item.latitude != null)
                           fcxlat = item.latitude;

                           if (item.longitude != null)
                           fcxlng = item.longitude;

                       }
                       llist += "{\"in_date\":\"" + item.QueryDate.ToString() + "\",\"tel\":\"" + item.callerid + "\"}";
                       intnum++;


                       if (intnum > 10)
                       {
                           break;
                       }


                   }

               }

               llist += "]";

               //基本次料
               string unitname = "";
               string brand = "";


               context.Response.Write(callback + "({" + "\"QueryResult\":\"" + myfwcode.resnote + "\"," + "\"CodeState\":\"" + cxState + "\"," + "\"loca\":\"" + fcxaddress + "\"," + "\"lng\":\"" + fcxlng + "\"," + "\"lat\":\"" + fcxlat + "\"," + "\"cxcs\":\"" + "0" + "\"," + "\"unitname\":\"" + unitname + "\"," + "\"brand\":\"" + brand + "\"," + "\"llist\":" + llist + "})");  


            }


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        protected string GetcxDesc(int status)
        {
            switch (status)
            {
                case 0:
                    return "错误";
                case 1:
                    return "正确";
                case 2:
                    return "重复";

                default:
                    return "错误";
            }
        }
    }
}
