using hmbzbfwfc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Xml;


namespace hmbzbfwfc.Commons
{
    public class ReadXml
    {

        public void readxml() {
            ConnectionStringEntities db = new ConnectionStringEntities();

            

            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Administrator\Desktop\pro.xml");
            
            XmlNode xn = doc.SelectSingleNode("NewDataSet");
            XmlNodeList xnl = xn.ChildNodes;
            
            foreach (XmlNode xns in xnl)
            {
                try
                {

                    Inv_Part p = new Inv_Part();
                    
                    XmlElement xe = (XmlElement)xns;
                    
                    XmlNodeList xnl0 = xe.ChildNodes;

                    if (xnl0.Count > 38)
                    {
                        p.p_no = xnl0.Item(1).InnerText;
                        p.pname = xnl0.Item(2).InnerText;
                        p.type = xnl0.Item(3).InnerText;
                        p.unit = xnl0.Item(4).InnerText;

                        p.loca = xnl0.Item(5).InnerText;
                        p.remark = xnl0.Item(6).InnerText;
                        p.remark2 = xnl0.Item(7).InnerText;
                        try
                        {
                            p.in_date = DateTime.Parse(xnl0.Item(8).InnerText);
                        }
                        catch { }

                        p.m_user = xnl0.Item(9).InnerText;
                        p.sfqty = Convert.ToInt16(xnl0.Item(10).InnerText);
                        p.price = Convert.ToDecimal(xnl0.Item(11).InnerText);
                        p.cur_no = xnl0.Item(12).InnerText;

                        p.mtype = xnl0.Item(13).InnerText;
                        p.back = Convert.ToDecimal(xnl0.Item(14).InnerText);
                        p.stqty = Convert.ToDecimal(xnl0.Item(15).InnerText);
                        p.inqty = Convert.ToDecimal(xnl0.Item(16).InnerText);
                        p.outqty = Convert.ToDecimal(xnl0.Item(17).InnerText);
                        p.parttype = xnl0.Item(18).InnerText;
                        p.curprice = Convert.ToDecimal(xnl0.Item(19).InnerText);
                        p.curchang = Convert.ToDecimal(xnl0.Item(20).InnerText);
                        p.qcdays = Convert.ToInt16(xnl0.Item(21).InnerText);
                        p.saftqty = Convert.ToInt16(xnl0.Item(22).InnerText);
                        p.bm_no = xnl0.Item(23).InnerText;
                        p.pqty = Convert.ToInt16(xnl0.Item(24).InnerText);
                        p.mprice = Convert.ToDecimal(xnl0.Item(25).InnerText);
                        p.mpack = Convert.ToDecimal(xnl0.Item(26).InnerText);
                        p.mall = Convert.ToDecimal(xnl0.Item(27).InnerText);
                        p.fwcode = xnl0.Item(28).InnerText;
                        p.usetype = xnl0.Item(29).InnerText;
                        p.usefor = xnl0.Item(30).InnerText;
                        p.xtcu_no = xnl0.Item(31).InnerText;
                        p.jf = Convert.ToDecimal(xnl0.Item(32).InnerText);
                        p.ajf = Convert.ToDecimal(xnl0.Item(33).InnerText);
                        p.bjf = Convert.ToDecimal(xnl0.Item(34).InnerText);
                        p.cjf = Convert.ToDecimal(xnl0.Item(35).InnerText);
                        p.upyn = xnl0.Item(36).InnerText;
                        p.zqty = Convert.ToInt16(xnl0.Item(37).InnerText);
                        p.unitcode = xnl0.Item(38).InnerText;
                        p.imgurl = xnl0.Item(39).InnerText;
                    }
                    else {
                        p.p_no = xnl0.Item(1).InnerText;
                        p.pname = xnl0.Item(2).InnerText;
                        p.type = xnl0.Item(3).InnerText;
                        p.unit = xnl0.Item(4).InnerText;

                       
                        p.remark = xnl0.Item(5).InnerText;
                        
                        p.price = Convert.ToDecimal(xnl0.Item(6).InnerText);
                      
                        p.parttype = xnl0.Item(7).InnerText;
                      
                        p.bm_no = xnl0.Item(8).InnerText;
                        p.pqty = Convert.ToInt16(xnl0.Item(9).InnerText);
                        p.mprice = Convert.ToDecimal(xnl0.Item(10).InnerText);
                        
                        p.jf = Convert.ToDecimal(xnl0.Item(11).InnerText);
                        p.ajf = Convert.ToDecimal(xnl0.Item(12).InnerText);
                        p.bjf = Convert.ToDecimal(xnl0.Item(13).InnerText);
                        p.cjf = Convert.ToDecimal(xnl0.Item(14).InnerText);
                        p.zqty = Convert.ToInt16(xnl0.Item(15).InnerText);
                        p.unitcode = xnl0.Item(16).InnerText;
                        p.imgurl = xnl0.Item(17).InnerText;
                    }
                   
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.Inv_Part.Add(p);
                        db.SaveChanges();
                   
                }
                catch (Exception ex)
                {

                    
                }
            }
          
            
        }
    }
}