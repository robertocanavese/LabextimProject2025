<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True"
    CodeBehind="ProductionOrderDetailsConsumption.aspx.cs" Inherits="LabExtim.ProductionOrderDetailsConsumption"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Import Namespace="CMLabExtim" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/CFBTimePicker.ascx" TagName="CFBTimePicker" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox" TagPrefix="cfb" %>
<%--<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"    TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--<asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />-->
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>

    <script type="text/javascript">

        var JSON = JSON || {};

        $(document).ready(function () {


            function EndRequestHandler() {

                SetAutoCompletePI();
                SetAutoCompleteMI();
            }


            function SetAutoCompletePI() {

                $("#ctl00_ContentPlaceHolder1_txtPickingItem").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: document.location.href.split("?")[0].split("#")[0] + '/GetPickingItems',
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
                        $("#ctl00_ContentPlaceHolder1_txtPickingItem").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_hidPickingItem").val(ui.item.value);
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

            function SetAutoCompleteMI() {

                $("#ctl00_ContentPlaceHolder1_txtMacroItem").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: document.location.href.split("?")[0].split("#")[0] + '/GetMacroItems',
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
                        $("#ctl00_ContentPlaceHolder1_txtMacroItem").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_hidMacroItem").val(ui.item.value);
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


            SetAutoCompletePI();
            SetAutoCompleteMI();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        });
    </script>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Consumo prodotti
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>

            <asp:Menu ID="mnuOperations1" runat="server" Orientation="Horizontal" CssClass="menu"
                DisappearAfter="1000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                DynamicHorizontalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                <DynamicMenuStyle CssClass="menu" />
            </asp:Menu>
            <br />
            <asp:Menu ID="mnuOperations2" runat="server" Orientation="Horizontal" CssClass="menuBlue"
                DisappearAfter="1000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                DynamicHorizontalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                <DynamicMenuStyle CssClass="menuBlue" />
            </asp:Menu>
            <br />


            <table class="searchEngine">
                <tr>
                    <td>
                        <asp:Label ID="lblitbNoOdP" runat="server" Text="Id OdP"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNumber" runat="server" Text="Anno/Numero"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCompanies" runat="server" Text="Azienda"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlType" runat="server" Text="Tipo"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPickingItem" runat="server" Text="Voce tabella base"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlGroupBy" runat="server" Text="Raggruppa voci per..."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="itbNoOdP" runat="server" CssClass="droplist" AutoPostBack="true" MaxLength="6" Width="50">
                        </asp:TextBox>
                        <asp:RangeValidator ControlToValidate="itbNoOdP"
                            MaximumValue="999999" MinimumValue="0" Type="Integer" ErrorMessage="Non valido!" runat="server" Display="None" />
                    </td>
                    <td>
                        <cfb:YearCounterTextBox ID="yctNumber" runat="server" CssClass="droplist" ShowFindButton="false"></cfb:YearCounterTextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCompanies" runat="server" AutoPostBack="true" DataSourceID="ldsCompanies"
                            DataTextField="Description" DataValueField="ID" CssClass="droplist" OnDataBound="ddlCompanies_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsCompanies" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Companies" OrderBy="ID">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTypes" runat="server" AutoPostBack="true" DataSourceID="ldsTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlTypes_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Types" Where='Category="I"' OrderBy="Order">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlItemTypes" runat="server" AutoPostBack="true" DataSourceID="ldsItemTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlItemTypes_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsItemTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ItemTypes" Where='Category="I"' OrderBy="Order">
                            <%--Where="Code=10 || Code=65"--%>
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:HiddenField ID="hidPickingItem" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtPickingItem" runat="server" CssClass="droplist" Width="200px">
                        </asp:TextBox>

                    </td>

                    <td>
                        <asp:DropDownList ID="ddlGroupBy" runat="server" AutoPostBack="true" DataTextField="Key"
                            DataValueField="Value" CssClass="droplist" OnSelectedIndexChanged="PersistSelection"
                            OnDataBound="ddlGroupBy_DataBound">
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td align="center" colspan="6">
                        <asp:Label ID="lblPeriod" runat="server" Text="Selezione periodo"></asp:Label></td>
                </tr>
                <tr>
                    <td align="center" colspan="6">
                        <cfb:CFBTimePicker ID="btpPeriod" runat="server" DropDownListCssClass="searchEngine"
                            LabelAnno="Anno: " LabelPeriodo=" Periodo: "></cfb:CFBTimePicker>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
            </table>
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdProductionOrderConsumptions"
                Display="None" />
            <asp:Panel ID="DetailsPanel" runat="server">
                <table>
                    <tr>
                        <td>&nbsp;
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" OnClick="PersistSelection" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lbtReset" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Red" Text="Reset" OnClick="lbtReset_Click" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <%--<asp:LinkButton ID="lbtPrintProductionOrderConsumptions" runat="server" CssClass="gridview"
                                Font-Bold="True" OnClick="lbtPrintProductionOrderConsumptions_Click" Text="Stampa tabella"
                                Enabled="false" />--%>
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lbtExportToExcel" runat="server" CssClass="gridview" Font-Bold="True"
                                Text="Esporta in Excel" OnClick="lbtExportToExcel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <span class="bold">Consumo prodotti di periodo</span>
                            <asp:GridView ID="grdProductionOrderConsumptions" runat="server" AutoGenerateColumns="False"
                                CssClass="gridview"
                                OnDataBound="grdProductionOrderConsumptions_DataBound" OnRowDataBound="grdProductionOrderConsumptions_RowDataBound"
                                OnRowDeleted="grdProductionOrderConsumptions_RowDeleted" OnRowCommand="grdProductionOrderConsumptions_RowCommand"
                                OnPreRender="grdProductionOrderConsumptions_PreRender">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <Columns>
                                    <asp:BoundField runat="server" DataField="ID_ProductionOrder" HeaderText="OdP" />
                                    <asp:BoundField runat="server" DataField="Number" HeaderText="Numero" />
                                    <asp:BoundField runat="server" DataField="YearProductionDate" HeaderText="Anno" />
                                    <asp:BoundField runat="server" DataField="MonthProductionDate" HeaderText="Mese" />
                                    <asp:BoundField runat="server" DataField="ProductionDate" HeaderText="Data produzione" />
                                    <asp:BoundField runat="server" DataField="TypeDescription" HeaderText="Type voce tabella base" />
                                    <asp:BoundField runat="server" DataField="ItemTypeDescription" HeaderText="ItemType voce tabella base" />
                                    <asp:BoundField runat="server" DataField="Name" HeaderText="Fornitore" />
                                    <asp:BoundField runat="server" DataField="ID_PickingItemDesc" HeaderText="Voce tabella base" />
                                    <asp:BoundField runat="server" DataField="Order" HeaderText="Ordine" />
                                    <asp:BoundField runat="server" DataField="UMDescription" HeaderText="UM" />
                                    <asp:BoundField runat="server" DataField="RawMaterialQuantity" DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="Right" HeaderText="Quantità" />
                                    <asp:BoundField runat="server" DataField="CurrentCost" DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="Right" HeaderText="Costo attuale €" />
                                    <asp:BoundField runat="server" DataField="HistoricalCost" DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="Right" HeaderText="Costo storico €" />
                                </Columns>
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                        <td style="vertical-align: top">
                            <span class="bold green">Ricorrenza e quantità macrovoci di periodo</span>
                            <asp:GridView ID="grdProductionOrderConsumptionMCount" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="grdProductionOrderConsumptionMCount_PageIndexChanging"
                                CssClass="gridview">
                                <RowStyle CssClass="row green" />
                                <AlternatingRowStyle CssClass="altRow green" />
                                <Columns>
                                    <asp:BoundField runat="server" DataField="ID_ProductionOrder" HeaderText="OdP" />
                                    <asp:BoundField runat="server" DataField="YearProductionDate" HeaderText="Anno" />
                                    <asp:BoundField runat="server" DataField="MonthProductionDate" HeaderText="Mese" />
                                    <asp:BoundField runat="server" DataField="ProductionDate" HeaderText="Data produzione" />
                                    <asp:BoundField runat="server" DataField="TypeDescription" HeaderText="Type voce macrovoce" />
                                    <asp:BoundField runat="server" DataField="ItemTypeDescription" HeaderText="ItemType macrovoce" />
                                    <asp:BoundField runat="server" DataField="descrizione" HeaderText="Macrovoce" />
                                    <asp:BoundField runat="server" DataField="Count" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderText="Volte/periodo" />
                                    <asp:BoundField runat="server" DataField="RawMaterialQuantity" DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderText="Quantità/periodo" />
                                </Columns>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtUpdateGrid" />
            <%--<asp:PostBackTrigger ControlID="lbtPrintProductionOrderConsumptions" />--%>
            <asp:PostBackTrigger ControlID="lbtExportToExcel" />
            <asp:PostBackTrigger ControlID="ddlCompanies" />
            <asp:PostBackTrigger ControlID="ddlTypes" />
            <asp:PostBackTrigger ControlID="ddlItemTypes" />
            <asp:PostBackTrigger ControlID="ddlGroupBy" />

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
