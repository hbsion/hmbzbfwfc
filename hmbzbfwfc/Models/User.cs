using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace hmbzbfwfc.Models
{
    [Serializable] 
    public class  User
    {
        public string UserType { get; set; }
        public string UserName { get; set; }
        public int UserFlag { get; set; }
        public string cu_no { get; set; }  //客户编号
        public string cu_name { get; set; }  //客户名称
        public string xtcu_no { get; set; }  //上级客户编号
        public string xtsysrole { get; set; }  //系统角色
        public string UserNo { get; set; }
        public string xtlc_no { get; set; }  //所属地区
        public string xtloca { get; set; }  //所属地区名称
        public string xtunitcode { get; set; }  //企业代号
        public int xtsninyn { get; set; }  //是否需要导入 0表示不需要，1表示需要
        public int xtpacktype { get; set; }  //包装方式 1表示单个，2表示大小
        public string xtunitname { get; set; }  //企业名称
        public string xtlogo { get; set; }  //企业LOGO
        public string xtfwunitcode { get; set; }  //防伪产品代码

        public string xtfwyn { get; set; }  //是否防伪
        public string xtfcyn { get; set; }  //是否防窜
        public string xtjfyn { get; set; }  //是否积分
        public string xtstyn { get; set; }  //是否库存

        public string xtckyn { get; set; }  //是否多库别

        public string xtfgiyn { get; set; }  //是否入库
        public string xtprdyn { get; set; }  //是否考虑产品
        public string xtrecl { get; set; }   //重复处理方式
        public string xtfcnote { get; set; }  //物流复回复内容

        public string xtthcl { get; set; }   //退货处理  
        public string xthyzl { get; set; }   //会员资料

        public string xtdelyn { get; set; }   //代理商

        public string xtqcyn { get; set; }  //是否质量追溯


        public string xtqkyn { get; set; }   //质保

        public string xthbyn { get; set; }  //是否红包
        public int xtfid { get; set; }  //ID


    }
}
