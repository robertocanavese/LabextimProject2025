<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ProductionOrderDetailsInsertByD.aspx.cs"
    Inherits="LabExtim.ProductionOrderDetailsInsertByD" MasterPageFile="~/Site.master"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%--<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="tp1" %>--%>
<%--<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
        <asp:Panel ID="DetailsPanel" runat="server">
            <h2 style="font-family: Verdana, Geneva, Tahoma, sans-serif; text-align: center">
                <%--Denuncia operazioni per operatore--%>
                <%--<asp:Label ID="lblItemNo" runat="server" />--%>
            </h2>
            <table>
                <tr>
                    <%--<td style="width: 33%" align="center">
                        <asp:label id="lblddlemployees" runat="server" text="operatore"></asp:label>
                    </td>--%>
                    <td style="width: 33%" align="center">
                        <asp:Label ID="lbldtedate" runat="server" Text="data operazione"></asp:Label>
                    </td>
                </tr>
                <tr style="width: 100%" align="center">
                     <td style="width: 33%" align="center">
                        <asp:TextBox ID="txtDate" runat="server" CssClass="droplist" Columns="10" OnTextChanged="txtDate_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                            ImageAlign="Middle" />
                        <cc1:CalendarExtender ID="txtDate_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                            CssClass="MyCalendar" PopupButtonID="ImageButton1" TargetControlID="txtDate"
                            PopupPosition="Right" SelectedDate='<%# DateTime.Today %>'>
                        </cc1:CalendarExtender>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <h5>
            Dettaglio denuncia giornaliera operazioni (voci libere e impreviste)</h5>
        <asp:Panel ID="pnlDetail" runat="server" >
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbtNewItem" runat="server" CssClass="gridview" Font-Bold="True"
                            ForeColor="Brown" Text="Aggiungi righe" OnClick="lbtNewItem_Click" />
                        &nbsp;
                        <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                            ForeColor="Green" Text="Aggiorna lista" />
                        &nbsp;
                        <asp:LinkButton ID="lbtSubmit" runat="server" CssClass="gridview" Font-Bold="True"
                            ForeColor="Red" Text="Invia ad archivo" OnClick="lbtSubmit_Click" />
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinqDataSource ID="ldsType" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Types" OrderBy="Order" Where='Category == "I"' OnSelected="ldsOfaDropDownList_Selected">
                        </asp:LinqDataSource>
                        <asp:LinqDataSource ID="ldsItemType" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ItemTypes" Where='Category == "I"' OrderBy="Order" OnSelected="ldsOfaDropDownList_Selected">
                        </asp:LinqDataSource>
                        <asp:LinqDataSource ID="ldsUM" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Units" OrderBy="Description" OnSelected="ldsOfaDropDownList_Selected">
                        </asp:LinqDataSource>
                        <asp:LinqDataSource ID="ldsSupplier"  runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Suppliers" OrderBy="Name" OnSelecting="ldsOfaDropDownList_Selecting" OnSelected="ldsOfaDropDownList_Selected">
                        </asp:LinqDataSource>
                        <asp:GridView ID="grdProductionOrderDetails" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                            OnRowCommand="grdProductionOrderDetails_RowCommand" OnRowDeleting="grdProductionOrderDetails_RowDeleting"
                            OnPreRender="grdProductionOrderDetails_PreRender" OnRowDataBound="grdProductionOrderDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%--<asp:HyperLink ID="EditHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'
                                Text="Modifica" />&nbsp;--%>
                                        <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" CausesValidation="false"
                                            Text="Elimina" OnClientClick="return confirm('Confermi l\'eliminazione di questa voce?');" />
                                        <%--&nbsp;<asp:HyperLink
                                                ID="DetailsHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Details, GetDataItem()) %>'
                                                Text="Dettaglio" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblState" runat="server" Text="Stato"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblState" runat="server" CssClass="droplist" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblID_ProductionOrder" runat="server" Text="ID OdP"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <cfb:IntTextBox ID="txtID_ProductionOrder" runat="server" CssClass="droplist" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblType" runat="server" Text="Tipo fase"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlType" runat="server" DataTextField="Description" DataValueField="Code"
                                            CssClass="droplist" DataSourceID="ldsType" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblItemType" runat="server" Text="Tipo prodotto"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlItemType" runat="server" DataTextField="Description" DataValueField="Code"
                                            CssClass="droplist" DataSourceID="ldsItemType" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text="Descrizione (max 255 caratteri)"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate >
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="droplist" Width="250" TextMode="MultiLine" Rows="1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUM" runat="server" Text="UM"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlUMRawMaterial" runat="server" DataTextField="Description" DataValueField="ID"
                                            CssClass="droplist" DataSourceID="ldsUM" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRawMaterialQuantity" runat="server" Text="Qtà"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <cfb:FloatTextBox ID="txtRawMaterialQuantity" runat="server" CssClass="droplist" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUnitCost" runat="server" Text="Costo Uni €"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <cfb:FloatTextBox ID="txtUnitCost" runat="server" CssClass="droplist" Width="60" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblSupplier" runat="server" Text="Fornitore"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlSupplier" runat="server" DataTextField="Name" DataValueField="Code"
                                            CssClass="droplist" DataSourceID="ldsSupplier" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblNote" runat="server" Text="Note"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtNote" runat="server" CssClass="droplist" TextMode="MultiLine" Rows="1" 
                                            Width="250" />
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
        </asp:Panel>
    </div>
</asp:Content>
