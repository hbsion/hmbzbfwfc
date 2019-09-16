using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using hmbzbfwfc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace hmbzbfwfc.Areas.BasicInfo.Controllers
{
    public class StoreController : Controller
    {
        // GET: BasicInfo/Store
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 库别列表
        /// </summary>
        /// <returns></returns>
        [Manage]
        public ActionResult StoreList()
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
            var data = pageImpl.GetPageDate<Inv_Store>("Inv_store", pageno, pagesize, "*", "fid desc", "st_name like '%" + Storename + "%'");


            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        [Manage]
        public ActionResult StoreEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.Inv_Store.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult StoreAddOrUpd(Inv_Store ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            Inv_Store ip = dbHelpers.Inv_Store.FirstOrDefault(db => db.fid == ins.fid);
            ins.in_date = DateTime.Now;
            if (ip == null)
            {
                if (string.IsNullOrEmpty(ins.st_no) || string.IsNullOrEmpty(ins.st_name))
                    return Json(new { status = "error", data = "库别编号或名称不能为空！" });
                var sm = dbHelpers.Inv_Store.FirstOrDefault(db => db.st_no == ins.st_no);
                if (sm != null)
                    return Json(new { status = "error", data = "该库别编号已被使用！" });

                dbHelpers.Inv_Store.Add(ins);
            }
            else
            {
                if (string.IsNullOrEmpty(ins.st_name))
                    return Json(new { status = "error", data = "库别名称不能为空！" });


                //ins 为提交的内容， ip为原有的内容
                PropertyInfo[] ippropertys = ip.GetType().GetProperties();
                foreach (PropertyInfo property in ippropertys)
                {
                    var myvalue = property.GetValue(ins, null);  // 获取提交来的值
                    if (property.GetValue(ip, null) != myvalue)  //有修改的内容
                    {
                        if (myvalue == null && property.GetValue(ip, null).GetType() == typeof(string))  //如果是字符串的则为空
                        {
                            property.SetValue(ip, "", null);
                        }
                        else
                        {
                            if (myvalue != null)  //其他类型为NULL时不替换
                            {
                                property.SetValue(ip, myvalue, null);
                            }
                        }
                    }
                }

       

            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }


  

        [Manage]
        public JsonResult StoreDel()
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
                        var umodel = dbHelpers.Inv_Store.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.Inv_Store.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.Inv_Store.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.Inv_Store.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
    }
}