<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True"
    CodeBehind="ProductionOrdersStats.aspx.cs" Inherits="LabExtim.ProductionOrdersStats"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script type="text/javascript">

        var JSON = JSON || {};

        $(document).ready(function () {


            var ajaxLoading4 = false;
            var ajaxLoading5 = false;

            function EndRequestHandler() {

                $('.updNoteToOdp').bind('change', updateNote);
                $('.updNonConformityCodeToOdp').bind('change', updNonConformityCode);
                $('.updCorrectiveActionCodeToOdp').bind('change', updCorrectiveActionCode);
                $('.updComplaintReceivedToOdp').bind('change', updComplaintReceived);
                SetAutoComplete();

            }

            function updateNote() {

                if (!ajaxLoading4) {
                    ajaxLoading4 = true;
                    params = $(this).prev()[0].defaultValue + '|' + $(this).find('textarea').val();
                    $.ajax({
                        type: "POST",
                        url: document.location.href + "/WriteNoteToOdP",
                        data: JSON.stringify({ pipedParams: params }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (msg) {
                            ajaxLoading4 = false;
                            //alert(msg.d)
                            //window.location.reload();
                        },
                        error: function (msg) {
                            ajaxLoading4 = false;
                            alert(msg.d)
                            //window.location.reload();
                        }
                    });
                }
                return false;
            };

            function updNonConformityCode() {

                if (!ajaxLoading5) {
                    ajaxLoading5 = true;
                    debugger;
                    params = $(this).prev()[0].defaultValue + '|' + $(this).find('input').val();
                    $.ajax({
                        type: "POST",
                        url: document.location.href + "/WriteNonConformityCodeToOdP",
                        data: JSON.stringify({ pipedParams: params }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (msg) {
                            ajaxLoading5 = false;
                            $('.nonConformityValue').val($(this).find('input').val());
                            //alert(msg.d)
                            //window.location.reload();
                        },
                        error: function (msg) {
                            ajaxLoading5 = false;
                            alert(msg.d)
                            //window.location.reload();
                        }
                    });
                }
                return false;
            };

            function updComplaintReceived() {

                if (!ajaxLoading5) {
                    ajaxLoading5 = true;
                    debugger;
                    params = $(this).prev()[0].defaultValue + '|' + $(this).find('select').val();
                    $.ajax({
                        type: "POST",
                        url: document.location.href + "/WriteComplaintReceivedToOdP",
                        data: JSON.stringify({ pipedParams: params }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (msg) {
                            ajaxLoading5 = false;
                            $('.WriteComplaintReceivedToOdP').val($(this).find('select').val());
                            //alert(msg.d)
                            //window.location.reload();
                        },
                        error: function (msg) {
                            ajaxLoading5 = false;
                            alert(msg.d)
                            //window.location.reload();
                        }
                    });
                }
                return false;
            };

            function updCorrectiveActionCode() {

                if (!ajaxLoading5) {
                    ajaxLoading5 = true;
                    debugger;
                    params = $(this).prev()[0].defaultValue + '|' + $(this).find('input').val();
                    $.ajax({
                        type: "POST",
                        url: document.location.href + "/WriteCorrectiveActionCodeToOdP",
                        data: JSON.stringify({ pipedParams: params }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (msg) {
                            ajaxLoading5 = false;
                            $('.correctiveActionValue').val($(this).find('input').val());
                            //alert(msg.d)
                            //window.location.reload();
                        },
                        error: function (msg) {
                            ajaxLoading5 = false;
                            alert(msg.d)
                            //window.location.reload();
                        }
                    });
                }
                return false;
            };


            function SetAutoComplete() {

                $("#ctl00_ContentPlaceHolder1_txtCustomer").autocomplete({
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
                        $("#ctl00_ContentPlaceHolder1_txtCustomer").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_hidCustomer").val(ui.item.value);
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

            $('.updNoteToOdp').bind('change', updateNote);
            $('.updNonConformityCodeToOdp').bind('change', updNonConformityCode);
            $('.updCorrectiveActionCodeToOdp').bind('change', updCorrectiveActionCode);
            $('.updComplaintReceivedToOdp').bind('change', updComplaintReceived);
            SetAutoComplete();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        });
    </script>

    <%-- <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">--%>
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Redditività Ordini di produzione
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>

            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdProductionOrdersStats"
                Display="None" />
            <asp:Panel ID="DetailsPanel" runat="server">

                <table class="searchEngineMaster">
                    <tr>
                        <td>
                            <table class="searchEngine">

                                <tr>
                                    <td>
                                        <asp:Label ID="lblNoOdP" runat="server" Text="ID OdP"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNumber" runat="server" Text="Anno/Numero"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTitleContains" runat="server" Text="Titolo contiene..."></asp:Label>
                                    </td>

                                    <td>
                                        <asp:Label ID="lblClosed" runat="server" Text="Stato"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLossMaking" runat="server" Text="Redditività"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAgente1" runat="server" Text="Agente1"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblManager" runat="server" Text="Gestione"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblddlCustomers" runat="server" Text="Cliente OdP"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNo" runat="server" Text="No preventivo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>
                                    </td>
                                    <%--<td>
                                        <asp:Label ID="Label2" runat="server" Text="Data fine da"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEndDate" runat="server" Text="Data fine a"></asp:Label>
                                    </td>--%>
                                    <td>
                                        <%--<asp:Label ID="lblEndDate" runat="server" Text="Data fine a"></asp:Label>--%>
                                    </td>

                                </tr>
                                <tr>

                                    <td>
                                        <cfb:IntTextBox ID="itbNoOdP" runat="server" CssClass="droplist" ShowFindButton="false"></cfb:IntTextBox>
                                    </td>
                                    <td>
                                        <cfb:YearCounterTextBox ID="yctNumber" runat="server" CssClass="droplist" ShowFindButton="false"></cfb:YearCounterTextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTitleContains" runat="server" CssClass="droplist">
                                        </asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:DropDownList ID="ddlClosed" runat="server" CssClass="droplist">
                                            <asp:ListItem Text="Tutti" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Evasi" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Non evasi" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLossMaking" runat="server" CssClass="droplist">
                                            <asp:ListItem Text="Tutti" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="In perdita" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="In attivo" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Superiore a 30%" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Superiore a 70%" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                    <td>
                                        <asp:DropDownList ID="ddlAgente1" runat="server" DataSourceID="ldsAgente1" DataTextField="DescrizioneAgente1" DataValueField="IDAgente1"
                                            CssClass="droplist" OnSelectedIndexChanged="PersistSelection" OnDataBound="ddlAgente1_DataBound">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="ldsAgente1" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                            TableName="Customers" OrderBy="DescrizioneAgente1" OnSelecting="ddlAgente1_Selectiong">
                                        </asp:LinqDataSource>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlManagers" runat="server" CssClass="droplist" DataSourceID="ldsManagers" DataTextField="Description" DataValueField="ID" OnDataBound="ddlManagers_DataBound" OnSelectedIndexChanged="PersistSelection">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="ldsManagers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext" OrderBy="Id" TableName="Managers">
                                        </asp:LinqDataSource>
                                    </td>

                                    <td>
                                        <asp:HiddenField ID="hidCustomer" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="droplist" Width="200px">
                                        </asp:TextBox>

                                    </td>
                                    <td>
                                        <cfb:IntTextBox ID="itbNo" runat="server" CssClass="droplist" ShowFindButton="false" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrderBy" runat="server" DataTextField="Key" DataValueField="Value"
                                            CssClass="droplist" OnSelectedIndexChanged="PersistSelection" OnDataBound="ddlOrderBy_DataBound">
                                        </asp:DropDownList>
                                    </td>

                                    <%--<td>
                                        <asp:TextBox ID="txtEndDateFrom" runat="server" Columns="12"></asp:TextBox>
                                        <asp:ImageButton ID="imgEndFrom" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                                            ImageAlign="Middle" />
                                        <cc1:CalendarExtender ID="txtEndDateFrom_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                                            CssClass="MyCalendar" PopupButtonID="imgEndFrom" TargetControlID="txtEndDateFrom"
                                            PopupPosition="Right">
                                        </cc1:CalendarExtender>
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtEndDateTo" runat="server" Columns="12"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtEndDateTo_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                                            CssClass="MyCalendar" PopupButtonID="imgEndTo" TargetControlID="txtEndDateTo"
                                            PopupPosition="Right">
                                        </cc1:CalendarExtender>

                                        <asp:ImageButton ID="imgEndTo" runat="server" ImageAlign="Middle" ImageUrl="~/DynamicData/Content/Images/Calendar.png" />
                                    </td>--%>
                                    <td align="center">&nbsp;</td>
                                </tr>
                            </table>
                            <table class="searchEngine">
                                <tr>
                                    
                                    <td>
                                        <asp:LinqDataSource ID="ldsOwners" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext" OrderBy="UniqueName" TableName="Employees" >
                                        </asp:LinqDataSource>
                                        <asp:Label ID="lblddlOwner" runat="server" Text="Proprietario preventivo"></asp:Label>
                                    </td>

                                    <td>
                                        <asp:Label ID="lblBollettato" runat="server" Text="Bollettato"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblValoreFatturato" runat="server" Text="Fatturato"></asp:Label>
                                    </td>
                                    <%--<td>
                                        <asp:Label ID="lblAgente2" runat="server" Text="Agente2"></asp:Label>
                                    </td>--%>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Operatore"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:LinqDataSource ID="ldsNonConformities" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext" OrderBy="ID" TableName="NonConformities">
                                        </asp:LinqDataSource>
                                        <asp:Label ID="lblNonConformities" runat="server" Text="Non conformità"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Reclamo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:LinqDataSource ID="ldsCorrectiveActions" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext" OrderBy="ID" TableName="CorrectiveActions">
                                        </asp:LinqDataSource>
                                        <asp:Label ID="Label2" runat="server" Text="Azione correttiva"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Data inizio da"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStartDate" runat="server" Text="Data inizio a"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="Data bolla da"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="Data bolla a"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td>
                                        <asp:DropDownList ID="ddlOwners" runat="server" CssClass="droplist" DataSourceID="ldsOwners" DataTextField="UniqueName" DataValueField="ID" OnDataBound="ddlOwners_DataBound" OnSelectedIndexChanged="PersistSelection">
                                        </asp:DropDownList>
                                    </td>

                                    <td>
                                        <asp:DropDownList ID="dlBollettato" runat="server" CssClass="droplist">
                                            <asp:ListItem Text="Tutti" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Bollettati" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Non bollettati" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dlFatturato" runat="server" CssClass="droplist">
                                            <asp:ListItem Text="Tutti" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Fatturato" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Non fatturato" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOperators" runat="server" CssClass="droplist" DataSourceID="ldsOwners" DataTextField="UniqueName" DataValueField="ID" OnDataBound="ddlOperators_DataBound" OnSelectedIndexChanged="PersistSelection">
                                            <asp:ListItem Text="Tutti" Value="" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNonConformities" runat="server" CssClass="droplist" DataSourceID="ldsNonConformities" DataTextField="Description" DataValueField="ID" OnDataBound="ddlNonConformities_DataBound" OnSelectedIndexChanged="PersistSelection">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlComplaintReceived" runat="server" CssClass="droplist" OnDataBound="ddlComplaintReceived_DataBound">
                                            <asp:ListItem Text="Tutti" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCorrectiveActions" runat="server" CssClass="droplist" DataSourceID="ldsCorrectiveActions" DataTextField="Description" DataValueField="ID" OnDataBound="ddlCorrectiveActions_DataBound" OnSelectedIndexChanged="PersistSelection">
                                        </asp:DropDownList>
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtDateStartFrom" runat="server" Columns="10" Width="60"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtDateStartFrom_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                                            CssClass="MyCalendar" PopupButtonID="imgStartFrom" TargetControlID="txtDateStartFrom"
                                            PopupPosition="Right">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgStartFrom" runat="server" ImageAlign="Middle" ImageUrl="~/DynamicData/Content/Images/Calendar.png" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateStartTo" runat="server" Columns="10" Width="60"></asp:TextBox>
                                        <asp:ImageButton ID="imgStartTo" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                                            ImageAlign="Middle" />
                                        <cc1:CalendarExtender ID="txtDateStartTo_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                                            CssClass="MyCalendar" PopupButtonID="imgStartTo" TargetControlID="txtDateStartTo"
                                            PopupPosition="Right">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBolFrom" runat="server" Columns="10" Width="60"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtBolFrom_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                                            CssClass="MyCalendar" PopupButtonID="imgBolFrom" TargetControlID="txtBolFrom"
                                            PopupPosition="Left">
                                        </cc1:CalendarExtender>

                                        <asp:ImageButton ID="imgBolFrom" runat="server" ImageAlign="Middle" ImageUrl="~/DynamicData/Content/Images/Calendar.png" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBolTo" runat="server" Columns="10" Width="60"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtBolTo_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                                            CssClass="MyCalendar" PopupButtonID="imgBolTo" TargetControlID="txtBolTo"
                                            PopupPosition="Left">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgBolTo" runat="server" ImageAlign="Middle" ImageUrl="~/DynamicData/Content/Images/Calendar.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9" align="left">
                                        <asp:LinkButton ID="lbtEmpty" runat="server" Text="RESET" Font-Bold="true" CssClass="gridview"
                                            ForeColor="Red" OnClick="lbtEmpty_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" ForeColor="Green"
                                            Font-Bold="true" Text="CERCA"
                                            OnClick="lbtUpdateGrid_Click" />
                                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="vertical-align: top!important">
                            <asp:DataList ID="dlsNonConformities" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                <HeaderTemplate>
                                    <span>Codici Non-Conformità</span>
                                </HeaderTemplate>
                                <HeaderStyle BorderWidth="1" BorderColor="Navy" HorizontalAlign="Center" BackColor="Navy" ForeColor="White" />
                                <ItemTemplate>
                                    <label><%# Container.DataItem %></label>
                                </ItemTemplate>
                                <ItemStyle BorderWidth="1" ForeColor="Black" />
                            </asp:DataList>
                        </td>
                        <td style="vertical-align: top!important">
                            <asp:DataList ID="dlsCorrectiveActions" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                <HeaderTemplate>
                                    <span>Codici Azioni Correttive</span>
                                </HeaderTemplate>
                                <HeaderStyle BorderWidth="1" BorderColor="Red" HorizontalAlign="Center" BackColor="Red" ForeColor="White" />
                                <ItemTemplate>
                                    <label><%# Container.DataItem %></label>
                                </ItemTemplate>
                                <ItemStyle BorderWidth="1" BorderColor="Red" ForeColor="Black" />
                            </asp:DataList>
                        </td>
                    </tr>

                </table>


                <table>
                    <tr>
                        <td>&nbsp;
                            <asp:LinkButton ID="lbtPrintProductionOrders" runat="server" CssClass="gridview"
                                Font-Bold="True" OnClick="lbtPrintProductionOrders_Click" Text="Stampa tabella" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lbtExportToExcel" runat="server" CssClass="gridview" Font-Bold="True"
                                Text="Esporta in Excel" OnClick="lbtExportToExcel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <asp:LinqDataSource ID="ldsProductionOrdersStats" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="VW_QUOPORCostsPrices" OnSelecting="ldsProductionOrders_Selecting"
                                AutoGenerateOrderByClause="true" EnableViewState="false">

                                <OrderByParameters>
                                    <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                </OrderByParameters>

                            </asp:LinqDataSource>

                            <asp:GridView ID="grdProductionOrdersStats" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" DataKeyNames="ID" DataSourceID="ldsProductionOrdersStats"
                                CssClass="gridview" OnPageIndexChanging="grdProductionOrders_PageIndexChanging"
                                OnRowDataBound="grdProductionOrders_RowDataBound" OnDataBound="grdProductionOrders_DataBound"
                                OnRowDeleted="grdProductionOrders_RowDeleted" OnRowCommand="grdProductionOrders_RowCommand"
                                PagerSettings-Position="Top" ShowFooter="True" OnPreRender="grdProductionOrders_PreRender">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <PagerStyle CssClass="footer" />
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
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Gestione voce" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--0--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblStartDate" runat="server" Text="Data lancio"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime"
                                                DataFormatString="{0:d}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--1--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CloseLinkButton" runat="server" CommandName="Close" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                CausesValidation="false" Text="Evadi" OnClientClick="return confirm('Confermi l\'evasione di questo ordine di produzione?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--2--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypDetails" runat="server" ImageUrl="~/Images/money_euro.png" ToolTip="Dettaglio statistiche" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--3--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--4--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNumber" runat="server" Text="Numero"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycNumber" runat="server" DataField="Number" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--5--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAgent" runat="server" Text="Agente"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DescrizioneAgente1") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--6--%>
                                    <asp:TemplateField SortExpression="QUOSubject">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text="Preventivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<asp:HyperLink ID="hypSubject" runat="server" Width="300" Text='<%# DataBinder.Eval(Container.DataItem, "QUOSubject") %>' ForeColor="#718ABE"
                                            ToolTip="Visualizza preventivo"></asp:HyperLink>--%>
                                            <asp:LinkButton ID="lbtSubject" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "QUOSubject") %>'
                                                ForeColor="#718ABE" ToolTip="Vai a preventivo" CommandName="GoToQuotation" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID_Quotation") %>'></asp:LinkButton>
                                            <%-- <asp:DynamicControl ID="dycSubject" runat="server" DataField="QUOSubject" UIHint="Text250" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField HeaderStyle-Width="70">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQUOTotCost" runat="server" Text="Costo preventivato €"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycQUOTotCost" runat="server" DataField="QUOTotCost" UIHint="Text"
                                                DataFormatString="{0:N2}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>--%>
                                    <%--7--%>
                                    <asp:TemplateField HeaderStyle-Width="70" Visible="false">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPORTotCost" runat="server" Text="Costo da consuntivo (CC) €"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPORTotCost" runat="server" DataField="PORTotCost" UIHint="Text"
                                                DataFormatString="{0:N2}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--8--%>
                                    <asp:TemplateField HeaderStyle-Width="70">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPORTotHistoricalCost" runat="server" Text="Costo storico da consuntivo (CSC) €"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPORTotHistoricalCost" runat="server" DataField="PORTotHistoricalCost"
                                                UIHint="Text" DataFormatString="{0:N2}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--9--%>
                                    <asp:TemplateField HeaderStyle-Width="70">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProvvTotValue" runat="server" Text="Provvigioni (P) €"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycProvvTotValue" runat="server" DataField="ProvvTotValue"
                                                UIHint="Text" DataFormatString="{0:N2}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--10--%>
                                    <asp:TemplateField HeaderStyle-Width="70">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblTotCosts" runat="server" Text="Totale costi (CSC+P) €"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycTotCosts" runat="server" DataField="TotCosts"
                                                UIHint="Text" DataFormatString="{0:N2}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--11--%>
                                    <asp:TemplateField HeaderStyle-Width="70">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblFATTotValue" runat="server" Text="Fatturato (F) €"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycFATTotValue" runat="server" DataField="FATTotValue" UIHint="Text"
                                                DataFormatString="{0:N2}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--12--%>
                                    <asp:TemplateField HeaderStyle-Width="70">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSaving" runat="server" Text="Differenza (F-CSC-P) €"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycSaving" runat="server" DataField="Saving" UIHint="Text"
                                                DataFormatString="{0:N2}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--13--%>
                                    <asp:TemplateField HeaderStyle-Width="70">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPercentageSaving" runat="server" Text="% risparmio effettivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPercentageSaving" runat="server" DataField="PercentageSaving"
                                                UIHint="Text" DataFormatString="{0:P2}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--14--%>
                                    <asp:TemplateField HeaderStyle-Width="10">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNonConformityCode" runat="server" Text="Cod. NC"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="hidden" id="hidCheck" value='<%# DataBinder.Eval(Container.DataItem, "ID").ToString()%>' />
                                            <asp:DynamicControl ID="dycNonConformityCode" runat="server" DataField="NonConformityCode" UIHint="SmallInteger_Edit"
                                                HtmlEncode="false" HeightInRows="2" CssClass="updNonConformityCodeToOdp" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--15--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNonConformityCode" runat="server" Text="Cod. NC"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="nonConformityValue"><%# DataBinder.Eval(Container.DataItem, "NonConformityCode")%></span>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--16--%>
                                    <asp:TemplateField HeaderStyle-Width="10">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCompliantReceived" runat="server" Text="Reclamo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="hidden" id="hidCheck" value='<%# DataBinder.Eval(Container.DataItem, "ID").ToString()%>' />
                                            <asp:DynamicControl ID="dycComplaintReceived" runat="server" DataField="ComplaintReceived" UIHint="YesNo_Edit"  AllowNullValue="false"
                                                HtmlEncode="false" HeightInRows="2" CssClass="updComplaintReceivedToOdp" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--17--%>
                                    <asp:TemplateField HeaderStyle-Width="10">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCompliantReceived" runat="server" Text="Reclamo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="compliantReceivedValue"><%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "ComplaintReceived")) == 1 ? "Si" : "No" %></span>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--18--%>
                                    <asp:TemplateField HeaderStyle-Width="10">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCorrectiveActionCode" runat="server" Text="Cod. AC"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="hidden" id="hidCheck" value='<%# DataBinder.Eval(Container.DataItem, "ID").ToString()%>' />
                                            <asp:DynamicControl ID="dycCorrectiveActionCode" runat="server" DataField="CorrectiveActionCode" UIHint="SmallInteger_Edit"
                                                HtmlEncode="false" HeightInRows="2" CssClass="updCorrectiveActionCodeToOdp" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--19--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCorrectiveActionCode" runat="server" Text="Cod. AC"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="correctiveActionValue"><%# DataBinder.Eval(Container.DataItem, "CorrectiveActionCode")%></span>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--20--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAccountNote" runat="server" Text="Note amministrative"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="hidden" id="hidCheck" value='<%# DataBinder.Eval(Container.DataItem, "ID").ToString()%>' />
                                            <asp:DynamicControl ID="dycAccountNote" runat="server" DataField="AccountNote" UIHint="Text250Multiline_Edit"
                                                HtmlEncode="false" HeightInRows="2" CssClass="updNoteToOdp" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAccountNoteRO" runat="server" Text="Note amministrative"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycAccountNoteRO" runat="server" DataField="AccountNote" UIHint="Text250"
                                                HtmlEncode="false" HeightInRows="1" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                    </asp:TemplateField>--%>

                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNote" runat="server" Text="Note tecniche"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text250Multiline"
                                                HtmlEncode="false" HeightInRows="2" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>--%>

                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPriceCom" runat="server" Text="Prezzo com. a Cliente"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPriceCom" runat="server" DataField="PriceCom" UIHint="Text"
                                                HtmlEncode="false" HeightInRows="1" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                    </asp:TemplateField>--%>
                                    <%--21--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblOwnerPrj" runat="server" Text="Resp. Progetto"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="dycOwnerPrj" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OwnerName") %>' Width="100" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--22--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblEndDate" runat="server" Text="Data fine"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycEndDate" runat="server" DataField="DataBolla" UIHint="DateTime"
                                                DataFormatString="{0:d}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCustomer" runat="server" DataField="PORID_Customer" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPORSUnitPrice" runat="server" Text="Prezzo unitario"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPORSUnitPrice" runat="server" DataField="PORSUnitPrice"
                                                UIHint="Text" DataFormatString="{0:N5}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPORSQuantity" runat="server" Text="Quantità prodotta"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPORSQuantity" runat="server" DataField="PORSQuantity"
                                                UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQUOUnitCost" runat="server" Text="Costo unitario da preventivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycQUOUnitCost" runat="server" DataField="QUOUnitCost" UIHint="Text"
                                                DataFormatString="{0:N5}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPORSUnitCost" runat="server" Text="Costo unitario effettivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPORSUnitCost" runat="server" DataField="PORSUnitCost"
                                                UIHint="Text" DataFormatString="{0:N5}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQuotationMarkup" runat="server" Text="% Markup Teo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycQuotationMarkup" runat="server" DataField="QuotationMarkup"
                                                UIHint="Text" DataFormatString="{0:P2}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProductionMarkup" runat="server" Text="% Markup Eff"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycProductionMarkup" runat="server" DataField="ProductionMarkup"
                                                UIHint="Text" DataFormatString="{0:P2}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <%--<PagerStyle CssClass="footer" />--%>
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

            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
