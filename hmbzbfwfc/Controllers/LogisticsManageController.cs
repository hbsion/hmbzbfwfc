using hmbzbfwfc.Commons;
using hmbzbfwfc.Attributes;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using marr.BusinessRule;
using marr.BusinessRule.Entity;
using System.Configuration;

namespace hmbzbfwfc.Controllers
{
    //物流管理控制器
    
    public class LogisticsManageController : ManageBaseController
    {
        // GET: LogisticsManage

        //数据文件列表
        [Manage]
        public ActionResult FileDataList()
        {
            string filetype = Request["filetype"];

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

            string parme = "filetype=" + filetype;
            var packdataname = Request["packdataname"];
            if (!string.IsNullOrEmpty(packdataname))
                parme += "&packdataname=" + packdataname;

            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            string dateparme = "and 1=1";
            if (!string.IsNullOrEmpty(startdate))
            {
                dateparme = "and add_date between '" + startdate + "' and '" + enddate + "'";
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            ConnectionStringEntities db = new ConnectionStringEntities();

            var cparme = "";

            try
            {
                if (Request.Cookies["loginuid"] != null)
                {
                    HttpCookie cookieuid = Request.Cookies["loginuid"];
                    var uid = DESEncrypt.Decrypt(cookieuid.Value);
                    if (Request.Cookies["logintype"].Value == "customer")
                    {
                        var cu_no = db.customer.FirstOrDefault(d => d.fid.ToString() == uid).cu_no;
                        cparme += "and  cu_no='" + cu_no + "'";
                    }
                }
            }
            catch
            {

            }

            PagesImpl pageImpl = new PagesImpl(db);
            PageinationInfo pageination = new PageinationInfo();
            pageination.TableName = "sal_packa";
            pageination.PageIndex = pageno;
            pageination.PageSize = pagesize;
            pageination.FieldName = "*";
            pageination.OrderCondition = "fid desc";
            pageination.WhereCondition = "pfilename like '%" + packdataname + "%' " + dateparme + " and type='" + filetype + "' " + cparme + "";
            var data = pageImpl.GetPageinationData<sal_packa>(pageination);

            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            ViewBag.filetype = filetype;
            return View();
        }

        public ActionResult FileUploadPage()
        {
            var filetype = Request["filetype"];

            ViewBag.filetype = filetype;
            return View();
        }

        #region 文件上传
        [HttpPost]

        public ActionResult FileIsUpload()
        {
            var filename = Request["filename"];
            var filetype = Request["filetype"];
            string update = DateTime.Now.ToString("yyyyMMdd");
            string relativepath = string.Format("/Upload/FileUpload/{0}/{1}", update, filename);
            var gs = filename.Substring(filename.LastIndexOf("."));
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var isupload = dbHelpers.sal_packa.FirstOrDefault(db => db.relativepath == relativepath);
            if (isupload != null)
                return Json(new { status = "no", data = "不能重复上传,请重新选择！" });
            if (filetype != "Pro" && filetype != "Cus" && filetype!="Ven")
            {
                if (gs.ToLower().Trim() != ".txt")
                    return Json(new { status = "no", data = "文件格式不正确,请重新选择！" });
            }
            return Json(new { status = "ok", data = "等待上传..." });
        }


        public ActionResult Upload()
        {
            ConnectionStringEntities dbHelperes = new ConnectionStringEntities();
            string update = DateTime.Now.ToString("yyyyMMdd");
            string fileName = Request["name"];
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号           

            var dir = Server.MapPath("/Upload/FileUpload/" + update + "/");//文件上传目录           

            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            //if (data != null)//为null可能是暂停的那一瞬间
            //{
            data.SaveAs(filePath);//报错
            //}
            return Json(new { status = "success", data = "上传成功" });//Demo，随便返回了个值，请勿参考
        }

        public string getrandom(int length)
        {
            string s = "100000";
            string e = "999999";
            Random ran2 = new Random();
            int myint2 = ran2.Next(int.Parse(s.Substring(0, length)), int.Parse(e.Substring(0, length)));
            string ranstr = Convert.ToString(myint2);
            return ranstr;
        }
        public ActionResult Merge()
        {
            var filetype = Request["filetype"];

            string isimport = Request["isimport"];
            string update = DateTime.Now.ToString("yyyyMMdd");
            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("/Upload/FileUpload/" + update + "/");//Upload 文件夹
            var fileName = Request["fileName"];//文件名
            var relativepath = string.Format("/Upload/FileUpload/{0}/{1}", update, fileName);
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));
            var dir = Path.Combine(uploadDir, fileRelName);//临时文件夹          
            var files = System.IO.Directory.GetFiles(dir);//获得下面的所有文件
            var finalPath = Path.Combine(uploadDir, fileName);//最终的文件名
            var fs = new FileStream(finalPath, FileMode.Create);
            foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            fs.Flush();
            fs.Close();
            System.IO.Directory.Delete(dir);//删除文件夹

