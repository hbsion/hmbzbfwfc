<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScanTstlist.aspx.cs" Inherits="UI.wxws.ScanTstlist" %>

<!DOCTYPE html >
<html lang="zh-CN">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />
    <meta content="telephone=no" name="format-detection" />
    <meta content="no-cache" http-equiv="Pragma" />
    <meta content="no-cache" http-equiv="Cache-Control" />
    <title>调拨扫描明细</title>
    <link href="../Content/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../wxws/css/common.css" rel="stylesheet" />



    <script src="../Content/js/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="../Content/js/jquery-jtemplates.js"></script>

    <link href="../wxws/css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="../wxws/css/alertify.default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../wxws/js/alertify.js"></script>

    <script src="../Content/js/bootstrap.min.js" type="text/javascript"></script>

    <script>


        $(function () {

            $('#loadmodal').modal({
                backdrop: 'static',
                keyboard: false,
                show: false
            });


            loadDataList();




        });



        function loadDataList() {

            $("#loadmodal").modal('show');

            var user_id = localStorage["xtuser_id"];


            var wheresql = " unitcode='" + user_id + "'";

            var url = "../App/appListOf.ashx";
            $.ajax({
                type: "post",
                url: url,
                data: { "key": "ScanTstlist", "query": "", "PageIndex": 1, "PageSize": 120, "wheresql": wheresql },
                timeout: 30000,
                datatype: "json",
                success: function (msg) {


                    var dataJson = $.parseJSON(msg);

                    if (dataJson.success) {

                        var datauceBeanList = dataJson.cont;

                        $("#result_temp").setTemplateElement("templateContainer");
                        $("#result_temp").processTemplate("");
                        if (datauceBeanList != null && datauceBeanList.length > 0) {
                            $("#result_temp").processTemplate(datauceBeanList);
                            $("#result").append($("#result_temp").html());

                        }
                    }


                    $("#loadmodal").modal('hide');


                }
            });
        }





    </script>


</head>


<body>
    <form id="Form2" runat="server">

        <div class="container-fluid">

            <div class="row" style="height: 40px; background-color: #4F81BD; text-align: center; line-height: 40px; color: #ffffff;">
                <div class="col-xs-1">
                    <img src="images/back.png" onclick="history.go(-1);" />
                </div>
                <div class="col-xs-10">调拨扫描明细 </div>
                <div class="col-xs-1"></div>

            </div>



            <div class="row" id="pobody">

                <!--明细显示-->
                <div id="result"></div>
                <div id="result_temp" style="display: none"></div>

            </div>

        </div>

        <textarea id="templateContainer" style="display: none;"> 
    {#foreach $T as row}
    <div class="container"  id="div_{$T.row.fid}" style="margin-bottom:5px; background:#eeeeee" >
    
          <div class="row"   style="margin:0px 5px 5px 5px;" >
                  <div  >
                  单号：  {$T.row.ship_no}
		         </div >
		               
                 <div  >
                  日期：  {$T.row.ship_date}
		         </div>
	               <div  >
                  数量（瓶）：  {$T.row.mqty}
		         </div>	         

	               <div  >
                  数码：  {$T.row.bsnno}
		         </div>	

	            <div  >
                  原库别：  {$T.row.ost_no}
		         </div>	
	            <div  >
                  新库别：  {$T.row.nst_no}   
		         </div>	
		         
          </div>
        
           
           <div class="row"  id="stau_{$T.row.order_id}" style="margin:0px 5px 5px 5px;">
        
              

          </div>
          
            
          
     </div>   
     


	<br />
	 
    {#/for}
    
    </textarea>
        <!--结果显示-->



        <!--弹窗遮罩-->
        <div class="modal fade bs-example-modal-sm" tabindex="-1" id="loadmodal" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content" style="margin-top: 50%; text-align: center; color: #F63; font-weight: bold; background-color: transparent;">
                    正在加载,请稍侯.....        
                </div>
            </div>
        </div>
        <!--弹窗遮罩-->

        <div style="display: none;">

            <input type="text" id="PageIndex" value="0" />
            <input type="text" id="PageSize" value="20" />

            <input type="text" id="txthy_no" runat="server" />

            <input type="text" id="wx_id" runat="server" />


        </div>

    </form>





</body>
</html>


