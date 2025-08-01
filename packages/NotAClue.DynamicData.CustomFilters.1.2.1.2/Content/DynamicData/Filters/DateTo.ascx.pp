<%@ Control Language="C#" 
    CodeBehind="DateTo.ascx.cs" 
    Inherits="$rootnamespace$.DateToFilter" %>

<asp:Label ID="Label1" runat="server" Text='<%= Column.DisplayName %>' AssociatedControlID="txbDateTo" />&nbsp;
<asp:TextBox 
    ID="txbDateTo" 
    runat="server" 
    CssClass="DDFilter">
</asp:TextBox>
<ajaxToolkit:CalendarExtender 
    ID="txbDateFrom_CalendarExtender" 
    runat="server" 
    CssClass="yui"
    Enabled="True" 
    TargetControlID="txbDateTo">
</ajaxToolkit:CalendarExtender>

<asp:Button 
    ID="btnRangeButton" 
    CssClass="DDFilter"
    runat="server" 
    Text="Filter" 
    OnClick="btnRangeButton_Click" />
<asp:Button 
    ID="btnClear" 
    CssClass="DDFilter"
    runat="server" 
    Text="Clear" 
    OnClick="btnRangeButton_Click" />
