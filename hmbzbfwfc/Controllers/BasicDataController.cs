using hmbzbfwfc.Commons;

using hmbzbfwfc.Attributes;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using marr.BusinessRule;
using marr.BusinessRule.Entity;



namespace hmbzbfwfc.Controllers
{
   
    //基本资料管理控制器
    public class BasicDataController : ManageBaseController
    {
        // GET: BasicData
 


        public class shiplist{
        public int fid {get;set;}
         public string ship_no{get;set;}
         public string bsnno{get;set;}
         public DateTime? ship_date{get;set;}
         public string pda_no{get;set;}
         public string cu_name{get;set;}
         public string pname { get; set; }
         public int? mqty { get; set; }
         public int? pqty { get; set; }
         public string p_no { get; set; }
         public string cu_no { get; set; }
         public string npyn { get; set; }
        }

        /// <summary>
        /// 出货明细
        /// </summary>
        /// <returns></returns>
        [Manage]
        #region shipList
        /*
        public ActionResult ShipList()
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

            var startdate=Request["startdate"];
            var enddate=Request["enddate"];
            string dateparme="and 1=1";
            if (!string.IsNullOrEmpty(startdate))
            {
                dateparme = "and ship_date between '" + startdate + "' and '" + enddate + "'";
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            ConnectionStringEntities db = new ConnectionStringEntities();
            PagesImpl pageImpl = new PagesImpl(db);
            PageinationInfo pageination = new PageinationInfo();
            pageination.TableName = "sal_ship";
            pageination.PageIndex = pageno;
            pageination.PageSize = pagesize;
            pageination.FieldName = "*";
            pageination.OrderCondition = "fid desc";
            pageination.WhereCondition = "(ship_no like '%" + shipname + "%' or bsnno like  '%" + shipname + "%' ) " + dateparme + "";
            var data = pageImpl.GetPageinationData<sal_ship>(pageination);

            List<sal_ship> ss = data.DataList;
            List<shiplist> lss = ss.Select(item => new shiplist { 
              fid =item.fid,
              ship_no=item.ship_no,
              bsnno=item.bsnno,
              ship_date=item.ship_date,
              pda_no=item.pda_no,
              cu_name=item.cu_name,
              pname =item.pname,
              mqty=item.mqty,
              pqty = db.Inv_Part.FirstOrDefault(d => d.p_no == item.p_no) == null ? 0 : db.Inv_Part.FirstOrDefault(d => d.p_no == item.p_no).pqty,
            }).ToList();

            ViewBag.list = lss;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }
         */
        #endregion  

        
        public ActionResult ShipList()
        {
            int pageno = 1;
            if (!string.IsNullOrEmpty(Request["pageno"]))
                pageno = int.Parse(Request["pageno"]);

            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request["pagesize"]))
                pagesize = int.Parse(Request["pagesize"]);

            string parme = "";
            var selparme = Request["selparme"];
            if (!string.IsNullOrEmpty(selparme))
                parme += "&selparme=" + selparme;

            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            var cpname = Request["cpname"];

            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_ship> lspb = db.sal_ship;
            //db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = lspb.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }

            if (!string.IsNullOrEmpty(selparme))
                lspb = lspb.Where(d => d.ship_no == selparme || d.bsnno == selparme);

            if (!string.IsNullOrEmpty(cpname))
            {
                lspb = lspb.Where(x => x.cu_no.Contains(cpname) || x.cu_name.Contains(cpname) || x.p_no.Contains(cpname) || x.pname.Contains(cpname));
                parme += "&cpname=" + cpname;
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
               
               row1.CreateCell(0).SetCellValue("出库单号");
               row1.CreateCell(1).SetCellValue("条码编号");
               row1.CreateCell(2).SetCellValue("出库日期");
               row1.CreateCell(3).SetCellValue("产品名称");
               row1.CreateCell(4).SetCellValue("产品规格");
               row1.CreateCell(5).SetCellValue("客户名称");
               row1.CreateCell(6).SetCellValue("数量");


               //将数据逐步写入sheet1各个行
               for (int i = 0; i < list.Count; i++)
               {
                   NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);                   
                   rowtemp.CreateCell(0).SetCellValue(list[i].ship_no);
                   rowtemp.CreateCell(1).SetCellValue(list[i].bsnno);
                   rowtemp.CreateCell(2).SetCellValue(list[i].ship_date.ToString());                   
                   rowtemp.CreateCell(3).SetCellValue(list[i].pname);
                   rowtemp.CreateCell(4).SetCellValue(DataEnum.GetPType(list[i].p_no));
                   rowtemp.CreateCell(5).SetCellValue(list[i].cu_name);
                   rowtemp.CreateCell(6).SetCellValue(list[i].mqty.ToString());


               }
               // 写入到客户端 
               System.IO.MemoryStream ms = new System.IO.MemoryStream();
               book.Write(ms);
               ms.Seek(0, SeekOrigin.Begin);
               return File(ms, "application/vnd.ms-excel", "出库信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
           }

