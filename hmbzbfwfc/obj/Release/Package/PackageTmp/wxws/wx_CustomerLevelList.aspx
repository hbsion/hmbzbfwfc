<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_CustomerLevelList.aspx.cs" Inherits="UI.wxws.wx_CustomerLevelList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" /><meta content="yes" name="apple-mobile-web-app-capable" /><meta content="black" name="apple-mobile-web-app-status-bar-style" /><meta content="telephone=no" name="format-detection" /><title>下级代理商-级别列表
</title>
 <link rel="stylesheet" href="css/Jingle.css"/>
    <link rel="stylesheet" href='css/app.css' />
    <link href="css/alertify.core.css" rel="stylesheet" type="text/css"/>
    <link href="css/alertify.default.css" rel="stylesheet" type="text/css"/>

    <script type="text/javascript" src="js/zepto.js"></script>

    <script type="text/javascript" src="js/iscroll.js"></script>

    <script type="text/javascript" src="js/alertify.js"></script>

    <script type="text/javascript" src="js/comm.js"></script>
 
 <script type="text/javascript">
   $(function(){
   loadLevelList();
   });
   function loadLevelList()
   {
     var cu_no=localStorage["hmcu_no"];
     if(cu_no=="undefined"){
       aleryify.alert("请登录!");
       location.href="wxLogin.aspx";
     }
     $.ajax({
         url: 'wxAPI/Manager.aspx',
         type: 'post',
         data: { "action": "level",cu_no:cu_no },
         dataType: 'json',
         timeout: 30000,
         success: function(data) {
             if (data.success) {
                 //alert(data.success);
                 var vhtml = "";
                 for (var n = 0; n < data.brandList.length; n++) {
                     vhtml += "<div class=\"weinxPlane\">";
                     vhtml += "<ul  class=\"list app-list\" >";
                     vhtml += "<li class=\"divider\">" + data.brandList[n].brand_name + "</li>";
                     for (var i = 0; i < data.brandList[n].brands.length; i++) {
                         vhtml += "<li>";
                         vhtml += "<i class=\"icon next\"></i>";
                         vhtml += "<a href=\"wxCustomerBrandList.aspx?brandId=" + encodeURIComponent(data.brandList[n].brand_no) + "&gclass=" + encodeURIComponent(data.brandList[n].brands[i].gclassName) + "\">";
                         vhtml += "<strong>" + data.brandList[n].brands[i].gclassName + "(" + data.brandList[n].brands[i].sumqty + ")</strong>\</a></li>";
                     }
                     vhtml += "</ul></div>";

                 }
                 $("#codelListArea").html(vhtml);
             }
             else {
                 alert(data.Messages);
             }
         },
         error: function(xrh, type) {
             alertify.alert(type);
             console.log(xrh['responseText']);
         }
     });
   }
   function CustomerGetCustomerList(brandNo,gclass)
   {
       $.ajax({
       url:'wxAPI/Manager.aspx',
       type:'post',
       data:{"action":"level2","brandId":brandNo,"gclass":gclass},
       dataType:'json',
       timeout:30000,
       success:function(data){
          
       },
       error:function(xrh,type)
       {
         alertify.alert(type);
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
                    <a href="javascript:history.go(-1);" data-icon="previous" data-target="back"><i class="icon previous"></i>返回</a>
                </nav>
                <h1 class="title">级别列表
                </h1>
                <nav class="right">
                    <a data-target="section" data-icon="info" href="#" id="manualBtn" > </a>
                </nav>

            </header> 
         <div class="scroll-area-list" id="codelListArea">
             
            <!-- <div class="weinxPlane">
          
              <ul  class="list app-list" >
                   
                  
                             <li class="divider">WHMASK</li>
                                    
                             <li>
                                 
                                  <i class="icon next"></i> 
                                       <a href="CustomerGetCustomerList.aspx?cbid=1&lid=6&ln=美丽顾问" > 
                                       <strong>美丽顾问 </strong>   
                                    </a>
                              </li>
                                                       <li>
                                 
                                  <i class="icon next"></i> 
                                       <a href="CustomerGetCustomerList.aspx?cbid=1&lid=6&ln=美丽顾问" > 
                                       <strong>美丽顾问 </strong>   
                                    </a>
                              </li>
               
              </ul>
          </div> -->

         </div>
        </section>
    </div> 
    
<div id="jingle_toast" class="success" style="display: none;"></div>
<div id="jingle_popup" style="display: none;" class="loading">
    <i class="icon spinner"></i>
    <p>加载中...</p>
</div>
<div id="jingle_popup_mask" style="opacity: 0.1; display: none;"></div>
</body>
</html>

