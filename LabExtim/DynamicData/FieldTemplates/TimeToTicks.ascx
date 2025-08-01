<%@ Control Language="C#" CodeBehind="TimeToTicks.ascx.cs" Inherits="LabExtim.DynamicData.FieldTemplates.TimeToTicksField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<asp:Literal runat="server" ID="Literal1"
 Text="<%# DateTime.MinValue.Add(new TimeSpan(Convert.ToInt64(FieldValueString))).ToShortTimeString() %>" />--%>
 
 <asp:TextBox ID="txtProductionTime" runat="server" CssClass="droplist" />
                                        <cc1:MaskedEditExtender ID="meeProductionTime" runat="server" AcceptNegative="None"
                                            Enabled="True" TargetControlID="txtProductionTime" MaskType="Time" AcceptAMPM="false"
                                            Mask="99:99:99" />
 