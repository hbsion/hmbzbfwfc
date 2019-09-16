using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using hmbzbfwfc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using hmbzbfwfc.Controllers;


namespace hmbzbfwfc.Areas.BasicInfo.Controllers
{
    public class ProductController : ManageBaseController
    {
        // GET: BasicInfo/Product
        public ActionResult Index()
        {
            return View();
        }

        [Manage]


        public ActionResult ProductList()
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


            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            var productname = Request["productname"];

            List<Inv_Part> gclist = new List<Inv_Part>();

            var parme = "";
            if (!string.IsNullOrEmpty(productname))
            {
                gclist = dbHelpers.Inv_Part.Where(db => db.pname.Contains(productname) || db.p_no.Contains(productname) || db.type==productname).ToList();
                parme += "productname=" + productname;
            }
            else
            {
                gclist = dbHelpers.Inv_Part.OrderByDescending(db => db.fid).ToList();
            }


            


            var list = gclist.Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            var count = gclist.Count();
            var pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.logintype = this.xtUser.UserType;
            ViewBag.list = list;
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = pagecount;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        [Manage]
        public ActionResult ProductEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.Inv_Part.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }




        [HttpPost]
        public ActionResult ProductAddOrUpd(Inv_Part ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            Inv_Part ip = dbHelpers.Inv_Part.FirstOrDefault(db => db.fid == ins.fid);
            ins.in_date = DateTime.Now;
            ins.unitcode = this.xtUser.xtunitcode;   //企业或品牌代码

            if (ip == null)
            {
                if (string.IsNullOrEmpty(ins.p_no) || string.IsNullOrEmpty(ins.pname))
                    return Json(new { status = "error", data = "产品编号或名称不能为空！" });

                var sm = dbHelpers.Inv_Part.FirstOrDefault(db => db.p_no == ins.p_no );
                if (sm != null)
                    return Json(new { status = "error", data = "该产品编号已被使用！" });

                dbHelpers.Inv_Part.Add(ins);
            }
            else
            {
                if (string.IsNullOrEmpty(ins.pname))
                    return Json(new { status = "error", data = "产品名称不能为空！" });
                ip.p_no = ins.p_no;
                ip.pname = ins.pname;
                ip.type = ins.type;
                ip.unit = ins.unit;
                ip.parttype = ins.parttype;
                ip.pqty = ins.pqty;
                ip.price = ins.price;
                ip.jf = ins.jf;
                ip.bm_no = ins.bm_no;
                ip.remark = ins.remark;
                ip.imgurl = ins.imgurl;
                ip.remark2 = ins.remark2;
                ip.unitcode = this.xtUser.xtunitcode;   //企业或品牌代码

            }


            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public JsonResult ProductDel()
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
                        var umodel = dbHelpers.Inv_Part.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.Inv_Part.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.Inv_Part.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.Inv_Part.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }





        //导出产品
        [Manage]
        public FileResult ProducttoExcel()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            //获取list数据
            string url = HttpContext.Request.Url.ToString();
            int ii = url.LastIndexOf("/BasicInfo/Product/ProducttoExcel");
            url = url.Remove(ii);

            var productname = Request["productname"];

            List<Inv_Part> list = new List<Inv_Part>();
            if (!string.IsNullOrEmpty(productname))
            {
                list = dbHelpers.Inv_Part.Where(db => db.p_no.ToString().Contains(productname) || db.pname.Contains(productname) || db.type.Contains(productname) || db.bm_no.Contains(productname)).ToList();

            }
            else
            {
                list = dbHelpers.Inv_Part.OrderByDescending(db => db.fid).Where(db => db.fid != 0).ToList();
            }
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("产品编号");
            row1.CreateCell(1).SetCellValue("产品名称");
            row1.CreateCell(2).SetCellValue("产品规格");
            row1.CreateCell(3).SetCellValue("单位");
            row1.CreateCell(4).SetCellValue("包装规格");
            row1.CreateCell(5).SetCellValue("建立日期");
            row1.CreateCell(6).SetCellValue("分类");
            row1.CreateCell(7).SetCellValue("产品图片");
            row1.CreateCell(8).SetCellValue("产品条码");
           

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].p_no);
                rowtemp.CreateCell(1).SetCellValue(list[i].pname);
                rowtemp.CreateCell(2).SetCellValue(list[i].type);
                rowtemp.CreateCell(3).SetCellValue(list[i].unit);
                rowtemp.CreateCell(4).SetCellValue(list[i].pqty.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].in_date.ToString());
                rowtemp.CreateCell(6).SetCellValue(list[i].parttype);
                rowtemp.CreateCell(7).SetCellValue(list[i].imgurl == "" ? "" : url + list[i].imgurl);
                rowtemp.CreateCell(8).SetCellValue(list[i].bm_no);
               
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "产品信息" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        }

        //下载产品模板
        [Manage]
        public FileResult ProductToExcelTemp()
        {

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("产品编号");
            row1.CreateCell(1).SetCellValue("产品名称");
            row1.CreateCell(2).SetCellValue("产品规格");
            row1.CreateCell(3).SetCellValue("单位");
            row1.CreateCell(4).SetCellValue("包装规格");
            row1.CreateCell(5).SetCellValue("建立日期");
            row1.CreateCell(6).SetCellValue("分类");
            row1.CreateCell(7).SetCellValue("产品图片");
            row1.CreateCell(8).SetCellValue("产品条码");
            
            //将数据逐步写入sheet1各个行

            NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(1);
            rowtemp.CreateCell(0).SetCellValue("1001");
            rowtemp.CreateCell(1).SetCellValue("产品名1");
            rowtemp.CreateCell(2).SetCellValue("100ml");
            rowtemp.CreateCell(3).SetCellValue("瓶");
            rowtemp.CreateCell(4).SetCellValue("1");
            rowtemp.CreateCell(5).SetCellValue("");
            rowtemp.CreateCell(6).SetCellValue("");
            rowtemp.CreateCell(7).SetCellValue("");
            rowtemp.CreateCell(8).SetCellValue("");
           

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "产品导入模板" + DateTime.Now.ToString("hhmmss") + ".xls");
        }
    }
}