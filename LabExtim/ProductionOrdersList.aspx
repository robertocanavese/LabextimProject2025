<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ProductionOrdersList.aspx.cs"
    Inherits="LabExtim.ProductionOrdersList" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox"
    TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />--%>
    <link href="~/Site.css" rel="stylesheet" type="text/css" />
</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 10px; background-color: #FBFBFB">
    <form id="form1" runat="server">
        <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
        EnableScriptGlobalization="true" />--%>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true"></asp:ToolkitScriptManager>
        <div>
            <h4>Lista Ordini di produzione
            <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h4>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="DetailsPanel" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--<asp:LinkButton ID="lbtNewItem" runat="server" CssClass="gridview" Font-Bold="True"
                                    ForeColor="Brown" Text="Nuova voce" />
                                &nbsp;--%>
                                    <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                        ForeColor="Green" Text="Aggiorna lista" />
                                    &nbsp;
                                <%--<asp:LinkButton ID="lbtPrintProductionOrders" runat="server" CssClass="gridview"
                                    Font-Bold="True" OnClick="lbtPrintProductionOrders_Click" Text="Stampa tabella" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinqDataSource ID="ldsProductionOrders" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                        TableName="ProductionOrders" OnSelected="ldsProductionOrders_Selected" EnableUpdate="True"
                                        EnableDelete="True" EnableInsert="True" OnSelecting="ldsProductionOrders_Selecting"
                                        AutoGenerateOrderByClause="true" EnableViewState="False">
                                        <WhereParameters>
                                            <asp:QueryStringParameter Name="ID_Quotation" QueryStringField="POquo" Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <asp:GridView ID="grdProductionOrders" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="True" DataSourceID="ldsProductionOrders" DataKeyNames="ID" CssClass="gridview"
                                        OnPageIndexChanging="grdProductionOrders_PageIndexChanging" OnDataBound="grdProductionOrders_DataBound"
                                        OnRowDataBound="grdProductionOrders_RowDataBound"
                                        OnRowCommand="grdProductionOrders_RowCommand" OnPreRender="grdProductionOrders_PreRender">
                                        <%--OnRowDeleted="grdProductionOrders_RowDeleted"--%>
                                        <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altRow" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtUpdate" runat="server" ImageUrl="~/Images/arrow_refresh.png"
                                                        ToolTip="Rileggi tabella" CommandName="Reload" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Gestione voce" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtDelete" runat="server" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                        ImageUrl="~/Images/bin.png" OnClientClick="return confirm('Confermi l\'annullamento di questo ordine di produzione?');"
                                                        ToolTip="Annulla voce" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="CloseLinkButton" runat="server" CommandName="Close" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                        CausesValidation="false" Text="Evadi" OnClientClick="return confirm('Confermi l\'evasione di questo ordine di produzione?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                                    <asp:Label ID="lblNumber" runat="server" Text="Numero"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycNumber" runat="server" DataField="Number" UIHint="Text" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text="Descrizione"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text250" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCompany" runat="server" Text="Azienda"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey" CssClass="red bold" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCustomerOrder" runat="server" Text="Ordine Cliente"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycCustomerOrder" runat="server" DataField="CustomerOrder"
                                                        UIHint="ForeignKey" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblQuotation" runat="server" Text="Preventivo"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycQuotation" runat="server" DataField="Quotation" UIHint="ForeignKey" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblStartDate" runat="server" Text="Data lancio"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime"
                                                        DataFormatString="{0:d}" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblQuantity" runat="server" Text="Quantità"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Text" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblQuantityOver" runat="server" Text="Evaso"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycQuantityOver" runat="server" DataField="QuantityOver"
                                                        UIHint="Text" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblOutstandingQuantity" runat="server" Text="Da evadere"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycOutstandingQuantity" runat="server" DataField="OutstandingQuantity"
                                                        UIHint="Text" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDeliveryDate" runat="server" Text="Data consegna"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate"
                                                        UIHint="DateTime" DataFormatString="{0:d}" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCost" runat="server" Text="Costo"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Text" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDirectSupply" runat="server" Text="Acquistato"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycDirectSupply" runat="server" DataField="DirectSupply"
                                                        UIHint="Boolean" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblStatuse" runat="server" Text="Stato"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblNote" runat="server" Text="Note"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="footer" />
                                        <%--<SelectedRowStyle CssClass="selected" />--%>
                                        <PagerTemplate>
                                            <asp:GridViewPager ID="Pager1" runat="server" />
                                        </PagerTemplate>
                                        <EmptyDataTemplate>
                                            Nessuna voce trovata.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
