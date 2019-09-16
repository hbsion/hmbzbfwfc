<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_company.aspx.cs" Inherits="UI.wxws.setup.wx_company" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>公司简介</title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=1.0, user-scalable=yes"/>
<meta content="yes" name="apple-mobile-web-app-capable"/>
<meta content="black" name="apple-mobile-web-app-status-bar-style"/>
<meta content="telephone=no" name="format-detection"/>
 <link href="../css/Jingle.css" rel="stylesheet" type="text/css" />
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
 .newslist
  {
  	width:100%;
  }
  .newslist li
  {
    width:100%;
    height:45px;
    line-height:45px;
    position:relative;
    border:1px solid #d8d8d8;
    border-radius:5px;
    margin-top:5px;
  }
  .newslist li a
  {
  	 width:100%;
  	     height:45px;
    line-height:45px;
  }
  .newstitle
  {
  	
  }
  .newsdate
  {
  	position:absolute;
  	top:0;
  	right:0;
  }
</style>
</head>
<body>
    <form id="form1" runat="server">
               <header>
            <nav class="left">
                  <a href="javascript:history.go(-1);" data-icon="previous" data-target="back" class="icon_back"><i class="icon previous"></i></nav>
            <h1 class="title">
               公司简介
            </h1>
      </header>
    <div id="content" class="content" runat="server">
    </div>
    </form>
</body>
</html>
