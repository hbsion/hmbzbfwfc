<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agent_Index.aspx.cs" Inherits="UI.wxws.Agent_Index" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>门店后台 </title>

    
    <link href="bootstrap-v3.3.7/css/bootstrap.min.css" rel="stylesheet" type="text/css" />


    <link rel="stylesheet" href="css/Jingle.css" />
    <link rel="stylesheet" href='css/app.css' />
    <link href="css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript" src="js/jquery-1.9.0.min.js"></script>

    <script type="text/javascript" src="js/alertify.js"></script>


    <script type="text/javascript">
        var mycu_no = window.localStorage.getItem("hmcu_no");
        $(function () {
            //alert(window.localStorage["user"]);
            loadData();
            $("#section_container .right a").on("click", function () {
                alertify.confirm("确认退出系统吗?", function (e) {
                    if (e) {
                        loginOut();
                    }
                    else {//不执行动作
                    }
                });
            })


        })
        function loadData() {
            $.ajax({
                type: 'POST',
                url: 'wxAPI/Manager.aspx',
                data: { action: 'list', cu_no: mycu_no },
                dataType: 'json',
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        $(".info").html("<b id='cuname'>你好," + data.name + "</b>");
                        window.localStorage.setItem("cu_name", data.name);

                    } else {
                        alertify.alert(data.Message);
                        location.href = "wxLogin.aspx";
                    }

                },
                error: function (xhr, type) {
                    //alert(type);
                    alertify.error('超时,或服务错误');
                    location.href = "wxLogin.aspx";
                }
            });





        }



        function loginOut() {
            $.ajax({
                type: 'POST',
                url: 'wxAPI/Manager.aspx',
                data: { action: 'loginOut' },
                dataType: 'text',
                //timeout: 30000,
                success: function (data) {
                    if (data == "OK")
                        localStorage["hmcu_no"] = "";
                    location.href = "wxLogin.aspx";
                }
            });
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
                  <a href="javascript:history.go(-1); " data-icon="previous" data-target="back"><i class="icon previous"></i>返回</a>
            </nav>
            <h1 class="title">
               门店后台
            </h1>
            <nav class="right">
                <a data-target="section" data-icon="info"  href="javascript:void(0)">
                    退出
                </a>
            </nav>

        </header>  
  
   <div class="member_info">
         
            <div class="info">
            </div>
        </div>
   <ul class="list inset app-list" >


  
       
            <li style="display:none;">
               <i class="icon next"></i>
                <span class="icon user"></span>
                  <a href="wxCustomerBrandList.aspx" >
                     <strong>下级经销查看</strong>
                      <p>下级经销查看</p>
                </a>
            </li>

  
             <li>                
                <i class="icon next" ></i>
                <span class="icon qrcode"></span>
                <a href="wx_cushipScan.aspx" >
                    <strong>销售扫码</strong> 
                     <p>扫描大箱条码或产品条码销售</p>
                </a>
            </li>
             
            <li id="r05">

                    <i class="icon next"></i>
                    <span class="icon cart"></span>
                    <a href="wx_cutstScan.aspx">
                        <strong>调拨扫描</strong>
                        <p>扫描大箱条码或产品调拨其他门店</p>
                    </a>
            </li>     

            <li>                
                <i class="icon next" ></i>
                <span class="icon qrcode"></span>
                <a href="wx_cureScan.aspx" >
                    <strong>退货扫码</strong> 
                     <p>扫描大箱条码或产品条码退货</p>
                </a>
            </li>
            
               <li id="r06" >
                    <i class="icon next"></i>
                    <span class="icon qrcode"></span>
                    <a href="wx_prodList.aspx">
                        <strong>销售列表</strong>
                        <p>查看销售数据</p>
                    </a>
                </li>
                



                <li id="r04" >
                    <i class="icon next"></i>
                    <span class="icon search"></span>
                    <a href="xt_queryRecord.aspx">
                        <strong>物流查询</strong>
                        <p>查询发货信息</p>
                    </a>
                </li>


            
           <li>
                
                <i class="icon next"></i>
                <span class="icon pencil-2"></span>
                <a href="modifyPassword.aspx" >
                    <strong>修改密码</strong> 
                    <p>修改登录密码</p>
                </a>
            </li> 
        </ul>

    </section>
    </div>
</body>
</html>
