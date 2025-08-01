<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductionOrderDetailsInsertByOdP.aspx.cs"
    Inherits="LabExtim.ProductionOrderDetailsInsertByOdP" MasterPageFile="~/Site.master"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox" TagPrefix="cfb" %>
<%--<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="tp1" %>--%>
<%--<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <%--<asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />--%>
        <asp:UpdatePanel ID="updDetail" runat="server">
            <ContentTemplate>
                <asp:Panel ID="DetailsPanel" runat="server">
                    <h2 style="font-family: Verdana, Geneva, Tahoma, sans-serif; text-align: center">
                        <%--Denuncia operazioni per operatore--%>
                        <%--<asp:Label ID="lblItemNo" runat="server" />--%>
                    </h2>
                    <table>
                        <tr>
                            <td colspan="3">
                                <asp:Menu ID="mnuOperations1" runat="server" Orientation="Horizontal" CssClass="menu"
                                    DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                    DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                                    <DynamicMenuStyle CssClass="menu" />
                                    <%--<DynamicHoverStyle Height="40px" />--%>
                                </asp:Menu>
                                <asp:Menu ID="mnuOperations2" runat="server" Orientation="Horizontal" CssClass="menu"
                                    DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                    DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                                    <DynamicMenuStyle CssClass="menu" />
                                    <%--<DynamicHoverStyle Height="40px" />--%>
                                </asp:Menu>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 10px">
                                <asp:Label ID="lblID_ProductionOrder" runat="server" Text="Id OdP (solo azienda corrente!)"></asp:Label>
                            </td>
                            <td style="padding-right: 10px">
                                <asp:Label ID="lblNumber" runat="server" Text="Id da Numero (solo azienda corrente!)"></asp:Label>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="padding-right: 10px">
                                <cfb:IntTextBox ID="itbNoOdP" runat="server" CssClass="droplist" ShowFindButton="true" />
                            </td>
                            <td style="padding-right: 10px">
                                <cfb:YearCounterTextBox ID="yctNumber" runat="server" CssClass="droplist" ShowFindButton="true"></cfb:YearCounterTextBox>
                            </td>
                            <td><asp:Label ID="lblInfo" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                    <br />
                </asp:Panel>
                <h5>Dettaglio denuncia produzione per OdP</h5>
                <%--<asp:UpdatePanel ID="updDetail" runat="server">
            <ContentTemplate>--%>
                <asp:Panel ID="pnlDetail" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lbtNewItem" runat="server" CssClass="gridview" Font-Bold="True"
                                    ForeColor="Brown" Text="Aggiungi righe" OnClick="lbtNewItem_Click" />
                                &nbsp;
                                <%--<asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                    ForeColor="Green" Text="Aggiorna lista" />
                                &nbsp;--%>
                                <asp:LinkButton ID="lbtSubmit" runat="server" CssClass="gridview" Font-Bold="True"
                                    ForeColor="Red" Text="Salva tutti" OnClick="lbtSubmit_Click" />
                                &nbsp;
                                <asp:Label runat="server" Text="(Attenzione: i comandi 'Salva' e 'Salva tutti' salvano i dati su disco e ricalcolano i costi di produzione sulla base delle tariffe correnti)"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<asp:LinqDataSource ID="ldsProductionOrderDetails" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ProductionOrderDetails" OnSelecting="ldsProductionOrderDetails_Selecting" 
                            >
                            <WhereParameters>
                                <asp:Parameter Name="Inserted" DefaultValue="true" Type="Boolean" />
                                <asp:ControlParameter ControlID="ddlEmployees" Name="ID_Owner" Type="Int32" PropertyName="SelectedValue"
                                    DefaultValue="" />
                                <asp:ControlParameter ControlID="ddlStatuses" Name="Status" Type="Int32" PropertyName="SelectedValue"
                    DefaultValue="" />
                <asp:ControlParameter ControlID="ddlCustomers" Name="CustomerCode" Type="Int32" PropertyName="SelectedValue"
                    DefaultValue="" />
                                <asp:DynamicQueryStringParameter Name="ID" />
                            </WhereParameters>
                            <OrderByParameters>
                <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                    DefaultValue="" />
            </OrderByParameters>
                        </asp:LinqDataSource>--%>
                                <%--asp:GridView ID="grdProductionOrderDetails" runat="server" AllowPaging="True" CssClass="gridview"
                             OnPageIndexChanging="grdProductionOrderDetails_PageIndexChanging"
                            AutoGenerateColumns="false" >--%>
                                <asp:LinqDataSource ID="ldsOwner" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    TableName="Employees" OrderBy="Name" OnSelected="ldsOfaDropDownList_Selected" OnSelecting="ldsOfaDropDownList_Selecting">
                                </asp:LinqDataSource>
                                <asp:LinqDataSource ID="ldsRawMaterial" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    TableName="" OnSelected="ldsOfaDropDownList_Selected" OnSelecting="ldsOfaDropDownList_Selecting">
                                </asp:LinqDataSource>
                                <asp:LinqDataSource ID="ldsPhase" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    TableName="PickingItems" OrderBy="ItemDescription" Where="TypeCode=31" OnSelected="ldsOfaDropDownList_Selected">
                                </asp:LinqDataSource>
                                <asp:LinqDataSource ID="ldsSupplier" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    TableName="Suppliers" OrderBy="Name" OnSelected="ldsOfaDropDownList_Selected">
                                </asp:LinqDataSource>
                                <asp:LinqDataSource ID="ldsUM" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    TableName="Units" OrderBy="Description" OnSelected="ldsOfaDropDownList_Selected">
                                </asp:LinqDataSource>
                                <asp:LinqDataSource ID="ldsRawMaterialSup" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    TableName="" OnSelected="ldsOfaDropDownList_Selected" OnSelecting="ldsOfaDropDownList_Selecting">
                                </asp:LinqDataSource>
                                <asp:GridView ID="grdProductionOrderDetails" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                                    OnRowCommand="grdProductionOrderDetails_RowCommand" OnRowDeleting="grdProductionOrderDetails_RowDeleting"
                                    OnPreRender="grdProductionOrderDetails_PreRender" OnRowDataBound="grdProductionOrderDetails_RowDataBound"
                                    OnRowCreated="grdProductionOrderDetails_RowCreated" OnRowEditing="grdProductionOrderDetails_RowEditing"
                                    OnRowUpdating="grdProductionOrderDetails_RowUpdating" OnRowCancelingEdit="grdProductionOrderDetails_RowCancelingEdit">
                                    <RowStyle BackColor="#ffffdd" HorizontalAlign="Center" />

                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="SubmitLinkButton" runat="server" CommandName="Submit" CommandArgument='<%# Container.DataItemIndex %>'
                                                    CausesValidation="true" Text="Salva" OnClientClick="return confirm('Confermi il ricalcolo ed il salvataggio di questa singola voce?');" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
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
                                        <asp:CommandField ButtonType="Link" EditText="Modifica" ShowEditButton="true" />
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
                                                <asp:Label ID="lblID_ProductionOrder" runat="server" Text='<%# Eval("ID_ProductionOrder") %>'
                                                    CssClass="droplist" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblEmployeeDesc" runat="server" Text="Operatore"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeDesc" runat="server" CssClass="droplist" Text='<%# Eval("EmployeeDesc") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEmployee" runat="server" DataTextField="UniqueName" DataValueField="ID"
                                                    CssClass="droplist" DataSourceID="ldsOwner" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblProductionDate" runat="server" Text="Data"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductionDate" runat="server" CssClass="droplist" Text='<%# Eval("ProductionDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtProductionDate" runat="server" CssClass="droplist" Columns="10" Text='<%# Bind("ProductionDate") %>'></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                                                    ImageAlign="Middle" />
                                                <cc1:CalendarExtender ID="txtProductionDate_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                                                    CssClass="MyCalendar" PopupButtonID="ImageButton1" TargetControlID="txtProductionDate"
                                                    PopupPosition="Right">
                                                </cc1:CalendarExtender>
                                            </EditItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblPhase" runat="server" Text="Fase"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ddlPhase" runat="server" Text='<%# Eval("PhaseDesc") %>' CssClass="droplist" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlPhase" runat="server" DataTextField="ItemDescription" DataValueField="ID"
                                                    CssClass="droplist" DataSourceID="ldsPhase" />
                                                <%--SelectedIndex='<%#GetSelectedIndex((DropDownList)sender, Eval("ID_Phase")) %>'--%>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblProductionTime" runat="server" Text="Tempo Prod (hh:mm)"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtProductionTime" runat="server" CssClass="droplist" Width="40"
                                                    Enabled="false" />
                                                <cc1:MaskedEditExtender ID="meeProductionTime" runat="server" AcceptNegative="None"
                                                    Enabled="True" TargetControlID="txtProductionTime" AcceptAMPM="false" MaskType="Time"
                                                    Mask="99:99:99" CultureName="EN-us" AutoCompleteValue="00:00:00" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtProductionTime" runat="server" CssClass="droplist" Width="40" />
                                                <cc1:MaskedEditExtender ID="meeProductionTime" runat="server" AcceptNegative="None"
                                                    Enabled="True" TargetControlID="txtProductionTime" AcceptAMPM="false" MaskType="Time"
                                                    Mask="99:99:99" CultureName="EN-us" AutoCompleteValue="00:00:00" Filtered=":" />
                                                <cc1:MaskedEditValidator ID="mevProductionTime" runat="server" ControlToValidate="txtProductionTime"
                                                    ControlExtender="meeProductionTime" IsValidEmpty="true" SetFocusOnError="true"
                                                    Display="None"></cc1:MaskedEditValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRawMaterial" runat="server" Text="Voce di base"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ddlRawMaterial" runat="server" Text='<%# Eval("PickingItemDesc") %>'
                                                    Width="170" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <table class="cellmenu">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbRawMaterial" runat="server" GroupName="TargetMenu" Checked="true" OnCheckedChanged="rdbRawMaterial_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRawMaterial" runat="server" DataTextField="ComboText" DataValueField="ComboValue"
                                                                CssClass="droplist" DataSourceID="ldsRawMaterial" AutoPostBack="true" Width="170"
                                                                OnSelectedIndexChanged="ddlRawMaterial_SelectedIndexChanged" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblUMRawMaterial" runat="server" Text="UM Voce Base"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ddlUMRawMaterial" runat="server" Text='<%# Eval("UMRawMaterialDesc") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlUMRawMaterial" runat="server" DataTextField="Description"
                                                    DataValueField="ID" CssClass="droplist" DataSourceID="ldsUM" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRawMaterialQuantity" runat="server" Text="Qtà Voce Base"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="txtRawMaterialQuantity" runat="server" CssClass="droplist" Text='<%# Eval("RawMaterialQuantity") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <cfb:FloatTextBox ID="txtRawMaterialQuantity" runat="server" CssClass="droplist"
                                                    Text='<%# Bind("RawMaterialQuantity") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRawMaterialSup" runat="server" Text="Voce supplementare"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ddlRawMaterialSup" runat="server" Text='<%# Eval("PickingItemSupDesc") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <table class="cellmenu">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbRawMaterialSup" runat="server" GroupName="TargetMenu" OnCheckedChanged="rdbRawMaterialSup_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRawMaterialSup" runat="server" DataTextField="ComboText"
                                                                DataValueField="ComboValue" CssClass="droplist" DataSourceID="ldsRawMaterialSup"
                                                                AutoPostBack="true" Width="170" OnSelectedIndexChanged="ddlRawMaterialSup_SelectedIndexChanged" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblUMUser" runat="server" Text="UM Voce Sup "></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ddlUMUser" runat="server" Text='<%# Eval("UMUserDesc") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlUMUser" runat="server" DataTextField="Description" DataValueField="ID"
                                                    CssClass="droplist" DataSourceID="ldsUM" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRawMaterialX" runat="server" Text="Qtà Voce Sup"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="txtRawMaterialX" runat="server" CssClass="droplist" Text='<%# Eval("RawMaterialX") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <cfb:FloatTextBox ID="txtRawMaterialX" runat="server" CssClass="droplist" Text='<%# Bind("RawMaterialX") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRawMaterialY" runat="server" Text="b (UMMS)"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="txtRawMaterialY" runat="server" CssClass="droplist" Text='<%# Eval("RawMaterialY") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <cfb:FloatTextBox ID="txtRawMaterialY" runat="server" CssClass="droplist" Text='<%# Bind("RawMaterialY") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRawMaterialZ" runat="server" Text="h (UMMS)"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="txtRawMaterialZ" runat="server" CssClass="droplist" Text='<%# Eval("RawMaterialZ") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <cfb:FloatTextBox ID="txtRawMaterialZ" runat="server" CssClass="droplist" Text='<%# Bind("RawMaterialZ") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQuantityOver" runat="server" Text="Evaso"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkQuantityOver" runat="server" CssClass="droplist" Enabled="false"
                                                    Checked='<%# Eval("QuantityOver") %>' />
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>
                                                <asp:CheckBox ID="chkQuantityOver" runat="server" CssClass="droplist" Checked='<%# Eval("QuantityOver") %>' />
                                            </EditItemTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNote" runat="server" Text="Note"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNote" runat="server" CssClass="droplist" TextMode="MultiLine"
                                                    ReadOnly="true" Text='<%# Eval("Note") %>' Width="150" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNote" runat="server" CssClass="droplist" TextMode="MultiLine"
                                                    Width="150" Text='<%# Bind("Note") %>' />
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
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
