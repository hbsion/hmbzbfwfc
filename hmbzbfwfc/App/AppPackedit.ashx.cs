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
using System.Configuration;


namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AppPackedit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.Cache.SetNoStore();
                string cu_no = context.Request["cu_no"];
                string pack_no = context.Request["pack_no"];
                string snno = context.Request["snno"];   //产品码
                string usnno = context.Request["usnno"]; //箱码
                string lot_no = context.Request["lot_no"];//批号
                string line_no = context.Request["line_no"];//线号
                string p_no = context.Request["p_no"];    //产品编号
                string in_date = context.Request["in_date"];
                string fc_no = "";   

                string delyn = context.Request["delyn"];

                string strpqty = context.Request["pqty"];

                int mypqy = 0;

                if (delyn == null)
                    delyn = "N";


                if (strpqty == null || strpqty.Length == 0)
                    strpqty = "0";

                mypqy = int.Parse(strpqty);



                if (usnno == null || snno == null || usnno.Length == 0 || snno.Length < 8)
                {
                    context.Response.Write("0|请输入内容!");
                    context.Response.End();
                }


                string strxt2dsplit = ConfigurationSettings.AppSettings["xt2dsplit"].ToString();


                int t = snno.LastIndexOf(strxt2dsplit);

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


                int z = usnno.LastIndexOf(strxt2dsplit);


                if (z > 0 && usnno.Length > (z + 1))
                {
                    usnno = usnno.Substring(z + 1, usnno.Length - z - 1);
                }

                if (usnno.Length > 30)
                    usnno = usnno.Substring(0, 30);


                if (delyn == "Y")  //删除操作
                {
                    using (brPackb brp = new brPackb())
                    {
                        sal_packb en = brp.Retrieve(mysnno);
                        if (en != null)
                        {
                            using(brShipScan bship=new brShipScan())
                            {
                                if( bship.reisship(mysnno))
                                {
                                    context.Response.Write("0|条码" + mysnno + "已出货!");
                                    context.Response.End();
                                }
                            }

                            brp.Delete(en.fid);
                            context.Response.Write("1|OK|删除成功!");
                            context.Response.End();
                        }
                        else
                        {
                            context.Response.Write("0|条码" + mysnno + "不存在!");
                            context.Response.End();
                        }
                    }


                }


            //    物流码类型
                Barcode barcode;
                using (brBarcode brcode = new brBarcode())
                {
                    barcode = brcode.GetBarcodeEntity(mysnno, "");
                    if (barcode != null)
                    {
                        pqty = barcode.Qty;
                    }
                }

                if (pqty == 0)
                {
                    //context.Response.Write("0|数码" + mysnno +"不正确");
                    //context.Response.End();
                    pqty = 1;
                }

                using(brFgiScan bf=new brFgiScan())
                {
                    sal_fgi fgi = bf.RetrieveP(mysnno,usnno);
                    if(fgi!=null)
                    {
                        context.Response.Write("0|数码" + fgi.bsnno + "已入库");
                        context.Response.End();
                        return;
                    }
                }

                //小标签是否重复
                using (brPackb brp = new brPackb())
                {
                    sal_packb en = brp.Retrieve(snno);
                    if (en != null)
                    {
                        context.Response.Write("0|小码" + mysnno + "重复");
                        context.Response.End();
                        return;
                    }


                    int i = brp.ReInt(usnno);

                    if (i >= mypqy && mypqy > 0)
                    {
                        context.Response.Write("0|该大码的包装数量已满，不能再扫了");
                        context.Response.End();
                        return;
                    }
                    
                    en = new sal_packb();
                    en.snno = snno;
                    en.usnno = usnno;
                    en.unitcode = "";
                    en.pack_no = pack_no;
                    try
                    {
                        en.in_date = DateTime.Parse(in_date);
                    }
                    catch {
                        en.in_date = DateTime.Now;
                    }
                    
                    en.line_no = line_no;
                    en.lot_no = lot_no;
                   
                    en.p_no = p_no;
                    brp.Insert(en);

                    context.Response.Write("1|OK|" + (i+1).ToString());
      
                }

    

            }
            catch (Exception ex)
            {

                context.Response.Write("0|ERROR,"+ex.Message);

       //         string mypath = System.Web.HttpContext.Current.Request.MapPath("/");

      //          DbHelperSQL.WriteLog(ex.Message, mypath);


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