            //文件重新转码
            var zmpath = Server.MapPath("/Upload/FileUpload/zm" + update + "/");
            var othersave = Path.Combine(zmpath, fileName);
            if (filetype == "S" || filetype == "R" || filetype == "X" || filetype == "Y")
            {
                using (StreamReader sr = new StreamReader(finalPath, Encoding.UTF8))
                {
                    if (!Directory.Exists(zmpath))
                    {
                        Directory.CreateDirectory(zmpath);
                    }
                    using (StreamWriter sw = new StreamWriter(othersave, false, Encoding.Default))
                    {
                        sw.Write(sr.ReadToEnd());
                    }
                    sr.Close();
                }
            }

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            dbHelpers.Database.CommandTimeout = 3600;
            var cu_no = "sysadmin";
            try
            {
                if (Request.Cookies["loginuid"] != null)
                {
                    HttpCookie cookieuid = Request.Cookies["loginuid"];
                    var uid = DESEncrypt.Decrypt(cookieuid.Value);
                    if (Request.Cookies["logintype"].Value == "customer")
                    {
                        cu_no = dbHelpers.customer.FirstOrDefault(db => db.fid.ToString() == uid).cu_no;
                    }
                }
            }
            catch
            {

            }
          
            sal_packa sp = new sal_packa();
            sp.pfilename = fileName;
            sp.uplog = "文件已上传，数据未导入";
            //保存文件名称信息            
            if (filetype == "P")
            {
                string[] fileheadstr = fileRelName.Split('｜');
                if (fileheadstr.Length < 5)
                {
                    sp.line_no = "";
                    sp.pack_date = DateTime.Now;
                    sp.wo_no = "";
                    sp.p_no = "";
                    sp.pack_no = DateTime.Now.ToString("yyyyMMddHHmmssfff") + getrandom(4);
                }
                else
                {
                    sp.line_no = fileheadstr[0].Replace("#", "");
                    sp.pack_date = DateTime.Parse(fileheadstr[1]);
                    sp.wo_no = fileheadstr[2];
                    sp.p_no = fileheadstr[3];
                    sp.pack_no = DateTime.Now.ToString("yyyyMMddHHmmssfff") + getrandom(4);
                }
            }
            else
            {
                sp.pack_no = DateTime.Now.ToString("yyyyMMddHHmmssfff") + getrandom(4);
            }
            sp.type = filetype;
            if(filetype=="S"||filetype=="R"||filetype=="X" ||filetype=="Y")
            {
                sp.absolutepath = othersave;
            }
            else { 
            sp.absolutepath = finalPath;
            }
            sp.relativepath = relativepath;
            sp.add_date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
            sp.status = 0;
            sp.cu_no = cu_no;
            dbHelpers.sal_packa.Add(sp);
            dbHelpers.SaveChanges();

            if (filetype == "Sys")
            {
                return RedirectToAction("SystemDataImport", "BasicData", sp);
            }
            else if (filetype == "Fx")
            {
                return RedirectToAction("FxDataImport", "BasicData", sp);
            }
            else if (filetype == "Pro" || filetype == "Cus" || filetype=="Ven")
            {
                ExcelToDB todb = new ExcelToDB();
                var count = 0;
                if (filetype == "Pro")
                    count = todb.ProToDataBase(sp.absolutepath);
                if (filetype == "Cus")
                    count = todb.CusToDataBase(sp.absolutepath);
                if (filetype == "Ven")
                    count = todb.VenToDataBase(sp.absolutepath);
                if (count > 0)
                {
                    var amodel = dbHelpers.sal_packa.FirstOrDefault(db => db.absolutepath == sp.absolutepath);
                    amodel.uplog = "成功导入" + count + "条数据";
                    amodel.status = 9;
                    amodel.okqty = count;
                    dbHelpers.SaveChanges();
                }

                return Json(new { status = "success", data = "导入" + count + "条数据" });
            }


            if (isimport == "ok")
            {
                PagesImpl pageImpl = new PagesImpl(dbHelpers);
                var result = "";
               /* if (filetype == "X" || filetype == "Y")
                {
                    result = pageImpl.CuPackDataImport(sp.absolutepath, sp.pack_no, filetype, cu_no);
                }
                else*/
                
                result = pageImpl.PackDataImport(sp.absolutepath, sp.pack_no, filetype,(DateTime)sp.add_date,sp.cu_no);
                
                if (result != "ok")
                    return Json(new { status = "error", data = result });
            }
            return Json(new { status = "success", data = "上传成功" });//随便返回个值，实际中根据需要返回
        }
        #endregion        

