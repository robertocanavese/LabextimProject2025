<%@ Control Language="C#" 
    CodeBehind="DateRange.ascx.cs" 
    Inherits="$rootnamespace$.DateRangeFilter" %>

<asp:TextBox 
    ID="txbDateFrom" 
    runat="server" 
    CssClass="DDFilter">
</asp:TextBox>
<ajaxToolkit:CalendarExtender 
    ID="txbDateFrom_CalendarExtender" 
    runat="server" 
    Enabled="True" 
    TargetControlID="txbDateFrom">
</ajaxToolkit:CalendarExtender>
to
<asp:TextBox 
    ID="txbDateTo" 
    runat="server" 
    CssClass="DDFilter">
</asp:TextBox>
<ajaxToolkit:CalendarExtender 
    ID="txbDateTo_CalendarExtender" 
    runat="server" 
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
