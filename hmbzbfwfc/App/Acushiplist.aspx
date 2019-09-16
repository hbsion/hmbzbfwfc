<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Acushiplist.aspx.cs" Inherits="UI.Acushiplist" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>出货明细查询</title>
    <META content="text/html; charset=utf-8" http-equiv=Content-Type>
<META name=viewport 
content="user-scalable=yes, width=device-width, initial-scale=1.0, maximum-scale=1.0">
<META content=no-cache http-equiv=Pragma>
<META content=no-cache http-equiv=Cache-Control>
<META content=0 http-equiv=Expires>
<STYLE>BODY {
	BORDER-BOTTOM: 0px; BORDER-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP: 0px; BORDER-RIGHT: 0px; PADDING-TOP: 0px;
	font-size:14pt;
}
UL {
	BORDER-BOTTOM: 0px; BORDER-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP: 0px; BORDER-RIGHT: 0px; PADDING-TOP: 0px
}
LI {
	BORDER-BOTTOM: 0px; BORDER-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP: 0px; BORDER-RIGHT: 0px; PADDING-TOP: 0px
}
P {
	BORDER-BOTTOM: 0px; BORDER-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP: 0px; BORDER-RIGHT: 0px; PADDING-TOP: 0px
}
IMG {
	BORDER-BOTTOM: 0px; BORDER-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP: 0px; BORDER-RIGHT: 0px; PADDING-TOP: 0px
}
DIV {
	BORDER-BOTTOM: 0px; BORDER-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FONT-FAMILY: "微软雅黑"; BORDER-TOP: 0px; BORDER-RIGHT: 0px; PADDING-TOP: 0px
}
LI {
	LIST-STYLE-TYPE: none
}
A {
	COLOR: #39f; CURSOR: pointer; TEXT-DECORATION: none
}
BODY {
	FONT-FAMILY: Helvetica, Arial, sans-serif; BACKGROUND: #fff; COLOR: #333; FONT-SIZE: 100%
}
.saoma A {
	WHITE-SPACE: normal; OVERFLOW: hidden
}
.saoma UL {
	BORDER-BOTTOM: #231815 1px solid; BORDER-LEFT: #231815 1px solid; MARGIN: 3px auto; WIDTH: 95%; WORD-WRAP: break-word; BACKGROUND: #fff; WORD-BREAK: normal; BORDER-TOP: #231815 1px solid; BORDER-RIGHT: #231815 1px solid; border-radius: 5px
}
.saoma LI {
	BORDER-BOTTOM: #ccc 1px solid; PADDING-BOTTOM: 0px; PADDING-LEFT: 5px; PADDING-RIGHT: 5px; PADDING-TOP: 0px
}
.saoma {
	LINE-HEIGHT: 30px; PADDING-LEFT: 0px; PADDING-TOP: 10px; text-shadow: 0 1px 0 #FFFFFF
}
.saoma P {
	PADDING-LEFT: 5px
}
.saoma INPUT {
	FLOAT: right
}
.music_txt {
	BORDER-BOTTOM: #ccc 1px solid; BORDER-LEFT: #ccc 1px solid; MARGIN: 10px; WORD-WRAP: break-word; BACKGROUND: #fff; WORD-BREAK: normal; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; border-radius: 5px
}
.music_txt {
	POSITION: relative; PADDING-BOTTOM: 10px; LINE-HEIGHT: 25px; PADDING-LEFT: 10px; PADDING-RIGHT: 10px; BACKGROUND: #fafafa; PADDING-TOP: 10px; text-shadow: 0 1px 0 #FFF
}
.music_txt P {
	LINE-HEIGHT: 140%; DISPLAY: block; MARGIN-BOTTOM: 5px
}
.price {
	COLOR: #900
}
.gre {
	COLOR: #919191; FONT-SIZE: 72%
}
.org {
	COLOR: orange; FONT-SIZE: 82%
}
.music_img {
	TEXT-ALIGN: center; MARGIN-TOP: 5px; WIDTH: 90px; MARGIN-BOTTOM: 5px; BACKGROUND: #fff; FLOAT: left; HEIGHT: 90px; MARGIN-LEFT: 5px; OVERFLOW: hidden
}
.music_img IMG {
	MAX-WIDTH: 90px; MAX-HEIGHT: 90px; OVERFLOW: hidden
}
.left_text {
	MARGIN-TOP: 12px; WIDTH: 65%; FLOAT: left; COLOR: #231815; MARGIN-LEFT: 8px; OVERFLOW: hidden
}
.left_text1 A {
	POSITION: absolute; FLOAT: left; COLOR: #900; OVERFLOW: hidden
}
.music_list UL {
	BORDER-BOTTOM: #ccc 1px solid; BORDER-LEFT: #ccc 1px solid; MARGIN: 2px; WORD-WRAP: break-word; BACKGROUND: #fff; COLOR: #333; WORD-BREAK: normal; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; border-radius: 5px
}
.music_list LI {
	LINE-HEIGHT: 20px; DISPLAY: block; HEIGHT: 80px
}
.music_list LI A {
	TEXT-DECORATION: none
}
.music_ico {
	BACKGROUND: url(images/ico.png) no-repeat right center; COLOR: #000; FONT-SIZE: 110%; OVERFLOW: hidden; FONT-WEIGHT: bold
}
.music_ico1 {
	BACKGROUND: url(images/ico.png) no-repeat right center; COLOR: #000; FONT-SIZE: 110%; OVERFLOW: hidden; BORDER-TOP: #ccc 1px solid; FONT-WEIGHT: bold
}
.box {
	Z-INDEX: 1; POSITION: fixed; TEXT-ALIGN: center; FILTER: Alpha(opacity =80); LINE-HEIGHT: 40px; WIDTH: 220px; DISPLAY: none; HEIGHT: 80px; COLOR: #fff; TOP: 85%; LEFT: 20%; border-radius: 5px
}
.box2 {
	Z-INDEX: 1; POSITION: fixed; TEXT-ALIGN: center; FILTER: Alpha(opacity =80); LINE-HEIGHT: 40px; WIDTH: 230px; DISPLAY: none; HEIGHT: 40px; COLOR: #fff; TOP: 85%; LEFT: 20%; border-radius: 5px
}
.more {
	BORDER-BOTTOM: #ccc 1px solid; TEXT-ALIGN: center; BORDER-LEFT: #ccc 1px solid; LINE-HEIGHT: 36px; MARGIN: 6px; BACKGROUND: url(images/bunt.png) repeat-x; HEIGHT: 36px; COLOR: #000; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; border-radius: 3px
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
</STYLE>

<SCRIPT type=text/javascript src="js/jquery-1.9.0.min.js"></SCRIPT>

<SCRIPT type=text/javascript src="js/jquery.lazyload.js"></SCRIPT>


</head>
<META name=GENERATOR content="MSHTML 8.00.6001.23501"></HEAD>
<body>
<form runat="server">
 
    <DIV class=music_txt2 style="text-align:center" ><SPAN><STRONG 
style="COLOR: #ffffff; FONT-SIZE: 12pt"> </STRONG> <A style="COLOR: #ffffff; font-size:15pt"
id=A1 href="" 
target=_blank>出货明细查询</A></SPAN> </DIV>


<div class="music_txt"  id ="Div1"   runat="server" >
<table width="100%">
                          <tr>
                          
                          <td align="center" style="font-size:14pt">
                          
                          <table width="98%">
                          
                           <tr>
                          <td style="text-align:right; width:90px">
                          关键字：
                          </td>
                            <td style="text-align:left">
                             
                               <asp:TextBox ID="txtscode" runat="server"></asp:TextBox>
                          </td>
                          </tr>
                          
                                                   <tr>
                          <td style="text-align:right; width:90px">
                          日期从：
                          </td>
                            <td style="text-align:left">
                             
                                   <telerik:RadDatePicker ID="txtdatef" runat="server" MinDate="1900-01-01" MaxDate="2099-01-01">
                                        <DateInput ID="DateInput1" runat="server" DateFormat="yyyy-MM-dd"></DateInput>
                                    </telerik:RadDatePicker>
                          </td>
                          </tr>
                          
                                                   <tr>
                          <td style="text-align:right; width:90px">
                          至：
                          </td>
                            <td style="text-align:left">
                             
                                    <telerik:RadDatePicker ID="txtdatet" runat="server" MinDate="1900-01-01" MaxDate="2099-01-01">
                                    <DateInput ID="DateInput2" runat="server" DateFormat="yyyy-MM-dd"></DateInput>
                                </telerik:RadDatePicker>
                                
                          </td>
                          </tr>
                          
                          <tr>
                          <td style="text-align:right;">
                          经销商：
                          </td>
                            <td style="text-align:left">
                             
                             <telerik:RadComboBox ID="cbocustomer" Runat="server"  Height="200px" 
                                   Width="180px" Skin="WebBlue"><Items><telerik:RadComboBoxItem runat="server" /></Items><CollapseAnimation Duration="200" Type="OutQuint" />
                                   </telerik:RadComboBox>
                          </td>
                          </tr>
                          
                             <tr>
                          <td style="text-align:right;">
                          产  品：
                          </td>
                            <td style="text-align:left">
                             
                            <telerik:RadComboBox ID="cboprod" Runat="server" Height="200px" 
                                   Width="180px" Skin="WebBlue" AutoPostBack="True" 
                                   ><Items>
                                   <telerik:RadComboBoxItem runat="server" /></Items><CollapseAnimation Duration="200" Type="OutQuint" /></telerik:RadComboBox>
                          </td>
                          </tr>
                          
    
                              <tr>
                          <td style="text-align:right;">
                       
                          </td>
                            <td style="text-align:left">
                          <asp:Button ID="Button3" runat="server" Text="查 询"
        BackColor="#CC0099" Font-Bold="True" ForeColor="White" Height="30px" Width="100px" onclick="Button3_Click" 
         />
                          </td>
                          </tr>
                          
                                                
                          
    
        
        
                          </table>
                          
                              
                              
                                  
                          </td>
                          </tr>
                          
                          <tr>
                          <td>
                              <asp:ScriptManager ID="ScriptManager1" runat="server">
                              </asp:ScriptManager>
                              <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                              <ContentTemplate>
                               
                            <asp:Repeater ID="awards" runat="server" 
                                     >
                                <HeaderTemplate>
                                       
                                </HeaderTemplate>
                                <ItemTemplate>
                         
                           <DIV class=music_list>
<UL id=htmlDiv>
                            <LI class=music_ico>
 <DIV class=org style="font-size:12pt">条码编号：<%#Eval("bsnno")%> </DIV>
  <DIV class=org style="font-size:12pt">经销商：<%#Eval("cu_no")%> /<%#Eval("cu_name")%></DIV>
   <DIV class=org style="font-size:12pt">产品：<%#Eval("p_no")%> /<%#Eval("pname")%> </DIV> 
  <DIV class=org style="font-size:12pt">日期：<%#Convert.ToDateTime(Eval("ship_date")).ToString("yy-MM-dd")%> 出货数量：<%#Eval("mqty")%> </DIV>

  </DIV>
  </DIV></a></LI></UL>
                             </DIV>
                          

 
                                </ItemTemplate>
                                <FooterTemplate>
                                <asp:Label ID="lbmessage"    style="font-size:14pt"     
    Text="没有记录。。。" runat="server"       
    Visible='<%#bool.Parse((awards.Items.Count==0).ToString())%>'>         
</asp:Label>   

                                  
                                </FooterTemplate>
                            </asp:Repeater>
                             </ContentTemplate>
                              </asp:UpdatePanel>
                             </td></tr>
                           </table></div>
                                                      <div class="music_txt"  id ="idresurl"   runat="server" > 
                         <input  type="button" onclick="javascript:history.go(-1);" 
                style=" background-color:#99FFCC; height:35px;line-height:35px;margin:0px auto;display:block;border-radius:5px;text-shadow: 0 1px 0 #3C8EE0;	font-weight:bold;width:99%;	margin:0px;-moz-box-shadow:-1px -1px 1px #3C8EE0 inset; -webkit-box-shadow:-1px -1px 1px #3C8EE0 inset;box-shadow:-1px -1px 1px #3C8EE0 inset; "                   
                value="返 回"      /> 
                
                
   
                           </div>
<div>
<asp:HiddenField ID="wlcode" runat="server" /> 
</form>
</div>
</html>
