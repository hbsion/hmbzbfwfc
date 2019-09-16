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
    public class InvLotController : ManageBaseController
    {
        // GET: BasicInfo/InvLot
        public ActionResult Index()
        {
            return View();
        }

        [Manage]


        public ActionResult InvLotList()
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

            var keysname = Request["keysname"];

            List<Inv_Lot> gclist = new List<Inv_Lot>();

            var parme = "";
            if (!string.IsNullOrEmpty(keysname))
            {
                gclist = dbHelpers.Inv_Lot.Where(db => (db.pname.Contains(keysname) || db.lot_no.Contains(keysname) || db.p_no.Contains(keysname)) && db.unitcode == this.xtUser.xtunitcode).ToList();
                parme += "keysname=" + keysname;
            }
            else
            {
                gclist = dbHelpers.Inv_Lot.Where(db => db.unitcode == this.xtUser.xtunitcode).OrderByDescending(db => db.fid).ToList();
            }


            parme += "unitcode=" + this.xtUser.xtunitcode;
  

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
        public ActionResult InvLotEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.Inv_Lot.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }




        [HttpPost]
  
        [ValidateInput(false)]
        public ActionResult InvLotAddOrUpd(Inv_Lot ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            Inv_Lot ip = dbHelpers.Inv_Lot.FirstOrDefault(db => db.fid == ins.fid);
            ins.in_date = DateTime.Now;
            ins.unitcode = this.xtUser.xtunitcode;   //企业或品牌代码

            if (ip == null)
            {
                if (string.IsNullOrEmpty(ins.lot_no) )
                    return Json(new { status = "error", data = "产品批号不能为空！" });

                var sm = dbHelpers.Inv_Lot.FirstOrDefault(db => db.lot_no == ins.lot_no && db.unitcode == this.xtUser.xtunitcode);
                if (sm != null)
                    return Json(new { status = "error", data = "该批号编号已被使用！" });

                dbHelpers.Inv_Lot.Add(ins);
            }
            else
            {

            
                ip.p_no = ins.p_no;
                ip.pname = ins.pname;
                ip.type = ins.type;

                ip.remark = ins.remark;
                ip.imgurl = ins.imgurl;
                ip.retxt = ins.retxt;

                ip.pr_date = ins.pr_date;
    
   
                ip.makeare = ins.makeare;

                ip.unitcode = this.xtUser.xtunitcode;   //企业或品牌代码

            }


            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public JsonResult InvLotDel()
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
                        var umodel = dbHelpers.Inv_Lot.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.Inv_Lot.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.Inv_Lot.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.Inv_Lot.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }

 

 

        //导出产品
        [Manage]
        public FileResult InvLottoExcel()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            //获取list数据
            string url = HttpContext.Request.Url.ToString();
            int ii = url.LastIndexOf("/BasicInfo/InvLot/InvLottoExcel");
            url = url.Remove(ii);

            var keysname = Request["keysname"];

            List<Inv_Lot> list = new List<Inv_Lot>();
            if (!string.IsNullOrEmpty(keysname))
            {
                list = dbHelpers.Inv_Lot.Where(db => db.p_no.ToString().Contains(keysname) || db.pname.Contains(keysname) || db.lot_no.Contains(keysname)).ToList();

            }
            else
            {
                list = dbHelpers.Inv_Lot.OrderByDescending(db => db.fid).Where(db => db.fid != 0).ToList();
            }
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("批号");
   
            row1.CreateCell(1).SetCellValue("产品编号");
            row1.CreateCell(2).SetCellValue("产品名称");

            row1.CreateCell(3).SetCellValue("说明");


            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].lot_no);
                rowtemp.CreateCell(1).SetCellValue(list[i].p_no);
                rowtemp.CreateCell(2).SetCellValue(list[i].pname);
                rowtemp.CreateCell(2).SetCellValue(list[i].remark);


            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "批号信息" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
        }


    }
}