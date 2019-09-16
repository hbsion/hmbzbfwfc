using hmbzbfwfc.Attributes;
using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Controllers
{
    public class IndexController : ManageBaseController
    {
        // GET: Index
        [Manage]
        public ActionResult WelCome()
        {
            return View();
        }

        public ActionResult MainMenu()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            HttpCookie cookiet = Request.Cookies["logintype"];
            var logintype = cookiet.Value;

            if (logintype == "customer")
                return View();

            HttpCookie cookieuid = Request.Cookies["loginuid"];
            var uid = DESEncrypt.Decrypt(cookieuid.Value);

            var umodel = dbHelpers.Gy_Czygl.FirstOrDefault(db => db.fid.ToString() == uid);


            if (umodel.czybm == "admin")
            {
                var onemenu = dbHelpers.xt_xtmenu.Where(db => string.IsNullOrEmpty(db.sjgnbm) && db.MenuList == 1).OrderBy(db=>db.gnbm);
                string menu = "";
                foreach (var item in onemenu)
                {
                    menu += "<li>";
                    menu += "<a href='#'><i class='fa " + item.icostyle + "'></i><span class='nav-label'>" + item.gnmc + "</span><span class='fa arrow'></span></a>";
                    var twomenu = dbHelpers.xt_xtmenu.Where(db => db.sjgnbm == item.gnbm).OrderBy(db => db.gnbm);
                    menu += "<ul class='nav nav-second-level'>";
                    foreach (var tm in twomenu)
                    {
                        menu += "<li><a class='J_menuItem' href='" + tm.acturl + "' >" + tm.gnmc + "</a></li>";
                    }
                    menu += "</ul>";
                    menu += "</li>";
                }
                ViewBag.menu = menu;
            }
            else
            {
                var cuspw = dbHelpers.xt_user_menu.Where(db => db.rms_name == umodel.rms_name);
                List<string> strList = new List<string>();
                foreach (var item in cuspw)
                {
                    strList.Add(item.munid.ToString());
                }
                string[] pwarray = strList.ToArray();

                var allmenu = dbHelpers.xt_xtmenu.OrderBy(db => db.fid);
                List<xt_xtmenu> xtmenu = new List<xt_xtmenu>();
                foreach (var item in allmenu)
                {
                    bool exists = ((IList)pwarray).Contains(item.fid.ToString());
                    if (exists)
                    {
                        var amenu = dbHelpers.xt_xtmenu.FirstOrDefault(db => db.fid == item.fid);
                        xtmenu.Add(amenu);
                    }
                }
                var onemenu = xtmenu.Where(db => string.IsNullOrEmpty(db.sjgnbm) && db.MenuList == 1).OrderBy(db => db.gnbm);
                string menu = "";
                foreach (var item in onemenu)
                {
                    menu += "<li>";
                    menu += "<a href='#'><i class='fa " + item.icostyle + "'></i><span class='nav-label'>" + item.gnmc + "</span><span class='fa arrow'></span></a>";
                    var twomenu = xtmenu.Where(db => db.sjgnbm == item.gnbm);
                    menu += "<ul class='nav nav-second-level'>";
                    foreach (var tm in twomenu)
                    {
                        menu += "<li><a class='J_menuItem' href='" + tm.acturl + "' >" + tm.gnmc + "</a></li>";
                    }
                    menu += "</ul>";
                    menu += "</li>";
                }
                ViewBag.menu = menu;

            }
            return View();
        }

        public ActionResult ErrorInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetProd(string pname_no)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var unitcode = this.xtUser.xtunitcode;

            var prodm = dbHelpers.Inv_Part.Where(db => db.unitcode == unitcode).ToList();
            if (!string.IsNullOrEmpty(pname_no))
                prodm = prodm.Where(db => (db.p_no.Contains(pname_no) || db.pname.Contains(pname_no))).ToList();

            return Json(prodm);
        }

        [HttpPost]
        public ActionResult GetCustomer(string parme, string xtcuno)
        {

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var unitcode = this.xtUser.xtunitcode;
            var prodm = dbHelpers.customer.Where(db => db.unitcode == unitcode).ToList();
            if (!string.IsNullOrEmpty(parme))
                prodm = prodm.Where(db => (db.cu_no.Contains(parme) || db.cu_name.Contains(parme))).ToList();
            if (!string.IsNullOrEmpty(xtcuno))
                prodm = prodm.Where(db => db.xtcu_no == xtcuno).ToList();
            return Json(prodm);
        }

    }


}