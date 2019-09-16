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
using brclass = marr.BusinessRule.brShipScan;
using entity = marr.DataLayer.sal_ship;
using marr.BusinessRule.Entity;
using hmbzbfwfc.Commons;


namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppAdminShip : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string snno = context.Request["snno"];
            string remark = context.Request["remark"];
            string shipcu_no = context.Request["shipcu_no"];
            string po_no = context.Request["po_no"];
            string p_no =context.Request["p_no"];
            string mycu_no = context.Request["mycu_no"];
            string ship_no = context.Request["ship_no"];
            string p_name = "";

            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            if (delyn == "Y")  //删除操作
            {
                if (cu_no == "")
                {
                    using (brShipScan brtst = new brShipScan())
                    {
                        sal_ship saltst = brtst.Retrieve(snno);
                        if (saltst != null)
                        {
                            brtst.Delete(saltst.fid);

                            context.Response.Write("1|OK|删除成功");
                            return;
                        }
                        else
                        {

                            context.Response.Write("0|物流码不存在");
                            return;
                        }

                    }
                }
                else
                {
                    using (brCuShipScan brtst = new brCuShipScan())
                    {
                        sal_cuship saltst = brtst.Retrieve(snno, cu_no);
                        if (saltst != null)
                        {
                            brtst.Delete(saltst.fid);

                            context.Response.Write("1|OK|删除成功");
                            return;
                        }
                        else
                        {

                            context.Response.Write("0|物流码不存在");
                            return;
                        }

                    }

                }
            }



            //using (brProduct br = new brProduct())
            //{
            //    Inv_Part myinvpart = br.Retrieve(p_no);
            //    if (myinvpart != null)
            //    {
            //        p_name = myinvpart.pname;

            //    }


            //}
        

            if ( snno == null  || snno.Length < 7 )
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

            string st_no = "";


            using (brPackb brfgi = new brPackb())
            {
                sal_packb salpack = brfgi.Retrieves(mysnno);
                if (salpack != null)
                {
                    if (!string.IsNullOrEmpty(p_no) )
                    {
                        if (p_no != salpack.p_no)
                        {
                            context.Response.Write("0|出库产品与包装入库产品不一致！");
                            return;
                        }
                        
                    }
                    else
                    {
                        p_no = salpack.p_no;
                        p_name = DataEnum.GetPName(salpack.p_no);
                    }
                }                
            }

            using (brFgiScan bf = new brFgiScan())
            {
                sal_fgi fgi = bf.Retrieve(mysnno);
                if (fgi != null)
                {
                    if (!string.IsNullOrEmpty(p_no))
                    {
                        if (p_no != fgi.p_no)
                        {
                            context.Response.Write("0|出库产品与入库产品不一致！");
                            return;
                        }

                    }
                    else
                    {
                        p_no = fgi.p_no;
                        p_name = DataEnum.GetPName(fgi.p_no);
                    }
                }
            }

            using (brCustomer br = new brCustomer())
            {
                customer mycust = br.Retrieve(shipcu_no);
                if  (mycust!=null)
                {
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
                        using (brFgiScan brp = new brFgiScan())
                        {
                            sal_fgi fgi = brp.Retrieve(mysnno);
                            if (fgi != null)
                            {
                                pqty = Convert.ToInt32(fgi.mqty);

                            }
                            else
                            {
                                context.Response.Write("0|物流码不正确");
                                context.Response.End();
                            }
                        }

                       
                    }

                    entity newship = new entity();
      
                    DateTime? redate=null;
                    int isre = 0;
                    using(brReScan brs =new brReScan())
                    {
                        sal_re sr = brs.Retrieve(barcode);
                        if (sr != null)
                        {
                            redate = sr.ship_date;
                            isre = 1;
                        }
                    }

                    using (brShipScan brtst = new brShipScan())
                    {
                        sal_ship saltst = brtst.Retrieve(barcode,redate);
                        if (saltst != null)
                        {
                            context.Response.Write("0|该编号已经出货！");
                            context.Response.End();
                        }

                    }

                    
      
                    using (brclass brcufgi = new brclass())
                    {
                        newship.p_no = p_no ;
                        newship.pname = p_name;

                        newship.bsnno = mysnno;
                        newship.cu_no = shipcu_no;
                        newship.cu_name = "";

                        newship.ship_date = System.DateTime.Now;
                        newship.mqty = pqty;
                        newship.loca = "";
                        newship.upyn = 'N';
                        newship.xtcu_no = "";
                        newship.xtcu_name = "";

                        newship.ship_no = ship_no;
                        newship.unitcode = "";
                        newship.st_no = st_no;
                        newship.remark = remark;
                        newship.pda_no = mycu_no;
                        newship.po_no = po_no;

                        newship.jf = 0;

                        try
                        {
                            brcufgi.Insert(newship);

                            context.Response.Write("1|OK|" +pqty.ToString()  );//1|OK|总数|已扫描数量|产品名称

                            if(isre==1)
                            {
                                int re=DbHelperSQL.ExecuteSql("update sal_re set upyn='S' where bsnno='"+mysnno+"'");
                            }

                            int tt = DbHelperSQL.ExecuteSql("update sal_ship  set cu_name=customer.cu_name   from customer where  sal_ship.cu_no=customer.cu_no and sal_ship.cu_name =''");
                                tt = DbHelperSQL.ExecuteSql("update sal_ship  set pname=inv_part.pname   from inv_part  where  sal_ship.p_no=inv_part.p_no and sal_ship.pname =''");
                            
                        }

                        catch (Exception ex)
                        {
                            context.Response.Write("0|error");
                        }
                    }
           
                }

                else
                {
                    context.Response.Write("0|客户编号错误！");
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
