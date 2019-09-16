using hmbzbfwfc.Attributes;
using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using hmbzbfwfc.Commons;
using System.Collections;
namespace hmbzbfwfc.Controllers
{
   
    [Manage]
    public class UsersController : ManageBaseController
    {
        // GET: Users

        public ActionResult UserList(int pageno)
        {
            int pagesize = 10;
            if (!string.IsNullOrEmpty(Request["pagesize"]))
            {
                pagesize = int.Parse(Request["pagesize"]);
            }

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            if (string.IsNullOrEmpty(pageno.ToString()))
                pageno = 1;
            var unameoruid = Request["unameoruid"];
            var ygbh = Request["ygbh"];
            var ygxm = Request["ygxm"];
            var bm = Request["bm"];
            var js = Request["js"];
            List<Gy_Czygl> gclist = new List<Gy_Czygl>();
            var parme = "";
            if (!string.IsNullOrEmpty(unameoruid))
            {
                gclist = dbHelpers.Gy_Czygl.Where(db => db.fid.ToString().Contains(unameoruid) || db.czymc.Contains(unameoruid)).ToList();
                parme += "unameoruid=" + unameoruid;

            }
            else
            {

                gclist = dbHelpers.Gy_Czygl.OrderByDescending(db => db.fid).Where(db => db.fid != 0).ToList();

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
        public ActionResult UserEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var poweritem = "";
            var power = dbHelpers.xt_user_rms.OrderBy(db => db.fid);
            if (power == null)
                poweritem = "暂无可选择的权限组";

            var fid = Request["fid"];
            if (string.IsNullOrEmpty(fid))
            {
                foreach (var item in power)
                {
                    poweritem += "<label><input type='radio' name='rms_name' value='" + item.rms_name + "'>" + item.rms_name + "</label>";
                }
                ViewBag.poweritem = poweritem;
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.Gy_Czygl.FirstOrDefault(db => db.fid.ToString() == fid);
            var mpower = dbHelpers.xt_user_rms.FirstOrDefault(db => db.rms_name == model.rms_name);
            var mrname = "";
            if (mpower != null)
                mrname = mpower.rms_name;
            foreach (var item in power)
            {
                if (!string.IsNullOrEmpty(item.rms_name) && item.rms_name == mrname)
                    poweritem += "<label><input checked type='radio' name='rms_name' value='" + item.rms_name + "'>" + item.rms_name + "</label>";
                else
                    poweritem += "<label><input type='radio' name='rms_name' value='" + item.rms_name + "'>" + item.rms_name + "</label>";
            }

            var pdac = "";
            if(!string.IsNullOrEmpty(model.cu_name))
            {
                pdac = getpdaqx(model.cu_name); 
            }

            ViewBag.pdac = pdac;
            ViewBag.poweritem = poweritem;
            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

        public string getpdaqx(string sel)
        {
            var pdac = "";
            string[] pq = sel.Split(',');
            if (pq.Length >= 5)
            {
                if (pq[0] == "1")
                {
                    pdac += " <label><input type='checkbox' checked name='rk' value='1' />入库</label>";
                }
                else
                {
                    pdac += " <label><input type='checkbox'  name='rk' value='1' />入库</label>";
                }
                if (pq[1] == "1")
                {
                    pdac += " <label><input type='checkbox' checked name='ck' value='1' />出库</label>";
                }
                else
                {
                    pdac += " <label><input type='checkbox'  name='ck' value='1' />出库</label>";
                }
                if (pq[2] == "1")
                {
                    pdac += " <label><input type='checkbox' checked name='th' value='1' />退货</label>";
                }
                else
                {
                    pdac += " <label><input type='checkbox'  name='th' value='1' />退货</label>";
                }
                if (pq[3] == "1")
                {
                    pdac += " <label><input type='checkbox' checked name='bz' value='1' />包装</label>";
                }
                else
                {
                    pdac += " <label><input type='checkbox'  name='bz' value='1' />包装</label>";
                }
                if (pq[4] == "1")
                {
                    pdac += " <label><input type='checkbox' checked name='db' value='1' />调拨</label>";
                }
                else
                {
                    pdac += " <label><input type='checkbox'  name='db' value='1' />调拨</label>";
                }
            }
            return pdac;
        }


        public ActionResult AddOrUpd(Gy_Czygl user,FormCollection coll)
        {

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            Gy_Czygl gy = dbHelpers.Gy_Czygl.FirstOrDefault(db => db.fid == user.fid);
            if (string.IsNullOrEmpty(user.czybm) || string.IsNullOrEmpty(user.czymc) || string.IsNullOrEmpty(user.czmm) || string.IsNullOrEmpty(user.sysrole))
                return Json(new { status = "error", data = "必填项不能为空！" });

            var p="";
            var cp = "";
            if(coll["rk"]=="1")
            {
                p += "1";
                cp += "1,";
            }
            else
            {
                p += "0";
                cp += "0,";
            }
            if (coll["ck"] == "1")
            {
                p += "1";
                cp += "1,";
            }
            else
            {
                p += "0";
                cp += "0,";
            }
            if (coll["th"] == "1")
            {
                p += "1";
                cp += "1,";
            }
            else
            {
                p += "0";
                cp += "0,";
            }
            if (coll["bz"] == "1")
            {
                p += "1";
                cp += "1,";
            }
            else
            {
                p += "0";
                cp += "0,";
            }
            if (coll["db"] == "1")
            {
                p += "1";
                cp += "1";
            }
            else
            {
                p += "0";
                cp += "0";
            }

            if (gy == null)
            {
                var czy = dbHelpers.Gy_Czygl.FirstOrDefault(db => db.czybm == user.czybm);
                if (czy != null)
                    return Json(new { status = "error", data = "该编号已被使用！" });
                user.czmm = Uties.CMD5(user.czmm);
                user.in_date = DateTime.Now;
                user.deptname = p;
                user.cu_name = cp;                
                user.unitcode = "";
                dbHelpers.Gy_Czygl.Add(user);
            }
            else
            {
                gy.cu_name = cp;
                gy.deptname = p;
                gy.czybm = user.czybm;
                gy.czymc = user.czymc;
                if (gy.czmm != user.czmm)
                {
                    gy.czmm = Uties.CMD5(user.czmm);
                }                
                gy.sysrole = user.sysrole;
                gy.checkyn = user.checkyn;
                gy.rms_name = user.rms_name;
                user.unitcode = "";
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
                        var umodel = dbHelpers.Gy_Czygl.FirstOrDefault(db => db.fid.ToString() == i);
                        dbHelpers.Gy_Czygl.Remove(umodel);
                    }
                }
                dbHelpers.SaveChanges();

                return Json("删除成功");
            }

            var model = dbHelpers.Gy_Czygl.FirstOrDefault(db => db.fid.ToString() == fid);
            dbHelpers.Gy_Czygl.Remove(model);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }

      
        //导出excel
        public FileResult Excel()
        {
            ConnectionStringEntities dbHelpers=new ConnectionStringEntities();
            //获取list数据
            
            var unameoruid = Request["unameoruid"];
            var ygbh = Request["ygbh"];
            var ygxm = Request["ygxm"];
            var bm = Request["bm"];
            var js = Request["js"];
            List<Gy_Czygl> list = new List<Gy_Czygl>();
            
            if (!string.IsNullOrEmpty(unameoruid))
            {
                list = dbHelpers.Gy_Czygl.Where(db => db.fid.ToString().Contains(unameoruid) || db.czymc.Contains(unameoruid)).ToList();               
            }
            else
            {

                list = dbHelpers.Gy_Czygl.OrderByDescending(db => db.fid).Where(db => db.fid != 0).ToList();
            }
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("ID");
            row1.CreateCell(1).SetCellValue("操作员编号");
            row1.CreateCell(2).SetCellValue("操作员姓名");
            row1.CreateCell(3).SetCellValue("部门名称");
            row1.CreateCell(4).SetCellValue("加入日期");
            row1.CreateCell(5).SetCellValue("代理商编号");
            row1.CreateCell(6).SetCellValue("代理商名称");
            row1.CreateCell(7).SetCellValue("备注");
           
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].fid);
                rowtemp.CreateCell(1).SetCellValue(list[i].czybm);
                rowtemp.CreateCell(2).SetCellValue(list[i].czymc);
                rowtemp.CreateCell(3).SetCellValue(list[i].deptname);
                rowtemp.CreateCell(4).SetCellValue(list[i].in_date.Value.ToString());
                rowtemp.CreateCell(5).SetCellValue(list[i].cu_no);
                rowtemp.CreateCell(6).SetCellValue(list[i].cu_name);
                rowtemp.CreateCell(7).SetCellValue(list[i].remark);
                
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "用户信息"+DateTime.Now.ToString("yyyyMMddHHmmss")+".xls");
        }        

        public ActionResult showeditpwd() {
            var logintype=Request["logintype"];
            ViewBag.logintype = logintype;
            return View("editpwd");
        }
        [HttpPost]
        public JsonResult editpwd() {
            var uid=Request["uid"];
            var oldpwd=Request["oldpwd"];
            var newpwd=Request["newpwd"];
            var rnewpwd=Request["rnewpwd"];
            var logintype=Request["logintype"];
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            if(string.IsNullOrEmpty(newpwd) || newpwd.Length<6)
                return Json(new { status = "error", data = "新密码不能为空,并且长度不能小于6位" });

            if (oldpwd == newpwd)
                return Json(new { status = "error", data = "新密码不能与旧密码一致" });
            if (newpwd != rnewpwd)
                return Json(new { status = "error", data = "两次新密码不一致" });

            if (logintype == "u") { 
            var umodel = dbHelpers.Gy_Czygl.FirstOrDefault(db => db.fid.ToString() == uid);            
            if (Uties.CMD5(oldpwd) != umodel.czmm)
                return Json(new { status="error",data="原密码错误"});           
            umodel.czmm = Uties.CMD5(newpwd);
            } if (logintype == "c")
            {
                var umodel = dbHelpers.customer.FirstOrDefault(db => db.fid.ToString() == uid);
                if (Uties.CMD5(oldpwd) != umodel.passwd)
                    return Json(new { status = "error", data = "原密码错误" });
                umodel.passwd = Uties.CMD5(newpwd);
            }
            var isok=dbHelpers.SaveChanges();
            if (isok > 0)
                return Json(new { status = "success", data = "密码修改成功" });
            return Json(new { status = "error", data = "密码修改失败" });

        }

        [Manage]
        public ActionResult PowerList()
        {
            var unitcode = DESEncrypt.Decrypt(Request.Cookies["unitcode"].Value);
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var powerlist = dbHelpers.xt_user_rms.Where(db => db.unitcode == unitcode);

            ViewBag.menu = powerlist;
            return View();
        }

        [Manage]
        public ActionResult MenuXjList()
        {
            var gnbm = Request["gnbm"];

            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var onemenu = dbHelpers.xt_xtmenu.Where(db => db.sjgnbm == gnbm).ToList();

            ViewBag.menu = onemenu;
            return View();
        }

        [Manage]
        public ActionResult PowerEdit()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];

            if (string.IsNullOrEmpty(fid))
            {
                ViewBag.type = "add";
                return View();
            }
            var model = dbHelpers.xt_user_rms.FirstOrDefault(db => db.fid.ToString() == fid);

            ViewBag.model = model;
            ViewBag.type = "upd";

            return View();
        }

        [HttpPost]
        public ActionResult PowerAddOrUpd(xt_user_rms ins)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            xt_user_rms ip = dbHelpers.xt_user_rms.FirstOrDefault(db => db.fid == ins.fid);
            var unitcode = DESEncrypt.Decrypt(Request.Cookies["unitcode"].Value);

            if (string.IsNullOrEmpty(ins.rms_name))
                return Json(new { status = "error", data = "必填项不能为空！" });
            if (ip == null)
            {
                var sm = dbHelpers.xt_user_rms.FirstOrDefault(db => db.rms_name == ins.rms_name);
                if (sm != null)
                    return Json(new { status = "error", data = "已有该权限组名称！" });
                ins.unitcode = unitcode;
                dbHelpers.xt_user_rms.Add(ins);
            }
            else
            {
                ip.rms_name = ins.rms_name;
                ip.remark = ins.remark;
            }
            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

        [Manage]
        public JsonResult PowerDel()
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
            var fid = Request["fid"];


            var model = dbHelpers.xt_user_rms.FirstOrDefault(db => db.fid.ToString() == fid);
            var mmodel = dbHelpers.xt_user_menu.Where(db => db.rms_name == model.rms_name);
            var isuse = dbHelpers.Gy_Czygl.Where(db => db.rms_name == model.rms_name);
            if (isuse.Count() > 0)
                return Json("权限组已有绑定操作员，删除失败");


            dbHelpers.xt_user_rms.Remove(model);
            dbHelpers.xt_user_menu.RemoveRange(mmodel);
            dbHelpers.SaveChanges();

            return Json("删除成功");
        }

        public ActionResult PowerSetting()
        {

            var fid = Request["fid"];
            var rname = Request["rname"];
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            var pw = dbHelpers.xt_user_menu.Where(db => db.rms_name == rname);
            List<string> strList = new List<string>();
            foreach (var item in pw)
            {
                strList.Add(item.munid.ToString());//循环添加元素
            }
            string[] pwarray = strList.ToArray();
            var onemenu = dbHelpers.xt_xtmenu.Where(db => string.IsNullOrEmpty(db.sjgnbm));
            var power = "";
            foreach (var one in onemenu)
            {
                bool exists = ((IList)pwarray).Contains(one.fid.ToString());
                if (exists)
                    power += "<tr><td><label  onclick='powersel(\"" + one.fid + "\");' id='all" + one.fid + "' class='pwsel" + one.fid + "'><input checked type='checkbox' name='power' value='" + one.fid + "'/>" + one.gnbm + "|" + one.gnmc + "</label></td><td id='sel" + one.fid + "'>";
                else
                    power += "<tr><td><label onclick='powersel(\"" + one.fid + "\");' id='all" + one.fid + "' class='pwsel" + one.fid + "'><input  type='checkbox' name='power' value='" + one.fid + "'/>" + one.gnbm + "|" + one.gnmc + "</label></td><td id='sel" + one.fid + "'>";
                var twomenu = dbHelpers.xt_xtmenu.Where(db => db.sjgnbm == one.gnbm);
                foreach (var two in twomenu)
                {
                    bool exists2 = ((IList)pwarray).Contains(two.fid.ToString());
                    if (exists2)
                        power += "<label><input class='pwsel" + one.fid + "' checked type='checkbox' name='power' value='" + two.fid + "'/>" + two.gnbm + "|" + two.gnmc + "</label>";
                    else
                        power += "<label><input class='pwsel" + one.fid + "'  type='checkbox' name='power' value='" + two.fid + "'/>" + two.gnbm + "|" + two.gnmc + "</label>";
                }
                power += "</td></tr>";

            }
            ViewBag.fid = fid;
            ViewBag.power = power;
            return View();
        }

        [HttpPost]
        public ActionResult PowerSettingUpd(string fid, string[] power)
        {
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            xt_user_rms ip = dbHelpers.xt_user_rms.FirstOrDefault(db => db.fid.ToString() == fid);


            if (power.Length > 0)
            {
                for (var i = 0; i < power.Length; i++)
                {
                    var poweri = power[i];

                    var jumppw = dbHelpers.xt_user_menu.FirstOrDefault(db => db.rms_name == ip.rms_name && db.munid.ToString() == poweri);
                    if (jumppw != null)
                        continue;
                    xt_user_menu ins = new xt_user_menu();
                    ins.rms_name = ip.rms_name;
                    ins.munid = int.Parse(power[i]);

                    ins.gnmc = dbHelpers.xt_xtmenu.FirstOrDefault(db => db.fid.ToString() == poweri).gnmc;
                    dbHelpers.xt_user_menu.Add(ins);
                }
            }


            if (dbHelpers.SaveChanges() > 0)
                return Json(new { status = "success", data = "操作成功" });
            return Json(new { status = "error", data = "操作失败" });
        }

    }
}