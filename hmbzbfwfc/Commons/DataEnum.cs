using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hmbzbfwfc.Commons
{
    public class DataEnum
    {
        public static string GetPName(string pno)
        {
            var stname = "";
            ConnectionStringEntities db = new ConnectionStringEntities();
            var st = db.Inv_Part.FirstOrDefault(x => x.p_no == pno);
            if (st != null)
                stname = st.pname;
            return stname;
        }

        public static int GetStock(string fgi,string ship)
        {
            int ifgi=0;
            try{
               ifgi=int.Parse(fgi);
            }catch{                
            }
            int iship=0;
            try{
               iship=int.Parse(ship);
            }catch{                
            }

            return ifgi - iship;
        }
      

        public static string GetPType(string pno)
        {
            var type = "";
            ConnectionStringEntities db = new ConnectionStringEntities();
            var st = db.Inv_Part.FirstOrDefault(x => x.p_no == pno);
            if (st != null)
                type = st.type;
            return type;
        }

        public static string GetPcx(string pno)
        {
            var type = "";
            ConnectionStringEntities db = new ConnectionStringEntities();
            var st = db.Inv_Part.FirstOrDefault(x => x.p_no == pno);
            if (st != null)
                type = st.remark2;
            return type;
        }
        public static string GetCName(string cno)
        {
            var stname = "";
            ConnectionStringEntities db = new ConnectionStringEntities();
            var st = db.customer.FirstOrDefault(x => x.cu_no == cno);
            if (st != null)
                stname = st.cu_name;
            return stname;
        }
        public static string GetStName(string stno)
        {
            var stname = "";
            ConnectionStringEntities db = new ConnectionStringEntities();
            var st = db.Inv_Store.FirstOrDefault(x => x.st_no == stno);
            if (st != null)
                stname = st.st_name;
            return stname;
        }

        public static string GetPdaName(string pno)
        {
            var stname = "";
            ConnectionStringEntities db = new ConnectionStringEntities();
            var st = db.Gy_Czygl.FirstOrDefault(x => x.czybm == pno);
            if (st != null)
                stname = st.czymc;
            return stname;
        }
       
        public static string GetItemName(string itemnos) {
            ConnectionStringEntities dbHelpers=new ConnectionStringEntities();
            try
            {
                string[] items = itemnos.Split(',');
                var itemname = "";
                foreach (var item in items)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    var model = dbHelpers.hob_item.FirstOrDefault(db => db.itme_no == item);
                    if (model != null)
                        itemname += model.itme_name + ",";
                }

                return itemname;
            }
            catch { 
            }

            return "";
        }


        public enum InStoreBillStatus
        {
            待审核 = 1,
            审核通过 = 2,
            审核失败 = 4
        }

        public enum djqstatus { 
            未使用=0,
            以使用=1
        }

        public enum djqtype
        {
            扫码获取 = 1,
            大转盘获取= 1
        }
        public enum errorType
        {
            字符 = 1,
            数字 = 2,
            实体 = 4,
            数组 = 8
        }
        public enum errorCode
        {
            程序异常 = 16,
            参数错误 = 32,
            数据不存在 = 64,
            其他错误 = 128,
            用户未登录 = 256,
            权限不足 = 512,
            登录失败 = 1024,
        }


        public enum filestatus
        {
            上传待导入 = 0,
            已导入 = 9,
            撤销 = 4,
            导入错误 = 1
        }


    }
}