<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_Register.aspx.cs" Inherits="UI.wxws.wx_Register" %>

<!DOCTYPE html>
<html>
<head>
    <title>新增经销商资料</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />


    <link href="bootstrap-v3.3.7/css/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <link href="css/hmcomm.css" rel="stylesheet" />


    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>


    <script src="js/bootstrap-show-password.min.js" type="text/javascript"></script>



    <script src="layer/layer.js" type="text/javascript"></script>

    <link href="layer/skin/layer.css" rel="stylesheet" type="text/css" />


    <script src="js/register.js?v=626785526" type="text/javascript"></script>

  

 


      <script type="text/javascript">


          $(function () {

              select1();

              $('#txtpwd').password()
              .password('focus');




          });


        function select1() {
            $.ajax(
           {
               type: "get",
               url: "../selectitem/PTDselect.ashx",
               dataType: "json",
               data: { "type": "province" },
               success: function (msg) {


                   for (var i = 0; i < msg.length; i++) {

                       if (msg[i].ProvinceName == $("#hid_province").val())
                           $("#province").append("<option selected='selected' value=" + msg[i].ProvinceID + ">" + msg[i].ProvinceName + "</option>");
                       else
                           $("#province").append("<option value=" + msg[i].ProvinceID + ">" + msg[i].ProvinceName + "</option>");

                   }
                   select2();
               }
           })
        };


        function select2() {
            //alert("a");
            $("#City").html("");
            var hid_province = $("#hid_province");
            var Pro = $("#province option:selected");
            var pID = $("#province").val();
            hid_province.val(Pro.text());
            $.ajax(
           {
               type: "get",
               dataType: "json",
               url: "../selectitem/PTDselect.ashx",
               data: { "type": "city", "provinceID": pID },
               success: function (msg) {


                   for (var i = 0; i < msg.length; i++) {
                       if (msg[i].CityName == $("#hid_city").val())
                           $("#City").append("<option selected='selected'  value=" + msg[i].CityID + ">" + msg[i].CityName + "</option>");
                       else
                           $("#City").append("<option value=" + msg[i].CityID + ">" + msg[i].CityName + "</option>");
                   }



               }
           })

            $("#City").prepend("<option value='-1'>---请选择---</option>");

        };

        function select3() {


            var hid_city = $("#hid_city");
            var city = $("#City option:selected");
            hid_city.val(city.text());
        };
    </script>

</head>
<body>


    <!-- 头部开始 -->
    <div class="container-fluid">
        <div class="row text-center hmheader">
            <div class="col-xs-2 hmheader_left" onclick="javascript:history.go(-1);">
                <span class="glyphicon glyphicon-menu-left"></span>
            </div>
            <div class="col-xs-8 hmheader_center" id="mytitle" runat="server">
                新增经销商
            </div>
            <div class="col-xs-2 hmheader_right">
            </div>
        </div>
    </div>
    <!-- 头部结束 -->




    <form id="Form1" runat="server">

        <div class="container" id="myreg">

            <div class="panel panel-default">
                <div class="panel-heading">经销商资料</div>
                <div class="panel-body">

                    <div class="form-group">
                        <label>*登录号(手机号码)：</label>
                        <input type="text" class="form-control" runat="server" id="txtcu_no" placeholder="登录号为手机号码" autocomplete="off" />
                    </div>

                    <div class="form-group">
                        <label>*登录密码：</label>
                        <input type="password" class="form-control" runat="server" id="txtpwd" placeholder="输入登录密码" autocomplete="off" />
                    </div>




                    <div class="form-group">
                        <label>*姓 名：</label>
                        <input type="text" class="form-control" runat="server" id="txtname" placeholder="输入姓 名" autocomplete="off" />
                    </div>




                    <div class="form-group">
                        <label>收货地址：</label>

                        <div style="line-height: 2em; background: #DEDEDE;">
                            <div>
                                <div class="mydiv">省份：</div>

                                <input type="hidden" id="hid_province" value="" runat="server" />
                                <input type="hidden" id="hid_city" value="" runat="server" />
                                <select id="province" runat="server" onchange="select2();" style="height: 23px; width: 200px;">
                                    <option value="-1">---请选择---</option>
                                </select>

                                <div style="clear: both;"></div>

                            </div>
                        </div>


                        <div style="line-height: 2em; background: #DEDEDE;">
                            <div>
                                <div class="mydiv">城市：</div>

                                <select id="City" runat="server" onchange="select3();" style="height: 23px; width: 200px;">
                                    <option value="-1">---请选择---</option>
                                </select>

                                <div style="clear: both;"></div>
                            </div>

                        </div>

                        <input type="text" class="form-control" runat="server" id="addr" placeholder="输入您的详细地址" />
                    </div>


                </div>
            </div>

        </div>

        <div class="row text-center">
            <button type="button" class="btn btn-success btn-sm" id="btnApply" style="width: 90%; height: 3em;" onclick="doEdit();">提交注册</button>
        </div>


        <div class="row">

            <asp:HiddenField ID="txtprov" runat="server" />
            <asp:HiddenField ID="txtcity" runat="server" />
            <asp:HiddenField ID="hiddLocalIds" runat="server" />
            <asp:HiddenField ID="hiddLocalIds2" runat="server" />
            <asp:HiddenField ID="hiddServerId" runat="server" />
            <asp:HiddenField ID="hiddServerId2" runat="server" />

            <asp:HiddenField ID="txtxtcu_no" runat="server" />
           <asp:HiddenField ID="stype" runat="server" />


        </div>


    </form>

</body>
</html>
