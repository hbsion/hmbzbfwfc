<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modifyPassword.aspx.cs" Inherits="UI.wxws.modifyPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>确认修改密码
    </title>
    <link rel="stylesheet" href="css/Jingle.css?r=20150505" />
    <link rel="stylesheet" href='css/app.css?r=2' />

    <link href="css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/zepto.js"></script>

    <script type="text/javascript" src="js/iscroll.js"></script>

    <script type="text/javascript" src="js/alertify.js"></script>
    <script type="text/javascript" src="js/comm.js?r=20150616"></script>



    <link href="css/fonts.css" rel="stylesheet" />


    <script type="text/javascript">
        var mycu_no = window.localStorage.getItem("hmcu_no");
        $(function () {
            loadData();
            $('#madifyBtn').on('click', function () {
                updatePassword();
            });



        });
        /*
        *修改密码
        */
        function updatePassword() {
            var OldPassWord = $("#OldPassWord").val();
            var NewPassWord = $('#NewPassWord').val();
            if (OldPassWord == "" || NewPassWord == "") {
                alertify.alert("修改的密码长度不可以为空!");
                $("#OldPassWord").focus();
                return;
            }
            try {

                $.ajax({
                    type: 'POST',
                    url: 'wxAPI/Manager.aspx',
                    data: { action: 'modifypwd', OldPassWord: OldPassWord, NewPassWord: NewPassWord },
                    dataType: 'json',
                    timeout: 30000,
                    success: function (data) {

                        if (data.success) {

                            alertify.alert(data.Message, function () {
                                location.href = "wxLogin.aspx";
                            });
                        }
                    },
                    error: function (xhr, type) {

                        alertify.error('超时,或服务错误');
                    }
                });

            } catch (e) {

                alert(e);
            }
        }
        function loadData() {
            $.ajax({
                type: 'POST',
                url: 'wxAPI/Manager.aspx',
                data: { action: 'list', cu_no: mycu_no },
                dataType: 'json',
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        $(".info").html("<b id='cuname'>你好," + data.name + "</b></p>");

                    } else {
                        alertify.alert(data.Message);
                        location.href = "wxLogin.aspx";
                    }

                },
                error: function (xhr, type) {
                    alert(type);
                    location.href = "wxLogin.aspx";
                    alertify.error('超时,或服务错误');
                }
            });
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
                    <a href="javascript:history.go(-1)" data-icon="previous" data-target="back"><i class="icon previous"></i>返回</a>
                </nav>
                <h1 class="title">确认修改密码
                </h1>
                <nav class="right">
                    <a data-target="section" data-icon="info" href="#" id="manualBtn" > </a>
                </nav>

            </header>
           <div class="member_info">
         
            <div class="info">

            </div>
        </div>
            <div   style="margin: 0 auto;padding: 5px;width: 96%;">

                
                   <p style="height: 10px;"></p>
            <div class="input-group"  >
             
               <div class="input-row" >
                   <label>新密码</label>
                    <input type="password" value="" id="OldPassWord" name="OldPassWord"    placeholder="新密码">
                     
                </div>  
                  <div class="input-row" >
                   <label>确认新密码</label>
                    <input type="password"  value="" id="NewPassWord" name="NewPassWord"    placeholder="确认新密码">
                     
                </div> 
              </div>
                
                 <p style="height: 10px;"></p>
                 <input type="submit" value="修改密码" id="madifyBtn"   class="button block"/>  

            </div>
             


        </section>


    </div>


</body>
</html>
