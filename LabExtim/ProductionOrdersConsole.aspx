<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProductionOrdersConsole.aspx.cs" Inherits="LabExtim.ProductionOrdersConsole"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox"
    TagPrefix="cfb" %>--%>
<%@ Register Src="~/CustomControls/SearchEngine.ascx" TagName="SearchEngine" TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            function EndRequestHandler() {

                SetAutoComplete();
            }

            function SetAutoComplete() {

                $("#ctl00_ContentPlaceHolder1_senMain_txtTextField2").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: document.location.href.split("?")[0].split("#")[0] + '/GetCustomers',
                            data: "{ 'q': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            success: function (result) {
                                if (result.hasOwnProperty("d")) { result = result.d; }
                                var data = jQuery.parseJSON(result);
                                response($.map(data, function (item) {
                                    return {
                                        label: item.Name,
                                        value: item.Code,
                                        markUp: item.MarkUp
                                    }
                                }))
                            },
                            error: function (response) {
                                //alert(response.responseText);
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                            }
                        });
                    },
                    minLength: 3,
                    select: function (event, ui) {
                        //log(ui.item ? ui.item.label : this.label);
                        $("#ctl00_ContentPlaceHolder1_senMain_txtTextField2").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_senMain_hidTextField2").val(ui.item.value);
                        return false;
                    },
                    open: function () {
                        //$(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                    },
                    close: function () {
                        //$(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                    }
                });

            }

            SetAutoComplete();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        });
    </script>





    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Gestione Ordini di produzione
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>
            <table width="100%">
                <tr>
                    <td>
                        <cfb:SearchEngine ID="senMain" runat="server"></cfb:SearchEngine>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <%--<asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdProductionOrders"
                Display="None" />--%>
            <asp:Panel ID="DetailsPanel" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtNewItem" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Brown" Text="Nuova voce" />
                            &nbsp;
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" OnClick="lbtUpdateGrid_Click" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintProductionOrders" runat="server" CssClass="gridview"
                                Font-Bold="True" OnClick="lbtPrintProductionOrders_Click" Text="Stampa tabella" />
                            <asp:Label ID="Label1" runat="server" Text="(a filtro vuoto sono visualizzati gli OdP degli ultimi 12 mesi)"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsProductionOrders" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="ProductionOrders" OnSelected="ldsProductionOrders_Selected" EnableUpdate="True"
                                EnableDelete="False" EnableInsert="False" OnSelecting="ldsProductionOrders_Selecting"
                                AutoGenerateOrderByClause="true">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdProductionOrders" runat="server" AutoGenerateColumns="False"
                                PagerSettings-Position="Top" AllowPaging="True" DataSourceID="ldsProductionOrders"
                                DataKeyNames="ID" CssClass="gridview" OnPageIndexChanging="grdProductionOrders_PageIndexChanging"
                                OnDataBound="grdProductionOrders_DataBound" OnRowDataBound="grdProductionOrders_RowDataBound"
                                OnRowDeleted="grdProductionOrders_RowDeleted" OnRowCommand="grdProductionOrders_RowCommand"
                                OnPreRender="grdProductionOrders_PreRender" AutoGenerateEditButton="true" OnRowUpdated="grdProductionOrders_RowUpdated">
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

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypDetails" runat="server" ImageUrl="~/Images/money_euro.png" ToolTip="Dettaglio statistiche" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="hypRecalcHistoricalCost" runat="server" ImageUrl="~/Images/calculator.gif"
                                                ToolTip="Ricalcola costo storico" CommandName="RecalcHistoricalCost" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                OnClientClick="return confirm('Confermi il ricalcolo del costo storico di questo ordine di produzione? (i costi relativi alle voci saranno aggiornati alle tariffe correnti e non sarà più possibile ripristinare quelli vecchi!)');" />
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
                                           <%-- <asp:LinkButton ID="DeliveredLinkButton" runat="server" CommandName="Delivered" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                CausesValidation="false" Text="Spedito" OnClientClick="return confirm('Confermi di aver spedito questo ordine di produzione?');" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ToWaitLinkButton" runat="server" CommandName="ToWait" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                CausesValidation="false" Text="Metti in attesa" OnClientClick="return confirm('Confermi la messa in attesa di questo ordine di produzione?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblID" runat="server" Text="OdP"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="false" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNumber" runat="server" Text="Numero"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycNumber" runat="server" DataField="Number" UIHint="Text" />
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblStatuse" runat="server" Text="Stato"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey" />
                                        </ItemTemplate>
                                        <ItemStyle Wrap="false" Font-Bold="true" />
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
                                            <asp:Label ID="lblDescription" runat="server" Text="Titolo OdP"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text250" />
                                        </ItemTemplate>
                                        <%--<EditItemTemplate>
                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text250_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text250_Edit" />
                                        </InsertItemTemplate>--%>
                                    </asp:TemplateField>


                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCustomerOrder" runat="server" Text="Ordine Cliente"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCustomerOrder" runat="server" DataField="CustomerOrder"
                                                UIHint="ForeignKey" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQuotation" runat="server" Text="Preventivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtQuotation" runat="server" CommandName="GoToQuotation" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID_Quotation") %>'
                                                Text='<%# DataBinder.Eval(Container.DataItem, "Quotation.Subject") %>' />
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
                                            <asp:Label ID="lblStartDate" runat="server" Text="Data lancio"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime"
                                                DataFormatString="{0:d}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
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
                                    </asp:TemplateField>--%>


                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDeliveryDate" runat="server" Text="Data consegna"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate" UIHint="DateTime" DataFormatString="{0:d}" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate" UIHint="DateTime_Edit" />
                                        </EditItemTemplate>
                                        <%--<ItemTemplate>
                                            <asp:TextBox ID="txtDeliveryDate" runat="server" Columns="10" CssClass="droplist" AutoPostBack="true" OnTextChanged="txtDeliveryDate_TextChanged" ></asp:TextBox>
                                            <asp:ImageButton ID="ibtDeliveryDate" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                                                ImageAlign="Middle" />
                                            <cc1:CalendarExtender ID="txtDeliveryDate_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                                                CssClass="MyCalendar" PopupButtonID="ibtDeliveryDate" TargetControlID="txtDeliveryDate"
                                                PopupPosition="Left" SelectedDate='<%#  Bind("DeliveryDate") %>'>
                                            </cc1:CalendarExtender>
                                            <asp:HiddenField ID="hidID" runat="server" Value='<%#  Bind("ID") %>' />
                                        </ItemTemplate>--%>
                                        <ItemStyle Font-Bold="true" Wrap="false" />
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField>
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
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNote" runat="server" Text="Descrizione OdP"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text" HtmlEncode="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <%--<SelectedRowStyle CssClass="selected" />--%>
                                <PagerStyle CssClass="footer" />
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
