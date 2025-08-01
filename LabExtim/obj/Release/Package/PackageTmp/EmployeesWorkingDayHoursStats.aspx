<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True"
    CodeBehind="EmployeesWorkingDayHoursStats.aspx.cs" Inherits="LabExtim.EmployeesWorkingDayHoursStats"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Import Namespace="CMLabExtim" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/CFBTimePicker.ascx" TagName="CFBTimePicker" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox" TagPrefix="cfb" %>
<%--<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"    TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var JSON = JSON || {};

        $(document).ready(function () {


            var ajaxLoading4 = false;

            function EndRequestHandler() {

                SetAutoComplete();
            }

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

            SetAutoComplete();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        });
    </script>

    <!--<asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />-->
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Ore di lavoro operatori
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>
            <table class="searchEngine">
                <tr>
                    <td>
                        <asp:Label ID="lblitbNoOdP" runat="server" Text="ID OdP"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNumber" runat="server" Text="Anno/Numero OdP"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlOwner" runat="server" Text="Operatore"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPhase" runat="server" Text="Fase"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlGroupBy" runat="server" Text="Raggruppa voci per..."></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>
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
                        <asp:DropDownList ID="ddlOwners" runat="server" AutoPostBack="true" DataSourceID="ldsOwners"
                            DataTextField="UniqueName" DataValueField="ID" CssClass="droplist" OnDataBound="ddlOwners_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsOwners" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Employees" OrderBy="UniqueName">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:HiddenField ID="hidCustomer" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="droplist" Width="200px">
                        </asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPhases" runat="server" AutoPostBack="true" DataTextField="ItemDescription"
                            DataValueField="ID" CssClass="droplist" DataSourceID="ldsPhases" OnSelectedIndexChanged="PersistSelection"
                            OnDataBound="ddlPhases_DataBound" />
                        <asp:LinqDataSource ID="ldsPhases" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="PickingItems" OrderBy="ItemDescription" Where="TypeCode=31">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlItemTypes" runat="server" AutoPostBack="true" DataSourceID="ldsItemTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlItemTypes_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsItemTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ItemTypes" Where="Code=10 || Code=65">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGroupBy" runat="server" AutoPostBack="true" DataTextField="Key"
                            DataValueField="Value" CssClass="droplist" OnSelectedIndexChanged="PersistSelection"
                            OnDataBound="ddlGroupBy_DataBound">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="true" DataTextField="Key"
                            DataValueField="Value" CssClass="droplist" OnSelectedIndexChanged="PersistSelection"
                            OnDataBound="ddlOrderBy_DataBound">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="7">
                        <asp:Label ID="lblPeriod" runat="server" Text="Selezione periodo (selezionando l'Odp questo filtro resta disattivato)"></asp:Label></td>
                </tr>
                <tr>
                    <td align="center" colspan="7">
                        <cfb:CFBTimePicker ID="btpPeriod" runat="server" DropDownListCssClass="searchEngine"
                            LabelAnno="Anno: " LabelPeriodo=" Periodo: "></cfb:CFBTimePicker>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
            </table>
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdEmployeesWorkingDaysHoursStats"
                Display="None" />
            <asp:Panel ID="pnlDefault" runat="server">
                <table>
                    <tr>
                        <td>&nbsp;
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" OnClick="PersistSelection" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lbtResetFilter" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Red" Text="Reset" OnClick="lbtResetFilter_Click" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lbtPrintEmployeesWorkingDaysHours" runat="server" CssClass="gridview"
                                Font-Bold="True" OnClick="lbtPrintEmployeesWorkingDaysHours_Click" Text="Stampa tabella"
                                Enabled="false" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lbtExportToExcel" runat="server" CssClass="gridview" Font-Bold="True"
                                Text="Esporta in Excel" OnClick="lbtExportToExcel_Click" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lbtGoToPivot" runat="server" CssClass="gridview" Font-Bold="True"
                                Text="Vai a pivot" OnClick="lbtGoToPivot_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<asp:LinqDataSource ID="ldsEmployeesWorkingDaysHoursStats" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="VW_EmployeesWorkingDayHour" 
                                OnSelected="ldsEmployeesWorkingDaysHours_Selected" AutoGenerateOrderByClause="false"
                                EnableViewState="False">
                            </asp:LinqDataSource>--%>
                            <%--OnSelecting="ldsEmployeesWorkingDaysHours_Selecting"--%>
                            <asp:GridView ID="grdEmployeesWorkingDaysHoursStats" runat="server" AutoGenerateColumns="False"
                                CssClass="gridview"
                                OnDataBound="grdEmployeesWorkingDaysHours_DataBound" OnRowDataBound="grdEmployeesWorkingDaysHours_RowDataBound"
                                OnRowDeleted="grdEmployeesWorkingDaysHours_RowDeleted" OnRowCommand="grdEmployeesWorkingDaysHours_RowCommand"
                                OnPreRender="grdEmployeesWorkingDaysHoursStats_PreRender">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <%-- DataKeyNames="ID, ProductionDate" OnPageIndexChanging="grdEmployeesWorkingDaysHours_PageIndexChanging" AllowPaging="True" DataSourceID="ldsEmployeesWorkingDaysHoursStats"--%>
                                <Columns>
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblOdP" runat="server" Text="OdP"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycOdP" runat="server" DataField="ID_ProductionOrder" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblOwner" runat="server" Text="Operatore"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycOwner" runat="server" DataField="UniqueName" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblYear" runat="server" Text="Anno"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycYear" runat="server" DataField="YearProductionDate" UIHint="Text" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblMonth" runat="server" Text="Mese"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycMonth" runat="server" DataField="MonthProductionDate"
                                                UIHint="Text" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProductionDate" runat="server" Text="Data produzione" Width="90"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycProductionDate" runat="server" DataField="ProductionDate"
                                                UIHint="DateTime" DataFormatString="{0:d}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPhase" runat="server" Text="Fase"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPhase" runat="server" DataField="ItemDescription" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemTypeDescription"
                                                UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Tipo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycType" runat="server" DataField="TypeDescription" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProductionTime" runat="server" Text="Ore lavoro (hh:mm)"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="dycProductionTime" runat="server" Text='<%# Utilities.DisplayAsTimeSpan(Convert.ToDecimal(Eval("ProductionTime"))) %>'
                                                Font-Bold="true" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProductionTime100" runat="server" Text="Ore lavoro (hh,cchh)"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="dycProductionTime100" runat="server" Text='<%#  Utilities.DisplayAsCentiHours(Convert.ToDecimal(Eval("ProductionTime"))) %>'
                                                Font-Bold="true" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblRawMaterialX" runat="server" Text="Quantità"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycRawMaterialX" runat="server" DataField="RawMaterialX"
                                                UIHint="Text" DataFormatString="{0:N0}" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblRawMaterialY" runat="server" Text="b"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycRawMaterialY" runat="server" DataField="RawMaterialY"
                                                UIHint="Text" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblRawMaterialZ" runat="server" Text="h"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycRawMaterialZ" runat="server" DataField="RawMaterialZ"
                                                UIHint="Text" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblHourlyProductivity" runat="server" Text="Q.tà/h"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycHourlyProductivity" runat="server" DataField="HourlyProductivity"
                                                UIHint="Text" DataFormatString="{0:N2}" />
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="true" HorizontalAlign="Right" />
                                    </asp:TemplateField>--%>



                                    <asp:BoundField runat="server" DataField="ID_ProductionOrder" HeaderText="OdP" />


                                    <asp:BoundField runat="server" DataField="UniqueName" HeaderText="Operatore" />


                                    <asp:BoundField runat="server" DataField="YearProductionDate" HeaderText="Anno" />


                                    <asp:BoundField runat="server" DataField="MonthProductionDate" HeaderText="Mese" />


                                    <asp:BoundField runat="server" DataField="ProductionDate" HeaderText="Data produzione" />


                                    <asp:BoundField runat="server" DataField="ItemDescription" HeaderText="Fase" />


                                    <asp:BoundField runat="server" DataField="ItemTypeDescription" HeaderText="Tipo voce" />


                                    <asp:BoundField runat="server" DataField="TypeDescription" HeaderText="Tipo" />

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProductionTime" runat="server" Text="Ore lavoro (hh:mm)"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="dycProductionTime" runat="server" Text='<%# Utilities.DisplayAsTimeSpan(Convert.ToDecimal(Eval("ProductionTime"))) %>'
                                                Font-Bold="true" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>


                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProductionTime100" runat="server" Text="Ore lavoro (hh,cchh)"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="dycProductionTime100" runat="server" Text='<%#  Utilities.DisplayAsCentiHours(Convert.ToDecimal(Eval("ProductionTime"))) %>'
                                                Font-Bold="true" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>



                                    <asp:BoundField runat="server" DataField="RawMaterialX" HeaderText="Quantità" ItemStyle-HorizontalAlign="Right" />


                                    <asp:BoundField runat="server" DataField="RawMaterialY" HeaderText="b" ItemStyle-HorizontalAlign="Right" />


                                    <asp:BoundField runat="server" DataField="RawMaterialZ" HeaderText="h" ItemStyle-HorizontalAlign="Right" />


                                    <asp:BoundField runat="server" DataField="HourlyProductivity" HeaderText="Q.tà/h" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />


                                </Columns>
                                <%--<PagerStyle CssClass="footer" />
                                <PagerTemplate>
                                    <asp:GridViewPager ID="Pager1" runat="server" />
                                </PagerTemplate>--%>
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>


            <asp:Panel ID="pnlPivot" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>&nbsp; 
                             <asp:LinkButton ID="lbtUpdatePivot" runat="server" CssClass="gridview" Font-Bold="True"
                                 ForeColor="Green" Text="Aggiorna pivot" OnClick="PersistSelection" />

                            &nbsp; &nbsp; &nbsp; &nbsp;
                                                        <asp:LinkButton ID="lbtExportPivotToExcel" runat="server" CssClass="gridview" Font-Bold="True"
                                                            Text="Esporta pivot in Excel" OnClick="lbtExportPivotToExcel_Click" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lbtGoToDefault" runat="server" CssClass="gridview" Font-Bold="True"
                                Text="Torna a statistiche" OnClick="lbtGoToDefault_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSubTitle" runat="server" Text="Prospetto ore mensili operatori"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdEmployeesWorkingDaysHoursPivot" runat="server" CssClass="gridview" Width="100%" AutoGenerateColumns="false" ShowFooter="true" OnDataBound="grdEmployeesWorkingDaysHoursPivot_DataBound" OnRowDataBound="grdEmployeesWorkingDaysHoursPivot_RowDataBound">
                                <RowStyle CssClass="row" HorizontalAlign="Right" />
                                <AlternatingRowStyle CssClass="altRow" HorizontalAlign="Right" />
                                <EmptyDataTemplate>
                                    Nessun dato
                                </EmptyDataTemplate>
                                <%--<HeaderStyle CssClass="gridHeader" />
                                <FooterStyle CssClass="gridFooter" />
                                <RowStyle CssClass="gridRow" />
                                <AlternatingRowStyle CssClass="altGridRow" />
                                <RowStyle Wrap="false" />--%>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtUpdateGrid" />
            <asp:PostBackTrigger ControlID="lbtPrintEmployeesWorkingDaysHours" />
            <asp:PostBackTrigger ControlID="lbtExportToExcel" />
            <asp:PostBackTrigger ControlID="ddlOwners" />
            <asp:PostBackTrigger ControlID="ddlPhases" />
            <asp:PostBackTrigger ControlID="ddlItemTypes" />
            <asp:PostBackTrigger ControlID="ddlGroupBy" />
            <asp:PostBackTrigger ControlID="ddlOrderBy" />

            <asp:PostBackTrigger ControlID="lbtGoToPivot" />
            <asp:PostBackTrigger ControlID="lbtExportPivotToExcel" />
            <asp:PostBackTrigger ControlID="lbtGoToDefault" />

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
