using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hmbzbfwfc.Controllers
{
    public class wController : Controller
    {
        // GET: w
        public ActionResult Index()
        {
            ConnectionStringEntities dbHelpers=new ConnectionStringEntities();
            ViewBag.code = Request["c"];
            ViewBag.pmodel = dbHelpers.Inv_Part.FirstOrDefault(x => x.p_no.Trim() == "1001");
            ViewBag.ymodel = dbHelpers.Inv_Mpart.OrderBy(db => db.fid);
            return View();
        }

        public ActionResult logisticsinfo()
        {
            ViewBag.code = Request["code"];            
            return View();
        }

        public ActionResult about()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var info = dbHelpers.xt_webnote;
            ViewBag.info = info;
            return View();
        }

       
    }
}