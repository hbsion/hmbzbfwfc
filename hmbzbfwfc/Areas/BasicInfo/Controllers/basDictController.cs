using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using hmbzbfwfc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Areas.BasicInfo.Controllers
{
    public class basDictController : Controller
    {
        // GET: BasicInfo/basDict
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 字典列表
        /// </summary>
        /// <returns></returns>
        [Manage]
        public ActionResult basDictList()
        {
            int pageno = 1;
            if (!string.IsNullOrEmpty(Request["pageno"]))
            {
                pageno = int.Parse(Request["pageno"]);
            }
            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request["pagesize"]))
            {
                pagesize = int.Parse(Request["pagesize"]);
            }

            string parme = "";
            var Storename = Request["Storename"];
            if (!string.IsNullOrEmpty(Storename))
                parme += "Storename=" + Storename;

            ConnectionStringEntities db = new ConnectionStringEntities();
            PagesImpl pageImpl = new PagesImpl(db);
            var data = pageImpl.GetPageDate<bas_dict>("bas_dict", pageno, pagesize, "*", "fid desc", "bm_name like '%" + Storename + "%'");


            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;

            ViewBag.url = "basDictList";

            return View();
        }

        [Manage]
        public ActionResult basDictEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.bas_dict.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

        [HttpPost]
        public ActionResult basDictAddOrUpd(bas_dict ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            bas_dict ip = dbHelpers.bas_dict.FirstOrDefault(db => db.fid == ins.fid);

            if (ip == null)
            {
                if (string.IsNullOrEmpty(ins.bm_no) || string.IsNullOrEmpty(ins.bm_name))
                    return Json(new { status = "error", data = "字典编号或名称不能为空！" });
                var sm = dbHelpers.bas_dict.FirstOrDefault(db => db.bm_no == ins.bm_no);
                if (sm != null)
                    return Json(new { status = "error", data = "该字典编号已被使用！" });
              
                ins.tcode = 1;
                ins.unitcode = "";

                dbHelpers.bas_dict.Add(ins);
            }
            else
            {
                if (string.IsNullOrEmpty(ins.bm_name))
                    return Json(new { status = "error", data = "字典名称不能为空！" });
                ip.bm_name = ins.bm_name;
  
            }

            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public JsonResult basDictDel()
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
                        var umodel = dbHelpers.bas_dict.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.bas_dict.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.bas_dict.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.bas_dict.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
    }
}