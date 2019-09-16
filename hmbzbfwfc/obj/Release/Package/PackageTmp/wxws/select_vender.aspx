<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_vender.aspx.cs" Inherits="UI.wxws.select_vender" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" name="viewport" id="viewport" />
    <title>供应商选择</title>
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
            pullWinH();
            loaded();
            loadCus("1");
            index = parent.layer.getFrameIndex(window.name);

            //点击传值给父窗口
            $("#listview").delegate('li', 'click', function () {
                var no = $(this).find(".labPhone").find("span").text();
                var name = $(this).find(".labcuname").find("span").text();
                parent.$("#cusInfo").val(no);
                parent.$("#cusName").html(name);
                parent.layer.close(index);

            });
        })
        //加载客户信息
        function loadCus(pageIndex) {
            var queryText = $("#myFilter").val();
            if (pageIndex == "1") {
                $("#listview").empty();
            }

            var username = window.localStorage.getItem("hmcu_no");
            
            $.ajax({
                url: "../App/appListOf.ashx",
                type: "post",
                data: { "key": "vender", "query": queryText, "PageIndex": pageIndex, "PageSize": 20,"cu_no":username},
                dataType: "json",
                success: function (data) {
                    var json = data;
                    var pList = $("#listview");
                    var tmpHtml = "";
                    if (json.success) {
                        var L = json.cont;
                        $.each(json.cont, function (i, v) {
                            tmpHtml += "<li data-icon='arrow-r'><a href='javascript:void(0)'>";
                            tmpHtml += "<label class='labcuname'>姓名:<span>" + v.cu_name + "</span></label>";
                            tmpHtml += "<label class='labPhone'>手机:<span>" + v.cu_no + "</span></label>";
                            tmpHtml += "<label class='labAdder'>地址:" + v.province + v.city + v.addr + "</label>";
                            tmpHtml += "</a></li>";
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
                loadCus(page);
            }, 500);
        }
        function pullDownAction() {
            //下拉刷新数据,如取回第一页数据.
            $("#PageIndex").val(1);
            setTimeout(function () {
                loadCus('1');
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
        function getCookieVal(str, key) {
            var userData = [];
            userData = str.split('&');//
            if (userData.length <= 0)
                return null;
            for (var i = 0; i < userData.length; i++) {
                var cookies = userData[i].split('=');
                if (cookies[0] == key)
                    return cookies[1];
            }
            return null;
        }
    </script>
</head>
<body>
    <div data-role="page">
        <form class="ui-filterable">
            <input id="myFilter" data-type="search" onclick="showSearch();" />
        </form>
        <div id="btnsearch" style="display:none" data-role="button" class="ui-content">
            <a class="ui-btn" onclick="onSearch();">搜索</a>
        </div>
        <div id="wrapper">
            <div id="scroller">
                <div id="pullDown">
                    <span class="pullDownIcon"></span><span class="pullDownLabel">下拉刷新...</span>
                </div>
                <ul id="listview" data-input="#myFilter" class="listview" data-role="listview" data-filter="true" data-filter-placeholder="输入编号/姓名/地址查询"></ul>
                <div id="pullUp" style="display: none;">
                    <span class="pullUpIcon"></span><span class="pullUpLabel">上拉加载更多...</span>
                </div>
            </div>
        </div>
        <input type="hidden" id="PageIndex" value="1" />
        <input type="hidden" id="PageSize" value="20" />
    </div>
</body>
</html>
