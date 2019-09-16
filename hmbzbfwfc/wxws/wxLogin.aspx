<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxLogin.aspx.cs" Inherits="UI.wxws.wxLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!doctype html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>经销商登录 </title>
    <link rel="stylesheet" href="css/Jingle.css?v=4" />
    <link rel="stylesheet" href='css/app.css?v=4' />
    <link href="css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css" />

    
    <script type="text/javascript" src="js/jquery-1.9.0.min.js"></script>


    <script type="text/javascript" src="js/alertify.js"></script>



    <style>
        #loginHtml {
            width: 90%;
            margin: 10px auto;
        }

        .tipBox {
            width: 90%;
            margin: 0 auto;
        }

        p.tip {
            line-height: 1.5em;
            font-weight: 600;
        }
    </style>

  

    <script type="text/javascript">
        $(function () {
            $('#UserName').val(localStorage["hmcu_no"]);
            $('#password').val(localStorage["hmpassword"]);
            viewLogin();
        });

        function viewLogin() {

            $('#btnlogin').on('click', function () {

                //alertify.alert('正在登录中...');
                var UserName = $("#UserName").val();
                var password = $("#password").val();

                $('#btnlogin').attr('disabled', 'disabled');
                $('#btnlogin').val('正在登录...');
                $('#btnlogin').addClass('asbestos');
                try {
                    $.ajax({
                        type: 'POST',
                        url: 'wxAPI/Manager.aspx',
                        data: { action: 'login', password: password, UserName: UserName },
                        dataType: 'json',
                        timeout: 30000,
                        success: function (data) {
                            if (data.success) {
                                initLogin();
                                window.localStorage["hmcu_no"] = data.cu_no;
                                window.localStorage["hmpassword"] = password;

                                window.localStorage["xtuser_id"] = "";


                                location.href = "Agent_Index.aspx";
            

                            } else {
                                initLogin();
                                alertify.alert(data.Message);
                            }

                        },
                        error: function (xhr, type) {
                            initLogin();
                            alertify.error('超时,或服务错误');
                        }
                    });

                } catch (e) {
                    initLogin();
                    alert(e);
                }

            });
        }

        function initLogin() {
            $('#btnlogin').val('登录');
            $('#btnlogin').removeAttr('disabled');
            $('#btnlogin').removeClass('asbestos');
        }

    </script>

</head>
<body>
    <div id="aside_container">
    </div>
    <div id="section_container">
        <section id="index_section" class="active"> 
         <header>
            <nav class="left">
                  
            </nav>
            <h1 class="title">
               经销商登录
            </h1>
            <nav class="right">
                <a data-target="section" data-icon="info" href="#">
                    
                </a>
            </nav>
        </header>
 <div class="contents">
 			<div class="info_head_img" id="info_head_img">
			     <i class="icon user"></i>
			</div>  
        <div id="loginHtml">  
        <input type="hidden" name="url" id="url" value=""/>
        <input name="UserName"  style="border:1px solid rgba(0,0,0,.2)" type="text" id="UserName" maxlength="20" placeholder="代理账号"/>
        <br>
        <input name="password" style="border:1px solid rgba(0,0,0,.2)" type="password" maxlength="20" id="password" placeholder="密    码"/>
        <br>
        <p style="margin: 10px auto;"><input type="button" value="登录" class="button block" id="btnlogin"/> </p>
        </div>
                <div class="tipBox">

    </div>
 </div>
 
 

    </section>

    </div>

    <div id="jingle_toast" class="success" style="display: none;">
    </div>
    <div id="jingle_popup" style="display: none;" class="loading">
        <i class="icon spinner"></i>
        <p>
            加载中...
        </p>
    </div>
    <div id="jingle_popup_mask" style="opacity: 0.1; display: none;">
    </div>
</body>
</html>
