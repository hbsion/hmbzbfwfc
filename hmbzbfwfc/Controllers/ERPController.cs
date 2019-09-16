using hmbzbfwfc.Commons;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Controllers
{
    public class ERPController : Controller
    {
        // GET: ERP
        public ActionResult EditInStoreBill()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            ViewBag.orderno = Uties.GetOrderNumber();
            ViewBag.kb = dbHelpers.Inv_Store.OrderByDescending(db => db.fid);
            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View("CreateInStoreBill");
            }
            var model = dbHelpers.InStore.FirstOrDefault(db => db.fid.ToString() == fid);
            ViewBag.model = model;
            ViewBag.pmodel = dbHelpers.InStoreDetail.Where(db => db.billno == model.OrderNum);
            ViewBag.type = "upd";
            return View("CreateInStoreBill");
        }

        public ActionResult SaveInStoreProd(InStoreDetail isd)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();            
            
            isd.indate = DateTime.Now;
           
            dbHelpers.InStoreDetail.Add(isd);

            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", fid = isd.fid });

            return Json(new { status = "error" });
        }

        public ActionResult DelInStoreProd(string fid, string orderno)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            if (!string.IsNullOrEmpty(orderno) && string.IsNullOrEmpty(fid))
            {
                var pro = dbHelpers.InStoreDetail.Where(db => db.billno == orderno);
                dbHelpers.InStoreDetail.RemoveRange(pro);
            }
            else
            {
                InStoreDetail sd = dbHelpers.InStoreDetail.FirstOrDefault(db => db.fid.ToString() == fid);
                dbHelpers.InStoreDetail.Remove(sd);
            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success" });

            return Json(new { status = "error" });
        }

        [HttpPost]
        public ActionResult CreateInStoreBill(InStore ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            if (string.IsNullOrEmpty(ins.SupNum) || string.IsNullOrEmpty(ins.SupName) || string.IsNullOrEmpty(ins.ContactName) || string.IsNullOrEmpty(ins.Phone))
                return Json(new { status = "error", data = "供应商信息不能为空" });
            var prod = dbHelpers.InStoreDetail.Where(db => db.billno == ins.OrderNum);
            if (prod.Count() <= 0)
                return Json(new { status = "error", data = "请添加入库产品" });
            InStore ais = dbHelpers.InStore.FirstOrDefault(db => db.fid == ins.fid);

            ins.CreateTime = DateTime.Now;
            ins.Status = 1;
            ins.Amount = double.Parse(prod.Sum(db => db.sumprice).ToString());
            ins.Num = double.Parse(prod.Sum(db => db.num).ToString());

            if (ais == null)
            {
                dbHelpers.InStore.Add(ins);
            }
            else
            {
                ais.InType = ins.InType;
                ais.CreateUser = ins.CreateUser;
                ais.ContractOrder = ins.ContractOrder;
                ais.SupNum = ins.SupNum;
                ais.SupName = ins.SupName;
                ais.ContactName = ins.ContactName;
                ais.Phone = ins.Phone;
                ais.Amount = ins.Amount;
                ais.Num = ins.Num;
            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        public ActionResult InStoreBillList()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            int pageno = 1;
            if (!string.IsNullOrEmpty(Request["pageno"]))
                pageno = int.Parse(Request["pageno"]);

            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request["pagesize"]))
                pagesize = int.Parse(Request["pagesize"]);
            var parme = "";
            var orderno = Request["orderno"];
            var startdate = Request["stratdate"];
            var enddate = Request["enddate"];

            IQueryable<InStore> ins = dbHelpers.InStore.OrderByDescending(db => db.fid);

            if (!string.IsNullOrEmpty(orderno))
            {
                ins = ins.Where(db => db.OrderNum.Contains(orderno));
                parme += "&orderno=" + orderno;
            }
            if (!string.IsNullOrEmpty(startdate))
            {
                var sdate = DateTime.Parse(DateTime.Parse(startdate).ToString("yyyy-MM-dd 00:00:00"));
                var edate = DateTime.Parse(DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59"));
                ins = ins.Where(db => db.OrderTime >= sdate && db.OrderTime <= edate);
                parme += "&startdate=" + startdate + "&enddate=" + enddate;
            }
            var count = ins.Count();
            var pagecount = (int)Math.Ceiling((decimal)count / pagesize);
            ViewBag.list = ins.Skip((pageno - 1) * pagesize).Take(pagesize).ToList();
            ViewBag.count = count;
            ViewBag.pageno = pageno;
            ViewBag.pagecount = pagecount;
            ViewBag.pagesize = pagesize;
            ViewBag.parme = parme;
            return View();
        }

        public JsonResult InStoreBillDel()
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
                        var umodel = dbHelpers.InStore.FirstOrDefault(db => db.fid.ToString() == i);
                        var bpmodel = dbHelpers.InStoreDetail.Where(db => db.billno == umodel.OrderNum);
                        dbHelpers.InStore.Remove(umodel);
                        dbHelpers.InStoreDetail.RemoveRange(bpmodel);

                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.InStore.FirstOrDefault(db => db.fid.ToString() == fid);
            var abpmodel = dbHelpers.InStoreDetail.Where(db => db.billno == model.OrderNum);
            dbHelpers.InStore.Remove(model);
            dbHelpers.InStoreDetail.RemoveRange(abpmodel);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }

        public ActionResult ShowInStoreBill()
        {
            var fid = Request["fid"];
            var type = Request["type"];
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var ins = dbHelpers.InStore.FirstOrDefault(db => db.fid.ToString() == fid);
            var insd = dbHelpers.InStoreDetail.Where(db => db.billno == ins.OrderNum);
            ViewBag.type = type;
            ViewBag.ins = ins;
            ViewBag.insd = insd;
            return View();
        }

        public ActionResult InStoreBillAudit()
        {
            var fid = Request["fid"];
            var status = Request["status"];
            var reason = Request["reason"];
            var audituser = Request["audituser"];
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var ins = dbHelpers.InStore.FirstOrDefault(db => db.fid.ToString() == fid);

            ins.AuditeTime = DateTime.Now;
            ins.AuditUser = audituser;
            ins.Reason = reason;
            ins.Status = int.Parse(status);
            if (dbHelpers.SaveChanges() <= 0)
                return Json(new { status = "error" });

            return Json(new { status = "success" });
        }
    }
}