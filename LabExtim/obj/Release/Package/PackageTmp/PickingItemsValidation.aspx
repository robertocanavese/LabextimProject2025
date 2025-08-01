<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="PickingItemsValidation.aspx.cs" Inherits="LabExtim.PickingItemsValidation"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>
                Validazione Tabella base (coppie voce/dipendenza con unità di misura diverse)
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>
            <table width="100%">
                <%--<tr style="width: 100%" align="center">
                    <asp:Label ID="lblTitle" runat="server" Text="Selezionare una o più categorie di cui selezionare le voci e l'eventuale ordinamento prescelto: "
                        Font-Bold="true"></asp:Label></tr>--%>
                <tr>
                    <td style="width: 25%" align="center">
                        <asp:Label ID="lblddlTypes" runat="server" Text="Tipo"></asp:Label>
                    </td>
                    <td style="width: 25%" align="center">
                        <asp:Label ID="lblddlItemTypes" runat="server" Text="Tipo voce"></asp:Label>
                    </td>
                    <%--<td style="width: 25%" align="center">
                        <asp:Label ID="lblddlSuppliers" runat="server" Text="Fornitore"></asp:Label>
                    </td>--%>
                    <td style="width: 25%" align="center">
                        <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>
                    </td>
                </tr>
                <tr style="width: 100%" align="center">
                    <td style="width: 25%" align="center">
                        <asp:DropDownList ID="ddlTypes" runat="server" AutoPostBack="true" DataSourceID="ldsTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlTypes_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Types" OrderBy="Order">
                        </asp:LinqDataSource>
                    </td>
                    <td style="width: 25%" align="center">
                        <asp:DropDownList ID="ddlItemTypes" runat="server" AutoPostBack="true" DataSourceID="ldsItemTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlItemTypes_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsItemTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ItemTypes" OrderBy="Order">
                        </asp:LinqDataSource>
                    </td>
                    <%--<td style="width: 25%" align="center">
                        <asp:DropDownList ID="ddlSuppliers" runat="server" AutoPostBack="true" DataSourceID="ldsSuppliers"
                            DataTextField="Name" DataValueField="Code" CssClass="droplist" OnDataBound="ddlSuppliers_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsSuppliers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Suppliers" OrderBy="Name">
                        </asp:LinqDataSource>
                    </td>--%>
                    <td style="width: 25%" align="center">
                        <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="true" DataTextField="Key"
                            DataValueField="Value" CssClass="droplist" OnSelectedIndexChanged="PersistSelection"
                            OnDataBound="ddlOrderBy_DataBound">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdPickingItems"
                Display="None" />
            <asp:Panel ID="DetailsPanel" runat="server" >
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtViewInputItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewInputItems_Click" Text="Visualizza gestione voci attive" />
                            &nbsp;
                            <asp:LinkButton ID="lbtViewDeactivatedItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewDeactivatedItems_Click" Text="Visualizza gestione voci disattivate" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintPickingItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtPrintPickingItems_Click" Text="Stampa tabella" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<asp:Panel runat="server" Height="670"  Width="100%" ScrollBars="Vertical" --%>
                            <asp:LinqDataSource ID="GridDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="VW_UngroupablePickingItems" OnSelected="GridDataSource_Selected" EnableUpdate="True"
                                EnableDelete="True" EnableInsert="True" OnSelecting="GridDataSource_Selecting"
                                AutoGenerateOrderByClause="true">
                                <WhereParameters>
                                    <asp:Parameter Name="Inserted" Type="Boolean" DefaultValue="True" />
                                    <asp:ControlParameter ControlID="ddlTypes" Name="TypeCodeChecked" Type="Int32" PropertyName="SelectedValue" 
                                        DefaultValue="" />
                                    <asp:ControlParameter ControlID="ddlItemTypes" Name="ItemTypeCodeChecked" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />
                  <%--                  <asp:ControlParameter ControlID="ddlSuppliers" Name="SupplierCode" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />--%>
                                </WhereParameters>
                                <OrderByParameters>
                                    <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                </OrderByParameters>
                            </asp:LinqDataSource>
                            <%--<WhereParameters>
                        <asp:QueryStringParameter Name="ID" QueryStringField="P0" Type="Int32" />
                    </WhereParameters>--%>
                            <asp:GridView ID="grdPickingItems" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                DataSourceID="GridDataSource" DataKeyNames="IDChecked" CssClass="gridview" OnPageIndexChanging="grdPickingItems_PageIndexChanging"
                                OnDataBound="OnGridViewDataBound" OnRowDataBound="grdPickingItems_RowDataBound"
                                Width="1400" OnRowDeleted="grdPickingItems_RowDeleted" OnRowCommand="grdPickingItems_RowCommand">
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
                                            <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Gestione voce" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="IDChecked" HeaderText="ID voce" />
                                    <asp:BoundField DataField="TypeDescChecked" HeaderText="Tipo" />
                                    <asp:BoundField DataField="ItemTypeDescChecked" HeaderText="Tipo Voce" />
                                    <asp:BoundField DataField="ItemDescriptionChecked" HeaderText="Descrizione" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="UMDescChecked" HeaderText="UM" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Red" />
                                    <asp:BoundField DataField="UMDescDep" HeaderText="DIP UM" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Red"/>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypEditDep" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Gestione dipendenza" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="IDDep" HeaderText="DIP ID voce" />
                                    <asp:BoundField DataField="TypeDescDep" HeaderText="DIP Tipo" />
                                    <asp:BoundField DataField="ItemTypeDescDep" HeaderText="DIP Tipo Voce" />
                                    <asp:BoundField DataField="ItemDescriptionDep" HeaderText="DIP Descrizione" ItemStyle-Width="250"  />
                                </Columns>
                                <PagerStyle CssClass="footer" />
                                <%--<SelectedRowStyle CssClass="selected" />--%>
                                <PagerTemplate>
                                    <asp:GridViewPager ID="Pager0" runat="server" />
                                </PagerTemplate>
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
