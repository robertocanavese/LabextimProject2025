<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductionOrderQuotationStats.aspx.cs"
    Inherits="LabExtim.ProductionOrderQuotationStats" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%--<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />--%>
    <link href="~/Site.css" rel="stylesheet" type="text/css" />

    <script src="Site.js" type="text/javascript"></script>

    <%--<style type="text/css" media="print">
        .Landscape
        {
             width: 100%; margin: 5%; float: none; 
            -webkit-transform: rotate(-90deg);
            -moz-transform: rotate(-90deg);
            -ms-transform: rotate(-90deg);
            -ms-filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=1);
            filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=1);
        }
    </style>--%>
    <style type="text/css" media="print">
        .Landscape {
            width: 100%;
            margin: 0;
            float: none;
        }
    </style>

    <script type="text/javascript">
        function printHelp() {
            //panel = document.getElementById("panelGSC");
            document.getElementById("lbtPrint").style.visibility = 'hidden';
            document.getElementById("lbtOptimizeLayout").style.visibility = 'hidden';
            //            document.getElementById("divDetails").style.visibility = 'hidden';
            //document.getElementById("CloseWindow").style.visibility = 'hidden';
            //oldPanelClientStyle = panel.getAttribute("style");
            //panel.removeAttribute("style");
            window.print();
            //panel.setAttribute("style", oldPanelClientStyle);
            document.getElementById("lbtPrint").style.visibility = 'visible';
            document.getElementById("lbtOptimizeLayout").style.visibility = 'visible';
            //            document.getElementById("divDetails").style.visibility = 'visible';
            //document.getElementById("CloseWindow").style.visibility = 'visible';
            return false;
        }
    </script>

