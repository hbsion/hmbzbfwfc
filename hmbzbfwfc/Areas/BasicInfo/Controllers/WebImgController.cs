using hmbzbfwfc.Attributes;
using hmbzbfwfc.Commons;
using hmbzbfwfc.Controllers;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Areas.BasicInfo.Controllers
{
    public class WebImgController : ManageBaseController
    {
        // GET: BasicInfo/WebImg
        public ActionResult Index()
        {
            return View();
        }
        [Manage]
        public ActionResult WebImgList()
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
            var cu_no = Request["cu_no"];
            var cuparme = "";
            if (!string.IsNullOrEmpty(cu_no))
            {
                parme += "&cu_no=" + cu_no;
                cuparme += "and unitcode=" + cu_no;
            }

            ConnectionStringEntities db = new ConnectionStringEntities();
            PagesImpl pageImpl = new PagesImpl(db);
            var data = pageImpl.GetPageDate<xt_images>("xt_images", pageno, pagesize, "*", "fid desc", "1=1 "+cuparme+"");


            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            ViewBag.url = "WebImgList";
            return View();
        }

        [Manage]
        public ActionResult WebImgEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.xt_images.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WebImgAddOrUpd(xt_images ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            xt_images ip = dbHelpers.xt_images.FirstOrDefault(db => db.fid == ins.fid);
            
            if (ip == null)
            {
                if (string.IsNullOrEmpty(ins.tempimage) )
                    return Json(new { status = "error", data = "图片不能为空！" });

                dbHelpers.xt_images.Add(ins);
            }
            else
            {
                ip.tempimage = ins.tempimage;
                ip.img_name = ins.img_name;
                ip.sindex = ins.sindex;
                ip.reurl = ins.reurl;
                
            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public JsonResult WebImgDel()
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
                        var umodel = dbHelpers.xt_images.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.xt_images.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.xt_images.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.xt_images.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
       
    }
}