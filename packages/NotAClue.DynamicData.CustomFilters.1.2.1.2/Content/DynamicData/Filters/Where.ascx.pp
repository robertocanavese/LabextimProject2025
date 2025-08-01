<%@ Control Language="C#" 
    CodeBehind="Where.ascx.cs" 
    Inherits="$rootnamespace$.WhereFilter" %>

<asp:TextBox 
    ID="TextBox1" 
    runat="server" 
    CssClass="DDFilter">
</asp:TextBox>&nbsp;
<asp:Button 
    ID="Button1" 
    CssClass="DDFilter"
    runat="server" 
    Text="Filter" onclick="Button1_Click" />
<asp:Button 
    ID="btnClear" 
    CssClass="DDFilter"
    runat="server" 
    Text="Clear" 
    OnClick="btnRangeButton_Click" />
