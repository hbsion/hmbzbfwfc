using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using hmbzbfwfc.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hmbzbfwfc.Controllers;



namespace hmbzbfwfc.Areas.BasicInfo.Controllers
{
    public class VenderController : ManageBaseController
    {
        // GET: BasicInfo/Customer
        public ActionResult Index()
        {
            return View();
        }

        [Manage]
        public ActionResult VenderList()
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

            var customername = Request["selparme"];

            IQueryable<vender> gclist = dbHelpers.vender;
         
            var parme = "";
            if (!string.IsNullOrEmpty(customername))
            {
                gclist = dbHelpers.vender.Where(db => db.cu_name.Contains(customername) );
                parme += "&selparme=" + customername;
            }
           
            

            var list = gclist.OrderByDescending(x=>x.fid).Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            var count = gclist.Count();
            var pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.logintype = this.xtUser.UserType;
            ViewBag.list = list;
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = pagecount;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            ViewBag.url = "VenderList";
            return View();
        }

        [Manage]
        public ActionResult VenderEdit()
        {
  
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
           
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }

            var model = dbHelpers.vender.FirstOrDefault(db => db.fid.ToString() == fid);            
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

       

        public ActionResult VenderAddOrUpd(vender  ct)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            if (string.IsNullOrEmpty(ct.cu_no) || string.IsNullOrEmpty(ct.cu_name) )
                return Json(new { status = "error", data = "必填项不能为空！" });

            vender ip = dbHelpers.vender.FirstOrDefault(db => db.fid == ct.fid);
           
            
            if (ip == null)
            {                
                var sm = dbHelpers.customer.FirstOrDefault(db => db.cu_no == ct.cu_no && db.unitcode == this.xtUser.xtunitcode);
                if (sm != null)
                    return Json(new { status = "error", data = "该供应商编号已被使用！" });
                if(!string.IsNullOrEmpty(ct.passwd))
                  ct.passwd = Uties.CMD5(ct.passwd);
                
                dbHelpers.vender.Add(ct);
            }
            else
            {
                ip.cu_no = ct.cu_no;
                ip.cu_name = ct.cu_name;
                ip.cutype = ct.cutype;
                ip.province = ct.province;
                ip.city = ct.city;
                ip.addr = ct.addr;
                ip.phone = ct.phone;
                ip.fax = ct.fax;           
                ip.link_man = ct.link_man;
                ip.remark = ct.remark;
                ip.xtcu_no = ct.xtcu_no;
                ip.unitcode = this.xtUser.xtunitcode;   //企业或品牌代码

                if (ip.passwd != ct.passwd &&  !string.IsNullOrEmpty(ct.passwd))
                {
                    ip.passwd = Uties.CMD5(ct.passwd);
                }
            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public JsonResult VenderDel()
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
                        var umodel = dbHelpers.vender.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.vender.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.vender.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.vender.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
        /// <summary>
        /// 客户资料导出
        /// </summary>        
        public FileResult VenderToExcel()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            //获取list数据           
            var customername = Request["customername"];

            List<vender> list = new List<vender>();
            if (!string.IsNullOrEmpty(customername))
            {
                list = dbHelpers.vender.Where(db => db.cu_no.ToString().Contains(customername) || db.cu_name.Contains(customername)).ToList();

            }
            else
            {
                list = dbHelpers.vender.OrderByDescending(db => db.fid).Where(db => db.fid != 0).ToList();
            }
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("供应商编号");
            row1.CreateCell(1).SetCellValue("供应商名称");            
            row1.CreateCell(2).SetCellValue("省份");
            row1.CreateCell(3).SetCellValue("城市");
            row1.CreateCell(4).SetCellValue("详细地址");
            row1.CreateCell(5).SetCellValue("电话");
            row1.CreateCell(6).SetCellValue("传真");            
            row1.CreateCell(7).SetCellValue("联系人");
            

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].cu_no);
                rowtemp.CreateCell(1).SetCellValue(list[i].cu_name);                
                rowtemp.CreateCell(2).SetCellValue(list[i].province);
                rowtemp.CreateCell(3).SetCellValue(list[i].city);
                rowtemp.CreateCell(4).SetCellValue(list[i].addr);
                rowtemp.CreateCell(5).SetCellValue(list[i].phone);
                rowtemp.CreateCell(6).SetCellValue(list[i].fax);                
                rowtemp.CreateCell(7).SetCellValue(list[i].link_man);                
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "供应商信息" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        }

        /// <summary>
        /// 客户资料导出
        /// </summary>        
        public FileResult VenderToExcelTemp()
        {
            
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("客户编号");
            row1.CreateCell(1).SetCellValue("客户名称");            
            row1.CreateCell(2).SetCellValue("省份");
            row1.CreateCell(3).SetCellValue("城市");
            row1.CreateCell(4).SetCellValue("详细地址");
            row1.CreateCell(5).SetCellValue("电话");
            row1.CreateCell(6).SetCellValue("传真");            
            row1.CreateCell(7).SetCellValue("联系人");
            

            //将数据逐步写入sheet1各个行
           
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow( 1);
                rowtemp.CreateCell(0).SetCellValue("1001");
                rowtemp.CreateCell(1).SetCellValue("供应商1");               
                rowtemp.CreateCell(2).SetCellValue("广东");
                rowtemp.CreateCell(3).SetCellValue("广州");
                rowtemp.CreateCell(4).SetCellValue("");
                rowtemp.CreateCell(5).SetCellValue("");
                rowtemp.CreateCell(6).SetCellValue("");
                rowtemp.CreateCell(7).SetCellValue("");                              
               
           
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "客户信息" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        }
    }
}