<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="QuotationFind.aspx.cs"
    Inherits="LabExtim.QuotationFind" %>

<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        var JSON = JSON || {};

        $(document).ready(function () {


            function EndRequestHandler() {
                SetAutoComplete();
            }

            function SetAutoComplete() {

                $("#ctl00_ContentPlaceHolder1_DetailsView1_txtSearchCli").autocomplete({
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
                        $("#ctl00_ContentPlaceHolder1_DetailsView1_txtSearchCli").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_DetailsView1_hidSearchCli").val(ui.item.value);
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
    <h2>Trova Bozza / Preventivo</h2>
    <%-- <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="Elenco degli errori di validazione" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1"
                Display="None" />
            <asp:Panel ID="DetailsPanel" runat="server">
                <asp:DetailsView ID="DetailsView1" runat="server" DataSourceID="DetailsDataSource"
                    AutoGenerateRows="false" DefaultMode="Insert"
                    OnItemCommand="DetailsView1_ItemCommand" OnItemInserted="DetailsView1_ItemInserted"
                    CssClass="detailstable" FieldHeaderStyle-CssClass="bold" OnItemInserting="DetailsView1_ItemInserting"
                    OnDataBound="DetailsView1_DataBound">
                    <FieldHeaderStyle CssClass="bold" />
                    <RowStyle CssClass="selected" />
                    <Fields>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblId" runat="server" Text="Id"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_ID" runat="server" DataField="ID_Quotation" UIHint="Integer_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblNumero" runat="server" Text="Numero"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Number" runat="server" DataField="Number" UIHint="YearCounterText_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblManager" runat="server" Text="Gestione"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Manager" runat="server" DataField="Manager" UIHint="ForeignKey_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblCustomerCode" runat="server" Text="Cliente"></asp:Label>
                            </HeaderTemplate>
                            <%-- <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_CustomerCode" runat="server" DataField="Customer"
                                    UIHint="Autocomplete_Edit" />
                            </ItemTemplate>--%>
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtSearchCli" MaxLength="255" Width="200" ToolTip="Digitare almeno 3 caratteri per avviare la ricerca" CssClass="droplist" />
                                <asp:HiddenField ID="hidSearchCli" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblOwner" runat="server" Text="Utente"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Owner" runat="server" DataField="Employee" UIHint="ForeignKey_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblDescrizione" runat="server" Text="Titolo contiene..."></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Subject" runat="server" DataField="Subject" UIHint="Find_Text_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblStartDate" runat="server" Text="Da..."></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_StartDate" DataField="StartDate" runat="server"
                                    UIHint="Find_DateTime_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text="a.."></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_EndDate" DataField="EndDate" runat="server" UIHint="Find_DateTime_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblIdOdP" runat="server" Text="Id OdP"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_IDOdP" runat="server" DataField="ID_ProductionOrder"
                                    UIHint="Integer_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblNumeroOdP" runat="server" Text="Numero OdP"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_NumberOdP" runat="server" DataField="Number_ProductionOrder" UIHint="YearCounterText_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Link" InsertText="Cerca" ShowInsertButton="true" ShowCancelButton="true" CancelText="Reset" ControlStyle-CssClass="default" />

                    </Fields>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:DetailsView>
                <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableInsert="true" TableName="Find_Quotations"
                    ContextTypeName="DLLabExtim.QuotationDataContext">
                </asp:LinqDataSource>
                <%--<asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdDetails"
                Display="None" />--%>
                <%--<asp:FilterRepeater ID="FilterRepeater" runat="server" >
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("DisplayName") %>' AssociatedControlID="DynamicFilter$DropDownList1" />
                    <asp:DynamicFilter runat="server" ID="DynamicFilter" OnSelectedIndexChanged="OnFilterSelectedIndexChanged" />
                </ItemTemplate>
                <FooterTemplate><br /><br /></FooterTemplate>
            </asp:FilterRepeater>--%>
                <table cellpadding="5">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rdlQuotationType" runat="server" ValidationGroup="QuotationType"
                                RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdlQuotationType_SelectedIndexChanged">
                                <asp:ListItem Selected="True">Preventivi</asp:ListItem>
                                <asp:ListItem Selected="False">Bozze</asp:ListItem>
                                <asp:ListItem Selected="False">Tutti</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>&nbsp
                        <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="true" DataTextField="Key"
                            DataValueField="Value" CssClass="droplist" OnSelectedIndexChanged="PersistSelection"
                            OnDataBound="ddlOrderBy_DataBound">
                        </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <h2>Trovati</h2>
                <asp:GridView ID="grdDetails" runat="server" AutoGenerateSelectButton="False" AutoGenerateEditButton="false"
                    AutoGenerateDeleteButton="false" AllowPaging="True" AllowSorting="True" DataSourceID="GridDataSource"
                    CssClass="gridview" OnRowCommand="grdDetails_RowCommand" AutoGenerateColumns="false"
                    PagerSettings-Position="Top" OnPreRender="grdDetails_PreRender" OnRowDataBound="grdDetails_RowDataBound"
                    OnPageIndexChanging="grdDetails_PageIndexChanging">
                    <PagerStyle CssClass="footer" />
                    <%--<SelectedRowStyle CssClass="selected" />--%>
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altRow" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--<asp:HyperLink ID="SelectHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.QuotationConsole, GetDataItem()) %>'
                                         Text="Seleziona" />--%>
                                <%--<asp:HyperLink ID="SelectHyperLink" runat="server" Text="Seleziona" NavigateUrl='<%# string.Format("{2}?{0}={1}", QuotationKey, DataBinder.Eval(Container.DataItem, "ID")  , QuotationConsolePage) %>'/> --%>
                                <asp:LinkButton ID="lbtSelectTemp" runat="server" Text="Apri" CommandName="SelectTemp"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") + "|" + DataBinder.Eval(Container.DataItem, "Subject") %>'></asp:LinkButton>
                                <%--<asp:LinkButton ID="lbtSelect" runat="server" Text="Seleziona" CommandName="Select"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") + "|" + DataBinder.Eval(Container.DataItem, "Subject")%>'></asp:LinkButton>--%>
                                <asp:LinkButton ID="lbtDelete" runat="server" Text="Elimina" CommandName="Delete"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") + "|" + DataBinder.Eval(Container.DataItem, "Subject") %>'
                                    OnClientClick='return (confirm("Confermi la cancellazione di questo preventivo?") && confirm("I dati del preventivo e le relative voci di dettaglio non saranno più recuperabili, confermi?"));'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblId" runat="server" Text="Id"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_ID" runat="server" DataField="ID" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblNumero" runat="server" Text="Numero"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Numebr" runat="server" DataField="Number" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblDraft" runat="server" Text="Bozza"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Draft" runat="server" DataField="Draft" UIHint="Boolean" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblDescrizione" runat="server" Text="Titolo"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Subject" runat="server" DataField="Subject" UIHint="Text250" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblCustomerCode" runat="server" Text="Cliente"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_CustomerCode" runat="server" DataField="Customer"
                                    UIHint="ForeignKey" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblOwner" runat="server" Text="Utente"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Owner" runat="server" DataField="Employee1" UIHint="ForeignKey" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblDate" runat="server" Text="Data creazione"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Date" DataField="Date" runat="server" UIHint="DateTime" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        Nessuna voce trovata.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" EnableUpdate="false"
                    EnableInsert="false" ContextTypeName="DLLabExtim.QuotationDataContext" TableName="Quotations"
                    OnSelecting="GridDataSource_Selecting" AutoGenerateOrderByClause="true" AutoGenerateWhereClause="false">
                    <%--<WhereParameters>
                    <asp:DynamicControlParameter ControlID="FilterRepeater" />
                </WhereParameters>--%>
                    <%--<OrderByParameters>
                    <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                        DefaultValue="" />
                </OrderByParameters>--%>
                </asp:LinqDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
