<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_kcshipScan.aspx.cs" Inherits="UI.wxws.wx_kcshipScan" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>出货扫描</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=2.0,user-scalable=no"
        name="viewport" id="viewport" />
    <script src="js/jquery-1.9.0.min.js" type="text/javascript"></script>

    <link href="jquery.mobile-1.4.2/jquery.mobile-1.4.2.min.css" rel="stylesheet" type="text/css" />

    <script src="jquery.mobile-1.4.2/jquery.mobile-1.4.2.min.js" type="text/javascript"></script>

    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js" type="text/javascript"></script>

    <script src="layer/layer.js" type="text/javascript"></script>

    <link href="layer/skin/layer.css" rel="stylesheet" type="text/css" />
    <link href="css/cuship.css" rel="stylesheet" type="text/css" />

    <script src="js/cuship.js" type="text/javascript"></script>

    <link href="css/Jingle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var username = undefined;
        username = localStorage["hmcu_no"];
        var corderQty = 0;
        var total = 0; //扫描的总数量.
        var content = ""; //扫描的条码内容
        var hoid = 0; //0:空闲，1：运行中
        window.onerror = function (e) {
            alert(e);
            return true;
        }

        $(document).ready(function () {

            username = localStorage["hmcu_no"];

            $("#ship_no").val(autoBillno());
            $("#btnInput").bind("click", function () {
                //手工输入
                if (username != null && username != undefined)
                    addData(username);
            });
            //调用微信扫一扫功能
            $("#btnScan").on("click", function () {
                if (username != null && username != undefined)
                    onQrcode(username);
            });


        });
        //选择客户
        function layerOpenWin(type) {
             if (type == "2") {
                onSelect("选择客户", "select_salcu.aspx", '100%', '85%');
            }else{

            }
        }




        //微信扫描出货
        function addData(xtcu_no) {

            var shipNo = $("#ship_no").val();
            var cusNo = $("#cusInfo").val();
            var barcode = $("#barcode").val();

         //   var po_no = $("#selectOrderPro").val();


            var user_id = localStorage["xtuser_id"];


            //删除操作
            var delyn = "N";
            if (document.getElementById("delyn").checked) {
                delyn = "Y";
            }

            if (barcode.length == 0) {
                layer.tips("请输入条码", $("#barcode"), { tips: 1, icon: 5 });
                return;
            }

               

            if (cusNo.length == 0) {
                layer.tips("请选择经销商", $("#cusInfo"), { tips: 1, icon: 5 });
                return;
            }               
         
            var index = layer.msg('正在扫码出货中...', { icon: 16 }); //0代表加载的风格，支持0-2
            if (hoid == 1) return;
            hoid = 1;
            var myurl = "../App/apporder.ashx";
            $.ajax({
                url: myurl,
                type: "get",
                cache: false,
                async: false,
                data: {
                     "cu_no": encodeURIComponent(xtcu_no),"optype": "kcshipScan", "snno": barcode, "ship_no": shipNo,
                     "shipcu_no": encodeURIComponent(cusNo), "delyn": delyn, "user_id": user_id,                     
                },
                dataType: "json",
                timeout: 30000,
                success: function (data) {
                    hoid = 0;
                    if (data.success) {
                        $("#ret_prod").html(data.pname + '【' + data.p_no + '】');
                        $("#ret_mqty").html(parseInt($("#ret_mqty").html()) + 1); //累加数量
   
                        layer.msg("条码:" + barcode + "出货成功");
                        //连续扫描
                        var rechecked = $("#resacn").prop('checked');
                        if (rechecked) {
                            setTimeout(function () {
                                onQrcode(xtcu_no);
                            }, 2000);
                        }
                    } else {
                        layer.closeAll();
                        layer.msg(data.Message);
                    }
                },
                error: function (ob, code, aa) {
                    hoid = 0;
                    alert(JSON.stringify(ob));
                }
            });
        }
        //公众号调用微信扫一扫功能
        function onQrcode(username) {
            wx.scanQRCode({
                needResult: 1,
                scanType: ["qrCode", "barCode"],
                success: function (res) {
                    var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
                    var i = result.lastIndexOf(',');
                    if (i > 0) {
                        result = result.substr(i + 1, result.length - i - 1);
                    }
                    i = result.lastIndexOf('?');
                    if (i > 0) {
                        result = result.substr(i + 1, result.length - i - 1);
                    }

                    $("#barcode").val(result);
                    //处理扫描完的结果
                    //alert(result);
                    addData(username);
                }
            });
        }
    </script>

</head>
<body>
    <form id="Form1" runat="server">
        <!-- 头部开始 -->
        <header>
            <nav class="left">
                  <a href="javascript:history.go(-1);"  data-icon="previous" data-target="back"><i class="icon previous"></i>返回 </a>
            </nav>
            <h1 class="title">
               产品出货
            </h1>
           </header>
        <div class="c">
        </div>
        <!-- 头部结束 -->
        <!--页面开始-->
        <div data-role="page" id="menu" style="position: relative; background-color: #ffffff">
            <!--内容开始-->
            <div data-role="content">
                
                 <div class="ub">
                    <label for="shipNo">
                        单号:</label>
                    <input type="text" style="margin-bottom: 0" placeholder="*必填" runat="server" name="shipNo" id="ship_no" />
                </div>
                <div class="ub notOrdership">
                    <label for="cusInfo">
                        客户:</label>
                    <input type="text" style="margin-bottom: 0" placeholder="*请选择客户" runat="server" name="cusInfo" id="cusInfo" />
                    <input value="选择客户" onclick="layerOpenWin('2')" type="button" id="select_cus" />
               
                </div>
                
                  <div   id="cusName">
                   
                 </div>

                <div class="ub">
                    <label for="barcode">
                        条码:</label>
                    <input type="text" style="margin-bottom: 0" placeholder="*必填" runat="server" name="barcode" id="barcode" />
                </div>
                <div class="ub1">
                    <input value="开始扫描" data-inline="true" type="button" id="btnScan" />
                    <input value="手工输入" runat="server" data-inline="true" type="button" id="btnInput" />

                </div>

                    <div class="ub1">

                    <input data-inline="true" type="checkbox" name="resacn" id="resacn" data-inline="true" />
                    <label for="resacn">
                        连续扫描</label>

                    <input data-inline="true" type="checkbox" name="delyn" id="delyn" data-inline="true" />
                    <label for="delyn">
                        删除操作</label>

                </div>

                <div class="shipContent">
                    <label>
                        扫描产品:</label><span id="ret_prod"></span><br />
                    <label>
                        扫描数量:</label><span id="ret_mqty">0</span><br />
                    <label>
                        扫描内容:</label><br />
                    <div id="mainContent" class="mainContent">
                    </div>
                </div>
                <div class="shipContent ordership">
                    <label>
                        订单数量:</label><span id="txtmqty">0</span><br />
                    <label>
                        已扫描数量:</label><span id="txtovqty">0</span><br />
                    <label>
                        订单详情:</label><br />
                    <div id="txtOrderDiv" class="mainContent">
                    </div>
                </div>
                <!--内容结束-->
            </div>
        </div>
        <div id="myConfig" runat="server">
        </div>
    </form>
</body>
</html>
