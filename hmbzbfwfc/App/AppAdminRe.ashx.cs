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
using brclass = marr.BusinessRule.brReScan;
using entity = marr.DataLayer.sal_re;
using marr.BusinessRule.Entity;


namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppAdminRe : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string snno = context.Request["snno"];
            string remark = context.Request["remark"];
            string retype = context.Request["retype"];
            string mycu_no = context.Request["mycu_no"];
            if (retype == null || retype.Length == 0)
                retype = "0";


            string p_no = "";
            string shipcu_no = "";
            string pname = "";
            string shipcu_name = "";
            string retmpstr = "";

        

            if ( snno == null  || snno.Length < 7 )
            {
                context.Response.Write("0|请输入条码内容!");
                context.Response.End();
            }

            int t = snno.LastIndexOf("?");
            string mysnno = snno;
            int pqty = 0;

            int intship = 0;

            string  lastcu_no = "";

            DateTime? lastTime = null;

            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);



 

                    //物流码类型
                    Barcode barcode;
                    using (brBarcode brcode = new brBarcode())
                    {
                        barcode = brcode.GetBarcodeEntity(mysnno,  ""); ;
                        if (barcode != null)
                        {
                            pqty = barcode.Qty;
                        }
                    }

                    if (pqty == 0)
                    {
                        context.Response.Write("0|物流码不正确");
                        context.Response.End();
                    }

                    

                   
                    //是否有出库
                    DateTime? shipdate=null;
                    using (brShipScan brtst = new brShipScan())
                    {
                        sal_ship saltst = brtst.Retrievetui(barcode);   //是否有销售
                        if (saltst != null)
                        {

                            p_no = saltst.p_no;
                            pname = saltst.pname;
                            shipcu_no = saltst.cu_no;
                            shipcu_name = saltst.cu_name;

                            retmpstr = "出货：产品：" + pname + " 出货客户：" + shipcu_name + " 出货日期：" + saltst.ship_date.ToString();

                            intship = 1;
                            shipdate=saltst.ship_date;
                        }
                        else
                        {
       
                            context.Response.Write("0|产品未出库或出库的客户不符");
                            context.Response.End();                             
                          
                        }
                    }

                    using (brReScan br = new brReScan())
                    {
                        if (br.Retrieve(barcode,shipdate) != null)
                        {
                            context.Response.Write("0|重复退库！");
                            return;
                        }
                    }

                    if (retype == "0")
                    {

                        context.Response.Write("1|OK|" + retmpstr);
                        context.Response.End();
                        return;
                    }



                    if (intship == 1)
                    {
                        int tt = DbHelperSQL.ExecuteSql("update  sal_ship  set upyn='R' where bsnno='" + mysnno + "'" );

                    }

  

                    using (brclass brcufgi = new brclass())
                    {
                        entity newship = new entity();
                        newship.p_no = p_no ;
                        newship.pname = pname ;

                        newship.bsnno = mysnno;
                        newship.cu_no = shipcu_no;
                        newship.cu_name = shipcu_name;

                        newship.ship_date = System.DateTime.Now;
                        newship.mqty = pqty;
                        newship.loca = "";
                        newship.upyn = 'N';
                        newship.xtcu_no = "";
                        newship.xtcu_name = "";
                        newship.pda_no = mycu_no;
                        newship.ship_no = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                        newship.unitcode = "";
                        newship.st_no = "";
                        //newship.remark = remark;

                        newship.jf = 0;

                        try
                        {
                            brcufgi.Insert(newship);

                            context.Response.Write("1|OK|" + newship.pname);
                    
                          //  int tt = DbHelperSQL.ExecuteSql("update sal_re  set cu_name=customer.cu_name   from customer where  sal_re.cu_no=customer.cu_no and sal_re.cu_name =''");

                        }

                        catch (Exception ex)
                        {
                            context.Response.Write("0|error");

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
