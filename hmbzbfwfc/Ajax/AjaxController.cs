using hmbzbfwfc.Controllers;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.ajax
{
    public class AjaxController : Controller
    {


        public ActionResult GetShipNo(string shipno, string startdate, string pro, string cus)
        {
            ConnectionStringEntities db = new ConnectionStringEntities();
            IQueryable<sal_ship> lspb=db.sal_ship;
            db.Database.CommandTimeout = 3600;
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                DateTime edate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 23:59:59"));     
                lspb = lspb.Where(d => d.ship_date >= sdate && d.ship_date <= edate);
            }
                       

          
            if (!string.IsNullOrEmpty(cus))            
                lspb = lspb.Where(d => d.cu_no == cus || d.cu_name==cus);
             
            if (!string.IsNullOrEmpty(pro))
                lspb = lspb.Where(d => d.p_no == pro || d.pname==pro);
             
            if (!string.IsNullOrEmpty(shipno))            
                lspb = lspb.Where(d => d.ship_no == shipno);

            List<sal_ship> ss = lspb.GroupBy(x => new { x.ship_no, x.cu_name, x.pname }).ToList().Select(s=> new sal_ship {             
             ship_no=s.Key.ship_no,
             cu_name=s.Key.cu_name,
             pname=s.Key.pname,
             mqty=s.Sum(d=>d.mqty)
            }).Take(100).ToList();

            return Json(ss);
        }

        public ActionResult GetVender(string vname) {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var ven = dbHelpers.vender.OrderByDescending(db => db.fid).ToList();
            if (!string.IsNullOrEmpty(vname))
                ven = ven.Where(db => db.cu_name.Contains(vname) || db.cu_no.Contains(vname)).ToList();
            
            return Json(ven);
        }

        // GET: Ajax
        [HttpPost]
        public ActionResult GetProd(string pname_no,string cu_no)
        {                     
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var prodm = dbHelpers.Inv_Part.OrderByDescending(db=>db.fid).ToList();
            if (!string.IsNullOrEmpty(pname_no))
                prodm = prodm.Where(db=>db.p_no.Contains(pname_no) || db.pname.Contains(pname_no)).ToList();
            if (!string.IsNullOrEmpty(cu_no))
                prodm = prodm.Where(db => db.cu_no == cu_no).ToList();
            return Json(prodm);
        }

        [HttpPost]
        public ActionResult GetCustomer(string parme, string pagesize)
        {
            int ipagesize = 10;
            if (!string.IsNullOrEmpty(pagesize))
                ipagesize = int.Parse(pagesize);
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var prodm = dbHelpers.customer.OrderByDescending(db => db.fid).ToList();
            if (!string.IsNullOrEmpty(parme))
                prodm = prodm.Where(db => db.cu_no.Contains(parme) || db.cu_name.Contains(parme)).ToList();
            prodm = prodm.OrderByDescending(db => db.fid).Take(ipagesize).ToList();
            return Json(prodm);
        }

              

        [HttpPost]
        public ActionResult GetInvLot(string pname_no, string cu_no)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var lotm = dbHelpers.Inv_Lot.OrderByDescending(db => db.fid).ToList();
            if (!string.IsNullOrEmpty(pname_no))
                lotm = lotm.Where(db => db.p_no.Contains(pname_no) || db.pname.Contains(pname_no)).ToList();
            if (!string.IsNullOrEmpty(cu_no))
                lotm = lotm.Where(db => db.unitcode == cu_no).ToList();
            return Json(lotm);
        }
        /// <summary>
        /// 流程工序
        /// </summary>        
        [HttpPost]
        public ActionResult GetProcess(string pname_no, string cu_no)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var lotm = dbHelpers.xt_process.OrderByDescending(db => db.fid).ToList();
            if (!string.IsNullOrEmpty(pname_no))
                lotm = lotm.Where(db => db.pc_no.Contains(pname_no) || db.pc_name.Contains(pname_no)).ToList();
            if (!string.IsNullOrEmpty(cu_no))
                lotm = lotm.Where(db => db.unitcode == cu_no).ToList();
            return Json(lotm);
        }

        /// <summary>
        /// 原料信息
        /// </summary>        
        [HttpPost]
        public ActionResult GetMpart(string pname_no, string cu_no)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var lotm = dbHelpers.Inv_Mpart.OrderByDescending(db => db.fid).ToList();
            if (!string.IsNullOrEmpty(pname_no))
                lotm = lotm.Where(db => db.p_no.Contains(pname_no) || db.pname.Contains(pname_no)).ToList();
            if (!string.IsNullOrEmpty(cu_no))
                lotm = lotm.Where(db => db.unitcode == cu_no).ToList();
            return Json(lotm);
        }

        
       
    }
}