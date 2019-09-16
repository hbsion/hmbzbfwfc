using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hmbzbfwfc.Models;


namespace hmbzbfwfc.Controllers
{
    public class ManageBaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            LoadResource();
        }


        protected User xtUser
        {
            get
            {
                return Session["User"] as User;
            }
        }


        public void LoadResource()
        {

            if (Session["User"]!=null)
            {
                User myUser = (User)Session["User"];

                if (myUser.UserType == "u")
                {
                    ViewBag.userno = myUser.UserNo;
                    ViewBag.username = myUser.UserName;
                    ViewBag.logintype = "u";
                    ViewBag.powertype = myUser.xtsysrole;
                    ViewBag.unitcode = myUser.xtunitcode;
                    ViewBag.unitname = myUser.xtunitname;
                    ViewBag.delyn = myUser.xtdelyn;
                }
                else
                {

                    ViewBag.cuno = myUser.UserNo;
                    ViewBag.username = myUser.UserName;
                    ViewBag.logintype = "c";
                }
                ViewBag.uid = myUser.xtfid.ToString();


            }
            else
            {
                Response.Redirect("/login/index");   
            }


        
        }

    }
}