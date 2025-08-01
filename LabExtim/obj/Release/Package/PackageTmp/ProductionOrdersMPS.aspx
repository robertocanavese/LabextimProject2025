<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProductionOrdersMPS.aspx.cs" Inherits="LabExtim.ProductionOrdersMPS"
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
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Pianificazione produzione
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
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
                                TableName="VW_ProductionMPs" OnSelected="ldsProductionMPS_Selected" EnableUpdate="True"
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
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="hypRecalcHistoricalCost" runat="server" ImageUrl="~/Images/calculator.gif"
                                                ToolTip="Ricalcola costo storico" CommandName="RecalcHistoricalCost" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                OnClientClick="return confirm('Confermi il ricalcolo del costo storico di questo ordine di produzione? (i costi relativi alle voci saranno aggiornati alle tariffe correnti e non sarà più possibile ripristinare quelli vecchi!)');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="OdP">
                                      
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CloseLinkButton" runat="server" CommandName="Close" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IDProductionOrder") %>'
                                                CausesValidation="false" Text="Evadi" OnClientClick="return confirm('Confermi l\'evasione di questo ordine di produzione?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="OdP" DataField="IDProductionOrder" />
                                    <asp:BoundField HeaderText="OdP" DataField="poDescription" />
                                    <asp:BoundField HeaderText="Cliente" DataField="cuName" />
                                    <asp:BoundField HeaderText="Stato OdP" DataField="stDescription" />
                                    <asp:BoundField HeaderText="Voce t.base" DataField="ItemDescription" />
                                    <asp:BoundField HeaderText="Macrovoce" DataField="MacroItemDescription" />
                                    <asp:BoundField HeaderText="Macchina" DataField="pmDescription" />
                                    <asp:BoundField HeaderText="N.mac." DataField="NumProductionMachine" />
                                    <asp:BoundField HeaderText="Inizio fase" DataField="ProdStart" />
                                    <asp:BoundField HeaderText="Durata" DataField="ProdTime" />
                                    <asp:BoundField HeaderText="Fine fase" DataField="ProdEnd" />
                                    <asp:BoundField HeaderText="Stato fase" DataField="mpstDescription" />
                                      <asp:TemplateField HeaderText="Fase">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ClosePhaseLinkButton" runat="server" CommandName="ClosePhase" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IDProductionOrder") + "|" +  DataBinder.Eval(Container.DataItem, "IdQuotationDetail") %>'
                                                CausesValidation="false" Text="Chiudi" OnClientClick="return confirm('Confermi il completamento di questa lavorazione?');" />
                                         </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fase">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ReopenPhaseLinButton" runat="server" CommandName="ReopenPhase" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IDProductionOrder") + "|" +  DataBinder.Eval(Container.DataItem, "IdQuotationDetail") %>'
                                                CausesValidation="false" Text="Riapri" OnClientClick="return confirm('Confermi il reinserimento di questa lavorazione nella schedulazione?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="Quantità" DataField="Quantity" />
                                    <asp:BoundField HeaderText="Data consegna" DataField="DeliveryDate" DataFormatString="{0:dd/MM/yyyy}" />
 
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQuotation" runat="server" Text="Preventivo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtQuotation" runat="server" CommandName="GoToQuotation" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID_Quotation") %>'
                                                Text="Vai a preventivo" />
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
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
