using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marr.BusinessRule.Entity
{
    [Serializable] 
    public  class Fwcode
    {
        public string fwmmcode { get; set; }
        public string unitcode { get; set; }
        public string unitname { get; set; }
        public string resnote { get; set; }   //回复内容
        public int  findresut { get; set; }   //查询结果
        public string sntype { get; set; }  //物流码类型
        public int snlen { get; set; }     //物流码长度
        public string  snpr { get; set; }     //物流码前缀

        public int mlength { get; set; }    //m长度
        public int msnlen { get; set; }    //物流m长度

        public long gkkk { get; set; }    //k值

        public decimal jf { get; set; }    //积分

        public string  p_no { get; set; }    //产品编号
        public string  pname { get; set; }    //产品名称
        public string  type { get; set; }    //产品规格

        public string packtype { get; set; }    //包装类型
        public string snno { get; set; }    //对应流水号
        public string tsnno { get; set; }    //对应上级条码
        public string usnno { get; set; }    //对应上级条码

        public decimal cjf { get; set; }    //促销员积分
        public int pqty { get; set; }    //包装数量
     
        public string xtunitcode { get; set; }  //企业代码

        public string fwmm { get; set; }  //验证码

        public string makeline { get; set; }  //生产线

        public string zp { get; set; }   //正品回复内容
        public int cxcs { get; set; }     //查询次数

        public string code { get; set; }    //对应物流码



    }
}


