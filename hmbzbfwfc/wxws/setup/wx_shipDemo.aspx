<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_shipDemo.aspx.cs" Inherits="UI.wxws.setup.wx_shipDemo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=yes"/>
<meta content="yes" name="apple-mobile-web-app-capable"/>
<meta content="black" name="apple-mobile-web-app-status-bar-style"/>
<meta content="telephone=no" name="format-detection"/>
 <link href="../css/Jingle.css" rel="stylesheet" type="text/css" />
 <link href="../css/wxApp.css" rel="stylesheet" type="text/css" />
<style type="text/css">
.content
 {
   overflow:auto;
 }
     .content h1
 {
 	width:100%;
 }
 .content img
 {
 	width:100%;
 }
</style>
    <title>操作演示</title>
</head>
<body>
    <form id="form1" runat="server">
                    <header>
            <nav class="left">
                  <a href="javascript:history.go(-1);" data-icon="previous" data-target="back" class="icon_back"><img src="../img/icon_arrow_left.png" alt="返回箭头图标" />返回</a>
            </nav>
            <h1 class="title">
               操作演示
            </h1>
      </header>
    <div id="content" class="content" runat="server">
    
    </div>
    </form>
</body>
</html>
