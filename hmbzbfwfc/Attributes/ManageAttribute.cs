using System;
using System.Web;
using System.Web.Mvc;
using hmbzbfwfc.Controllers;
using hmbzbfwfc.Models;

namespace hmbzbfwfc.Attributes
{
    public class ManageAttribute : AuthorizeAttribute, IResultFilter
    {
        /// <summary>
        /// 前后台及接口 LoginAllow
        /// </summary>
        public int Allow { get; set; }
        /// <summary>
        /// 角色权限 PowerType
        /// </summary>
        public int Role { get; set; }
        private bool Right { get; set; }
        private string UrlTo { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Right;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            Right = true;
            var context = filterContext.RequestContext.HttpContext;
            try
            {
                string code = context.Request.Cookies.Count > 0 ? context.Request.Cookies["loginuid"].Value : "";

                string tcode = context.Request.Cookies.Count > 0 ? context.Request.Cookies["logintype"].Value : "";

                string ucode = context.Request.Cookies.Count > 0 ? context.Request.Cookies["unitcode"].Value : "";

                
                if (string.IsNullOrEmpty(tcode))
                {
                    tcode = context.Request.Headers["logintype"];
                }
                if (string.IsNullOrEmpty(tcode))
                {
                    UrlTo = "/Login/Index";
                    Right = false;
                }

                if (string.IsNullOrEmpty(code))
                {
                    code = context.Request.Headers["loginuid"];
                }
                if (string.IsNullOrEmpty(code))
                {
                    UrlTo = "/Login/Index";
                    Right = false;
                }

                if (string.IsNullOrEmpty(ucode))
                {
                    ucode = context.Request.Headers["unitcode"];
                }
                if (string.IsNullOrEmpty(ucode))
                {
                    UrlTo = "/Login/Index";
                    Right = false;
                }
            }
            catch (Exception e)
            {
                UrlTo = "/Login/Index";
                Right = false;
            }
            base.OnAuthorization(filterContext);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var context = filterContext.RequestContext.HttpContext;
            if (context.Request.Cookies["loginuid"] != null)
            {                
                var cookuid = context.Request.Cookies["loginuid"];
                cookuid.Expires = DateTime.Now.AddMinutes(20);
                context.Response.Cookies.Add(cookuid);
            }
            if (context.Request.Cookies["logintype"] != null)
            {
                var cookt = context.Request.Cookies["logintype"];
                cookt.Expires = DateTime.Now.AddDays(1);
                context.Response.Cookies.Add(cookt);
            }
            if (context.Request.Cookies["unitcode"] != null)
            {
                var cookunitcode = context.Request.Cookies["unitcode"];
                cookunitcode.Expires = DateTime.Now.AddMinutes(20);
                context.Response.Cookies.Add(cookunitcode);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            
                filterContext.Result = new RedirectResult(UrlTo);
        }
    }
}