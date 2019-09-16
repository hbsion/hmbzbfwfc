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
using brclass = marr.BusinessRule.brCuReScan;
using entity = marr.DataLayer.sal_cure;
using marr.BusinessRule.Entity;


namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppCuRe : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string snno = context.Request["snno"];
            string remark = context.Request["remark"];
            string retype = context.Request["retype"];

            if (retype == null || retype.Length == 0)
                retype = "0";


            string p_no = "";
            string shipcu_no = "";
            string pname = "";
            string shipcu_name = "";
            string retmpstr = "";

        

            if (cu_no == null || snno == null || cu_no.Length == 0 || snno.Length < 8 )
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



            using (brCustomer br = new brCustomer())
            {
                customer mycust = br.Retrieve(cu_no);
                if  (mycust!=null)
                {
                    //if (DbHelperSQL.chkrepeat("sal_cure", "bsnno", mysnno,cu_no))
                    //{
                    //    context.Response.Write("0|该编号已经已经退货！");
                    //    context.Response.End();
                    //}



                    //物流码类型
                    Barcode barcode;
                    using (brBarcode brcode = new brBarcode())
                    {
                        barcode = brcode.GetBarcodeEntity(mysnno, ""); ;
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

                    using (brCuShipScan brtst = new brCuShipScan())
                    {
                        sal_cuship saltst = brtst.resale(barcode, cu_no);   //是否有销售
                        if (saltst != null)
                        {

                            p_no = saltst.p_no;
                            pname = saltst.pname;
                            shipcu_no = saltst.cu_no;
                            shipcu_name = saltst.cu_name;

                            retmpstr = "销售出货：产品：" + pname + " 出货客户：" + shipcu_name + " 出货日期：" + saltst.ship_date.ToString();

                            intship = 0;

                        }
                        else
                        {
                            using (brCuSaleScan brsale = new brCuSaleScan())
                            {
                                sal_cusale cusale = brsale.resale(barcode, cu_no);   //是否有销售
                                if (cusale != null)
                                {
                                    p_no = cusale.p_no;
                                    pname = cusale.pname;
                                    shipcu_no = cusale.cu_no;
                                    shipcu_name = cusale.cu_name;

                                    retmpstr = "终端销售：产品：" + pname + " 销售客户：" + shipcu_name + " 电话：" + cusale.chetel + " 车型：" + cusale.chetype + " 销售日期：" + cusale.ship_date.ToString();

                                    intship = 1;

                                }
                                else
                                {

                                    context.Response.Write("0|产品未出库或出库的客户不符");
                                    context.Response.End();
                                }

                            }
                        }
                    }

                    if (retype == "0")
                    {

                        context.Response.Write("1|OK|" + retmpstr);
                        context.Response.End();
                        return;
                    }



                    if (intship == 0)
                    {
                        int tt = DbHelperSQL.ExecuteSql("delete  sal_cuship   where bsnno='" + mysnno + "' and  xtcu_no='" + cu_no + "'" );

                    }

                    if (intship ==1)
                    {
                        int tt = DbHelperSQL.ExecuteSql("delete  sal_cusale   where bsnno='" + mysnno + "' and  xtcu_no='" + cu_no + "'");

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
                        newship.xtcu_no = cu_no;
                        newship.xtcu_name = mycust.cu_name;

                        newship.ship_no = System.DateTime.Now.ToString("yyyyMMdd");
                        newship.unitcode = "";
                        newship.st_no = "";
                        newship.remark = remark;

                        newship.jf = 0;

                        try
                        {
                            brcufgi.Insert(newship);

                            context.Response.Write("1|OK|" + newship.pname);
                    
                            int tt = DbHelperSQL.ExecuteSql("update sal_cure  set cu_name=customer.cu_name   from customer where  sal_cure.cu_no=customer.cu_no and sal_cure.cu_name =''");

                        }

                        catch (Exception ex)
                        {
                            context.Response.Write("0|error");

                        }



                    }



                }

                else
                {
                    context.Response.Write("0|账号错误！");
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
