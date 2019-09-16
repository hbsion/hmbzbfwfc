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

    public partial class sy : System.Web.UI.Page
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


                if (fwcode.Length == 0)
                    return;


                Barcode barcode =null;

   

                using (brBarcode br = new brBarcode())
                {

                    barcode = br.GetBarcodeEntity(fwcode,"");

                }

                //    Response.Redirect("/w/index?c="+fwcode);



            }



        }







        public string get_urlencode(string postusl)
        {
            return HttpUtility.UrlEncode(postusl);
        }

       


    }
}
