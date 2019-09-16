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
using brclass = marr.BusinessRule.brCuShipScan;
using entity = marr.DataLayer.sal_cuship;
using marr.BusinessRule.Entity;


namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppCuShip : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string snno = context.Request["snno"];
            string remark = context.Request["remark"];
            string shipcu_no = context.Request["shipcu_no"];
   
            string p_no = "";

            string delyn = context.Request["delyn"];
            if (delyn == null)
            delyn = "N";


            if (delyn == "Y")  //删除操作
            {
                context.Response.Write("1|OK|删除成功!");
                context.Response.End();
            }

             



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
                      //  pqty = 1;
                       context.Response.Write("0|物流码不正确");
                      context.Response.End();
                    }


                    entity newship = new entity();
                    newship.p_no = "";
                    newship.pname = "";



                    using (brCuSaleScan brtst = new brCuSaleScan())
                    {
                        sal_cusale saltst = brtst.resale(barcode, cu_no);
                        if (saltst != null)
                        {
                            context.Response.Write("0|该编号已经销售！");
                            context.Response.End();
                        }

                    }

                    using (brCuShipScan brtst = new brCuShipScan())
                    {
                        sal_cuship saltst = brtst.resale(barcode, cu_no);
                        if (saltst != null)
                        {
                            context.Response.Write("0|该编号已经出货！");
                            context.Response.End();
                        }

                    }




                    //是否有入库

                    //using (brCuFgiScan brtst = new brCuFgiScan())
                    //{
                    //    sal_cufgi saltst = brtst.Retrieve(barcode, cu_no);
                    //    if (saltst != null)
                    //    {

                    //        lastTime = saltst.ship_date;
                    //        lastcu_no = saltst.cu_no;
                    //        if (newship.p_no.Length == 0)
                    //        {
                    //            newship.p_no = saltst.p_no;
                    //            newship.pname = saltst.pname;
                    //        }

                    //    }
                    //    else
                    //    {
                    //        context.Response.Write("0|产品未入库");
                    //        context.Response.End();
                    //    }


                    //}



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
                        newship.cu_no = shipcu_no ;
                        newship.cu_name = "";

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

                            int tt = DbHelperSQL.ExecuteSql("update sal_cuship  set cu_name=customer.cu_name   from customer where  sal_cuship.cu_no=customer.cu_no and sal_cuship.cu_name =''");

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
