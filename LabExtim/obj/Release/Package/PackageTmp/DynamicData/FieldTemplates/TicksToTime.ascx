<%@ Control Language="C#" CodeBehind="TicksToTime.ascx.cs" Inherits="LabExtim.DynamicData.FieldTemplates.TicksToTimeField" %>

<asp:Literal runat="server" ID="Literal1"
             Text="<%# DateTime.MinValue.Add(new TimeSpan(FieldValue == null ? 0 : Convert.ToInt64(FieldValueString))).ToLongTimeString() %>" />