<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fw.aspx.cs" Inherits="UI.w.fw" %>

<!DOCTYPE html>
<html>
<head>
    <title>防伪验证</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="keywords" content="物流溯源管理系统,二维码物流追踪,二维码防伪,防伪防窜货,防伪标签,二维码防伪" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="renderer" content="webkit" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <meta name="mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <meta name="full-screen" content="yes" />
    <meta name="x5-fullscreen" content="true" />
    <meta name="browsermode" content="application" />
    <meta name="x5-page-mode" content="app" />

    <link href="AmazeUI/css/amazeui.min.css" rel="stylesheet" type="text/css" />
    <link href="AmazeUI/css/app.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="AmazeUI/js/amazeui.min.js" type="text/javascript"></script>
    <script src="js/pdm.wap.js" type="text/javascript"></script>
    <style>
        .am-panel-title span {
            font-weight: normal;
            font-size: 95%;
        }

        .am-list-static, .am-comments-list li {
            font-size: 90%;
        }

        .am-panel-bd {
            padding: 0.75rem;
        }

        .am-comment-avatar {
            color: #FFFFFF;
            text-align: center;
            line-height: 30px;
            background: #0099CC;
        }
    </style>
</head>
<body>
    <header data-am-widget="header" class="am-header am-header-default am-header-fixed">
        <h1 class="am-header-title" style="margin: 0; font-size: 1.2em;">防伪验证</h1>
        <div class="am-header-left am-header-nav">
            <a href="javascript:history.back();void(0)" class="">
                <i class="am-header-icon am-icon-reply">&nbsp;</i>
            </a>
        </div>
    </header>

    <div id="pdm_slider_index" class="am-slider  am-slider-a5" data-am-slider='{&quot;directionNav&quot;:false}'>
        <ul class="am-slides">
            <li>&nbsp;</li>
        </ul>
    </div>
    <hr data-am-widget="divider" style="" class="am-divider am-divider-dashed" />
    <form class="am-form" id="form_input">

        <fieldset>
            <div class="am-form-group">
                <input type="tel" class="am-form-field am-radius" id="txt_fw_code" runat="server" placeholder="在此输入需要查询的防伪码" maxlength="25" />
            </div>
            <center>
            <button type="button" class="am-btn am-btn-default am-radius am-btn-primary am-btn-sm" onclick="sumbitquerycode()">
                <i class="am-icon-search"></i>
                确认查询
            </button>
        </center>
        </fieldset>
    </form>


    <div class="am-panel am-panel-default" id="form_result" style="display: none;">
        <div class="am-panel-hd">
            <h3 class="am-panel-title" id="fw_title"></h3>
        </div>
        <div class="am-panel-bd" id="fw_result">
        </div>
        <center>
            <button type="button" class="am-btn am-btn-default am-radius am-btn-primary am-btn-sm" onclick="ReturnQuery()">
                <i class="am-icon-reply"></i>
                返回
            </button>
        </center>
        <p>
    </div>
    <hr data-am-widget="divider" style="" class="am-divider am-divider-dashed" />
    <footer data-am-widget="footer" class="am-footer am-footer-default">
        <div class="am-footer-miscs ">
        </div>
    </footer>


    <div class="am-modal am-modal-alert" tabindex="-1" id="my-alert">
        <div class="am-modal-dialog">
            <div class="am-modal-bd" id="tip_result">
            </div>
            <div class="am-modal-footer">
                <span class="am-modal-btn">确定</span>
            </div>
        </div>
    </div>

    <!--以下为获取地理位置代码-->

    <div id="address" style="color: Gray; text-align: left;">
    </div>

    <div id="mymap" style="position: absolute; width: 20px; height: 20px; border: 0px solid gray; overflow: hidden; display: none;">
    </div>

    <div style="display: none;">


        <input type="hidden" id="txtlatitude" value="0.0000" />
        <input type="hidden" id="txtlongitude" value="0.0000" />

    </div>


    <script type="text/javascript" src="https://3gimg.qq.com/lightmap/components/geolocation/geolocation.min.js"></script>


    <script type="text/javascript">


        var geolocation = new qq.maps.Geolocation("CPYBZ-X6S3X-BWV4G-Z3VQ4-PCDP2-VMBNX", "myapp");


        var positionNum = 0;
        var options = { timeout: 8000 };

        function showPosition(position) {


            var addComp = position;

            $("#txtlatitude").val(addComp.lat);
            $("#txtlongitude").val(addComp.lng);

            $("#address").html("查询地址：" + addComp.province + addComp.city + addComp.district + addComp.addr);

            if ($("#FwCode").val().length > 8) {
                fwcx($("#FwCode").val());
            }


        };


        function showErr() {

            alert("定位失败！");

        };


        window.onload = function () {

            geolocation.getLocation(showPosition, showErr, options)



        }




    </script>
    <!--获取地理位置代码结束-->

    <script type="text/javascript">




        //防伪查询
        function sumbitquerycode() {


            var $modal = $('#my-alert');
            var code = $('#txt_fw_code').val();
            if (code.length <= 0) {
                $('#tip_result').text('请输入您所查询的防伪编码');;
                $modal.modal('open');
                return false;
            }


            //增加传位置接口
            var address = $("#address").html();
            var lat = $("#txtlatitude").val();
            var lng = $("#txtlongitude").val();


            $.ajax({
                type: "GET", url: "fwqueryjson.ashx?callback=?", data: { "fwcode": code, "address": address, "timestamp": new Date().getTime(), "lat": lat, "lng": lng }, dataType: "jsonp", jsonp: "callback", success: function (data) {


                    //防伪查询结果

                    var resesult = data.QueryResult;

                    if (data.CodeState == "2" && data.loca.length > 0)  //多次查询
                    {
                        resesult = resesult + " 首次查询地址：" + data.loca + "<a href=\"map.aspx?lat=" + data.lat + "&lng=" + data.lng + "\">详情</a>";
                    }

                    //  $("#ReturnResult").html(resesult);


                    $('#txt_fw_code').val('');
                    $('#form_input').hide();
                    $('#form_result').show();
                    $('#fw_result').html(resesult);
                    $('#fw_title').text("防伪验证结果");
                    $('#return_arrow').show();

                }
            });

        }

        function ReturnQuery() {

            $('#form_input').show();
            $('#form_result').hide();

        }
    </script>

</body>
</html>



