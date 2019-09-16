using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using marr.BusinessRule;
using marr.DataLayer;
using marr.BusinessRule.Entity;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UI.WXPay;

namespace UI.w
{

    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string url = Request.Url.ToString();


                int t = url.LastIndexOf("?");
                string fwcode = "";


                if (t > 0 && url.Length > (t + 1))
                {
                    fwcode = url.Substring(t + 1, url.Length - t - 1);
                }


                fwcode = fwcode.Replace("?", "");

                fwcode = fwcode.Replace("=", "");


                if (fwcode.Length > 0)
                {

                    Session["fwtype"] = "1";

                    Session["fwcode"] = fwcode;


                    //Barcode barcode = null;



                    //using (brBarcode br = new brBarcode())
                    //{

                    //    barcode = br.GetBarcodeEntity(fwcode, "");

                    //}
                }
                


                StringBuilder infoBuilder = new StringBuilder();
                StringBuilder infopot = new StringBuilder();

                int js = 0;
                using (brImages brimgs = new brImages())
                {
                    foreach (var item in brimgs.Query())
                    {


                        if (js == 0)
                        {
                            infopot.Append(" <li data-target=\"#myCarousel\" data-slide-to=\"" + js.ToString() + "\" class=\"active\"></li>");

                            infoBuilder.Append("<div class='item active topslideimg'><img src='" + item.tempimage + "'  width='100%'> </div> ");
                        }
                        else
                        {
                            infopot.Append(" <li data-target=\"#myCarousel\" data-slide-to=\"" + js.ToString() + "\" ></li>");

                            infoBuilder.Append("<div class='item topslideimg'><img src='" + item.tempimage + "'  width='100%'> </div> ");
                        }


                        js++;
                    }


                    if (js > 0)
                    {

                        demo3.InnerHtml = infoBuilder.ToString();
                     //   ptop.InnerHtml = infopot.ToString();
                    }

                }

            }



        }







        public string get_urlencode(string postusl)
        {
            return HttpUtility.UrlEncode(postusl);
        }

       


    }
}
