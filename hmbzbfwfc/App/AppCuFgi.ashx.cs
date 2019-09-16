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
using brclass = marr.BusinessRule.brCuFgiScan;
using entity = marr.DataLayer.sal_cufgi;
using marr.BusinessRule.Entity;


namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppCuFgi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string snno = context.Request["snno"];
            string remark = context.Request["remark"];

            string p_no = "";

            //if (p_no == null)
            //    p_no = "";

            //string p_name = "";

            //using (brProduct br = new brProduct())
            //{
            //    Inv_Part myinvpart = br.Retrieve(p_no);
            //    if (myinvpart != null)
            //    {
            //        p_name = myinvpart.pname;

            //    }


            //}
        

            if (cu_no == null || snno == null || cu_no.Length == 0 || snno.Length < 8 )
            {
                context.Response.Write("0|请输入条码内容!");
                context.Response.End();
            }

            int t = snno.LastIndexOf("?");
            string mysnno = snno;
            int pqty = 0;

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
                    if (DbHelperSQL.chkrepeat("sal_cufgi", "bsnno", mysnno,cu_no))
                    {
                        context.Response.Write("0|该编号已经已经入库！");
                        context.Response.End();
                    }



                    //物流码类型
                    Barcode barcode;
                    using (brBarcode brcode = new brBarcode())
                    {
                        barcode = brcode.GetBarcodeEntity(mysnno, 3, "", ""); 
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


                    entity newship = new entity();
                    newship.p_no = "";
                    newship.pname = "";


                    //是否有出库

                    using (brShipScan brtst = new brShipScan())
                    {
                        sal_ship saltst = brtst.Retship(barcode, cu_no);  //总部是否出货
                        if (saltst != null)
                        {

                            lastTime = saltst.ship_date;
                            lastcu_no = saltst.cu_no;
                            if (newship.p_no.Length == 0)
                            {
                                newship.p_no = saltst.p_no;
                                newship.pname = saltst.pname;
                            }

                        }
                        else
                        {
                            using (brCuShipScan brcutst = new brCuShipScan())
                            {

                                sal_cuship cusaltst = brcutst.RetrieveB(barcode, cu_no);  //上级是否出货
                                if (cusaltst != null)
                                {
                                    lastTime = cusaltst.ship_date;
                                    lastcu_no = cusaltst.cu_no;
                                    if (newship.p_no.Length == 0)
                                    {
                                        newship.p_no = cusaltst.p_no;
                                        newship.pname = cusaltst.pname;
                                    }

                                }
                                else
                                {

                                    context.Response.Write("0|产品未出货，或出货客户不符");
                                    context.Response.End();
                                }
                            }

                        }


                    }



                    using (brclass brcufgi = new brclass())
                    {

                        newship.bsnno = mysnno;
                        newship.cu_no = cu_no ;
                        newship.cu_name = mycust.cu_name;

                        newship.ship_date = System.DateTime.Now;
                        newship.mqty = pqty;
                        newship.loca = "";
                        newship.upyn = "N";
                        newship.xtcu_no = cu_no;
                        newship.xtcu_name = mycust.cu_name;

                        newship.ship_no = System.DateTime.Now.ToString("yyyyMMdd");
                        newship.unitcode ="";
                        newship.st_no = "";
                        newship.remark = remark;

                        newship.jf = 0;

                        try
                        {
                            brcufgi.Insert(newship);

 
                            context.Response.Write("1|OK|" + newship.pname);
                        
     

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