</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 10px; background-color: #FBFBFB; color: navy">
    <%--background-color: #fdffb8"--%>
    <form id="form1" runat="server">
        <div class="Landscape">
            <%--<asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />--%>
            <%--<asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"
            EnablePartialRendering="true" />--%>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true"
                EnableScriptGlobalization="true">
            </asp:ToolkitScriptManager>
            <asp:Panel ID="DetailsPanel" runat="server">
                <h5 style="text-align: center; font-size: small">Ordine di produzione
                <asp:Label ID="lblItemNo" runat="server" Font-Size="Small" />
                    <br />
                    Quadro comparativo costi preventivo / consuntivo
                </h5>
                <table>
                    <tr>
                        <td id="tdDetails" runat="server" rowspan="5" valign="top">
                            <div id="divDetails" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSuccess" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:DetailsView ID="dtvProductionOrder" runat="server" DataSourceID="DetailsDataSource"
                                                OnPreRender="OnDetailsViewPreRender" AutoGenerateRows="false">
                                                <%--AutoGenerateEditButton="true" OnModeChanging="OnDetailsViewModeChanging" 
                                                OnItemUpdated="OnDetailsViewItemUpdated" AutoGenerateRows="false" 
                                                DataKeyNames="ID" --%>
                                                <FieldHeaderStyle Font-Bold="true" />
                                                <RowStyle CssClass="selected" />
                                                <FooterStyle CssClass="selected" />

                                                <Fields>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblNumber" runat="server" Text="Numero OdP"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycNumber" runat="server" DataField="Number" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblIDQuotation" runat="server" Text="ID preventivo"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycIDQuotation" runat="server" DataField="ID_Quotation" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblDescription" runat="server" Text="Descrizione"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text" />
                                                        </ItemTemplate>
                                                        <%--<EditItemTemplate>
                                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="MultilineText_Edit" />
                                                        </EditItemTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblManager" runat="server" Text="Gestione"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey" CssClass="bold red" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey_Edit"
                                                                Mode="Edit" />
                                                        </EditItemTemplate>
                                                        <InsertItemTemplate>
                                                            <asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey_Edit" />
                                                        </InsertItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit"
                                                                Mode="Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblCustomerOrder" runat="server" Text="Ordine Cliente"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycCustomerOrder" runat="server" DataField="CustomerOrder"
                                                                UIHint="ForeignKey" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycCustomerOrder" runat="server" DataField="CustomerOrder"
                                                                UIHint="ForeignKey_Edit" Mode="Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-- <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblQuotation" runat="server" Text="Preventivo"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycQuotation" runat="server" DataField="Quotation" UIHint="ForeignKey" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycQuotation" runat="server" DataField="Quotation" UIHint="ForeignKey_Edit"
                                                                Mode="Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblStartDate" runat="server" Text="Data lancio"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime"
                                                                DataFormatString="{0:d}" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime_Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text="Quantità"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Text" />
                                                        </ItemTemplate>
                                                        <%--<ItemStyle HorizontalAlign="Right" />--%>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Integer_Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblQuantityOver" runat="server" Text="Evaso"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycQuantityOver" runat="server" DataField="QuantityOver"
                                                                UIHint="Text" />
                                                        </ItemTemplate>
                                                        <%--<ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblOutstandingQuantity" runat="server" Text="Da evadere"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycOutstandingQuantity" runat="server" DataField="OutstandingQuantity"
                                                                UIHint="Text" />
                                                        </ItemTemplate>
                                                        <%--<ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblDeliveryDate" runat="server" Text="Data consegna"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate"
                                                                UIHint="DateTime" DataFormatString="{0:d}" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate"
                                                                UIHint="DateTime_Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblCost" runat="server" Text="Costo"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Text" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblDirectSupply" runat="server" Text="Acquistato"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycDirectSupply" runat="server" DataField="DirectSupply"
                                                                UIHint="Boolean" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycDirectSupply" runat="server" DataField="DirectSupply"
                                                                UIHint="Boolean_Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblPrice" runat="server" Text="Prezzo"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycPrice" runat="server" DataField="Price" UIHint="Text" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycPrice" runat="server" DataField="Price" UIHint="Decimal_Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblStatuse" runat="server" Text="Stato"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey_Edit"
                                                                Mode="Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblNonConformity" runat="server" Text="Non conformità"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycNonConformity" runat="server" DataField="NonConformity" UIHint="ForeignKey" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycNonConformity" runat="server" DataField="NonConformity" UIHint="ForeignKey_Edit"
                                                                Mode="Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblComplaintReceived" runat="server" Text="Ricevuto reclamo"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycComplaintReceived" runat="server" DataField="ComplaintReceived" UIHint="YesNo_Edit" ReadOnly="true" AllowNullValue="false" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycComplaintReceived" runat="server" DataField="ComplaintReceived" UIHint="YesNo_Edit" ReadOnly="false"   AllowNullValue="false"
                                                                Mode="Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblCorrectiveAction" runat="server" Text="Azione correttiva"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycCorrectiveAction" runat="server" DataField="CorrectiveAction" UIHint="ForeignKey" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycCorrectiveAction" runat="server" DataField="CorrectiveAction" UIHint="ForeignKey_Edit"
                                                                Mode="Edit" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblNote" runat="server" Text="Descrizione lavorazione"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="MultilineText"
                                                                HtmlEncode="false" />
                                                        </ItemTemplate>
                                                        <%--<EditItemTemplate>
                                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="MultilineText_Edit"
                                                                HtmlEncode="false" />
                                                        </EditItemTemplate>
                                                        <InsertItemTemplate>
                                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="MultilineText_Edit"
                                                                HtmlEncode="false" />
                                                        </InsertItemTemplate>--%>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblNoteFromProduction" runat="server" Text="Note tecniche da OdP precedenti"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycNoteFromProduction" runat="server" DataField="NoteFromProduction" UIHint="MultilineText"
                                                                HtmlEncode="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAccountNoteFromProduction" runat="server" Text="Segnalazioni da produzione"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycAccountNoteFromProduction" runat="server" DataField="AccountNoteFromProduction" UIHint="MultilineText"
                                                                HtmlEncode="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAccountNote" runat="server" Text="Note amministrative"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycAccountNote" runat="server" DataField="AccountNote" UIHint="MultilineText"
                                                                HtmlEncode="false" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycAccountNote" runat="server" DataField="AccountNote" UIHint="MultilineText_Edit"
                                                                HtmlEncode="false" />
                                                        </EditItemTemplate>
                                                        <InsertItemTemplate>
                                                            <asp:DynamicControl ID="dycAccountNote" runat="server" DataField="AccountNote" UIHint="MultilineText_Edit"
                                                                HtmlEncode="false" />
                                                        </InsertItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblNote1" runat="server" Text="Appunti"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycNote1" runat="server" DataField="Note1" UIHint="MultilineText"
                                                                HtmlEncode="false" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DynamicControl ID="dycNote1" runat="server" DataField="Note1" UIHint="MultilineText_Edit"
                                                                HtmlEncode="false" />
                                                        </EditItemTemplate>
                                                        <InsertItemTemplate>
                                                            <asp:DynamicControl ID="dycNote1" runat="server" DataField="Note1" UIHint="MultilineText_Edit"
                                                                HtmlEncode="false" />
                                                        </InsertItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblContractor" runat="server" Text="Capo commessa"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DynamicControl ID="dycContractor" runat="server" DataField="Employee" UIHint="ForeignKey" />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                </Fields>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:DetailsView>
                                            <br />
                                            <asp:Button ID="lbtPrintProductionOrder" runat="server" CssClass="myButton" Font-Bold="False"
                                                OnClientClick="javascript:return confirm('Vuoi stampare l\'ordine di produzione corrente o creato?');"
                                                Text="Stampa" OnClick="lbtPrintProductionOrder_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="lbtCloneProductionOrder" runat="server" CssClass="myButton" Font-Bold="False"
                                            OnClientClick="javascript:return confirm('Vuoi creare un nuovo ordine di produzione uguale a quello corrente?');"
                                            Text="Clona" OnClick="lbtCloneProductionOrder_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="lbtClose" runat="server" CssClass="myButton" Font-Bold="False"
                                            OnClientClick="javascript:window.close();" Text="Chiudi" />
                                            <br />
                                            <asp:Button ID="lbtNote" Visible="false" runat="server" Text="Visualizza annotazioni da produzione"
                                                CssClass="myButton" Font-Bold="False"></asp:Button>
                                            <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                                TableName="ProductionOrders" OnSelecting="DetailsDataSource_Selecting">
                                                <%--EnableUpdate="true" OnUpdating="DetailsDataSource_Updating"--%>
                                                <%--<WhereParameters>
                                <asp:DynamicQueryStringParameter Name="ID" />
                            </WhereParameters>--%>
                                            </asp:LinqDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <table id="tblStats" runat="server" style="background-color: #f2f2f2">
                                    <tr>
                                        <th colspan="2">Statistiche redditività</th>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left">
                                            <asp:Label ID="lblQUOTotCost" runat="server" Text="Costo preventivato €"></asp:Label></th>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblQUOTotCostv" runat="server" Text=""></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left">
                                            <asp:Label ID="lblPORTotCost" runat="server" Text="Costo da consuntivo (CC) €"></asp:Label></th>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPORTotCostv" runat="server" Text=""></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left">
                                            <asp:Label ID="lblPORTotHistoricalCost" runat="server" Text="Costo storico da consuntivo (CSC) €"></asp:Label></th>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPORTotHistoricalCostv" runat="server" Text=""></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left">
                                            <asp:Label ID="lblProvvTotValue" runat="server" Text="Provvigioni (P) €"></asp:Label></th>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblProvvTotValuev" runat="server" Text=""></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left">
                                            <asp:Label ID="lblTotCosts" runat="server" Text="Totale costi (CSC+P) €"></asp:Label></th>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTotCostsv" runat="server" Text=""></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left">
                                            <asp:Label ID="lblFATTotValue" runat="server" Text="Fatturato (F) €"></asp:Label></th>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblFATTotValuev" runat="server" Text=""></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left">
                                            <asp:Label ID="lblSaving" runat="server" Text="Differenza (F-CSC-P) €"></asp:Label></th>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSavingv" runat="server" Text=""></asp:Label></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: left">
                                            <asp:Label ID="lblPercentageSaving" runat="server" Text="% risparmio effettivo"></asp:Label></th>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblPercentageSavingv" runat="server" Text=""></asp:Label></td>
                                        <td></td>
                                    </tr>

                                </table>
                            </div>
                        </td>
                        <td>
                            <span style="font-weight: bold">Voci preventivo (costi ricalcolati
                            per la quantità dell'Odp)</span>
                            <asp:GridView ID="grdQuotationDetails" runat="server" AllowPaging="True" CssClass="gridview"
                                OnPageIndexChanging="grdQuotationDetails_PageIndexChanging" AutoGenerateColumns="false"
                                ShowFooter="true" Width="100%">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <Columns>
                                    <%--<asp:BoundField DataField="POQuantity" HeaderText="Quantità OdP" />--%>
                                    <asp:BoundField DataField="TypeDescription" HeaderText="Tipo fase" ItemStyle-Width="80" />
                                    <asp:BoundField DataField="ItemTypeDescription" HeaderText="Tipo prodotto" ItemStyle-Width="80" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Descrizione" ItemStyle-Width="160" />
                                    <asp:BoundField DataField="UnitDescription" HeaderText="UM" ItemStyle-Width="30" />
                                    <asp:BoundField DataField="Quantity" HeaderText="Quantità" DataFormatString="{0:N2}"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="Cost" HeaderText="Costo €" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="50" />
                                    <asp:BoundField DataField="ProductionDate" HeaderText="Data" DataFormatString="{0:dd/MM/yy}"
                                        ItemStyle-Width="50" />
                                    <asp:BoundField DataField="SupplierName" HeaderText="Fornitore" ItemStyle-Width="140" />
                                    <%--<asp:BoundField DataField="ProducedQuantity" HeaderText="Quantità prodotta" />--%>
                                </Columns>
                                <PagerStyle CssClass="footer" />
                                <%--<SelectedRowStyle CssClass="selected" />--%>
                                <FooterStyle Font-Bold="true" CssClass="footer" HorizontalAlign="Right" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td>
                            <br />
                            <span style="font-weight: bold">Voci consuntivo</span>
                            <asp:LinqDataSource ID="ldsProductionOrderDetails" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="ProductionOrderDetails" OnSelecting="ldsProductionOrderDetails_Selecting"
                                AutoGenerateWhereClause="true">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdProductionOrderDetails" runat="server" AllowPaging="True" CssClass="gridview"
                                DataSourceID="ldsProductionOrderDetails" OnPageIndexChanging="grdProductionOrderDetails_PageIndexChanging"
                                AutoGenerateColumns="false" ShowFooter="true" Width="100%">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <Columns>
                                     <asp:BoundField DataField="TypeDescription" HeaderText="Tipo fase" ItemStyle-Width="80" />
                                    <asp:BoundField DataField="ItemTypeDescription" HeaderText="Tipo prodotto" ItemStyle-Width="80" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Descrizione" ItemStyle-Width="160" />
                                    <asp:BoundField DataField="UnitDescription" HeaderText="UM" ItemStyle-Width="30" />
                                    <asp:BoundField DataField="Quantity" HeaderText="Quantità" DataFormatString="{0:N2}"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="Cost" HeaderText="Costo attuale €" DataFormatString="{0:N2}"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="HistoricalCost" HeaderText="Costo storico €" DataFormatString="{0:N2}"
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="ProductionDate" HeaderText="Data" DataFormatString="{0:dd/MM/yy}"
                                        ItemStyle-Width="50" />
                                    <asp:BoundField DataField="SupplierName" HeaderText="Fornitore" ItemStyle-Width="140" />
                                </Columns>
                                <PagerStyle CssClass="footer" />
                                <FooterStyle Font-Bold="true" CssClass="footer" HorizontalAlign="Right" />
                                <PagerSettings Mode="NumericFirstLast" />
                                 <caption>
                                    <EmptyDataTemplate>
                                        Nessuna voce trovata.
                                    </EmptyDataTemplate>
                                    </asp:gridview>
                                    </td>
                                    </tr>--%>
                    <tr>
                        <td>
                            <br />
                            <span style="font-weight: bold">Voci consuntivo aggregate per macrovoci</span>
                            <asp:LinqDataSource ID="ldsProductionOrderDetails_MV" runat="server" AutoGenerateWhereClause="true" ContextTypeName="DLLabExtim.QuotationDataContext" OnSelecting="ldsProductionOrderDetails_MV_Selecting" TableName="ProductionOrderDetails">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdProductionOrderDetails_MV" runat="server" AllowPaging="True" AutoGenerateColumns="false" CssClass="gridview" DataSourceID="ldsProductionOrderDetails_MV" OnPageIndexChanging="grdProductionOrderDetails_MV_PageIndexChanging" ShowFooter="true" Width="100%">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <Columns>
                                    <%--<asp:BoundField DataField="POQuantity" HeaderText="Quantità OdP" />--%>
                                    <asp:BoundField DataField="TypeDescription" HeaderText="Tipo fase" ItemStyle-Width="80" />
                                    <asp:BoundField DataField="ItemTypeDescription" HeaderText="Tipo prodotto" ItemStyle-Width="80" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Descrizione" ItemStyle-Width="160" />
                                    <asp:BoundField DataField="UnitDescription" HeaderText="UM" ItemStyle-Width="30" />
                                    <asp:BoundField DataField="Quantity" DataFormatString="{0:N2}" HeaderText="Quantità" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="Cost" DataFormatString="{0:N2}" HeaderText="Costo attuale €" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="HistoricalCost" DataFormatString="{0:N2}" HeaderText="Costo storico €" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="ProductionDate" DataFormatString="{0:dd/MM/yy}" HeaderText="Data" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="SupplierName" HeaderText="Fornitore" ItemStyle-Width="140" />
                                    <%--<asp:BoundField DataField="ProducedQuantity" HeaderText="Quantità prodotta" />--%>
                                </Columns>
                                <PagerStyle CssClass="footer" />
                                <%--<SelectedRowStyle CssClass="selected" />--%>
                                <FooterStyle CssClass="footer" Font-Bold="true" HorizontalAlign="Right" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <%--<PagerTemplate>
                                <asp:GridViewPager ID="Pager1" runat="server" />
                            </PagerTemplate>--%>
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <span style="font-weight: bold">Dettaglio operazioni interne</span>
                            <asp:LinqDataSource ID="ldsPhasesDetails" runat="server" AutoGenerateWhereClause="true" ContextTypeName="DLLabExtim.QuotationDataContext" OnSelecting="ldsPhasesDetails_Selecting" TableName="ProductionOrderDetails">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdPhasesDetails" runat="server" AllowPaging="True" AutoGenerateColumns="false" CssClass="gridview" DataSourceID="ldsPhasesDetails" OnPageIndexChanging="grdPhasesDetails_PageIndexChanging" ShowFooter="true" Width="100%">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <Columns>
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Tipo fase" ItemStyle-Width="160" />
                                    <asp:BoundField DataField="UniqueName" HeaderText="Operatore" ItemStyle-Width="120" />
                                    <asp:BoundField DataField="ProductionDate" DataFormatString="{0:dd/MM/yy}" HeaderText="Data" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="ProductionTime" DataFormatString="{0:N2}" HeaderText="Ore impiegate" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="ProductionCost" DataFormatString="{0:N2}" HeaderText="Costo attuale €" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <%--<asp:BoundField DataField="HistoricalCost" HeaderText="Costo storico €" DataFormatString="{0:N2}"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />--%>
                                    <asp:BoundField DataField="OkCopiesCount" HeaderText="Quantità OK" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="KoCopiesCount" HeaderText="Quantità di scarto" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="ProducedQuantity" HeaderText="Quantità prodotta" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50" />
                                    <asp:BoundField DataField="Note" HeaderText="Note tecniche" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120" />
                                </Columns>
                                <PagerStyle CssClass="footer" />
                                <%--<SelectedRowStyle CssClass="selected" />--%>
                                <FooterStyle CssClass="footer" Font-Bold="true" HorizontalAlign="Right" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <%--<PagerTemplate>
                                <asp:GridViewPager ID="Pager1" runat="server" />
                            </PagerTemplate>--%>
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <span style="font-weight: bold">Dettaglio bollettato</span>
                            <asp:LinqDataSource ID="ldsDNDetails" runat="server" AutoGenerateWhereClause="true" ContextTypeName="DLLabExtim.QuotationDataContext" OnSelecting="ldsDNDetails_Selecting" TableName="ProductionOrderDetails">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdDNDetails" runat="server" AllowPaging="True" AutoGenerateColumns="false" CssClass="gridview" DataSourceID="ldsDNDetails" OnPageIndexChanging="grdDNDetails_PageIndexChanging" ShowFooter="true" Width="100%">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <Columns>
                                    <asp:BoundField DataField="mm_anno" HeaderText="Anno" />
                                    <%--<asp:BoundField DataField="mm_serie" HeaderText="Serie" />--%>
                                    <asp:BoundField DataField="mm_numdoc" HeaderText="No DDT" />
                                    <asp:BoundField DataField="mm_riga" HeaderText="Riga" />
                                    <asp:BoundField DataField="mm_datfin" DataFormatString="{0:dd/MM/yy}" HeaderText="Data" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100" />
                                    <asp:BoundField DataField="mm_codart" HeaderText="Cd Articolo" ItemStyle-Width="60" />
                                    <asp:BoundField DataField="mm_descr" HeaderText="Descrizione" ItemStyle-Width="150" />
                                    <asp:BoundField DataField="mm_unmis" HeaderText="UM" />
                                    <asp:BoundField DataField="mm_quant" DataFormatString="{0:N0}" HeaderText="Qtà" ItemStyle-HorizontalAlign="Right" />
                                    <%--<asp:BoundField DataField="mm_ump" HeaderText="Val" />--%>
                                    <asp:BoundField DataField="mm_prezzo" DataFormatString="{0:N4}" HeaderText="Prezzo Uni" ItemStyle-HorizontalAlign="Right" />
                                    <%--<asp:BoundField DataField="mm_desint" HeaderText=""  />
                                <asp:BoundField DataField="mm_lotto" HeaderText="OdP"  />--%>
                                    <asp:BoundField DataField="mm_valore" DataFormatString="{0:N2}" HeaderText="Totale" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="mm_vprovv" DataFormatString="{0:N2}" HeaderText="Provvigioni" ItemStyle-HorizontalAlign="Right" />
                                    <%--<asp:BoundField DataField="mm_tipork" HeaderText="Tipo"  />--%>
                                </Columns>
                                <PagerStyle CssClass="footer" />
                                <%--<SelectedRowStyle CssClass="selected" />--%>
                                <FooterStyle CssClass="footer" Font-Bold="true" HorizontalAlign="Right" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <%--<PagerTemplate>
                                <asp:GridViewPager ID="Pager1" runat="server" />
                            </PagerTemplate>--%>
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" style="padding: 10px">
                            <asp:Button ID="lbtOptimizeLayout" runat="server" CssClass="myButton" Font-Bold="False" OnClick="lbtOptimizeLayout_Click" Text="Ottimizza per visualizzazione o stampa" />
                            &nbsp;&nbsp;
                                            <asp:Button ID="lbtPrintPOFinalCost" runat="server" CssClass="myButton" Font-Bold="False" OnClick="lbtPrintPOFinalCost_Click" OnClientClick="javascript:return confirm('Vuoi stampare il consuntivo costi dell\'ordine di produzione corrente?');" Text="Stampa consuntivo" />
                            &nbsp;&nbsp;
                                            <asp:Button ID="lbtPrint" runat="server" CssClass="myButton" OnClientClick="printHelp();" Text="Stampa scheda" />
                        </td>
                    </tr>

                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
