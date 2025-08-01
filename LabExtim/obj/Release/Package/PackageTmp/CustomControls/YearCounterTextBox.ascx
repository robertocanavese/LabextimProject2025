<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YearCounterTextBox.ascx.cs" Inherits="LabExtim.CustomControls.YearCounterTextBox" %>
<asp:DropDownList ID="ddlYears" runat="server" CssClass="droplist">
    <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
</asp:DropDownList>

<asp:TextBox ID="TextBox1" runat="server" CssClass="droplist" MaxLength="10" Width="65" />&nbsp;
<asp:ImageButton ID="ibtFind" runat="server" ImageUrl="~/Images/find.png"
                 ToolTip="Cerca" 
                 ImageAlign="Middle" Visible="false" onclick="ibtFind_Click" />