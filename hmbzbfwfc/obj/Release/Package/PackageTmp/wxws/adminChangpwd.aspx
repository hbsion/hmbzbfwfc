<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminChangpwd.aspx.cs" Inherits="UI.wxws.adminChangpwd" %>

<!DOCTYPE html >

<html>
<head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
<meta content="yes" name="apple-mobile-web-app-capable" /><meta content="black" name="apple-mobile-web-app-status-bar-style" /><meta content="telephone=no" name="format-detection" />
<title>
	修改密码
</title>
<link rel="stylesheet" href="css/Jingle.css?r=20150505"/>
  <link rel="stylesheet" href='css/app.css?r=2' /> 
   
    <link href="css/alertify.core.css" rel="stylesheet" type="text/css"/>
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css"/>
<script type="text/javascript" src="js/zepto.js"></script>
 
<script type="text/javascript" src="js/iscroll.js"></script>
  
<script type="text/javascript" src="js/alertify.js"></script>
<script type="text/javascript" src="js/comm.js?r=20150616"></script>
 
 


    <script type="text/javascript"> 
         
        $(function() { 

            $('#madifyBtn').on('click', function () {
                 updatePassword();
            });
        });

        /*
        *修改密码
        */
        function updatePassword() {
            var OldPassWord = $.trim($('#OldPasswd').val());
            
            var NewPassWord = $.trim($('#NewPassWord').val());
            var NewPassWord2 = $.trim($('#NewPassWord2').val());
            
            var gadmin =  localStorage["xtuser_id"] ;
         
            if  (NewPassWord!=NewPassWord2)
            
           {
               alertify.alert('两次密码不一致!');
               
               return ;
           }
            
            
            try {

                $.ajax({
                    type: 'POST',
                    url: '../App/AppChpass.ashx',
                    data: { "cu_no": '', "gadmin": gadmin, "opass": OldPassWord,"npass": NewPassWord},
                    dataType: 'html',
                    timeout: 30000,
                    success: function (data) {

                        if (data=="OK") {

                            alertify.alert("修改成功!", function () {
                             
                                location.href = "admin_Index.aspx";
                                
                                
                            });

                        } 
                        else {

                            alertify.alert(data);
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
                <h1 class="title">修改密码
                </h1>
                <nav class="right">
                    <a data-target="section" data-icon="info" href="#" id="manualBtn" ></a>
                </nav>

            </header>

            <div   style="margin: 0 auto;padding: 5px;width: 96%;">

                
                   <p style="height: 10px;"></p>
            <div class="input-group"  >
             
                  <div class="input-row" >
                   <label>旧密码</label>
                    <input type="password" value="" id="OldPasswd"      placeholder="旧密码">
                     
                </div>  
                
                
               <div class="input-row" >
                   <label>新密码</label>
                    <input type="password" value="" id="NewPassWord"      placeholder="新密码">
                     
                </div>  
                
                
                  <div class="input-row" >
                   <label>确认新密码</label>
                    <input type="password"  value="" id="NewPassWord2"     placeholder="确认新密码">
                     
                </div> 
              </div>
                
                 <p style="height: 10px;"></p>
                 <input type="submit" value="修改密码" id="madifyBtn"   class="button block"/>  

            </div>
        </section>


    </div> 
 

</body>
</html>
