<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="UI.w.index" %>

<!DOCTYPE html>

<html>
<head>
    <title>内蒙古骆驼酒业集团股份有限公司 </title>
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Cache-control" content="no-cache" />
    <meta http-equiv="Cache" content="no-cache" />

    <link href="css/bootstrap.min.css" rel="stylesheet" />

    <link rel="stylesheet" type="text/css" href="css/darkblue.css" />
    <script src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }

        .jiage {
            padding: 0px;
        }

        .djxq {
            margin: 0px;
        }
    </style>

</head>
<body>
    <form id="form1">

        <div id="myCarousel" class="carousel slide" data-ride="carousel">
            <ol class="carousel-indicators" id="ptop" runat="server">
            </ol>

            <div class="carousel-inner" id="demo3" runat="server">
            </div>

            <a class="carousel-control left" href="#myCarousel" data-slide="prev">&lsaquo;</a>
            <a class="carousel-control right" href="#myCarousel" data-slide="next">&rsaquo;</a>
        </div>




        <script type="text/javascript">
            $(".carousel").carousel({
                interval: 2000
            });
        </script>


        <div class="container ">


            <div class="row" style="height: 50px;">
            </div>

            <div class="row mokuairow ">
                <div class="col-xs-4 tjmoduleicon">
                    <div class="tjmoduleiconsingle">
                        <a target="_self" href="fw.aspx">
                            <img src="img/a01.png"></a>
                        <div class="classicon"><span><strong>防伪查询</strong></span></div>
                    </div>
                </div>
                <div class="tjmoduleicon col-xs-4">
                    <div class="tjmoduleiconsingle">
                        <a target="_self" href="v.aspx?b=A013">
                            <img src="img/a02.png"></a>
                        <div class="classicon"><span><strong>最新活动</strong></span></div>
                    </div>
                </div>

                <div class="tjmoduleicon col-xs-4">
                    <div class="tjmoduleiconsingle">
                        <a target="_self" href="fc.aspx">
                            <img src="img/a03.png"></a>
                        <div class="classicon"><span><strong>物流查询</strong></span></div>
                    </div>
                </div>
            </div>

            <div class="row" style="height: 20px;">
            </div>

            <div class="row mokuairow ">
                <div class="col-xs-4 tjmoduleicon">
                    <div class="tjmoduleiconsingle">
                        <a target="_self" href="v.aspx?b=A012">
                            <img src="img/c02.png"></a>
                        <div class="classicon"><span><strong>热卖产品</strong></span></div>
                    </div>
                </div>
                <div class="tjmoduleicon col-xs-4">
                    <div class="tjmoduleiconsingle">
                        <a target="_self" href="v.aspx?b=A008">
                            <img src="img/b02.png"></a>
                        <div class="classicon"><span><strong>联系我们</strong></span></div>
                    </div>
                </div>
                <div class="tjmoduleicon col-xs-4">
                    <div class="tjmoduleiconsingle">
                        <a target="_self" href="v.aspx?b=A001">
                            <img src="img/b03.png"></a>
                        <div class="classicon"><span><strong>企业介绍</strong></span></div>
                    </div>
                </div>
            </div>

        </div>


        <div class="row" style="height: 60px;">
        </div>

        <nav class="navbar navbar-default navbar-fixed-bottom">

            <div id="jszc" class="duihuan_footer  ">

                <p class="copyright " >
                    <b style="color:#4F9243;">技术支持：天津高鑫鸿儒防伪科技有限公司</b>
                    <br />
                </p>
            </div>
        </nav>
    </form>

</body>
</html>
