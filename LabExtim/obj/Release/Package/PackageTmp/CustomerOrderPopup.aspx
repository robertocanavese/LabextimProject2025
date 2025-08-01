<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerOrderPopup.aspx.cs"
    Inherits="LabExtim.CustomerOrderPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
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
        <asp:Panel ID="DetailsPanel" runat="server" >
            <h5 style="text-align: center">
                Ordine Clienti
                <asp:Label ID="lblItemNo" runat="server" />
            </h5>
            <table>
                <tr>
                    <td  valign="top">
                        <asp:DetailsView ID="dtvCustomerOrder" runat="server" DataSourceID="DetailsDataSource"
                            AutoGenerateEditButton="true" AutoGenerateInsertButton="true" OnModeChanging="OnDetailsViewModeChanging"
                            OnPreRender="OnDetailsViewPreRender" OnItemUpdated="OnDetailsViewItemUpdated"
                            OnItemInserted="OnDetailsViewItemInserted" AutoGenerateRows="false" OnItemCommand="dtvCustomerOrder_ItemCommand"
                            DataKeyNames="ID" OnItemCreated="dtvCustomerOrder_ItemCreated">
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
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" Mode="ReadOnly" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblInserted" runat="server" Text="Inserisci"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean_Edit"
                                Mode="Insert" />
                        </InsertItemTemplate>
                    </asp:TemplateField>--%>
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
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit"
                                            Mode="Insert" />
                                        <asp:ImageButton ID="ibtFind" runat="server" ImageUrl="~/Images/find.png" ToolTip="Seleziona ordini e preventivi Cliente"
                                            ImageAlign="Middle" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblCustomerOrderCode" runat="server" Text="Riferimento Cliente"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycCustomerOrderCode" runat="server" DataField="CustomerOrderCode"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycCustomerOrderCode" runat="server" DataField="CustomerOrderCode"
                                            UIHint="Text_Edit" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycCustomerOrderCode" runat="server" DataField="CustomerOrderCode"
                                            UIHint="Text_Edit" Mode="Insert" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
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
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycQuotation" runat="server" DataField="Quotation" UIHint="ForeignKey_Edit"
                                            Mode="Insert" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOrderDate" runat="server" Text="Data Ordine"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycOrderDate" runat="server" DataField="OrderDate" UIHint="DateTime"
                                            DataFormatString="{0:d}" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycOrderDate" runat="server" DataField="OrderDate" UIHint="DateTime_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycOrderDate" runat="server" DataField="OrderDate" UIHint="DateTime_Edit"
                                            Mode="Insert" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text="Quantità"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Text" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Integer_Edit" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Integer_Edit"
                                            Mode="Insert" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuantityOver" runat="server" Text="Evaso"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycQuantityOver" runat="server" DataField="QuantityOver"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <%--<ItemStyle HorizontalAlign="Right" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOutstandingQuantity" runat="server" Text="Da evadere"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycOutstandingQuantity" runat="server" DataField="OutstandingQuantity"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <%--<ItemStyle HorizontalAlign="Right" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblConfirmDate" runat="server" Text="Data Conferma"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycConfirmDate" runat="server" DataField="ConfirmDate" UIHint="DateTime"
                                            DataFormatString="{0:d}" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycConfirmDate" runat="server" DataField="ConfirmDate" UIHint="DateTime_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycConfirmDate" runat="server" DataField="ConfirmDate" UIHint="DateTime_Edit"
                                            Mode="Insert" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblLastDeliveryDate" runat="server" Text="Data Consegna (ultima)"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycLastDeliveryDate" runat="server" DataField="LastDeliveryDate"
                                            UIHint="DateTime" DataFormatString="{0:d}" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycLastDeliveryDate" runat="server" DataField="LastDeliveryDate"
                                            UIHint="DateTime_Edit" Mode="Edit" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycLastDeliveryDate" runat="server" DataField="LastDeliveryDate"
                                            UIHint="DateTime_Edit" Mode="Insert" />
                                    </InsertItemTemplate>
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
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey_Edit"
                                            Mode="Insert" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblNote" runat="server" Text="Note"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text_Edit" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text_Edit"
                                            Mode="Insert" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                            <FooterTemplate>
                                <%--<asp:LinkButton ID="lbtDeactivate" runat="server" CssClass="selected" Font-Bold="False"
                                    Font-Size="X-Small" Text="Disattiva voce" CommandName="Deactivate" Enabled="false"></asp:LinkButton>
                                <asp:LinkButton ID="lbtActivate" runat="server" CssClass="selected" Font-Bold="False"
                                    Font-Size="X-Small" Text="Riattiva voce" CommandName="Activate" Enabled="false"></asp:LinkButton>--%>
                            </FooterTemplate>
                        </asp:DetailsView>
                        <br />
                        <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="DetailsDataSource_Inserting"
                            TableName="CustomerOrders">
                            <WhereParameters>
                                <asp:DynamicQueryStringParameter Name="ID" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <h5>
            Ordini di produzione
        </h5>
        <asp:LinqDataSource ID="ldsProductionOrders" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
            TableName="ProductionOrders" OnSelecting="ldsProductionOrders_Selecting" AutoGenerateWhereClause="true">
            <WhereParameters>
                <%--<asp:Parameter Name="Inserted" DefaultValue="true" Type="Boolean" />--%>
                <%--<asp:ControlParameter ControlID="ddlTypes" Name="TypeCode" Type="Int32" PropertyName="SelectedValue"
                        DefaultValue="" />--%>
                <%--<asp:ControlParameter ControlID="ddlStatuses" Name="Status" Type="Int32" PropertyName="SelectedValue"
                    DefaultValue="" />
                <asp:ControlParameter ControlID="ddlCustomers" Name="CustomerCode" Type="Int32" PropertyName="SelectedValue"
                    DefaultValue="" />--%>
                <asp:ControlParameter ControlID="dtvCustomerOrder" Name="ID_CustomerOrder" PropertyName="SelectedValue" />
                <%--<asp:DynamicQueryStringParameter Name="ID" />--%>
            </WhereParameters>
            <%--<OrderByParameters>
                <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                    DefaultValue="" />
            </OrderByParameters>--%>
        </asp:LinqDataSource>
        <asp:GridView ID="grdProductionOrders" runat="server" AllowPaging="True" CssClass="gridview"
            DataSourceID="ldsProductionOrders" OnPageIndexChanging="grdProductionOrders_PageIndexChanging"
            AutoGenerateColumns="false">
            <Columns>
                <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="EditHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'
                                Text="Modifica" />&nbsp;<asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                                    CausesValidation="false" Text="Cancella" OnClientClick='return confirm("Confermi la cancellazione di questa voce?");' />&nbsp;<asp:HyperLink
                                        ID="DetailsHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Details, GetDataItem()) %>'
                                        Text="Dettaglio" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblInserted" runat="server" Text="Inserisci"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean_Edit"
                                Mode="Insert" />
                        </InsertItemTemplate>
                    </asp:TemplateField>--%>
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
                        <asp:Label ID="lblDeliveryDate" runat="server" Text="Data consegna"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate"
                            UIHint="DateTime" DataFormatString="{0:d}" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblQuantityOver" runat="server" Text="Evaso"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:DynamicControl ID="dycQuantityOver" runat="server" DataField="QuantityOver"
                            UIHint="Text" />
                    </ItemTemplate>
                    <%--<ItemStyle HorizontalAlign="Right" />--%>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblOutstandingQuantity" runat="server" Text="Da evadere"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:DynamicControl ID="dycOutstandingQuantity" runat="server" DataField="OutstandingQuantity"
                            UIHint="Text" />
                    </ItemTemplate>
                    <%--<ItemStyle HorizontalAlign="Right" />--%>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblCost" runat="server" Text="Costo"></asp:Label></HeaderTemplate>
                    <ItemTemplate>
                        <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Text" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit" />
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit" />
                    </InsertItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblDirectSupply" runat="server" Text="Acquistato"></asp:Label></HeaderTemplate>
                    <ItemTemplate>
                        <asp:DynamicControl ID="dycDirectSupply" runat="server" DataField="DirectSupply"
                            UIHint="Boolean" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DynamicControl ID="dycDirectSupply" runat="server" DataField="DirectSupply"
                            UIHint="Boolean_Edit" />
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DynamicControl ID="dycDirectSupply" runat="server" DataField="DirectSupply"
                            UIHint="Boolean_Edit" />
                    </InsertItemTemplate>
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
    </div>
    </form>
</body>
</html>
