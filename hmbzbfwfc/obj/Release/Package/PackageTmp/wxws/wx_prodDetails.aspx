<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_prodDetails.aspx.cs" Inherits="UI.wxws.wx_prodDetails" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" name="viewport" id="viewport" />
    <title>产品出货详情</title>
    <script src="js/jquery-1.9.0.min.js" type="text/javascript"></script>

    <script src="js/jquery.cookie.js" type="text/javascript"></script>
    
    <script src="jquery.mobile-1.4.2/jquery.mobile-1.4.2.min.js" type="text/javascript"></script>

    <link href="jquery.mobile-1.4.2/jquery.mobile-1.4.2.min.css" rel="stylesheet" type="text/css" />

    <script src="layer/layer.js" type="text/javascript"></script>

    <link href="layer/skin/layer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            font-family: "Microsoft YaHei",Georgia;
            font-size: 1.5em;
            font-weight:700;
            width: 100%;
        }
            .listview li {
                border-bottom: 1px solid black;
            }

                .listview li a {
                    height: 2.5em;
                    border: 0px;
                    width: 100%;
                    text-decoration: none;
                }

                .listview li label {
                    display: inline-block;
                    color: #333;
                }

        .labcuname {
            padding-left: 1em;
        }

        .listview li label.labPhone {
            display: block;
            padding-bottom: .425em;
            margin: 0;
            border: 0;
        }
    </style>
    <script>
        var PageHTML = "";//分页加载全局变量;
        var index;; //获取窗口索引
        $(function () {//加载完成后触发
           var fid=getUrlParam("fid");
                    $.ajax({
                        url: "../App/appListOf.ashx",
                    type: "post",
                    data: { "key": "ship2","PageIndex":1,"PageSize":1,"fid":fid,"cu_no":"13800000001"},
                    dataType: "json",
                    success: function (data) {
                        var json = data;
                        var tmpHtml = "";
                        if(json.success){
                        $.each(json.cont,function(i,v){
                         $("#barcode").val(v.bsnno);
                          $("#p_no").val(v.p_no);
                          $("#pname").val(v.pname);
                          $("#cu_no").val(v.cu_no);
                          $("#cu_name").val(v.cu_name);
                          $("#mqty").val(v.mqty);
                          $("#in_date").val(v.ship_date);
                        });
                     }
                    },
                    error: function (ob, status, error) {
                       alert("服务器繁忙...请稍候再试.");
                    }
                });
        });
    </script>
    <script type="text/javascript">
    function getUrlParam(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数,返回是一个数组.
        if (r != null) return unescape(r[2]); return null; //返回参数值
    }
    </script>
</head>
<body>
    <!--产品详情开始-->
    <div data-role="page">
        <div data-role="fieldcontain">
            <label for="name">条码:</label>
            <input type="text" name="name" id="barcode" disabled="disabled" value="" />
        </div>
        <div data-role="fieldcontain">
            <label for="name">产品编号:</label>
            <input type="text" name="name" id="p_no" disabled="disabled" value="" />
        </div>
        <div data-role="fieldcontain">
            <label for="name">产品名称:</label>
            <input type="text" name="name" id="pname" disabled="disabled" value="" />
        </div>
        <div data-role="fieldcontain">
            <label for="name">客户:</label>
            <input type="text" name="name" id="cu_no" disabled="disabled" value="" />
        </div>
        <div data-role="fieldcontain">
            <label for="name">姓名:</label>
            <input type="text" name="name" id="cu_name" disabled="disabled" value="" />
        </div>
        <div data-role="fieldcontain">
            <label for="name">数量:</label>
            <input type="text" name="name" id="mqty" disabled="disabled" value="" />
        </div>
        <div data-role="fieldcontain">
            <label for="name">出货时间:</label>
            <input type="text" name="name" id="in_date" disabled="disabled" value="" />
        </div>
        <div>
        </div>
        <!--产品详情结束-->
    </div>
</body>
</html>
