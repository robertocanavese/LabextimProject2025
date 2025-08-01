<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="SuppliersConsole.aspx.cs" Inherits="LabExtim.SuppliersConsole" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox"
    TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/SearchEngine.ascx" TagName="SearchEngine" TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>
                Gestione Fornitori
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
            <%--<asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdSuppliers"
                Display="None" />--%>
            <asp:Panel ID="DetailsPanel" runat="server" >
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintSuppliers" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtPrintSuppliers_Click" Text="Stampa tabella" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsSuppliers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="Suppliers" OnSelected="ldsSuppliers_Selected" EnableUpdate="True"
                                AutoGenerateOrderByClause="true" EnableDelete="True" EnableInsert="True" OnSelecting="ldsSuppliers_Selecting">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdSuppliers" runat="server" AutoGenerateColumns="False"
                                AllowPaging="True" DataSourceID="ldsSuppliers" DataKeyNames="ID" CssClass="gridview"
                                OnPageIndexChanging="grdSuppliers_PageIndexChanging" OnDataBound="grdSuppliers_DataBound"
                                OnRowDataBound="grdSuppliers_RowDataBound" OnRowDeleted="grdSuppliers_RowDeleted"
                                OnRowCommand="grdSuppliers_RowCommand" OnPreRender="grdSuppliers_PreRender">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtUpdate" runat="server" ImageUrl="~/Images/arrow_refresh.png"
                                                ToolTip="Rileggi tabella" CommandName="Reload" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Gestione voce" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCode" runat="server" DataField="Code" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblName" runat="server" Text="Ragione Sociale"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycName" runat="server" DataField="Name" UIHint="Text250" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblMarkUp" runat="server" Text="Ricarico commerciale"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblMarkUp1" runat="server" Text='<%# Eval("SuppliersMarkUp.MarkUp") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblContact" runat="server" Text="Nome contatto"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycContact" runat="server" DataField="Contact" UIHint="Text250" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblEMail" runat="server" Text="EMail"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycEMail" runat="server" DataField="EMail" UIHint="Text250" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPhone" runat="server" Text="Telefono"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPhone" runat="server" DataField="Phone" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycFax" runat="server" DataField="Fax" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblStreet" runat="server" Text="Indirizzo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycStreet" runat="server" DataField="Street" UIHint="Text250" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCAP" runat="server" Text="CAP"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCAP" runat="server" DataField="CAP" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCity" runat="server" Text="Località"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCity" runat="server" DataField="City" UIHint="Text250" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProvince" runat="server" Text="Provincia"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycProvince" runat="server" DataField="Province" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblP_IVA" runat="server" Text="Partita IVA"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycP_IVA" runat="server" DataField="P_IVA" UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNote" runat="server" Text="Note"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text250" />
                                        </ItemTemplate>
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
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
