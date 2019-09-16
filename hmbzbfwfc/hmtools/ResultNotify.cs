﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using marr.BusinessRule;
using marr.DataLayer;
using UI.hmtools;

namespace UI.WXPay
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify:Notify
    {
        public ResultNotify(Page page):base(page)
        {
        }

        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info(this.GetType().ToString(), "我的订单 query success : " + res.ToXml());



                //处理订单
                if (notifyData.GetValue("out_trade_no") != null && notifyData.GetValue("result_code") != null && notifyData.GetValue("result_code").ToString() == "SUCCESS")
                {
                    string out_trade_no = notifyData.GetValue("out_trade_no").ToString();

                    string strsql = "update  hob_yuerintemp  set inremark='充值成功'  where bill_no='" + out_trade_no + "' and inremark='未支付' ";
                    int ii = DbHelperSQL.ExecuteSql(strsql);
                    if (ii > 0) { 
                    string addsql = "insert into hob_yuerin select bill_no,in_date,mqty,dian_no,czybm,remark,intype,inzh,inname,inremark,unitcode,sxf  from hob_yuerintemp where bill_no='"+out_trade_no+"'";
                    int add = DbHelperSQL.ExecuteSql(addsql);
                    }
                }


                page.Response.Write(res.ToXml());
                page.Response.End();


            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req,6);

            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}