using hmbzbfwfc.Attributes;
using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Areas.BasicInfo.Controllers
{
    public class SalesManController : Controller
    {
        // GET: BasicInfo/SalesMan


        public ActionResult Index()
        {
            return View();
        }

        [Manage]
        public ActionResult SalesManList()
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
            var data = pageImpl.GetPageDate<hy_dian>("hy_dian", pageno, pagesize, "*", "fid desc", "fid  like '%" + Storename + "%' or hy_no like '%" + Storename + "%'");


            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

       
        public ActionResult SalesManSh(string fid)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            
            var model = dbHelpers.hy_dian.FirstOrDefault(db => db.fid.ToString() == fid);
            model.statu = 4;
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public ActionResult SalesManEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.hy_dian.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SalesManAddOrUpd(hy_dian ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            hy_dian ip = dbHelpers.hy_dian.FirstOrDefault(db => db.fid == ins.fid);
            ins.in_date = DateTime.Now;

            if (string.IsNullOrEmpty(ins.hy_no) || string.IsNullOrEmpty(ins.hy_cn))
                return Json(new { status = "error", data = "必填不能为空！" });
            if (ip == null)
            {               
                var sm = dbHelpers.Inv_Store.FirstOrDefault(db => db.st_no == ins.hy_no);
                if (sm != null)
                    return Json(new { status = "error", data = "该促销员编号已被使用！" });

                dbHelpers.hy_dian.Add(ins);
            }
            else
            {               

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
        public JsonResult SalesManDel()
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
                        var umodel = dbHelpers.hy_dian.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.hy_dian.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.hy_dian.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.hy_dian.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
    }
}