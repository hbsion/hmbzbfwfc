<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="UI.wxws.adminlogin" %>


<!doctype html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>总部登录 </title>
    <link rel="stylesheet" href="css/Jingle.css?r=20150505" />
    <link rel="stylesheet" href='css/app.css?r=2' />
    <link href="css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.9.0.min.js"></script>



    <script type="text/javascript" src="js/alertify.js"></script>

    

    <style>
        #loginHtml
        {
            width: 90%;
            margin: 10px auto;
        }
    </style>


    <script type="text/javascript">
        $(function () {

            $('#UserName').val(localStorage["xtuser_id"]);
            $('#password').val(localStorage["hmpassword"]);

            viewLogin();
        });

        function viewLogin() {

            $('#btnlogin').on('click', function () {

                var UserName = $("#UserName").val();
                var password = $("#password").val();

                $('#btnlogin').attr('disabled', 'disabled');
                $('#btnlogin').val('正在登录...');
                $('#btnlogin').addClass('asbestos');
                try {

                    $.ajax({
                        type: 'POST',
                        url: '../App/AppAdminLogin.ashx',
                        data: { action: 'login', password: password, cu_no: UserName },
                        dataType: 'text',
                        timeout: 30000,
                        success: function (data) {

                            if (data.length > 0 && data.substring(0, 1) == "1") {
                                initLogin();
                                window.localStorage["hmcu_no"] = "";
                                window.localStorage["login_account"] = "";
                                window.localStorage["xtuser_id"] = UserName;
                                window.localStorage["hmpassword"] = password;

                                if (data.length >= 8)
                                    window.localStorage["myright"] = data.substring(2, 8);
                                else
                                    window.localStorage["myright"] = "00000";


                                location.href = "admin_Index.aspx";


                            } else {
                                initLogin();

                                alertify.alert("密码或账号错误!");
                            }

                        },
                        error: function (xhr, type) {
                            initLogin();
                            alert(type);
                            //alertify.error('超时,或服务错误');
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

 <div class="contents">
 			<div style="text-align:center; margin-bottom:2em;"  >
			   <!--<img src="images/admin_logo.png" alt="" />-->
                 <img src="../images/hlogo.png" style="width:80%" />
				 <h3 style="margin-top:50px;color:#e5007f;font-weight:bold">总部登录</h3>
			</div>  
        <div id="loginHtml"  >  
        <input type="hidden" name="url" id="url" value=""/>
        <input name="UserName" type="text" id="UserName" maxlength="20" placeholder="账  号"/>
        <br>
        <input name="password" type="password" maxlength="20" id="password" placeholder="密   码"/>
        <br>
        <p style="margin: 10px auto;"><input type="button" value="登录" class="button block" style="background:#e5007f" id="btnlogin"/> </p>
        
        </div>
 </div>
 
 

    </section>
    </div>
    <div id="jingle_toast" class="success" style="display: none;">
    </div>
    <div id="jingle_popup" style="display: none;" class="loading">
        <i class="icon spinner"></i>
        <p>
            加载中...</p>
    </div>
    <div id="jingle_popup_mask" style="opacity: 0.1; display: none;">
    </div>
</body>
</html>
