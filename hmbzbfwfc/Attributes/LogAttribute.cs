using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Attributes
{
    public class LogAttribute : HandleErrorAttribute  
    {
        //创建一个队列
        public static Queue<Exception> execptionQueue = new Queue<Exception>();
        /// <summary>
        /// 可捕获异常数据
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;
            //把错误信息写进队列
            execptionQueue.Enqueue(ex);
            //跳转到错误页
            filterContext.HttpContext.Response.Redirect("/Index/ErrorInfo"); 
        }

    }
}