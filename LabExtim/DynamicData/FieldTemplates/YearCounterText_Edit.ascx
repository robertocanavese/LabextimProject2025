<%@ Control Language="C#" CodeBehind="YearCounterText_Edit.ascx.cs" Inherits="LabExtim.DynamicData.FieldTemplates.YearCounterText_EditField" %>

<asp:DropDownList ID="ddlYears" runat="server" CssClass="droplist">
    <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
</asp:DropDownList>
<asp:TextBox ID="TextBox1" runat="server" Text='<%# FieldValueEditString %>' 
             CssClass="droplist"  MaxLength="10" Width="65"   ></asp:TextBox>
