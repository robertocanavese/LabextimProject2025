<%@ Control Language="C#" 
    CodeBehind="GreaterThanOrEqualTo.ascx.cs" 
    Inherits="$rootnamespace$.GreaterThanOrEqualToFilter" %>

<asp:TextBox 
    ID="txbFrom" 
    runat="server" 
    CssClass="DDFilter">
</asp:TextBox>
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
