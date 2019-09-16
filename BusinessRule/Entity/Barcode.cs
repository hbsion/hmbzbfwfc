using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule.Entity
{
    public class Barcode
    {
        public Barcode(sal_packb packEntity)
        {
            this.Code = packEntity.snno;
            this.Ucode = packEntity.usnno;
            this.Tcode = packEntity.usnno;
            this.Qty =  0;
            this.packtype = "0";
            this.snk = 0;
            this.unitcode = "";
            this.unitname = "";
            this.fwmm = "";
            this.p_no = packEntity.p_no;

        }

        public Barcode()
        {
            this.Code = "";
            this.Ucode = "";
            this.Tcode = "";
            this.Qty =  0;
            this.packtype = "";
            this.fwmm = "";
            this.snk = 0;
            this.p_no = "";
        }

        public string Code { get; set; }
        public string Ucode { get; set; }
        public string Tcode { get; set; }
        public int Qty { get; set; }
        public string packtype { get; set; }
        public string fwmm { get; set; }  //防伪码
        public long snk { get; set; }
        public int findresut { get; set; }   //查询结果
        public string p_no { get; set; }    //产品编号
        public string pname { get; set; }    //产品名称
        public string type { get; set; }    //产品规格
        public string unitcode { get; set; }  //企业代码
        public string unitname { get; set; }
        public string resnote { get; set; }   //回复内容

        public int mlength { get; set; }    //m长度
        public int msnlen { get; set; }    //物流m长度

        public int snlen { get; set; }     //物流码长度

        public  long pindex { get; set; }   //一箱排几？



    }
}
