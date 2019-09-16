<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="am.aspx.cs" Inherits="UI.app.am" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<HTML xmlns="http://www.w3.org/1999/xhtml"><HEAD><TITLE>报表与查询</TITLE>
<META content="text/html; charset=utf-8" http-equiv=Content-Type />
<META name=viewport content="user-scalable=yes, width=device-width, initial-scale=1.0, maximum-scale=1.0" />
<STYLE>BODY {
	PADDING-BOTTOM: 0px; BORDER-RIGHT-WIDTH: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; PADDING-TOP: 0px;
	font-size:14pt;
}
UL {
	PADDING-BOTTOM: 0px; BORDER-RIGHT-WIDTH: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; PADDING-TOP: 0px
}
LI {
	PADDING-BOTTOM: 0px; BORDER-RIGHT-WIDTH: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; PADDING-TOP: 0px
}
P {
	PADDING-BOTTOM: 0px; BORDER-RIGHT-WIDTH: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; PADDING-TOP: 0px
}
IMG {
	PADDING-BOTTOM: 0px; BORDER-RIGHT-WIDTH: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; PADDING-TOP: 0px
}
DIV {
	PADDING-BOTTOM: 0px; BORDER-RIGHT-WIDTH: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; PADDING-TOP: 0px
}
LI {
	LIST-STYLE-TYPE: none
}
A {
	CURSOR: pointer; TEXT-DECORATION: none
}
BODY {
	FONT-FAMILY: Helvetica, Arial, sans-serif; BACKGROUND: #e8e8e8; COLOR: #333; FONT-SIZE: 100%
}
.info_box {
	POSITION: relative; MARGIN: 6px; HEIGHT: 86px;
        top: 0px;
        left: 0px;
    }
