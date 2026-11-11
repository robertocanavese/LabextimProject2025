<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="CloseOfDaysConsole.aspx.cs" Inherits="LabExtim.CloseOfDaysConsole"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

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

                $("#ctl00_ContentPlaceHolder1_senMain_txtTextField1").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: document.location.href.split("?")[0].split("#")[0] + '/GetUsers',
                            data: JSON.stringify({ q: request.term }),
                            dataType: "json",
                            type: "POST",
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            success: function (result) {
                                if (result.hasOwnProperty("d")) { result = result.d; }
                                var data = jQuery.parseJSON(result);
                                response($.map(data, function (item) {
                                    return {
                                        label: item.UniqueName,
                                        value: item.UniqueName
                                    }
                                }))
                            },
                            error: function (response) {
                            },
                            failure: function (response) {
                            }
                        });
                    },
                    minLength: 3,
                    select: function (event, ui) {
                        // ricerca operatore in LIKE: si valorizza solo il testo
                        $("#ctl00_ContentPlaceHolder1_senMain_txtTextField1").val(ui.item.label);
                        return false;
                    }
                });

            }

            SetAutoComplete();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        });
    </script>

    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Segnalazioni di fine giornata
            </h2>
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdCloseOfDays"
                Display="None" />
            <table width="100%">
                <tr>
                    <td>
                        <cfb:SearchEngine ID="senMain" runat="server"></cfb:SearchEngine>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="#C62828"></asp:Label>
                    </td>
                </tr>
            </table>

            <asp:Panel ID="DetailsPanel" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="#2E7D32" Text="Aggiorna lista" OnClick="lbtUpdateGrid_Click" />
                            &nbsp;
                            <asp:LinkButton ID="lbtExportToExcel" runat="server" CssClass="gridview" Font-Bold="True"
                                Text="Esporta in Excel" OnClick="lbtExportToExcel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsCloseOfDays" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="CloseOfDays"
                                AutoGenerateOrderByClause="true" OnSelecting="ldsCloseOfDays_Selecting" >
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdCloseOfDays" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" DataSourceID="ldsCloseOfDays" DataKeyNames="Id_User,ProductionDate" CssClass="gridview"
                                OnPageIndexChanging="grdCloseOfDays_PageIndexChanging" OnDataBound="grdCloseOfDays_DataBound"
                                PagerSettings-Position="Top" ShowFooter="True" AllowSorting="true" OnSorting="grdCloseOfDays_Sorting">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <PagerStyle CssClass="footer" />
                                <Columns>
                                    <asp:BoundField DataField="ProductionDate" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Font-Bold="true" SortExpression="ProductionDate" />
                                    <asp:BoundField DataField="Employee.UniqueName" HeaderText="Operatore" ItemStyle-Font-Bold="true" SortExpression="Employee.UniqueName" />
                                    <asp:BoundField DataField="Company.Description" HeaderText="Azienda" SortExpression="Company.Description" />
                                    <asp:TemplateField HeaderText="Note" SortExpression="Note">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNote" runat="server" Text='<%# Eval("Note") %>' TextMode="MultiLine" Rows="3"
                                                ReadOnly="true" EnableViewState="false" Width="500px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
