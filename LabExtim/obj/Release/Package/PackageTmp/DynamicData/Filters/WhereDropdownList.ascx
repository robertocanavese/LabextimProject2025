<%@ Control Language="C#" 
    CodeBehind="WhereDropdownList.ascx.cs" 
    Inherits="LabExtim.WhereDropdownListFilter" %>

<asp:DropDownList 
    runat="server" 
    ID="DropDownList1" 
    AutoPostBack="True" 
    CssClass="DDFilter"
    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
    <asp:ListItem Text="All" Value="" />
</asp:DropDownList>
