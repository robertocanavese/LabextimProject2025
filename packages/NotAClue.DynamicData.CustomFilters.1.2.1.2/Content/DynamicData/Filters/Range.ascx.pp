<%@ Control Language="C#" 
    CodeBehind="Range.ascx.cs" 
    Inherits="$rootnamespace$.RangeFilter" %>

<asp:TextBox 
    ID="txbFrom" 
    runat="server" 
    CssClass="DDFilter">
</asp:TextBox>
to
<asp:TextBox 
    ID="txbTo" 
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
