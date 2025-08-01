<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntTextBox.ascx.cs" Inherits="LabExtim.CustomControls.IntTextBox" %>
<asp:TextBox ID="TextBox1" runat="server" CssClass="droplist" MaxLength="6" Width="35" />&nbsp;
<asp:ImageButton ID="ibtFind" runat="server" ImageUrl="~/Images/find.png"
                 ToolTip="Cerca" 
                 ImageAlign="Middle" Visible="false" onclick="ibtFind_Click" />
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" EnableClientScript="true"
                                ControlToValidate="TextBox1" ValidationExpression="[0-9]*" SetFocusOnError="true" ></asp:RegularExpressionValidator>