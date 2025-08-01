<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="CustomerOrdersConsole.aspx.cs" Inherits="LabExtim.CustomerOrdersConsole"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
     
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>
                Gestione Ordini Clienti
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>
            <table width="100%">
                <%--<tr style="width: 100%" align="center">
                    <asp:Label ID="lblTitle" runat="server" Text="Selezionare una o più categorie di cui selezionare le voci e l'eventuale ordinamento prescelto: "
                        Font-Bold="true"></asp:Label></tr>--%>
                <tr>
                    <%--<td style="width: 25%" align="center">
                        <asp:Label ID="lblddlStatuses" runat="server" Text="Stato"></asp:Label>
                    </td>--%>
                    <td style="width: 25%" align="center">
                        <asp:Label ID="lblNo" runat="server" Text="Numero"></asp:Label>
                    </td>
                    <td style="width: 25%" align="center">
                        <asp:Label ID="lblddlStatuses" runat="server" Text="Stato"></asp:Label>
                    </td>
                    <td style="width: 25%" align="center">
                        <asp:Label ID="lblddlCustomers" runat="server" Text="Cliente"></asp:Label>
                    </td>
                    <td style="width: 25%" align="center">
                        <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>
                    </td>
                </tr>
                <tr style="width: 100%" align="center">
                    <%--<td style="width: 25%" align="center">
                        <asp:DropDownList ID="ddlTypes" runat="server" AutoPostBack="true" DataSourceID="ldsTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlTypes_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Types" OrderBy="Order">
                        </asp:LinqDataSource>
                    </td>--%>
                    <td style="width: 25%" align="center">
                        <cfb:IntTextBox ID="itbNo" runat="server" AutoPostBack="true" CssClass="droplist"
                            ShowFindButton="true"></cfb:IntTextBox>
                    </td>
                    <td style="width: 25%" align="center">
                        <asp:DropDownList ID="ddlStatuses" runat="server" AutoPostBack="true" DataSourceID="ldsStatuses"
                            DataTextField="Description" DataValueField="ID" CssClass="droplist" OnDataBound="ddlStatuses_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsStatuses" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Statuses" OrderBy="Description" Where="StatusType=2">
                        </asp:LinqDataSource>
                    </td>
                    <td style="width: 25%" align="center">
                        <asp:DropDownList ID="ddlCustomers" runat="server" AutoPostBack="true" DataSourceID="ldsCustomers"
                            DataTextField="Name" DataValueField="Code" CssClass="droplist" OnDataBound="ddlCustomers_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsCustomers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Customers" OrderBy="Name">
                        </asp:LinqDataSource>
                    </td>
                    <td style="width: 25%" align="center">
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
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdCustomerOrders"
                Display="None" />
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
                                OnClick="lbtViewInputItems_Click" Text="Visualizza gestione voci attive" />
                            &nbsp;
                            <asp:LinkButton ID="lbtViewCalculated" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewCalculated_Click" Text="Visualizza prospetto calcolato" />
                            &nbsp;
                            <asp:LinkButton ID="lbtViewDeactivatedItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewDeactivatedItems_Click" Text="Visualizza gestione voci disattivate" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintCustomerOrders" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtPrintCustomerOrders_Click" Text="Stampa tabella" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<asp:Panel runat="server" Height="670"  Width="100%" ScrollBars="Vertical" --%>
                            <asp:LinqDataSource ID="GridDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="CustomerOrders" OnSelected="GridDataSource_Selected" EnableUpdate="True"
                                EnableDelete="True" EnableInsert="True" OnSelecting="GridDataSource_Selecting"
                                AutoGenerateOrderByClause="true"  EnableViewState="False">
                                <WhereParameters>
                                    <%--<asp:Parameter Name="Inserted" Type="Boolean" DefaultValue="True" />--%>
                                    <%--<asp:ControlParameter ControlID="ddlTypes" Name="TypeCode" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />--%>
                                    <asp:ControlParameter ControlID="ddlStatuses" Name="Status" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                    <asp:ControlParameter ControlID="ddlCustomers" Name="CustomerCode" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                </WhereParameters>
                                <OrderByParameters>
                                    <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                </OrderByParameters>
                            </asp:LinqDataSource>
                            <%--<WhereParameters>
                        <asp:QueryStringParameter Name="ID" QueryStringField="P0" Type="Int32" />
                    </WhereParameters>--%>
                            <asp:GridView ID="grdCustomerOrders" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                DataSourceID="GridDataSource" DataKeyNames="ID" CssClass="gridview" OnPageIndexChanging="grdCustomerOrders_PageIndexChanging"
                                OnDataBound="OnGridViewDataBound" OnRowDataBound="grdCustomerOrders_RowDataBound"
                                Width="1400" OnRowDeleted="grdCustomerOrders_RowDeleted" OnRowCommand="grdCustomerOrders_RowCommand">
                                <Columns>
                                    <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" CausesValidation="false"
                                Text="Elimina" OnClientClick='return confirm("Confermi la cancellazione di questa voce?");' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                    <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="InsertLinkButton" runat="server" CommandName="Insert" CausesValidation="false"
                                Text="Nuovo" OnClientClick='return confirm("Confermi la creazione di una nuova voce?");' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
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
                                        <ItemStyle HorizontalAlign="Right" />
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
                                </Columns>
                                <PagerStyle CssClass="footer" />
                                <%--<SelectedRowStyle CssClass="selected" />--%>
                                <PagerTemplate>
                                    <asp:GridViewPager ID="Pager0" runat="server" />
                                </PagerTemplate>
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <asp:LinqDataSource ID="CalculatedDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                TableName="SPCustomerOrdersCalculated" OnSelecting="CalculatedDataSource_Selecting"
                AutoGenerateWhereClause="true">
                <WhereParameters>
                    <%--<asp:Parameter Name="Inserted" DefaultValue="true" Type="Boolean" />--%>
                    <%--<asp:ControlParameter ControlID="ddlTypes" Name="TypeCode" Type="Int32" PropertyName="SelectedValue"
                        DefaultValue="" />--%>
                    <asp:ControlParameter ControlID="ddlStatuses" Name="Status" Type="Int32" PropertyName="SelectedValue"
                        DefaultValue="" />
                    <asp:ControlParameter ControlID="ddlCustomers" Name="CustomerCode" Type="Int32" PropertyName="SelectedValue"
                        DefaultValue="" />
                </WhereParameters>
                <OrderByParameters>
                    <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                        DefaultValue="" />
                </OrderByParameters>
            </asp:LinqDataSource>
            <asp:GridView ID="grdCalculated" runat="server" AllowPaging="True" CssClass="gridview"
                DataSourceID="CalculatedDataSource" OnPageIndexChanging="grdCalculated_PageIndexChanging"
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
                            <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblCustomerOrderCode" runat="server" Text="Riferimento Cliente"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycCustomerOrderCode" runat="server" DataField="CustomerOrderCode"
                                UIHint="Text" />
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
                            <asp:Label ID="lblOrderDate" runat="server" Text="Data Ordine"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycOrderDate" runat="server" DataField="OrderDate" UIHint="DateTime"
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
                            <asp:Label ID="lblConfirmDate" runat="server" Text="Data Conferma"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycConfirmDate" runat="server" DataField="ConfirmDate" UIHint="DateTime"
                                DataFormatString="{0:d}" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblLastDeliveryDate" runat="server" Text="Data Consegna (ultima)"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycLastDeliveryDate" runat="server" DataField="LastDeliveryDate"
                                UIHint="DateTime" DataFormatString="{0:d}" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
