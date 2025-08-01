<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductionOrderPopup.aspx.cs"
    Inherits="LabExtim.ProductionOrderPopup" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="~/Site.css" rel="stylesheet" type="text/css" />
    <script src="Site.js" type="text/javascript"></script>
</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 10px; background-color: #FBFBFB; color: navy">

    <script type="text/javascript">

        function SetParametersAndOpenPopUp() {

            OpenItem('DeliveryTripPopup.aspx?DTid=-1');
            return false;

        }

    </script>

    <form id="form1" runat="server">
        <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true"></asp:ToolkitScriptManager>
        <h4>Ordine di Produzione<asp:Label ID="lblItemNo" runat="server" />
        </h4>
        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
        <table>
            <%--<tr>
                <td>
                    <asp:LinkButton ID="lbtShowHide" runat="server" CssClass="gridview" Font-Bold="False"
                        Text="Nascondi/visualizza testata" OnClick="lbtShowHide_Click" />
                    &nbsp;
                    <asp:LinkButton ID="lblShowHideMP" runat="server" CssClass="gridview" Font-Bold="False"
                        Text="Nascondi/visualizza griglia pianificazione di dettaglio" OnClick="lblShowHideMP_Click" />
                    <br />
                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                </td>

            </tr>--%>
            <tr>
                <td>
                    <table id="tblTestata" runat="server">
                        <tr>
                            <td valign="top">
                                <h4>Sequenza fasi di lavorazione</h4>

                                <asp:GridView ID="grdVW_ProductionExtMPS_GroupedByPhasesNew" runat="server" AutoGenerateColumns="False" Visible="true"
                                    CssClass="gridview" OnRowDataBound="grdVW_ProductionExtMPS_GroupedByPhasesNew_RowDataBound" OnRowCommand="grdVW_ProductionExtMPS_GroupedByPhasesNew_RowCommand">
                                    <RowStyle CssClass="row" />
                                    <%--<AlternatingRowStyle CssClass="altRow" />--%>
                                    <Columns>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Modifica" OnClick="hypEdit_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--<asp:BoundField HeaderText="Macchina" DataField="pmDescription" />--%>

                                        <asp:TemplateField HeaderText="Macchina">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidID1" runat="server" Value='<%# Eval("ID") %>' />
                                                <asp:Label ID="lblProductionMachine" runat="server" Text='<%# Eval("pmDescription") %>' />
                                                <asp:DropDownList ID="ddlProductionMachines" CssClass="droplist" runat="server" OnSelectedIndexChanged="ddlProductionMachines_SelectedIndexChanged" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Previsto (h)" DataField="prodTime" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Effettivo (h)" DataField="prodEffTime" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Image ID="imgSemaphore" runat="server"
                                                    ImageUrl='<%# Eval("SemaphoreImage", "~/Images/{0}.gif") %>'
                                                    AlternateText='<%# Bind("SemaphoreTitle") %>'
                                                    ToolTip='<%# Bind("SemaphoreTitle") %>' />
                                                <asp:Label ID="lblSemaphore" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stato">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidID2" runat="server" Value='<%# Eval("ID") %>' />
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("mpstDescription") %>' />
                                                <asp:DropDownList ID="ddlStatuses" runat="server" CssClass="droplist" OnSelectedIndexChanged="ddlStatuses_SelectedIndexChanged" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Dato OPERATORE Copie OK" DataField="OkCopiesCount" ItemStyle-HorizontalAlign="Right" />


                                        <%--<asp:BoundField ItemStyle-BackColor="WhiteSmoke" HeaderText="Dato MACCHINA Copie richieste" DataField="DatiMacchinaCopieRichieste" ItemStyle-HorizontalAlign="Right"/>--%>
                                        <asp:BoundField ItemStyle-BackColor="WhiteSmoke" HeaderText="Dato MACCHINA Copie lavorate" DataField="DatiMacchinaCopieLavorate" ItemStyle-HorizontalAlign="Right" />

                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtAdvance" runat="server" CommandName="Advance" CommandArgument='<%# Eval("ID").ToString() %>'
                                                    ImageUrl="~/images/arrow_up.png" ToolTip="Aumenta priorità"></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtRetard" runat="server" CommandName="Retard" CommandArgument='<%# Eval("ID").ToString() %>'
                                                    ImageUrl="~/images/arrow_down.png" ToolTip="Diminuisci priorità"></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Esecuzione" DataField="ExternalCompanyDescription" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Bold="true" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        Nessuna voce trovata.
                                    </EmptyDataTemplate>
                                </asp:GridView>


                                <%-- <asp:LinqDataSource ID="ldsProductionMPS" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    TableName="ProductionMPs" EnableUpdate="True" EnableDelete="false" EnableInsert="false" OnSelecting="ldsProductionMPS_Selecting" OnUpdating="ldsProductionMPS_Updating">
                                </asp:LinqDataSource>

                                <asp:GridView ID="grdProductionMPS" runat="server" Visible="false" AutoGenerateColumns="False" DataSourceID="ldsProductionMPS"
                                    DataKeyNames="ID" CssClass="gridview" AutoGenerateEditButton="true" OnRowCommand="grdProductionMPS_RowCommand">
                                    <RowStyle CssClass="row" />
                                    <AlternatingRowStyle CssClass="altRow" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblMacroItem" runat="server" Text="Macrovoce"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycMacroItem" runat="server" DataField="MacroItem" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblMachine" runat="server" Text="Macchina"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycProductionMachine" runat="server" DataField="ProductionMachine" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycProductionMachine" runat="server" DataField="ProductionMachine" UIHint="ForeignKey_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Durata (min.)"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycProdTimeMin" runat="server" DataField="ProdTimeMin" UIHint="Text" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Fine"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycProdEnd" runat="server" DataField="ProdEnd" UIHint="Text" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text="Stato"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycStatus" runat="server" DataField="Statuse" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycStatus" runat="server" DataField="Statuse" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtAdvance" runat="server" CommandName="Advance" CommandArgument='<%# Eval("ID").ToString() %>'
                                                    ImageUrl="~/images/arrow_up.png" ToolTip="Aumenta priorità"></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtRetard" runat="server" CommandName="Retard" CommandArgument='<%# Eval("ID").ToString() %>'
                                                    ImageUrl="~/images/arrow_down.png" ToolTip="Diminuisci priorità"></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerStyle CssClass="footer" />
                                    <PagerTemplate>
                                        <asp:GridViewPager ID="Pager1" runat="server" />
                                    </PagerTemplate>
                                    <EmptyDataTemplate>
                                        Nessuna voce trovata.
                                    </EmptyDataTemplate>
                                </asp:GridView>--%>

                            </td>

                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblQtaProdBOBST" runat="server" ForeColor="DarkGreen"></asp:Label>&nbsp;<asp:ImageButton ID="ibtQtaProdBOBST" Visible="false" runat="server" ImageUrl="~/Images/arrow_refresh.png" ToolTip="Aggiorna" OnClick="ibtQtaProdBOBST_Click" />
                                <h4>Dati Odp    </h4>
                                <asp:DetailsView ID="dtvProductionOrder" runat="server" DataSourceID="DetailsDataSource"
                                    AutoGenerateEditButton="true" AutoGenerateInsertButton="true" OnModeChanging="OnDetailsViewModeChanging"
                                    OnPreRender="OnDetailsViewPreRender" OnItemUpdated="OnDetailsViewItemUpdated"
                                    OnItemInserted="OnDetailsViewItemInserted" AutoGenerateRows="false" OnItemCommand="dtvProductionOrder_ItemCommand"
                                    DataKeyNames="ID" OnItemCreated="dtvProductionOrder_ItemCreated" OnItemUpdating="dtvProductionOrder_ItemUpdating"
                                    OnItemInserting="dtvProductionOrder_ItemInserting">
                                    <FieldHeaderStyle Font-Bold="true" />
                                    <RowStyle CssClass="selected" />
                                    <FooterStyle CssClass="selected" />
                                    <Fields>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblID" runat="server" Text="ID OdP"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNumber" runat="server" Text="Numero OdP"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycNumber" runat="server" DataField="Number" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblIDQuotation" runat="server" Text="ID preventivo"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycIDQuotation" runat="server" DataField="ID_Quotation" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycIDQuotation" runat="server" DataField="ID_Quotation" UIHint="Text_Edit" Mode="ReadOnly" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycIDQuotation" runat="server" DataField="ID_Quotation" UIHint="Text_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCompany" runat="server" Text="Azienda"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit" Mode="Edit" AllowNullValue="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit" Mode="Edit" AllowNullValue="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblManager" runat="server" Text="Gestione"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey" CssClass="bold red" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit" Mode="Edit" />
                                                <%--<asp:ImageButton ID="ibtFind" runat="server" ImageUrl="~/Images/find.png" ToolTip="Seleziona preventivi Cliente"
                                                ImageAlign="Middle" />--%>
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text="Descrizione"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="MultilineText_Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="MultilineText_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStartDate" runat="server" Text="Data lancio"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime"
                                                    DataFormatString="{0:d}" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime_EditRO" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime_EditRO" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Text="Quantità"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Integer_Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycQuantity" runat="server" DataField="Quantity" UIHint="Integer_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDeliveryDate" runat="server" Text="Data consegna"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate"
                                                    UIHint="DateTime" DataFormatString="{0:d}" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate"
                                                    UIHint="DateTime_Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycDeliveryDate" runat="server" DataField="DeliveryDate"
                                                    UIHint="DateTime_Future_Edit" />
                                                (Da impostare superiore ad oggi)
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStatuse" runat="server" Text="Stato"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNote" runat="server" Text="Descrizione lavorazione"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="MultilineText"
                                                    HtmlEncode="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="MultilineText_Edit"
                                                    HtmlEncode="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="MultilineText_Edit"
                                                    HtmlEncode="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNoteFromProduction" runat="server" Text="Note tecniche da OdP precedenti"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycNoteFromProduction" runat="server" DataField="NoteFromProduction" UIHint="MultilineText"
                                                    HtmlEncode="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNonConformity" runat="server" Text="Non conformità"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycNonConformity" runat="server" DataField="NonConformity" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycNonConformity" runat="server" DataField="NonConformity" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycNonConformity" runat="server" DataField="NonConformity" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblComplaintReceived" runat="server" Text="Ricevuto reclamo"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycComplaintReceived" runat="server" DataField="ComplaintReceived" UIHint="YesNo_Edit" ReadOnly="true" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycComplaintReceived" runat="server" DataField="ComplaintReceived" UIHint="YesNo_Edit"  AllowNullValue="false"
                                                    ReadOnly="false"  />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycComplaintReceived" runat="server" DataField="ComplaintReceived" UIHint="YesNo_Edit"  AllowNullValue="true"  ReadOnly="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCorrectiveAction" runat="server" Text="Azione correttiva"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycCorrectiveAction" runat="server" DataField="CorrectiveAction" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycCorrectiveAction" runat="server" DataField="CorrectiveAction" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCorrectiveAction" runat="server" DataField="CorrectiveAction" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAccountNoteFromProduction" runat="server" Text="Segnalazioni da produzione"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycAccountNoteFromProduction" runat="server" DataField="AccountNoteFromProduction" UIHint="MultilineText"
                                                    HtmlEncode="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAccountNote" runat="server" Text="Note amministrative"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycAccountNote" runat="server" DataField="AccountNote" UIHint="MultilineText"
                                                    HtmlEncode="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycAccountNote" runat="server" DataField="AccountNote" UIHint="MultilineText_Edit"
                                                    HtmlEncode="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycAccountNote" runat="server" DataField="AccountNote" UIHint="MultilineText_Edit"
                                                    HtmlEncode="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNote1" runat="server" Text="Appunti"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycNote1" runat="server" DataField="Note1" UIHint="MultilineText"
                                                    HtmlEncode="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycNote1" runat="server" DataField="Note1" UIHint="MultilineText_Edit"
                                                    HtmlEncode="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycNote1" runat="server" DataField="Note1" UIHint="MultilineText_Edit"
                                                    HtmlEncode="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblContractor" runat="server" Text="Capo commessa"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycContractor" runat="server" DataField="Employee" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycContractor" runat="server" DataField="Employee" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycContractor" runat="server" DataField="Employee" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                    <FooterTemplate>
                                        <%--<asp:LinkButton ID="lbtDeactivate" runat="server" CssClass="selected" Font-Bold="False"
                                    Font-Size="X-Small" Text="Disattiva voce" CommandName="Deactivate" Enabled="false"></asp:LinkButton>
                                <asp:LinkButton ID="lbtActivate" runat="server" CssClass="selected" Font-Bold="False"
                                    Font-Size="X-Small" Text="Riattiva voce" CommandName="Activate" Enabled="false"></asp:LinkButton>--%>
                                    </FooterTemplate>
                                </asp:DetailsView>
                                <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="DetailsDataSource_Inserting"
                                    TableName="ProductionOrders" OnSelecting="DetailsDataSource_Selecting" OnInserted="DetailsDataSource_Inserted"
                                    OnUpdating="DetailsDataSource_Updating" OnUpdated="DetailsDataSource_Updated">
                                </asp:LinqDataSource>
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label1" runat="server" ForeColor="DarkGreen"></asp:Label>&nbsp;<asp:ImageButton ID="ImageButton1" Visible="false" runat="server" ImageUrl="~/Images/arrow_refresh.png" ToolTip="Aggiorna" OnClick="ibtQtaProdBOBST_Click" />
                                <h4>Dati formato plastificazione trasmessi alla macchina</h4>
                                <asp:DetailsView ID="dtvPlasticCoating" runat="server" DataSourceID="ldsPlasticCoating" AutoGenerateRows="false"
                                    AutoGenerateEditButton="true" OnItemUpdated="dtvPlasticCoating_ItemUpdated" DataKeyNames="Id_ProductionOrder">
                                    <FieldHeaderStyle Font-Bold="true" />
                                    <RowStyle CssClass="selected" />
                                    <FooterStyle CssClass="selected" />
                                    <Fields>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblIDProductionOrder" runat="server" Text="ID OdP"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycID" runat="server" DataField="Id_ProductionOrder" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblLATI" runat="server" Text="Lati (no.)"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycLATI" runat="server" DataField="LATI" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycLATI" runat="server" DataField="LATI" UIHint="Integer_Edit" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblBASE_1" runat="server" Text="Base (mm.)"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycBASE_1" runat="server" DataField="BASE_1" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycBASE_1" runat="server" DataField="BASE_1" UIHint="Integer_Edit" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblALTEZZA_1" runat="server" Text="Altezza (mm.)"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycALTEZZA_1" runat="server" DataField="ALTEZZA_1" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycALTEZZA_1" runat="server" DataField="ALTEZZA_1" UIHint="Integer_Edit" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                    </Fields>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:DetailsView>
                                <asp:LinqDataSource ID="ldsPlasticCoating" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    EnableDelete="false" EnableInsert="false" EnableUpdate="true" AutoGenerateWhereClause="true"
                                    TableName="PlasticCoatingMachineParameters" OnSelecting="ldsPlasticCoating_Selecting">
                                </asp:LinqDataSource>
                            </td>

                        </tr>
                    </table>
                </td>


            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSuspend" runat="server" CssClass="myButton" Font-Bold="False" ForeColor="Red"
                        OnClientClick="javascript:return confirm('Confermi la sospensione di questo OdP?');"
                        Text="Sospendi" OnClick="btnSuspend_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnPortaInCorso" runat="server" CssClass="myButton" Font-Bold="False" ForeColor="Green"
                                OnClientClick="javascript:return confirm('Confermi di voler portare \'In corso\' questo OdP?');"
                                Text="Porta in corso" OnClick="btnPortaInCorso_Click" />&nbsp;&nbsp;
                     <asp:Button ID="btnPortainAttesa" runat="server" CssClass="myButton" Font-Bold="False" ForeColor="Brown"
                         OnClientClick="javascript:return confirm('Confermi di voler portare \'In attesa\' questo OdP?');"
                         Text="Porta in attesa" OnClick="btnPortainAttesa_Click" />
                    <asp:Button ID="btnResetSchedule" runat="server" CssClass="myButton" Font-Bold="False"
                        OnClientClick="javascript:return confirm('Confermi il ripristino completo della pianificazione di questo OdP?');"
                        Text="Reset pianificazione" OnClick="btnResetSchedule_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnResetPrioritySchedule" runat="server" CssClass="myButton" Font-Bold="False"
                        OnClientClick="javascript:return confirm('Confermi il ripristino completo delle macchine modificate, delle priorità di fase e della pianificazione di questo OdP?');"
                        Text="Reset macchine, priorità e pianificazione" OnClick="btnResetPrioritySchedule_Click" />
                    
                    <br />
                    <%--<asp:Button ID="lbtCloneProductionOrder" runat="server" CssClass="myButton" Font-Bold="False"
                                OnClientClick="javascript:return confirm('Vuoi creare un nuovo ordine di produzione uguale a quello corrente?');"
                                Text="Clona" OnClick="lbtCloneProductionOrder_Click" />&nbsp;&nbsp;--%>
                    <asp:Button ID="lbtPrintProductionOrder" runat="server" CssClass="myButton" Font-Bold="False"
                        OnClientClick="javascript:return confirm('Vuoi stampare l\'ordine di produzione corrente o creato?');"
                        Text="Stampa" OnClick="lbtPrintProductionOrder_Click" />

                    <%-- <asp:Button ID="lbtClose" runat="server" CssClass="myButton" Font-Bold="False"
                                OnClientClick="javascript:window.close();" Text="Chiudi finestra" />
                            <br />--%>
                    
                </td>



            </tr>
            <tr>
                <td>

                    <asp:LinkButton ID="lbtShowHideTechSpecs" runat="server" CssClass="gridview" Font-Bold="False"
                        Text="Nascondi/visualizza gestione note tecniche OdP" OnClick="lbtShowHideTechSpecs_Click" />
                    <br />
                    <asp:Panel runat="server" ID="pnlTechSpecs" Visible="false">

                        <%--<h4>Gestione Storico note tecniche preventivo e Note tecniche OdP</h4>
                        <asp:DetailsView ID="dtvPriceCom" runat="server" DataSourceID="ldsPriceCom" AutoGenerateEditButton="true" DataKeyNames="ID" AutoGenerateRows="false">
                            <FieldHeaderStyle Font-Bold="true" />
                            <RowStyle CssClass="selected" />
                            <FooterStyle CssClass="selected" />
                            <Fields>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblPriceCom" runat="server" Text="Storico note tecniche preventivo"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycPriceCom" runat="server" DataField="PriceCom" UIHint="MultilineText"
                                            HtmlEncode="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycPriceCom" runat="server" DataField="PriceCom" UIHint="MultilineText_Edit"
                                            HtmlEncode="false" />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DynamicControl ID="dycPriceCom" runat="server" DataField="PriceCom" UIHint="MultilineText_Edit"
                                            HtmlEncode="false" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:DetailsView>
                        <asp:LinqDataSource ID="ldsPriceCom" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            EnableDelete="false" EnableInsert="false" EnableUpdate="true" TableName="Quotations" OnSelecting="ldsPriceCom_Selecting">
                        </asp:LinqDataSource>--%>

                        <h4>Note tecniche OdP</h4>
                        <asp:LinqDataSource ID="ldsProductionOrderTechSpecs" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ProductionOrderTechSpecs" OnSelecting="ldsProductionOrderTechSpecs_Selecting" AutoGenerateWhereClause="false" EnableUpdate="true">
                        </asp:LinqDataSource>

                        <asp:GridView ID="grdProductionOrderTechSpecs" runat="server" CssClass="gridview" AutoGenerateEditButton="true" DataSourceID="ldsProductionOrderTechSpecs" AutoGenerateColumns="false" DataKeyNames="ID">
                            <Columns>
                                <asp:DynamicField UIHint="Text" DataField="ID" HeaderText="ID" Visible="false" />
                                <asp:DynamicField UIHint="Text" DataField="ProductionDate" HeaderText="Data" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:DynamicField UIHint="ForeignKey" DataField="Employee" HeaderText="Operatore" ReadOnly="true" />
                                <asp:DynamicField UIHint="ForeignKey" DataField="PickingItem" HeaderText="Fase" ReadOnly="true" />
                                <asp:DynamicField UIHint="Text" DataField="CodiceMarcaFilm" HeaderText="Codice/marca film" />
                                <%--<asp:DynamicField UIHint="Text" DataField="ClicheReso" HeaderText="Cliche reso" ReadOnly="true"/>
                                <asp:DynamicField UIHint="Text" DataField="ClicheCondizioni" HeaderText="Condizioni" ReadOnly="true"/>--%>
                                <asp:DynamicField UIHint="ForeignKey" DataField="ToolsReturnedTo" HeaderText="Cliche reso" ReadOnly="false" />
                                <asp:DynamicField UIHint="ForeignKey" DataField="ToolsCondition" HeaderText="Condizioni" ReadOnly="false" />
                                <asp:DynamicField UIHint="Text" DataField="StampaTemperatura" HeaderText="Temp. stampa" />
                                <asp:DynamicField UIHint="Text" DataField="AltreInfo" HeaderText="Altre info" />
                                <asp:DynamicField UIHint="Text" DataField="CodiceMarcaInchiostro" HeaderText="Codice/marca inchiostro" />
                                <asp:DynamicField UIHint="Boolean" DataField="Ricetta" HeaderText="Ricetta" />
                                <asp:DynamicField UIHint="Text" DataField="TelaioNumeroFili" HeaderText="Numero fili telaio" />
                                <asp:DynamicField UIHint="Text" DataField="GelatinaSpessore" HeaderText="Spess. gelatina" />
                                <asp:DynamicField UIHint="Text" DataField="RaclaInclinazione" HeaderText="Incl. racla" />
                                <asp:DynamicField UIHint="Text" DataField="RaclaDurezzaSpigolo" HeaderText="Durezza spigolo racla" />
                                <%--<asp:DynamicField UIHint="Text" DataField="FustellaResa" HeaderText="Fustella resa" ReadOnly="true"/>
                                <asp:DynamicField UIHint="Text" DataField="FustellaCondizioni" HeaderText="Condizioni" ReadOnly="true"/>--%>
                                <asp:DynamicField UIHint="ForeignKey" DataField="ToolsReturnedTo1" HeaderText="Fustella resa" ReadOnly="false" />
                                <asp:DynamicField UIHint="ForeignKey" DataField="ToolsCondition1" HeaderText="Condizioni" ReadOnly="false" />
                                <asp:DynamicField UIHint="Text" DataField="ControCordonatori" HeaderText="Contro-cordonatori utilizzati" />
                                <asp:DynamicField UIHint="Text" DataField="AltreNoteDaProduzione" HeaderText="Altre note tecniche" />
                                <asp:DynamicField UIHint="ForeignKey" DataField="Statuse" HeaderText="Stato" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>

                    <asp:LinkButton ID="lbtShowHideDet" runat="server" CssClass="gridview" Font-Bold="False"
                        Text="Nascondi/visualizza dettaglio" OnClick="lbtShowHideDet_Click" />
                    <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                        <h4>
                            <asp:Label ID="lblDetails" runat="server" Text="Dettaglio denunce produzione" />
                        </h4>
                        <asp:LinqDataSource ID="ldsProductionOrderDetails" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ProductionOrderDetails" OnSelecting="ldsProductionOrderDetails_Selecting"
                            AutoGenerateWhereClause="true" EnableUpdate="true" EnableDelete="true">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="dtvProductionOrder" Name="ID_ProductionOrder" Type="Int32"
                                    PropertyName="SelectedValue" DefaultValue="" />
                            </WhereParameters>

                        </asp:LinqDataSource>
                        <asp:GridView ID="grdProductionOrderDetails" runat="server" AllowPaging="True" CssClass="gridview"
                            DataSourceID="ldsProductionOrderDetails" OnPageIndexChanging="grdProductionOrderDetails_PageIndexChanging"
                            AutoGenerateColumns="false" AutoGenerateEditButton="true" OnRowDataBound="grdProductionOrderDetails_RowDataBound"
                            OnRowUpdated="grdProductionOrderDetails_RowUpdated">
                            <RowStyle CssClass="row" />
                            <AlternatingRowStyle CssClass="altRow" />

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" CausesValidation="false"
                                            Text="Cancella" OnClientClick='return confirm("Confermi la cancellazione di questa voce?");' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblIDQuotationDetail" runat="server" Text="ID Voce preventivo"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycIDQuotationDetail" runat="server" DataField="ID_QuotationDetail"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOwner" runat="server" Text="Operatore"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycOwner" runat="server" DataField="Employee" UIHint="ForeignKey" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycOwner" runat="server" DataField="Employee" UIHint="ForeignKey_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblPhase" runat="server" Text="Fase"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycPhase" runat="server" DataField="PickingItem" UIHint="ForeignKey" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycPhase" runat="server" DataField="PickingItem" UIHint="ForeignKey_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblProductionDate" runat="server" Text="Data produzione" Width="90"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycProductionDate" runat="server" DataField="ProductionDate"
                                            UIHint="DateTime" DataFormatString="{0:d}" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycProductionDate" runat="server" DataField="ProductionDate"
                                            UIHint="DateTime_Edit" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblProductionTime" runat="server" Text="Tempo produzione (hh.mm.ss)"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycProductionTime" runat="server" DataField="ProductionTime"
                                            UIHint="TicksToTime" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycProductionTime" runat="server" DataField="ProductionTime"
                                            UIHint="TimeToTicks" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRawMaterial" runat="server" Text="Prodotto di Base"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterial" runat="server" DataField="PickingItemDesc" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblSupplier" runat="server" Text="Fornitore"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycSupplier" runat="server" DataField="Supplier" UIHint="ForeignKey" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycSupplier" runat="server" DataField="Supplier" UIHint="ForeignKey_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUMRawMaterial" runat="server" Text="UM Mat Base"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycUMRawMaterial" runat="server" DataField="UMRawMaterialDesc" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycUMRawMaterial" runat="server" DataField="Unit1" UIHint="ForeignKey_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRawMaterialUArea" runat="server" Text="Area Uni"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialUArea" runat="server" DataField="RawMaterialUArea"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRawMaterialQuantity" runat="server" Text="Q.tà Mat Base"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialQuantity" runat="server" DataField="RawMaterialQuantity"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialQuantity" runat="server" DataField="RawMaterialQuantity"
                                            UIHint="Decimal_Edit" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRawMaterialSup" runat="server" Text="Prodotto supplementare"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialSup" runat="server" DataField="PickingItemSupDesc" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblSupplierSup" runat="server" Text="Fornitore Prod sup"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycSupplierSup" runat="server" DataField="Supplier1" UIHint="ForeignKey" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycSupplierSup" runat="server" DataField="Supplier1" UIHint="ForeignKey_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUMUser" runat="server" Text="UM Mat Sup"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycUMUser" runat="server" DataField="UMUserDesc" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycUMUser" runat="server" DataField="Unit2" UIHint="ForeignKey_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRawMaterialX" runat="server" Text="Q.tà Mat Sup"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialX" runat="server" DataField="RawMaterialX"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialX" runat="server" DataField="RawMaterialX"
                                            UIHint="Decimal_Edit" Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRawMaterialY" runat="server" Text="b (UMMS)"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialY" runat="server" DataField="RawMaterialY"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialY" runat="server" DataField="RawMaterialY"
                                            UIHint="Decimal_Edit" Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRawMaterialZ" runat="server" Text="h (UMMS)"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialZ" runat="server" DataField="RawMaterialZ"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycRawMaterialZ" runat="server" DataField="RawMaterialZ"
                                            UIHint="Decimal_Edit" Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblCostCalc" runat="server" Text="Costo calc €"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycCostCalc" runat="server" DataField="CostCalc" UIHint="Text"
                                            DataFormatString="{0:N2}" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblHistoryCostCalc" runat="server" Text="Costo storico calc €"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycHistoryCostCalc" runat="server" DataField="HistoryCostCalc"
                                            UIHint="Text" DataFormatString="{0:N2}" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuantityOver" runat="server" Text="Evaso"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycQuantityOver" runat="server" DataField="QuantityOver"
                                            UIHint="Boolean" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycQuantityOver" runat="server" DataField="QuantityOver"
                                            UIHint="Boolean_Edit" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblCost" runat="server" Text="Costo"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Text" DataFormatString="{0:N2}" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDirectSupply" runat="server" Text="Acquistato"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycDirectSupply" runat="server" DataField="DirectSupply"
                                            UIHint="Boolean" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycDirectSupply" runat="server" DataField="DirectSupply"
                                            UIHint="Boolean_Edit" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle BackColor="SeaShell" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblFreeType" runat="server" Text="Tipo (voce libera)"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycFreeType" runat="server" DataField="Type" UIHint="ForeignKey" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycFreeType" runat="server" DataField="Type" UIHint="ForeignKey_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle BackColor="SeaShell" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblItemFreeType" runat="server" Text="Tipo prodotto (voce libera)"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycItemFreeType" runat="server" DataField="ItemType" UIHint="ForeignKey" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycItemFreeType" runat="server" DataField="ItemType" UIHint="ForeignKey_Edit"
                                            Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle BackColor="SeaShell" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblFreeItemDescription" runat="server" Text="Descrizione (voce libera)"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycFreeItemDescription" runat="server" DataField="FreeItemDescription"
                                            UIHint="Text250" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycFreeItemDescription" runat="server" DataField="FreeItemDescription"
                                            UIHint="Text250_Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOkCopiesCount" runat="server" Text="Copie OK contate"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycOkCopiesCount" runat="server" DataField="OkCopiesCount"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycOkCopiesCount" runat="server" DataField="OkCopiesCount"
                                            UIHint="Decimal_Edit" Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblKoCopiesCount" runat="server" Text="Copie di scarto"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycKoCopiesCount" runat="server" DataField="KoCopiesCount"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycKoCopiesCount" runat="server" DataField="KoCopiesCount"
                                            UIHint="Decimal_Edit" Mode="Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblProducedQuantity" runat="server" Text="Q.tà prodotta"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycProducedQuantity" runat="server" DataField="ProducedQuantity"
                                            UIHint="Text" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycProducedQuantity" runat="server" DataField="ProducedQuantity"
                                            UIHint="Decimal_Edit_Edit" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblNote" runat="server" Text="Segnalazioni ad Amministrazione"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text250" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text250_Edit" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
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
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="lbtPrintPOFinalCost" runat="server" CssClass="myButton" Font-Bold="False"
                        OnClientClick="javascript:return confirm('Vuoi stampare il consuntivo costi dell\'ordine di produzione corrente?');"
                        Text="Stampa consuntivo" OnClick="lbtPrintPOFinalCost_Click" /></td>
            </tr>
            <%--<tr>
                <td>
                    <asp:LinkButton ID="lblShowHideTrips" runat="server" CssClass="gridview" Font-Bold="False"
                        Text="Nascondi/visualizza griglia viaggi" OnClick="lblShowHideTrips_Click" />
                    <br />
                    <asp:Panel runat="server" ID="pnlTrips" Visible="false">
                        <h4>Viaggio di arrivo associato</h4>
                        <asp:LinqDataSource ID="ldsDeliveryTrips" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="DeliveryTrips" OnSelecting="ldsDeliveryTrips_Selecting">
                        </asp:LinqDataSource>
                        <asp:Button ID="btnNewTrip" runat="server" Text="Crea nuovo viaggio" CssClass="myButton" Font-Bold="False" OnClientClick="javascript:return SetParametersAndOpenPopUp();" />

                        <asp:GridView ID="grdDeliveryTrips" runat="server" AutoGenerateColumns="False" DataSourceID="ldsDeliveryTrips"
                            DataKeyNames="ID" CssClass="gridview" OnRowCommand="grdDeliveryTrips_RowCommand" OnRowDataBound="grdDeliveryTrips_RowDataBound" OnPreRender="grdDeliveryTrips_PreRender">
                            <RowStyle CssClass="row" />
                            <AlternatingRowStyle CssClass="altRow" />
                            <SelectedRowStyle BackColor="Yellow" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ibtClose" runat="server" CommandName="Closed" CommandArgument='<%# Eval("ID").ToString() %>'
                                            Text="Chiudi" ToolTip="Chiudi viaggio" OnClientClick="javascript:return confirm('Sei sicuro di voler chiudere questo viaggio? (non sarà più disponibile per l\'associazione agli OdP)');"></asp:LinkButton>
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
                                <asp:BoundField DataField="StartDate" HeaderText="Data viaggio" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ibtSelect" runat="server" CommandName="Selected" CommandArgument='<%# Eval("ID").ToString() %>'
                                            Text="Seleziona" ToolTip="Seleziona viaggio di arrivo"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                Nessuna voce trovata.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>--%>
        </table>
    </form>
</body>
</html>
