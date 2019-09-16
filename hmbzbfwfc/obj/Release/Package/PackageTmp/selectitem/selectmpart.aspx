<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectmpart.aspx.cs" Inherits="UI.selectmpart" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html >
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="pragma" content="no-cache">
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate">
    <meta http-equiv="expires" content="0">
    <link rel="stylesheet" href="../Css/list.css" type="text/css" />

    <script src="../Js/public.js" type="text/javascript" charset="utf-8"></script>

    <title>字典信息</title>

    <script>
    var P = window.parent;
    var E;
    if (typeof P.setDialog != 'undefined') {
        E = P.setDialog();
    }
    function doOk(){
        //拼Json字符串
        //var data="{root:[{id:'1',value:'岗位1'},{id:'2',value:'岗位2'}]}";
        var str = "";
        for (var i=0;i<document.forms[0].elements.length;i++)   
        {   
            var e = document.forms[0].elements[i];   
            if(e.type=="checkbox" && e.checked){
                str+=e.value+",";
            }     
        }
        for (var i=0;i<document.forms[0].elements.length;i++)   
        {   
            var e = document.forms[0].elements[i];   
            if(e.type=="radio" && e.checked){
                str+=e.value+",";
            }     
        }
        str = str!="" ? str.substring(0,str.length-1) : "";
        str = "{root:["+str+"]}";
        P.A().custom(str);
        P.cancel();
    }
    </script>

    <style type="text/css">
        .style2
        {
            width: 142px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" target="_self">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" align="center" style="font-size: 13px">
        <tr>
            <td>
                <input type="text" id="txtSearch" onkeyup="btnSearch.click();" />
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="txtSearchText.value = txtSearch.value;"
                    Text="查询" />
                (可根据代码、名称搜索)
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:HiddenField ID="txtSearchText" runat="server" />
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                       <telerik:RadGrid ID="grdList" runat="server" AutoGenerateColumns="False" 
                                                     AllowPaging="True"    OnItemDataBound="grdList_ItemDataBound"
                                                    onpageindexchanged="grdList_PageIndexChanged" 
                                                    PageSize="10" AllowCustomPaging="True" AllowSorting="True" 
                                                    onsortcommand="grdList_SortCommand"
                          Width="98%" Skin="Vista"  >
                                                    
                                                                                       
                                                    
                                                   <HeaderStyle  Height="19px" 
                                                       ></HeaderStyle>
                                                                    
                                                    <ClientSettings >
                                                        <Selecting AllowRowSelect="True" />
            

                                                    </ClientSettings>
                                                    
                                                    <SelectedItemStyle BackColor="AliceBlue" BorderWidth="0px" 
                                                       ForeColor="#FF6666"/>
                                                    
                                                    <mastertableview >

                                                        <Columns >
                                                            <telerik:GridTemplateColumn>
                                                                <HeaderTemplate>
                                                                  -
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                   <asp:Label id="Label2" runat="server"></asp:Label>
                                                                   <%-- <asp:HiddenField ID="fid" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"fid")%>' />--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="5px" />                                                            
                                                            </telerik:GridTemplateColumn>
                                                          
                                                             <telerik:GridBoundColumn DataField="p_no" HeaderText="原辅料编号" ShowSortIcon="true" SortExpression="p_no" ItemStyle-HorizontalAlign="Left" />
                                                        
                                                            <telerik:GridBoundColumn DataField="pname" HeaderText="原辅料名称" ShowSortIcon="true" SortExpression="pname" ItemStyle-HorizontalAlign="Left" />
                                                            <telerik:GridBoundColumn DataField="type" HeaderText="原辅料规格" ItemStyle-HorizontalAlign="Left" />
                                                            </Columns>
                                                        

                                                        
                                                        <NoRecordsTemplate>没有数据显示</NoRecordsTemplate>
                                                        
                                                        <rowindicatorcolumn visible="False">
                                                            <HeaderStyle Width="20px" />
                                                        </rowindicatorcolumn>
                                                        
                                                        <expandcollapsecolumn resizable="False" visible="False">
                                                            <HeaderStyle Width="20px" />
                                                        </expandcollapsecolumn>
                                                        
                                                        <editformsettings>
                                                            <popupsettings scrollbars="None" />
                                                        </editformsettings>
                                                        
                                                    </mastertableview>
                                                 </telerik:RadGrid>
                        

                    </td>
                </tr>
                <tr>
                    <td height="30">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                
                                
                                   
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="30" align="center">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <button type="button" onclick="javascript:doOk()">
                                        确定</button>
                                    <button type="button" onclick="javascript: P.cancel();">
                                        取消</button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    

</body>
</html>