.music_img {
	BORDER-BOTTOM: #ccc 1px solid; TEXT-ALIGN: center; BORDER-LEFT: #ccc 1px solid; PADDING-BOTTOM: 2px; PADDING-LEFT: 2px; WIDTH: 80px; PADDING-RIGHT: 2px; BACKGROUND: #fff; FLOAT: left; HEIGHT: 85px; OVERFLOW: hidden; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; PADDING-TOP: 2px; border-radius: 5px
}
.music_img IMG {
	MARGIN: 5px auto; MAX-WIDTH: 80px; MAX-HEIGHT: 85px; OVERFLOW: hidden
}
.left_text A {
	POSITION: absolute; PADDING-BOTTOM: 0px; LINE-HEIGHT: 15px; PADDING-LEFT: 8px; PADDING-RIGHT: 0px; FLOAT: left; HEIGHT: 60px; COLOR: #000000; FONT-SIZE: 90%; OVERFLOW: hidden; PADDING-TOP: 2px; LEFT: 88px
}
.music_txt {
	BORDER-BOTTOM: #ccc 1px solid; BORDER-LEFT: #ccc 1px solid; MARGIN: 10px 15px; WORD-WRAP: break-word; BACKGROUND: #fff; WORD-BREAK: normal; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; border-radius: 5px
}
.music_txt {
	POSITION: relative; PADDING-BOTTOM: 10px; LINE-HEIGHT: 25px; PADDING-LEFT: 10px; PADDING-RIGHT: 10px; BACKGROUND: #fafafa; PADDING-TOP: 10px
}
.music_txt P {
	LINE-HEIGHT: 140%; DISPLAY: block; MARGIN-BOTTOM: 5px
}
.music_txt A {
	LINE-HEIGHT: 140%; DISPLAY: block; MARGIN-BOTTOM: 5px; height:30px
}
.music_txt2 {
	BORDER-BOTTOM: #ccc 1px solid; BORDER-LEFT: #ccc 1px solid; MARGIN: 10px 15px; WORD-WRAP: break-word; WORD-BREAK: normal; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; border-radius: 5px
}
.music_txt2 {
	POSITION: relative; PADDING-BOTTOM: 0px; LINE-HEIGHT: 25px; PADDING-LEFT: 5px; PADDING-RIGHT: 0px; BACKGROUND: #a80082; PADDING-TOP: 5px
}
.music_txt2 P {
	LINE-HEIGHT: 140%; DISPLAY: block; MARGIN-BOTTOM: 5px
}
.orange {
	COLOR: #900; FONT-SIZE: 90%; FONT-WEIGHT: bold
}
.orange:hover {
	BACKGROUND: #e8e8e8; COLOR: #333
}
A.zishe {
	
}
A.zishe:hover {
	
}
A.zishe {
	BORDER-BOTTOM: #999 1px solid; TEXT-ALIGN: center; BORDER-LEFT: #999 1px solid; BORDER-TOP: #999 1px solid; BORDER-RIGHT: #999 1px solid
}
A.zishe:hover {
	BORDER-BOTTOM: #999 1px solid; TEXT-ALIGN: center; BORDER-LEFT: #999 1px solid; BORDER-TOP: #999 1px solid; BORDER-RIGHT: #999 1px solid
}
.zishe {
	Z-INDEX: 2; POSITION: relative; MARGIN-TOP: 5px; WIDTH: 80px
}
A.zishe {
	MARGIN: 0px auto; FONT-WEIGHT: bold; border-radius: 5px; text-shadow: 0 1px 0 #A80082; -moz-box-shadow: -1px -1px 1px #A80082 inset; -webkit-box-shadow: -1px -1px 1px #A80082 inset; box-shadow: -1px -1px 1px #A80082 inset
}
A.zishe {
	LINE-HEIGHT: 22px; MARGIN: 5px auto; WIDTH: 80px; HEIGHT: 22px; COLOR: #ffffff
}
.fr {
	FLOAT: right
}
.clear {
	CLEAR: both
}
.price {
	COLOR: #900
}
.scan_text {
	LINE-HEIGHT: 20px; FONT-SIZE: 90%
}
.lef {
	MARGIN-TOP: 16px; MARGIN-BOTTOM: 5px; MARGIN-LEFT: 10px
}
.rig {
	MARGIN-TOP: 16px; MARGIN-BOTTOM: 5px; MARGIN-RIGHT: 10px
}
.blue {
	BORDER-BOTTOM: #ccc 1px solid; BORDER-LEFT: #ccc 1px solid; BACKGROUND: #a80082; COLOR: #fff; MARGIN-LEFT: 2px; BORDER-TOP: #ccc 1px solid; MARGIN-RIGHT: 2px; BORDER-RIGHT: #ccc 1px solid; border-radius: 3px
}
.led2 {
	MARGIN-TOP: 2px; DISPLAY: inline-block; FLOAT: right; FONT-SIZE: 108%
}
.led {
	MARGIN-TOP: 2px; DISPLAY: inline-block; FLOAT: left; FONT-SIZE: 108%
}
.right {
	WIDTH: 180px; FLOAT: left; HEIGHT: 100px
}
.sell {
	PADDING-BOTTOM: 0px; MARGIN-TOP: 68px; PADDING-LEFT: 8px; WIDTH: 100%; PADDING-RIGHT: 0px; COLOR: red; FONT-SIZE: 80%; PADDING-TOP: 2px
}
.bg_div {
	BORDER-BOTTOM: #ccc 1px solid; BORDER-LEFT: #ccc 1px solid; PADDING-BOTTOM: 5px; MARGIN: 8px 15px; PADDING-LEFT: 5px; PADDING-RIGHT: 5px; BACKGROUND: #fff; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; PADDING-TOP: 5px; border-radius: 5px
}
.div_bnt {
	TEXT-ALIGN: center; MARGIN: 10px; WIDTH: 96%
}
A.btn_login3 {
	BORDER-BOTTOM: #ccc 1px solid; TEXT-ALIGN: center; BORDER-LEFT: #ccc 1px solid; LINE-HEIGHT: 32px; WIDTH: 45%; DISPLAY: block; BACKGROUND: #39f; FLOAT: left; HEIGHT: 32px; COLOR: #fff; MARGIN-LEFT: 6px; BORDER-TOP: #ccc 1px solid; FONT-WEIGHT: bold; BORDER-RIGHT: #ccc 1px solid; border-radius: 5px
}
A.btn_login {
	BORDER-BOTTOM: #ccc 1px solid; TEXT-ALIGN: center; BORDER-LEFT: #ccc 1px solid; LINE-HEIGHT: 32px; WIDTH: 45%; DISPLAY: block; MARGIN-BOTTOM: 6px; BACKGROUND: #a80082; FLOAT: left; HEIGHT: 32px; COLOR: #fff; MARGIN-LEFT: 8px; BORDER-TOP: #ccc 1px solid; FONT-WEIGHT: bold; BORDER-RIGHT: #ccc 1px solid; border-radius: 5px
}
.img_td {
	TEXT-ALIGN: center; MARGIN: 0px auto; FONT-SIZE: 90%
}
.music_img1 {
	TEXT-ALIGN: center; MARGIN: 0px auto; WIDTH: 76px; HEIGHT: 76px; OVERFLOW: hidden
}
.music_img1 IMG {
	MARGIN: 2px auto; WIDTH: 72px; MAX-WIDTH: 72px; HEIGHT: 72px; MAX-HEIGHT: 72px; OVERFLOW: hidden
}
.music_txt1 {
	BORDER-BOTTOM: #ccc 1px solid; POSITION: relative; BORDER-LEFT: #ccc 1px solid; PADDING-BOTTOM: 6px; MARGIN: 15px; PADDING-LEFT: 6px; PADDING-RIGHT: 6px; WORD-WRAP: break-word; BACKGROUND: #f6f6f6; FONT-SIZE: 90%; WORD-BREAK: normal; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; PADDING-TOP: 6px; border-radius: 3px; -moz-box-shadow: 0px 0px 6px #D9D9D9; -webkit-box-shadow: 0px 0px 6px #D9D9D9; box-shadow: 0px 0px 6px #D9D9D9; -moz-border-radius: 3px; -webkit-border-radius: 3px; text-align:center; padding:5px; overflow:auto;background-color:#fff
}
.music_txt1 P {
	LINE-HEIGHT: 150%
}
input{ width:100%; height:30px; line-height:30px; margin:0px; padding:0px;}
</STYLE>



