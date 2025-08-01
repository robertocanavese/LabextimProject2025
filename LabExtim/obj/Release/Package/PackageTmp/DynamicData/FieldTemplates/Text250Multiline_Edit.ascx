<%@ Control Language="C#" CodeBehind="Text250Multiline_Edit.ascx.cs" Inherits="LabExtim.DynamicData.FieldTemplates.Text250Multiline_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" Text='<%# FieldValueEditString %>' 
             CssClass="droplist" Width="250"  Rows="3" TextMode="MultiLine" BorderStyle="None" ></asp:TextBox>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" />
<asp:CustomValidator runat="server" ID="CustomValidator1" CssClass="droplist" 
                     ControlToValidate="TextBox1" Display="Dynamic"  Text="Valore già esistente"
                     onservervalidate="CustomValidator1_ServerValidate"  />