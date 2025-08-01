<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="QuotationTemplateFind.aspx.cs"
    Inherits="LabExtim.QuotationTemplateFind" %>

<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <h2>Trova Modello di Preventivo</h2>
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
                    CssClass="detailstable" FieldHeaderStyle-CssClass="bold" OnItemInserting="DetailsView1_ItemInserting">
                    <FieldHeaderStyle CssClass="bold" />
                    <RowStyle CssClass="selected" />
                    <Fields>
                        <%--<asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblNumero" runat="server" Text="Numero"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_ID" runat="server" DataField="ID_QuotationTemplate" UIHint="Find_Text_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione contiene..."></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Description" runat="server" DataField="Description" UIHint="Find_Text_Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblType" runat="server" Text="Tipo"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey_Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey_Edit" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        
                        <asp:CommandField ButtonType="Link" InsertText="Cerca" ShowInsertButton="true" ShowCancelButton="true" CancelText="Reset" ControlStyle-CssClass="default" />
                    </Fields>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:DetailsView>
                <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableInsert="true" TableName="Find_QuotationTemplates"
                    ContextTypeName="DLLabExtim.QuotationDataContext" >
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
                <h2>Trovati</h2>
                <asp:GridView ID="grdDetails" runat="server" AutoGenerateSelectButton="False" AutoGenerateEditButton="false"
                    AutoGenerateDeleteButton="false" AllowPaging="True" AllowSorting="True" DataSourceID="GridDataSource"
                    CssClass="gridview" OnRowCommand="grdDetails_RowCommand" AutoGenerateColumns="false"
                    OnPreRender="grdDetails_PreRender"
                    OnRowDataBound="grdDetails_RowDataBound" >
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altRow" />
                    <PagerStyle CssClass="footer" />
                    <%--<SelectedRowStyle CssClass="selected" />--%>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--<asp:HyperLink ID="SelectHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.QuotationConsole, GetDataItem()) %>'
                                         Text="Seleziona" />--%>
                                <%--<asp:HyperLink ID="SelectHyperLink" runat="server" Text="Seleziona" NavigateUrl='<%# string.Format("{2}?{0}={1}", QuotationKey, DataBinder.Eval(Container.DataItem, "ID")  , QuotationConsolePage) %>'/> --%>
                                <asp:LinkButton ID="lbtSelect" runat="server" Text="Seleziona" CommandName="Select"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") + "|" + DataBinder.Eval(Container.DataItem, "Description") %>'></asp:LinkButton>
                                <asp:LinkButton ID="lbtDelete" runat="server" Text="Elimina" CommandName="Delete"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") + "|" + DataBinder.Eval(Container.DataItem, "Description") %>'
                                    OnClientClick='return (confirm("Confermi la cancellazione di questo modello di preventivo?") && confirm("I dati del modello di preventivo e le relative voci di dettaglio non saranno più recuperabili, confermi?"));'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblOrder" runat="server" Text="Ordine"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycOrder" runat="server" DataField="Order" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblType" runat="server" Text="Tipo"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey" />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPicker_Description" runat="server" DataField="Description" UIHint="Text250" />
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
                    EnableInsert="false" ContextTypeName="DLLabExtim.QuotationDataContext" TableName="QuotationTemplates" OnSelecting="GridDataSource_Selecting">
                    <%--<WhereParameters>
                    <asp:DynamicControlParameter ControlID="FilterRepeater" />
                </WhereParameters>--%>
                </asp:LinqDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
