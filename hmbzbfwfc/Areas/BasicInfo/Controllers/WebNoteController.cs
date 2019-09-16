using hmbzbfwfc.Attributes;
using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Areas.BasicInfo.Controllers
{
    public class WebNoteController : Controller
    {
        // GET: BasicInfo/WebNote
        public ActionResult Index()
        {
            return View();
        }
        [Manage]
        public ActionResult WebNoteList()
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
            var selparme = Request["selparme"];
            if (!string.IsNullOrEmpty(selparme))
                parme += "selparme=" + selparme;

            ConnectionStringEntities db = new ConnectionStringEntities();
            PagesImpl pageImpl = new PagesImpl(db);
            var data = pageImpl.GetPageDate<xt_webnote>("xt_webnote", pageno, pagesize, "*", "fid desc", "sys_title like '%" + selparme + "%'");


            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            ViewBag.url = "WebNoteList";
            return View();
        }

        [Manage]
        public ActionResult WebNoteEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.xt_webnote.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WebNoteAddOrUpd(xt_webnote ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            xt_webnote ip = dbHelpers.xt_webnote.FirstOrDefault(db => db.fid == ins.fid);

            
            if (ip == null)
            {
                if (string.IsNullOrEmpty(ins.sys_no) || string.IsNullOrEmpty(ins.sys_title))
                    return Json(new { status = "error", data = "内容选项不能为空！" });

                var sm = dbHelpers.xt_webnote.FirstOrDefault(db => db.sys_no == ins.sys_no);
                if (sm != null)
                    return Json(new { status = "error", data = "内容选项不能重复！" });
                ins.tcode = 1;
                dbHelpers.xt_webnote.Add(ins);
            }
            else
            {                
                ip.sys_title = ins.sys_title;
                ip.sys_note = ins.sys_note;
                ip.remark = ins.remark;
            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        public ActionResult ShowWebContent() {
            var fid = Request["fid"];
            ConnectionStringEntities db = new ConnectionStringEntities();
            var model = db.xt_webnote.FirstOrDefault(x => x.fid.ToString() == fid);
            ViewBag.model = model;
            return View();
        
        }

        [Manage]
        public JsonResult WebNoteDel()
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
                        var umodel = dbHelpers.xt_webnote.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.xt_webnote.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.xt_webnote.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.xt_webnote.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
       
    }
}