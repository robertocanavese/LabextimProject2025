<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FloatTextBox.ascx.cs" Inherits="LabExtim.CustomControls.FloatTextBox" %>
<asp:TextBox ID="TextBox1" runat="server" CssClass="droplist" MaxLength="7" Width="35" />
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" EnableClientScript="true"
                                ControlToValidate="TextBox1" ValidationExpression="-?[0-9]*[\.\,]?[0-9]+" SetFocusOnError="true"  ></asp:RegularExpressionValidator>