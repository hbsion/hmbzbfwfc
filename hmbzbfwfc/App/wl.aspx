<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wl.aspx.cs" Inherits="UI.app.wl" %>

<!DOCTYPE html>

<html lang="zh-CN">
<head>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Cache-control" content="no-cache" />
    <meta http-equiv="Cache" content="no-cache" />
    <title>物流查询</title>
    <link href="../Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="images/css.css" rel="stylesheet" />
   
</head>
<body>
    <form id="form1" runat="server">
     <div id="loading" ><img src="images/bigloading.gif"/><br><br>数据加载中</div>
    <div id="show" runat="server">
    
    <div style="position:absolute;top:50%; left:0px; width:100%; text-align:center; font-size:1.5em; margin-top:-120px;">
    <img src="images/linecons.png" alt="X" style=" width:30%;"/><br/>
    上级尚未出库
    </div>
    
    </div>
      <script>document.getElementById('loading').style.display = "none";
</script>
    </form>
</body>
</html>
