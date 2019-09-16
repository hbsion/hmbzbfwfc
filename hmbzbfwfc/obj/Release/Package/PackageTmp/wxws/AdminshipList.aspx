<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminshipList.aspx.cs" Inherits="UI.wxws.AdminshipList" %>

<!DOCTYPE HTML >

<html>

<head>
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" name="viewport" id="viewport" />
    <title>产品出货列表</title>
    <script src="js/jquery-1.9.0.min.js" type="text/javascript"></script>
    <script src="js/jquery.cookie.js" type="text/javascript"></script>

    <script src="jquery.mobile-1.4.2/jquery.mobile-1.4.2.min.js" type="text/javascript"></script>

    <link href="jquery.mobile-1.4.2/jquery.mobile-1.4.2.min.css" rel="stylesheet" type="text/css" />

    <script src="layer/layer.js" type="text/javascript"></script>

    <link href="layer/skin/layer.css" rel="stylesheet" type="text/css" />
    <script src="iscroll/iscroll.js" type="text/javascript"></script>
    <script src="iscroll/appIscroll.js" type="text/javascript"></script>

    <link href="iscroll/Iscroll.css" rel="stylesheet" type="text/css" />
    <link href="iscroll/pull.css" rel="stylesheet" type="text/css" />
    <link href="css/Jingle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .listview {
            font-family: "Microsoft YaHei",Georgia;
            font-size: 1em;
            /*font-weight:700;*/
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
                    display: block;
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
        $(function () {//加载完成后触发
            pullWinH();
            loaded();
            loadData("1");//分页
        })
        //加载客户信息
        function loadData(pageIndex) {
            var queryText = $("#myFilter").val();
            if (pageIndex == "1") {
                $("#listview").empty();
            }
            var userinfo = "";
            var username = "";//总部
            $.ajax({
                url: "../App/appListOf.ashx",
                type: "post",
                data: { "key": "ship1", "query": queryText, "PageIndex": pageIndex, "PageSize": 10, "cu_no": username },
                dataType: "json",
                success: function (data) {
                    var json = data;
                    var pList = $("#listview");
                    var tmpHtml = "";
                    if (json.success) {
                        var L = json.cont;
                        $.each(json.cont, function (i, v) {
                            tmpHtml += "<li data-icon='arrow-r' fid='" + v.fid + "'>";
                            tmpHtml += "<label>单号:" + v.ship_no + "</label>";
                            tmpHtml += "<label class='labcuno'><span>姓名:" + v.cu_name + "</span>&nbsp;&nbsp;<span>电话:" + v.cu_no + "</span></label>";
                            tmpHtml += "<label>名称:" + v.pname + "&nbsp;&nbsp;数量:" + v.mqty + "&nbsp;&nbsp;日期:" + v.ship_date + "</label>";
                            tmpHtml += "<label>条码:" + v.bsnno + "&nbsp;&nbsp;" + "</label>";
                            tmpHtml += "</li>";
                        });
                        pList.append(tmpHtml);
                        setTimeout(function () { myScroll.refresh(); }, 500);
                    }
                    else {
                        layer.msg(data.Message);
                        setTimeout(function () { myScroll.refresh(); }, 500);
                    }
                },
                error: function (ob, status, error) {
                }
            });
        }
    </script>

    <script type="text/javascript">
        function pullUpAction() {//上拉加载数据.
            var page = parseInt($("#PageIndex").val());
            ++page;
            $("#PageIndex").val(page);
            setTimeout(function () {
                loadData(page);
            }, 500);
        }
        function pullDownAction() {
            //下拉刷新数据,如取回第一页数据.
            $("#PageIndex").val(1);
            setTimeout(function () {
                loadData('1');
            }, 500);
        }
        function pullWinH() {
            var headH = "1";
            var fotH = "1";
            var winH = $(window).height();
            if (headH > 0 && fotH > 0 && winH > 0) {
                var HH = winH - headH - fotH;
                if (HH > 0) { HH = HH + "px"; } else { HH = "500px" }

                document.getElementById("wrapper").style.height = HH;
                document.getElementById("listview").style.minHeight = HH;
            }
            else {
                document.getElementById("wrapper").style.height = "500px";
                document.getElementById("listview").style.minHeight = "500px";
            }
        }
        function onSearch() {//搜索
            loadCus("1");
            $("#btnsearch").hide();
        }
        function showSearch() {
            $("#btnsearch").show();
        }


    </script>
</head>
<body>
    <form id="Form1" runat="server">
        <div data-role="page">
            <header>
                <nav class="left">
                    <a href="javascript:history.go(-1);" data-icon="previous" data-target="back"><i class="icon previous"></i>返回</a>
                </nav>
                <h1 class="title">出货列表
                </h1>
            </header>
            <form class="ui-filterable">
                <input id="myFilter" data-type="search" onclick="showSearch();" />
            </form>
            <div id="btnsearch" style="display: none" data-role="button" class="ui-content">
                <a class="ui-btn" onclick="onSearch();">搜索</a>
            </div>
            <div id="wrapper">
                <div id="scroller">
                    <div id="pullDown">
                        <span class="pullDownIcon"></span><span class="pullDownLabel">下拉刷新...</span>
                    </div>
                    <!--产品列表开始-->
                    <ul id="listview" data-input="#myFilter" class="listview" data-role="listview" data-filter="true" data-filter-placeholder="输入编号/姓名/地址查询"></ul>
                    <!--产品列表结束-->
                    <div id="pullUp" style="display: none;">
                        <span class="pullUpIcon"></span><span class="pullUpLabel">上拉加载更多...</span>
                    </div>
                </div>
            </div>
            <input type="hidden" id="PageIndex" value="1" />
            <input type="hidden" id="PageSize" value="20" />
        </div>
        <div id="myConfig" runat="server">
        </div>
    </form>
</body>
</html>
