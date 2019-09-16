using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hmbzbfwfc.Models
{
    public class MsgException:Exception
    {
        public MsgException() { }
        public MsgException(string str) : base(str) { }
    }
}