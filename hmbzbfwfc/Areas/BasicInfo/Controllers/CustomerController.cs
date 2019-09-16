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
    public class CustomerController : ManageBaseController
    {
        // GET: BasicInfo/Customer
        public ActionResult Index()
        {
            return View();
        }

        [Manage]
        public ActionResult CustomerList()
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

            var customername = Request["customername"];

            List<customer> gclist = new List<customer>();
         
            var parme = "";
            if (!string.IsNullOrEmpty(customername))
            {
                gclist = dbHelpers.customer.Where(db => db.cu_name.Contains(customername) || db.cu_no.Contains(customername) || db.province==customername ||db.city==customername ).ToList();
                parme += "customername=" + customername;
            }
            else
            {
                gclist = dbHelpers.customer.OrderByDescending(db => db.fid).ToList();
            }
            if (this.xtUser.UserType=="c")
            {
                gclist = dbHelpers.customer.Where(db => db.xtcu_no == this.xtUser.UserNo ).ToList();
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
        public ActionResult CustomerEdit()
        {
  
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var cuslist = dbHelpers.customer.OrderBy(db => db.fid);
            ViewBag.logintype = this.xtUser.UserType;
            ViewBag.cuslist = cuslist;
            ViewBag.cuscount = cuslist.Count();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }

            var model = dbHelpers.customer.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.cuslist = cuslist.Where(db => db.fid != model.fid && db.xtcu_no != model.cu_no);
            var sjmodel = dbHelpers.customer.FirstOrDefault(db => db.cu_no.ToString() == model.xtcu_no);
            if (sjmodel != null)
                ViewBag.sjmodelname = sjmodel.cu_name;
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

       

        public ActionResult ACustomerAddOrUpd(customer ct, string logintype)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            if (string.IsNullOrEmpty(ct.cu_name) )
                return Json(new { status = "error", data = "必填项不能为空！" });
          
            customer ip = dbHelpers.customer.FirstOrDefault(db => db.fid == ct.fid);
            ct.in_date = DateTime.Now;

            ct.unitcode = this.xtUser.xtunitcode;   //企业或品牌代码


            if (this.xtUser.UserType == "c" )
            {
                ct.xtcu_no = this.xtUser.UserNo;
            }
            else
            {
                ct.xtcu_no = "";
            }
            if (ip == null)
            {
                if (string.IsNullOrEmpty(ct.cu_no))
                    return Json(new { status = "error", data = "经销商编号不能为空！" });
                var sm = dbHelpers.customer.FirstOrDefault(db => db.cu_no == ct.cu_no && db.unitcode == this.xtUser.xtunitcode);
                if (sm != null)
                    return Json(new { status = "error", data = "该经销商编号已被使用！" });
                ct.passwd = Uties.CMD5(ct.passwd);

                dbHelpers.customer.Add(ct);
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
            //    ip.email = ct.email;
                ip.link_man = ct.link_man;
                ip.remark = ct.remark;
                ip.xtcu_no = ct.xtcu_no;
                ip.unitcode = this.xtUser.xtunitcode;   //企业或品牌代码

                if (ip.passwd != ct.passwd && ct.passwd !=null)
                {
                    ip.passwd = Uties.CMD5(ct.passwd);
                }
            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public JsonResult CustomerDel()
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
                        var umodel = dbHelpers.customer.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.customer.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.customer.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.customer.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
        /// <summary>
        /// 客户资料导出
        /// </summary>        
        public FileResult CustomertoExcel()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            //获取list数据           
            var customername = Request["customername"];

            List<customer> list = new List<customer>();
            if (!string.IsNullOrEmpty(customername))
            {
                list = dbHelpers.customer.Where(db => db.cu_no.ToString().Contains(customername) || db.cu_name.Contains(customername)).ToList();

            }
            else
            {
                list = dbHelpers.customer.OrderByDescending(db => db.fid).Where(db => db.fid != 0).ToList();
            }
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("客户编号");
            row1.CreateCell(1).SetCellValue("客户名称");
            row1.CreateCell(2).SetCellValue("客户分类");
            row1.CreateCell(3).SetCellValue("上级客户编号");
            row1.CreateCell(4).SetCellValue("省份");
            row1.CreateCell(5).SetCellValue("城市");
            row1.CreateCell(6).SetCellValue("详细地址");
            row1.CreateCell(7).SetCellValue("电话");
            row1.CreateCell(8).SetCellValue("传真");
            row1.CreateCell(9).SetCellValue("Email");
            row1.CreateCell(10).SetCellValue("联系人");
            row1.CreateCell(11).SetCellValue("密码");
            //row1.CreateCell(5).SetCellValue("单价");
            //row1.CreateCell(7).SetCellValue("积分");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].cu_no);
                rowtemp.CreateCell(1).SetCellValue(list[i].cu_name);
                rowtemp.CreateCell(2).SetCellValue(list[i].cutype);
                rowtemp.CreateCell(3).SetCellValue(list[i].xtcu_no);
                rowtemp.CreateCell(4).SetCellValue(list[i].province);
                rowtemp.CreateCell(5).SetCellValue(list[i].city);
                rowtemp.CreateCell(6).SetCellValue(list[i].addr);
                rowtemp.CreateCell(7).SetCellValue(list[i].phone);
                rowtemp.CreateCell(8).SetCellValue(list[i].fax);
                rowtemp.CreateCell(9).SetCellValue(list[i].email);
                rowtemp.CreateCell(10).SetCellValue(list[i].link_man);
                rowtemp.CreateCell(11).SetCellValue("");
                //rowtemp.CreateCell(5).SetCellValue(list[i].price.ToString());
                //rowtemp.CreateCell(7).SetCellValue(list[i].jf.ToString());

            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "客户信息" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        }

        /// <summary>
        /// 客户资料导出
        /// </summary>        
        public FileResult CustomerToExcelTemp()
        {
            
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("客户编号");
            row1.CreateCell(1).SetCellValue("客户名称");
            row1.CreateCell(2).SetCellValue("客户分类");
            row1.CreateCell(3).SetCellValue("上级客户编号");
            row1.CreateCell(4).SetCellValue("省份");
            row1.CreateCell(5).SetCellValue("城市");
            row1.CreateCell(6).SetCellValue("详细地址");
            row1.CreateCell(7).SetCellValue("电话");
            row1.CreateCell(8).SetCellValue("传真");
            row1.CreateCell(9).SetCellValue("Email");
            row1.CreateCell(10).SetCellValue("联系人");
            row1.CreateCell(11).SetCellValue("密码");
            //row1.CreateCell(5).SetCellValue("单价");
            //row1.CreateCell(7).SetCellValue("积分");

            //将数据逐步写入sheet1各个行
           
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow( 1);
                rowtemp.CreateCell(0).SetCellValue("1001");
                rowtemp.CreateCell(1).SetCellValue("经销商1");
                rowtemp.CreateCell(2).SetCellValue("");
                rowtemp.CreateCell(3).SetCellValue("");
                rowtemp.CreateCell(4).SetCellValue("广东");
                rowtemp.CreateCell(5).SetCellValue("广州");
                rowtemp.CreateCell(6).SetCellValue("");
                rowtemp.CreateCell(7).SetCellValue("");
                rowtemp.CreateCell(8).SetCellValue("");
                rowtemp.CreateCell(9).SetCellValue("");
                rowtemp.CreateCell(10).SetCellValue("");
                rowtemp.CreateCell(11).SetCellValue("1234");
                //rowtemp.CreateCell(5).SetCellValue(list[i].price.ToString());
                //rowtemp.CreateCell(7).SetCellValue(list[i].jf.ToString());

           
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "客户信息" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        }
    }
}