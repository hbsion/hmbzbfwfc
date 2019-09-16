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
using brclass = marr.BusinessRule.brFgiScan;
using entity = marr.DataLayer.sal_fgi;
using marr.BusinessRule.Entity;
using System.Configuration;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppAdminFgi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string snno = context.Request["snno"];
            string gys_no = context.Request["gys_no"];
            string lot_no = context.Request["lot_no"];
            string p_no = context.Request["p_no"];
            string ship_no = context.Request["ship_no"];
            string mycu_no = context.Request["mycu_no"];
            string st_no = context.Request["st_no"];
 
            if (st_no == null)
                st_no = "";

            if (p_no == null)
                p_no = "";
            if (snno == null || snno.Length < 6)
            {
                context.Response.Write("0|请输入条码内容!");
                context.Response.End();
            }

            string strxt2dsplit = "?";


            int t = snno.LastIndexOf(strxt2dsplit);
            string mysnno = snno;
     

            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            //if (mysnno.Length > 30)
            //    mysnno = mysnno.Substring(0, 30);


            mysnno = mysnno.Replace("=", "");




            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            if (delyn == "Y")  //删除操作
            {

                using (brclass brtst = new brclass())
                {
                    entity saltst = brtst.Retrieve(mysnno);
                    if (saltst != null)
                    {
                        brtst.Delete(saltst.fid);

                         context.Response.Write("1|OK|删除成功!");
                         return ;
                    }
                    else
                    {
                       
                         context.Response.Write("0|物流码不存在");

                         return;
                    }

                }


   
            }

          

         
            int pqty = 0;
            int partqty = 1;
            string p_name = "";


            string lastcu_no = "";

            DateTime? lastTime = null;

            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);




            //Barcode barcode;
            //using (brBarcode brcode = new brBarcode())
            //{
            //    barcode = brcode.GetBarcodeEntity(mysnno, "");
            //    if (barcode != null)
            //    {
            //        pqty = barcode.Qty;
            //    }
            //}
            //if (pqty == 0 )   //不让扫描小瓶
            //{
            //    context.Response.Write("0|物流码不正确");
            //    context.Response.End();
            //    pqty=1;
            //}

            


            using (brProduct br = new brProduct())
            {
                Inv_Part myinvpart = br.Retrieve(p_no);
                if (myinvpart != null)
                {
                    p_name = myinvpart.pname;
                    partqty = (myinvpart.pqty ==null ? 1 : (int)myinvpart.pqty);

                }
                else
                {
                    context.Response.Write("0|入库产品不能为空！");
                    context.Response.End();
                }

            }

            using(brPackb bpb=new brPackb())
            {
                sal_packb pb = bpb.Retrieves(mysnno);
                if(pb!=null)
                {
                    context.Response.Write("0|该数码已包装入库！");
                    context.Response.End();
                }
            }
            
            
            entity newship = new entity();

            using (brclass brtst = new brclass())
            {
                entity saltst = brtst.Retrieve(mysnno);
                if (saltst != null)
                {
                    context.Response.Write("0|该条码已经入库！");
                    context.Response.End();
                }

            }

            using (brclass brcufgi = new brclass())
            {
                newship.p_no = p_no;
                newship.pname = p_name;

                newship.bsnno = mysnno;
                newship.lot_no = lot_no;


                newship.ship_date = System.DateTime.Now;
                newship.mqty = partqty;
                newship.loca = "";
                newship.upyn = 'N';
                newship.xtcu_no = "";
                
                newship.ship_no = ship_no;
                newship.unitcode = "";
                newship.st_no = st_no;
                newship.cu_no = "";
                newship.cu_name = "";
                newship.pda_no = mycu_no;
                newship.jf = 0;





                try
                {
                    brcufgi.Insert(newship);

                    context.Response.Write("1|OK|" + pqty.ToString());//1|OK|总数|已扫描数量|产品名称
                  
                    //int tt = DbHelperSQL.ExecuteSql("update sal_fgi  set pname=inv_part.pname   from inv_part  where  sal_fgi.p_no=inv_part.p_no and sal_fgi.pname =''");

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
