using System;
using System.Collections.Generic;
using System.Web;

namespace UI.WXPay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}