<META name=GENERATOR content="MSHTML 8.00.6001.23487" />
</HEAD>
<BODY>

<form runat="server" id="log_Form">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>



<div class=" music_txt1">
<TABLE style="MARGIN: auto; WIDTH: 100%">
  <TBODY>
  <TR>
    <TD style="HEIGHT: 88px" class=img_td><A 
      href="Acustlist.aspx">
      <DIV class=music_img1><IMG 
      src="../m/images/a.png"> </DIV>  <SPAN  
style="FONT-SIZE: 12pt"> 库存报表  </SPAN>
      </A></TD>
    <TD style="HEIGHT: 88px" class=img_td><A 
       href="Acusaltj.aspx">
      <DIV class=music_img1><IMG 
      src="../m/images/b.png"> </DIV> <SPAN  
style="FONT-SIZE: 12pt"> 销售统计表   </SPAN>
      </A></TD>
  
      
      
      </TR>

  <TR>
    <TD style="HEIGHT: 88px" class=img_td><A 
      href="Acushiplist.aspx">
      <DIV class=music_img1><IMG 
      src="../m/images/d.png"> </DIV><SPAN  
style="FONT-SIZE: 12pt"> 出货明细表 </SPAN> 
      </A></TD>
    <TD style="HEIGHT: 88px" class=img_td><A 
      href="Acusalelist.aspx">
      <DIV class=music_img1><IMG 
      src="../m/images/e.png"> </DIV><SPAN  
style="FONT-SIZE: 12pt">终端销售明细表 </SPAN> 
      </A></TD>

      
      </TR>


  <TR>
    <TD style="HEIGHT: 88px" class=img_td><A 
      href="Amyretj.aspx">
      <DIV class=music_img1><IMG 
      src="../m/images/f.png"> </DIV><SPAN  
style="FONT-SIZE: 12pt"> 本级退货统计 </SPAN> 
      </A></TD>
    <TD style="HEIGHT: 88px" class=img_td><A 
      href="Acuretj.aspx">
      <DIV class=music_img1><IMG 
      src="../m/images/c.png"> </DIV><SPAN  
style="FONT-SIZE: 12pt">下级退货统计 </SPAN> 
      </A></TD>

      
      </TR>
      
 </TBODY></TABLE>
</div>
<DIV class=clear></DIV>





<div id="w" runat="server" class="music_txt" visible="false">
<asp:Label ID="lbfc" runat="server" Text=""></asp:Label>
</div> 
 <asp:HiddenField ID="xtcu_no" runat="server" />

 </ContentTemplate>
 </asp:UpdatePanel>
</form>

</BODY>

</HTML>
