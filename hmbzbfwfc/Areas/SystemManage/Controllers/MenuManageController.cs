using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using hmbzbfwfc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Areas.SystemManage.Controllers
{
    public class MenuManageController : Controller
    {
        // GET: SystemManage/MenuManage
        public ActionResult Index()
        {
            return View();
        }

        public string getsjname(string bm)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var sjname = dbHelpers.xt_xtmenu.FirstOrDefault(db=>db.gnbm==bm);
            if (sjname != null)
                return sjname.gnmc;
            return "一级菜单";
        }

        [Manage]
        public ActionResult MenuList()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var onemenu = dbHelpers.xt_xtmenu.Where(db => string.IsNullOrEmpty(db.sjgnbm)).ToList().Select(item => new xt_xtmenu { 
             fid =item.fid,             
               gnbm=item.gnbm,
               gnsy=item.gnsy,
               gnmc=item.gnmc,
               sjgnbm=getsjname(item.sjgnbm),                        
               acturl=item.acturl,
               icostyle=item.icostyle
            });
            
            ViewBag.menu = onemenu;            
            return View();
        }

        [Manage]
        public ActionResult MenuXjList()
        {
            var gnbm=Request["gnbm"];

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var onemenu = dbHelpers.xt_xtmenu.Where(db => db.sjgnbm==gnbm).ToList().Select(item => new xt_xtmenu
            {
                fid = item.fid,
                gnbm = item.gnbm,
                gnsy = item.gnsy,
                gnmc = item.gnmc,
                sjgnbm = getsjname(item.sjgnbm),
                acturl = item.acturl,
                icostyle = item.icostyle
            });

            ViewBag.menu = onemenu;
            return View();
        }

        [Manage]
        public ActionResult MenuEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            var onemenu = dbHelpers.xt_xtmenu.Where(db => string.IsNullOrEmpty(db.sjgnbm));
            ViewBag.onemenu = onemenu;
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.xt_xtmenu.FirstOrDefault(db => db.fid.ToString() == fid);
            if (!string.IsNullOrEmpty(model.sjgnbm))
            {
                ViewBag.sjbm = model.sjgnbm;
                ViewBag.sjmc = dbHelpers.xt_xtmenu.FirstOrDefault(db => db.gnbm == model.sjgnbm).gnmc;
            }
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

        [HttpPost]
        public ActionResult MenuAddOrUpd(xt_xtmenu ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            xt_xtmenu ip = dbHelpers.xt_xtmenu.FirstOrDefault(db => db.fid == ins.fid);

            if (string.IsNullOrEmpty(ins.gnbm) || string.IsNullOrEmpty(ins.gnmc) )
                return Json(new { status = "error", data = "必填项不能为空！" });
            if (ip == null)
            {               
                var sm = dbHelpers.xt_xtmenu.FirstOrDefault(db => db.gnbm == ins.gnbm);
                if (sm != null)
                    return Json(new { status = "error", data = "该编号已被使用！" });

                dbHelpers.xt_xtmenu.Add(ins);
            }
            else
            {
                ip.gnmc = ins.gnmc;
                ip.sjgnbm = ins.sjgnbm;
                ip.acturl = ins.acturl;
                ip.remark = ins.remark;
            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public JsonResult MenuDel()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                var idlist = Request["idlist"];// checkitem复选框的名
                string[] strDelete = idlist.Split(',');
                foreach (var i in strDelete)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        var umodel = dbHelpers.xt_xtmenu.FirstOrDefault(db => db.fid.ToString() == i);
                        var xjmodel = dbHelpers.xt_xtmenu.Where(db=>db.sjgnbm==umodel.gnbm);
                        dbHelpers.xt_xtmenu.Remove(umodel);
                        dbHelpers.xt_xtmenu.RemoveRange(xjmodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.xt_xtmenu.FirstOrDefault(db => db.fid.ToString() == fid);
            var xjmodel2 = dbHelpers.xt_xtmenu.Where(db => db.sjgnbm == model.gnbm);            
            dbHelpers.xt_xtmenu.Remove(model);
            dbHelpers.xt_xtmenu.RemoveRange(xjmodel2);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
    }
}