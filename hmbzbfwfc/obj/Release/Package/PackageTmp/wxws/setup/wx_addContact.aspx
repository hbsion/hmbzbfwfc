<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_addContact.aspx.cs"
    Inherits="UI.wxws.setup.wx_addContact" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>公众号二维码</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0, maximum-scale=3.0, user-scalable=yes" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <link rel="stylesheet" href="../css/Jingle.css" />
    <style type="text/css">
        .imgBox
{
 padding:5em;
}
      .imgBox img
       {
       	 width:120px;
       	 height:120px;
       	 margin-top:2em;
       }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <header>
                <nav class="left">
                    <a href="javascript:history.go(-1);" data-icon="previous" data-target="back" class="icon_back"><i class="icon previous"></i>返回</a>
                </nav>
                <h1 class="title">官方二维码
                </h1>
                <nav class="right">
                    <a data-target="section" data-icon="info" href="#" id="manualBtn" > </a>
                </nav> 
            </header>
    <div class="imgBox" style="text-align: center">
        <%-- <img src="../img/wx.jpg" alt="官方公众号" />--%>
        <div>
            长按上图二维码关注官方公众号</div>
    </div>
    </form>
</body>
</html>
