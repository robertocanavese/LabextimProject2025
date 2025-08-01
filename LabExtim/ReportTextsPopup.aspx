<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportTextsPopup.aspx.cs"
    Inherits="LabExtim.ReportTextsPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/ReportCustomizer.ascx" TagName="ReportCustomizer"
    TagPrefix="cc1" %>
<%--<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="~/Site.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript">
        window.onclose = opener.location.reload();
    </script>--%>
</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif; background-color: #fdffb8">
    <form id="form1" runat="server">
    <div>
        <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
       <%-- <asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"
            EnablePartialRendering="true" />--%>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true"></asp:ToolkitScriptManager>
        <asp:Panel ID="DetailsPanel" runat="server" Font-Size="Smaller">
            <h2 style="font-family: Verdana, Geneva, Tahoma, sans-serif; text-align: center">
                Selezione testi stampa&nbsp;
                <asp:Label ID="lblItemNo" runat="server" />
            </h2>
            <h2 style="font-family: Verdana, Geneva, Tahoma, sans-serif; text-align: center">
                <asp:DropDownList ID="ddlReportTypes" runat="server" DataTextField="Value" DataValueField="Key" CssClass="droplist" AutoPostBack="true" />
            </h2>
            <cc1:ReportCustomizer ID="rcsReportTexts" runat="server" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
