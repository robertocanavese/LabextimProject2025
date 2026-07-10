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
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdLeaveRequests"
                Display="None" />
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
                                ForeColor="Green" Text="Aggiorna lista" OnClick="lbtUpdateGrid_Click" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintLeaveRequests" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtPrintLeaveRequests_Click" Text="Stampa tabella" />
                            &nbsp;
                            <asp:LinkButton ID="lbtExportToExcel" runat="server" CssClass="gridview" Font-Bold="True"
                                Text="Esporta in Excel" OnClick="lbtExportToExcel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsLeaveRequests" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="LeaveRequests" 
                                AutoGenerateOrderByClause="true" OnSelecting="ldsLeaveRequests_Selecting" >
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdLeaveRequests" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True"  DataSourceID="ldsLeaveRequests" DataKeyNames="ID" CssClass="gridview"
                                OnPageIndexChanging="grdLeaveRequests_PageIndexChanging" OnDataBound="grdLeaveRequests_DataBound"
                                OnRowDataBound="grdLeaveRequests_RowDataBound" OnRowDeleted="grdLeaveRequests_RowDeleted"
                                OnRowCommand="grdLeaveRequests_RowCommand" PagerSettings-Position="Top" ShowFooter="True"  OnPreRender="grdLeaveRequests_PreRender" AllowSorting="true" OnSorting="grdLeaveRequests_Sorting">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                                <PagerStyle CssClass="footer" />
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
                                    <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Font-Bold="true"  SortExpression="ID" />
                                    <asp:BoundField DataField="Company.Description" HeaderText="Company"  SortExpression="Company.Description"  />
                                    <asp:BoundField DataField="RequestDate" HeaderText="Data richiesta" DataFormatString="{0:dd/MM/yyyy}" SortExpression="RequestDate" />
                                    <asp:BoundField DataField="Employee.UniqueName" HeaderText="Richiedente" ItemStyle-Font-Bold="true"  SortExpression="Employee.UniqueName" />
                                    <asp:BoundField DataField="LeaveType1.Description" HeaderText="Tipo permesso" SortExpression="LeaveType1.Description" />
                                    <asp:BoundField DataField="StartDate" HeaderText="Data inizio assenza" DataFormatString="{0:dd/MM/yyyy}" SortExpression="StartDate"  />
                                    <asp:BoundField DataField="EndDate" HeaderText="Data fine assenza" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EndDate"  />
                                    <asp:BoundField DataField="DayFraction1.Description" HeaderText="Orario"  SortExpression="DayFraction1.Description" />
                                    <asp:BoundField DataField="VacationDays" HeaderText="Giorni assenza" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" SortExpression="VacationDays"  />
                                    <asp:BoundField DataField="VacationHours" HeaderText="Ore assenza" ItemStyle-HorizontalAlign="Right" SortExpression="VacationHours" DataFormatString="{0:N2}"  />
                                    <asp:BoundField DataField="Statuse.Description" HeaderText="Stato richiesta" ItemStyle-Font-Bold="true"  SortExpression="Statuse.Description"  />
                                    <asp:BoundField DataField="StatusDate" HeaderText="Aggiornamento" DataFormatString="{0:dd/MM/yyyy}" SortExpression="StatusDate"  />
                                    <asp:BoundField DataField="Employee1.UniqueName" HeaderText="Richiesta a" SortExpression="Employee1.UniqueName"  />
                                    <asp:BoundField DataField="MessageToManager" HeaderText="Comunicazioni da richiedente"  SortExpression="MessageToManager" />
                                    <asp:BoundField DataField="Employee2.UniqueName" HeaderText="Approvata da" ItemStyle-Font-Bold="true"  SortExpression="Employee2.UniqueName"  />
                                    <asp:BoundField DataField="MessageToApplicant" HeaderText="Comunicazioni da responsabile" SortExpression="MessageToApplicant"  />
                                </Columns>
                                <PagerTemplate>
                                    <asp:GridViewPager ID="Pager1" runat="server"  />
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
