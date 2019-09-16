using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using marr.BusinessRule;
using marr.DataLayer;
using System.Data;
using System.Collections;
using System.Web.Script.Serialization;
using UI.hmtools;
using marr.BusinessRule.Entity;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class appOrder : IHttpHandler
    {
        //新订单类
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);
            string optype = context.Request["optype"];
            if (optype == "list")
            {
                list(context);//订单浏览
            }
            else if (optype == "Qty")
            {
                Qty(context);
            }
            else if (optype == "shipScan")//出库扫描，总部和经销商二合一
            {
                shipScan(context);
            }
            else if (optype == "cushipScan")//出售扫描
            {
                cushipScan(context);
            }
            else if (optype == "kcshipScan")//库存出货
            {
                kcshipScan(context);
            }
            else if (optype == "fgiScan")//入库扫描 
            {
                fgiScan(context);
            }
            else if (optype == "reScan")//退货扫描 
            {
                reScan(context);
            }
            else if (optype == "cureScan")//退货扫描 
            {
                cureScan(context);
            }
            else if (optype == "tstScan")//调拨扫描 
            {
                tstScan(context);
            }
            else if (optype == "cutstScan")//经销商调拨扫描 
            {
                cutstScan(context);
            }
            else
            {
                context.Response.Write(MesExcep("参数错误"));
                context.Response.End();
            }
        }

        #region 订单浏览
        public void list(HttpContext context)
        {


        }
        public string ynstring(string yn)
        {
            switch (yn)
            {
                case "N": return "未审核";
                case "Y": return "已审核";
                case "X": return "订单作废";
            }
            return "";
        }

        public string DateTostring(DateTime? date)
        {
            if (date == null) return "";
            return date.Value.ToString("yyyy-MM-dd HH:mm");
        }

        #endregion

        #region 返回订单下某个产品的数量
        public void Qty(HttpContext context)
        {
  //
        }
        #endregion

        #region 直接出库扫描
        public void shipScan(HttpContext context)
        {
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);//操作人
            string shipcu_no = context.Server.UrlDecode(context.Request["shipcu_no"]);//下级经销商
            string p_no = context.Server.UrlDecode(context.Request["p_no"]);//产品
            string po_no = context.Request["po_no"];//订单明细号
            string snno = context.Request["snno"];//条码
            string ship_no = context.Request["ship_no"];//单号

            string user_id = context.Request["user_id"];//用户

            var fst_no = context.Request["st_no"];   //库别
            var flot_no = context.Request["lot_no"]; //批号

            string max = context.Request["intMax"];//数据控制，总数
            string qty = context.Request["intQty"];//数据控制，已经扫描数量
            int intMax = 0;
            int intQty = 0;
            try { intMax = int.Parse(max); }
            catch { }
            try { intQty = int.Parse(qty); }
            catch { }

            //判断码
            if (snno == null || snno.Length < 8)
            {
                context.Response.Write(MesExcep("请输入条码内容"));
                context.Response.End();
            }

            if (user_id==null)
            {
                user_id = cu_no;
            }

            if (user_id == null)
            {
                context.Response.Write(MesExcep("您尚未登录"));
                context.Response.End();
            }

            int t = snno.LastIndexOf("=");
            string mysnno = snno;
            int pqty = 0;
            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }
            string xtfgiyn = "Y";//总部是否入库
            string xtcufgiyn = "N";//经销商是否入库
          
            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);

            string tmpp_no = "";
            string st_no = "";
            string lastst_no = "";

            using (brFgiScan brfgi = new brFgiScan())
            {
                sal_fgi salfgi = brfgi.Retrieve(mysnno);
                if (salfgi != null)
                {
                    if (p_no==null && p_no.Length==0)
                       p_no = salfgi.p_no;
                     st_no = salfgi.st_no;
                     lastst_no = salfgi.st_no;

                }
                else
                {
                    //context.Response.Write(MesExcep("产品未入库！"));
                    //return;
                }
            }

            Barcode barcode;
            customer mycust = new customer() { cu_no=cu_no,xtcu_no=""};//操作人
            string shipProd_no = "";//出库记录的产品编号
            string pname = "";
            Hashtable hs = new Hashtable();//哈希表序列号返回结果
            hs.Add("success", true);
            hs.Add("Message", "");
            hs.Add("po_no", po_no);
            hs.Add("intMax", intMax);
            hs.Add("intQty", intQty);
            hs.Add("snno", snno);

            using (brBarcode brcode = new brBarcode())
            {
                barcode = brcode.GetBarcodeEntity(mysnno,"") ;
                if (barcode != null)
                {
                    pqty = barcode.Qty;
                }
            }
            if (pqty == 0 || pqty == 1)   //不让扫描小瓶
            {
                context.Response.Write(MesExcep("物流码不正确或扫描是瓶标"));
                context.Response.End();
            }


           // //判断是否拆箱
           // if (DbHelperSQL.chkrepeat("sal_cha", "snno", mysnno))
           // {
           //     context.Response.Write(MesExcep("此码已经拆箱，不能再使用"));
           //     context.Response.End();
           // }
           ////拆箱记录
           // if (barcode.Tcode.Length > 0 && barcode.Tcode != barcode.Code)
           // {
           //     if (!DbHelperSQL.chkrepeat("sal_cha", "snno", barcode.Tcode))
           //     {
           //         int ss = DbHelperSQL.ExecuteSql(" insert into sal_cha(snno,unitcode) values('" + barcode.Tcode + "','')");
           //     }

           // }

            //if (barcode.Ucode.Length > 0 && barcode.Ucode != barcode.Code && barcode.Ucode != barcode.Tcode)
            //{
            //    if (!DbHelperSQL.chkrepeat("sal_cha", "snno", barcode.Ucode))
            //    {
            //        int ss = DbHelperSQL.ExecuteSql(" insert into sal_cha(snno,unitcode) values('" + barcode.Ucode + "','')");
            //    }
            //}

            ////根据角色，判断码是否能用
            ////是否终端销售
            //using (brCuSaleScan brtst = new brCuSaleScan())
            //{
            //    sal_cusale saltst = brtst.Retrieve(barcode);
            //    if (saltst != null)
            //    {
            //        context.Response.Write(MesExcep("该编号已经销售！"));
            //        context.Response.End();
            //    }

            //}


            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            if (delyn == "Y")  //删除操作
            {
                if (cu_no == "")
                {
                    using (brShipScan brtst = new brShipScan())
                    {
                        sal_ship saltst = brtst.Retrieve(mysnno);
                        if (saltst != null)
                        {
                            brtst.Delete(saltst.fid);

                            context.Response.Write(MesExcep("删除成功"));
                            return;
                        }
                        else
                        {

                            context.Response.Write(MesExcep("物流码不存在"));
                            return;
                        }

                    }
                }
                else
                {
                    using (brCuShipScan brtst = new brCuShipScan())
                    {
                        sal_cuship saltst = brtst.Retrieve(mysnno,cu_no);
                        if (saltst != null)
                        {
                            brtst.Delete(saltst.fid);

                            context.Response.Write(MesExcep("删除成功"));
                            return;
                        }
                        else
                        {

                            context.Response.Write(MesExcep("物流码不存在"));
                            return;
                        }

                    }

                }

            }




            //产品控制
            //if (p_no == null || p_no == "")
            //    p_no = shipProd_no;

          //  if ((p_no == null || p_no == "") && cu_no == "")//总部出货，没有选择产品
          //  {
          //      context.Response.Write(MesExcep("请选择产品"));
          //      context.Response.End();
          //  }

            string type = "";
            if (p_no != "")
            {
                using (brProduct br = new brProduct())
                {
                    Inv_Part part = br.Retrieve(p_no);
                    if (part != null)
                    { 
                        pname = part.pname;
                        type = part.type;
                    }
                }
            }

            if (cu_no == "")
            {
                //总部扫描

                DateTime? reTime = null;
                //是否退货
                using (brReScan brtst = new brReScan())
                {
                    sal_re saltst = brtst.Retrieve(barcode);
                    if (saltst != null)
                    {
                        reTime = saltst.ship_date;
                    }
                }


                using (brShipScan brtst = new brShipScan())
                {
                    sal_ship saltst = brtst.Retrieve(barcode, reTime);
                    if (saltst != null)
                    {
                        context.Response.Write(MesExcep("该编号已经出货！"));
                        context.Response.End();
                    }
                }

                using (brTstScan brtst = new brTstScan())
                {
                    //获取最后的库别
                    foreach (var item in brtst.Query(mysnno, reTime))
                    {
                        lastst_no = item.nst_no.Trim();
                    }


                }
                st_no = lastst_no;



            }
            else
            {
                string cu_name = "";
                //操作人
                using (brCustomer br = new brCustomer())
                {
                    mycust = br.Retrieve(cu_no);
                    if (mycust == null)
                    {
                        context.Response.Write(MesExcep("您的账号有误，请重新登录"));
                        context.Response.End();
                    }
                    else
                    {
                        cu_name = mycust.cu_name;
                    }
                }


                string lastcu_no = "";  //最后经销商
                //上级是否发货
       
                    DateTime? reTime = null;
                    //是否退货
                    using (brReScan brtst = new brReScan())
                    {
                        sal_re saltst = brtst.Retrieve(barcode);
                        if (saltst != null)
                        {
                            reTime = saltst.ship_date;
                        }
                    }
                    using (brShipScan brtst = new brShipScan())
                    {
                        sal_ship saltst = brtst.Retrieve(barcode, reTime);
                        if (saltst == null)
                        {
                            //总部未出货，直接出货

                            using (brShipScan br = new brShipScan())
                            {
                                sal_ship en = new sal_ship();
                                en.cu_no = cu_no;
                                en.cu_name = cu_name;//收货人
                                en.mqty = pqty;
                                en.p_no = p_no;
                                en.pname = pname;
                                en.ship_no = ship_no;
                                en.po_no = po_no;
                                en.bsnno = mysnno;
                                en.st_no = st_no;
                                en.ship_date = DateTime.Now;
                                en.xtcu_no = "";
                                en.xtcu_name = "";
                                en.unitcode = "";
                                en.pda_no = user_id;
                                en.remark = "下级直接出货";

                                try
                                {
                                    br.Insert(en);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("APPORDER", ex.Message);
                                    hs["success"] = false;
                                    hs["Message"] = "插入记录失败";
                                }
                            }

                            lastcu_no = cu_no;

                            shipProd_no = "";


                            // context.Response.Write(MesExcep("总部尚未出库！"));
                            // context.Response.End();
                        }
                        else
                        {
                            lastcu_no = saltst.cu_no;

                            shipProd_no = saltst.p_no;
                        }
                    }
     
                    //不是一级经销商
                    //上级经销商是否出库
                    using (brCuShipScan brCutst = new brCuShipScan())
                    {
                        sal_cuship Cusaltst = brCutst.RetrieveRE(barcode, cu_no, "");
                        if (Cusaltst != null)
                        {
         
                            lastcu_no = Cusaltst.cu_no;
                            shipProd_no = Cusaltst.p_no;
                        }
            
                    }

                    if (lastcu_no == null || lastcu_no.Length == 0 || lastcu_no.Trim() != cu_no)
                    {
                        context.Response.Write(MesExcep("上级未出库，或出库的经销商不正确!"));
                        context.Response.End();
                    }


                    //是否授权下一级
                    using (brCuShipScan brCutst = new brCuShipScan())
                    {
                        sal_cuship Cusaltst2 = brCutst.RetrieveXt(barcode, cu_no, "");
                        if (Cusaltst2 != null)
                        {
                            //是否退货
                            using (brCuReScan brCuRe = new brCuReScan())
                            {
                
                                    context.Response.Write(MesExcep("出货不成功！该码已经出货给：" + Cusaltst2.cu_no + "---" + Cusaltst2.cu_name + "，请勿重复扫描！"));
                                    context.Response.End();
                           
                            }
                        }
                    }
            }

            //订单判断
            //if (po_no != null && po_no.Trim().Length > 1)
            //{
            //    string orderp_no = context.Server.UrlDecode(context.Request["orderp_no"]);
            //    using (brCorder brcorder = new brCorder())
            //    {
            //        sal_corder _corder = brcorder.Retrieve(po_no, orderp_no,0);
            //        if (_corder != null && orderp_no!=null)
            //        {
            //            if (_corder.ovqty >= _corder.mqty)
            //            {
            //                context.Response.Write(MesExcep("子订单已经完成扫描！"));
            //                context.Response.End();
            //            }

            //            if ((_corder.ovqty + pqty) > _corder.mqty)
            //            {
            //                context.Response.Write(MesExcep("扫描数量大于订单数量！"));
            //                context.Response.End();
            //            }
            //            if (shipProd_no != null && shipProd_no.Trim() != _corder.p_no.Trim() && shipProd_no.Trim() != "")
            //            {
            //                context.Response.Write(MesExcep("产品错误!"));
            //                context.Response.End();
            //            }
            //            intMax = (int)_corder.mqty;
            //            intQty = (int)_corder.ovqty;
            //            shipcu_no = _corder.cu_no;//收货经销商替换成订单的
            //            p_no = _corder.p_no;
            //        }
            //        else
            //        {
            //            context.Response.Write(MesExcep("子订单不存在"));
            //            context.Response.End();
            //        }

            //    }
            //}





            ////数量控制
            //if (intMax > 0 && (intMax - intQty) < pqty)
            //{
            //    context.Response.Write(MesExcep("出库失败，超出了最大数量！"));
            //    context.Response.End();
            //}
            //收货经销商信息
            customer shipcust = null;
            using (brCustomer br = new brCustomer())
            {
                shipcust = br.Retrieve(shipcu_no);
                if (shipcust == null)
                {
                    context.Response.Write(MesExcep("出库客户不存在"));
                    context.Response.End();
                }
            }
            //保存出货记录

            if ( cu_no =="" &&  shipcust.xtcu_no != cu_no)
            {
                //跨级出货
                using (brShipScan br = new brShipScan())
                {
                    sal_ship en = new sal_ship();
                    en.cu_no = shipcust.cu_no;
                    en.cu_name = shipcust.cu_name;//收货人
                    en.mqty = pqty;
                    en.p_no = p_no;
                    en.pname = pname;
                    en.ship_no = ship_no;
                    en.po_no = po_no;
                    en.bsnno = mysnno;
                    en.pda_no = user_id;
                    en.st_no = st_no;
                    en.ship_date = DateTime.Now;
                    en.xtcu_no = mycust.cu_no;//操作人
                    en.xtcu_name = mycust.cu_name;
                    en.unitcode = "";
                    en.remark = "总部代" + shipcust.xtcu_no + "出货";

                    try
                    {
                        br.Insert(en);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("APPORDER", ex.Message);
                        hs["success"] = false;
                        hs["Message"] = "插入记录失败";
                    }
                }
            }
            else
            {
                //直属发货
                if (cu_no =="")
                {
                    //总部出货未入库
                    using (brFgiScan brfgi = new brFgiScan())
                    {
                        sal_fgi salfgi = brfgi.Retrieve(mysnno);
                        if (salfgi == null)
                        {
                            using (brFgiScan br = new brFgiScan())
                            {
                                

                                sal_fgi en = new sal_fgi();

                                en.cu_no = "";
                                en.cu_name = "";//收货人
                                en.mqty = pqty;
                                en.p_no = p_no;
                                en.pname = pname;
                                en.ship_no = ship_no;
                                en.pda_no = user_id;
                                en.bsnno = mysnno;
                                en.st_no = fst_no;
                                en.lot_no = flot_no;
                                en.type = type;
                                en.ship_date = DateTime.Now;
                                en.xtcu_no = "";
                                en.xtcu_name = "";
                                en.unitcode = "";

                                try
                                {
                                    br.Insert(en);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("APPORDER", ex.Message);
                                    hs["success"] = false;
                                    hs["Message"] = "插入记录失败";
                                }
                            }
                        }
                       
                    }                    

                    //总部出货
                    using (brShipScan br = new brShipScan())
                    {
                        sal_ship en = new sal_ship();
                        en.cu_no = shipcust.cu_no;
                        en.cu_name = shipcust.cu_name;
                        en.mqty = pqty;
                        en.p_no = p_no;
                        en.pname = pname;
                        en.ship_no = ship_no;
                        en.po_no = po_no;
                        en.bsnno = mysnno;
                        en.st_no = string.IsNullOrEmpty(st_no)? fst_no : st_no;
                        en.ship_date = DateTime.Now;
                        en.xtcu_no = "";
                        en.unitcode = "";
                        en.pda_no = user_id;
                        en.lot_no = flot_no;

                        try
                        {
                            br.Insert(en);
                        }
                        catch (Exception ex)
                        {
                           Log.Error("APPORDER", ex.Message);
                            hs["success"] = false;
                            hs["Message"] = "插入记录失败";
                        }
                    }
                }
                else
                {
                    //经销商出货
                    using (brCuShipScan br = new brCuShipScan())
                    {
                        sal_cuship en = new sal_cuship();
                        en.cu_no = shipcust.cu_no;
                        en.cu_name = shipcust.cu_name;//收货人
                        en.mqty = pqty;
                        en.p_no = p_no;
                        en.pname = pname;
                        en.ship_no = ship_no;
                        en.bsnno = mysnno;
                        en.st_no = "";
                        en.ship_date = DateTime.Now;
                        en.xtcu_no = mycust.cu_no;//操作人
                        en.xtcu_name = mycust.cu_name;
                        en.pda_no = user_id;
                        en.unitcode = "";
                        try
                        {
                            br.Insert(en);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("APPORDER", ex.Message);
                            hs["success"] = false;
                            hs["Message"] = "插入记录失败";
                        }
                    }
                }
            }

            int tt = DbHelperSQL.ExecuteSql("update sal_ship  set cu_name=customer.cu_name,loca=customer.loca  from customer where  sal_ship.cu_no=customer.cu_no and sal_ship.cu_name =''");
       //     tt = DbHelperSQL.ExecuteSql("update sal_ship  set pname=Inv_Part.pname, type=Inv_Part.type   from Inv_Part where  sal_ship.p_no=Inv_Part.p_no and sal_ship.pname =''");
            tt = DbHelperSQL.ExecuteSql("update sal_cuship  set p_no=sal_ship.p_no,pname=sal_ship.pname, type=sal_ship.type   from sal_ship where  sal_cuship.bsnno=sal_ship.bsnno and (sal_cuship.p_no is null or  sal_cuship.p_no ='')");
            tt = DbHelperSQL.ExecuteSql("update sal_cuship  set cu_name=customer.cu_name   from customer where  sal_cuship.cu_no=customer.cu_no and sal_cuship.cu_name =''");


            hs["intMax"] = intMax;
            hs["intQty"] = intQty + pqty;
            hs.Add("pname", pname);
            hs.Add("p_no", p_no);
            hs.Add("pqty", pqty);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string teml = jss.Serialize(hs);
            Log.Debug("ShipScan", teml);//记录出库扫描明细
            context.Response.Write(teml);
            context.Response.End();
        }
        #endregion

        #region 库存出库扫描
        public void kcshipScan(HttpContext context)
        {
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);//操作人
            string shipcu_no = context.Server.UrlDecode(context.Request["shipcu_no"]);//下级经销商
            string p_no = context.Server.UrlDecode(context.Request["p_no"]);//产品
            string po_no = context.Request["po_no"];//订单明细号
            string snno = context.Request["snno"];//条码
            string ship_no = context.Request["ship_no"];//单号

            string user_id = context.Request["user_id"];//用户           

            string max = context.Request["intMax"];//数据控制，总数
            string qty = context.Request["intQty"];//数据控制，已经扫描数量
            int intMax = 0;
            int intQty = 0;
            try { intMax = int.Parse(max); }
            catch { }
            try { intQty = int.Parse(qty); }
            catch { }

            //判断码
            if (snno == null || snno.Length < 8)
            {
                context.Response.Write(MesExcep("请输入条码内容"));
                context.Response.End();
            }

            if (user_id == null)
            {
                user_id = cu_no;
            }

            if (user_id == null)
            {
                context.Response.Write(MesExcep("您尚未登录"));
                context.Response.End();
            }

            int t = snno.LastIndexOf("=");
            string mysnno = snno;
            int pqty = 0;
            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }
            string xtfgiyn = "Y";//总部是否入库
            string xtcufgiyn = "N";//经销商是否入库

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);

            string tmpp_no = "";
            string st_no = "";
            string lastst_no = "";

           

            Barcode barcode;
            customer mycust = new customer() { cu_no = cu_no, xtcu_no = "" };//操作人
            string shipProd_no = "";//出库记录的产品编号
            string pname = "";
            Hashtable hs = new Hashtable();//哈希表序列号返回结果
            hs.Add("success", true);
            hs.Add("Message", "");
            hs.Add("po_no", po_no);
            hs.Add("intMax", intMax);
            hs.Add("intQty", intQty);
            hs.Add("snno", snno);

            using (brBarcode brcode = new brBarcode())
            {
                barcode = brcode.GetBarcodeEntity(mysnno, "");
                if (barcode != null)
                {
                    pqty = barcode.Qty;
                }
            }
            //if (pqty == 0 || pqty == 1)   //不让扫描小瓶
            //{
            //    pqty = 1;
            //}

            using (brFgiScan brfgi = new brFgiScan())
            {
                sal_fgi salfgi = brfgi.Retrieve(barcode);
                if (salfgi != null)
                {
                    if (string.IsNullOrEmpty(p_no))
                        p_no = salfgi.p_no;
                    st_no = salfgi.st_no;
                    lastst_no = salfgi.st_no;

                }
                else
                {
                    context.Response.Write(MesExcep("产品未入库！"));
                    return;
                }
            }

            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            if (delyn == "Y")  //删除操作
            {
                if (cu_no == "")
                {
                    using (brShipScan brtst = new brShipScan())
                    {
                        sal_ship saltst = brtst.Retrieve(mysnno);
                        if (saltst != null)
                        {
                            brtst.Delete(saltst.fid);

                            context.Response.Write(MesExcep("删除成功"));
                            return;
                        }
                        else
                        {

                            context.Response.Write(MesExcep("物流码不存在"));
                            return;
                        }

                    }
                }
                else
                {
                    using (brCuShipScan brtst = new brCuShipScan())
                    {
                        sal_cuship saltst = brtst.Retrieve(mysnno, cu_no);
                        if (saltst != null)
                        {
                            brtst.Delete(saltst.fid);

                            context.Response.Write(MesExcep("删除成功"));
                            return;
                        }
                        else
                        {

                            context.Response.Write(MesExcep("物流码不存在"));
                            return;
                        }

                    }

                }

            }
            

            string type = "";
            if (p_no != "")
            {
                using (brProduct br = new brProduct())
                {
                    Inv_Part part = br.Retrieve(p_no);
                    if (part != null)
                    {
                        pname = part.pname;
                        type = part.type;
                    }
                }
            }

            if (cu_no == "")
            {
                //总部扫描

                DateTime? reTime = null;
                //是否退货
                using (brReScan brtst = new brReScan())
                {
                    sal_re saltst = brtst.Retrieve(barcode);
                    if (saltst != null)
                    {
                        reTime = saltst.ship_date;
                    }
                }


                using (brShipScan brtst = new brShipScan())
                {
                    sal_ship saltst = brtst.Retrieve(barcode, reTime);
                    if (saltst != null)
                    {
                        context.Response.Write(MesExcep("该编号已经出货！"));
                        context.Response.End();
                    }
                }

                using (brTstScan brtst = new brTstScan())
                {
                    //获取最后的库别
                    foreach (var item in brtst.Query(mysnno, reTime))
                    {
                        lastst_no = item.nst_no.Trim();
                    }


                }
                st_no = lastst_no;



            }
            else
            {
                string cu_name = "";
                //操作人
                using (brCustomer br = new brCustomer())
                {
                    mycust = br.Retrieve(cu_no);
                    if (mycust == null)
                    {
                        context.Response.Write(MesExcep("您的账号有误，请重新登录"));
                        context.Response.End();
                    }
                    else
                    {
                        cu_name = mycust.cu_name;
                    }
                }


                string lastcu_no = "";  //最后经销商
                //上级是否发货

                DateTime? reTime = null;
                //是否退货
                using (brReScan brtst = new brReScan())
                {
                    sal_re saltst = brtst.Retrieve(barcode);
                    if (saltst != null)
                    {
                        reTime = saltst.ship_date;
                    }
                }
                using (brShipScan brtst = new brShipScan())
                {
                    sal_ship saltst = brtst.Retrieve(barcode, reTime);
                    if (saltst == null)
                    {
                        //总部未出货，直接出货

                        using (brShipScan br = new brShipScan())
                        {
                            sal_ship en = new sal_ship();
                            en.cu_no = cu_no;
                            en.cu_name = cu_name;//收货人
                            en.mqty = pqty;
                            en.p_no = p_no;
                            en.pname = pname;
                            en.ship_no = ship_no;
                            en.po_no = po_no;
                            en.bsnno = mysnno;
                            en.st_no = st_no;
                            en.ship_date = DateTime.Now;
                            en.xtcu_no = "";
                            en.xtcu_name = "";
                            en.unitcode = "";
                            en.pda_no = user_id;
                            en.remark = "下级直接出货";

                            try
                            {
                                br.Insert(en);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("APPORDER", ex.Message);
                                hs["success"] = false;
                                hs["Message"] = "插入记录失败";
                            }
                        }

                        lastcu_no = cu_no;

                        shipProd_no = "";


                        // context.Response.Write(MesExcep("总部尚未出库！"));
                        // context.Response.End();
                    }
                    else
                    {
                        lastcu_no = saltst.cu_no;

                        shipProd_no = saltst.p_no;
                    }
                }

                //不是一级经销商
                //上级经销商是否出库
                using (brCuShipScan brCutst = new brCuShipScan())
                {
                    sal_cuship Cusaltst = brCutst.RetrieveRE(barcode, cu_no, "");
                    if (Cusaltst != null)
                    {

                        lastcu_no = Cusaltst.cu_no;
                        shipProd_no = Cusaltst.p_no;
                    }

                }

                if (lastcu_no == null || lastcu_no.Length == 0 || lastcu_no.Trim() != cu_no)
                {
                    context.Response.Write(MesExcep("上级未出库，或出库的经销商不正确!"));
                    context.Response.End();
                }


                //是否授权下一级
                using (brCuShipScan brCutst = new brCuShipScan())
                {
                    sal_cuship Cusaltst2 = brCutst.RetrieveXt(barcode, cu_no, "");
                    if (Cusaltst2 != null)
                    {
                        //是否退货
                        using (brCuReScan brCuRe = new brCuReScan())
                        {

                            context.Response.Write(MesExcep("出货不成功！该码已经出货给：" + Cusaltst2.cu_no + "---" + Cusaltst2.cu_name + "，请勿重复扫描！"));
                            context.Response.End();

                        }
                    }
                }
            }

           
            customer shipcust = null;
            using (brCustomer br = new brCustomer())
            {
                shipcust = br.Retrieve(shipcu_no);
                if (shipcust == null)
                {
                    context.Response.Write(MesExcep("出库客户不存在"));
                    context.Response.End();
                }
            }
            //保存出货记录

            if (cu_no == "" && shipcust.xtcu_no != cu_no)
            {
                //跨级出货
                using (brShipScan br = new brShipScan())
                {
                    sal_ship en = new sal_ship();
                    en.cu_no = shipcust.cu_no;
                    en.cu_name = shipcust.cu_name;//收货人
                    en.mqty = pqty;
                    en.p_no = p_no;
                    en.pname = pname;
                    en.ship_no = ship_no;
                    en.po_no = po_no;
                    en.bsnno = mysnno;
                    en.pda_no = user_id;
                    en.st_no = st_no;
                    en.ship_date = DateTime.Now;
                    en.xtcu_no = mycust.cu_no;//操作人
                    en.xtcu_name = mycust.cu_name;
                    en.unitcode = "";
                    en.remark = "总部代" + shipcust.xtcu_no + "出货";

                    try
                    {
                        br.Insert(en);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("APPORDER", ex.Message);
                        hs["success"] = false;
                        hs["Message"] = "插入记录失败";
                    }
                }
            }
            else
            {
                //直属发货
                if (cu_no == "")
                {
                    

                    //总部出货
                    using (brShipScan br = new brShipScan())
                    {
                        sal_ship en = new sal_ship();
                        en.cu_no = shipcust.cu_no;
                        en.cu_name = shipcust.cu_name;
                        en.mqty = pqty;
                        en.p_no = p_no;
                        en.pname = pname;
                        en.ship_no = ship_no;
                        en.po_no = po_no;
                        en.bsnno = mysnno;
                        en.st_no =  st_no;
                        en.ship_date = DateTime.Now;
                        en.xtcu_no = "";
                        en.unitcode = "";
                        en.pda_no = user_id;
                        
                        try
                        {
                            br.Insert(en);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("APPORDER", ex.Message);
                            hs["success"] = false;
                            hs["Message"] = "插入记录失败";
                        }
                    }
                }
                else
                {
                    //经销商出货
                    using (brCuShipScan br = new brCuShipScan())
                    {
                        sal_cuship en = new sal_cuship();
                        en.cu_no = shipcust.cu_no;
                        en.cu_name = shipcust.cu_name;//收货人
                        en.mqty = pqty;
                        en.p_no = p_no;
                        en.pname = pname;
                        en.ship_no = ship_no;
                        en.bsnno = mysnno;
                        en.st_no = "";
                        en.ship_date = DateTime.Now;
                        en.xtcu_no = mycust.cu_no;//操作人
                        en.xtcu_name = mycust.cu_name;
                        en.pda_no = user_id;
                        en.unitcode = "";
                        try
                        {
                            br.Insert(en);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("APPORDER", ex.Message);
                            hs["success"] = false;
                            hs["Message"] = "插入记录失败";
                        }
                    }
                }
            }

            int tt = DbHelperSQL.ExecuteSql("update sal_ship  set cu_name=customer.cu_name,loca=customer.loca  from customer where  sal_ship.cu_no=customer.cu_no and sal_ship.cu_name =''");
            //     tt = DbHelperSQL.ExecuteSql("update sal_ship  set pname=Inv_Part.pname, type=Inv_Part.type   from Inv_Part where  sal_ship.p_no=Inv_Part.p_no and sal_ship.pname =''");
            tt = DbHelperSQL.ExecuteSql("update sal_cuship  set p_no=sal_ship.p_no,pname=sal_ship.pname, type=sal_ship.type   from sal_ship where  sal_cuship.bsnno=sal_ship.bsnno and (sal_cuship.p_no is null or  sal_cuship.p_no ='')");
            tt = DbHelperSQL.ExecuteSql("update sal_cuship  set cu_name=customer.cu_name   from customer where  sal_cuship.cu_no=customer.cu_no and sal_cuship.cu_name =''");


            hs["intMax"] = intMax;
            hs["intQty"] = intQty + pqty;
            hs.Add("pname", pname);
            hs.Add("p_no", p_no);
            hs.Add("pqty", pqty);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string teml = jss.Serialize(hs);
            Log.Debug("ShipScan", teml);//记录出库扫描明细
            context.Response.Write(teml);
            context.Response.End();
        }
        #endregion

        #region 经销商出售扫描
        public void cushipScan(HttpContext context)
        {
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);//操作人
            string shipcu_no = context.Server.UrlDecode(context.Request["shipcu_no"]);//下级经销商
            string p_no = context.Server.UrlDecode(context.Request["p_no"]);//产品
            string po_no = context.Request["po_no"];//订单明细号
            string snno = context.Request["snno"];//条码
            string ship_no = context.Request["ship_no"];//单号

            string user_id = context.Request["user_id"];//用户           

            string max = context.Request["intMax"];//数据控制，总数
            string qty = context.Request["intQty"];//数据控制，已经扫描数量
            int intMax = 0;
            int intQty = 0;
            try { intMax = int.Parse(max); }
            catch { }
            try { intQty = int.Parse(qty); }
            catch { }

            //判断码
            if (snno == null || snno.Length < 8)
            {
                context.Response.Write(MesExcep("请输入条码内容"));
                context.Response.End();
            }

            if (string.IsNullOrEmpty(user_id))
            {
                user_id = cu_no;
            }

            if (string.IsNullOrEmpty(user_id))
            {
                context.Response.Write(MesExcep("您尚未登录"));
                context.Response.End();
            }

            int t = snno.LastIndexOf("=");
            string mysnno = snno;
            int pqty = 0;
            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }
            

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);

            string tmpp_no = "";
            string st_no = "";
            string lastst_no = "";
            
            
            Barcode barcode;
            using (brBarcode brcode = new brBarcode())
            {
                barcode = brcode.GetBarcodeEntity(mysnno, "");
                if (barcode != null)
                {
                    pqty = barcode.Qty;
                }
            }
            if (pqty == 0)   //不让扫描小瓶
            {
                context.Response.Write(MesExcep("物流码不正确"));
                context.Response.End();
            }            

           
            customer mycust = new customer() { cu_no = cu_no, xtcu_no = "" };//操作人
            string shipProd_no = "";//出库记录的产品编号
            string pname = "";
            Hashtable hs = new Hashtable();//哈希表序列号返回结果
            hs.Add("success", true);
            hs.Add("Message", "");
            hs.Add("po_no", po_no);
            hs.Add("intMax", intMax);
            hs.Add("intQty", intQty);
            hs.Add("snno", snno);

            


            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            if (delyn == "Y")  //删除操作
            {
                
                    using (brCuShipScan brtst = new brCuShipScan())
                    {
                        sal_cuship saltst = brtst.Retrieve(mysnno, cu_no);
                        if (saltst != null)
                        {
                            brtst.Delete(saltst.fid);

                            context.Response.Write(MesExcep("删除成功"));
                            return;
                        }
                        else
                        {

                            context.Response.Write(MesExcep("物流码不存在"));
                            return;
                        }

                    }                

            }

            DateTime? re_date=null;
            using (brCuReScan cr = new brCuReScan())
            {
                sal_cure sc = cr.Retrieve(mysnno);
                if (sc != null)
                    re_date = sc.ship_date;
            }

            using(brCuShipScan cs=new brCuShipScan())
            {
                   sal_cuship sc=cs.RetrieveB(barcode,re_date);                   
                   if (sc != null)
                   {
                       context.Response.Write(MesExcep("重复出售"));
                       return;
                   }

                   sal_cuship sc2 = cs.Retrieve(barcode.Tcode);
                   if (sc2 != null)
                   {
                       context.Response.Write(MesExcep("箱码已出货，产品码不能出货"));
                       return;
                   }                  
                   
            }

            using (brShipScan ss = new brShipScan())
            {

                sal_ship ship = ss.Retship(barcode, cu_no);
                if (ship == null )
                {
                    context.Response.Write(MesExcep("总部未发货"));
                    return;
                }
                else {
                    if (string.IsNullOrEmpty(p_no) && ship!=null)
                        p_no = ship.p_no;                    
                }

            }


            string type = "";
            if (p_no != "")
            {
                using (brProduct br = new brProduct())
                {
                    Inv_Part part = br.Retrieve(p_no);
                    if (part != null)
                    {
                        pname = part.pname;
                        type = part.type;
                    }
                }
            }

            
                string cu_name = "";
                //操作人
                using (brCustomer br = new brCustomer())
                {
                    mycust = br.Retrieve(cu_no);
                    if (mycust == null)
                    {
                        context.Response.Write(MesExcep("您的账号有误，请重新登录"));
                        context.Response.End();
                    }
                    else
                    {
                        cu_name = mycust.cu_name;
                    }
                }


                //经销商出货
                using (brCuShipScan br = new brCuShipScan())
                {
                    sal_cuship en = new sal_cuship();
                   
                    en.mqty = pqty;
                    en.p_no = p_no;
                    en.pname = pname;
                    en.ship_no = ship_no;
                    en.bsnno = mysnno;
                    en.st_no = "";
                    en.ship_date = DateTime.Now;
                    en.xtcu_no = mycust.cu_no;//操作人
                    en.xtcu_name = mycust.cu_name;
                    en.pda_no = user_id;
                    en.unitcode = "";
                    try
                    {
                        br.Insert(en);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("APPORDER", ex.Message);
                        hs["success"] = false;
                        hs["Message"] = "插入记录失败";
                    }
                }

            int tt = DbHelperSQL.ExecuteSql("update sal_ship  set cu_name=customer.cu_name,loca=customer.loca  from customer where  sal_ship.cu_no=customer.cu_no and sal_ship.cu_name =''");
            //     tt = DbHelperSQL.ExecuteSql("update sal_ship  set pname=Inv_Part.pname, type=Inv_Part.type   from Inv_Part where  sal_ship.p_no=Inv_Part.p_no and sal_ship.pname =''");
            tt = DbHelperSQL.ExecuteSql("update sal_cuship  set p_no=sal_ship.p_no,pname=sal_ship.pname, type=sal_ship.type   from sal_ship where  sal_cuship.bsnno=sal_ship.bsnno and (sal_cuship.p_no is null or  sal_cuship.p_no ='')");
            tt = DbHelperSQL.ExecuteSql("update sal_cuship  set cu_name=customer.cu_name   from customer where  sal_cuship.cu_no=customer.cu_no and sal_cuship.cu_name =''");


            hs["intMax"] = intMax;
            hs["intQty"] = intQty + pqty;
            hs.Add("pname", pname);
            hs.Add("p_no", p_no);
            hs.Add("pqty", pqty);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string teml = jss.Serialize(hs);
            Log.Debug("ShipScan", teml);//记录出库扫描明细
            context.Response.Write(teml);
            context.Response.End();
        }
        #endregion

        #region 入库扫描
        public void fgiScan(HttpContext context)
        {
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);//操作人
            string shipcu_no = context.Server.UrlDecode(context.Request["shipcu_no"]);//下级经销商
            string p_no = context.Server.UrlDecode(context.Request["p_no"]);//产品
            string lot_no = context.Request["lot_no"];//批号
            string snno = context.Request["snno"];//条码
            string ship_no = context.Request["ship_no"];//单号
            string st_no = context.Request["st_no"];//库别
            string max = context.Request["intMax"];//数据控制，总数
            string qty = context.Request["intQty"];//数据控制，已经扫描数量
            int intMax = 0;
            int intQty = 0;
            try { intMax = int.Parse(max); }
            catch { }
            try { intQty = int.Parse(qty); }
            catch { }

            //判断码
            if (snno == null || snno.Length < 8)
            {
                context.Response.Write(MesExcep("请输入条码内容"));
                return;
            }

            if (cu_no == null || cu_no.Length == 0)
            {
                context.Response.Write(MesExcep("您尚未登录"));
                return;
            }

            int t = snno.LastIndexOf("=");
            string mysnno = snno;
            int pqty = 0;
            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);

            Barcode barcode;

            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            if (delyn == "Y")  //删除操作
            {

                using (brFgiScan brtst = new brFgiScan())
                {
                    sal_fgi saltst = brtst.Retrieve(mysnno);
                    if (saltst != null)
                    {
                        brtst.Delete(saltst.fid);

                        context.Response.Write(MesExcep("删除成功"));
                        return;
                    }
                    else
                    {

                        context.Response.Write(MesExcep("物流码不存在"));
                        return;
                    }

                }

            }


            string pname = "";
            string shipcu_name = "";
            Hashtable hs = new Hashtable();//哈希表序列号返回结果
            hs.Add("success", true);
            hs.Add("Message", "");
            hs.Add("lot_no", lot_no);
            hs.Add("st_no", st_no);
            hs.Add("intMax", intMax);
            hs.Add("intQty", intQty);
            hs.Add("snno", snno);
            using (brBarcode brcode = new brBarcode())
            {
                barcode = brcode.GetBarcodeEntity(mysnno, "");
                if (barcode != null)
                {
                    pqty = barcode.Qty;
                }
            }
            if (pqty == 0 || pqty == 1)   //不让扫描小瓶
            {
                //context.Response.Write(MesExcep("物流码不正确或扫描是瓶标"));
                //context.Response.End();
                pqty = 1;
            }




            if ((p_no == null || p_no == "") && cu_no == "")//总部出货，没有选择产品
            {
                context.Response.Write(MesExcep("请选择产品"));
                return;
            }
            if (p_no != "")
            {
                using (brProduct br = new brProduct())
                {
                    Inv_Part part = br.Retrieve(p_no);
                    if (part != null)
                        pname = part.pname;
                }
            }

            if ((shipcu_no == null || shipcu_no == "") && cu_no == "")//总部出货，没有选择产品
            {
                context.Response.Write(MesExcep("请选择供应商"));
                return;
            }
            if (shipcu_no != "")
            {
                using (brCustomer br = new brCustomer())
                {
                    customer part = br.Retrieve(shipcu_no);
                    if (part != null)
                        shipcu_name = part.cu_name;
                }
            }

            ////数量控制
            //if (intMax > 0 && (intMax - intQty) < pqty)
            //{
            //    context.Response.Write(MesExcep("出库失败，超出了最大数量！"));
            //    context.Response.End();
            //}

            //保存记录

            using (brFgiScan br = new brFgiScan())
            {
                if (br.Retrieve(mysnno) !=null)
                {
                    context.Response.Write(MesExcep("重复入库！"));
                    return;
                }

                sal_fgi en = new sal_fgi();

                en.cu_no = shipcu_no;
                en.cu_name =shipcu_name;//收货人
                en.mqty = pqty;
                en.p_no = p_no;
                en.pname = pname;
                en.ship_no = ship_no;
                en.pda_no = cu_no;
                en.bsnno = mysnno;
                en.st_no = st_no;
                en.lot_no = lot_no;

                en.ship_date = DateTime.Now;
                en.xtcu_no = "";
                en.xtcu_name = "";
                en.unitcode = "";

                try
                {
                    br.Insert(en);
                }
                catch (Exception ex)
                {
                    Log.Error("APPORDER", ex.Message);
                    hs["success"] = false;
                    hs["Message"] = "插入记录失败";
                }
            }


            hs["intMax"] = intMax;
            hs["intQty"] = intQty + pqty;
            hs.Add("pname", pname);
            hs.Add("p_no", p_no);
            hs.Add("pqty", pqty);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string teml = jss.Serialize(hs);
            Log.Debug("fgiScan", teml);//记录入库扫描明细
            context.Response.Write(teml);
            return;
        }
        #endregion


        #region 退货扫描
        public void reScan(HttpContext context)
        {
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);//操作人
           

            string snno = context.Request["snno"];//条码
            string ship_no = context.Request["ship_no"];//单号
          


            //判断码
            if (snno == null || snno.Length < 8)
            {
                context.Response.Write(MesExcep("请输入条码内容"));
                return;
            }

            if (cu_no == null || cu_no.Length == 0)
            {
                context.Response.Write(MesExcep("您尚未登录"));
                return;
            }

            int t = snno.LastIndexOf("=");
            string mysnno = snno;
            int? pqty = 0;
            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);

            Barcode barcode;

            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            if (delyn == "Y")  //删除操作
            {

                using (brReScan brtst = new brReScan())
                {
                    sal_re saltst = brtst.Retrieve(mysnno);
                    if (saltst != null)
                    {
                        brtst.Delete(saltst.fid);

                        context.Response.Write(MesExcep("删除成功"));
                        return;
                    }
                    else
                    {

                        context.Response.Write(MesExcep("物流码不存在"));
                        return;
                    }

                }

            }

            string p_no = "";
            string pname = "";
            string cu_name = "";
            var    shipcu_no = "";
            var    st_no = "";




            using (brBarcode brcode = new brBarcode())
            {
                barcode = brcode.GetBarcodeEntity(mysnno, "");
                if (barcode != null)
                {
                    pqty = barcode.Qty;
                }
            }

            DateTime? ship_date = null;
            using (brShipScan br = new brShipScan())
            {
                sal_ship mycust = br.Retrieve(barcode);
                if (mycust == null)
                {
                    context.Response.Write(MesExcep("产品未出库"));
                    return;
                }
                else
                {
                    cu_name = mycust.cu_name;
                    p_no = mycust.p_no;
                    pname = mycust.pname;
                    shipcu_no = mycust.cu_no;
                    cu_name = mycust.cu_name;
                    st_no = mycust.st_no;
                    pqty = mycust.mqty;
                    ship_date = mycust.ship_date;
                }
            }


            Hashtable hs = new Hashtable();//哈希表序列号返回结果
            hs.Add("success", true);
            hs.Add("Message", "");
            hs.Add("shipcu_no", shipcu_no);
            hs.Add("st_no", st_no);
            hs.Add("snno", snno);
            



          

            using (brCuShipScan cs = new brCuShipScan())
            {
                if (cs.Retrieve(mysnno) != null)
                {
                    context.Response.Write(MesExcep("产品门店已售出！"));
                    return;
                }
            }

            using (brReScan br = new brReScan())
            {
                if (br.Retrieve(barcode,ship_date) != null)
                {
                    context.Response.Write(MesExcep("重复退库！"));
                    return;
                }

                sal_re en = new sal_re();

                en.cu_no = shipcu_no;
                en.cu_name = cu_name; 
                en.mqty = pqty;
                en.p_no = p_no;
                en.pname = pname;
                en.ship_no = ship_no;
                en.pda_no = cu_no;
                en.bsnno = mysnno;
                en.st_no = st_no;

                en.ship_date = DateTime.Now;
                en.xtcu_no = "";
                en.xtcu_name = "";
                en.unitcode = "";

                try
                {
                    br.Insert(en);
                }
                catch (Exception ex)
                {
                    Log.Error("APPORDER", ex.Message);
                    hs["success"] = false;
                    hs["Message"] = "插入记录失败";
                }
            }


            hs.Add("pname", pname);
            hs.Add("p_no", p_no);
            hs.Add("pqty", pqty);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string teml = jss.Serialize(hs);
            Log.Debug("recan", teml);//记录入库扫描明细
            context.Response.Write(teml);
            return;
        }
        #endregion

        #region 经销商退货扫描
        public void cureScan(HttpContext context)
        {
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);//操作人


            string snno = context.Request["snno"];//条码
            string ship_no = context.Request["ship_no"];//单号



            //判断码
            if (snno == null || snno.Length < 8)
            {
                context.Response.Write(MesExcep("请输入条码内容"));
                return;
            }

            if (cu_no == null || cu_no.Length == 0)
            {
                context.Response.Write(MesExcep("您尚未登录"));
                return;
            }

            int t = snno.LastIndexOf("=");
            string mysnno = snno;
            int? pqty = 0;
            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);

            Barcode barcode;

            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            if (delyn == "Y")  //删除操作
            {

                using (brCuReScan brtst = new brCuReScan())
                {
                    sal_cure saltst = brtst.Retrieve(mysnno);
                    if (saltst != null)
                    {
                        brtst.Delete(saltst.fid);

                        context.Response.Write(MesExcep("删除成功"));
                        return;
                    }
                    else
                    {

                        context.Response.Write(MesExcep("物流码不存在"));
                        return;
                    }

                }

            }

            string p_no = "";
            string pname = "";
            string cu_name = "";
            var shipcu_no = "";
            var st_no = "";
            DateTime? ship_date = null;

            using (brCuShipScan br = new brCuShipScan())
            {
                sal_cuship mycust = br.Retrieve(mysnno);
                if (mycust == null)
                {
                    context.Response.Write(MesExcep("产品未出售"));
                    return;
                }
                else
                {
                    cu_name = mycust.cu_name;
                    p_no = mycust.p_no;
                    pname = mycust.pname;
                    shipcu_no = mycust.cu_no;
                    cu_name = mycust.cu_name;
                    st_no = mycust.st_no;
                    pqty = mycust.mqty;
                    ship_date = mycust.ship_date;
                }
            }


            Hashtable hs = new Hashtable();//哈希表序列号返回结果
            hs.Add("success", true);
            hs.Add("Message", "");
            hs.Add("shipcu_no", shipcu_no);
            hs.Add("st_no", st_no);
            hs.Add("snno", snno);

            using (brCuReScan br = new brCuReScan())
            {
                if (br.Retrieve(mysnno, shipcu_no,ship_date) != null)
                {
                    context.Response.Write(MesExcep("重复退库！"));
                    return;
                }

                sal_cure en = new sal_cure();

                en.cu_no = shipcu_no;
                en.cu_name = cu_name;
                en.mqty = pqty;
                en.p_no = p_no;
                en.pname = pname;
                en.ship_no = ship_no;
                en.pda_no = cu_no;
                en.bsnno = mysnno;
                en.st_no = st_no;

                en.ship_date = DateTime.Now;
                en.xtcu_no = "";
                en.xtcu_name = "";
                en.unitcode = "";

                try
                {
                    br.Insert(en);
                }
                catch (Exception ex)
                {
                    Log.Error("APPORDER", ex.Message);
                    hs["success"] = false;
                    hs["Message"] = "插入记录失败";
                }
            }


            hs.Add("pname", pname);
            hs.Add("p_no", p_no);
            hs.Add("pqty", pqty);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string teml = jss.Serialize(hs);
            Log.Debug("recan", teml);//记录入库扫描明细
            context.Response.Write(teml);
            return;
        }
        #endregion

        #region 调拨扫描
        public void tstScan(HttpContext context)
        {
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);//操作人

            string snno = context.Request["snno"];//条码
            string ship_no = context.Request["ship_no"];//单号
            string nst_no = context.Request["nst_no"];//新库别
            string ost_no = context.Request["ost_no"];//旧库别


            //判断码
            if (snno == null || snno.Length < 8)
            {
                context.Response.Write(MesExcep("请输入条码内容"));
                return;
            }

            if (nst_no == null || nst_no.Length == 0 || ost_no == null || ost_no.Length == 0)
            {
                context.Response.Write(MesExcep("请输入新旧库别"));
                return;
            }


            if (cu_no == null || cu_no.Length == 0)
            {
                context.Response.Write(MesExcep("您尚未登录"));
                return;
            }

            int t = snno.LastIndexOf("=");
            string mysnno = snno;
            int pqty = 0;
            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);

            Barcode barcode;

            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            string p_no = "";
            string pname = "";
            string lastst_no = "";  //最后库别


            //还需判断是否正确



            Hashtable hs = new Hashtable();//哈希表序列号返回结果
            hs.Add("success", true);
            hs.Add("Message", "");
  
            hs.Add("nst_no", nst_no);
            hs.Add("ost_no", ost_no);
            hs.Add("snno", snno);



            using (brBarcode brcode = new brBarcode())
            {
                barcode = brcode.GetBarcodeEntity(mysnno, "");
                if (barcode != null)
                {
                    pqty = barcode.Qty;
                }
            }

            if (pqty == 0 || pqty==1)   //不让扫描小瓶
            {
                context.Response.Write(MesExcep("物流码不正确或扫描是瓶标"));
                context.Response.End();
            }


            using (brFgiScan brfgi = new brFgiScan())
            {
                sal_fgi salfgi = brfgi.Retrieve(mysnno);
                if (salfgi != null)
                {

                    p_no = salfgi.p_no;
                    pname = salfgi.pname;
                    lastst_no = salfgi.st_no;
                }
                else
                {
                    context.Response.Write(MesExcep("产品未入库！"));
                    return;
                }
            }



            DateTime? reTime = null;   //  退货时间
            //是否退货
            using (brReScan brtst = new brReScan())
            {
                sal_re saltst = brtst.Retrieve(mysnno);
                if (saltst != null)
                {
                    reTime = saltst.ship_date;
                    lastst_no = saltst.st_no;
                }
            }


            using (brShipScan brtst = new brShipScan())
            {
                sal_ship saltst = brtst.Retrieve(mysnno, reTime);
                if (saltst != null)
                {
                    context.Response.Write(MesExcep("该编号已经出货！"));
                    context.Response.End();
                }
            }


            using (brTstScan br = new brTstScan())
            {

                if (br.Retrievetst(mysnno, nst_no, ost_no) != null)
                {
                    context.Response.Write(MesExcep("重复调拨！"));
                    return;
                }


                //获取最后的库别
                foreach (var item in br.Query(mysnno, reTime))
                {
                    lastst_no = item.nst_no.Trim();
                }

                if  (lastst_no.Trim() != ost_no.Trim())
                {
                    context.Response.Write(MesExcep("原库别不正确，应该是！" + lastst_no));
                    return;
                }

                //判断是否出货退货

                sal_tst en = new sal_tst();


                en.mqty = pqty;
                en.p_no = p_no;
                en.pname = pname;
                en.ship_no = ship_no;
                en.unitcode = cu_no;   //操作人员
                en.bsnno = mysnno;
                en.nst_no = nst_no;
                en.ost_no = ost_no;

                en.ship_date = DateTime.Now;


                try
                {
                    br.Insert(en);
                }
                catch (Exception ex)
                {
                    Log.Error("APPORDER", ex.Message);
                    hs["success"] = false;
                    hs["Message"] = "插入记录失败";
                }
            }


            hs.Add("pname", pname);
            hs.Add("p_no", p_no);
            hs.Add("pqty", pqty);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string teml = jss.Serialize(hs);
            Log.Debug("recan", teml);//记录入库扫描明细
            context.Response.Write(teml);
            return;
        }
        #endregion

        #region 经销商调拨扫描
        public void cutstScan(HttpContext context)
        {
            string cu_no = context.Server.UrlDecode(context.Request["cu_no"]);//操作人

            string snno = context.Request["snno"];//条码
            string ship_no = context.Request["ship_no"];//单号
            string nst_no = context.Request["nst_no"];//调拨门店
            


            //判断码
            if (snno == null || snno.Length < 8)
            {
                context.Response.Write(MesExcep("请输入条码内容"));
                return;
            }

            if (nst_no == null || nst_no.Length == 0)
            {
                context.Response.Write(MesExcep("请输入新旧库别"));
                return;
            }



            if (cu_no == null || cu_no.Length == 0)
            {
                context.Response.Write(MesExcep("您尚未登录"));
                return;
            }

            if(cu_no==nst_no)
            {
                context.Response.Write(MesExcep("无效的调拨"));
                return;
            }
            int t = snno.LastIndexOf("=");
            string mysnno = snno;
            int pqty = 0;
            if (t > 0 && snno.Length > (t + 1))
            {
                mysnno = snno.Substring(t + 1, snno.Length - t - 1);
            }

            if (mysnno.Length > 30)
                mysnno = mysnno.Substring(0, 30);

            Barcode barcode;

            string delyn = context.Request["delyn"];

            if (delyn == null)
                delyn = "N";


            string p_no = "";
            string pname = "";
            string lastst_no = "";  //最后库别


            //还需判断是否正确



            Hashtable hs = new Hashtable();//哈希表序列号返回结果
            hs.Add("success", true);
            hs.Add("Message", "");

            hs.Add("nst_no", nst_no);
            hs.Add("ost_no", cu_no);
            hs.Add("snno", snno);



            using (brBarcode brcode = new brBarcode())
            {
                barcode = brcode.GetBarcodeEntity(mysnno, "");
                if (barcode != null)
                {
                    pqty = barcode.Qty;
                }
            }

            if (pqty == 0 )   //不让扫描小瓶
            {
                context.Response.Write(MesExcep("物流码不正确"));
                context.Response.End();
            }


            using (brShipScan brfgi = new brShipScan())
            {
                sal_ship salfgi = brfgi.Retrievetui(barcode);
                if (salfgi != null)
                {

                    p_no = salfgi.p_no;
                    pname = salfgi.pname;
                    lastst_no = salfgi.st_no;
                }
                else
                {
                    context.Response.Write(MesExcep("无效的调拨操作！"));
                    return;
                }
            }



            DateTime? reTime = null;   //  退货时间
            //是否退货
            using (brReScan brtst = new brReScan())
            {
                sal_re saltst = brtst.Retrieve(mysnno);
                if (saltst != null)
                {
                    reTime = saltst.ship_date;
                    lastst_no = saltst.st_no;
                }
            }


            using (brCuShipScan brtst = new brCuShipScan())
            {
                sal_cuship saltst = brtst.RetrieveB(barcode, reTime);
                if (saltst != null)
                {
                    context.Response.Write(MesExcep("该产品已经出售！"));
                    context.Response.End();
                }
            }


            using (brTstScan br = new brTstScan())
            {

                if (br.Retrievetst(mysnno, nst_no, cu_no) != null)
                {
                    context.Response.Write(MesExcep("重复调拨！"));
                    return;
                }


                //获取最后的库别
                var item = br.Query(mysnno, reTime).FirstOrDefault();
                if(item!=null)
                {
                    lastst_no = item.nst_no.Trim();
                }

               
                sal_tst en = new sal_tst();


                en.mqty = pqty;
                en.p_no = p_no;
                en.pname = pname;
                en.ship_no = ship_no;
                en.unitcode = cu_no;   //操作人员
                en.bsnno = mysnno;
                en.nst_no = nst_no;
                en.ost_no = cu_no;

                en.ship_date = DateTime.Now;


                try
                {
                    br.Insert(en);
                }
                catch (Exception ex)
                {
                    Log.Error("APPORDER", ex.Message);
                    hs["success"] = false;
                    hs["Message"] = "插入记录失败";
                }
            }


            hs.Add("pname", pname);
            hs.Add("p_no", p_no);
            hs.Add("pqty", pqty);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string teml = jss.Serialize(hs);
            Log.Debug("recan", teml);//记录入库扫描明细
            context.Response.Write(teml);
            return;
        }
        #endregion

        #region json 公共类
        /// <summary>
        /// json 公共类
        /// </summary>
        /// <param name="strMemo"></param> 
        /// <param name="logPath"></param>  
        public string Mes()
        {
            return "{\"success\":true,\"Message\":\"" + "" + "\"}";
        }
        public string Mes(string s)
        {
            if (s == null || s == "")
                s = "[]";
            if (s.Substring(0, 1) != "[" && s.Substring(0, 1) != "{")
            {
                s = "\"" + s + "\"";
            }
            return "{\"success\":true,\"Message\":\"" + "" + "\",\"cont\":" + s + "}";
        }
        public string Mes(string s, string list)
        {
            if (list == null || list.Trim() == "")
                list = "[]";
            // string callback = Request["callback"];
            return "{\"success\":true,\"Message\":\"" + "" + "\",\"cont\":" + s + ",\"list\":" + list + "}";
        }
        public string MesExcep(string s)
        {
            // string callback = Request["callback"];
            return "{\"success\":false,\"Message\":\"" + s + "\"}";
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
