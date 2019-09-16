using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO;
using System.Text;
using UI.hmtools;
using System.IO;
using marr.BusinessRule;
using marr.DataLayer;
using marr.BusinessRule.Entity;

namespace UI
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class appUpfileIn : IHttpHandler
    {
      

        public void ProcessRequest(HttpContext context)
        {
            string cu_no = context.Request["cu_no"];
            string filename = context.Request["filename"];


            Log.Error("upfilein", filename);


            cu_no = cu_no.Trim();

            if (cu_no.Length == 0)
                cu_no = "my888";


            if (filename == null || filename.Length == 0)
                filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";


            String path = System.Web.Hosting.HostingEnvironment.MapPath("~");



            context.Response.ContentType = "text/plain; charset=utf-8";
            System.Web.HttpFileCollection _files = System.Web.HttpContext.Current.Request.Files;

            for (System.Int32 _iFile = 0; _iFile < _files.Count; _iFile++)
            {

                try
                {
                    System.Web.HttpPostedFile _postedFile = _files[_iFile];
                    System.String _fileName, _fileExtension;

                    _fileName = System.IO.Path.GetFileName(_postedFile.FileName);



                    _fileExtension = System.IO.Path.GetExtension(_fileName);
                    string forderPathStr;


                    forderPathStr = System.Web.HttpContext.Current.Request.MapPath("/ptempfile/" + cu_no + "/");


                    if (!Directory.Exists(forderPathStr))
                    {
                        Directory.CreateDirectory(forderPathStr);
                    }



                    Random ran2 = new Random();

                    int myint2 = ran2.Next(1000, 9999);

                    string ranstr = Convert.ToString(myint2);

                    string pack_no = filename.Substring(0, 1) + DateTime.Now.ToString("yyMMddhhmmss") + ranstr;


                    string myfilename = pack_no + "_" + filename.Trim();



                    string savefile = forderPathStr + myfilename;
                    _postedFile.SaveAs(savefile);

                    //保存记录
                    //处理记录
                    using (brSalPack br = new brSalPack())
                    {

                        sal_packa en = new sal_packa();
                        en.pack_no = pack_no;
                        en.pfilename = myfilename;
                        en.type = pack_no.Substring(0, 1);
                        en.absolutepath = savefile;
                        en.pack_date =DateTime.Now;
                        en.add_date = DateTime.Now;
                        en.status = 0;
                        en.uplog = "文件已上传，数据待导入";
                        en.okqty = 0;
                        en.errqty = 0;
                        en.cu_no = cu_no;

                        br.Insert(en);
                    }

                    if (pack_no.Substring(0, 1).ToUpper() == "P")
                    {                       
                        string sql = "exec psal_pack_cl '" + savefile + "','" + pack_no + "','" + pack_no.Substring(0, 1) + "','" + DateTime.Now + "','"+cu_no+"'";
                        Log.Debug("salpack", sql);

                        DbHelperSQL.ExecuteSql(sql);

                        String backpath = path + "\\backfile" + "\\";


                        if (!Directory.Exists(backpath))
                        {
                            Directory.CreateDirectory(backpath);
                        }


                        System.IO.File.Move(savefile, backpath + myfilename);  //移动文件

                        context.Response.Write("OK");


                    }

                    else
                    {

                        //       string savefile = forderPathStr + filename ;
                        //       _postedFile.SaveAs(savefile);


                        //保存记录
                        //处理记录

                        string strSql3 = "insert into xt_uplogs(cu_no,filename,status)  values('" + cu_no + "','" + savefile + "',0) ";
                        int i = DbHelperSQL.ExecuteSql(strSql3);

                        //   context.Response.Write("OK");


                        //   svc.procuplog(savefile, cu_no);  //启动处理程序



                        SaveClFiles(savefile, cu_no); //及时处理

                        context.Response.Write("OK");
                    }

                }

                catch (Exception ex)
                {

                    Log.Error("upfilein", ex.Message);

                }
            }




        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        protected void SaveClFiles(string inmyfilename, string incu_no)
        {



            //开始处理
            Log.Debug("appup", "开始处理" + inmyfilename);

            using (brUplogs brup = new brUplogs())
            {

                try
                {

                    string strmyfile = inmyfilename;
                    string mycu_no = incu_no;
                    string st_no = incu_no;

                    if (strmyfile == null || strmyfile.Length == 0)
                        return;

                    if (mycu_no == "my888")
                        mycu_no = "";

                    //判断文件是否存在

                    if (!System.IO.File.Exists(strmyfile))
                    {
                        string strSql14 = "update xt_uplogs set  status=9,uplogs='文件早期已经处理完毕'   where filename='" + strmyfile + "' ";
                        int i12 = DbHelperSQL.ExecuteSql(strSql14);

                        return;
                    }




                    string strSql4 = "update xt_uplogs set  status=1   where filename='" + strmyfile + "' ";
                    int i2 = DbHelperSQL.ExecuteSql(strSql4);



                    String path = System.Web.Hosting.HostingEnvironment.MapPath("~");

                    //  读文件


                    String tmppath = strmyfile;


                    int hh = strmyfile.LastIndexOf("\\");
                    string btfile = strmyfile.Substring(hh + 1, strmyfile.Length - hh - 1).ToUpper();

                    String FileName = btfile.Substring(0, 1) + System.DateTime.Now.ToString("yyMMddhhmmss") + ".txt";



                    String backpath = path + "\\backfile" + "\\";


                    if (!Directory.Exists(backpath))
                    {
                        Directory.CreateDirectory(backpath);
                    }




                    StringBuilder infoBuilder = new StringBuilder();
                    int successedCnt = 0;
                    int errqty = 0;


                    brShipScan brship = new brShipScan();
                    brCuShipScan brcuship = new brCuShipScan();
                    brFgiScan brfgi = new brFgiScan();

                    using (StreamReader sr = new StreamReader(tmppath, System.Text.Encoding.GetEncoding("gb2312")))
                    {


                        infoBuilder.Append(string.Format("开始导入时间：{0} ,", System.DateTime.Now));


                        string line;
                        int pqty = 1;
                        string ship_no = btfile.Substring(0, btfile.LastIndexOf(".")).ToUpper();

                        int dxint = 0;
                        string dxstrm = "";
                        string tmpst_no = "";
                        string tmpp_no = "";
                        string tmplot_no = "";
                        string tmpship_date = "";


                        while ((line = sr.ReadLine()) != null)
                        {
                            try
                            {
                                if (line.Length < 5)
                                {
                                    continue;
                                }

                                string[] arrsn = line.Split(',');


                                if (arrsn.Length >= 2)
                                {
                                    try
                                    {
                                        if (arrsn[0].Trim().Length < 7)
                                        {
                                            errqty++;
                                            continue;
                                        }

                                        pqty = 1;

                                        
                                       Barcode barcode = null;
                                       string pno = "";
                                     
                                       using (brBarcode br = new brBarcode())
                                       {                                               
                                           barcode = br.GetBarcodeEntity(arrsn[0].Trim(), "");
                                           if (barcode != null)
                                           {
                                               pqty = barcode.Qty;
                                               pno = barcode.p_no;
                                           }

                                           using (brFgiScan bf = new brFgiScan())
                                           {
                                               sal_fgi fgi = bf.Retrieve(arrsn[0].Trim());
                                               if (fgi != null)
                                               {
                                                   pno = fgi.p_no;
                                               }
                                           }
                                           if (pqty == 0 && ship_no.Substring(0, 1) == "S")
                                           {
                                               using (brFgiScan brp = new brFgiScan())
                                               {
                                                   sal_fgi fgi = brp.Retrieve(arrsn[0].Trim());
                                                   if (fgi != null)
                                                   {
                                                       pqty = Convert.ToInt32(fgi.mqty);

                                                   }
                                                   else
                                                   {
                                                       errqty++;
                                                       continue;
                                                   }
                                               }
                                           }
                                     
                                       }
                                        


                                        if (successedCnt == 0 && (ship_no.Substring(0, 1) == "P" || ship_no.Substring(0, 1) == "Y"))
                                        {
                                            dxstrm = arrsn[1].Trim();
                                        }

                                        string strsql = "";



                                        if (ship_no.Substring(0, 1) == "P") //包装
                                        {
                                            //snno, usnno, p_no, pack_no, lot_no, line_no
                                            using (brPackb brp = new brPackb())
                                            {
                                                sal_packb en = brp.Retrieve(arrsn[0].Trim());
                                                if (en != null)
                                                {
                                                    errqty++;

                                                }
                                                else
                                                {
                                                    if (arrsn.Length >= 6)

                                                        strsql = "insert into sal_packb(snno,usnno,p_no,pack_no,lot_no,line_no,in_date)  values('" + arrsn[0].Trim() + "','" + arrsn[1].Trim() + "','" + arrsn[2].Trim() + "','" + arrsn[3].Trim() + "','" + arrsn[4].Trim() + "','" + arrsn[5].Trim() + "','"+DateTime.Now+"')";
                                                    else
                                                        strsql = "insert into sal_packb(pack_no,snno,usnno)  values('" + mycu_no + "','" + arrsn[0].Trim() + "','" + arrsn[1].Trim() + "')";


                                                    int ii = DbHelperSQL.ExecuteSql(strsql);
                                                    successedCnt++;
                                                }
                                            }


                                        }

                                        else if (ship_no.Substring(0, 1) == "S") //出货
                                        {
                                                string p_no = pno;
                                                string pname = "";
                                                if (!string.IsNullOrEmpty(arrsn[2]))
                                                {
                                                    if(arrsn[2] != p_no)
                                                    {
                                                        errqty++;
                                                        continue;
                                                    }
                                                }                                                    

                                                 if (brship.reisship(arrsn[0].Trim()) == false)
                                                 {                                                                                                                                                        
                                                     //189075672609930,202,1903,2019-3-13 16:13,,SH2019031316,
                                                     if (arrsn.Length >= 7)
                                                         strsql = "insert into sal_ship(upyn,ship_no,bsnno,cu_no,p_no,pname,ship_date,mqty,st_no,pda_no) values('N','" + arrsn[5] + "','" + arrsn[0].Trim() + "','" + arrsn[1].Trim() + "','" + p_no.Trim() + "','" + pname.Trim() + "','" + DateTime.Parse(arrsn[3].Trim()) + "','" + pqty.ToString() + "','','')";
                                                     else
                                                         strsql = "insert into sal_ship(upyn,ship_no,bsnno,cu_no,p_no,ship_date,mqty) values('N','" + arrsn[5] + "','" + arrsn[0].Trim() + "','" + arrsn[1].Trim() + "','" + arrsn[2].Trim() + "','" + DateTime.Parse(arrsn[3].Trim()) + "','" + pqty.ToString() + "')";
                                                 
                                                 
                                                     //  Log.Debug("出库", strsql);
                                                 
                                                     int ii = DbHelperSQL.ExecuteSql(strsql);
                                                     successedCnt++;
                                                 }
                                                 else
                                                 {
                                                     errqty++;
                                                 }
                                            

                                        }
                                        
                                        else if (ship_no.Substring(0, 1) == "F") //入库
                                        {

                                            //6923052268225,,SBWX1003,2019-5-20 10:54,,RK2019052010,admin,
                                            if (brfgi.reisfgi(arrsn[0].Trim()) == false)
                                            {
                                                string p_name = "";
                                                int partqty = 1;
                                                using (brProduct br = new brProduct())
                                                {
                                                    Inv_Part myinvpart = br.Retrieve(arrsn[2]);
                                                    if (myinvpart != null)
                                                    {
                                                        p_name = myinvpart.pname;
                                                        partqty = (myinvpart.pqty == null ? 1 : (int)myinvpart.pqty);

                                                    }
                                                    else
                                                    {
                                                        errqty++;
                                                        continue;
                                                    }

                                                }

                                                using (brPackb bpb = new brPackb())
                                                {
                                                    sal_packb pb = bpb.Retrieves(arrsn[0]);
                                                    if (pb != null)
                                                    {
                                                        errqty++;
                                                        continue;
                                                    }
                                                }

                                                var pda_no = "";

                                                string temlSip_no = ship_no;
                                                if (arrsn.Length > 5)
                                                    temlSip_no = arrsn[5];

                                                if (arrsn.Length > 6)
                                                    pda_no = arrsn[6];

                                                //0054110,电子烟产品,7110-3601-000,2018-10-22 14:23,03,电子烟01,admin,
                                                if (arrsn.Length > 4)
                                                    strsql = "insert into sal_fgi(upyn,ship_no,bsnno,lot_no,p_no,ship_date,mqty,st_no,pda_no) values('N','" + temlSip_no + "','" + arrsn[0].Trim() + "','" + arrsn[1].Trim() + "','" + arrsn[2].Trim() + "','" + DateTime.Parse(arrsn[3].Trim()) + "','" + pqty.ToString() + "','" + arrsn[4].Trim() + "','"+pda_no+"')";
                                                else
                                                    strsql = "insert into sal_fgi(upyn,ship_no,bsnno,lot_no,p_no,ship_date,mqty) values('N','" + temlSip_no + "','" + arrsn[0].Trim() + "','" + arrsn[1].Trim() + "','" + arrsn[2].Trim() + "','" + DateTime.Parse(arrsn[3].Trim()) + "','" + pqty.ToString() + "')";

                                                int ii = DbHelperSQL.ExecuteSql(strsql);
                                                successedCnt++;
                                            }
                                            else
                                            {
                                                errqty++;
                                            }

                                        }

                                    }
                                    catch (Exception ex)
                                    {

                                        Log.Debug("appup", ex.Message);

                                        errqty++;


                                    }
                                }

                            }
                            catch (Exception e1)
                            {

                                Log.Debug("appup", e1.Message);

                            }
                        }



                    }

                    string tmpFileName = FileName;


                    while (System.IO.File.Exists(backpath + tmpFileName))
                    {
                        tmpFileName = tmpFileName + ".txt";
                    }

                    System.IO.File.Move(strmyfile, backpath + tmpFileName);  //移动文件


                    infoBuilder.Append(string.Format("完成导入时间：{0} , ", System.DateTime.Now));

                    infoBuilder.Append(string.Format("共导入数码：{0} 其中错误：{1}  成功导入：{2} ", successedCnt + errqty, errqty, successedCnt));


                    int tt = DbHelperSQL.ExecuteSql("update sal_ship  set cu_name=customer.cu_name  from customer where  sal_ship.cu_no=customer.cu_no and (sal_ship.cu_name ='' or sal_ship.cu_name is null )");
                    tt = DbHelperSQL.ExecuteSql("update sal_ship  set pname=Inv_Part.pname, type=Inv_Part.type   from Inv_Part where  sal_ship.p_no=Inv_Part.p_no and ( sal_ship.pname ='' or sal_ship.pname is null ) ");
                    tt = DbHelperSQL.ExecuteSql("update sal_fgi set cu_name=vender.cu_name from vender where sal_fgi.cu_no=vender.cu_no and ( sal_fgi.cu_name='' or sal_fgi.cu_name is null)");
                    tt = DbHelperSQL.ExecuteSql("update sal_fgi  set pname=Inv_Part.pname, type=Inv_Part.type   from Inv_Part where  sal_fgi.p_no=Inv_Part.p_no and ( sal_fgi.pname ='' or sal_fgi.pname is null ) ");

                    SaveLogFiles("导入资料", infoBuilder.ToString(), "", strmyfile);

                    UpdPacka(successedCnt, errqty, infoBuilder.ToString(), strmyfile);

                    return;


                }
                catch (Exception ex)
                {


                    Log.Debug("appup", ex.Message);


                }

            }

            //处理完毕


        }

        protected void UpdPacka(int okqty,int errqty, string errorContent, string myfilename)
        { 
            string mystrtmp;
            errorContent = errorContent.Replace('\'', '\"');
            if (errorContent.Length > 1000)
                mystrtmp = errorContent.Substring(0, 1000);
            else
                mystrtmp = errorContent;
            string sql = "update sal_packa set uplog='" + errorContent + "',status=9,okqty="+okqty+",errqty="+errqty+" where absolutepath='" + myfilename + "'";
           
            int i2 = DbHelperSQL.ExecuteSql(sql);
        }

        protected void SaveLogFiles(string item, string errorContent, string FileName_Prefix, string myfilename)
        {
            string mystrtmp;
            errorContent = errorContent.Replace('\'', '\"');
            if (errorContent.Length > 1000)
                mystrtmp = errorContent.Substring(0, 1000);
            else
                mystrtmp = errorContent;


            string strSql4 = "update xt_uplogs set  uplogs='" + mystrtmp + "',status=9   where filename='" + myfilename + "' ";


            int i2 = DbHelperSQL.ExecuteSql(strSql4);


        }
    }
}
