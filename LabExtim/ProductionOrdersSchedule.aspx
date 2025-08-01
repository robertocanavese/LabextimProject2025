<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProductionOrdersSchedule.aspx.cs" Inherits="LabExtim.ProductionOrdersSchedule"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox"
    TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>
                Gestione Ordini di produzione
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>
            <table class="searchEngine">
                <tr>
                    <td >
                        <asp:Label ID="lblNo" runat="server" Text="ID"></asp:Label>
                    </td>
                    <td >
                        <asp:Label ID="lblNumber" runat="server" Text="Numero"></asp:Label>
                    </td>
                    <td >
                        <asp:Label ID="lblddlStatuses" runat="server" Text="Stato"></asp:Label>
                    </td>
                    <td >
                        <asp:Label ID="lblddlCustomers" runat="server" Text="Cliente"></asp:Label>
                    </td>
                    <td >
                        <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td >
                        <cfb:IntTextBox ID="itbNo" runat="server" AutoPostBack="true" CssClass="droplist"
                            ShowFindButton="true"></cfb:IntTextBox>
                    </td>
                    <td >
                        <cfb:YearCounterTextBox ID="yctNumber" runat="server" AutoPostBack="true" CssClass="droplist"
                            ShowFindButton="true"></cfb:YearCounterTextBox>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlStatuses" runat="server" AutoPostBack="true" DataSourceID="ldsStatuses"
                            DataTextField="Description" DataValueField="ID" CssClass="droplist" OnDataBound="ddlStatuses_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsStatuses" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Statuses" OrderBy="Description" Where="StatusType=1">
                        </asp:LinqDataSource>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlCustomers" runat="server" AutoPostBack="true" DataSourceID="ldsCustomers"
                            DataTextField="Name" DataValueField="Code" CssClass="droplist" OnDataBound="ddlCustomers_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsCustomers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Customers" OrderBy="Name">
                        </asp:LinqDataSource>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="true" DataTextField="Key"
                            DataValueField="Value" CssClass="droplist" OnSelectedIndexChanged="PersistSelection"
                            OnDataBound="ddlOrderBy_DataBound">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <%--<asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdProductionOrders"
                Display="None" />--%>
            <asp:Panel ID="DetailsPanel" runat="server" >
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtNewItem" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Brown" Text="Nuova voce" />
                            &nbsp;
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" />
                            &nbsp;
                            <asp:LinkButton ID="lbtViewInputItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewInputItems_Click" Text="Visualizza gestione voci attive" Enabled="false" />
                            &nbsp;
                            <asp:LinkButton ID="lbtViewCalculated" runat="server" CssClass="gridview" Font-Bold="True"
                                Enabled="false" OnClick="lbtViewCalculated_Click" Text="Visualizza prospetto calcolato" />
                            &nbsp;
                            <asp:LinkButton ID="lbtViewDeactivatedItems" runat="server" CssClass="gridview" Font-Bold="True"
                                Enabled="false" OnClick="lbtViewDeactivatedItems_Click" Text="Visualizza gestione voci disattivate" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintProductionOrders" runat="server" CssClass="gridview"
                                Font-Bold="True" OnClick="lbtPrintProductionOrders_Click" Text="Stampa tabella" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsProductionOrders" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="ProductionOrders" OnSelected="ldsProductionOrders_Selected" EnableUpdate="True"
                                EnableDelete="True" EnableInsert="True" OnSelecting="ldsProductionOrders_Selecting"
                                AutoGenerateOrderByClause="true" EnableViewState="False">
                                <WhereParameters>
                                    <asp:ControlParameter ControlID="ddlStatuses" Name="Status" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                    <asp:ControlParameter ControlID="ddlCustomers" Name="ID_Customer" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                </WhereParameters>
                                <OrderByParameters>
                                    <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                </OrderByParameters>
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdProductionOrders" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" PagerSettings-Position="TopAndBottom" DataSourceID="ldsProductionOrders" DataKeyNames="ID" CssClass="gridview"
                                OnPageIndexChanging="grdProductionOrders_PageIndexChanging" OnDataBound="grdProductionOrders_DataBound"
                                OnRowDataBound="grdProductionOrders_RowDataBound" OnRowDeleted="grdProductionOrders_RowDeleted"
                                OnRowCommand="grdProductionOrders_RowCommand" OnPreRender="grdProductionOrders_PreRender">
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
                                                ImageUrl="~/Images/bin.png" OnClientClick='return confirm("Confermi la cancellazione di questa voce?");'
                                                ToolTip="Cancella voce" />
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
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text250_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text250_Edit" />
                                        </InsertItemTemplate>
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
                                            <%--<asp:DynamicControl ID="dycQuotation" runat="server" DataField="Quotation" UIHint="ForeignKey" />--%>
                                            <asp:LinkButton ID="lbtQuotation" runat="server" CommandName="GoToQuotation" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID_Quotation") %>'
                                                Text='<%# DataBinder.Eval(Container.DataItem, "Quotation.Subject") %>' Enabled="false" />
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
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
