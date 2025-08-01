<%@ Control Language="C#" 
    CodeBehind="LessThan.ascx.cs" 
    Inherits="$rootnamespace$.LessThanFilter" %>

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
