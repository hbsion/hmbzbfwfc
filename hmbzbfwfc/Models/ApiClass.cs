using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hmbzbfwfc.Models
{

    public class AqQuery
    {
        public Boolean Success { get; set; }
        public string Message { get; set; }
    }
    public class Result {
        public Boolean Success { get; set; }
        public string Message { get; set; }
        public resultdata ResultData { get; set; }
    }
    public class resultdata {
        public packingobj PackingObj { get; set; }
        public productionobj ProductionObj { get; set; }
        public putinstorateobj PutInStorateObj{ get; set; }

    }

    public class packingobj {
        public string PackRelat { get; set; }
        public string Logistics { get; set; }
        public List<string> SecurityCode { get; set; }
    }

    public class securitycode { 
    
    }

    public class productionobj {

        public string CreateDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string EquipmentNo { get; set; }
        public string OperationUser { get; set; }
        public string BatchNo { get; set; }
        

    }

    public class putinstorateobj {
        public level_1  Level_1 { get; set; }
        public level_2 Level_2 { get; set; }
    }
    public class level_1 {
        public putinfo PutInfo { get; set; }
        public outinfo OutInfo { get; set; }
    }
    public class level_2
    {
        public putinfo PutInfo { get; set; }
        public outinfo OutInfo { get; set; }
    }

    public class putinfo {
        public string PutDate{get;set;}
        public string TakeCustomer{get;set;}
        public string TakeOutBoundNo{get; set;}

    }

    public class outinfo {
        public string ProvinceCity{get;set;}
        public string SendDate{get;set;}
        public string OutBoundNo{get; set;}
        public string Customer { get; set; }
    }

    public class resultPack {
        public Boolean Success { get; set; }
        public string Message { get; set; }
        public packingobj ResultData { get; set; }
    }

    public class resultProd
    {
        public Boolean Success { get; set; }
        public string Message { get; set; }
        public productionobj ResultData { get; set; }
    }

    public class resultstora
    {
        public Boolean Success { get; set; }
        public string Message { get; set; }
        public  putinstorateobj ResultData { get; set; }
    }
}