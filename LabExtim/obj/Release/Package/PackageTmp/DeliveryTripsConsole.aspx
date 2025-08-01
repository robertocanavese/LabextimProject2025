<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="DeliveryTripsConsole.aspx.cs" Inherits="LabExtim.DeliveryTripsConsole" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox"
    TagPrefix="cfb" %>
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
                        $("#ctl00_ContentPlaceHolder1_senMain_txtTextField1").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_senMain_hidTextField1").val(ui.item.value);
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
            <h2>Gestione Viaggi
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

            <asp:Panel ID="DetailsPanel" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtNewItem" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Nuovo viaggio" />
                            &nbsp;
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintDeliveryTrips" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtPrintDeliveryTrips_Click" Text="Stampa tabella" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsDeliveryTrips" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="DeliveryTrips" OnSelected="ldsDeliveryTrips_Selected" EnableUpdate="True"
                                AutoGenerateOrderByClause="true" EnableDelete="True" EnableInsert="True" OnSelecting="ldsDeliveryTrips_Selecting">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdDeliveryTrips" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" DataSourceID="ldsDeliveryTrips" DataKeyNames="ID" CssClass="gridview"
                                OnPageIndexChanging="grdDeliveryTrips_PageIndexChanging" OnDataBound="grdDeliveryTrips_DataBound"
                                OnRowDataBound="grdDeliveryTrips_RowDataBound" OnRowDeleted="grdDeliveryTrips_RowDeleted"
                                OnRowCommand="grdDeliveryTrips_RowCommand" OnPreRender="grdDeliveryTrips_PreRender">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibtClose" runat="server" CommandName="Closed" CommandArgument='<%# Eval("ID").ToString() %>'
                                                Text="Chiudi" ToolTip="Chiudi viaggio" OnClientClick="javascript:return confirm('Sei sicuro di voler chiudere questo viaggio? (non sarà più disponibile per l\'associazione agli OdP)');"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibtReopen" runat="server" CommandName="Reopen" CommandArgument='<%# Eval("ID").ToString() %>'
                                                Text="Riapri" ToolTip="Riapri viaggio" OnClientClick="javascript:return confirm('Sei sicuro di voler riaprire questo viaggio?');"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Modifica viaggio" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ID" HeaderText="ID" />
                                    <asp:BoundField DataField="Customer.Name" HeaderText="Cliente" />
                                    <asp:BoundField DataField="Location.Name" HeaderText="Altra destinazione" />
                                    <asp:BoundField DataField="Employee.UniqueName" HeaderText="Autista" />
                                    <asp:BoundField DataField="StartDate" HeaderText="Data inizio" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="EndDate" HeaderText="Data chiusura" DataFormatString="{0:dd/MM/yyyy}" />
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