        [Manage]
        public JsonResult PackFileDel()
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
                        var umodel = dbHelpers.sal_packa.FirstOrDefault(db => db.fid.ToString() == i);
                        string fullFileName1 = Server.MapPath(umodel.pfilename);
                        try
                        {
                            System.IO.File.Delete(fullFileName1);
                        }
                        catch (Exception) { }
                        dbHelpers.sal_packa.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.sal_packa.FirstOrDefault(db => db.fid.ToString() == fid);
            string fullFileName2 = Server.MapPath(model.relativepath);
            try
            {
                System.IO.File.Delete(fullFileName2);
            }
            catch (Exception) { }
            dbHelpers.sal_packa.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }

        public ActionResult DataRevoke()
        {
            var fid = Request["fid"];
            var type = Request["type"];
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var model = dbHelpers.sal_packa.FirstOrDefault(db => db.fid.ToString() == fid);
            PagesImpl pageImpl = new PagesImpl(dbHelpers);
            int num = pageImpl.FileDataDel(model.absolutepath, model.pack_no, model.type,(DateTime)model.add_date);
            return Json(new { count = num });
        }


        [Manage]
        public ActionResult PackDataDetailList()
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
            var Detailname = Request["Detailname"];
            if (!string.IsNullOrEmpty(Detailname))
                parme += "&Detailname=" + Detailname;

            var startdate = Request["startdate"];
            var enddate = Request["enddate"];

            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_packb> lspb ;
            db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(startdate);
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = db.sal_packb.Where(d => d.in_date >= sdate && d.in_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }else
            {
                DateTime sdate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00"));
                lspb = db.sal_packb.Where(d => d.in_date >= sdate && d.in_date <= DateTime.Now);
            }

            if (!string.IsNullOrEmpty(Detailname))
            {
                lspb = lspb.Where(d => d.usnno == Detailname || d.snno == Detailname || d.pack_no == Detailname || d.p_no==Detailname);
            }
            
            if(Request["dc"]=="ok")
            {
                var list = lspb.ToList();
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("包装单号");
                row1.CreateCell(1).SetCellValue("产品码");
                row1.CreateCell(2).SetCellValue("包装箱码");
                row1.CreateCell(3).SetCellValue("日期");
                row1.CreateCell(4).SetCellValue("产品");
                row1.CreateCell(5).SetCellValue("产品规格");
                row1.CreateCell(6).SetCellValue("线号");
                row1.CreateCell(7).SetCellValue("批号");


                //将数据逐步写入sheet1各个行
                for (int i = 0; i < list.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(list[i].pack_no);
                    rowtemp.CreateCell(1).SetCellValue(list[i].snno);
                    rowtemp.CreateCell(2).SetCellValue(list[i].usnno);
                    rowtemp.CreateCell(3).SetCellValue(list[i].in_date.ToString());                    
                    rowtemp.CreateCell(4).SetCellValue(DataEnum.GetPName(list[i].p_no));
                    rowtemp.CreateCell(5).SetCellValue(DataEnum.GetPType(list[i].p_no));
                    rowtemp.CreateCell(6).SetCellValue(list[i].line_no);
                    rowtemp.CreateCell(7).SetCellValue(list[i].lot_no);


                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "包装入库" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            
            var count = lspb.Count(); 
            ViewBag.startdate = startdate;
            ViewBag.enddate = enddate;
            ViewBag.list = lspb.OrderByDescending(d => d.fid).Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }
        
        [Manage]
        public JsonResult SalPackbDel()
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
                        var umodel = dbHelpers.sal_packb.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.sal_packb.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.sal_packb.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.sal_packb.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }

        public class InvStqty{
        public string p_no {get;set;}
        public string p_name { get; set; }
        public decimal? inqty {get;set;}
        public decimal? stqty  {get;set;}
        public decimal? bakqty{get;set;}
        public decimal? outqty{get;set;}
        public DateTime? m_date { get; set; }
        }
        [Manage]
        public ActionResult StQty()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            try
            {
                string sql = "exec ptjinv_stqty";
                dbHelpers.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e) { }

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

            var pname = Request["pname"];

            List<Inv_Stqty> gclist = new List<Inv_Stqty>();
            var parme = "";
            if (!string.IsNullOrEmpty(pname))
            {
                gclist = dbHelpers.Inv_Stqty.Where(db => db.p_no==pname).ToList();
                parme += "pname=" + pname;
            }
            else
            {
                gclist = dbHelpers.Inv_Stqty.OrderByDescending(db => db.fid).ToList();
            }
            BasicDataController bc = new BasicDataController();
            var list = gclist.Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            var slist = list.Select(item => new InvStqty
            {
                p_no=item.p_no,
                p_name = bc.getname("p", item.p_no),
                inqty = item.inqty,
                stqty = item.stqty,
                bakqty = item.bakqty,
                outqty = item.outqty,
                m_date = item.m_date
            }).ToList();
            var count = gclist.Count();
            var pagecount = (int)Math.Ceiling((decimal)count / pagesize);

            ViewBag.list = slist;
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = pagecount;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }     
       

        public ActionResult FileDataNowImport() {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var filetype = Request["filetype"];
            var sdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            
            PagesImpl pageImpl = new PagesImpl(dbHelpers);
            var result = pageImpl.PackDataBatchImport(filetype, sdate);

            return Redirect("FileDataList?filetype="+filetype);
        }

        public ActionResult SalPackScanPage()
        {
            ViewBag.packno = "P" + Uties.GetOrderNumber();
            return View();
        }

        /// <summary>
        /// 入库扫描页
        /// </summary>
        /// <returns></returns>
        public ActionResult SalFgiScanPage()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            ViewBag.kb = dbHelpers.Inv_Store.OrderByDescending(db => db.fid);
            ViewBag.prod = dbHelpers.Inv_Part.OrderByDescending(db => db.fid);
            ViewBag.fgino = "F" + Uties.GetOrderNumber();

            return View();
        }

        /// <summary>
        /// 入库扫描操作
        /// </summary>
        /// <returns></returns>
        public ActionResult SalFgiScan(sal_fgi sf ,string cpsl,string js)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();


            
            int pjs = 0;
            if (!string.IsNullOrEmpty(js))
                pjs = int.Parse(js);

            int cps = 0;
            if (!string.IsNullOrEmpty(cpsl))
                cps = int.Parse(cpsl);

            var isfgi = dbHelpers.sal_fgi.FirstOrDefault(db => db.bsnno == sf.bsnno);
            if (isfgi != null)
                return Json(new { status = false, data = sf.bsnno + "重复入库！<br/>" });

            sf.mqty = 1;

            //计算条码量


            brBarcode brcd = new brBarcode();
            Barcode barcode = brcd.GetBarcodeEntity(sf.bsnno, "");

            int pqty = 0;

            if (barcode != null)
            {
                pqty = barcode.Qty;
            }
            
            if (pqty == 0 )   //不让扫描小瓶
            {
                return Json(new { status = false, data = sf.bsnno + "物流码不正确<br/>" });               
               
            }


            sf.mqty = pqty; 

            var pro = dbHelpers.Inv_Part.FirstOrDefault(db => db.p_no == sf.p_no);
            if (pro != null)
                sf.type = pro.type;

            try
            {
                sf.ship_date = sf.ship_date;
            }
            catch {
                return Json(new { status = false, data = sf.bsnno + "入库日期格式错误<br/>" });
            }

            sf.pda_no = this.xtUser.UserNo;
            sf.unitcode = "";
            sf.upyn = "N";
            dbHelpers.sal_fgi.Add(sf);


            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = true, data = sf.bsnno + "入库成功！<br/>", js = pjs + 1, cpsl = cps + pqty });
            return Json(new { status = false, data = sf.bsnno + "入库失败！<br/>" });
        }


        /// <summary>
        /// 入库明细
        /// </summary>
        [Manage]      
        public ActionResult FgiList() {
            int pageno = 1;
            if (!string.IsNullOrEmpty(Request["pageno"]))
                pageno = int.Parse(Request["pageno"]);
           
            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request["pagesize"]))
                pagesize = int.Parse(Request["pagesize"]);

            string parme = "";
            var fginame = Request["fginame"];
            if (!string.IsNullOrEmpty(fginame))
                parme += "fginame=" + fginame;
           
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            var kb = Request["kb"];
            var pno = Request["pno"];

            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_fgi> lspb = db.sal_fgi;
            //db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = lspb.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }

            if (!string.IsNullOrEmpty(fginame))
                lspb = lspb.Where(d => d.ship_no == fginame || d.bsnno==fginame);

            if(!string.IsNullOrEmpty(kb))
            {
                lspb = lspb.Where(x => x.st_no == kb);
                parme += "&kb=" + kb;
            }

            if (!string.IsNullOrEmpty(pno))
            {
                lspb = lspb.Where(x => x.p_no == pno);
                parme += "&pno=" + pno;
            }

            //var  lspb2 = lspb.ToList().Select(l=>new sal_fgi{
            //   ship_no=l.ship_no,
            //   st_no = string.IsNullOrEmpty(l.st_no) ? "" : l.st_no,
            //   ship_date = l.ship_date != null ? DateTime.Parse(DateTime.Parse(l.ship_date.ToString()).ToString("yyyy-MM-dd")) : DateTime.Now,
            //   pda_no = string.IsNullOrEmpty(l.pda_no) ? "" : l.pda_no,
            //   mqty=l.mqty,             
            //   p_no=l.p_no,
            //   cu_no=l.cu_no
            //});
            //
            //var sf = lspb2.GroupBy(x => new { x.ship_no,x.st_no ,x.ship_date,x.pda_no,x.p_no,x.cu_no}).ToList().Select(s => new sal_fgi
            //{
            //    ship_no = s.Key.ship_no,
            //    st_no=s.Key.st_no,
            //    ship_date=s.Key.ship_date,
            //    pda_no=s.Key.pda_no,
            //    p_no=s.Key.p_no,
            //    cu_no=s.Key.cu_no,
            //    mqty = s.Count()
            //});

            var count = lspb.Count();
            var slist = lspb.OrderByDescending(d => d.ship_date).Skip((pageno - 1) * pagesize).Take(pagesize);
            ViewBag.zj = slist.Sum(d => d.mqty);
            ViewBag.list = slist.ToList();
            ViewBag.count =count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            ViewBag.kb = db.Inv_Store;
            return View();
        }

        [Manage]
        public JsonResult FgiDel()
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
                        var umodel = dbHelpers.sal_fgi.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.sal_fgi.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.sal_fgi.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.sal_fgi.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
        public ActionResult SeeFgiInfo() {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var shipno = Request["shipno"];
            var stno = Request["stno"];
            var pno = Request["pno"];
            var pdano = Request["pdano"];
            var dc = Request["dc"];

            var fgi = dbHelpers.sal_fgi.Where(db =>  db.ship_no == shipno );

            if (!string.IsNullOrEmpty(pno))
                fgi = fgi.Where(db => db.p_no == pno);
            else
                fgi = fgi.Where(db => db.p_no == "" || db.p_no == null);

            if(!string.IsNullOrEmpty(stno))
                fgi = fgi.Where(db =>  db.st_no == stno );
            else
                fgi = fgi.Where(db => db.st_no == null || db.st_no == "");
            if(!string.IsNullOrEmpty(pdano))
                fgi = fgi.Where(db =>  db.pda_no == pdano);
            else
                fgi = fgi.Where(db => db.pda_no == null || db.pda_no == "");

            if(dc=="ok")
            {
                var list = fgi.ToList();
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

                row1.CreateCell(0).SetCellValue("单号");
                row1.CreateCell(1).SetCellValue("条码");
                row1.CreateCell(2).SetCellValue("入库日期");
                row1.CreateCell(3).SetCellValue("产品编号");
                row1.CreateCell(4).SetCellValue("产品名称");
                row1.CreateCell(5).SetCellValue("规格");
                row1.CreateCell(6).SetCellValue("批号");
                row1.CreateCell(7).SetCellValue("数量");

                //将数据逐步写入sheet1各个行
                for (int i = 0; i < list.Count(); i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(list[i].ship_no);
                    rowtemp.CreateCell(1).SetCellValue(list[i].bsnno);
                    rowtemp.CreateCell(2).SetCellValue(list[i].ship_date.ToString());
                    rowtemp.CreateCell(3).SetCellValue(list[i].p_no);
                    rowtemp.CreateCell(4).SetCellValue(list[i].pname);
                    rowtemp.CreateCell(5).SetCellValue(list[i].type);
                    rowtemp.CreateCell(6).SetCellValue(list[i].lot_no);
                    rowtemp.CreateCell(7).SetCellValue(list[i].mqty.ToString());
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", shipno + "入库数据.xls");
            }
            ViewBag.fgi = fgi;
            ViewBag.parme = "shipno=" + shipno + "&stno=" + stno + "&pdano=" + pdano+"&pno="+pno;
            return View();
        }
       

        public class fgiinfo
        {
            public string p_no { get; set; }
            public string pname { get; set; }
            public string bsnno { get; set; }
            public string st_no { get; set; }
            public string st_name { get; set; }

        }
        public ActionResult GetShipCode()
        {
            var tmbh = Request["tmbh"];
            ConnectionStringEntities db = new ConnectionStringEntities();
            var fgi = db.sal_fgi.Where(d => d.bsnno.Contains(tmbh));
            List<sal_fgi> lfgi = fgi.ToList();
            var lf = lfgi.Select(item => new fgiinfo
            {
                p_no = item.p_no,
                pname = item.pname,
                bsnno = item.bsnno,
                st_no = item.st_no,
                st_name = getname("s", item.st_no),
            }).Take(10);

            return Json(lf);
        }
        public string getname(string type, string parme)
        {
            ConnectionStringEntities db = new ConnectionStringEntities();
            if (type == "c")
            {
                var cmodel = db.customer.FirstOrDefault(d => d.cu_no == parme);
                if (cmodel == null)
                    return "";
                return cmodel.cu_name;
            }
            else if (type == "p")
            {
                var cmodel = db.Inv_Part.FirstOrDefault(d => d.p_no == parme);
                if (cmodel == null)
                    return "";
                return cmodel.pname;
            }
            else if (type == "s")
            {
                var smodel = db.Inv_Store.FirstOrDefault(d => d.st_no == parme);
                if (smodel == null)
                    return "";
                return smodel.st_name.Trim();
            }
            return "";
        }
        /// <summary>
        /// 出库扫描页
        /// </summary>
        /// <returns></returns>
        public ActionResult SalShipScanPage()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            ViewBag.kb = dbHelpers.Inv_Store.OrderByDescending(db => db.fid);
            ViewBag.prod = dbHelpers.Inv_Part.OrderByDescending(db => db.fid);
            ViewBag.cus = dbHelpers.customer.OrderByDescending(db => db.fid);
            ViewBag.fgino = "S" + Uties.GetOrderNumber();

            return View();
        }

        /// <summary>
        /// 出库扫描操作
        /// </summary>
        /// <returns></returns>
        public ActionResult SalShipScan(sal_ship ss,string cpsl,string js)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            string strxt2dsplit = ConfigurationManager.AppSettings["xt2dsplit"].ToString();

            int t = ss.bsnno.LastIndexOf(strxt2dsplit);                     

            if (t > 0 && ss.bsnno.Length > (t + 1))
            {
                ss.bsnno = ss.bsnno.Substring(t + 1, ss.bsnno.Length - t - 1);
            }

            int cps = 0;
            if (!string.IsNullOrEmpty(cpsl))
                cps = int.Parse(cpsl);

            int pjs = 0;
            if (!string.IsNullOrEmpty(js))
                pjs = int.Parse(js);

            ss.mqty = 1;

            string p_no = "";
            string pname = "";
            

            var isship = dbHelpers.sal_ship.FirstOrDefault(db => db.bsnno == ss.bsnno && db.upyn=="N");
            if (isship != null)
                return Json(new { status = false, data = ss.bsnno + "重复出库！<br/>" });



            brBarcode brcd = new brBarcode();


            Barcode barcode = brcd.GetBarcodeEntity(ss.bsnno, "");

            if (barcode != null)
            {
                ss.mqty = barcode.Qty;                
                p_no = barcode.p_no;
            }
            if(ss.mqty==0)
            {
                return Json(new { status = false, data = ss.bsnno + "物流码不正确！<br/>" });
            }       
     
            if(string.IsNullOrEmpty(ss.p_no))            
                ss.p_no = p_no;

            if (ss.p_no != p_no)
                return Json(new { status = false, data = "出库产品与包装入库产品不一致！<br/>" });

            //if(string.IsNullOrEmpty(ss.p_no))
            //    return Json(new { status = false, data = "请选择产品！<br/>" });
           
            ss.pname = DataEnum.GetPName(p_no);
            ss.pda_no = this.xtUser.UserNo;
            ss.unitcode = "";
            ss.upyn = "N";
            dbHelpers.sal_ship.Add(ss);
            if (dbHelpers.SaveChanges() > 0)
            {                
                return Json(new { status = true, data = ss.bsnno + "出库成功！<br/>", js = pjs+1, cpsl=cps+ss.mqty });
            }
            return Json(new { status = false, data = ss.bsnno + "出库失败！<br/>" });
        }

        public class shipinfo
        {
            public string p_no { get; set; }
            public string pname { get; set; }
            public string bsnno { get; set; }
            public string st_no { get; set; }
            public string st_name { get; set; }
            public string cu_no { get; set; }
            public string cu_name { get; set; }

        }
        public ActionResult GetReCode()
        {
            var tmbh = Request["tmbh"];
            ConnectionStringEntities db = new ConnectionStringEntities();
            var fgi = db.sal_ship.Where(d => d.bsnno.Contains(tmbh) && d.bsnno=="N");
            List<sal_ship> lfgi = fgi.ToList();
            var lf = lfgi.Select(item => new shipinfo
            {
                p_no = item.p_no,
                pname = item.pname,
                bsnno = item.bsnno,
                st_no = item.st_no,
                st_name = getname("s", item.st_no),
                cu_no=item.cu_no,
                cu_name=item.cu_name,
            }).Take(10);

            return Json(lf);
        }
        /// <summary>
        /// 退货扫描页
        /// </summary>
        /// <returns></returns>
        public ActionResult SalReScanPage()
        {
            
            ViewBag.fgino = "R" + Uties.GetOrderNumber();

            return View();
        }

        /// <summary>
        /// 退货扫描操作
        /// </summary>
        /// <returns></returns>
        public ActionResult SalReScan( sal_re sr,string cpsl,string js)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            

            int cps = 0;
            if (!string.IsNullOrEmpty(cpsl))
                cps = int.Parse(cpsl);

            int pjs = 0;
            if (!string.IsNullOrEmpty(js))
                pjs = int.Parse(js);

            var ship = dbHelpers.sal_ship.FirstOrDefault(db => db.bsnno == sr.bsnno );
            if (ship==null)
                return Json(new { status = false, data = sr.bsnno + "条码错误！<br/>" });


            var isship = dbHelpers.sal_re.FirstOrDefault(db => db.bsnno == sr.bsnno && db.ship_date>=ship.ship_date);
            if (isship != null)
                return Json(new { status = false, data = sr.bsnno + "重复退货！<br/>" });
          
            sr.mqty = ship.mqty;
            if(string.IsNullOrEmpty(sr.p_no))
            {
                sr.p_no = ship.p_no;
                sr.pname = ship.pname;
            }
            if (string.IsNullOrEmpty(sr.cu_no))
            {
                sr.cu_no = ship.cu_no;
                sr.cu_name = ship.cu_name;
            }
            sr.pda_no = this.xtUser.UserNo;
            sr.unitcode = "";
            sr.upyn = "N";
            dbHelpers.sal_re.Add(sr);
            if (dbHelpers.SaveChanges() > 0)
            {
                var sship = dbHelpers.sal_ship.FirstOrDefault(db => db.bsnno == sr.bsnno && db.upyn == "N");
                if (sship != null)
                {
                    sship.upyn = "R";
                    dbHelpers.SaveChanges();
                }
                return Json(new { status = true, data = sr.bsnno + "退货成功！<br/>", js = pjs + 1, cpsl = cps + sr.mqty });
            }
            return Json(new { status = false, data = sr.bsnno + "退货失败！<br/>" });
        }

        public ActionResult Sal_ShipEdit()
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
            var shipname = Request["shipname"];
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            var cus = Request["cus"];
            var pro = Request["pro"];
            var shipno = Request["shipno"];
            var cname = Request["cname"];
            var pname = Request["pname"];
            ViewBag.cname = cname;
            ViewBag.pname = pname;
            ViewBag.shipname = shipname;
            ViewBag.cus = cus;
            ViewBag.pro = pro;
            ViewBag.shipno = shipno;
            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_ship> lspb;
            db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = db.sal_ship.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            else
            {

                DateTime sdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                lspb = db.sal_ship.Where(d => d.ship_date >= sdate && d.ship_date <= DateTime.Now);
            }

            if (!string.IsNullOrEmpty(shipname))
            {
                lspb = lspb.Where(d => d.bsnno == shipname);
                parme += "shipname=" + shipname;
            }
            if (!string.IsNullOrEmpty(cus))
            {
                lspb = lspb.Where(d => d.cu_no == cus);
                parme += "&cus=" + cus + "&cname=" + cname;
            }
            if (!string.IsNullOrEmpty(pro))
            {
                lspb = lspb.Where(d => d.p_no == pro);
                parme += "&pro=" + pro + "&pname=" + pname;
            }
            if (!string.IsNullOrEmpty(shipno))
            {
                lspb = lspb.Where(d => d.ship_no == shipno);
                parme += "&shipno=" + shipno;
            }
            List<sal_ship> ss = lspb.OrderByDescending(d => d.fid).Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            ViewBag.startdate = startdate;
            ViewBag.enddate = enddate;

            ViewBag.list = ss;
            ViewBag.count = lspb.Count();
            ViewBag.pageno = pageno;
            ViewBag.pagecount = (int)Math.Ceiling((decimal)lspb.Count() / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        public ActionResult Sal_ShipUpd()
        {

            string parme = "";
            var shipname = Request["shipname"];
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            var cus = Request["cus"];
            var pro = Request["pro"];
            var shipno = Request["shipno"];

            var ncno = Request["ncno"];
            var ncname = Request["ncname"];
            var npno = Request["npno"];
            var npname = Request["npname"];
            var nshipno = Request["nshipno"];
            var nshipdate = Request["nshipdate"];

            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_ship> lspb;
            db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = db.sal_ship.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            else
            {

                DateTime sdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                lspb = db.sal_ship.Where(d => d.ship_date >= sdate && d.ship_date <= DateTime.Now);
            }

            if (!string.IsNullOrEmpty(shipname))
                lspb = lspb.Where(d => d.bsnno == shipname);

            if (!string.IsNullOrEmpty(cus))
                lspb = lspb.Where(d => d.cu_no == cus);

            if (!string.IsNullOrEmpty(pro))
                lspb = lspb.Where(d => d.p_no == pro);

            if (!string.IsNullOrEmpty(shipno))
                lspb = lspb.Where(d => d.ship_no == shipno);

            int i = 0;
            try
            {
                foreach (var item in lspb)
                {
                    if (!string.IsNullOrEmpty(ncno))
                    {
                        item.cu_no = ncno;
                        item.cu_name = ncname;
                    }
                    if (!string.IsNullOrEmpty(npno))
                    {
                        item.p_no = npno;
                        item.pname = npname;
                    }
                    if (!string.IsNullOrEmpty(nshipno))
                        item.ship_no = nshipno;
                    if (!string.IsNullOrEmpty(nshipdate))
                        item.ship_date = DateTime.Parse(nshipdate);

                }
            }
            catch (Exception e)
            {
                return Json(new { status = "error", data = e.Message });
            }

            i = db.SaveChanges();
            return Json(new { status = "success", data = "成功修改" + i + "条数据" });
        }

        public ActionResult Sal_TstList()
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
            var shipname = Request["shipname"];
            if (!string.IsNullOrEmpty(shipname))
                parme += "shipname=" + shipname;

            var startdate = Request["startdate"];
            var enddate = Request["enddate"];

            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_tst> lspb = db.sal_tst;
            db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = lspb.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }


            if (!string.IsNullOrEmpty(shipname))
            {
                lspb = lspb.Where(d => d.bsnno == shipname || d.ship_no == shipname);
                parme += "shipname=" + shipname;
            }

            List<sal_tst> ss = lspb.OrderByDescending(d => d.fid).Skip((pageno - 1) * pagesize).Take(pagesize).ToList();

            ViewBag.list = ss;
            ViewBag.count = lspb.Count();
            ViewBag.pageno = pageno;
            ViewBag.pagecount = (int)Math.Ceiling((decimal)lspb.Count() / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        public ActionResult Sal_TstAdd()
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
            var shipname = Request["shipname"];
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            var okb = Request["okb"];
            var pro = Request["pro"];
            var shipno = Request["shipno"];

            ViewBag.okb = okb;
            ViewBag.pname = Request["pname"];
            ViewBag.shipname = shipname;
            ViewBag.pro = pro;
            ViewBag.shipno = shipno;
            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_fgi> lspb;
            db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = db.sal_fgi.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            else
            {

                DateTime sdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                lspb = db.sal_fgi.Where(d => d.ship_date >= sdate && d.ship_date <= DateTime.Now);
            }

            if (!string.IsNullOrEmpty(shipname))
            {
                lspb = lspb.Where(d => d.bsnno == shipname);
                parme += "shipname=" + shipname;
            }

            if (!string.IsNullOrEmpty(pro))
            {
                lspb = lspb.Where(d => d.p_no == pro);
                parme += "&pro=" + pro;
            }
            if (!string.IsNullOrEmpty(shipno))
            {
                lspb = lspb.Where(d => d.ship_no == shipno);
                parme += "&shipno=" + shipno;
            }
            if (!string.IsNullOrEmpty(okb))
            {
                lspb = lspb.Where(d => d.st_no == okb);
                parme += "&okb=" + okb;
            }

            List<sal_fgi> ss = lspb.OrderByDescending(d => d.fid).Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            ViewBag.st = db.Inv_Store;
            ViewBag.startdate = startdate;
            ViewBag.enddate = enddate;
            ViewBag.list = ss;
            ViewBag.count = lspb.Count();
            ViewBag.pageno = pageno;
            ViewBag.pagecount = (int)Math.Ceiling((decimal)lspb.Count() / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        public ActionResult Sal_TstUpd()
        {
            string parme = "";
            var shipname = Request["shipname"];
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            var okb = Request["okb"];
            var pro = Request["pro"];
            var shipno = Request["shipno"];
            var nkb = Request["nkb"];

            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_fgi> lspb;
            db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = db.sal_fgi.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            else
            {

                DateTime sdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                lspb = db.sal_fgi.Where(d => d.ship_date >= sdate && d.ship_date <= DateTime.Now);
            }

            if (!string.IsNullOrEmpty(shipname))
                lspb = lspb.Where(d => d.bsnno == shipname);

            if (!string.IsNullOrEmpty(pro))
                lspb = lspb.Where(d => d.p_no == pro);

            if (!string.IsNullOrEmpty(shipno))
                lspb = lspb.Where(d => d.ship_no == shipno);

            if (!string.IsNullOrEmpty(okb))
                lspb = lspb.Where(d => d.st_no == okb);

            int i = 0;
            try
            {
                foreach (var it in lspb)
                {
                    var isdb = db.sal_tst.FirstOrDefault(d => d.nst_no == nkb && d.ost_no == it.st_no);

                    if (!string.IsNullOrEmpty(nkb) && isdb == null && nkb.Trim() != okb.Trim())
                    {
                        sal_tst st = new sal_tst();
                        st.bsnno = it.bsnno;
                        st.mqty = it.mqty;
                        st.p_no = it.p_no;
                        st.pname = it.pname;
                        st.ship_no = it.ship_no;
                        st.ship_date = it.ship_date;
                        st.type = it.type;
                        st.ost_no = it.st_no;
                        st.nst_no = nkb;
                        db.sal_tst.Add(st);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "error", data = e.Message });
            }

            i = db.SaveChanges();
            return Json(new { status = "success", data = "成功调拨" + i + "条数据" });
        }

        [Manage]       
        public ActionResult SalStockList()
        {

            ConnectionStringEntities db = new ConnectionStringEntities();
            try
            {

                string sql = "exec pinv_stqty";
                db.Database.ExecuteSqlCommand(sql);
            }
            catch { }


            int pageno = 1;
            if (!string.IsNullOrEmpty(Request["pageno"]))
                pageno = int.Parse(Request["pageno"]);

            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request["pagesize"]))
                pagesize = int.Parse(Request["pagesize"]);

            string parme = "";

            var pno = Request["pno"];

            IQueryable<Inv_Stqty> lspb = db.Inv_Stqty;           

            if (!string.IsNullOrEmpty(pno))
            {
                
                lspb = lspb.Where(d => d.p_no==pno);

                parme += "&pno=" + pno;
            }

            if(Request["dc"]=="ok")
            {
                var list = lspb.ToList();
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                string[] line1 = new string[] { "产品编号", "产品名称", "产品规格", "包装入库量", "直接入库量", "出库量", "退货量", "库存" };

                for (int i = 0; i < line1.Length; i++)
                {
                    row1.CreateCell(i).SetCellValue(line1[i]);
                }

                //将数据逐步写入sheet1各个行
                for (int i = 0; i < list.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    string[] line2 = new string[]{list[i].p_no,DataEnum.GetPName(list[i].p_no),DataEnum.GetPType(list[i].p_no)
                        ,list[i].inqty.ToString(),list[i].fgiqty.ToString(),list[i].outqty.ToString(),list[i].bakqty.ToString(),list[i].stqty.ToString()};

                    for (int l = 0; l < line2.Length; l++)
                    {
                        rowtemp.CreateCell(l).SetCellValue(line2[l]);
                    }

                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "库存数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }

            var count = lspb.Count();
            var slist = lspb.OrderBy(d => d.fid).Skip((pageno - 1) * pagesize).Take(pagesize);
            ViewBag.list = slist.ToList();
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            ViewBag.url = "SalStockList";
            return View();
        }

        public int? shipnum(string pno)
        {
            
            ConnectionStringEntities db = new ConnectionStringEntities();
            var ship = db.sal_ship.Where(x => x.p_no == pno).Sum(x => x.mqty);
            if (ship == null)
                ship = 0;
            return ship;
        }
    }
}