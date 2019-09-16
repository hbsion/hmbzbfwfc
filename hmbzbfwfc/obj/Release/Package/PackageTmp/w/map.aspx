<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="map.aspx.cs" Inherits="UI.map" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="user-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />

    <title>地图</title>

    <link href="../Content/css/bootstrap.min.css" rel="stylesheet" />

    <script src="../Content/js/jquery.min.js" type="text/javascript"></script>




    <style type="text/css">
        html, body, form {
            height: 100%;
            margin: 0;
            padding: 0;
        }

        #dituContent {
            height: 100%;
        }
    </style>

</head>

<body>



    <div class="container-fluid" style="width: 100%; height: 100%; margin: 0; padding: 0;">

        <!--百度地图容器-->
        <div style="width: 100%; height: 100%;" id="container">
        </div>



    </div>





    <div style="display: none;">

          <input type="hidden" id="txtlongitude" value="39.916527" runat="server" />

        <input type="hidden" id="txtlatitude" value="116.397128" runat="server" />
    


    </div>


  <script charset="utf-8" src="http://map.qq.com/api/js?v=2.exp"></script>
<script>

    window.onload = function () {

        //直接加载地图


        //初始化地图函数  自定义函数名init
        function init() {
            //定义map变量 调用 qq.maps.Map() 构造函数   获取地图显示容器

            var center = new qq.maps.LatLng($('#txtlatitude').val(), $('#txtlongitude').val());

            var map = new qq.maps.Map(document.getElementById("container"), {
                center: center,      // 地图的中心地理坐标。
                zoom: 13                                                // 地图的中心地理坐标。
            });

            var marker = new qq.maps.Marker({
                position: center,
                map: map
            });

        }

        //调用初始化函数地图
        init();


    }
</script>


   




</body>


</html>



