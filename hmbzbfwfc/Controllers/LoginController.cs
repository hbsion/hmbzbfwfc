using hmbzbfwfc.Attributes;
using hmbzbfwfc.Models;
using hmbzbfwfc.Commons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Controllers
{    
    public class LoginController : Controller
    {
        // GET: Login
       
        public ActionResult Index()
        {
            ViewBag.logintype = "user";
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            ViewBag.pp = dbHelpers.xt_unit.OrderBy(db => db.fid);
            return View();
        }

        public ActionResult CustomerLogin() {
            ViewBag.logintype = "customer";
            return View("Index");
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        public JsonResult Login() 
        {
            var logintype=Request["logintype"];

            var yzm = Request["yzm"];
            if (string.IsNullOrEmpty(yzm))
                return Json(new { status = "error", data = "验证码不能为空" }, "text/html");
            try
            {
                if (yzm != Session["ValidateCode"].ToString())
                    return Json(new { status = "error", data = "验证码不匹配" }, "text/html");
            }
            catch
            {
                return Json(new { status = "error", data = "验证码失效" }, "text/html");
            }


            var uname=Request["uname"];
            var password=Request["pwd"];
            var unitcode=Request["unitcode"];   //品牌
            if (unitcode==null)
            {
                unitcode = "";
            }
            string username = "";
            string sysrole = "";
            string delyn="";
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            password = Uties.CMD5(password);
            string fid = "";
            if (logintype == "user") { 
            var umodel=dbHelpers.Gy_Czygl.FirstOrDefault(db=>db.czybm==uname && db.czmm==password );
            if (umodel == null)
                return Json(new { status = "error", data = "用户名或密码错误" }, "text/html");
            else
            {
                username = umodel.czymc;
                sysrole = umodel.sysrole;
                   delyn=umodel.checkyn;   

            }

            fid = umodel.fid.ToString();

            }else if (logintype == "customer")
            {
                var umodel = dbHelpers.customer.FirstOrDefault(db => db.cu_no == uname && db.passwd == password);
                if (umodel == null)
                    return Json(new { status = "error", data = "经销商编号错误或密码错误" }, "text/html");
                else
                    username = umodel.cu_name;

                fid = umodel.fid.ToString();
            }


                      
            HttpCookie cookie = new HttpCookie("loginuid");
            cookie.Value = DESEncrypt.Encrypt(fid);
            cookie.Expires = DateTime.Now.AddMinutes(20);                        
            Response.Cookies.Add(cookie);
            HttpCookie tcookie = new HttpCookie("logintype");
            tcookie.Value = logintype;
            tcookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(tcookie);

            HttpCookie ucookie = new HttpCookie("unitcode");
            ucookie.Value = DESEncrypt.Encrypt(unitcode);
            ucookie.Expires = DateTime.Now.AddMinutes(20);
            Response.Cookies.Add(ucookie);


            //保存  Session["User"] 
            User User = new User();
            User.UserNo = uname;
            User.UserName = username;
            if (logintype == "user")
            {
                User.UserType = "u";
                User.xtcu_no = "";
            }
            else
            {
                User.xtcu_no = uname; 
                User.UserType = "c";
            }

            User.xtsysrole = sysrole;  //系统角色
            User.xtunitcode = unitcode; // 品牌编号
            User.xtdelyn = delyn;

            string unitname = "";
            if (unitcode.Length > 0)
            {
                var unit = dbHelpers.xt_unit.FirstOrDefault(db => db.unitcode == unitcode);
                if (unit != null)
                    unitname = unit.unitname;
            }

            User.xtunitname = unitname;
            User.xtfid = int.Parse(fid);


            Session["User"] = User;


            return Json(new  { status="success" },"text/html");
        }

        public ActionResult UserExit()
        {

            if (Request.Cookies["loginuid"] != null)
                Response.Cookies["loginuid"].Expires = DateTime.Now.AddDays(-1);

            Session["User"] = null;
            Session.Abandon();

            return RedirectToAction("Index", "Login");

        }

        public ActionResult addcus()
        {
            ReadXml rd = new ReadXml();
            rd.readxml();
            return Content("ok");
        }
    }
}