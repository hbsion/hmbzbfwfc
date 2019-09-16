<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_Advice.aspx.cs" Inherits="UI.wxws.setup.wx_Advice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>投诉建议</title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=yes"/>
<meta content="yes" name="apple-mobile-web-app-capable"/>
<meta content="black" name="apple-mobile-web-app-status-bar-style"/>
<meta content="telephone=no" name="format-detection"/>
 <link href="../css/Jingle.css" rel="stylesheet" type="text/css" />
    <link href="../css/alertify.core.css" rel="stylesheet" type="text/css" />
    <link href="../css/alertify.default.css" rel="stylesheet" type="text/css" />
    <link href="../css/btnStyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/zepto.js" type="text/javascript"></script>
    <script src="../js/alertify.js" type="text/javascript"></script>
<style type="text/css">
body
{
	min-width:320px;
	max-width:640px;
	width:100%;
	margin:0 auto;
	font-size:16px;
}
.main
{
	width:100%;
		margin:0 auto;
		    text-align: center
}
.inputItem
{
	width:100%;
	margin-top:20px;
}
.inputItem input[type='text'],.btn1,.areaItem
{
	width:90%;
	margin:0 auto;
}
.btn1
{
	padding:5px;
	border-radius:10px;
	text-shadow:none;
	box-shadow:none;
}
</style>
<script type="text/javascript">
$(function(){
  $("#btnSubmit").on("click",function(){
      var vName=$("#txtname").val();
      var vPhone=$("#txtphone").val();
      var vContent=$("#txtcontent").val();
      //
      var pattern = /0?(13|14|15|18)[0-9]{9}/;
      if(vName.length==0)
      {
        alertify.alert("请输入姓名");
        return;
      }
      if(vPhone.length==0)
      {
        alertify.alert("请输入手机号");
        return;
      }
      if(!pattern.test(vPhone))
      {
        alertify.alert("请输入正确的手机号码");
        console.log(pattern.test(vPhone));
        return;
      }
      if(vContent.length==0)
      {
        alertify.alert("请输入内容");
        return;
      }
      $.ajax({
          url: "../../App/wx_Advice.ashx",
      data:{vname:vName,vphone:vPhone,vcontent:vContent},
      dataType:"json",
      //timeout:30000
      success:function(data){
        if(data.success)
        {
       $("#txtname").val('');
     $("#txtphone").val('');
      $("#txtcontent").val('');
           alertify.alert(data.messages);
        }
        else
        {
          alertify.alert(data.messages);
        }
      },
        error:function(ob,errcode){
       alertify.error("服务超时或错误");
      }
      });
  });
})
</script>
</head>
<body>
    <form id="form1" runat="server">
               <header>
            <nav class="left">
                  <a href="javascript:history.go(-1);" data-icon="previous" data-target="back" class="icon_back"><i class="icon previous"></i>返回</a>
            </nav>
            <h1 class="title">
               投诉建议
            </h1>
      </header>
     <div class="main">
      <div class="inputItem"><input type="text" id="txtname" name="txtname" placeholder="*姓名(必填)"/></div>
      <div class="inputItem"><input type="text" id="txtphone" name="txtphone" placeholder="*手机号(必填)"/></div>
      <div class="inputItem"><textarea id="txtcontent" class="areaItem" placeholder="*请输入投诉建议内容"></textarea></div>
      <div class="inputItem"><input type="button" name="btnSubmit" class="btn1 bgbtn fctxt" id="btnSubmit" value="提交"></div>
     </div>
    </form>
</body>
</html>
