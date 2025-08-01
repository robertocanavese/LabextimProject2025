<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="QuotationFreeItems.aspx.cs" Inherits="LabExtim.QuotationFreeItems"
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

                //SetAutoCompleteCustomer();
                SetAutoCompleteSupplier();
            }


            //function SetAutoCompleteCustomer() {

            //    $("#ctl00_ContentPlaceHolder1_senMain_txtTextField2").autocomplete({
            //        source: function (request, response) {
            //            $.ajax({
            //                url: document.location.href.split("?")[0].split("#")[0] + '/GetCustomers',
            //                data: "{ 'q': '" + request.term + "'}",
            //                dataType: "json",
            //                type: "POST",
            //                async: false,
            //                contentType: "application/json; charset=utf-8",
            //                success: function (result) {
            //                    if (result.hasOwnProperty("d")) { result = result.d; }
            //                    var data = jQuery.parseJSON(result);
            //                    response($.map(data, function (item) {
            //                        return {
            //                            label: item.Name,
            //                            value: item.Code,
            //                            markUp: item.MarkUp
            //                        }
            //                    }))
            //                },
            //                error: function (response) {
            //                    //alert(response.responseText);
            //                },
            //                failure: function (response) {
            //                    //alert(response.responseText);
            //                }
            //            });
            //        },
            //        minLength: 3,
            //        select: function (event, ui) {
            //            //log(ui.item ? ui.item.label : this.label);
            //            $("#ctl00_ContentPlaceHolder1_senMain_txtTextField2").val(ui.item.label);
            //            $("#ctl00_ContentPlaceHolder1_senMain_hidTextField2").val(ui.item.value);
            //            return false;
            //        },
            //        open: function () {
            //            //$(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            //        },
            //        close: function () {
            //            //$(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            //        }
            //    });

            //}

            //function SetAutoCompleteFreeItemDescriptions() {

            //    $("#ctl00_ContentPlaceHolder1_senMain_txtTextField2").autocomplete({
            //        source: function (request, response) {
            //            $.ajax({
            //                url: document.location.href.split("?")[0].split("#")[0] + '/GetFreeTypeDescriptions',
            //                data: "{ 'q': '" + request.term + "'}",
            //                dataType: "json",
            //                type: "POST",
            //                async: false,
            //                contentType: "application/json; charset=utf-8",
            //                success: function (result) {
            //                    if (result.hasOwnProperty("d")) { result = result.d; }
            //                    var data = jQuery.parseJSON(result);
            //                    response($.map(data, function (item) {
            //                        return {
            //                            label: item.Name,
            //                            value: item.Code
            //                        }
            //                    }))
            //                },
            //                error: function (response) {
            //                    //alert(response.responseText);
            //                },
            //                failure: function (response) {
            //                    //alert(response.responseText);
            //                }
            //            });
            //        },
            //        minLength: 3,
            //        select: function (event, ui) {
            //            //log(ui.item ? ui.item.label : this.label);
            //            $("#ctl00_ContentPlaceHolder1_senMain_txtTextField2").val(ui.item.label);
            //            $("#ctl00_ContentPlaceHolder1_senMain_hidTextField2").val(ui.item.value);
            //            return false;
            //        },
            //        open: function () {
            //            //$(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            //        },
            //        close: function () {
            //            //$(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            //        }
            //    });

            //}

            function SetAutoCompleteSupplier() {

                $("#ctl00_ContentPlaceHolder1_senMain_txtTextField3").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: document.location.href.split("?")[0].split("#")[0] + '/GetSuppliers',
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
                                        value: item.Code
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
                        $("#ctl00_ContentPlaceHolder1_senMain_txtTextField3").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_senMain_hidTextField3").val(ui.item.value);
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
            //SetAutoCompleteCustomer();
            SetAutoCompleteSupplier();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        });
    </script>





    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Lista voci libere preventivi
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
                            <asp:Label ID="Label1" runat="server" Text="(a filtro vuoto sono visualizzati i preventivi degli ultimi 12 mesi)"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsProductionOrders" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="VW_QuotationFreeItems" OnSelected="ldsProductionOrders_Selected" EnableUpdate="False"
                                EnableDelete="False" EnableInsert="False" OnSelecting="ldsProductionOrders_Selecting"
                                AutoGenerateOrderByClause="true">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdProductionOrders" runat="server" AutoGenerateColumns="False"
                                PagerSettings-Position="Top" AllowPaging="True" DataSourceID="ldsProductionOrders"
                                DataKeyNames="ID" CssClass="gridview" OnPageIndexChanging="grdProductionOrders_PageIndexChanging"
                                OnRowDataBound="grdProductionOrders_RowDataBound"
                                AutoGenerateEditButton="false" OnRowCommand="grdProductionOrders_RowCommand">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <Columns>
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


                                    <asp:TemplateField >
                                        <HeaderTemplate>
                                            <asp:Label ID="lblID_Quotation" runat="server" Text="Preventivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtSubject" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Subject") %>'
                                                ForeColor="#718ABE" ToolTip="Vai a preventivo" CommandName="GoToQuotation" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID_Quotation") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCreationDate" runat="server" Text="Data"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCreationDate" runat="server" DataField="CreationDate" UIHint="DateTime"
                                                DataFormatString="{0:dd/MM/yyyy}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblManagerDesc" runat="server" Text="Gestione"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycManagerDesc" runat="server" DataField="ManagerDescription" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Type"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycType" runat="server" DataField="TypeDescription" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblItemType" runat="server" Text="ItemType"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemTypeDescription" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSupplierName" runat="server" Text="Fornitore"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycSupplierNamer" runat="server" DataField="SupplierName" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblFreeTypeDescription" runat="server" Text="Voce libera"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycFreeTypeDescription" runat="server" DataField="FreeTypeDescription" UIHint="Text250" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblUMDescription" runat="server" Text="UM"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycUMDescription" runat="server" DataField="UMDescription" UIHint="Text" />
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
