
using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Controllers
{
    public class CuLogisticsController : ManageBaseController
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CuGetFgiCode(string code,string cu_no) {
            ConnectionStringEntities dbHelpers=new ConnectionStringEntities();
            
                var cmodel = dbHelpers.sal_ship.Where(db=>db.bsnno.Contains(code) && db.cu_no==cu_no);
                List<sal_ship> lfgi = cmodel.ToList();
                var lf = lfgi.Select(item => new fgiinfo
                {
                    p_no = item.p_no,
                    pname = item.pname,
                    bsnno = item.bsnno,                   
                }).Take(10);

                return Json(lf);
            
        }
        /// <summary>
        /// 入库扫描页
        /// </summary>
        /// <returns></returns>
        public ActionResult CuSalFgiScanPage()
        {
                  
            ViewBag.fgino = "Z" + Uties.GetOrderNumber();

            return View();
        }

        /// <summary>
        /// 入库扫描操作
        /// </summary>
        /// <returns></returns>
        public ActionResult CuSalFgiScan(sal_cufgi scf,string cpsl,string js)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
           
            
            var ship = dbHelpers.sal_ship.FirstOrDefault(db=>db.cu_no==scf.xtcu_no && db.bsnno==scf.bsnno);
            if(ship==null)
                return Json(new { status = false, data = scf.bsnno + "条码错误！<br/>" });

            int cps = 0;
            if (!string.IsNullOrEmpty(cpsl))
                cps = int.Parse(cpsl);

            int pjs = 0;
            if (!string.IsNullOrEmpty(js))
                pjs = int.Parse(js);

            var isfgi = dbHelpers.sal_cufgi.FirstOrDefault(db => db.bsnno == scf.bsnno);
            if (isfgi != null)
                return Json(new { status = false, data = scf.bsnno + "重复入库！<br/>" });
            scf.mqty = ship.mqty;
            
            scf.unitcode = "";
            dbHelpers.sal_cufgi.Add(scf);
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = true, data = scf.bsnno + "入库成功！<br/>",js=pjs+1, cpsl = cps+scf.mqty });
            return Json(new { status = false, data = scf.bsnno + "入库失败！<br/>" });
        }

        public class fgiinfo
        {
            public string p_no { get; set; }
            public string pname { get; set; }
            public string bsnno { get; set; }
            public string st_no { get; set; }
            public string st_name { get; set; }

        }
        public ActionResult CuGetShipCode()
        {
            var tmbh = Request["tmbh"];
            ConnectionStringEntities db = new ConnectionStringEntities();
            var fgi = db.sal_cufgi.Where(d => d.bsnno.Contains(tmbh));
            List<sal_cufgi> lfgi = fgi.ToList();
            var lf = lfgi.Select(item => new fgiinfo
            {
                p_no = item.p_no,
                pname = item.pname,
                bsnno = item.bsnno,
                
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
        public ActionResult CuSalShipScanPage()
        {
            
            ViewBag.fgino = "X" + Uties.GetOrderNumber();

            return View();
        }

        /// <summary>
        /// 出库扫描操作
        /// </summary>
        /// <returns></returns>
        public ActionResult CuSalShipScan(sal_cuship scs,string cpsl, string js)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            
            var ship = dbHelpers.sal_cufgi.FirstOrDefault(db => db.xtcu_no == scs.xtcu_no && db.bsnno == scs.bsnno);
            if (ship== null)
                return Json(new { status = false, data = scs.bsnno + "条码错误！<br/>" });

            int cps = 0;
            if (!string.IsNullOrEmpty(cpsl))
                cps = int.Parse(cpsl);

            int pjs = 0;
            if (!string.IsNullOrEmpty(js))
                pjs = int.Parse(js);

            var isship = dbHelpers.sal_cuship.FirstOrDefault(db => db.bsnno == scs.bsnno);
            if (isship != null)
                return Json(new { status = false, data = scs.bsnno + "重复出库！<br/>" });

            scs.mqty =ship.mqty;
            

            scs.unitcode = "";
            dbHelpers.sal_cuship.Add(scs);
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = true, data = scs.bsnno + "出库成功！<br/>", js = pjs + 1, cpsl = cps + scs.mqty });
            return Json(new { status = false, data = scs.bsnno + "出库失败！<br/>" });
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
        public ActionResult CuGetReCode()
        {
            var tmbh = Request["tmbh"];
            ConnectionStringEntities db = new ConnectionStringEntities();
            var fgi = db.sal_cuship.Where(d => d.bsnno.Contains(tmbh));
            List<sal_cuship> lfgi = fgi.ToList();
            var lf = lfgi.Select(item => new shipinfo
            {
                p_no = item.p_no,
                pname = item.pname,
                bsnno = item.bsnno,                
                cu_no = item.cu_no,
                cu_name = item.cu_name,
            }).Take(10);

            return Json(lf);
        }
        /// <summary>
        /// 退货扫描页
        /// </summary>
        /// <returns></returns>
        public ActionResult CuSalReScanPage()
        {

            ViewBag.fgino = "Y" + Uties.GetOrderNumber();

            return View();
        }

        /// <summary>
        /// 退货扫描操作
        /// </summary>
        /// <returns></returns>
        public ActionResult CuSalReScan(sal_cure scr, string cpsl,string js)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            var ship = dbHelpers.sal_cuship.FirstOrDefault(db => db.xtcu_no == scr.xtcu_no && db.bsnno == scr.bsnno);
            if (ship== null)
                return Json(new { status = false, data = scr.bsnno + "条码错误！<br/>" });

            int cps = 0;
            if (!string.IsNullOrEmpty(cpsl))
                cps = int.Parse(cpsl);

            int pjs = 0;
            if (!string.IsNullOrEmpty(js))
                pjs = int.Parse(js);

            var isship = dbHelpers.sal_cure.FirstOrDefault(db => db.bsnno == scr.bsnno);
            if (isship != null)
                return Json(new { status = false, data = scr.bsnno + "重复退货！<br/>" });

            scr.mqty = ship.mqty;           

            scr.unitcode = "";
            dbHelpers.sal_cure.Add(scr);
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = true, data = scr.bsnno + "退货成功！<br/>", js = pjs + 1, cpsl = cps + scr.mqty });
            return Json(new { status = false, data = scr.bsnno + "退货失败！<br/>" });
        }
    }
}