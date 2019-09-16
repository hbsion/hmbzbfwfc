<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sy.aspx.cs" Inherits="UI.w.sy" %>

<!DOCTYPE html>
<html>
<head>
    <title>产品溯源查询</title>
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
        <h1 class="am-header-title" id="fangwei_title" style="  margin: 0;font-size:1.2em;">溯源查询</h1>
    </header>
    <div id="pdm_slider_index" class="am-slider  am-slider-a5" data-am-slider='{&quot;directionNav&quot;:false}'>
        <ul class="am-slides">
            <li>&nbsp;</li>

        </ul>
    </div>
    <article data-am-widget="paragraph" class="am-paragraph am-paragraph-default" data-am-paragraph="{ tableScrollable: true, pureview: true }" id="querynotresult" style="display:none;">
        <hr data-am-widget="divider" style="" class="am-divider am-divider-dashed" />
        <span id="span_notresult"></span>
        <hr data-am-widget="divider" style="" class="am-divider am-divider-dashed" />
    </article>
    <div class="am-panel-group" id="queryresult">

        <div class="am-panel am-panel-default">
            <div class="am-panel-hd">
                <h4 class="am-panel-title" data-am-collapse="{parent: '#accordion', target: '#do-not-say-1'}">
                    <span>基本信息</span>
                </h4>
            </div>
            <div id="do-not-say-1" class="am-panel-collapse am-collapse">
                <div class="am-panel-bd">
                    <ul class="am-list am-list-static" id="ul_base">


                    </ul>
                </div>
            </div>
        </div>
        <div class="am-panel am-panel-default" id="div_productimg">
            <div class="am-panel-hd">
                <h4 class="am-panel-title" data-am-collapse="{parent: '#accordion', target: '#do-not-say-5'}">
                    <span>产品图片</span>
                </h4>
            </div>
            <div id="do-not-say-5" class="am-panel-collapse am-collapse">
                <div class="am-panel-bd" style="padding-top: 0px;">
                    <ul data-am-widget="gallery" class="am-gallery am-avg-sm-2 am-avg-md-3 am-avg-lg-4 am-gallery-overlay" data-am-gallery="{ pureview: true }" id="ul_productimage"></ul>
                </div>
            </div>
        </div>


        <div class="am-panel am-panel-default">
            <div class="am-panel-hd">
                <h4 class="am-panel-title" data-am-collapse="{parent: '#accordion', target: '#do-not-say-2'}">
                    <span>原料信息</span>
                </h4>
            </div>
            <div id="do-not-say-2" class="am-panel-collapse am-collapse">
                <div class="am-panel-bd" style="padding-top: 0px;">
                    <ul class="am-comments-list am-comments-list-flip" id="ul_source"></ul>
                </div>
            </div>
        </div>
        <div class="am-panel am-panel-default">
            <div class="am-panel-hd">
                <h4 class="am-panel-title" data-am-collapse="{parent: '#accordion', target: '#do-not-say-3'}">
                    <span>生产流程</span>
                </h4>
            </div>
            <div id="do-not-say-3" class="am-panel-collapse am-collapse">
                <div class="am-panel-bd" style="padding-top: 0px;">
                    <ul class="am-comments-list am-comments-list-flip" id="ul_process"></ul>
                </div>
            </div>
        </div>
        <div class="am-panel am-panel-default">
            <div class="am-panel-hd">
                <h4 class="am-panel-title" data-am-collapse="{parent: '#accordion', target: '#do-not-say-4'}">
                    <span>质量检验</span>
                </h4>
            </div>
            <div id="do-not-say-4" class="am-panel-collapse am-collapse">
                <div class="am-panel-bd" style="padding-top: 0px;">

                    <ul class="am-list am-list-static am-list-border" id="ul_test" style="padding-top:5px;"></ul>
                    <ul data-am-widget="gallery" class="am-gallery am-avg-sm-2 am-avg-md-3 am-avg-lg-4 am-gallery-bordered" data-am-gallery="{  }" id="ul_testimage"></ul>
                </div>
            </div>
        </div>
    </div>




    <footer data-am-widget="footer" class="am-footer am-footer-default">
        <div class="am-footer-miscs ">

        </div>
    </footer>

 
</body>
</html>



  