using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;
using marr.BusinessRule.Entity;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace marr.BusinessRule
{
    public class brBarcode : IDisposable
    {
        string[] xtm2code1 = new string[10];
        string[] xtm2code2 = new string[10];
        string[] xtm2code3 = new string[10];

        public brBarcode()
        {
            db = new dbDataContext(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);

            string xtukeyID = System.Configuration.ConfigurationSettings.AppSettings["xtukeyID"];
            string keytmp;
            string keytmp2;
            string keytmp3;


            if (xtukeyID == null || xtukeyID.Length < 19)
            {
                keytmp = "9741382605";
            }
            else
            {
                keytmp = xtukeyID.Substring(8, 10);
            }

            keytmp2 = keytmp.Substring(7, 3) + keytmp.Substring(3, 4) + keytmp.Substring(0, 3);

            keytmp3 = keytmp.Substring(3, 3) + keytmp.Substring(0, 3) + keytmp.Substring(6, 4);

            for (int nnn = 0; nnn < 10; nnn++)
            {

                xtm2code1[Convert.ToInt32(keytmp.Substring(nnn, 1))] = nnn.ToString();
                xtm2code2[Convert.ToInt32(keytmp2.Substring(nnn, 1))] = nnn.ToString();
                xtm2code3[Convert.ToInt32(keytmp3.Substring(nnn, 1))] = nnn.ToString();


            }


        }

        private dbDataContext db;

        public void Dispose()
        {
            db.Dispose();
            db = null;
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        public bool IsBarcodeValid(string barcode)
        {
            if (barcode != null)
            {
                if (barcode.Trim().Length == 10) return true;
                if (barcode.Trim().Length == 11) return true;
                return false;
            }
            return false;
        }




        //计算物流码
        public Barcode GetBarcodeEntity(string inbarcode, string inunitcode)
        {

            if (inbarcode==null || inbarcode.Length==0)
            {
                return null;
            }


            Boolean findok = false;
            Barcode bcode = new Barcode();

            //如是导入
            sal_packb packb = db.sal_packb.Where(x => x.usnno == inbarcode).OrderByDescending(z => z.fid).FirstOrDefault();
            if (packb != null)
            {
                Barcode bcode3 = new Barcode();

                bcode3.Code = inbarcode;
                bcode3.Tcode = "";
                bcode3.Ucode = "";

                bcode3.p_no = packb.p_no;

                bcode3.packtype = "2";

                //如果是箱标
                sal_packb ddb2 = db.sal_packb.FirstOrDefault(x => x.snno == inbarcode);
                if (ddb2 != null)
                {
                    bcode3.Tcode = ddb2.usnno;
                    bcode3.Ucode = ddb2.usnno;
                }

                bcode3.Qty = db.sal_packb.Where(x => x.usnno == inbarcode).Count();

                //修正跺标
                int pxqty = db.sal_packb.Where(x => x.usnno == packb.snno).Count();

                if (pxqty > 1)
                {
                    bcode3.packtype = "3";
                    bcode3.Qty = bcode3.Qty * pxqty;
                }

                return bcode3;

            }

            sal_fgi fgi = db.sal_fgi.Where(x => x.bsnno == inbarcode).OrderByDescending(z => z.fid).FirstOrDefault();
            if (fgi != null)
            {
                Barcode bcode3 = new Barcode();

                bcode3.Code = inbarcode;
                bcode3.Tcode = "";
                bcode3.Ucode = "";

                bcode3.p_no = fgi.p_no;

                bcode3.packtype = "2";



                bcode3.Qty = (fgi.mqty ==null ? 1 : (int)fgi.mqty );
                

                return bcode3;

            }

            //}

            //小标
            sal_packb dbEntity = db.sal_packb.FirstOrDefault(x => x.snno == inbarcode);
            if (dbEntity != null)
            {
                Barcode bcode2 = new Barcode(dbEntity);

                if (bcode2.Ucode != null && bcode2.Ucode != "")
                {
                    sal_packb ddb2 = db.sal_packb.FirstOrDefault(x => x.snno == bcode2.Ucode);
                    if (ddb2 != null)
                    {
                        bcode2.Tcode = ddb2.usnno;
                    }
                }                
                bcode2.Qty = 1;
                bcode2.packtype = "1";
                return bcode2;
            }



            IQueryable<dhm_cust> ret = db.dhm_cust.Where(x => x.wlLen == inbarcode.Length);

            foreach (var item in ret)
            {
                string snpr = item.snpr.Trim();
                inunitcode = item.UnitCode.Trim();


                //需要修改

                long fwk = sncodetok(inbarcode);


                 bcode = sntosntype(inunitcode, fwk, inbarcode, (int)item.codeLen );


                if (bcode != null && bcode.Qty > 0)
                {
                    bcode.unitcode = item.UnitCode;
                    findok = true;
                    return bcode;

                }


            }

            return bcode;




        }


        public Barcode sntosntype(string inunitcode, long myk,  string insnno, int inmycodelen)
        {
            Barcode bcode = new Barcode();
            bcode.Code = insnno;
            bcode.pindex = 0;
            long dkkk;
            long zkkk;

            int mysnlen = insnno.Length;   //物流码原始长度

            string mysnpr = "";//前缀

            Boolean findsell = false;   //'是否有发行
      
            int mmlength = inmycodelen;


            dhm_fwout dhm_fwout;

            dhm_fwout = db.dhm_fwout.FirstOrDefault(x => x.UnitCode == inunitcode && x.myBegin <= myk && (x.myBegin + x.SellCount) > myk);


            if (dhm_fwout != null)
            {

                string packtype = dhm_fwout.packtype.Substring(0, 1);
                int pdqty = (int)dhm_fwout.pdqty;
                int pzqty = (int)dhm_fwout.pzqty;
                int pxqty = (int)dhm_fwout.pxqty;
                long bkkk = (long)dhm_fwout.myBegin;


                string snsnbegin = dhm_fwout.snbegin.Trim();
                int mqty = Convert.ToInt32(dhm_fwout.mqty);

                if (packtype == "0")
                    return null;

                int inttmp = snsnbegin.Length;


                if (packtype == "1")
                    {
                        bcode.Qty = 1;

                    }

                    if (packtype == "2")
                    {
                        dkkk = bkkk + (pxqty + 1) * ((myk - bkkk) / (pxqty + 1)); //大箱k
                        if (dkkk == myk) //'大箱
                        {
                            bcode.Qty = pxqty;
                            bcode.pindex = 0;
                        }
                        else
                        {
                            bcode.Qty = 1;
                            bcode.Tcode = mysnpr + ktosncode(dkkk, inunitcode, mysnlen);
                            bcode.Ucode = bcode.Tcode;
                            bcode.pindex = myk - dkkk;
                        }

                    }


                    if (packtype == "3")
                    {

                        dkkk = bkkk + (pxqty * pzqty + pzqty + 1) * ((myk - bkkk) / (pxqty * pzqty + pzqty + 1)); //'大箱k
                        zkkk = dkkk + 1 + ((myk - dkkk - 1) / (pxqty + 1)) * (pxqty + 1);

                        if (dkkk == myk) //'大箱
                        {
                            bcode.Qty = pxqty * pzqty;
                            bcode.pindex = 0;
                        }

                        else  //  '中箱
                        {
                            if (zkkk == myk)
                            {
                                bcode.Qty = pxqty;
                                bcode.Tcode = mysnpr + ktosncode(dkkk, inunitcode, mysnlen);
                                bcode.Ucode = mysnpr + ktosncode(dkkk, inunitcode, mysnlen);
                                bcode.pindex = 0;
                            }
                            else
                            {
                                bcode.Qty = 1;
                                bcode.Tcode = mysnpr + ktosncode(zkkk, inunitcode, mysnlen);
                                bcode.Ucode = mysnpr + ktosncode(dkkk, inunitcode, mysnlen);
                                bcode.pindex = myk - zkkk;

                            }

                        }

                    }

     

            }
            else
            {
                return null;
            }



            return bcode;

        }

        public long sncodetok(string inwlcode)   // sncode to k
        {

            //   计算的jswllen--mmlength
            int mmlength = jswllen(inwlcode.Length);



            //找出验证码    
            int intvlen;
            //求计算验证码是否正确
            
            int m = db.dhm_snmm.Where(x => x.codelen == mmlength).Count();

            int ai = 1, bi = 1, ci = 1;


            if (mmlength < 8 || mmlength > 20)
                return 0;

            switch (mmlength)
            {

                case 9:
                    ai = 3;
                    bi = 3;
                    ci = 3;
                    break;
                case 12:
                    ai = 4;
                    bi = 4;
                    ci = 4;
                    break;
                case 15:
                    ai = 5;
                    bi = 5;
                    ci = 5;
                    break;
                case 18:
                    ai = 6;
                    bi = 6;
                    ci = 6;
                    break;
                case 21:
                    ai = 7;
                    bi = 7;
                    ci = 7;
                    break;

            }

            int n1 = 0;
            int n2 = 0;
            int n3 = 0;



            intvlen = inwlcode.Length - mmlength;



            /*
            intvlen = wllen - mywllen
                   
                   
             If intvlen = 0 Then   '加0位验证码  cdb
                     wlstr = sncodec(An1 - 1) + sncoded(An2 - 1) + sncodeb(An3 - 1)
             End If
                  
             If intvlen = 1 Then   '加1位验证码  dac 加后面
                     wlstr = sncoded(An1 - 1) + sncodea(An2 - 1) + sncodec(An3 - 1)
                     wlstr = wlstr + fwprst(wlstr, 9, 1)
 
             End If
              
            If intvlen = 2 Then   '加2位验证码  bda 加后面
                     wlstr = sncodeb(An1 - 1) + sncoded(An2 - 1) + sncodea(An3 - 1)
                     wlstr = fwprst(wlstr, 3, 1) + wlstr
                     wlstr = wlstr + fwprst(wlstr, 8, 1)
 
             End If
              
            wlstr = codetom(wlstr)

           */




            if (intvlen == 0)  //加0位验证码  cdb
            {
                //还原
                string myinfwcode = mtocode(inwlcode);


                if (db.dhm_snmm.Where(x => x.CodeC == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n1 = (int)db.dhm_snmm.Where(x => x.CodeC == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_snmm.Where(x => x.CodeD == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n2 = (int)db.dhm_snmm.Where(x => x.CodeD == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_snmm.Where(x => x.CodeB == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n3 = (int)db.dhm_snmm.Where(x => x.CodeB == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();
            }


            if (intvlen == 1)  //加1位验证码  dac 加后面
            {

                string myinfwcode = mtocode(inwlcode.Substring(0,inwlcode.Length-1));

                if (db.dhm_snmm.Where(x => x.CodeD == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n1 = (int)db.dhm_snmm.Where(x => x.CodeD == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_snmm.Where(x => x.CodeA == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n2 = (int)db.dhm_snmm.Where(x => x.CodeA == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_snmm.Where(x => x.CodeC == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n3 = (int)db.dhm_snmm.Where(x => x.CodeC == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();


            }

            if (intvlen == 2)   //加2位验证码  bda 加后面
            {
                string myinfwcode = mtocode(inwlcode.Substring(1, inwlcode.Length - 2));

                if (db.dhm_snmm.Where(x => x.CodeB == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n1 = (int)db.dhm_snmm.Where(x => x.CodeB == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_snmm.Where(x => x.CodeD == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n2 = (int)db.dhm_snmm.Where(x => x.CodeD == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_snmm.Where(x => x.CodeA == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n3 = (int)db.dhm_snmm.Where(x => x.CodeA == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

            }

            if (n1 == 0 || n2 == 0 || n3 == 0)
                return 0;

            int p = n2 - n1;
            if (p < 0) p = m + n2 - n1;
            int q = n3 - n1;

            if (q < 0) q = m + n3 - n1;

            int k = n1 + m * p + m * m * q;

            return k;

        }


        public string ktosncode(long ink, string inunitcode, int intmm)   //k to sncode
        {

            //   计算的jswllen--mmlength
            int mmlength = jswllen(intmm);

            int m = db.dhm_snmm.Where(x => x.codelen == mmlength).Count();

            if (ink == 0 || m == 0)
                return "";



            string[] codea = db.dhm_snmm.Where(x => x.codelen == mmlength).OrderBy(x => x.Address).Select(x => x.CodeA).ToArray();
            string[] codeb = db.dhm_snmm.Where(x => x.codelen == mmlength).OrderBy(x => x.Address).Select(x => x.CodeB).ToArray();
            string[] codec = db.dhm_snmm.Where(x => x.codelen == mmlength).OrderBy(x => x.Address).Select(x => x.CodeC).ToArray();
            string[] coded = db.dhm_snmm.Where(x => x.codelen == mmlength).OrderBy(x => x.Address).Select(x => x.CodeD).ToArray();

            long q3 = (ink - 1) / (m * m);

            long p2 = (ink - 1 - (m * m * q3)) / m;

            long n1 = ink - p2 * m - q3 * m * m;
            long n2 = n1 + p2;

            if (n2 > m)
                n2 = n2 - m;

            long n3 = n1 + q3;

            if (n3 > m)
                n3 = n3 - m;

            string wlstr = "";


            int intvlen = intmm - mmlength;


            if (intvlen == 0)  //加0位验证码  cdb
            {
                wlstr = codec[n1 - 1].Trim() + coded[n2 - 1].Trim() + codeb[n3 - 1].Trim();
            }




            if (intvlen == 1)  //   '加1位验证码  dac 加后面
            {
                wlstr = coded[n1 - 1].Trim() + codea[n2 - 1].Trim() + codec[n3 - 1].Trim();
                wlstr = wlstr + fwprst(wlstr, 9, 1);

            }

            if (intvlen == 2)  //   '加2位验证码  bda 加后面
            {
                wlstr = codeb[n1 - 1].Trim() + coded[n2 - 1].Trim() + codea[n3 - 1].Trim();
                wlstr = fwprst(wlstr, 3, 1) + wlstr;
                wlstr = wlstr + fwprst(wlstr, 8, 1);

            }

            wlstr = codetom(wlstr);


            return wlstr;

        }

        //public string[] codetoarrray(int mylen,string mystr)
        //{
        //    if (mystr == "A")
        //        return db.fw_code.Where(x => x.codelen == mylen).OrderBy(x => x.Address).Select(x => x.CodeA).ToArray();
        //    else if (mystr == "B")
        //        return db.fw_code.Where(x => x.codelen == mylen).OrderBy(x => x.Address).Select(x => x.CodeB).ToArray();
        //    else if (mystr == "C")
        //        return db.fw_code.Where(x => x.codelen == mylen).OrderBy(x => x.Address).Select(x => x.CodeC).ToArray();
        //    else
        //        return null;


        //}

        //public string[] sncodetoarrray(int mylen, string mystr)
        //{
        //    if (mystr == "A")
        //        return db.fw_snmm.Where(x => x.codelen == mylen).OrderBy(x => x.Address).Select(x => x.CodeA).ToArray();
        //    else if (mystr == "B")
        //        return db.fw_snmm.Where(x => x.codelen == mylen).OrderBy(x => x.Address).Select(x => x.CodeB).ToArray();
        //    else if (mystr == "C")
        //        return db.fw_snmm.Where(x => x.codelen == mylen).OrderBy(x => x.Address).Select(x => x.CodeC).ToArray();
        //    else
        ////        return null;


        //}




        //物流码查防伪
        public Fwcode refccode(string infwcode)
        {

            Fwcode myfwcode = new Fwcode();

            string gfw0; //防伪不存在
            string gfw1; //防伪正确
            string gfw2; //防伪重复
            string gfw5;  //多次防伪重复


            using (brBasoneto brb = new brBasoneto())
            {
                gfw0 = brb.resendmess("FW0");
                gfw1 = brb.resendmess("FW1");
                gfw2 = brb.resendmess("FW2");
                gfw5 = brb.resendmess("FW5");

            }


            myfwcode.findresut = 0;
            myfwcode.p_no = "";
            myfwcode.fwmmcode = infwcode.Trim();
            myfwcode.fwmm = "";
            myfwcode.snno = "";
            myfwcode.makeline = "";
            myfwcode.unitcode = "";
            myfwcode.jf = 0;
            myfwcode.cxcs = 1;


            string packtype = "";
            int pdqty = 0;
            int pzqty = 0;
            int pxqty = 0;
            long bkkk = 0;

            string snsnbegin = "";
            int mqty = 0;


            myfwcode.resnote = gfw0;

            if (infwcode.Length < 8)
                return myfwcode;


            //判断是否全是数字
            if (!isNumeric(infwcode))
                return myfwcode;

            //判断是否导入

            Boolean oldin = false;

            int fwcodelen = 0;


            if (oldin == false)
            {


                IQueryable<dhm_cust> ret = db.dhm_cust.Where(x => x.wlLen == infwcode.Length);

                foreach (var item in ret)
                {
                    string snpr = item.snpr.Trim();
                    myfwcode.unitcode = item.UnitCode.Trim();


                    //需要修改

                    myfwcode.gkkk = sncodetok(infwcode);


                    if (myfwcode.gkkk != 0)
                    {
                        Barcode bcode = sntosntype(myfwcode.unitcode, myfwcode.gkkk, infwcode, (int)item.codeLen);


                        if (bcode != null && bcode.Qty > 0)
                        {

                            fwcodelen = (int)item.codeLen;
                            break;

                        }
                    }


                }



                //还原 code

         
                if (myfwcode.gkkk == 0)
                {
                    myfwcode.resnote = gfw0;
                    return myfwcode;
                }


                //判断是否有发行

                using (brDhmfwout brs = new brDhmfwout())
                {
                    dhm_fwout sells = brs.Retrieve(fwcodelen, myfwcode.gkkk);
                    if (sells == null)
                    {
                        myfwcode.findresut = 0;
                        myfwcode.resnote = gfw0;


                        return myfwcode;
                    }
                    else
                    {
                        myfwcode.unitcode = sells.UnitCode;

                        packtype = sells.packtype.Substring(0, 1);
                        pdqty = (int)sells.pdqty;
                        pzqty = (int)sells.pzqty;
                        pxqty = (int)sells.pxqty;
                        bkkk = (long)sells.myBegin;

                        snsnbegin = sells.snbegin.Trim();
                        mqty = Convert.ToInt32(sells.mqty);

                    }
                }

            }


            dhm_cust fwcust = db.dhm_cust.FirstOrDefault(x => x.UnitCode == myfwcode.unitcode);

            if (fwcust == null)
                return myfwcode;


            myfwcode.unitname = fwcust.UnitName;

            myfwcode.resnote = fwcust.renote.Trim();

            myfwcode.msnlen = (int)fwcust.wlLen;
            myfwcode.jf = 0;

            myfwcode.snpr = fwcust.snpr.Trim();
            myfwcode.snlen = (int)fwcust.wlLen;

            myfwcode.fwmm = fwprst(infwcode, myfwcode.gkkk, 4);



            using (brTellist brt = new brTellist())
            {
                tellist telist = brt.Retrieve(infwcode);
                if (telist != null)
                {

                    int cxcs = brt.refwCount(infwcode);

                    if (cxcs >= 3)
                        myfwcode.resnote = gfw5;
                    else
                        myfwcode.resnote = gfw2;


                    myfwcode.resnote = myfwcode.resnote.Replace("@rq@", Convert.ToString(telist.QueryDate));

                    myfwcode.resnote = myfwcode.resnote.Replace("@cs@", Convert.ToString(cxcs));

                    myfwcode.resnote = myfwcode.resnote.Replace("@nm@", myfwcode.unitname);

                    myfwcode.resnote = myfwcode.resnote.Replace("@qt@", Getcxtyp(telist.qutype.Trim()));


                    myfwcode.findresut = 2;
                    myfwcode.cxcs = cxcs;

                }
                else
                {
                    myfwcode.findresut = 1;


                }

            }


            return myfwcode;

        }



       

        // 防伪码查询
        public Fwcode refwcode(string infwcode)
        {

            Fwcode myfwcode = new Fwcode();

            string gfw0; //防伪不存在
            string gfw1; //防伪正确
            string gfw2; //防伪重复
            string gfw5;  //多次防伪重复


            using (brBasoneto brb = new brBasoneto())
            {
                gfw0 = brb.resendmess("FW0");
                gfw1 = brb.resendmess("FW1");
                gfw2 = brb.resendmess("FW2");
                gfw5 = brb.resendmess("FW5");

            }


            myfwcode.findresut = 0;
            myfwcode.p_no = "";
            myfwcode.fwmmcode = infwcode.Trim();
            myfwcode.fwmm = "";
            myfwcode.snno = "";
            myfwcode.makeline = "";
            myfwcode.unitcode = "";
            myfwcode.jf = 0;
            myfwcode.cxcs = 1;


            string packtype = "";
            int pdqty = 0;
            int pzqty = 0;
            int pxqty = 0;
            long bkkk = 0;

            string snsnbegin = "";
            int mqty = 0;


            myfwcode.resnote = gfw0;

            if (infwcode.Length < 8)
                return myfwcode;


            //判断是否全是数字
            if (!isNumeric(infwcode))
                return myfwcode;

            //判断是否导入

            Boolean oldin = false;




            if (oldin == false)
            {


                //   计算的fwlen--mmlength
                int mmlength = jsfwlen(infwcode.Length);

                // 计算m
                myfwcode.mlength = db.dhm_code.Where(x => x.codelen == mmlength).Count();


                //找出验证码    
                int intvlen;
                int intsumcode;
                string mycheckfwcode = "";
                string vcode1;
                string vcode2;
                string vcode3;



                intvlen = infwcode.Length - mmlength;

                if (intvlen == 3)   //加3位验证码  dac 两头 +中间
                {


                    vcode1 = infwcode.Substring(0, 1);

                    vcode2 = infwcode.Substring(infwcode.Length - 1, 1);




                    intsumcode = Convert.ToInt32(infwcode.Substring(0, 1)) + Convert.ToInt32(infwcode.Substring(1, 1));
                    if (intsumcode > 9)
                        intsumcode = intsumcode - 10;

                    if (intsumcode <= 2)
                        intsumcode = 6;



                    mycheckfwcode = infwcode.Substring(0, intsumcode) + infwcode.Substring(intsumcode + 1, infwcode.Length - intsumcode - 1);
                    mycheckfwcode = mycheckfwcode.Substring(1, mycheckfwcode.Length - 2);
                    vcode3 = infwcode.Substring(intsumcode, 1);


                    if (vcode1 != fwprst(mycheckfwcode, 3, 1))
                    {
                        myfwcode.resnote = gfw0;
                        return myfwcode;
                    }

                    if (vcode2 != fwprst(vcode1 + mycheckfwcode, 7, 1))
                    {
                        myfwcode.resnote = gfw0;
                        return myfwcode;
                    }

                    if (vcode3 != fwprst(vcode1 + mycheckfwcode + vcode2, 5, 1))
                    {
                        myfwcode.resnote = gfw0;
                        return myfwcode;
                    }


                }
                if (intvlen == 2)   //'加2位验证码 cba  加在最前 及中间
                {


                    vcode1 = infwcode.Substring(0, 1);
                    intsumcode = Convert.ToInt32(infwcode.Substring(0, 1)) + Convert.ToInt32(infwcode.Substring(infwcode.Length - 1, 1));
                    if (intsumcode > 9)
                        intsumcode = intsumcode - 10;

                    if (intsumcode <= 1)
                        intsumcode = 5;


                    mycheckfwcode = infwcode.Substring(0, intsumcode) + infwcode.Substring(intsumcode + 1, infwcode.Length - intsumcode - 1);
                    mycheckfwcode = mycheckfwcode.Substring(1, mycheckfwcode.Length - 1);
                    vcode2 = infwcode.Substring(intsumcode, 1);



                    if (vcode1 != fwprst(mycheckfwcode, 9, 1))
                    {
                        myfwcode.resnote = gfw0;
                        return myfwcode;
                    }

                    if (vcode2 != fwprst(vcode1 + mycheckfwcode, 5, 1))
                    {
                        myfwcode.resnote = gfw0;
                        return myfwcode;
                    }

                }
                if (intvlen == 1)   //'加1位验证码  bdc 加在最后
                {


                    vcode1 = infwcode.Substring(infwcode.Length - 1, 1);
                    mycheckfwcode = infwcode.Substring(0, infwcode.Length - 1);

                    if (vcode1 != fwprst(mycheckfwcode, 3, 1))
                    {
                        myfwcode.resnote = gfw0;
                        return myfwcode;
                    }

                }


                //还原 code

                myfwcode.gkkk = codetok(mycheckfwcode, mmlength, intvlen);

                if (myfwcode.gkkk == 0)
                {
                    myfwcode.resnote = gfw0;
                    return myfwcode;
                }


                //判断是否有发行

                using (brDhmfwout brs = new brDhmfwout())
                {
                    dhm_fwout sells = brs.Retrieve(infwcode.Length, myfwcode.gkkk);
                    if (sells == null)
                    {
                        myfwcode.findresut = 0;
                        myfwcode.resnote = gfw0;


                        return myfwcode;
                    }
                    else
                    {
                        myfwcode.unitcode = sells.UnitCode;

                        packtype = sells.packtype.Substring(0, 1);
                        pdqty = (int)sells.pdqty;
                        pzqty = (int)sells.pzqty;
                        pxqty = (int)sells.pxqty;
                        bkkk = (long)sells.myBegin;

                        snsnbegin = sells.snbegin.Trim();
                        mqty = Convert.ToInt32(sells.mqty);

                    }
                }

            }


            dhm_cust fwcust = db.dhm_cust.FirstOrDefault(x => x.UnitCode == myfwcode.unitcode);

            if (fwcust == null)
                return myfwcode;


            myfwcode.unitname = fwcust.UnitName;

            myfwcode.resnote = fwcust.renote.Trim();

            myfwcode.msnlen = (int)fwcust.wlLen;
            myfwcode.jf = 0;

            myfwcode.snpr = fwcust.snpr.Trim();
            myfwcode.snlen = (int)fwcust.wlLen;

            myfwcode.fwmm = fwprst(infwcode, myfwcode.gkkk, 4);
            //计算物流码
            if (fwcust.wlyn != null && fwcust.wlyn == 'Y')
            {
                myfwcode.code = ktosncode(myfwcode.gkkk,"",myfwcode.snlen);

            }
            else
            {
                myfwcode.code = "";
            }
            //计算流水号
            if (fwcust.snyn != null && fwcust.snyn == 'Y')
            {

                if (snsnbegin.Length != 0)
                {
                    int inttmp = snsnbegin.Length;

                    int mysnlen = snsnbegin.Length;

                    int outlen = Convert.ToString(mqty).Length;

                    if (outlen <= inttmp)
                    {

                        string strformatd;
                        string strxbfm;
                        string strzbfm;

                        outlen = Convert.ToString(Convert.ToInt32(snsnbegin.Substring(inttmp - outlen, outlen)) + mqty).Length;
                        long myk = myfwcode.gkkk;


                        long ekkk = myk - bkkk;   // 相差k


                        //这里只算单个包装的 还需要完善算期他的

                        if (outlen <= inttmp)
                        {
                            strformatd = "{0,-10:D" + Convert.ToString(outlen) + "}";
                            myfwcode.snno = snsnbegin.Substring(0, inttmp - outlen) + string.Format(strformatd, Convert.ToInt32(snsnbegin.Substring(inttmp - outlen, outlen)) + ekkk).Trim();
                        }
                        else
                        {
                            myfwcode.snno = "";
                        }

                    }
                }



            }


            using (brTellist brt = new brTellist())
            {
                tellist telist = brt.Retrieve(infwcode);
                if (telist != null)
                {

                    int cxcs = brt.refwCount(infwcode);

                    if (cxcs >= 5)
                        myfwcode.resnote = gfw5;
                    else
                        myfwcode.resnote = gfw2;


                    myfwcode.resnote = myfwcode.resnote.Replace("@rq@", Convert.ToString(telist.QueryDate));

                    myfwcode.resnote = myfwcode.resnote.Replace("@cs@", Convert.ToString(cxcs));

                    myfwcode.resnote = myfwcode.resnote.Replace("@nm@", myfwcode.unitname);

                    myfwcode.resnote = myfwcode.resnote.Replace("@qt@", Getcxtyp(telist.qutype.Trim()));


                    myfwcode.findresut = 2;
                    myfwcode.cxcs = cxcs;

                }
                else
                {
                    myfwcode.findresut = 1;


                }

            }


            return myfwcode;

        }






        public long codetok(string infwcode, int mmlength, int intop)   //fwcode to k
        {

            int m = db.dhm_code.Where(x => x.codelen == mmlength).Count();

            int ai = 1, bi = 1, ci = 1;


            if (mmlength < 8 || mmlength > 20)
                return 0;

            switch (mmlength)
            {

                case 9:
                    ai = 3;
                    bi = 3;
                    ci = 3;
                    break;
                case 12:
                    ai = 4;
                    bi = 4;
                    ci = 4;
                    break;
                case 15:
                    ai = 5;
                    bi = 5;
                    ci = 5;
                    break;
                case 18:
                    ai = 6;
                    bi = 6;
                    ci = 6;
                    break;
                case 21:
                    ai = 7;
                    bi = 7;
                    ci = 7;
                    break;

            }

            int n1 = 0;
            int n2 = 0;
            int n3 = 0;


            //还原
            string myinfwcode = fwmtocode(infwcode.Substring(0, ai), 1) + fwmtocode(infwcode.Substring(ai, bi), 2) + fwmtocode(infwcode.Substring(ai + bi, ci), 3);



            if (intop == 3)  //dac
            {
                if (db.dhm_code.Where(x => x.CodeD == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n1 = (int)db.dhm_code.Where(x => x.CodeD == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_code.Where(x => x.CodeA == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n2 = (int)db.dhm_code.Where(x => x.CodeA == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_code.Where(x => x.CodeC == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n3 = (int)db.dhm_code.Where(x => x.CodeC == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();
            }


            if (intop == 2)  //cba
            {
                if (db.dhm_code.Where(x => x.CodeC == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n1 = (int)db.dhm_code.Where(x => x.CodeC == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_code.Where(x => x.CodeB == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n2 = (int)db.dhm_code.Where(x => x.CodeB == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_code.Where(x => x.CodeA == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n3 = (int)db.dhm_code.Where(x => x.CodeA == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();
            }

            if (intop == 1)  //bdc
            {
                if (db.dhm_code.Where(x => x.CodeB == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n1 = (int)db.dhm_code.Where(x => x.CodeB == myinfwcode.Substring(0, ai) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_code.Where(x => x.CodeD == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n2 = (int)db.dhm_code.Where(x => x.CodeD == myinfwcode.Substring(ai, bi) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();

                if (db.dhm_code.Where(x => x.CodeC == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault() != null)
                    n3 = (int)db.dhm_code.Where(x => x.CodeC == myinfwcode.Substring(ai + bi, ci) && x.codelen == mmlength).Select(x => x.Address).FirstOrDefault();
            }

            if (n1 == 0 || n2 == 0 || n3 == 0)
                return 0;

            int p = n2 - n1;
            if (p < 0) p = m + n2 - n1;
            int q = n3 - n1;

            if (q < 0) q = m + n3 - n1;

            int k = n1 + m * p + m * m * q;

            return k;

        }






       


        //public Barcode fwtosntype(string infwcode, long myk, string mysntype, string mysnpr)   //防伪码to条码
        //{
        //    Barcode bcode = new Barcode();

        //    if (infwcode.Length <8 )
        //        return bcode ;




        //    int mmlength = infwcode.Length - 2;
        //    int mmsnlen = 0;

        //    long dkkk;
        //    long zkkk;
        //    int inm=0 ;



        //    Boolean findsell = false;   //'是否有发行

        //    dhm_fwout dhm_fwout = db.dhm_fwout.FirstOrDefault(x => x.codelen == (mmlength+2) && x.myBegin <= myk && (x.myBegin + x.SellCount) > myk);

        //    if (dhm_fwout != null)
        //    {
        //        string packtype = dhm_fwout.packtype.Substring(0, 1);
        //        bcode.packtype = packtype;
        //        int pdqty = (int)dhm_fwout.pdqty;
        //        int pzqty = (int)dhm_fwout.pzqty;
        //        int pxqty = (int)dhm_fwout.pxqty;
        //        long bkkk = (long)dhm_fwout.myBegin;


        //        string inunitcode = dhm_fwout.UnitCode ;

        //        fw_cust fwcust=db.fw_cust.FirstOrDefault(x=>x.UnitCode==inunitcode );
        //        if (fwcust != null)
        //            mmsnlen = (int)fwcust.snLen;


        //        string snsnbegin = dhm_fwout.snbegin.Trim();
        //        int mqty = Convert.ToInt32(dhm_fwout.mqty);

        //        if (packtype == "0")
        //            return null;


        //        int inttmp = snsnbegin.Length;


        //        if (mysntype == "1")   //一码两用
        //        {
        //            bcode.Code = infwcode;
        //            if (packtype == "1")
        //            {
        //                bcode.Qty = 1;
        //            }

        //            if (packtype == "2")
        //            {
        //                dkkk = bkkk + (pxqty + 1) * ((myk - bkkk) / (pxqty + 1)); //大箱k
        //                if (dkkk == myk) //'大箱
        //                {
        //                    bcode.Qty = pxqty;
        //                }
        //                else
        //                {
        //                    bcode.Qty = 1;
        //                    bcode.Tcode = ktocode(dkkk, inunitcode, mmlength);
        //                    bcode.Ucode = bcode.Tcode;
        //                }

        //            }

        //            if (packtype == "3")
        //            {

        //                dkkk = bkkk + (pxqty * pzqty + pzqty + 1) * ((myk - bkkk) / (pxqty * pzqty + pzqty + 1)); //'大箱k
        //                zkkk = dkkk + 1 + ((myk - dkkk - 1) / (pxqty + 1)) * (pxqty + 1);

        //                if (dkkk == myk) //'大箱
        //                {
        //                    bcode.Qty = pxqty * pzqty;
        //                }

        //                else  //  '中箱
        //                {
        //                    if (zkkk == myk)
        //                    {
        //                        bcode.Qty = pxqty;
        //                        bcode.Tcode = ktocode(dkkk, inunitcode, mmlength);
        //                        bcode.Ucode = ktocode(dkkk, inunitcode, mmlength);
        //                    }
        //                    else
        //                    {
        //                        bcode.Qty = 1;
        //                        bcode.Tcode = ktocode(zkkk, inunitcode, mmlength);
        //                        bcode.Ucode = ktocode(dkkk, inunitcode, mmlength);

        //                    }

        //                }


        //            }
        //        }

        //        else if (mysntype == "2")  //流水号
        //        {
        //            bcode.Code = mysnpr + (myk + 100000000).ToString().Substring(1, 8);
        //            if (packtype == "1")
        //            {
        //                bcode.Qty = 1;
        //            }

        //            if (packtype == "2")
        //            {

        //                bcode.Qty = 1;
        //                bcode.Ucode = mysnpr + "9" + (bkkk + ((myk - bkkk) / (pxqty)) + 10000000).ToString().Substring(1, 7);
        //                bcode.Tcode = bcode.Ucode;

        //            }

        //            if (packtype == "3") //'大中小包装
        //            {
        //                bcode.Qty = 1;
        //                bcode.Tcode = mysnpr + "8" + (bkkk + ((myk - bkkk) / (pxqty)) + 10000000).ToString().Substring(1, 7);
        //                bcode.Ucode = mysnpr + "9" + (bkkk + ((myk - bkkk) / (pxqty * pzqty)) + 10000000).ToString().Substring(1, 7);

        //            }


        //        }


        //        else if (mysntype == "3")   //乱码
        //        {
        //            bcode.Code = mysnpr + ktosncode(myk, inunitcode, mmsnlen);
        //            if (packtype == "1")
        //            {
        //                bcode.Qty = 1;

        //            }

        //            if (packtype == "2")
        //            {
        //                dkkk = bkkk + (pxqty + 1) * ((myk - bkkk) / (pxqty + 1)); //大箱k
        //                if (dkkk == myk) //'大箱
        //                {
        //                    bcode.Qty = pxqty;
        //                }
        //                else
        //                {
        //                    bcode.Qty = 1;
        //                    bcode.Tcode = mysnpr + ktosncode(dkkk, inunitcode, mmsnlen);
        //                    bcode.Ucode = bcode.Tcode;
        //                }

        //            }

        //            if (packtype == "3")
        //            {

        //                dkkk = bkkk + (pxqty * pzqty + pzqty + 1) * ((myk - bkkk) / (pxqty * pzqty + pzqty + 1)); //'大箱k
        //                zkkk = dkkk + 1 + ((myk - dkkk - 1) / (pxqty + 1)) * (pxqty + 1);

        //                if (dkkk == myk) //'大箱
        //                {
        //                    bcode.Qty = pxqty * pzqty;
        //                }

        //                else  //  '中箱
        //                {
        //                    if (zkkk == myk)
        //                    {
        //                        bcode.Qty = pxqty;
        //                        bcode.Tcode = mysnpr + ktosncode(dkkk, inunitcode, mmsnlen);
        //                        bcode.Ucode = mysnpr + ktosncode(dkkk, inunitcode, mmsnlen);
        //                    }
        //                    else
        //                    {
        //                        bcode.Qty = 1;
        //                        bcode.Tcode = mysnpr + ktosncode(zkkk, inunitcode, mmsnlen);
        //                        bcode.Ucode = mysnpr + ktosncode(dkkk, inunitcode, mmsnlen);

        //                    }

        //                }

        //            }

        //        }

        //        else if (mysntype == "4")   //流水号2 
        //        {
        //            if (snsnbegin.Length == 0)
        //                return bcode;

        //            int mysnlen = snsnbegin.Length;

        //            int outlen = Convert.ToString(mqty).Length;



        //            if (outlen > inttmp)
        //                return bcode;

        //            string strformatd;
        //            string strxbfm;
        //            string strzbfm;

        //            outlen = Convert.ToString(Convert.ToInt32(snsnbegin.Substring(inttmp - outlen, outlen)) + mqty).Length;


        //            long ekkk = myk - bkkk;   // 相差k

        //            if (outlen <= inttmp)
        //            {
        //                strformatd = "{0,-10:D" + Convert.ToString(outlen) + "}";
        //                bcode.Code = snsnbegin.Substring(0, inttmp - outlen) + string.Format(strformatd, Convert.ToInt32(snsnbegin.Substring(inttmp - outlen, outlen)) + ekkk).Trim();
        //            }
        //            else
        //            {
        //                return bcode;
        //            }

        //            if (packtype == "1")
        //            {
        //                bcode.Qty = 1;
        //            }

        //            if (packtype == "2")
        //            {
        //                if (pxqty == 0)
        //                    return bcode;


        //                strxbfm = "{0,-10:D" + Convert.ToString(Convert.ToString(pxqty).Length) + "}";

        //                int dxsl = Convert.ToInt32(ekkk / (pxqty)); // 大箱数量

        //                dkkk = bkkk + (pxqty) * dxsl;  // 大箱k

        //                string dxsnno = snsnbegin.Substring(0, inttmp - outlen) + string.Format(strformatd, Convert.ToInt32(snsnbegin.Substring(inttmp - outlen, outlen)) + dxsl).Trim();  // '大箱snno

        //                bcode.Code = dxsnno.Trim() + string.Format(strxbfm, myk - dkkk + 1);


        //                bcode.Qty = 1;
        //                bcode.Tcode = dxsnno;
        //                bcode.Ucode = dxsnno;


        //            }


        //            if (packtype == "3")
        //            {
        //                if (pxqty == 0 || pzqty == 0)
        //                    return bcode;

        //                strxbfm = "{0,-10:D" + Convert.ToString(Convert.ToString(pxqty).Length) + "}";
        //                strzbfm = "{0,-10:D" + Convert.ToString(Convert.ToString(pzqty).Length) + "}";


        //                int dxsl = Convert.ToInt32(ekkk / (pxqty * pzqty)); // 大箱数量

        //                dkkk = bkkk + (pxqty * pzqty) * dxsl; //大箱k

        //                long zxsl = Convert.ToInt32((myk - dkkk) / pxqty);

        //                long xxsl = (myk - dkkk - pxqty * zxsl);

        //                string dxsnno = snsnbegin.Substring(0, inttmp - outlen) + string.Format(strformatd, Convert.ToInt32(snsnbegin.Substring(inttmp - outlen, outlen)) + dxsl).Trim();  // '大箱snno

        //                bcode.Code = dxsnno.Trim() + string.Format(strzbfm, zxsl + 1).Trim() + string.Format(strxbfm, xxsl + 1).Trim();
        //                bcode.Tcode = dxsnno.Trim() + string.Format(strzbfm, zxsl + 1).Trim();
        //                bcode.Ucode = dxsnno;


        //            }

        //        }

        //        else
        //        {
        //            return null;
        //        }

        //    }

        //    return bcode;

        //}

        public void addsave()
        {
            db.SubmitChanges();
        }


        protected string Getcxtyp(string status)
        {
            switch (status)
            {
                case "1":
                    return "电话";
                case "2":
                    return "短信";
                case "3":
                    return "网络";
                case "4":
                    return "二维码扫描";
                case "5":
                    return "微信";
                default:
                    return "--";
            }
        }

        public string fwprst(string myfwcode, long myk, int subint)
        {
            if (myfwcode.Length < 8)
                return "";



            long lngtmp = 0;

            if (subint == 2)
            {

                //     lngtmp = myk + 1359771 + Mid(myfwcode, 1, 1) * 106 + Mid(myfwcode, 3, 1) * 35 + Mid(myfwcode, 5, 1) * 9 + Mid(myfwcode, 7, 1) * 3 + Mid(myfwcode, 8, 1) * 123456


                lngtmp = myk + 1359771 + Convert.ToInt32(myfwcode.Substring(0, 1)) * 106 + Convert.ToInt32(myfwcode.Substring(2, 1)) * 35 + Convert.ToInt32(myfwcode.Substring(4, 1)) * 9 + Convert.ToInt32(myfwcode.Substring(6, 1)) * 3 + Convert.ToInt32(myfwcode.Substring(8, 1)) * 123456;


            }
            else
            {




                //lngtmp = myk + 16123457 + Val(Mid(mystrtmp, 1, 1)) * 1216 + Val(Mid(mystrtmp, 2, 1)) * 515 + Val(Mid(mystrtmp, 3, 1)) * 52356 + Val(Mid(mystrtmp, 4, 1)) * 323 + Val(Mid(mystrtmp, 5, 1)) * 815 + Val(Mid(mystrtmp, 6, 1)) * 3226 + Val(Mid(mystrtmp, 7, 1)) * 73239
                //lngtmp = lngtmp + Val(Mid(mystrtmp, 8, 1)) * 3226 + Val(Mid(mystrtmp, 9, 1)) * 6129 + Val(Mid(mystrtmp, 10, 1)) * 568 + Val(Mid(mystrtmp, 11, 1)) * 5372 + Val(Mid(mystrtmp, 12, 1)) * 1334 + Val(Mid(mystrtmp, 13, 1)) * 3214 + Val(Mid(mystrtmp, 14, 1)) * 1347


                lngtmp = myk + 16123457 + Convert.ToInt32(myfwcode.Substring(0, 1)) * 1216 + Convert.ToInt32(myfwcode.Substring(1, 1)) * 515 + Convert.ToInt32(myfwcode.Substring(2, 1)) * 52356 + Convert.ToInt32(myfwcode.Substring(3, 1)) * 323 + Convert.ToInt32(myfwcode.Substring(4, 1)) * 815 + Convert.ToInt32(myfwcode.Substring(5, 1)) * 3226 + Convert.ToInt32(myfwcode.Substring(6, 1)) * 73239;



            }

            lngtmp = lngtmp + Convert.ToInt32(myfwcode.Substring(7, 1)) * 3226;

            if (myfwcode.Length >= 9)
                lngtmp = lngtmp + Convert.ToInt32(myfwcode.Substring(8, 1)) * 6129;

            if (myfwcode.Length >= 10)
                lngtmp = lngtmp + Convert.ToInt32(myfwcode.Substring(9, 1)) * 568;


            if (myfwcode.Length >= 11)
                lngtmp = lngtmp + Convert.ToInt32(myfwcode.Substring(10, 1)) * 5372;


            if (myfwcode.Length >= 12)
                lngtmp = lngtmp + Convert.ToInt32(myfwcode.Substring(11, 1)) * 1334;


            if (myfwcode.Length >= 13)
                lngtmp = lngtmp + Convert.ToInt32(myfwcode.Substring(12, 1)) * 3214;


            if (myfwcode.Length >= 14)
                lngtmp = lngtmp + Convert.ToInt32(myfwcode.Substring(13, 1)) * 1347;


            string fwprst = Convert.ToString(lngtmp);
            if (subint <= 8)
                return fwprst.Substring(fwprst.Length - subint, subint);
            else
                return fwprst.Substring(fwprst.Length - 8, 8);



        }


        //记算防伪码长度
        public int jsfwlen(int incodelen)
        {
            return Convert.ToInt32((incodelen - 1) / 3) * 3;
        }

        //记算物流码长度
        public int jswllen(int incodelen)
        {
            return Convert.ToInt32((incodelen) / 3) * 3;
        }



        public string fwmtocode(string incode, int op)
        {
            string retfwmcode = "";

            string strtmp = "";


            for (int t = 0; t < incode.Length; t++)
            {
                if (op == 1)
                    strtmp = xtm2code1[Convert.ToInt32(incode.Substring(t, 1))];

                if (op == 2)
                    strtmp = xtm2code2[Convert.ToInt32(incode.Substring(t, 1))];

                if (op == 3)
                    strtmp = xtm2code3[Convert.ToInt32(incode.Substring(t, 1))];

                retfwmcode = retfwmcode + strtmp;
            }

            return retfwmcode;

        }

        protected bool isNumeric(string message)
        {

            if (message != "" && Regex.IsMatch(message, @"^[0-9]*$"))
            {

                //成功

                return true;
            }

            else

                //失败
                return false;

        }

        protected string mtocode(string incode)
        {     //0123456789
            //9741382605

            string strtmp = "";

            for (int T = 0; T < incode.Length; T++)
            {

                switch (incode.Substring(T, 1))
                {
                    case "0":
                        strtmp += "9";
                        break;
                    case "1":
                        strtmp += "7";
                        break;
                    case "2":
                        strtmp += "4";
                        break;
                    case "3":
                        strtmp += "1";
                        break;
                    case "4":
                        strtmp += "3";
                        break;
                    case "5":
                        strtmp += "8";
                        break;
                    case "6":
                        strtmp += "2";
                        break;
                    case "7":
                        strtmp += "6";
                        break;
                    case "8":
                        strtmp += "0";
                        break;
                    case "9":
                        strtmp += "5";
                        break;
                }
            }

            return strtmp;


        }


        protected string codetom(string incode)
        {
            //9741382605
            //0123456789

            string strtmp = "";

            for (int T = 0; T < incode.Length; T++)
            {

                switch (incode.Substring(T, 1))
                {
                    case "0":
                        strtmp += "8";
                        break;
                    case "1":
                        strtmp += "3";
                        break;
                    case "2":
                        strtmp += "6";
                        break;
                    case "3":
                        strtmp += "4";
                        break;
                    case "4":
                        strtmp += "2";
                        break;
                    case "5":
                        strtmp += "9";
                        break;
                    case "6":
                        strtmp += "7";
                        break;
                    case "7":
                        strtmp += "1";
                        break;
                    case "8":
                        strtmp += "5";
                        break;
                    case "9":
                        strtmp += "0";
                        break;
                }
            }

            return strtmp;



        }

    }
}
