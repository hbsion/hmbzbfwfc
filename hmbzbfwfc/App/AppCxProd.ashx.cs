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
    public class AppCxProd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";

            string cu_no = context.Request["cu_no"];
            string snno = context.Request["snno"];




            if (cu_no == null || snno == null || snno.Length < 8)
            {
                context.Response.Write("0|请输入条码内容!");
                context.Response.End();
            }

            int t = snno.LastIndexOf("?");
            string mysnno = snno;
            int pqty = 0;

            string lastcu_no = "";

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



            string p_no = "";
            string pname = "";


            //是否有出库

            using (brShipScan brtst = new brShipScan())
            {
                sal_ship saltst = brtst.Retrieve(barcode);
                if (saltst != null)
                {

                    lastTime = saltst.ship_date;
                    lastcu_no = saltst.cu_no.Trim() + "-" + saltst.cu_name ;
                    if (p_no.Length == 0)
                    {
                        p_no = saltst.p_no;
                        pname = saltst.pname;
                    }

                }
                else
                {
                    context.Response.Write("0|产品未出货");
                    context.Response.End();
                }


            }

            //多级经销商出货

            string cushipstr = "";

            using (brCuShipScan brb = new brCuShipScan())
            {

                foreach (var item in brb.Query(barcode, "", ""))
                {

                    cushipstr += "  出货日期：" + string.Format("{0:yyyy-MM-dd HH:mm}", item.ship_date) + "<br/> ";
                    cushipstr += "  发往客户：" + item.cu_no.Trim() +"-" +  item.cu_name + "<br/> ";
                    cushipstr += "  出货客户：" + item.xtcu_no + "<br/> ";

                }
            }

            using (brCuSaleScan brc = new brCuSaleScan())
            {

                sal_cusale mysale = brc.Retrieve(mysnno);

                if (mysale != null)
                {


                    cushipstr += "  终端销售日期：" + string.Format("{0:yyyy-MM-dd HH:mm}", mysale.ship_date) + "<br/> ";

                    cushipstr += "  发往客户：" + mysale.cu_name + "<br/> ";
                    cushipstr += "  客户电话：" + mysale.chetel + "<br/> ";
                    cushipstr += "  销售地址：" + mysale.chetype + "<br/> ";
                    cushipstr += "  备注说明：" + mysale.remark + "<br/> ";

                }
            }


            if (cu_no.Length == 0 || cushipstr.LastIndexOf(cu_no) >= 0 || lastcu_no.LastIndexOf(cu_no) >=0)
            {
                context.Response.Write("1|OK|条码：" + mysnno + "<br/> 产品编号：" + p_no + "<br/> 产品名称：" + pname + "<br/> 总部出货日期：" + lastTime.ToString() + "<br/> 经销商：" + lastcu_no + "<br/> " + cushipstr);

            }
            else
            {
                context.Response.Write("0|条码：" + mysnno +",您无权查询此产品的信息!");
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
