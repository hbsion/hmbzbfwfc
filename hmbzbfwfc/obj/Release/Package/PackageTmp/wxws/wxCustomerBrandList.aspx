<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxCustomerBrandList.aspx.cs" Inherits="UI.wxws.wxCustomerBrandList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<!doctype html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>下级列表
    </title>
    <link rel="stylesheet" href="css/Jingle.css?r=20150505" />
    <link rel="stylesheet" href='css/app.css?r=2' />

    <link href="css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/zepto.js"></script>

    <script type="text/javascript" src="js/iscroll.js"></script>

    <script type="text/javascript" src="js/alertify.js"></script>
    <script type="text/javascript" src="js/comm.js?r=20150616"></script>


    <style type="text/css">
        .btnSearch {
            width: 100%;
            padding: 6px 0px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            -webkit-box-shadow: 0px;
            box-shadow: 0px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //获取参数
            var brandType = "";
            var gclass = "";
            //alert(gclass);
            //第一页
            GetCustomerList("0", brandType, gclass)
            //下一页
            $('#morePage').on('click', function (e) {

                var countPage = parseInt($("#pageIndex").val()) + 1;
                GetCustomerList(countPage, brandType, gclass);
                $("#pageIndex").val(countPage);
            });
            $("#btnSearch").on("click", function () {
                GetCustomerList("", brandType, gclass);
            });
        });
        function GetCustomerList(page, brandId, gclass) {
            var cu_no = localStorage["hmcu_no"];

            var searchText = $("#searchText").val();
            try {
                TipLoad.loading();
                $.ajax({
                    type: 'POST',
                    url: 'wxAPI/queryAPI.ashx',
                    data: { "cu_no": cu_no, "type": "1", "page": page, "brandId": brandId, "gclass": gclass, "searchText": searchText },
                    dataType: 'json',
                    timeout: 30000,
                    success: function (data) {

                        var vhtml = "";
                        if (data.success) {
                        
                            TipLoad.close();
                            for (var i = 0; i < data.lists.length; i++) {
                                vhtml += "<a href='sal_cust_edit.aspx?id=" + data.lists[i].id + "'>";
                                vhtml += " <p>手机号(账号):" + data.lists[i].id + "</p>";
                                vhtml += " <p>姓名:" + data.lists[i].title + "</p>";
                                vhtml += "</p> " + "</p></li></a>";

                            }
                            $(".list").html(vhtml);
                        }
                        else {
                            
                            alertify.alert(data.Messages);
                        }

                    },
                    error: function (xhr, type) {
                        
                       // alert(type);
                        //alert('超时,或服务错误');
                    }
                });

            } catch (e) {
               // TipLoad.close();
                //alert(e);
            }
        }
        function getQueryString(name) {
            //unescape(r[2])
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return r[2]; return null;
        }
    </script>
</head>
<body>

    <div id="aside_container" style="display: block;">
    </div>

    <div id="section_container">
        <section id="index_section" class="active">
            <header >
                <nav class="left">
                    <a href="Agent_Index.aspx" data-icon="previous" data-target="back"><i class="icon previous"></i>返回</a>
                </nav>
                <h1 class="title" id="title" runat="server">
                下级列表
                </h1>
                <nav class="right">
                    <a data-target="section" data-icon="info" href="wx_Register.aspx" id="manualBtn" >新增 </a>
                </nav>

            </header>
          <div class="searchBox">
           <input type="text" id="searchText"  placeholder="姓名/手机号"/>
           <input type="button" id="btnSearch" class="btnSearch" value="搜索" />
          </div>
            <!--内容开始-->
         <div class="scroll-area-list" id="codelListArea" runat="server" style="margin-bottom: 30px;"> 
          <ul class="list" >
         </ul>
          </div> 
        <!--内容结束-->

        </section>
        <div class="navbar_div" style="height: 40px;">
            <input id="morePage" type="button" class="button block" value="下一页" style="color: #ffffff; line-height: 35px;" />
        </div>
        <input type="hidden" id="pageIndex" value="0" />
    </div>
</body>
</html>

