<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sal_cust_edit1.aspx.cs" Inherits="UI.sal_cust_edit2" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>客户资料</title>
<META name=viewport content="user-scalable=yes, width=device-width, initial-scale=1.0, maximum-scale=1.0" />

     <link rel="stylesheet" href="../hmadmin/css/list.css" type="text/css" />
    <link href="../hmadmin/billlayout.css" rel="stylesheet" type="text/css" /> 
    <style>body{ margin:0px; padding:0px; border:0px;}</style>
       <script src="../jquery1.4.2/jquery-1.4.2-vsdoc.js" type="text/javascript"></script>
    
     <script type="text/javascript">
        //省市区
        var okTip = '<img src="images/image/true.gif" />  ';
        var errTip = '<img src="images/image/false.gif" />  ';
        var okColor = "Green";
        var errColor = "red";

        var repeatName = true;



        $(function()
        {

           initPro();

            $("#province").change(function()
            {
                var pid = $(this).val();
                if (pid == "-1") {
                    $("#City option:first").attr("selected", "selected");
                    $("#province").siblings().attr("disabled", "disabled");
                    return;
                }
                $("#province").siblings().attr("disabled", "");
                var hid_province = $("#hid_province"); var Pro = $("#province option:selected"); hid_province.val(Pro.text());

                UI.Webs.Webs_AreaInfo.releCity(pid, onSuccessCity, onError);
            });

            $("#City").change(function()
            {
                var cid = $(this).val();
                if (cid == "-1") { return; }
                var hid_city = $("#hid_city");
                var city = $("#City option:selected");
                hid_city.val(city.text());
            });

        })

        function onUntitySuccess(result)
        {
            alert(result);
        }

        function selectinit(id)
        {
            $("#" + id + " option:first").attr("selected", "selected");
        }

        function onSuccessCity(result)
        {
            var city_info = eval("(" + result + ")");
            clearALLOptions('City');
            for (var i = 0; i < city_info.length; i++) {
                addOption("City", city_info[i].cid, city_info[i].cname);
            }
            $("#City").prepend("<option value='-1'>----------请选择-------</option>");
        }

        function onSuccessinit(result)
        {
            var data = $.getJSON(result);
            var pro_info = eval("(" + result + ")");
            for (var i = 0; i < pro_info.length; i++) {
                addOption("province", pro_info[i].pid, pro_info[i].pname);
            }
        }

        function onError(error)
        {
            //            alert(error.get_message());
        }

        function addOption(id, value, text)
        {
            $("#" + id).append("<option value='" + value + "'>" + text + "</option>");
        }

        function removeOption(id, value)
        {
            $("#" + id + " [value='" + value + "']").remove();
        }

        function clearALLOptions(id)
        {
            $("#" + id + " option").remove();
        }

        function initPro()
        {
            UI.Webs.Webs_AreaInfo.initProvince(onSuccessinit, onError);
            //            $("#province").siblings().attr("disabled", "disabled");
        }


       
    </script>
</head>
<body>
    <div id="loading" ><img src="../hmadmin/images/bigloading.gif"/><br><br>
        数据加载中</div>
    <form id="Form1" name="frm" runat="server">
    <telerik:RadScriptManager id="ScriptManager1" runat="server" 
        EnableTheming="True">
         <Services>
            <asp:ServiceReference Path="../Webs/Webs_AreaInfo.asmx" InlineScript="true" />
        </Services>
        
    </telerik:RadScriptManager>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table class="DataForm" >
                <tr class="FormHeader">
                        <td align="center" colspan="6">
                            客户资料
                        </td>
                    </tr>
                          
                          <tr class="FormItemStyle"><td style="text-align: right">客户账号：</td><td style="text-align: left"><telerik:RadTextBox ID="txtcu_no" Runat="server" MaxLength="50"></telerik:RadTextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                  ControlToValidate="txtcu_no" Enabled="False" ErrorMessage="客户账号必须输入">
                              </asp:RequiredFieldValidator>
                              </td></tr>
                                    
 
                                 <tr class="FormItemStyle"><td style="text-align: right">客户名称：</td><td style="text-align: left"><telerik:RadTextBox ID="txtcu_name" Runat="server" MaxLength="50" Width="150px">

                                     </telerik:RadTextBox></td></tr>
        
                                   
                                  <tr class="FormItemStyle"><td style="text-align: right">客户等级：</td><td style="text-align: left"><telerik:RadTextBox ID="txtcutype" Runat="server" MaxLength="50" Width="150px">

                                     </telerik:RadTextBox></td></tr>
                                                                       
                                   <tr class="FormItemStyle"><td style="text-align: right">省份：</td><td style="text-align: left">           
                                                              <input type="hidden" id="hid_province" value="" runat="server" />
                            <input type="hidden" id="hid_city" value="" runat="server" />
                            <select id="province" runat="server" style=" height:23px;">
                                <option value="-1">--------请选择--------</option>
                            </select>
                          
                          
                                     </td></tr>
                                   
                                    <tr class="FormItemStyle"><td style="text-align: right">城市：</td><td style="text-align: left">           
                                  
             
                                 <select id="City" runat="server"  style=" height:23px;">
                                   <option value="-1">---------请选择--------</option>
                                 </select>
                                   </td></tr>                                   
                                                                   

                                  <tr class="FormItemStyle"><td style="text-align: right">地址：</td><td style="text-align: left"><telerik:RadTextBox ID="txtaddr" Runat="server" MaxLength="50" Width="150px">
<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<EnabledStyle Resize="None"></EnabledStyle>
                                      </telerik:RadTextBox></td></tr>

                                  <tr class="FormItemStyle"><td style="text-align: right">电话：</td><td style="text-align: left"><telerik:RadTextBox ID="txtphone" Runat="server" MaxLength="50" Width="150px">
<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<EnabledStyle Resize="None"></EnabledStyle>
                                      </telerik:RadTextBox></td></tr>




                   
                                  <tr class="FormItemStyle"><td style="text-align: right">备注：</td><td style="text-align: left"><telerik:RadTextBox ID="txtremark" Runat="server" MaxLength="50" Width="150px">
<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<EnabledStyle Resize="None"></EnabledStyle>
                                      </telerik:RadTextBox></td></tr>
                                   <tr class="FormItemStyle"><td style="text-align: right">登录密码：</td><td style="text-align: left"><telerik:RadTextBox ID="txtpasswd" Runat="server" MaxLength="50" Width="150px">
<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<EnabledStyle Resize="None"></EnabledStyle>
                                       </telerik:RadTextBox></td></tr>
                              

                               
                                                                               
                                <tr class="FormItemStyle"><td colspan="2" style="height: 20px"><asp:label id="Lbl_message" runat="server"></asp:label></td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center" class="FormBottom">
                            <asp:Button CssClass="small-btn" runat="server" ID="btnSave" OnClick="btnSave_Click"
                                Text="保 存" />
                            &nbsp;&nbsp;                     
                                    </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script>document.getElementById('loading').style.display = "none"</script>

</body>
</html>
