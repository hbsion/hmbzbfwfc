using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

using System.Collections.Generic;
using System.Linq;
using hmbzbfwfc.Models;

namespace hmbzbfwfc.Commons
{   
    public partial class ExcelToDB : System.Web.UI.Page
    {
        public int ProToDataBase(string filepath)
        {
            FileSvr fileSvr = new FileSvr();
            System.Data.DataTable dt = fileSvr.GetExcelDatatable(filepath, "mapTable");            
           return  fileSvr.InsetProt(dt);
        }
        public int CusToDataBase(string filepath)
        {
            FileSvr fileSvr = new FileSvr();
            System.Data.DataTable dt = fileSvr.GetExcelDatatable(filepath, "mapTable");
            return fileSvr.InsetCus(dt);
        }

        public int VenToDataBase(string filepath)
        {
            FileSvr fileSvr = new FileSvr();
            System.Data.DataTable dt = fileSvr.GetExcelDatatable(filepath, "mapTable");
            return fileSvr.InsetVen(dt);
        }
    }
        
    class FileSvr
    {
        /// <summary>
        /// Excel数据导入Datable
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public System.Data.DataTable GetExcelDatatable(string fileUrl, string table)
        {
            //office2007之前 仅支持.xls
            //const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";
            //支持.xls和.xlsx，即包括office2010等版本的   HDR=Yes代表第一行是标题，不是数据；
            const string cmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

            System.Data.DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, fileUrl));
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }


                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //获取Excel的第一个Sheet名称
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();

                //查询sheet中的数据
                string strSql = "select * from [" + sheetName + "]";
                OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, table);
                dt = ds.Tables[0];

                return dt;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }

       

        /// <summary>
        /// 从System.Data.DataTable导入数据到数据库
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int InsetProt(System.Data.DataTable dt)
        {
           
            int i = 0;
            string p_no = "";
            string pname = "";                   
            string  type= "";
            int pqty = 1;
            string unit = "";
            DateTime in_date =DateTime.Now;
            string parttype = "";            
            string imgurl = "";
            string bm_no = "";
            string remark2 = "";
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();
         
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                 
                     p_no = dr["产品编号"].ToString().Trim();
                     pname = dr["产品名称"].ToString().Trim();
                     type = dr["产品规格"].ToString().Trim();
                     unit = dr["单位"].ToString().Trim();
                     try
                     {
                         pqty = int.Parse(dr["包装规格"].ToString().Trim());
                     }
                     catch
                     {
                         pqty = 1;
                     }
                     in_date = dr["建立日期"].ToString().Trim() == "" ? DateTime.Now : DateTime.Parse(dr["建立日期"].ToString().Trim());
                     parttype = dr["分类"].ToString().Trim();
                     imgurl = dr["产品图片"].ToString().Trim();
                     bm_no = dr["产品条码"].ToString().Trim();
                     //remark2 = dr["适用车型"].ToString().Trim();

                     //sw = string.IsNullOrEmpty(sw) ? "null" : sw;
                     //kr = string.IsNullOrEmpty(kr) ? "null" : kr;


                  var pmodel = dbHelpers.Inv_Part.FirstOrDefault(db => db.p_no == p_no);
                  if (pmodel == null)
                  {
                     Inv_Part ip = new Inv_Part();
                     ip.p_no = p_no;
                     ip.pname = pname;
                     ip.type = type;
                     ip.unit = unit;
                     ip.pqty = pqty;
                     ip.in_date = in_date;
                     ip.parttype = parttype;
                     ip.imgurl = imgurl;
                     ip.bm_no = bm_no;
                     ip.unitcode = "";
                     ip.remark2 = remark2;
                     dbHelpers.Inv_Part.Add(ip);
                     i++;
                 } 
                }
            }
            catch {
                return 0;
            }
            dbHelpers.SaveChanges();
            return i;
        }

        public int InsetCus(System.Data.DataTable dt)
        {
            
            int i = 0;
            string cu_no = "";
            string cuname = "";
            string cutype = "";
            
            string xtcu_no = "";
            string province ="";
            string city = "";
            string addr = "";
            string phone = "";
            string fax = "";
            string email = "";
            string link_man = "";
            string pasword="";
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            try
            {
                foreach (DataRow dr in dt.Rows)
                {

                    cu_no= dr["客户编号"].ToString().Trim();
                    cuname = dr["客户名称"].ToString().Trim();
                    cutype= dr["客户分类"].ToString().Trim();
                    xtcu_no= dr["上级客户编号"].ToString().Trim();
                    
                    province = dr["省份"].ToString().Trim();
                    city = dr["城市"].ToString().Trim();
                    addr= dr["详细地址"].ToString().Trim();
                    phone = dr["电话"].ToString().Trim();
                    fax = dr["传真"].ToString().Trim();
                    email= dr["Email"].ToString().Trim();
                    link_man = dr["联系人"].ToString().Trim();
                    pasword = dr["密码"].ToString().Trim();

                 var cmodel = dbHelpers.customer.FirstOrDefault(db => db.cu_no == cu_no);
                 if (cmodel == null) 
                 { 
                    customer ip = new customer();                                        
                    ip.cu_no=   cu_no;
                    ip.cu_name = cuname;
                    ip.cutype=  cutype;
                    ip.xtcu_no= xtcu_no;                    
                    ip.province=province;
                    ip.city = city ;
                    ip.addr= addr;
                    ip.phone =  phone;
                    ip.fax = fax;
                    ip.email=email;
                    ip.link_man = link_man;
                    ip.passwd = Uties.CMD5(pasword);
                    ip.unitcode = "";
                    dbHelpers.customer.Add(ip);
                    i++;
                 }
                }
            }
            catch
            {
                return 0;
            }
            dbHelpers.SaveChanges();
            return i;
        }

        public int InsetVen(System.Data.DataTable dt)
        {

            int i = 0;
            string cu_no = "";
            string cuname = "";
           
            string province = "";
            string city = "";
            string addr = "";
            string phone = "";
            string fax = "";            
            string link_man = "";
            
            ConnectionStringEntities dbHelpers = new ConnectionStringEntities();

            try
            {
                foreach (DataRow dr in dt.Rows)
                {

                    cu_no = dr["客户编号"].ToString().Trim();
                    cuname = dr["客户名称"].ToString().Trim();                    
                    province = dr["省份"].ToString().Trim();
                    city = dr["城市"].ToString().Trim();
                    addr = dr["详细地址"].ToString().Trim();
                    phone = dr["电话"].ToString().Trim();
                    fax = dr["传真"].ToString().Trim();
                  
                    link_man = dr["联系人"].ToString().Trim();
                    

                    var cmodel = dbHelpers.vender.FirstOrDefault(db => db.cu_no == cu_no);
                    if (cmodel == null)
                    {
                        vender ip = new vender();
                        ip.cu_no = cu_no;
                        ip.cu_name = cuname;                       
                        ip.province = province;
                        ip.city = city;
                        ip.addr = addr;
                        ip.phone = phone;
                        ip.fax = fax;                      
                        ip.link_man = link_man;                       
                        ip.unitcode = "";
                        dbHelpers.vender.Add(ip);
                        i++;
                    }
                }
            }
            catch
            {
                return 0;
            }
            dbHelpers.SaveChanges();
            return i;
        }
    }
}