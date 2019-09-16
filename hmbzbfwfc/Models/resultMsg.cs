
using Newtonsoft.Json;
using System.Net;
using System.Web.Mvc;

namespace hmbzbfwfc.Models
{
    public class resoultMsg
    {
        public bool flag { get; set; }
        public object data { get; set; }
        public string msg { get; set; }
    }
    public class resoultMsgStr : ActionResult
    {
        public resoultMsg resoultmsg;
        public string contentText;
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.TrySkipIisCustomErrors = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "application/json";
            response.Write(this.contentText);
            response.End();
        }
        public resoultMsgStr()
            : this(false, "", "fair")
        {

        }
        public resoultMsgStr(string _msg)
            : this(true, "", _msg)
        {
        }
        public resoultMsgStr(bool flag, string msg)
            : this(flag, "", msg)
        {
        }
        public resoultMsgStr(bool flag, object data, string msg)
        {
            resoultmsg = new resoultMsg();
            resoultmsg.data = data;
            resoultmsg.flag = flag;
            resoultmsg.msg = msg;
            contentText = JsonConvert.SerializeObject(resoultmsg);
           
        }
        public resoultMsgStr(object data)
            : this(true, data, "success")
        {

        }
    }
}
