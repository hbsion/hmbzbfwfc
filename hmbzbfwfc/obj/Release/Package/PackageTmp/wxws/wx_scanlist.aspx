<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_scanlist.aspx.cs" Inherits="UI.wxws.wx_scanlist" %>

<!DOCTYPE html >
<html>
<head>
    <title>扫描查询</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <link rel="stylesheet" href="css/Jingle.css" />
    <link rel="stylesheet" href='css/app.css' />
    <link rel="stylesheet" href='css/wxApp.css' />

    <link href="css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/zepto.js"></script>

    <script type="text/javascript" src="js/iscroll.js"></script>

    <script type="text/javascript" src="js/alertify.js"></script>

    <script type="text/javascript" src="js/comm.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#mybtn").on("click", "li", function () {
                var id = this.id;
                if (id == "btn01") {
                    //入库明细
                    location.href = "ScanFgilist.aspx";
                } else if (id == "btn02") {
                    //出货明细
                    location.href = "ScanShiplist.aspx";
                }
                else if (id == "btn03") {
                    //退货明细
                    location.href = "ScanRelist.aspx";
                }
                else if (id == "btn04") {
                    //调拨明细
                    location.href = "ScanTstlist.aspx";
                }
                else if (id == "btn05") {
                    //库存查询
                    location.href = "InvStlist.aspx";
                }
            })
        });

    </script>

</head>
<body>

            <!-- 头部开始 -->
        <header>
            <nav class="left">
                <a href="javascript:history.go(-1);" data-icon="previous" data-target="back"><i class="icon previous"></i>返回 </a>
            </nav>
            <h1 class="title">扫描明细
            </h1>
        </header>
        <div class="c">
        </div>
        <!-- 头部结束 -->


 
    <div id="section_container">
        <section id="index_section" class="active">
 
            <ul class="list inset app-list" id="mybtn">
                <li id="btn01">
                    <i class="icon next"></i>
                    <span class="icon pencil"></span>
                    <a href="javascript:;">
                        <strong>入库明细</strong>
                        <p>查询入库明细</p>
                    </a>
                </li>
                <li id="btn02">
                    <i class="icon next"></i>
                    <span class="icon signup"></span>
                    <a href="javascript:;">
                        <strong>出货明细</strong>
                        <p>查询出货明细</p>
                    </a>
                </li>

                <li id="btn03">

                    <i class="icon next"></i>
                    <span class="icon stack"></span>
                    <a href="javascript:;">
                        <strong>退货明细</strong>
                        <p>查询出货明细</p>
                    </a>
                </li>
                <li id="btn04">
                    <i class="icon next"></i>
                    <span class="icon newspaper"></span>
                    <a href="javascript:;">
                        <strong>调拨明细</strong>
                        <p>查询调拨明细</p>
                    </a>
                </li>

                  <li id="btn05">
                    <i class="icon next"></i>
                    <span class="icon newspaper"></span>
                    <a href="javascript:;">
                        <strong>库存查询</strong>
                        <p>查询各仓库的库存</p>
                    </a>
                </li>


            </ul>

        </section>
    </div>
</body>
</html>
