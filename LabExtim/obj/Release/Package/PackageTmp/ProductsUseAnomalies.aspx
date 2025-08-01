<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProductsUseAnomalies.aspx.cs" Inherits="LabExtim.ProductsUseAnomalies"
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

                LoadCheckUncheck();
                SetAutoComplete();

            }

            function LoadCheckUncheck() {

                //$(".checkbox").change(function () {
                //    if (this.checked) {
                //        //Do stuff
                //    }
                //});
                $(".checkUncheck").change(function (e) {
                    e.preventDefault();
                    params = $(this).prev()[0].defaultValue;
                    if (!loading$) {
                        loading$ = true;
                        $.ajax({
                            type: "POST",
                            url: document.location.href + "/CheckUnCheck",
                            data: JSON.stringify({ pipedParams: params }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (msg) {
                                loading$ = false;
                            },
                            error: function (msg) {
                                loading$ = false;
                                alert(msg.d);
                                //window.location.reload();
                            }
                        });
                    }
                    return true;

                });

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

            var loading$ = false;
            LoadCheckUncheck();
            SetAutoComplete();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        });
    </script>





    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Statistica mancata denuncia prodotti specificati nel preventivo
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
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" OnClick="lbtUpdateGrid_Click" />
                            <asp:Label ID="Label1" runat="server" Text="(a filtro vuoto sono visualizzati gli OdP degli ultimi 3 mesi)"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsProductionOrders" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="VW_QuotationDetailsUnusedOnProductions" OnSelected="ldsProductionOrders_Selected" EnableUpdate="False"
                                EnableDelete="False" EnableInsert="False" OnSelecting="ldsProductionOrders_Selecting"
                                AutoGenerateOrderByClause="true">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdProductionOrders" runat="server" AutoGenerateColumns="False"
                                PagerSettings-Position="Top" AllowPaging="True" DataSourceID="ldsProductionOrders"
                                DataKeyNames="ID_ProductionOrder" CssClass="gridview" OnPageIndexChanging="grdProductionOrders_PageIndexChanging"
                                OnRowDataBound="grdProductionOrders_RowDataBound"
                                AutoGenerateEditButton="false">
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
                                            <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Vai a OdP" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypDetails" runat="server" ImageUrl="~/Images/money_euro.png" ToolTip="Dettaglio statistiche" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Ok">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidUnusedProductsCheck" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.ID_ProductionOrder")%>'></asp:HiddenField>
                                            <asp:CheckBox ID="chkUnusedProductsCheck" CssClass="checkUncheck" runat="server" ToolTip="Valida / annulla validazione" Checked='<%# Bind("UnusedProductsCheck") %>'></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text="Cliente"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCustomerName" runat="server" DataField="CustomerName" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblID_Quotation" runat="server" Text="ID Preventivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycID_Quotation" runat="server" DataField="ID_Quotation" UIHint="Text" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSubject" runat="server" Text="Descrizione Preventivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycSubject" runat="server" DataField="Subject" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblManagerDesc" runat="server" Text="Gestione"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycManagerDesc" runat="server" DataField="ManagerDesc" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCommonKey" runat="server" Text="Voce tabella base"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCommonKey" runat="server" DataField="CommonKey" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblMacroItemKey" runat="server" Text="Macrovoce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycMacroItemKey" runat="server" DataField="MacroItemKey" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSupplier" runat="server" Text="Fornitore"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycSupplier" runat="server" DataField="Supplier" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblItemTypeDescription" runat="server" Text="Voce preventivo non denunciata"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycItemTypeDescription" runat="server" DataField="ItemTypeDescription" UIHint="Text250" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblUMDesc" runat="server" Text="UM"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycUMDesc" runat="server" DataField="UMDesc" UIHint="Text" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
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
                                            <asp:Label ID="lblID_ProductionOrder" runat="server" Text="Odp"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycID_ProductionOrder" runat="server" DataField="ID_ProductionOrder" UIHint="Text" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblStartDate" runat="server" Text="Data inizio"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime"
                                                DataFormatString="{0:dd/MM/yyyy}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDeliveryDate" runat="server" Text="Data consegna"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate" UIHint="DateTime"
                                                DataFormatString="{0:dd/MM/yyyy}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
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
