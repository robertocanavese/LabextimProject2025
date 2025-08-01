<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchEngine.ascx.cs"
    Inherits="LabExtim.CustomControls.SearchEngine" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox"
    TagPrefix="cfb" %>
<table class="searchEngineMaster">
    <tr>

        <td>
            <table id="Table1" runat="server"  class="searchEngine">
                <tr>
                    <td>
                        <asp:Label ID="lblNo" runat="server" Text="ID"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNumber" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTextField1" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTextField2" runat="server"></asp:Label>
                    </td>
                     <td>
                        <asp:Label ID="lblTextField3" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDateFrom" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDateTo" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblAgente" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cfb:IntTextBox ID="itbNo" runat="server" ShowFindButton="true"></cfb:IntTextBox>
                    </td>
                    <td>
                        <cfb:YearCounterTextBox ID="yctNumber" runat="server" ShowFindButton="true"></cfb:YearCounterTextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTextField1" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hidTextField1" runat="server"></asp:HiddenField>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTextField2" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hidTextField2" runat="server"></asp:HiddenField>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTextField3" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hidTextField3" runat="server"></asp:HiddenField>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" Columns="10"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                            ImageAlign="Middle" />
                        <cc1:CalendarExtender ID="txtDate_CalendarExtenderFrom" runat="server" FirstDayOfWeek="Monday"
                            CssClass="MyCalendar" PopupButtonID="ImageButton1" TargetControlID="txtDateFrom"
                            PopupPosition="Right" SelectedDate='<%# DateTime.Now.Date.AddDays(-15) %>'>
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateTo" runat="server" Columns="10"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                            ImageAlign="Middle" />
                        <cc1:CalendarExtender ID="txtDate_CalendarExtenderTo" runat="server" FirstDayOfWeek="Monday"
                            CssClass="MyCalendar" PopupButtonID="ImageButton2" TargetControlID="txtDateTo"
                            PopupPosition="Left" SelectedDate='<%#  DateTime.Now.Date.AddDays(1) %>'>
                        </cc1:CalendarExtender>
                    </td>
                    <td id="tdDdlAgente" runat="server">
                        <asp:DropDownList ID="DropDownListAgente" runat="server" OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table id="Table2" runat="server"  class="searchEngine">
                <tr>
                    <td id="tdLbl1" runat="server">
                        <asp:Label ID="lblDropDownList1" runat="server"></asp:Label>
                    </td>
                    <td id="tdLbl2" runat="server">
                        <asp:Label ID="lblDropDownList2" runat="server"></asp:Label>
                    </td>
                    <td id="tdLbl3" runat="server">
                        <asp:Label ID="lblDropDownList3" runat="server"></asp:Label>
                    </td>
                    <td id="tdLbl4" runat="server">
                        <asp:Label ID="lblDropDownList4" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDropDownList5" runat="server"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td id="tdDdl1" runat="server">
                        <asp:DropDownList ID="ddlDropDownList1" runat="server" OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                    </td>
                    <td id="tdDdl2" runat="server">
                        <asp:DropDownList ID="ddlDropDownList2" runat="server" OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                    </td>
                    <td id="tdDdl3" runat="server">
                        <asp:DropDownList ID="ddlDropDownList3" runat="server" OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                    </td>
                    <td id="tdDdl4" runat="server">
                        <asp:DropDownList ID="ddlDropDownList4" runat="server" OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDropDownList5" runat="server" OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlOrderBy" runat="server" OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" align="left">
                        <asp:LinkButton ID="lbtEmpty" runat="server" Text="RESET" Font-Bold="true"
                            ForeColor="Red" OnClick="lbtEmpty_Click"></asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="lbtSearch" runat="server" Text="CERCA" Font-Bold="true"
                ForeColor="Green" OnClick="lbtSearch_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="4" >
            <asp:ImageButton ID="ibtEmpty" runat="server" OnClick="lbtEmpty_Click" ImageUrl="~/Images/BigReset.png" />
            <asp:ImageButton ID="ibtSearch" runat="server" OnClick="lbtSearch_Click" ImageUrl="~/Images/BigSearch.png" />
        </td>
    </tr>
</table>
