<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_Index.aspx.cs" Inherits="UI.wxws.admin_Index" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>总部后台 </title>
    <link href="bootstrap-v3.3.7/css/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.9.0.min.js"></script>

    <link rel="stylesheet" href="css/Jingle.css?r=20150505" />
    <link rel="stylesheet" href='css/app.css?r=2' />
    <link href="css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/alertify.js"></script>

    <script type="text/javascript">
        $(function () {


            var admin = window.localStorage["xtuser_id"];
            if (admin == null || admin == "undefined" || admin == "") {
                location.href = "AdminLogin.aspx";
                return false;
            }



            $(".info").html("<b id='cuname'>总部：" + admin + "</b>");

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



        function loginOut() {
            $.ajax({
                type: 'POST',
                url: 'wxAPI/Manager.aspx',
                data: { action: 'loginOut' },
                dataType: 'text',
                //timeout: 30000,
                success: function (data) {
                    if (data == "OK") {
                        localStorage["hmcu_no"] = "";
                    }
                    localStorage["xtuser_id"] = "";
                    location.href = "AdminLogin.aspx";
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
                <h1 class="title">总部后台
                </h1>
                <nav class="right">
                    <a data-target="section" data-icon="info" href="javascript:void(0)">退出
                    </a>
                </nav>

            </header>

            <div class="info" style="height: 30px;">
            </div>

            <ul class="list inset app-list">

                <li id="r01">
                    <i class="icon next"></i>
                    <span class="icon podcast"></span>
                    <a href="wx_fgiScan.aspx">
                        <strong>入库扫描</strong>
                        <p>扫描产品入库</p>
                    </a>
                </li>


                <li id="r02" style="display:none;">

                    <i class="icon next"></i>
                    <span class="icon qrcode"></span>
                    <a href="wx_shipScan.aspx">
                        <strong>直接出货</strong>
                        <p>出货及入库扫描大箱条码或产品条码发货给下级经销</p>
                    </a>
                </li>
                <li id="r07">

                    <i class="icon next"></i>
                    <span class="icon qrcode"></span>
                    <a href="wx_kcshipScan.aspx">
                        <strong>库存出货</strong>
                        <p>扫描大箱条码或产品条码发货给下级经销</p>
                    </a>
                </li>

               <li id="r03">

                    <i class="icon next"></i>
                    <span class="icon book"></span>
                    <a href="wx_reScan.aspx">
                        <strong>退货扫描</strong>
                        <p>扫描大箱条码或产品条码退货入库</p>
                    </a>
                </li>


                 <li id="r05" style="display:none;">

                    <i class="icon next"></i>
                    <span class="icon cart"></span>
                    <a href="wx_tstScan.aspx">
                        <strong>调拨扫描</strong>
                        <p>扫描大箱条码或产品调拨库别</p>
                    </a>
                </li>



                <li id="r04">
                    <i class="icon next"></i>
                    <span class="icon search"></span>
                    <a href="xt_queryRecord.aspx">
                        <strong>物流查询</strong>
                        <p>查询发货信息</p>
                    </a>
                </li>


                <li id="r06">
                    <i class="icon next"></i>
                    <span class="icon bell"></span>
                    <a href="wx_scanlist.aspx">
                        <strong>扫描</strong>
                        <p>查询入库出库退货调拨扫描明细</p>
                    </a>
                </li>


                <li>

                    <i class="icon next"></i>
                    <span class="icon pencil-2"></span>
                    <a href="adminChangpwd.aspx">
                        <strong>修改密码</strong>
                        <p>修改登录密码</p>
                    </a>
                </li>
            </ul>

        </section>
    </div>
</body>
</html>
