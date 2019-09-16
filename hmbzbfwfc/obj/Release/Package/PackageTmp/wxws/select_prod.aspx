<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_prod.aspx.cs" Inherits="UI.wxws.select_prod" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" name="viewport" id="viewport" />
    <title>产品选择</title>
        <script src="js/jquery-1.9.0.min.js" type="text/javascript"></script>

    <script src="jquery.mobile-1.4.2/jquery.mobile-1.4.2.min.js" type="text/javascript"></script>
    
    <link href="jquery.mobile-1.4.2/jquery.mobile-1.4.2.min.css" rel="stylesheet" type="text/css" />

    <script src="layer/layer.js" type="text/javascript"></script>

    <link href="layer/skin/layer.css" rel="stylesheet" type="text/css" />
    <script src="iscroll/iscroll.js" type="text/javascript"></script>

    <script src="iscroll/appIscroll.js" type="text/javascript"></script>

    <link href="iscroll/Iscroll.css" rel="stylesheet" type="text/css" />
    <link href="iscroll/pull.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .listview  
         {
         font-family:"Microsoft YaHei",Georgia;
          font-size:1em;
          font-weight:700;
          width:100%;
         }
         .listview li
         {
         	line-height:2.5em;
         	border-bottom:1px solid black;
         }
        .listview li a
        {
        	height:2.5em;
        	border:0px;
        	width:100%;
        	text-decoration:none;
        }
        .listview li a
        {
        	text-decoration:none;
        }
        .listview li label
        {
        	display:inline-block;
        	margin-left:.453em;
        	color:#333;
        }
    </style>
    <script>
   var index;; //获取窗口索引
   $(function () {//加载完成后触发
   pullWinH();
   loaded();
   loadProd("1");
     index= parent.layer.getFrameIndex(window.name);
     //点击传值给父窗口
     $("#listview").delegate('li','click',function(){
     var _this=$(this);
     var p_no=_this.find(".labpno").find("span").text().trim();
     var pname=_this.find(".labpname").find("span").text().trim();
     parent.$("#prodInfo").val(p_no);
     parent.$("#prodName").html(pname);
     parent.layer.close(index);
   });
        })
           //加载产品信息
            function loadProd(pageIndex)
            {
             var query=$("#myFilter").val();
             
                if(pageIndex=="1")
                {
                  $("#listview").empty();
                }
                $.ajax({
                    url: "../App/appListOf.ashx",
                    type: "post",
                    data: { "key": "prod" ,"query":query,"PageIndex":pageIndex,"PageSize":20},
                    dataType: "json",
                    success: function (data) {
                        var json = data;
                        var pList = $("#listview");
                        var tmpHtml = "";
                        if (json.success) {
                            var L = json.cont;
                            if (L.length > 0) {
                                $.each(json.cont, function (i, v) {
                                    tmpHtml += "<li data-icon='arrow-r'><a href='javascript:void(0)'>";
                                    tmpHtml+="<label class='labpno'>产品编号:<span>" + v.p_no + "</span></label><label class='labpname'>产品名称:<span>"+v.pname+"<span></label>";
                                    tmpHtml+="</a></li>";
                                });
                            }
                            $("#myFilter").val("");
                            pList.append(tmpHtml);
                            setTimeout(function() { myScroll.refresh(); }, 500);
                        }
                        else
                        {
                          layer.msg(json.Message);
                          setTimeout(function() { myScroll.refresh(); }, 500);
                        }
                    },
                    error: function (ob, status, error) {
                    }
                });
            }
    </script>
      <script type="text/javascript">
        function pullUpAction() {//上拉加载数据.
        var page=parseInt($("#PageIndex").val());
        ++page;
        $("#PageIndex").val(page);
            setTimeout(function() {
                loadProd(page);
            }, 500);
        }
        function pullDownAction() {
            //下拉刷新数据,如取回第一页数据.
            $("#PageIndex").val(1);
            setTimeout(function() {
                loadProd('1');
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
          loadProd("1");
           $("#btnsearch").hide();
        }
        function showSearch()
        {
          $("#btnsearch").show();
        }
    </script>
</head>
<body>
    <div data-role="page">
        <form class="ui-filterable">
         <input id="myFilter" data-type="search" onclick="showSearch()"/>
        </form>
        <div  id="btnsearch" style="display:none" data-role="button" class="ui-content">
      <a class="ui-btn" onclick="onSearch();">搜索</a>
    </div>
        <div id="wrapper">
        <div id="scroller">
            <div id="pullDown">
                <span class="pullDownIcon"></span><span class="pullDownLabel">下拉刷新...</span>
            </div>
            
       <ul id="listview" data-input="#myFilter" class="listview" data-role="listview" data-filter="true" data-filter-placeholder="输入产品信息查询">
         </ul>

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
