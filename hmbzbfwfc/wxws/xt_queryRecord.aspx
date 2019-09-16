<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xt_queryRecord.aspx.cs" Inherits="UI.wxws.xt_queryRecord" %>


<!DOCTYPE html >
<html lang="zh-CN">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />
    <meta content="telephone=no" name="format-detection" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>物流查询</title>
    <link href="../Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/hmcomm.css" rel="stylesheet" />

    <script src="../Content/js/jquery.min.js" type="text/javascript"></script>

    <link href="css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/alertify.js"></script>

   <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js" type="text/javascript"></script>

    <script type="text/javascript">

        function onSearch() {

            var username = localStorage["hmcu_no"];


            var code = $("#myFilter").val();

            var url = "../App/wl.aspx";
            $.ajax({
                type: "post",
                url: url,
                data: { "cu_no": username, "c": code },
                timeout: 30000,
                datatype: "html",
                success: function (msg) {


                    $("#codelListArea").html(msg);


                }


            });
        }

        //公众号调用微信扫一扫功能
        function onQrcode() {
            wx.scanQRCode({
                needResult: 1,
                scanType: ["qrCode", "barCode"],
                success: function (res) {
                    var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
                    var i = result.lastIndexOf('?');
                    if (i > 0) {
                        result = result.substr(i + 1, result.length - i - 1);
                    }
                    i = result.lastIndexOf(',');
                    if (i > 0) {
                        result = result.substr(i + 1, result.length - i - 1);
                    }

                    $("#myFilter").val(result);
                    onSearch();
           
                }


            });
        }

    </script>
</head>
<body>

    <!-- 头部开始 -->
    <div class="container-fluid">
        <div class="row text-center hmheader">
            <div class="col-xs-2 hmheader_left" onclick="javascript:history.go(-1);">
                <span class="glyphicon glyphicon-menu-left"></span>
            </div>
            <div class="col-xs-8 hmheader_center">
                物流查询 
            </div>
            <div class="col-xs-2 hmheader_right">
            </div>
        </div>
    </div>
    <!-- 头部结束 -->




    <div class="container-fluid" style="margin-top: 5px; margin-bottom: 5px;">
        <div class="input-group" style="margin-left: -10px; margin-right: -10px; z-index: 0;">
            <input type="text" class="form-control" id="myFilter" placeholder="输入物流码" />
            <span class="input-group-btn">
                <button class="btn btn-success" type="button" id="btnsearch" onclick="onSearch();">查询</button>
            </span>
        </div>
    </div>

        <div class="row text-center"  style="margin-top: 15px; " >
            <button type="button" class="btn  btn-info btn-sm" id="btnApply" style="width: 60%; height: 3em;" onclick="onQrcode(); ">扫描</button>
        </div>

    <div class="container-fluid" style="background-color: #fff;">

        <div id="codelListArea" style="margin: 0 auto; width: 100%;">
        </div>

    </div>

    
    <div id="myConfig" runat="server">
    </div>

</body>
</html>
