using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace UI
{
    public class myunit
    {
  

        public static string Encode(string text)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] input = System.Text.Encoding.Default.GetBytes(text);
            return Convert.ToBase64String(md5.TransformFinalBlock(input, 0, input.Length));
        }

        public static string Mmjm(string Srmm)
        {
            int Zfcte;
            int Jsqte;
            string Mmjm = "";

            for (Jsqte = 0; Jsqte < Srmm.Length; Jsqte++)
            {
                Zfcte = (int)((Srmm.Substring(Jsqte, 1)).ToCharArray()[0]);
                Zfcte += (int)((Srmm.Substring(Srmm.Length - Jsqte - 1, 1)).ToCharArray()[0]);
                Zfcte += Srmm.Length + Jsqte + 1;

                Mmjm = Mmjm + Zfcte.ToString();
            }
            return Mmjm;
        }

    }

}
