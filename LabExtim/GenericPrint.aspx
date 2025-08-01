<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
         CodeBehind="GenericPrint.aspx.cs" Inherits="LabExtim.GenericPrint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
             TagPrefix="asp" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <h2>
        Stampa Tabella
        <%= TableHeader.Key %>
        (<%= TableHeader.Value %>)
    </h2>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <asp:Panel ID="DetailsPanel" runat="server">
        <table style="width: 100%">
            <tr>
                <td style="width: 33%" valign="top">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                           HeaderText="Elenco degli errori di validazione" />
                    <br />
                    <asp:LinkButton ID="lbtExportPDF" runat="server" CssClass="gridview" Font-Bold="False"
                                     OnClick="lbtExportPDF_Click" Text="Stampa in PDF" />
                    <br />
                    <asp:LinkButton ID="lbtExportExcel" runat="server" CssClass="gridview" Font-Bold="False"
                                     OnClick="lbtExportExcel_Click" Text="Esporta in Excel" />
                    <br />
                    <asp:HyperLink ID="hypTableConsole" runat="server" CssClass="template" Font-Bold="False"
                                    Text="Torna a Console Tabella" />
                    <br />
                    <br />
                    <asp:Repeater ID="PrintersList" runat="server" OnItemCommand="PrintersList_ItemCommand" OnItemDataBound="PrintersList_ItemDataBound">
                        <HeaderTemplate><h2>Seleziona la stampante (lato server)</h2></HeaderTemplate>
                        <ItemTemplate>
                                <asp:LinkButton ID="Printer" runat="server"></asp:LinkButton><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td valign="top" align="left">
                    <CR:CrystalReportViewer ID="crvGeneric" runat="server" EnableDatabaseLogonPrompt="False"
                                            EnableParameterPrompt="False" HasCrystalLogo="False" HasExportButton="False"
                                            HasPrintButton="False" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>