using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using marr.BusinessRule;
using marr.DataLayer;
using System.Configuration;

using marr.BusinessRule.Entity;

namespace UI.app
{
    public partial class wl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string fwcode = this.Request["c"];
                string cu_no=Request["cu_no"];
                string userId = Request["u"];
                string passwd = Request["p"];

                //判断用户
                if (userId != null && passwd != null)
                {

                    using (brCzygl br = new brCzygl())
                    {
                        Gy_Czygl user = null;
                        if (br.CheckLogin(userId, passwd, out user))
                        {
                            //

                        }
                        else
                        {
                            show.InnerHtml = "由【总部】发货！";
                            return;
                        }

                    }
                }


                if (Session["fwtype"] != null && Session["fwtype"].ToString() == "2" && Session["wlcode"]!=null)  //类型  防伪码查
                {
                  fwcode =  Session["wlcode"].ToString();

                    //用防伪码算出物流码

                }
     
               
                if (fwcode != null && fwcode.Trim().Length > 6)
                {
                    wlcx(fwcode,cu_no);
                }
            }
        }
        private void wlcx(string mysnno, string cu_no)
        {
            StringBuilder strPro = new StringBuilder();//产品信息
            StringBuilder strPack = new StringBuilder();//包装信息
            StringBuilder strWl = new StringBuilder();//物流信息
            Barcode barcode;
            string p_no = "";

            using (brBarcode br = new brBarcode())
            {
                barcode = br.GetBarcodeEntity(mysnno,  "");
            }
            Boolean findok = true;//是否显示结果
          
            
            string xtcuwlyn = "Y";
           try
           {
               xtcuwlyn = ConfigurationSettings.AppSettings["xtcuwlyn"].ToString();   //是否限定经销商查询物流
            }
          catch { }


   //       if (cu_no != "admin" && xtcuwlyn == "Y")
   //            findok = false;

            if (barcode != null)
            {
                p_no = barcode.p_no;
                strPack.Append(" <div class=\"wrap\"><div class=\"main\"><div class=\"content\"><div class=\"lay-title\">包装规格</div><div class=\"prod\"><div class=\"prod-cont\">");
                strPack.Append("  物流码：" + barcode.Code + "<br/> ");

                if ((barcode.Tcode != null && barcode.Tcode.Length > 0) || (barcode.Ucode != null && barcode.Ucode.Length > 0))
                    if (barcode.Tcode != barcode.Ucode)
                        strPack.Append("  上级物流码：" + barcode.Tcode + "/" + barcode.Ucode + "<br/> ");
                    else
                        strPack.Append("  上级物流码：" + barcode.Tcode + "<br/> ");
                else
                    strPack.Append("  大箱物流码或单个包装" + "<br/> ");

                using (brPackb bp = new brPackb())
                {
                    var bpb = bp.Retucodes(barcode.Code);
                    int b = 0;
                    foreach (var item in bpb)
                    {
                        if (b == 0)
                            strPack.Append("   下级物流码：" + item.snno + "   /   ");
                        else
                            strPack.Append(item.snno + "   /   ");

                        b++;
                    }
                }

                strPack.Append("  <br/>包装数量：" + Convert.ToString(barcode.Qty) + "<br/> ");
                strPack.Append(" </div> </div></div> </div> </div>");
            }
            else
            {
                findok = true;
                barcode = new Barcode();
                barcode.Code = mysnno;
                barcode.Qty = 1;
                strPack.Append(" <div class=\"wrap\"><div class=\"main\"><div class=\"content\"><div class=\"lay-title\">包装规格</div><div class=\"prod\"><div class=\"prod-cont\">");
                strPack.Append("  单个包装 , 包装数量： " + Convert.ToString(barcode.Qty) + "</div> </div></div> </div> </div> ");
            }
            DataSet ds = new DataSet();
            DataTable tb = new DataTable("table_ax");
            tb.Columns.Add("cont", Type.GetType("System.String"));
            tb.Columns.Add("date", Type.GetType("System.DateTime"));




            //入库

            using (brFgiScan br = new brFgiScan())
            {

                foreach (var item in br.Query(barcode, "", ""))
                {
                    if (item.p_no != null && item.p_no.Length > 0 && p_no.Length == 0)
                        p_no = item.p_no;

                    DataRow dr = tb.NewRow();
                    dr["cont"] = "【入库】";
                    dr["date"] = item.ship_date;
                    dr["cont"] += " 入库单号：" + item.ship_no   + " <br/>";                   


                    tb.Rows.Add(dr);


                }
            }

            using (brShipScan br = new brShipScan())
            {

                foreach (var item in br.Query(barcode, "", ""))
                {
                    if (item.p_no != null && item.p_no.Length > 0 && p_no.Length == 0)
                        p_no = item.p_no;

                    DataRow dr = tb.NewRow();
                    dr["cont"] = "【总部】发往";
                    dr["date"] = item.ship_date;

                    using (brCustomer brcu = new brCustomer())
                    {
                        customer em = brcu.Retrieve(item.cu_no);
                        if (em != null)
                        {
                            dr["cont"] += "    单号：" + item.ship_no+"<br/>";
                            dr["cont"] += "    代理商：" + em.cu_no + " " + em.cu_name + " (" + em.province + em.city + ")<br/>";
 
                        }

                    }
                    tb.Rows.Add(dr);

                    try
                    {
                        if (findok == false && cu_no.Trim() == item.cu_no.Trim())
                            findok = true;
                    }
                    catch { }
                }
            }

            //退货

            using (brReScan br = new brReScan())
            {

                foreach (var item in br.Query(barcode, "", ""))
                {
                    DataRow dr = tb.NewRow();

                    

                    dr["cont"] = "产品退货，退货单号：" + item.ship_no + "<br/> ";
                    using (brCustomer brcu = new brCustomer())
                    {
                        customer em = brcu.Retrieve(item.cu_no);
                        if (em != null)
                        {
                            dr["cont"] += "      退货经销商：" + em.cu_no + "   " + em.cu_name + "(" + em.province + em.city + ") <br/>";
                         //   dr["cont"] += "    级别：" + em.gclass + "  微信号：" + em.WxId + " <br/>";
                        }
                    }
                    dr["date"] = item.ship_date;
                    tb.Rows.Add(dr);
                }
            }





            //多级经销商出货

            using (brCuShipScan br = new brCuShipScan())
            {

                foreach (var item in br.Query(barcode, "", ""))
                {
                    DataRow dr = tb.NewRow();

                    dr["cont"] = "【经销商】" + item.xtcu_no +  item.xtcu_name  + " 出货";
                    dr["date"] = item.ship_date;

                    using (brCustomer brcu = new brCustomer())
                    {
                        customer em = brcu.Retrieve(item.cu_no);
                        if (em != null)
                        {
                            dr["cont"] += "    经销商：" + em.cu_no + " " + em.cu_name + " (" + em.province + em.city + ")<br/>";

                        }
                    }
                    dr["cont"] += ",单号：" + item.ship_no;
                    tb.Rows.Add(dr);
                    try
                    {
                        if (findok == false && cu_no.Trim() == item.cu_no.Trim())
                            findok = true;
                    }
                    catch { }
                }
            }





            if (p_no != null && p_no.Length > 0)
            {
                using (brProduct br = new brProduct())
                {
                    Inv_Part invpart = br.Retrieve(p_no.Trim(), "");
                    if (invpart != null)
                    {
                        strPro.Append("  <div class=\"wrap\"><div class=\"main\"><div class=\"content\"><div class=\"lay-title\">产品信息</div><div class=\"prod\"><div class=\"prod-img\"><div class=\"prod-img-shadow\">");
                        if (invpart.imgurl != null && invpart.imgurl.Length > 0)
                        {
                            strPro.Append(" <img src='" + invpart.imgurl + "' style=\"width:200px;\"  /> ");
                        }
                        else
                        {
                            strPro.Append(" <img alt='prod' src=''  /> ");
                        }
                        strPro.Append("</div></div><div class=\"prod-cont\">");
                        strPro.Append("产品编号：" + invpart.p_no + "<br/> ");
                        strPro.Append("产品名称：" + invpart.pname + "<br/> ");
                        strPro.Append("产品规格：" + invpart.type + "<br/> ");
                        //strPro.Append("产品价格：" + invpart.price.ToString() + "<br/> ");
                        strPro.Append(" </div></div> </div> </div></div>");

                    }
                }
            }
            if (findok==true)
                show.InnerHtml = strPro.ToString() + strPack.ToString() + " <div class=\"wrap\"> <div class=\"main\"> <div class=\"content\"><div class=\"lay-title\">物流追踪</div>   <ul class=\"myUl\">" + retunTB(tb) + " </ul> </div></div> </div>";

        }
        private string retunTB(DataTable tb)
        {
            DataView dv = tb.DefaultView;
            dv.Sort = "date Desc";
            string temp = " ";
            int i = dv.ToTable().Rows.Count;
            int ii = 1;
            foreach (DataRow item in dv.ToTable().Rows)
            {
                if (ii == 1)
                {
                    temp += retunWl(item["cont"].ToString(), item["date"].ToString(),1);//第一行
                }
                else if (ii >= i)
                {
                    temp += retunWl(item["cont"].ToString(), item["date"].ToString(), 2);//末行
                }
                else
                {
                    temp += retunWl(item["cont"].ToString(), item["date"].ToString(), 0);//中间行
                }

                ii++;
            }
            return temp;
        }
        private string retunWl(string cont, string date, int i)
        {
            string ret = "";
            if (i == 1)
            {
                //第一行 
                ret += "  <li class=\"myLi lay-bg-top\">";
            }
            else if (i == 2)
            {
                //末行
                ret += "  <li class=\"myLi lay-bg-foot\">";
            }
            else
            {
                //中间行
                ret += "  <li class=\"myLi lay-bg-cent\">";
            }
           // ret += "  <li class=\"myLi lay-bg-top\">";
            ret += " <div class=\"lay-main\"> <div class=\"lay-cont\">";
            ret += cont;
            ret += "</div> <div class=\"lay-date\">";
            try
            {
                ret += DateTime.Parse(date).ToString("yyyy-MM-dd HH:mm");
            }
            catch
            {
                ret += date;
            }
            ret += "</div></div> </li>";
            return ret;
        }
    }
}
