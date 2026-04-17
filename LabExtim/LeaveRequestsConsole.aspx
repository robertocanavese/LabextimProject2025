<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="LeaveRequestsConsole.aspx.cs" Inherits="LabExtim.LeaveRequestsConsole" MaintainScrollPositionOnPostback="true" %>

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
                            url: document.location.href.split("?")[0].split("#")[0] + '/GetUsers',
                            data: "{ 'q': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            success: function (result) {
                                if (result.hasOwnProperty("d")) { result = result.d; }
                                debugger;
                                var data = jQuery.parseJSON(result);
                                response($.map(data, function (item) {
                                    return {
                                        label: item.UniqueName,
                                        value: item.Id
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
                        debugger;
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
            <h2>Gestione permessi/ferie
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
                                ForeColor="Green" Text="Nuova richiesta" />
                            &nbsp;
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintLeaveRequests" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtPrintLeaveRequests_Click" Text="Stampa tabella" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsLeaveRequests" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="LeaveRequests" OnSelected="ldsLeaveRequests_Selected" EnableUpdate="True"
                                AutoGenerateOrderByClause="true" EnableDelete="True" EnableInsert="True" OnSelecting="ldsLeaveRequests_Selecting">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdLeaveRequests" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" DataSourceID="ldsLeaveRequests" DataKeyNames="ID" CssClass="gridview"
                                OnPageIndexChanging="grdLeaveRequests_PageIndexChanging" OnDataBound="grdLeaveRequests_DataBound"
                                OnRowDataBound="grdLeaveRequests_RowDataBound" OnRowDeleted="grdLeaveRequests_RowDeleted"
                                OnRowCommand="grdLeaveRequests_RowCommand" OnPreRender="grdLeaveRequests_PreRender">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibtApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("ID").ToString() %>'
                                                Text="Approva" ToolTip="Approva permesso" OnClientClick="javascript:return confirm('Sei sicuro di voler approvare questa richiesta di permesso?');"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibtDeny" runat="server" CommandName="Deny" CommandArgument='<%# Eval("ID").ToString() %>'
                                                Text="Respingi" ToolTip="Respingi permesso" OnClientClick="javascript:return confirm('Sei sicuro di voler respingere questa richiesta di permesso?');"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Modifica richiesta" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="Company.Description" HeaderText="Company" />
                                    <asp:BoundField DataField="RequestDate" HeaderText="Data richiesta" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Employee.UniqueName" HeaderText="Richiedente" ItemStyle-Font-Bold="true"  />
                                    <asp:BoundField DataField="LeaveType1.Description" HeaderText="Tipo permesso" />
                                    <asp:BoundField DataField="StartDate" HeaderText="Data inizio assenza" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="EndDate" HeaderText="Data fine assenza" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="DayFraction1.Description" HeaderText="Orario" />
                                    <asp:BoundField DataField="VacationDays" HeaderText="Giorni assenza" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="VacationHours" HeaderText="Ore totali assenza" ItemStyle-HorizontalAlign="Right"  />
                                    <asp:BoundField DataField="Statuse.Description" HeaderText="Stato richiesta" ItemStyle-Font-Bold="true"  />
                                    <asp:BoundField DataField="StatusDate" HeaderText="Aggiornamento" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="Employee1.UniqueName" HeaderText="Richiesta a" />
                                    <asp:BoundField DataField="MessageToManager" HeaderText="Comunicazioni da richiedente" />
                                    <asp:BoundField DataField="Employee2.UniqueName" HeaderText="Approvata da" ItemStyle-Font-Bold="true"  />
                                    <asp:BoundField DataField="MessageToApplicant" HeaderText="Comunicazioni da responsabile" />
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
