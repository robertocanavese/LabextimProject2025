<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickingItemsPopup.aspx.cs" Inherits="LabExtim.PickingItemsPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%--<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="~/Site.css" rel="stylesheet" type="text/css" />
</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif">
    <form id="form1" runat="server">
    <div>
        <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
        <h2 style="font-family: Verdana, Geneva, Tahoma, sans-serif;">
            Preventivo No <%= QuotationHeader.Key %> (<%= QuotationHeader.Value %>) -> Picking list categoria
            <%--<%= MenuParameter %>--%></h2>
        <%--<%= table.DisplayName%>--%>
        <%--<asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true" EnablePartialRendering="true" />--%>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true"></asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                    HeaderText="Elenco degli errori di validazione" />
                <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1"
                    Display="None" />
                <%--<asp:FilterRepeater ID="FilterRepeater" runat="server">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("DisplayName") %>' AssociatedControlID="DynamicFilter$DropDownList1" />
                    <asp:DynamicFilter runat="server" ID="DynamicFilter" OnSelectedIndexChanged="OnFilterSelectedIndexChanged" />
                </ItemTemplate>
                <FooterTemplate>
                    <br />
                    <br />
                </FooterTemplate>
            </asp:FilterRepeater>--%>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True"  DataSourceID="GridDataSource"
                    AutoGenerateColumns="false" AllowSorting="True" CssClass="gridview" 
                    PageSize="15" onrowdatabound="GridView1_RowDataBound">
                    <Columns>
                    <%--DataSourceID="GridDataSource"--%>
                        <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="EditHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'
                                Text="Modifica" />&nbsp;<asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                                    CausesValidation="false" Text="Cancella" OnClientClick='return confirm("Confermi la cancellazione di questa voce?");' />&nbsp;<asp:HyperLink
                                        ID="DetailsHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Details, GetDataItem()) %>'
                                        Text="Dettaglio" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblInserted" runat="server" Text="Inserisci"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkInserted" runat="server" />
                                <%--<asp:DynamicControl ID="dycInserted" runat="server" Mode="Edit" DataField="Inserted"
                                    UIHint="Boolean_edit"    />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dynID" runat="server" DataField="ID" Visible="false" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblCompany" runat="server" Text="Azienda"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="CompanyDescription" UIHint="Text"   />
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
                                <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemTypeDescription" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblItemDescription" runat="server" Text="Descrizione voce"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="ItemDescription"
                                    UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblUM" runat="server" Text="UM"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycUM" runat="server" DataField="UnitDescription" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblCost" runat="server" Text="Cst Uni"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblMultiply" runat="server" Text="Molt"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblPercentage" runat="server" Text="% Ric std"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblSupplierName" runat="server" Text="Fornitore"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycSupplierName" runat="server" DataField="SupplierName" UIHint="Text" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="footer" />
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        <%--There are currently no items in this table.--%>
                        Nessuna voce trovata.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:LinqDataSource ID="GridDataSource" runat="server"  Tablename="SPPickingItems" ContextTypeName="DLLabExtim.QuotationDataContext"
                    onselecting="GridDataSource_Selecting"  >
                    <%--<WhereParameters>
                        <asp:DynamicControlParameter ControlId="FilterRepeater" />
                    </WhereParameters--%>
                </asp:LinqDataSource>
                <br />
                <div class="bottomhyperlink">
                    <asp:HyperLink ID="InsertHyperLink" runat="server" Enabled="true"><img runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Aggiungi nuova voce" />Aggiungi nuova voce</asp:HyperLink>
                    <br />
                    <asp:LinkButton ID="lbtAddToQuotation" runat="server" Enabled="true" 
                        OnClick="lbtAddToQuotation_Click" 
                        ><img runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Inserisci voci selezionate nel preventivo" />
                        <%--onclientclick="window.close();"window.opener.location.reload(); --%>
Inserisci voci selezionate nel preventivo</asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
