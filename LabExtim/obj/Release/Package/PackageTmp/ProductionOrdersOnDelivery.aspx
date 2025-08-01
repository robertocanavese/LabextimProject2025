<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProductionOrdersOnDelivery.aspx.cs" Inherits="LabExtim.ProductionOrdersOnDelivery"
    MaintainScrollPositionOnPostback="true" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox"
    TagPrefix="cfb" %>--%>
<%@ Register Src="~/CustomControls/SearchEngine.ascx" TagName="SearchEngine" TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


     <script type="text/javascript">

        var JSON = JSON || {};

        $(document).ready(function () {


            var ajaxLoading4 = false;

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
            <h2>OdP in consegna
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>
            <table width="100%">
                <tr>
                    <td>
                        <cfb:searchengine id="senMain" runat="server"></cfb:searchengine>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <%--<asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdProductionMPS"
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
                            <asp:LinkButton ID="lbtPrintProductionMPS" runat="server" CssClass="gridview"
                                Font-Bold="True" OnClick="lbtPrintProductionMPS_Click" Text="Stampa tabella" />
                            <asp:Label ID="Label1" runat="server" Text="(a filtro vuoto sono visualizzati gli OdP degli ultimi 12 mesi)"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsProductionMPS" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="VW_ProductionMPSSnapshots" OnSelected="ldsProductionMPS_Selected" EnableUpdate="True"
                                EnableDelete="True" EnableInsert="True" OnSelecting="ldsProductionMPS_Selecting"
                                AutoGenerateOrderByClause="true" EnableViewState="false">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdProductionMPS" runat="server" AutoGenerateColumns="False"
                                PagerSettings-Position="Top" AllowPaging="True" DataSourceID="ldsProductionMPS"
                                DataKeyNames="ID" CssClass="gridview" OnPageIndexChanging="grdProductionMPS_PageIndexChanging"
                                OnDataBound="grdProductionMPS_DataBound" OnRowDataBound="grdProductionMPS_RowDataBound"
                                OnRowDeleted="grdProductionMPS_RowDeleted" OnRowCommand="grdProductionMPS_RowCommand"
                                OnPreRender="grdProductionMPS_PreRender">
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
                                            <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Gestione ordine di produzione" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblID" runat="server" Text="Odp"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CloseLinkButton" runat="server" CommandName="Close" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'
                                                CausesValidation="false" Text="Evadi" OnClientClick="return confirm('Confermi l\'evasione di questo ordine di produzione?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:BoundField HeaderText="OdP" DataField="poDescription" />--%>


                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text="Descrizione"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtSubject" runat="server" Width="300" Text='<%# DataBinder.Eval(Container.DataItem, "poDescription") %>'
                                                ForeColor="#718ABE" ToolTip="Vai a preventivo" CommandName="GoToQuotation" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID_Quotation") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="Cliente" DataField="cuName" />

                                    
                                    <asp:BoundField HeaderText="Quantità" DataField="Quantity" />
                                    <asp:BoundField HeaderText="Data inizio" DataField="StartDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Data consegna" DataField="DeliveryDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Stato OdP" DataField="poStatudDesc" />
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