            var count = lspb.Count();
            var slist = lspb.OrderByDescending(d => d.ship_date).Skip((pageno - 1) * pagesize).Take(pagesize);
            ViewBag.zj = slist.Sum(d => d.mqty);
            ViewBag.list = slist.ToList();
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        [Manage]
        public JsonResult ShipDel()
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
                        var umodel = dbHelpers.sal_ship.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.sal_ship.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.sal_ship.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.sal_ship.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }
        public ActionResult SeeShipInfo()
        {
            var shipno = Request["shipno"];           
            var pdano = Request["pdano"];
            var cno = Request["cno"];
            var pno = Request["pno"];
            var dc = Request["dc"];

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fgi = dbHelpers.sal_ship.Where(db => db.ship_no == shipno);
            if (!string.IsNullOrEmpty(pdano))
                fgi = fgi.Where(db => db.pda_no == pdano);
            else
                fgi = fgi.Where(db => db.pda_no == null || db.pda_no == "");

            if (!string.IsNullOrEmpty(pno))
                fgi = fgi.Where(db => db.p_no == pno);
            else
                fgi = fgi.Where(db => db.p_no == null || db.p_no == "");

            if (!string.IsNullOrEmpty(cno))
                fgi = fgi.Where(db => db.cu_no == cno);
            else
                fgi = fgi.Where(db => db.cu_no == null || db.cu_no == "");

            if (dc == "ok")
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
                row1.CreateCell(2).SetCellValue("出库日期");
                row1.CreateCell(3).SetCellValue("产品编号");
                row1.CreateCell(4).SetCellValue("产品名称");
                row1.CreateCell(5).SetCellValue("经销商编号");
                row1.CreateCell(6).SetCellValue("经销商名称");
                //row1.CreateCell(7).SetCellValue("规格");
                row1.CreateCell(7).SetCellValue("批号");
                row1.CreateCell(8).SetCellValue("数量");

                //将数据逐步写入sheet1各个行
                for (int i = 0; i < list.Count(); i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(list[i].ship_no);
                    rowtemp.CreateCell(1).SetCellValue(list[i].bsnno);
                    rowtemp.CreateCell(2).SetCellValue(list[i].ship_date.ToString());
                    rowtemp.CreateCell(3).SetCellValue(list[i].p_no);
                    rowtemp.CreateCell(4).SetCellValue(list[i].pname);
                    rowtemp.CreateCell(5).SetCellValue(list[i].cu_no);
                    rowtemp.CreateCell(6).SetCellValue(list[i].cu_name);
                    // rowtemp.CreateCell(7).SetCellValue(list[i].type);
                    rowtemp.CreateCell(7).SetCellValue(list[i].lot_no);
                    rowtemp.CreateCell(8).SetCellValue(list[i].mqty.ToString());
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", shipno + "出库数据.xls");
            }

            ViewBag.fgi = fgi;
            ViewBag.parme = "shipno=" + shipno + "&pdano=" + pdano + "&pno=" + pno + "&cno=" + cno;
            return View();
        }
       

        
        /// <summary>
        /// 入库明细
        /// </summary>
        [Manage]
        public ActionResult FgiList()
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
            var fginame = Request["fginame"];
            if (!string.IsNullOrEmpty(fginame))
                parme += "fginame=" + fginame;

            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            string dateparme = "and 1=1";
            if (!string.IsNullOrEmpty(startdate))
            {
                dateparme = "and ship_date between '" + startdate + "' and '" + enddate + "'";
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            var unitcode=DESEncrypt.Decrypt(Request.Cookies["unitcode"].Value);

            ConnectionStringEntities db = new ConnectionStringEntities();
            PagesImpl pageImpl = new PagesImpl(db);
            PageinationInfo pageination = new PageinationInfo();
            pageination.TableName = "sal_fgi";
            pageination.PageIndex = pageno;
            pageination.PageSize = pagesize;
            pageination.FieldName = "*";
            pageination.OrderCondition = "fid desc";
            pageination.WhereCondition = " unitcode='"+unitcode+"' ship_no like '%" + fginame + "%' " + dateparme + "";
            var data = pageImpl.GetPageinationData<sal_fgi>(pageination);

            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        /// <summary>
        /// 退货明细
        /// </summary>
        /// <returns></returns>

        [Manage]
        public ActionResult ReList()
        {
            int pageno = 1;
            if (!string.IsNullOrEmpty(Request["pageno"]))
                pageno = int.Parse(Request["pageno"]);

            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request["pagesize"]))
                pagesize = int.Parse(Request["pagesize"]);

            string parme = "";
            var rename = Request["rename"];
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            var cpname = Request["cpname"];

            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_re> lspb = db.sal_re;

            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                lspb = lspb.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }

            if (!string.IsNullOrEmpty(rename))
            {
                lspb = lspb.Where(d => d.ship_no == rename || d.bsnno == rename);
                parme += "&rename=" + rename;
            }
            if (!string.IsNullOrEmpty(cpname))
            {
                lspb = lspb.Where(x => x.cu_no.Contains(cpname) || x.cu_name.Contains(cpname) || x.p_no.Contains(cpname) || x.pname.Contains(cpname));
                parme += "&cpname=" + cpname;
            }

            if (Request["dc"] == "ok")
            {
                var list = lspb.ToList();
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

                row1.CreateCell(0).SetCellValue("单号");
                row1.CreateCell(1).SetCellValue("条码编号");
                row1.CreateCell(2).SetCellValue("日期");
                row1.CreateCell(3).SetCellValue("产品名称");
                row1.CreateCell(4).SetCellValue("产品规格");
                row1.CreateCell(5).SetCellValue("经销商名称");
                row1.CreateCell(6).SetCellValue("数量");


                //将数据逐步写入sheet1各个行
                for (int i = 0; i < list.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(list[i].ship_no);
                    rowtemp.CreateCell(1).SetCellValue(list[i].bsnno);
                    rowtemp.CreateCell(2).SetCellValue(list[i].ship_date.ToString());
                    rowtemp.CreateCell(3).SetCellValue(list[i].pname);
                    rowtemp.CreateCell(4).SetCellValue(DataEnum.GetPType(list[i].p_no));
                    rowtemp.CreateCell(5).SetCellValue(list[i].cu_name);
                    rowtemp.CreateCell(6).SetCellValue(list[i].mqty.ToString());


                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "退货数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }

            var count = lspb.Count();
            var slist = lspb.OrderByDescending(d => d.ship_date).Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            ViewBag.zj = slist.Sum(d => d.mqty);
            ViewBag.list = slist;
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        public ActionResult SeeReInfo()
        {
            var shipno = Request["shipno"];

            var pdano = Request["pdano"];

            var cno = Request["cno"];
            var pno = Request["pno"];
            var dc = Request["dc"];

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fgi = dbHelpers.sal_re.Where(db => db.ship_no == shipno);
            if (!string.IsNullOrEmpty(pdano))
                fgi = fgi.Where(db => db.pda_no == pdano);
            else
                fgi = fgi.Where(db => db.pda_no == null || db.pda_no == "");

            if (!string.IsNullOrEmpty(pno))
                fgi = fgi.Where(db => db.p_no == pno);
            else
                fgi = fgi.Where(db => db.p_no == null || db.p_no == "");

            if (!string.IsNullOrEmpty(cno))
                fgi = fgi.Where(db => db.cu_no == cno);
            else
                fgi = fgi.Where(db => db.cu_no == null || db.cu_no == "");

            if (dc == "ok")
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
                row1.CreateCell(2).SetCellValue("退货日期");
                row1.CreateCell(3).SetCellValue("产品编号");
                row1.CreateCell(4).SetCellValue("产品名称");
                row1.CreateCell(5).SetCellValue("经销商编号");
                row1.CreateCell(6).SetCellValue("经销商名称");
                //row1.CreateCell(7).SetCellValue("规格");
                row1.CreateCell(7).SetCellValue("批号");
                row1.CreateCell(8).SetCellValue("数量");

                //将数据逐步写入sheet1各个行
                for (int i = 0; i < list.Count(); i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(list[i].ship_no);
                    rowtemp.CreateCell(1).SetCellValue(list[i].bsnno);
                    rowtemp.CreateCell(2).SetCellValue(list[i].ship_date.ToString());
                    rowtemp.CreateCell(3).SetCellValue(list[i].p_no);
                    rowtemp.CreateCell(4).SetCellValue(list[i].pname);
                    rowtemp.CreateCell(5).SetCellValue(list[i].cu_no);
                    rowtemp.CreateCell(6).SetCellValue(list[i].cu_name);
                    //rowtemp.CreateCell(7).SetCellValue(list[i].type);
                    rowtemp.CreateCell(7).SetCellValue(list[i].lot_no);
                    rowtemp.CreateCell(8).SetCellValue(list[i].mqty.ToString());
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", shipno + "退货数据.xls");
            }

            ViewBag.fgi = fgi;
            ViewBag.parme = "shipno=" + shipno + "&pdano=" + pdano+"&pno="+pno+"&cno="+cno;
            return View();
        }

        [Manage]
        public JsonResult ReToVender()
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
                        var umodel = dbHelpers.sal_re.FirstOrDefault(db => db.fid.ToString() == i);
                        umodel.upyn = "V";                        
                    }
                }
                if(dbHelpers.SaveChanges()>0)
                    return Json("退回成功");
                return Json("退回失败");
            }

            var model = dbHelpers.sal_re.FirstOrDefault(db => db.fid.ToString() == fid);
            model.upyn = "V";
            if(dbHelpers.SaveChanges()>0)
                return Json("退回成功");
            return Json("删除失败");
        }
        public ActionResult SystemDataImportPage()
        {
            var filetype=Request["filetype"];
            ViewBag.filetype = filetype;
            return View();
        }
      
        /// <summary>
        /// 系统参数导入
        /// </summary>
        [Manage]
        public ActionResult SystemDataImport(sal_packa spa)
        {
            bool codeyn = false;
            bool snmmyn = false;
            int intcode = 1;
            int intsnmm = 1;
            try
            {
                ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
                sal_packa sp = dbHelpers.sal_packa.FirstOrDefault(db => db.fid == spa.fid);

                StringBuilder infoBuilder = new StringBuilder();
                string intbname = "";

                FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + spa.relativepath, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string line;
                int inum = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        if (line.Substring(0, 2) == "##") //    代码
                        {
                            intbname = line.Replace("##", "");

                        }
                        else
                        {
                            string[] asnn = line.Split('|');
                            if (intbname == "dhm_cust" && asnn.Length >= 6)
                            {
                                string myunitcode = asnn[0].Trim();

                                var delcust = dbHelpers.dhm_cust.Where(db => db.UnitCode == myunitcode);
                              
                                dbHelpers.dhm_cust.RemoveRange(delcust);



                                string strSql = "insert into dhm_cust(unitcode,unitname,remark,RegDate,codeLen,renote,wlyn,wllen,snyn,vyn,vlen)   values('" + asnn[0] + "','" + asnn[1] + "','" + asnn[2] + "','" + asnn[3] + "'," + asnn[4] + ",'" + asnn[6] + "','" + asnn[7] + "'," + asnn[8] + ",'" + asnn[9] + "','" + asnn[10] + "'," + asnn[11] + ")";
                                int i = DbHelperSQL.ExecuteSql(strSql);



                            }

                            if (intbname == "dhm_code" && asnn.Length >= 6)
                            {
                                //判断是否之前有没有导入
                                if (intcode == 1)
                                {
                                    var ccodem = dbHelpers.dhm_code.Where(db => db.codelen == int.Parse(asnn[0]));
                                    try
                                    {
                                        if (ccodem.Count() > 0)
                                        {
                                            //删除原来的
                                            var delcode = dbHelpers.dhm_code.Where(db => db.codelen == int.Parse(asnn[0]));
                                            dbHelpers.dhm_code.RemoveRange(delcode);
                                            codeyn = false;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        codeyn = false;
                                    }
                                }
                                if (!codeyn)
                                {
                                    //           strtmp = rstmp1!codelen & "|" & rstmp1!Address & "|" & rstmp1!codea & "|" & rstmp1!codeb & "|" & rstmp1!codec & "|" & rstmp1!coded & "|" & rstmp1!vcode

                                    string strSql = "insert into dhm_code(codelen,address,codea,codeb,codec,coded,vcode) values(" + asnn[0] + "," + asnn[1] + ",'" + asnn[2] + "','" + asnn[3] + "','" + asnn[4] + "','" + asnn[5] + "','" + asnn[6] + "')";
                                    int ii = DbHelperSQL.ExecuteSql(strSql);
                                }
                                intcode++;
                            }
                            if (intbname == "dhm_snmm" && asnn.Length >= 6)
                            {
                                //判断是否之前有没有导入
                                if (intsnmm == 1)
                                {
                                    var dsnmm = dbHelpers.dhm_snmm.Where(db => db.codelen == int.Parse(asnn[0]));
                                    try
                                    {
                                        if (dsnmm.Count() > 0)
                                        {
                                            var delcode = dbHelpers.dhm_snmm.Where(db => db.codelen == int.Parse(asnn[0]));
                                            dbHelpers.dhm_snmm.RemoveRange(delcode);
                                            snmmyn = false;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        codeyn = false;
                                    }
                                }
                                if (!snmmyn)
                                {
                                    string strSql = "insert into dhm_snmm(codelen,address,codea,codeb,codec,coded,vcode) values(" + asnn[0] + "," + asnn[1] + ",'" + asnn[2] + "','" + asnn[3] + "','" + asnn[4] + "','" + asnn[5] + "','" + asnn[6] + "')";

                                    int ii = DbHelperSQL.ExecuteSql(strSql);
                                }
                                intsnmm++;
                            }
                        }
                        inum++;
                    }
                    catch (Exception ex)
                    {
                        sr.Close();
                        infoBuilder.Append(string.Format("添加记录{0}出错: {1}. 忽略.<br />", line, ex.Message));
                        return Json(new { status = "error", data = infoBuilder.ToString() }, JsonRequestBehavior.AllowGet);
                    }
                }
                sr.Close();
                infoBuilder.Append(string.Format("成功导入" + inum + "条系统参数！!"));
                sp.status = 9;
                sp.okqty = inum;
                sp.uplog = "成功导入"+inum+"条系统参数！";
                dbHelpers.SaveChanges();
                return Json(new { status = "success", data = infoBuilder.ToString() },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", data = "导入失败" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 发行记录导入
        /// </summary>

        public ActionResult FxDataImport(sal_packa spa)
        {
            try
            {              
                
                ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
                sal_packa sp = dbHelpers.sal_packa.FirstOrDefault(db=>db.fid==spa.fid);

                StringBuilder infoBuilder = new StringBuilder();
                string intbname = "";

                FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + spa.relativepath, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string line;
                int inum = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        try
                        {
                            if (line.Substring(0, 2) == "##") //    代码
                            {
                                intbname = line.Replace("##", "");
                            }
                            else
                            {
                                string[] asnn = line.Split('|');

                                if (intbname == "dhm_fwout" && asnn.Length >= 14)
                                {

                                    string strsql2 = "delete dhm_fwout where UnitCode='" + asnn[0] + "' and myBegin=" + asnn[4] + "";

                                    DbHelperSQL.ExecuteSql(strsql2);


                                    if (asnn[13].Trim().Length == 0)
                                    {
                                        asnn[13] = "0";
                                    }


                                        string strSql = "insert into dhm_fwout(UnitCode,SellDateTime,SellCount,mqty,myBegin,packtype,pdqty,pzqty,pxqty,snbegin,snend,sxqty,bxqty,codelen)    values('" + asnn[0] + "','" + asnn[1] + "'," + asnn[2] + "," + asnn[3] + "," + asnn[4] + ",'" + asnn[5] + "'," + asnn[6] + "," + asnn[7] + "," + asnn[8] + ",'" + asnn[9] + "','" + asnn[10] + "'," + asnn[11] + "," + asnn[12] + "," + asnn[13] + ")";

                                        int i = DbHelperSQL.ExecuteSql(strSql);


                                }
                            }
                            inum++;
                        }
                        catch (Exception ex)
                        {
                            sr.Close();
                            infoBuilder.Append(string.Format("添加记录{0}出错: {1}. 忽略.<br />", line, ex.Message));
                            return Json(new { status = "error", data = infoBuilder.ToString() }, JsonRequestBehavior.AllowGet);
                        }
                    }
                     sr.Close();
                     infoBuilder.Append(string.Format("成功导入" + inum + "条系统参数！!"));
                     sp.status = 9;
                     sp.okqty = inum;
                     sp.uplog = "成功导入" + inum + "条发行参数！";
                     dbHelpers.SaveChanges();
                     return Json(new { status = "success", data = infoBuilder.ToString() },JsonRequestBehavior.AllowGet);                    
            }
            catch (Exception ex)
            {
               return Json(new { status = "error", data = "导入失败" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //写入查询历史
        public void  QueryLog(string selnum,DateTime seldate, string selresult) {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
             HttpCookie cookieuid = Request.Cookies["loginuid"];
             var uid = DESEncrypt.Decrypt(cookieuid.Value);
             var umodel = dbHelpers.Gy_Czygl.FirstOrDefault(db=>db.fid.ToString()==uid);
            QueryLog ql = new QueryLog();
            ql.seluser = umodel.czymc;
            ql.seldate = seldate;
            ql.selresult = selresult;
            ql.selnum = selnum;
            ql.type = "new";
            dbHelpers.QueryLog.Add(ql);
            dbHelpers.SaveChanges();            
        }
        /// <summary>
        /// 物流查询历史记录
        /// </summary>        
        [Manage]
        public ActionResult LogisticsQueryLog()
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

            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            string dateparme = "";
            if (!string.IsNullOrEmpty(startdate))
            {
                var sdate = DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00");
                var edate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                dateparme = "and seldate >= '" + sdate + "' and  seldate<='" + edate + "'";
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            ConnectionStringEntities db = new ConnectionStringEntities();
            PagesImpl pageImpl = new PagesImpl(db);
            PageinationInfo pageination = new PageinationInfo();
            pageination.TableName = "QueryLog";
            pageination.PageIndex = pageno;
            pageination.PageSize = pagesize;
            pageination.FieldName = "*";
            pageination.OrderCondition = "fid desc";
            pageination.WhereCondition = "1=1 "+dateparme+"";
            var data = pageImpl.GetPageinationData<QueryLog>(pageination);

            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }
        /// <summary>
        /// 导出查询记录
        /// </summary>                
        [Manage]
        public FileResult QueryLogToExcel()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
           
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            
            List<QueryLog> list = new List<QueryLog>();
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(startdate);
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                list = dbHelpers.QueryLog.Where(d => d.seldate >= sdate && d.seldate <= edate ).ToList();              
            }
            else
            {
                list = dbHelpers.QueryLog.OrderByDescending(db=>db.fid).ToList();
            }
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("查询用户");
            row1.CreateCell(1).SetCellValue("查询日期");
            row1.CreateCell(2).SetCellValue("查询数码");
            row1.CreateCell(3).SetCellValue("查询结果");
        

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].seluser);
                rowtemp.CreateCell(1).SetCellValue(list[i].seldate.ToString());
                rowtemp.CreateCell(2).SetCellValue(list[i].selnum);
                rowtemp.CreateCell(3).SetCellValue(list[i].selresult);
                
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "查询信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        #region//物流查询
        [Manage]
        /*public ActionResult LogisticsInfo() {
            var parme=Request["parme"];
            ConnectionStringEntities dbHelpers=new ConnectionStringEntities();
            var wlsm="";        //物流数码
            var cpmc = "";       //产品名称
            var cpgg = "";       // 产品规格
            var scxh = "";       //生产线号
            var bzxq = "";       //包装详情
            var ckqk = "";       //出库情况
            var thqk = "";       //退货情况          

            var model = dbHelpers.sal_packb.Where(db=>db.snno==parme || db.usnno==parme).ToList();
            StringBuilder selresult = new StringBuilder();
            if (model.Count ==1 )
            {
                var amodel=model.FirstOrDefault();
                wlsm="物流数码："+parme+"";
                var shipstatus = dbHelpers.sal_ship.Where(db=>db.bsnno==amodel.usnno || db.bsnno==amodel.snno);
                if (shipstatus.Count() == 0)
                {
                    ViewBag.shipstatus = "0";     //未出库
                    ckqk = "产品未出库";
                }
                else 
                {
                    var ship=shipstatus.FirstOrDefault();
                    ViewBag.shipstatus = "1";    //已出库
                    ViewBag.ship = ship;

                    ckqk = "经销商:"+ship.cu_name+",产品名:"+ship.pname+",出库日期:"+ship.ship_date+",出库数量："+ship.mqty;
                }
                var re = dbHelpers.sal_re.Where(db => db.bsnno == parme);
                if (re.Count()>0)
                {
                    ViewBag.shipstatus = "2";  //已退货
                    thqk = "产品已退货";
                }
                var packamodel = dbHelpers.sal_packa.FirstOrDefault(db => db.pack_no == amodel.pack_no);
                var promodel = dbHelpers.Inv_Part.FirstOrDefault(db => db.p_no == packamodel.p_no);
                bzxq = "大标数码：" + amodel.usnno;
                if (promodel != null)
                {
                    cpmc = "产品名称："+promodel.pname;
                    cpgg = "产品规格：" + promodel.pqty;
                }
                scxh="生产线号："+packamodel.line_no;

                selresult.Append(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",wlsm,cpmc,cpgg,scxh,bzxq,ckqk,thqk));
                QueryLog(parme,DateTime.Now,selresult.ToString());

                ViewBag.promodel = promodel;
                ViewBag.packamodel = packamodel;
                ViewBag.showtype = "xb";
                ViewBag.parme = parme;
                ViewBag.amodel =amodel;                
                return View();
            }
            if (model.Count > 1)
            {

                ViewBag.xbcount = model.Count;
                ViewBag.xbmodel = model;

                wlsm = "物流数码：" + parme + "";

                var shipstatus = dbHelpers.sal_ship.Where(db => db.bsnno == parme);
                if (shipstatus.Count() == 0)
                {
                    ViewBag.shipstatus = "0"; //未出库
                    ckqk = "产品未出库";
                }
                else
                {
                    var ship = shipstatus.FirstOrDefault();
                    ViewBag.shipstatus = "1"; //已出库
                    ViewBag.ship = ship;
                    ckqk = "经销商:" + ship.cu_name + ",产品名:" + ship.pname + ",出库日期:" + ship.ship_date + ",出库数量：" + ship.mqty;
                }
                var re = dbHelpers.sal_re.Where(db => db.bsnno==parme);
                if (re.Count() > 0)
                {
                    ViewBag.shipstatus = "2";  //已退货
                    thqk = "产品已退货";
                }
                var fmodel = model.FirstOrDefault();
                var packamodel = dbHelpers.sal_packa.FirstOrDefault(db => db.pack_no == fmodel.pack_no);
                var promodel = dbHelpers.Inv_Part.FirstOrDefault(db => db.p_no == packamodel.p_no);

                bzxq = "大标数码：" + parme;
                var xb = "小标数码:";
                foreach (var xbsm in model)
                {
                    xb += xbsm.snno+"|";
                }

                if (promodel != null)
                {
                    cpmc = "产品名称：" + promodel.pname;
                    cpgg = "产品规格：" + promodel.pqty;
                }
                scxh = "生产线号：" + packamodel.line_no;

                selresult.Append(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", wlsm, cpmc, cpgg, scxh, bzxq,xb, ckqk, thqk));
                QueryLog(parme, DateTime.Now, selresult.ToString());

                ViewBag.promodel = promodel;
                ViewBag.packamodel = packamodel;
                ViewBag.parme =  parme;
                ViewBag.showtype = "db";
                return View();
            }
            else {
                if(!string.IsNullOrEmpty(parme))
                   ViewBag.message = "未查询到有关输入的信息";
                return View();
            }
            
        }*/
        #endregion

     

        public string getreinfo(string code, string xtcu_no)
        {
            ConnectionStringEntities dbHelpers=new ConnectionStringEntities();
            List<sal_cure> re = dbHelpers.sal_cure.Where(db=>db.bsnno==code && db.xtcu_no==xtcu_no && db.upyn!="S").ToList();
            if (re.Count>0)
                return re.FirstOrDefault().ship_date+"退货！";
            return "";
        }

        public class logisinfo
        {
           public string  cu_name {get;set;}
           public string  xtcu_name{get;set;}
           public  DateTime?   ship_date{get;set;}
           public string remark { get; set; }

        }

        public ActionResult LogisticsInfo()
        {
            var parme = Request["parme"];
            ViewBag.parme = parme;
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var wlsm = "";        //物流数码
            var cpmc = "";       //产品名称
            var cpgg = "";       // 产品规格
            var scxh = "";       //生产线号
            var bzxq = "";       //包装详情
            var ckqk = "";       //出库情况
            var thqk = "";       //退货情况    

            var fgi = "";       //入库情况  
            var cuckqk = "";
            var xb = "";
            var dab = parme;

            int pqty = 0;

            brBarcode brcd = new brBarcode();


            Barcode barcode = brcd.GetBarcodeEntity(parme, "");
            if (barcode != null)
            {
                pqty = barcode.Qty;
            }
            else
            {

                   if(!string.IsNullOrEmpty(parme))
                       ViewBag.message = "数码不正确！";
                    return View();


                //barcode = new Barcode()
                //{
                //    Code = parme,
                //    Ucode = "",
                //    Tcode = "",
                //    Qty = 1


                //};
                //pqty = 1;

               

            } 



            if (!string.IsNullOrEmpty(parme))
            {

                var model = dbHelpers.sal_fgi.FirstOrDefault(db => db.bsnno == barcode.Code || db.bsnno == barcode.Ucode ||  db.bsnno == barcode.Tcode );

                StringBuilder selresult = new StringBuilder();
             
                
                if (model !=null )
                {
                 
                   
                    ViewBag.xbmodel = model;                    
        

                    wlsm = "物流数码：" + parme + "";


                    fgi = "入库批号:" + model.lot_no + ",入库日期:" + model.ship_date + ",入库单号：" + model.ship_no;

                    
                    var shippno="";
                    var shipstatus = dbHelpers.sal_ship.Where(db => db.bsnno == barcode.Code || db.bsnno == barcode.Ucode || db.bsnno == barcode.Tcode);
                    int sstatus = 0;
                    if (shipstatus.Count() == 0)
                    {
                        sstatus = 0;
                        ViewBag.shipstatus = "0"; //未出库
                        ckqk = "产品未出库";
                    }
                    else
                    {
                        sstatus = 1;
                        var ship = shipstatus.FirstOrDefault();
                        ViewBag.shipstatus = "1"; //已出库
                        ViewBag.ship = ship;
                        shippno=ship.p_no;
                        ckqk = "经销商:" + ship.cu_name + ",产品名:" + ship.pname + ",出库日期:" + ship.ship_date + ",出库单号：" + ship.ship_no;
                    }
                    var re = dbHelpers.sal_re.Where(db => (db.bsnno == barcode.Code || db.bsnno == barcode.Ucode || db.bsnno == barcode.Tcode));
                    if (re.Count() > 0)
                    {
                        var fre = re.FirstOrDefault();
                        sstatus = 0;
                        ViewBag.shipstatus = "2";  //已退货
                        ViewBag.re_no = fre.ship_no;
                        var restatus = "退回总部";
                        if (fre.upyn == "V")
                            restatus = "退回供应商";
                        ViewBag.restatus=restatus;
                        ViewBag.redate = fre.ship_date;
                        thqk = "产品已退货-退货单号：" + fre.ship_no;
                    }
                    if (sstatus == 1)
                    {                       
                        var cuship = dbHelpers.sal_cuship.FirstOrDefault(db => db.bsnno == barcode.Code || db.bsnno == barcode.Ucode || db.bsnno == barcode.Tcode);
                        if (cuship!=null)
                        {
                            cuckqk += "门店售出情况：";

                            cuckqk += "门店" + cuship.xtcu_name + "卖出，出售日期-" + cuship.ship_date + "|";
                               
                            ViewBag.cushipstatus = 1;                            
                            ViewBag.cuship = cuship;
                        }
                    }
               
                    bzxq = "大标数码：" + dab;
                    
                    ViewBag.db = dab;
                    
  
                    var promodel = dbHelpers.Inv_Part.FirstOrDefault(db => db.p_no == model.p_no);
                    if (promodel != null)
                    {
                        cpmc = "产品名称：" + promodel.pname;
                        cpgg = "产品规格：" + promodel.pqty;
                    }
                
                    ViewBag.promodel = promodel;


                    selresult.Append(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}{9}", wlsm, cpmc, cpgg, scxh, bzxq, xb, ckqk, thqk, cuckqk, fgi));
                 
                   QueryLog(parme, DateTime.Now, selresult.ToString());


                    ViewBag.showtype = "db";
                    return View();

                }
                else
                {
                    if (!string.IsNullOrEmpty(parme))
                        ViewBag.message = "条码未入库";
                    return View();
                }
            }
            else {
              
                return View();
            }           
        }

        public class pdatacount {
            public string pno { get; set; }
            public string cno { get; set; }
            public string xtcno { get; set; }
            public string pname { get; set; }
            public string cname { get; set; }
            public string xtcname { get; set; }
            public int? pcount { get; set; }
            public int acount  { get; set; }
            public DateTime? date { get; set; }
            public string usnno { get; set; }
            public string pack_no { get; set; }
            public string ship_no { get; set; }
            public string sum { get; set; }
        }

        public string getname(string corp,string parme) {
            ConnectionStringEntities db = new ConnectionStringEntities();
            if (corp == "c") { 
            var cmodel = db.customer.FirstOrDefault(d=>d.cu_no==parme);
            if (cmodel == null)
                return"";
            return cmodel.cu_name;
            }else if (corp == "p") { 
            var cmodel = db.Inv_Part.FirstOrDefault(d=>d.p_no==parme);
            if (cmodel == null)
                return"";
            return cmodel.pname;
            }
            return "";
        }
        public class prod {
            public string pno { get; set; }
            public string pname { get; set; }
        }

        [HttpPost]
        public ActionResult SelProd() {
            var pname=Request["pname"];
            ConnectionStringEntities db = new ConnectionStringEntities();
            var prod = db.Inv_Part.Where(d => d.p_no.Contains(pname) || d.pname.Contains(pname)).Select(item => new prod
            {
            pno=item.p_no,
            pname=item.pname
            }).Take(8);

            return Json(prod);
        }

        [HttpPost]
        public ActionResult SelCus()
        {
            var cname = Request["cname"];
            ConnectionStringEntities db = new ConnectionStringEntities();
            var cus = db.customer.Where(d => d.cu_no.Contains(cname) || d.cu_name.Contains(cname)).Select(item => new prod
            {
                pno = item.cu_no,
                pname = item.cu_name
            }).Take(8);

            return Json(cus);
        }

        [HttpPost]
        public ActionResult SelXtCus()
        {
            var cname = Request["cname"];
            var logincno = Request["logincno"];
            ConnectionStringEntities db = new ConnectionStringEntities();
            List<prod> cus = new List<prod>();
            if (string.IsNullOrEmpty(logincno))
            {
                cus = db.customer.Where(d => d.cu_no.Contains(cname) || d.cu_name.Contains(cname)).Select(item => new prod
                {
                    pno = item.cu_no,
                    pname = item.cu_name
                }).Take(8).ToList();
            }
            else
            {
                cus = db.customer.Where(d => d.cu_no.Contains(cname) && d.xtcu_no==logincno || d.cu_name.Contains(cname) && d.xtcu_no==logincno).Select(item => new prod
                {
                    pno = item.cu_no,
                    pname = item.cu_name
                }).Take(8).ToList();
            }
            return Json(cus);
        }
        /// <summary>
        /// 包装，出库，退货数据统计
        /// </summary>        
       [Manage]
        public ActionResult DataCount() {
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
            
            var pno=Request["pno"];
            var cno=Request["cno"]; 

            var output=Request["output"];
            string type = "P";
            if(!string.IsNullOrEmpty(Request["type"]))
                type=Request["type"];

            string parme = "";            
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];      
            ConnectionStringEntities db = new ConnectionStringEntities();           
            if (type == "P")
            {
                IQueryable<sal_packb> sship;
                if (!string.IsNullOrEmpty(startdate))
                {
                    DateTime sdate = DateTime.Parse(startdate);
                    DateTime edate = DateTime.Parse(enddate + "  23:59:59");
                    sship = db.sal_packb.Where(x => x.in_date >= sdate && x.in_date <= edate);
                    parme += "&startdate=" + startdate + "&enddate=" + enddate;
                }
                else
                {
                    sship = db.sal_packb.OrderByDescending(x => x.fid);
                }

                if (!string.IsNullOrEmpty(pno))
                {
                    sship = sship.Where(x => x.p_no == pno);
                    parme += "&pno=" + pno;
                }

                var sship2 = sship.ToList().Select(item => new sal_packb
                {                    
                    p_no = item.p_no,                   
                    in_date = DateTime.Parse(DateTime.Parse(item.in_date.ToString()).ToString("yyyy-MM-dd")),                    
                });

                var ship = sship2.GroupBy(x => new { x.p_no, x.in_date })
                    .Select(group => new pdatacount
                    {
                        date = group.Key.in_date,                        
                        pno = group.Key.p_no,                        
                        acount = group.Count()
                    }).OrderByDescending(d => d.date).ToList();


                ViewBag.sorcount = ship.Sum(d => d.acount);
                if (output == "T")
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
                    
                    row1.CreateCell(3).SetCellValue("数量");
                    row1.CreateCell(4).SetCellValue("时间");

                    //将数据逐步写入sheet1各个行
                    for (int b = 0; b < ship.Count; b++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(b + 1);
                        rowtemp.CreateCell(0).SetCellValue(ship[b].pno);
                        rowtemp.CreateCell(1).SetCellValue(DataEnum.GetPName(ship[b].pno));
                        rowtemp.CreateCell(2).SetCellValue(DataEnum.GetPType(ship[b].pno));  
                        rowtemp.CreateCell(3).SetCellValue(ship[b].acount);
                        rowtemp.CreateCell(4).SetCellValue(ship[b].date.ToString());
                    }
                    // 写入到客户端 
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    book.Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, "application/vnd.ms-excel", "包装入库数据统计" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                }
                ViewBag.list = ship.Skip((pageno - 1) * pagesize).Take(pagesize);
                ViewBag.count = ship.Count;
                ViewBag.pageno = pageno;
                ViewBag.pagecount = (int)Math.Ceiling((decimal)ship.Count / pagesize);
            } if (type == "F")
            {
                IQueryable<sal_fgi> sship;
                if (!string.IsNullOrEmpty(startdate))
                {
                    DateTime sdate = DateTime.Parse(startdate);
                    DateTime edate = DateTime.Parse(enddate + "  23:59:59");
                    sship = db.sal_fgi.Where(x => x.ship_date >= sdate && x.ship_date <= edate);
                    parme += "&startdate=" + startdate + "&enddate=" + enddate;
                }
                else
                {
                    sship = db.sal_fgi.OrderByDescending(x => x.fid);
                }

                if (!string.IsNullOrEmpty(pno))
                {
                    sship = sship.Where(x => x.p_no == pno);
                    parme += "&pno=" + pno;
                }               

                var sship2 = sship.ToList().Select(item => new sal_fgi
                {
                    fid = item.fid,
                    p_no = item.p_no,
                    pname = item.pname,
                    cu_no = item.cu_no,
                    cu_name = item.cu_name,
                    ship_date = DateTime.Parse(DateTime.Parse(item.ship_date.ToString()).ToString("yyyy-MM-dd")),
                    ship_no = item.ship_no,
                    mqty = item.mqty,
                });

                var ship = sship2.GroupBy(x => new { x.pname, x.cu_name, x.ship_date,x.p_no})
                    .Select(group => new pdatacount
                    {
                        date = group.Key.ship_date,
                        pno=group.Key.p_no,
                        //ship_no=group.Key.ship_no,
                        pname = group.Key.pname,
                        cname = group.Key.cu_name,
                        pcount = group.Sum(g => g.mqty),
                        acount = group.Count()
                    }).OrderByDescending(d => d.date).ToList();


                ViewBag.sorcount = ship.Sum(d => d.pcount);
                if (output == "T")
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

                    row1.CreateCell(3).SetCellValue("供应商");
                    row1.CreateCell(4).SetCellValue("件数");
                    row1.CreateCell(5).SetCellValue("时间");

                    //将数据逐步写入sheet1各个行
                    for (int b = 0; b < ship.Count; b++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(b + 1);
                        rowtemp.CreateCell(0).SetCellValue(ship[b].pno);
                        rowtemp.CreateCell(1).SetCellValue(ship[b].pname);
                        rowtemp.CreateCell(2).SetCellValue(DataEnum.GetPType(ship[b].pno)); 

                        rowtemp.CreateCell(3).SetCellValue(ship[b].cname);
                        rowtemp.CreateCell(4).SetCellValue(ship[b].acount);
                        rowtemp.CreateCell(5).SetCellValue(ship[b].date.ToString());
                    }
                    // 写入到客户端 
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    book.Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, "application/vnd.ms-excel", "入库数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                }
                ViewBag.list = ship.Skip((pageno - 1) * pagesize).Take(pagesize);
                ViewBag.count = ship.Count;
                ViewBag.pageno = pageno;
                ViewBag.pagecount = (int)Math.Ceiling((decimal)ship.Count / pagesize);
            }
            if (type == "S")
            {
                IQueryable<sal_ship> sship;
                if (!string.IsNullOrEmpty(startdate))
                {
                    DateTime sdate = DateTime.Parse(startdate);
                    DateTime edate = DateTime.Parse(enddate + "  23:59:59");
                    sship = db.sal_ship.Where(x => x.ship_date >= sdate && x.ship_date <= edate);
                    parme += "&startdate=" + startdate + "&enddate=" + enddate;
                }
                else
                {
                    sship = db.sal_ship.OrderByDescending(x => x.fid);                
                }

                if (!string.IsNullOrEmpty(pno)) {
                    sship = sship.Where(x=>x.p_no==pno);
                    parme += "&pno=" + pno;
                }

                if (!string.IsNullOrEmpty(cno))
                {
                    sship = sship.Where(x => x.cu_no == cno);
                    parme += "&cno=" + cno;
                }

                var sship2 = sship.ToList().Select(item => new sal_ship
                {
                    fid=item.fid,
                    p_no=item.p_no,
                    pname=item.pname,
                    cu_no=item.cu_no,
                    cu_name=item.cu_name,
                    ship_date=DateTime.Parse(DateTime.Parse(item.ship_date.ToString()).ToString("yyyy-MM-dd")),
                    ship_no=item.ship_no,
                    mqty=item.mqty,                    
                });

                var ship = sship2.GroupBy(x => new { x.pname, x.cu_name, x.ship_date,x.p_no })
                    .Select(group => new pdatacount
                    {
                        date=group.Key.ship_date,
                        pno=group.Key.p_no,
                        //ship_no=group.Key.ship_no,
                        pname = group.Key.pname,
                        cname = group.Key.cu_name,
                        pcount=group.Sum(g=>g.mqty),
                        acount = group.Count()
                    }).OrderByDescending(d => d.date).ToList();

                
                ViewBag.sorcount = ship.Sum(d=>d.pcount);
                if (output == "T")
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

                    row1.CreateCell(3).SetCellValue("经销商");
                    row1.CreateCell(4).SetCellValue("件数");                   
                    row1.CreateCell(5).SetCellValue("时间");

                    //将数据逐步写入sheet1各个行
                    for (int b = 0; b < ship.Count; b++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(b + 1);
                        rowtemp.CreateCell(0).SetCellValue(ship[b].pno);
                        rowtemp.CreateCell(1).SetCellValue(ship[b].pname);
                        rowtemp.CreateCell(2).SetCellValue(DataEnum.GetPType(ship[b].pno)); 

                        rowtemp.CreateCell(3).SetCellValue(ship[b].cname);                 
                        rowtemp.CreateCell(4).SetCellValue(ship[b].acount);
                        rowtemp.CreateCell(5).SetCellValue(ship[b].date.ToString());
                    }
                    // 写入到客户端 
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    book.Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, "application/vnd.ms-excel", "出库数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                }
                ViewBag.list = ship.Skip((pageno - 1) * pagesize).Take(pagesize);
                ViewBag.count = ship.Count;
                ViewBag.pageno = pageno;
                ViewBag.pagecount = (int)Math.Ceiling((decimal)ship.Count / pagesize);
            }
            if (type == "R")
            {
                IQueryable<sal_re> sre;
                if (!string.IsNullOrEmpty(startdate))
                {
                    DateTime sdate = DateTime.Parse(startdate);
                    DateTime edate = DateTime.Parse(enddate + "  23:59:59");
                    sre = db.sal_re.Where(x => x.ship_date >= sdate && x.ship_date <= edate);
                    parme += "&startdate=" + startdate + "&enddate=" + enddate;
                }
                else
                {
                    sre = db.sal_re.OrderByDescending(x => x.fid);
                }

                if (!string.IsNullOrEmpty(pno)) {
                    sre = sre.Where(x=>x.p_no==pno);
                    parme += "&pno="+pno;
                }
                if (!string.IsNullOrEmpty(cno))
                {
                    sre = sre.Where(x => x.cu_no == cno);
                    parme += "&cno=" + cno;
                }

                var sre2 = sre.ToList().Select(item => new sal_re
                {
                    fid = item.fid,
                    p_no = item.p_no,
                    pname = item.pname,
                    cu_no = item.cu_no,
                    cu_name = item.cu_name,
                    ship_date = DateTime.Parse(DateTime.Parse(item.ship_date.ToString()).ToString("yyyy-MM-dd")),
                    ship_no = item.ship_no,
                    mqty = item.mqty,                    
                });

                var re = sre.GroupBy(x => new { x.pname, x.cu_name, x.ship_date,x.p_no })
                    .Select(group => new pdatacount
                    {
                        pno=group.Key.p_no,
                        date = group.Key.ship_date,                        
                        pname = group.Key.pname,
                        cname = group.Key.cu_name,
                        pcount=group.Sum(g=>g.mqty),
                        acount = group.Count()
                    }).OrderByDescending(d => d.date).ToList();

               
                ViewBag.sorcount = re.Sum(d => d.pcount);
                
                if (output == "T")
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

                    row1.CreateCell(3).SetCellValue("经销商");
                    row1.CreateCell(4).SetCellValue("件数");                    
                    row1.CreateCell(5).SetCellValue("时间");

                    //将数据逐步写入sheet1各个行
                    for (int b = 0; b < re.Count; b++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(b + 1);
                        rowtemp.CreateCell(0).SetCellValue(re[b].pno);
                        rowtemp.CreateCell(1).SetCellValue(re[b].pname);
                        rowtemp.CreateCell(2).SetCellValue(DataEnum.GetPType(re[b].pno)); 

                        rowtemp.CreateCell(3).SetCellValue( re[b].cname);                    
                        rowtemp.CreateCell(4).SetCellValue(re[b].acount);
                        rowtemp.CreateCell(5).SetCellValue(re[b].date.ToString());
                    }
                    // 写入到客户端 
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    book.Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, "application/vnd.ms-excel", "退货数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                }
                ViewBag.list = re.Skip((pageno - 1) * pagesize).Take(pagesize);
                ViewBag.count = re.Count;
                ViewBag.pageno = pageno;
                ViewBag.pagecount = (int)Math.Ceiling((decimal)re.Count / pagesize);
            }
            ViewBag.type = type;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

       /// <summary>
       /// 经销商入库明细
       /// </summary>        
       //[Manage]       
       //public ActionResult CuFgiList()
       //{
       //    ConnectionStringEntities db = new ConnectionStringEntities();
       //    HttpCookie cookieuid = Request.Cookies["loginuid"];
       //    var uid = DESEncrypt.Decrypt(cookieuid.Value);
       //    var cmodel = db.customer.FirstOrDefault(d => d.fid.ToString() == uid);
       //    int pageno = 1;
       //    if (!string.IsNullOrEmpty(Request["pageno"]))
       //    {
       //        pageno = int.Parse(Request["pageno"]);
       //    }
       //    int pagesize = 10;
       //    if (!string.IsNullOrEmpty(Request["pagesize"]))
       //    {
       //        pagesize = int.Parse(Request["pagesize"]);
       //    }
       //    var logintype = Request["logintype"];
       //    string parme = "";
       //    var cufginame = Request["cufginame"];
       //    if (!string.IsNullOrEmpty(cufginame))
       //        parme += "cufginame=" + cufginame;

       //    var startdate = Request["startdate"];
       //    var enddate = Request["enddate"];
       //    string dateparme = "and 1=1";
       //    if (!string.IsNullOrEmpty(startdate))
       //    {
       //        DateTime sdate = DateTime.Parse(startdate);
       //        DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
       //        dateparme = "and ship_date between '" + sdate + "' and '" + edate + "'";
       //        parme += "&startdate=" + startdate + "&enddate=" + enddate;
       //    }

       //    PagesImpl pageImpl = new PagesImpl(db);
       //    PageinationInfo pageination = new PageinationInfo();
       //    pageination.TableName = "sal_cufgi";
       //    pageination.PageIndex = pageno;
       //    pageination.PageSize = pagesize;
       //    pageination.FieldName = "*";
       //    pageination.OrderCondition = "fid desc";
       //    if (logintype == "u")
       //    {
       //        pageination.WhereCondition = "ship_no like '%" + cufginame + "%' " + dateparme + "";
       //    }
       //    else if (logintype == "c")
       //    {
       //        pageination.WhereCondition = "ship_no like '%" + cufginame + "%' and  xtcu_no='" + cmodel.cu_no + "' " + dateparme + "";
       //    }
       //    var data = pageImpl.GetPageinationData<sal_cufgi>(pageination);

       //    ViewBag.logintype = logintype;
       //    ViewBag.list = data.DataList;
       //    ViewBag.count = data.TotalCount;
       //    ViewBag.pageno = pageno;
       //    ViewBag.pagecount = data.TotalPages;
       //    ViewBag.pagesize = pagesize;
       //    ViewBag.parme = parme;
       //    return View();
       //}

       public ActionResult CuFgiList()
       {
          

           int pageno = 1;
           if (!string.IsNullOrEmpty(Request["pageno"]))
               pageno = int.Parse(Request["pageno"]);

           int pagesize = 10;
           if (!string.IsNullOrEmpty(Request["pagesize"]))
               pagesize = int.Parse(Request["pagesize"]);

           string parme = "";
           var fginame = Request["cufginame"];
           if (!string.IsNullOrEmpty(fginame))
               parme += "cufginame=" + fginame;

           var startdate = Request["startdate"];
           var enddate = Request["enddate"];

           ConnectionStringEntities db = new ConnectionStringEntities();

           HttpCookie cookieuid = Request.Cookies["loginuid"];
           var uid = DESEncrypt.Decrypt(cookieuid.Value);
           var cmodel = db.customer.FirstOrDefault(d => d.fid.ToString() == uid);
           var logintype = Request["logintype"];

           IQueryable<sal_cufgi> lspb = db.sal_cufgi;


           if (logintype == "c")
               lspb = lspb.Where(x => x.xtcu_no == cmodel.cu_no);

           if (!string.IsNullOrEmpty(startdate))
           {
               DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
               DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
               lspb = lspb.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
               parme += "&startdate=" + startdate + "&enddate=" + enddate;
           }

           if (!string.IsNullOrEmpty(fginame))
               lspb = lspb.Where(d => d.ship_no == fginame);

           var lspb2 = lspb.ToList().Select(l => new sal_cufgi
           {
               ship_no = l.ship_no,               
               ship_date = l.ship_date != null ? DateTime.Parse(DateTime.Parse(l.ship_date.ToString()).ToString("yyyy-MM-dd")) : DateTime.Now,              
               mqty = l.mqty,
               p_no = l.p_no,
               cu_no=l.cu_no
           });

           var sf = lspb2.GroupBy(x => new { x.ship_no,  x.ship_date,  x.p_no,x.cu_no }).ToList().Select(s => new sal_cufgi
           {
               ship_no = s.Key.ship_no,              
               ship_date = s.Key.ship_date,              
               p_no = s.Key.p_no,
               cu_no=s.Key.cu_no,
               mqty = s.Count()
           });

           var count = sf.Count();
           var slist = sf.OrderByDescending(d => d.ship_date).Skip((pageno - 1) * pagesize).Take(pagesize);
           ViewBag.zj = slist.Sum(d => d.mqty);
           ViewBag.list = slist.ToList();
           ViewBag.count = count;
           ViewBag.pageno = pageno;
           ViewBag.pagecount = (int)Math.Ceiling((decimal)count / pagesize);
           ViewBag.pagesize = pagesize;
           ViewBag.parme = parme;
           return View();
       }

       public ActionResult SeeCuFgiInfo()
       {
           ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
           var shipno = Request["shipno"];           
           var pno = Request["pno"];
           var cno = Request["cno"];
           var dc = Request["dc"];

           var fgi = dbHelpers.sal_cufgi.Where(db => db.ship_no == shipno);

           if (!string.IsNullOrEmpty(pno))
               fgi = fgi.Where(db => db.p_no == pno);
           else
               fgi = fgi.Where(db => db.p_no == "" || db.p_no == null);

           if (!string.IsNullOrEmpty(cno))
               fgi = fgi.Where(db => db.cu_no == cno);
           else
               fgi = fgi.Where(db => db.st_no == null || db.st_no == "");           

           if (dc == "ok")
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
               row1.CreateCell(5).SetCellValue("经销商编号");
               row1.CreateCell(6).SetCellValue("经销商名称");
               row1.CreateCell(7).SetCellValue("批号");
               row1.CreateCell(8).SetCellValue("数量");

               //将数据逐步写入sheet1各个行
               for (int i = 0; i < list.Count(); i++)
               {
                   NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                   rowtemp.CreateCell(0).SetCellValue(list[i].ship_no);
                   rowtemp.CreateCell(1).SetCellValue(list[i].bsnno);
                   rowtemp.CreateCell(2).SetCellValue(list[i].ship_date.ToString());
                   rowtemp.CreateCell(3).SetCellValue(list[i].p_no);
                   rowtemp.CreateCell(4).SetCellValue(list[i].pname);
                   rowtemp.CreateCell(5).SetCellValue(list[i].cu_no);
                   rowtemp.CreateCell(6).SetCellValue(list[i].cu_name);
                   rowtemp.CreateCell(7).SetCellValue(list[i].lot_no);
                   rowtemp.CreateCell(8).SetCellValue(list[i].mqty.ToString());
               }
               // 写入到客户端 
               System.IO.MemoryStream ms = new System.IO.MemoryStream();
               book.Write(ms);
               ms.Seek(0, SeekOrigin.Begin);
               return File(ms, "application/vnd.ms-excel", shipno + "经销商入库数据.xls");
           }
           ViewBag.fgi = fgi;
           ViewBag.parme = "shipno=" + shipno + "&cno=" + cno + "&pno=" + pno;
           return View();
       }

        /// <summary>
        /// 经销商出货明细
        /// </summary>        
       //[Manage]
       //public ActionResult CuShipList()
       //{
       //    ConnectionStringEntities db = new ConnectionStringEntities();
       //    HttpCookie cookieuid = Request.Cookies["loginuid"];
       //    var uid = DESEncrypt.Decrypt(cookieuid.Value);
       //    var cmodel = db.customer.FirstOrDefault(d => d.fid.ToString() == uid);
       //    int pageno = 1;
       //    if (!string.IsNullOrEmpty(Request["pageno"]))
       //    {
       //        pageno = int.Parse(Request["pageno"]);
       //    }
       //    int pagesize = 10;
       //    if (!string.IsNullOrEmpty(Request["pagesize"]))
       //    {
       //        pagesize = int.Parse(Request["pagesize"]);
       //    }
       //    var logintype=Request["logintype"];
       //    string parme = "";
       //    var cushipname = Request["cushipname"];
       //    if (!string.IsNullOrEmpty(cushipname))
       //        parme += "cushipname=" + cushipname;
       //
       //    var startdate = Request["startdate"];
       //    var enddate = Request["enddate"];
       //    string dateparme = "and 1=1";
       //    if (!string.IsNullOrEmpty(startdate))
       //    {
       //        DateTime sdate = DateTime.Parse(startdate);
       //        DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
       //        dateparme = "and ship_date between '" + sdate + "' and '" + edate + "'";
       //        parme += "&startdate=" + startdate + "&enddate=" + enddate;
       //    }
       //   
       //    PagesImpl pageImpl = new PagesImpl(db);
       //    PageinationInfo pageination = new PageinationInfo();
       //    pageination.TableName = "sal_cuship";
       //    pageination.PageIndex = pageno;
       //    pageination.PageSize = pagesize;
       //    pageination.FieldName = "*";
       //    pageination.OrderCondition = "fid desc";
       //    if (logintype == "u")
       //    {
       //        pageination.WhereCondition = "ship_no like '%" + cushipname + "%' " + dateparme + "";
       //    }
       //    else if(logintype=="c"){
       //        pageination.WhereCondition = "ship_no like '%" + cushipname + "%' and  xtcu_no='"+cmodel.cu_no+"' " + dateparme + "";
       //    }
       //    var data = pageImpl.GetPageinationData<sal_cuship>(pageination);
       //
       //    ViewBag.logintype = logintype;
       //    ViewBag.list = data.DataList;
       //    ViewBag.count = data.TotalCount;
       //    ViewBag.pageno = pageno;
       //    ViewBag.pagecount = data.TotalPages;
       //    ViewBag.pagesize = pagesize;
       //    ViewBag.parme = parme;
       //    return View();
       //}

       public ActionResult CuShipList()
       {

           int pageno = 1;
           if (!string.IsNullOrEmpty(Request["pageno"]))
               pageno = int.Parse(Request["pageno"]);

           int pagesize = 10;
           if (!string.IsNullOrEmpty(Request["pagesize"]))
               pagesize = int.Parse(Request["pagesize"]);

           string parme = "";
           var fginame = Request["cushipname"];
           if (!string.IsNullOrEmpty(fginame))
               parme += "cushipname=" + fginame;

           var startdate = Request["startdate"];
           var enddate = Request["enddate"];
           var cno = Request["cno"];
           ConnectionStringEntities db = new ConnectionStringEntities();

           HttpCookie cookieuid = Request.Cookies["loginuid"];
           var uid = DESEncrypt.Decrypt(cookieuid.Value);
           var cmodel = db.customer.FirstOrDefault(d => d.fid.ToString() == uid);
           var logintype = Request["logintype"];

           IQueryable<sal_cuship> lspb = db.sal_cuship;


           if (logintype == "c")
               lspb = lspb.Where(x => x.xtcu_no == cmodel.cu_no);

           if (!string.IsNullOrEmpty(startdate))
           {
               DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
               DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
               lspb = lspb.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
               parme += "&startdate=" + startdate + "&enddate=" + enddate;
           }

           if (!string.IsNullOrEmpty(fginame))
               lspb = lspb.Where(d => d.ship_no == fginame);

           if(!string.IsNullOrEmpty(cno))
           {
               lspb = lspb.Where(x => x.cu_no == cno);
               parme += "cno=" + cno;
           }


           var lspb2 = lspb.ToList().Select(l => new sal_cuship
           {
               ship_no = l.ship_no,
               ship_date = l.ship_date != null ? DateTime.Parse(DateTime.Parse(l.ship_date.ToString()).ToString("yyyy-MM-dd")) : DateTime.Now,
               mqty = l.mqty,
               p_no = l.p_no,
               
               xtcu_no=l.xtcu_no
           });

           var sf = lspb2.GroupBy(x => new { x.ship_no, x.ship_date, x.p_no,x.xtcu_no}).ToList().Select(s => new sal_cuship
           {
               ship_no = s.Key.ship_no,
               ship_date = s.Key.ship_date,
               p_no = s.Key.p_no,
              
               xtcu_no=s.Key.xtcu_no,
               mqty = s.Sum(x=>x.mqty)
           });

           var count = sf.Count();
           var slist = sf.OrderByDescending(d => d.ship_date).Skip((pageno - 1) * pagesize).Take(pagesize);
           ViewBag.zj = slist.Sum(d => d.mqty);
           ViewBag.list = slist.ToList();
           ViewBag.count = count;
           ViewBag.pageno = pageno;
           ViewBag.pagecount = (int)Math.Ceiling((decimal)count / pagesize);
           ViewBag.pagesize = pagesize;
           ViewBag.parme = parme;
           return View();
       }

       public ActionResult SeeCuShipInfo()
       {
           ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
           var shipno = Request["shipno"];
           var pno = Request["pno"];
           var cno = Request["cno"];
           var xtcno = Request["xtcno"];
           var dc = Request["dc"];

           var fgi = dbHelpers.sal_cuship.Where(db => db.ship_no == shipno);

           if (!string.IsNullOrEmpty(pno))
               fgi = fgi.Where(db => db.p_no == pno);
           else
               fgi = fgi.Where(db => db.p_no == "" || db.p_no == null);

           if (!string.IsNullOrEmpty(cno))
               fgi = fgi.Where(db => db.cu_no == cno);
           else
               fgi = fgi.Where(db => db.cu_no == null || db.cu_no == "");

           if (!string.IsNullOrEmpty(xtcno))
               fgi = fgi.Where(db => db.xtcu_no == xtcno);
           else
               fgi = fgi.Where(db => db.xtcu_no == null || db.xtcu_no == "");


           if (dc == "ok")
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
               row1.CreateCell(2).SetCellValue("出库日期");
               row1.CreateCell(3).SetCellValue("产品编号");
               row1.CreateCell(4).SetCellValue("产品名称");
               row1.CreateCell(5).SetCellValue("发货经销商编号");
               row1.CreateCell(6).SetCellValue("发货经销商名称");
               row1.CreateCell(7).SetCellValue("收货经销商编号");
               row1.CreateCell(8).SetCellValue("收货经销商名称");
               row1.CreateCell(9).SetCellValue("批号");
               row1.CreateCell(10).SetCellValue("数量");

               //将数据逐步写入sheet1各个行
               for (int i = 0; i < list.Count(); i++)
               {
                   NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                   rowtemp.CreateCell(0).SetCellValue(list[i].ship_no);
                   rowtemp.CreateCell(1).SetCellValue(list[i].bsnno);
                   rowtemp.CreateCell(2).SetCellValue(list[i].ship_date.ToString());
                   rowtemp.CreateCell(3).SetCellValue(list[i].p_no);
                   rowtemp.CreateCell(4).SetCellValue(list[i].pname);
                   rowtemp.CreateCell(5).SetCellValue(list[i].xtcu_no);
                   rowtemp.CreateCell(6).SetCellValue(list[i].xtcu_name);
                   rowtemp.CreateCell(7).SetCellValue(list[i].cu_no);
                   rowtemp.CreateCell(8).SetCellValue(list[i].cu_name);
                   rowtemp.CreateCell(9).SetCellValue(list[i].lot_no);
                   rowtemp.CreateCell(10).SetCellValue(list[i].mqty.ToString());
               }
               // 写入到客户端 
               System.IO.MemoryStream ms = new System.IO.MemoryStream();
               book.Write(ms);
               ms.Seek(0, SeekOrigin.Begin);
               return File(ms, "application/vnd.ms-excel", shipno + "经销商出库数据.xls");
           }
           ViewBag.fgi = fgi;
           ViewBag.parme = "shipno=" + shipno + "&cno=" + cno + "&pno=" + pno+"&xtcno="+xtcno;
           return View();
       }

        /// <summary>
        /// 经销商退货明细
        /// </summary>
        [Manage]
        public ActionResult CuReList()
        {
            ConnectionStringEntities db = new ConnectionStringEntities();
            HttpCookie cookieuid = Request.Cookies["loginuid"];
            var uid = DESEncrypt.Decrypt(cookieuid.Value);
            var cmodel = db.customer.FirstOrDefault(d => d.fid.ToString() == uid);

            var logintype=Request["logintype"];
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
            var rename = Request["rename"];
            if (!string.IsNullOrEmpty(rename))
                parme += "rename=" + rename;

            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
            string dateparme = "and 1=1";
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(startdate);
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                dateparme = "and ship_date between '" + sdate + "' and '" + edate + "'";
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
          
            PagesImpl pageImpl = new PagesImpl(db);
            PageinationInfo pageination = new PageinationInfo();
            pageination.TableName = "sal_cure";
            pageination.PageIndex = pageno;
            pageination.PageSize = pagesize;
            pageination.FieldName = "*";
            pageination.OrderCondition = "fid desc";
            if (logintype == "u")
            {
                pageination.WhereCondition = "ship_no like '%" + rename + "%' " + dateparme + "";
            }
            else if (logintype == "c") { 
                pageination.WhereCondition = "ship_no like '%" + rename + "%' and xtcu_no='"+cmodel.cu_no+"' " + dateparme + "";
            }
            var data = pageImpl.GetPageinationData<sal_cure>(pageination);
            ViewBag.logintype = logintype;
            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }
        public class packandupload { 
         public DateTime date{get;set;}
         public int?   packaunm{get;set;}
         public int? xtupnum { get; set; }

        }
        public ActionResult PackaUploadCount() {
            ConnectionStringEntities dbHelpers=new ConnectionStringEntities();
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
            List<xt_uplist> gclist = new List<xt_uplist>();
            var parme = "";
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];

            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(startdate);
                DateTime edate = DateTime.Parse(enddate + "  23:59:59");
                gclist = dbHelpers.xt_uplist.Where(x => x.in_date >= sdate && x.in_date <= edate).ToList();
                parme += "&startdate=" + sdate + "&enddate=" + edate;
            }
            else
            {
                gclist = dbHelpers.xt_uplist.OrderByDescending(x => x.fid).ToList();
            }

          var  gclist2 = gclist.Select(item => new xt_uplist
            {
                in_date=DateTime.Parse(DateTime.Parse(item.in_date.ToString()).ToString("yyyy-MM-dd")),
                mqty=item.mqty,
                status=item.status,
                fid=item.fid
            });
            
           var glist = gclist2.GroupBy(db => new { db.in_date }).Select(item => new packandupload
            {
               date=DateTime.Parse(item.Key.in_date.ToString()),
              // packaunm = getpackbcount(item.Key.in_date.ToString()),
               xtupnum=item.Sum(i=>i.mqty),
            }).Skip((pageno - 1) * pagesize).Take(pagesize);


           
            var count = glist.Count();
            var pagecount = (int)Math.Ceiling((decimal)count / pagesize);            
            ViewBag.list = glist;
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = pagecount;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();            
        }

        public int? getpackbcount(string dt) {

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var sdate =DateTime.Parse( DateTime.Parse(dt).ToString("yyyy-MM-dd 00:00:00"));
            var edate = DateTime.Parse(DateTime.Parse(dt).ToString("yyyy-MM-dd 23:59:59"));
            var packb = dbHelpers.sal_packa.Where(db => db.pack_date>=sdate && db.pack_date<=edate && db.type=="P");

            var packbcount = packb.Sum(db=>db.okqty);

            return packbcount;
        }

        public FileResult PackaUploadDataToExcel()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
           
            var startdate = Request["startdate"];
            var enddate = Request["enddate"];
    
            List<xt_uplist> list = new List<xt_uplist>();
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(startdate);
                DateTime edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                list = dbHelpers.xt_uplist.Where(d => d.in_date >= sdate && d.in_date <= edate ).ToList();              
            }
            else
            {
                list = dbHelpers.xt_uplist.OrderByDescending(db=>db.in_date).ToList();
            }
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("上传时间");
            row1.CreateCell(1).SetCellValue("数量");
            row1.CreateCell(2).SetCellValue("备注");
            row1.CreateCell(3).SetCellValue("文件路径");
        

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].up_date.ToString());
                rowtemp.CreateCell(1).SetCellValue(list[i].mqty.ToString());
                rowtemp.CreateCell(2).SetCellValue(list[i].remark);
                rowtemp.CreateCell(3).SetCellValue(list[i].filename);
                
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "包装入库上传激活统计" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

     
        public ActionResult CuDataList() {
                var output = Request["output"];
                var cno = Request["cno"];
                var xtcno=Request["xtcno"];   //下级经销商编号
                var pno = Request["pno"];
                var logincno=Request["logincno"];            
                ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
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

                string type = "CS";
                if (!string.IsNullOrEmpty(Request["type"]))
                    type = Request["type"];

                var parme = "";
                var startdate = Request["startdate"];
                var enddate = Request["enddate"];
            
            
             if (type == "CS") {

                 IQueryable<sal_cuship> sship;

                if (!string.IsNullOrEmpty(logincno))
                {
                    sship = dbHelpers.sal_cuship.Where(db=>db.xtcu_no==logincno);
                    ViewBag.logincno = logincno;
                }
                else
                {
                    sship = dbHelpers.sal_cuship.OrderByDescending(db=>db.fid);
                }

                if (!string.IsNullOrEmpty(startdate))
                {
                    DateTime sdate = DateTime.Parse(startdate);
                    DateTime edate = DateTime.Parse(enddate + "  23:59:59");
                    sship = sship.Where(x => x.ship_date >= sdate && x.ship_date <= edate);
                    parme += "&startdate=" + startdate + "&enddate=" + enddate;
                }
                
                if (!string.IsNullOrEmpty(pno))
                {
                    sship = sship.Where(b=> b.p_no.Trim() == pno);
                    parme += "&pno=" + pno;
                }
                if (!string.IsNullOrEmpty(cno))
                {
                    sship = sship.Where(x => x.xtcu_no == cno);
                    parme += "&cno=" + cno;
                }
                if (!string.IsNullOrEmpty(xtcno))
                {
                    sship = sship.Where(x => x.cu_no == xtcno);
                    parme += "&xtcno=" + xtcno;
                }

                var sship2 = sship.ToList().Select(item => new sal_cuship
                {
                    fid = item.fid,
                    p_no = item.p_no,
                    pname = item.pname,
                    
                    xtcu_no = item.xtcu_no,
                    xtcu_name = item.xtcu_name,
                    ship_date = DateTime.Parse(DateTime.Parse(item.ship_date.ToString()).ToString("yyyy-MM-dd")),
                    ship_no = item.ship_no,
                    mqty = item.mqty,
                });


                var ship = sship2.GroupBy(x => new { x.pname,x.p_no, x.xtcu_name,x.xtcu_no, x.ship_date })
                    .Select(group => new pdatacount
                    {
                        date = group.Key.ship_date,
                       // ship_no = group.Key.ship_no,
                        pname = group.Key.pname,
                        pno=group.Key.p_no,
                       
                        xtcname=group.Key.xtcu_name,
                        xtcno=group.Key.xtcu_no,
                        pcount = group.Sum(g => g.mqty),        
                        acount = group.Count()
                    }).ToList();

                var pship = ship.OrderByDescending(d => d.date).Skip((pageno - 1) * pagesize).Take(pagesize).ToList();

                if (output == "T")
                {                   
                    //创建Excel文件的对象
                    NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                    //添加一个sheet
                    NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

                    //给sheet1添加第一行的头部标题
                    NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                    row1.CreateCell(0).SetCellValue("产品编号");
                    row1.CreateCell(1).SetCellValue("产品名称");
                    row1.CreateCell(2).SetCellValue("经销商编号");
                    row1.CreateCell(3).SetCellValue("经销商");                   
                    row1.CreateCell(4).SetCellValue("件数");
                    row1.CreateCell(5).SetCellValue("总数");
                    row1.CreateCell(6).SetCellValue("日期");

                    //将数据逐步写入sheet1各个行
                    for (int b = 0; b < ship.Count; b++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(b + 1);
                        rowtemp.CreateCell(0).SetCellValue(ship[b].pno);
                        rowtemp.CreateCell(1).SetCellValue(ship[b].pname);
                        rowtemp.CreateCell(2).SetCellValue(ship[b].xtcno);
                        rowtemp.CreateCell(3).SetCellValue(ship[b].xtcname);                        
                        rowtemp.CreateCell(4).SetCellValue(ship[b].acount.ToString());
                        rowtemp.CreateCell(5).SetCellValue(ship[b].pcount.ToString());
                        rowtemp.CreateCell(6).SetCellValue(ship[b].date.ToString());
                    }
                    // 写入到客户端 
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    book.Write(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, "application/vnd.ms-excel", "门店出库数据统计" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                }

                ViewBag.sorcount = pship.Sum(d => d.pcount);  //件数总数               
                ViewBag.list = pship;
                ViewBag.count = ship.Count();
                ViewBag.pageno = pageno;
                ViewBag.pagecount = (int)Math.Ceiling((decimal)ship.Count() / pagesize);
               
             }else if (type == "CF")
             {
                 IQueryable<sal_ship> sre;

                 if (!string.IsNullOrEmpty(logincno))
                 {
                     sre = dbHelpers.sal_ship.Where(db => db.xtcu_no == logincno);
                     ViewBag.logincno = logincno;
                 }
                 else
                 {
                     sre = dbHelpers.sal_ship.OrderByDescending(db => db.fid);
                 }

                 if (!string.IsNullOrEmpty(startdate))
                 {
                     DateTime sdate = DateTime.Parse(startdate);
                     DateTime edate = DateTime.Parse(enddate + "  23:59:59");
                     sre = sre.Where(x => x.ship_date >= sdate && x.ship_date <= edate);
                     parme += "&startdate=" + startdate + "&enddate=" + enddate;
                 }                
            
                 if (!string.IsNullOrEmpty(pno))
                 {
                     sre = sre.Where(x => x.p_no == pno);
                     parme += "&pno=" + pno;
                 }
                 if (!string.IsNullOrEmpty(cno))
                 {
                     sre = sre.Where(x => x.xtcu_no == cno);
                     parme += "&cno=" + cno;
                 }
                 if (!string.IsNullOrEmpty(xtcno))
                 {
                     sre = sre.Where(x => x.cu_no == xtcno);
                     parme += "&xtcno=" + xtcno;
                 }

                 var sre2 = sre.ToList().Select(item => new sal_cure
                 {
                     fid = item.fid,
                     p_no = item.p_no,
                     pname = item.pname,
                    
                     cu_no=item.cu_no,
                     cu_name=item.cu_name,
                     ship_date = DateTime.Parse(DateTime.Parse(item.ship_date.ToString()).ToString("yyyy-MM-dd")),
                     ship_no = item.ship_no,
                     mqty = item.mqty,
                 });

                 var re = sre.GroupBy(x => new { x.pname, x.p_no,x.cu_name,x.cu_no, x.ship_date })
                     .Select(group => new pdatacount
                     {
                         date = group.Key.ship_date,                         
                         pno=group.Key.p_no,
                         pname= group.Key.pname,                        
                         xtcno=group.Key.cu_no,
                         xtcname=group.Key.cu_name,
                         pcount=group.Sum(g=>g.mqty),
                         acount = group.Count()
                     }).ToList();

                 var pre = re.OrderByDescending(d => d.date).Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
                 if (output == "T")
                 {
                     //创建Excel文件的对象
                     NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                     //添加一个sheet
                     NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

                     //给sheet1添加第一行的头部标题
                     NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                     row1.CreateCell(0).SetCellValue("产品编号");
                     row1.CreateCell(1).SetCellValue("产品名称");
                     row1.CreateCell(2).SetCellValue("门店编号");
                     row1.CreateCell(3).SetCellValue("门店");                     
                     row1.CreateCell(4).SetCellValue("件数");
                     row1.CreateCell(5).SetCellValue("总数");
                     row1.CreateCell(6).SetCellValue("日期");

                     //将数据逐步写入sheet1各个行
                     for (int b = 0; b < re.Count; b++)
                     {
                         NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(b + 1);
                         rowtemp.CreateCell(0).SetCellValue(re[b].pno);
                         rowtemp.CreateCell(1).SetCellValue(re[b].pname);
                         rowtemp.CreateCell(2).SetCellValue(re[b].xtcno);
                         rowtemp.CreateCell(3).SetCellValue(re[b].xtcname);                         
                         rowtemp.CreateCell(4).SetCellValue(re[b].acount.ToString());
                         rowtemp.CreateCell(5).SetCellValue(re[b].pcount.ToString());
                         rowtemp.CreateCell(6).SetCellValue(re[b].date.ToString());
                     }
                     // 写入到客户端 
                     System.IO.MemoryStream ms = new System.IO.MemoryStream();
                     book.Write(ms);
                     ms.Seek(0, SeekOrigin.Begin);
                     return File(ms, "application/vnd.ms-excel", "门店入库数据统计" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                 }
                 ViewBag.sorcount = pre.Sum(d => d.pcount);                                                      
                 ViewBag.list = pre;
                 ViewBag.count = re.Count();
                 ViewBag.pageno = pageno;
                 ViewBag.pagecount = (int)Math.Ceiling((decimal)re.Count() / pagesize);
             }
             ViewBag.pagesize = pagesize;
             ViewBag.parme = parme;
             ViewBag.type = type;
            return View();   
        }


        public ActionResult PdaList()
        {
            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request["pagesize"]))
                pagesize = int.Parse(Request["pagesize"]);
            int pageno = 1;
            if (!string.IsNullOrEmpty(Request["pageno"]))
                pageno  = int.Parse(Request["pageno"]);

            var logincno = Request["logincno"];
            ViewBag.logincno = logincno;

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            
            var unameoruid = Request["unameoruid"];
            
            List<pdauser> gclist = new List<pdauser>();
            
            var parme = "";
            if (logincno!="admin")
            {
                if (!string.IsNullOrEmpty(unameoruid))
                {
                    gclist = dbHelpers.pdauser.Where(db => (db.pdano.Contains(unameoruid) || db.pdaname.Contains(unameoruid) ) && db.cu_no == logincno).ToList();
                    parme += "unameoruid=" + unameoruid;
                }
                else
                {
                    gclist = dbHelpers.pdauser.OrderByDescending(db => db.fid).Where(db => db.cu_no == logincno).ToList();
                }
            }
            else 
            {
                if (!string.IsNullOrEmpty(unameoruid))
                {
                    gclist = dbHelpers.pdauser.Where(db => ( db.pdano.Contains(unameoruid) || db.pdaname.Contains(unameoruid) ) && db.cu_no == "admin").ToList();
                    parme += "unameoruid=" + unameoruid;
                }
                else
                {
                    gclist = dbHelpers.pdauser.Where(db => db.cu_no == "admin").ToList();
                }
            }
            var list = gclist.Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            var count = gclist.Count();
            var pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.list = list;
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = pagecount;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }
        public ActionResult PdaEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];
            var logincno = Request["logincno"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                ViewBag.logincno = logincno;
                return View();
            }
            var model = dbHelpers.pdauser.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.type = "upd";
            ViewBag.logincno = logincno;
            return View();
        }

        public ActionResult PdaAddOrUpd()
        {
            var pdano = Request["pdano"];
            var pdaname = Request["pdaname"];
            var pdapwd = Request["pdapwd"];
            var realname = Request["realname"];
            var phone = Request["phone"];
            var fid = Request["fid"];
            var logincno = Request["logincno"];                        

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            if(string.IsNullOrEmpty(pdano) ||string.IsNullOrEmpty(pdaname) ||string.IsNullOrEmpty(pdapwd) ||string.IsNullOrEmpty(pdano) )
                return Json(new { status = "error", data = "必填项不能为空！" });

            pdauser gy = new pdauser();
            if (!string.IsNullOrEmpty(fid))
            {
                gy = dbHelpers.pdauser.FirstOrDefault(db => db.fid.ToString() == fid);
            }

            if (string.IsNullOrEmpty(fid))
            {
                gy.pdapwd = Uties.CMD5(pdapwd);
            }
            else
            {
                if (gy.pdapwd != pdapwd)
                {
                    gy.pdapwd = Uties.CMD5(pdapwd);
                }
            }  
            
            gy.pdaname = pdaname;         
            gy.add_date = DateTime.Now;
            gy.phone = phone;
            gy.realname = realname;
            gy.cu_no = logincno;
            if (string.IsNullOrEmpty(fid))
            {
                gy.pdano = logincno + "@" + pdano;

                var pmodel = dbHelpers.pdauser.FirstOrDefault(db => db.pdano == gy.pdano);
                if (pmodel != null)
                    return Json(new { status = "error", data = "pda编号已被使用！" });
               
                dbHelpers.pdauser.Add(gy);
            }

           

            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        public JsonResult deleteuser()
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
                        var umodel = dbHelpers.pdauser.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.pdauser.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.pdauser.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.pdauser.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }

        public ActionResult SalReplaceList()
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
            var data = pageImpl.GetPageDate<sal_tiaoji>("sal_tiaoji", pageno, pagesize, "*", "fid desc", "nclass like '%" + selparme + "%' ");
            ViewBag.list = data.DataList;
            ViewBag.count = data.TotalCount;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = data.TotalPages;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            ViewBag.url = "SalReplaceList";
            return View();
        }
        public ActionResult SalReplace() {

            return View();
        }

        public ActionResult SalReplaceAdd(sal_tiaoji st) {

            if (st.oclass ==st.nclass)
                return Json(new { status = "error", data = "标签不能一致！" });

            ConnectionStringEntities db = new ConnectionStringEntities();
            var packb = db.sal_packb.FirstOrDefault(x => x.snno == st.oclass || x.usnno == st.oclass);
            var ship = db.sal_fgi.FirstOrDefault(x => x.bsnno == st.oclass);
            if(packb==null && ship==null)
                return Json(new { status = "error", data = "未找到原标签！" });
            var npackb = db.sal_packb.FirstOrDefault(x => x.snno == st.nclass || x.usnno == st.nclass);
            var nship = db.sal_fgi.FirstOrDefault(x => x.bsnno == st.nclass);
            if (npackb != null || nship != null)
                return Json(new { status = "error", data = "新标签已被使用！" });

            var result = "";
            var sb = db.sal_packb.FirstOrDefault(x => x.usnno == st.oclass);
            if (sb != null)
            { 
                sb.usnno = st.nclass;
                result += "包装大标：" + st.oclass + "-->" + st.nclass+"\n\t";
            }
            var sb2 = db.sal_packb.FirstOrDefault(x => x.snno == st.oclass);
            if (sb2 != null)
            {
                sb2.snno = st.nclass;
                result += "包装小标：" + st.oclass + "-->" + st.nclass + "\n\t";
            }
            var sf = db.sal_fgi.FirstOrDefault(x => x.bsnno == st.oclass);
            if (sf != null)
            {
                sf.bsnno = st.nclass;
                result += "总部入库：" + st.oclass + "-->" + st.nclass + "\n\t";
            }
            var ss = db.sal_ship.FirstOrDefault(x => x.bsnno == st.oclass);
            if (ss != null)
            {
                ss.bsnno = st.nclass;
                result += "总部出货：" + st.oclass + "-->" + st.nclass + "\n\t";
            }
            var sr = db.sal_re.FirstOrDefault(x => x.bsnno == st.oclass);
            if (sr != null)
            {
                sr.bsnno = st.nclass;
                result += "总部退货：" + st.oclass + "-->" + st.nclass + "\n\t";
            }
            var csf = db.sal_cufgi.FirstOrDefault(x => x.bsnno == st.oclass);
            if (csf != null)
            {
                csf.bsnno = st.nclass;
                result += "门店入库：" + st.oclass + "-->" + st.nclass + "\n\t";
            }
            var css = db.sal_cuship.FirstOrDefault(x => x.bsnno == st.oclass);
            if (css != null)
            {
                css.bsnno = st.nclass;
                result += "门店买出：" + st.oclass + "-->" + st.nclass + "\n\t";
            }
            var a = db.SaveChanges();
            if(a>0)
            {
                st.ship_date = DateTime.Now;
                //st.remark = result;
                db.sal_tiaoji.Add(st);
                db.SaveChanges();
                return Json(new { status = "error", data = "成功替换"+a+"条数据！" });
            }
            return Json(new { status = "error", data = "替换失败！" });
        }
    }
}