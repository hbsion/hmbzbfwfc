<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fc.aspx.cs" Inherits="UI.w.fc" %>

<!DOCTYPE html>
<html>
<head>
    <title>物流查询</title>
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

     <form id="form1" runat="server">
    <header data-am-widget="header" class="am-header am-header-default am-header-fixed">
        <h1 class="am-header-title" style="margin: 0; font-size: 1.2em;">物流查询</h1>
        <div class="am-header-left am-header-nav">
            <a href="javascript:history.back();void(0)" class="">
                <i class="am-header-icon am-icon-reply">&nbsp;</i>
            </a>
        </div>
    </header>

    <hr data-am-widget="divider" style="margin-top: 10px;" class="am-divider am-divider-dashed" />


        <fieldset>
            <div class="am-form-group">
                <input type="tel" class="am-form-field am-radius" id="txt_fw_code" runat="server" placeholder="在此输入需要查询的物流码" maxlength="25" />
            </div>


             <div class="am-form-group">
                <input type="text" class="am-form-field am-radius" id="wlcxzh" placeholder="在此输入账号" maxlength="25" />
            </div>
            <div class="am-form-group">
                <input type="password" class="am-form-field am-radius" id="wlcxmm" placeholder="在此输入密码" maxlength="25" />
            </div>

            <center>
  

              <button type="button" class="am-btn am-btn-default am-radius am-btn-primary am-btn-sm" onclick="onSearch()">
                <i class="am-icon-search"></i>
                物流查询
            </button>

            </center>

      

        </fieldset>
   


    <div class="am-panel am-panel-default"  >

        <div class="am-panel-bd" id="codelListArea">
       
         
       </div>

    </div>
    <hr data-am-widget="divider" style="" class="am-divider am-divider-dashed" />
    <footer data-am-widget="footer" class="am-footer am-footer-default">
        <div class="am-footer-miscs ">
        </div>
    </footer>



 </form>

        <script type="text/javascript">

        function onSearch() {

            var username = "";


            var code = $("#txt_fw_code").val();
            var userid = $("#wlcxzh").val();
            var passwd = $("#wlcxmm").val();


            var url = "../App/wl.aspx";
            $.ajax({
                type: "post",
                url: url,
                data: { "cu_no": username, "c": code, "u": userid, "p": passwd },
                timeout: 30000,
                datatype: "html",
                success: function (msg) {


                    $("#codelListArea").html(msg);


                }


            });
        }


    </script>


</body>
</html>



