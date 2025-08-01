<%@ Control Language="C#" CodeBehind="MultilineText.ascx.cs" Inherits="LabExtim.DynamicData.FieldTemplates.MultilineTextField" %>

<asp:TextBox ID="Literal1" runat="server" CssClass="droplist" TextMode="MultiLine" Text='<%# FieldValueString %>' Columns="80" Rows="5" ReadOnly="true"  ></asp:TextBox>

