<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sal_cust_edit.aspx.cs" Inherits="UI.wxws.sal_cust_edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0,user-scalable=no" name="viewport" id="viewport" />
    <title>经销商资料</title>
    <link href="css/Iscroll.css" rel="stylesheet" />
    <link href="css/Jingle.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.9.0.min.js" type="text/javascript"></script>

    <style type="text/css">
        .d {
            width: 100%;
            line-height: 35px;
            background: #fff;
        }

        .dmain {
            width: 100%;
            border-bottom: 1px dashed #ccc;
            text-align: center;
        }

            .dmain img {
                width: 50%;
            }

        .dL {
            width: 30%;
            float: left;
            text-align: right;
        }

        .dR {
            width: 70%;
            float: left;
            text-align: left;
        }

        .c {
            clear: both;
        }

        .p {
            border-bottom: 1px dashed #ccc;
        }

        .box-shadow-1 {
            -webkit-box-shadow: 0 0 10px #ccc;
            -moz-box-shadow: 0 0 10px #ccc;
            box-shadow: 0 0 10px #ccc;
        }

        .level {
            width: 100%;
            height: 35px;
            line-height: 35px;
            background-color: #58B5E1;
            webkit-border-radius: 5px;
            border-radius: 5px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <nav class="left">
                <a href="javascript:history.go(-1);" data-icon="previous" data-target="back"><i class="icon previous"></i>返回</a>
            </nav>
            <h1 class="title">资料
            </h1>
        </header>
        <div style="margin: 5px 10px;">
            <div class="d box-shadow-1" id="myD" runat="server">
            </div>
            <div id="editLevel" runat="server" visible="true">
            </div>
        </div>
    </form>
</body>
</html>
