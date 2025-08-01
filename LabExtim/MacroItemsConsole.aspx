<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="MacroItemsConsole.aspx.cs" Inherits="LabExtim.MacroItemsConsole"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Gestione Macrovoci
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>


            <asp:Menu ID="mnuOperations" runat="server" Orientation="Horizontal" CssClass="menu"
                DisappearAfter="1000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                DynamicHorizontalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                <DynamicMenuStyle CssClass="menu" />
            </asp:Menu>
            <br />
            <table class="searchEngineMaster">
                <tr>
                    <td>
                        <asp:Label ID="lblID" runat="server" Text="ID voce"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTextContains" runat="server" Text="Descrizione contiene..."></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCompanies" runat="server" Text="Azienda"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlTypes" runat="server" Text="Tipo"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlItemTypes" runat="server" Text="Tipo voce"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cfb:IntTextBox ID="itbNo" runat="server" ShowFindButton="true"></cfb:IntTextBox>
                    </td>
                    <td>
                         <asp:TextBox ID="txtTitleContains" runat="server" CssClass="droplist" AutoPostBack="true" OnTextChanged="txtTitleContains_TextChanged" >
                        </asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCompanies" runat="server" AutoPostBack="true" DataSourceID="ldsCompanies"
                            DataTextField="Description" DataValueField="ID" CssClass="droplist" OnDataBound="ddlCompanies_DataBound"
                            OnSelectedIndexChanged="ddlCompanies_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsCompanies" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Companies" OrderBy="ID">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTypes" runat="server" AutoPostBack="true" DataSourceID="ldsTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlTypes_DataBound"
                            OnSelectedIndexChanged="ddlTypes_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Types" Where='Category="I"' OrderBy="Order">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlItemTypes" runat="server" AutoPostBack="true" DataSourceID="ldsItemTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlItemTypes_DataBound"
                            OnSelectedIndexChanged="ddlItemTypes_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsItemTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ItemTypes" Where='Category="I"' OrderBy="Order">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="true" DataTextField="Key"
                            DataValueField="Value" CssClass="droplist" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <h4 style="width: 100%; text-align: center">
                <asp:LinkButton ID="lbtToggleView" runat="server" ForeColor="Blue" Text="Lista macrovoci <-> Dettaglio macrovoce selezionata"
                    OnClick="lbtToggleView_Click"></asp:LinkButton>
            </h4>

            <%--<asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdMacroItems"
                Display="None" />--%>
            <asp:Panel ID="DetailsPanel" runat="server">
                <table>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:DetailsView ID="dtvMacroItem" runat="server" DataSourceID="ldsMacroItem" AutoGenerateEditButton="true"
                                AutoGenerateInsertButton="true" OnModeChanging="OnDetailsViewModeChanging" OnPreRender="OnDetailsViewPreRender"
                                OnItemUpdated="OnDetailsViewItemUpdated" OnItemInserted="OnDetailsViewItemInserted"
                                AutoGenerateRows="false" OnItemCommand="dtvMacroItem_ItemCommand" DataKeyNames="ID"
                                OnItemCreated="dtvMicroItem_ItemCreated">
                                <FieldHeaderStyle Font-Bold="true" />
                                <RowStyle CssClass="selected" />
                                <FooterStyle CssClass="selected" />
                                <Fields>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblID" runat="server" Text="ID" Width="120"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" Mode="ReadOnly" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblInserted" runat="server" Text="Inserisci"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean_Edit" />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean_Edit" />
                            </InsertItemTemplate>
                        </asp:TemplateField>--%>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCompany" runat="server" Text="Azienda"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit"
                                                Mode="Edit" AllowNullValue="false" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit" AllowNullValue="false" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblType" runat="server" Text="Tipo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey_Edit"
                                                Mode="Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey_Edit"
                                                Mode="Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblItemDescription" runat="server" Text="Descrizione macrovoce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="MacroItemDescription"
                                                UIHint="Text250" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="MacroItemDescription"
                                                UIHint="Text250_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="MacroItemDescription"
                                                UIHint="Text250_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblUM" runat="server" Text="UM"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycUM" runat="server" DataField="Unit" UIHint="ForeignKey" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycUM" runat="server" DataField="Unit" UIHint="ForeignKey_Edit"
                                                Mode="Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycUM" runat="server" DataField="Unit" UIHint="ForeignKey_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCost" runat="server" Text="Costo imposto"></asp:Label></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCostCalc" runat="server" Text="Costo calcolato"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCostCalc" runat="server" DataField="CostCalc" UIHint="Text"
                                                DataFormatString="{0:f4}" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPercentage" runat="server" Text="Ricarico standard imposto"></asp:Label></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Integer_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Integer_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPercentageCalc" runat="server" Text="Ricarico standard calcolato"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPercentageCalc" runat="server" DataField="PercentageCalc"
                                                UIHint="Text" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblMultiply" runat="server" Text="Moltiplica"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text="Data"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime"
                                                DataFormatString="{0:d}" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblLink" runat="server" Text="Dipendenza da macrovoce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycLink" runat="server" DataField="Link" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycLink" runat="server" DataField="Link" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycLink" runat="server" DataField="Link" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPILink" runat="server" Text="Dipendenza da voce di tabella base"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPILink" runat="server" DataField="PILink" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycPILink" runat="server" DataField="PILink" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycPILink" runat="server" DataField="PILink" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblOrder" runat="server" Text="Ordine"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycOrder" runat="server" DataField="Order" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycOrder" runat="server" DataField="Order" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycOrder" runat="server" DataField="Order" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblExpandInStats" runat="server" Text="Espandi nelle statistiche"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycExpandInStats" runat="server" DataField="ExpandInStats" UIHint="Boolean" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycExpandInStats" runat="server" DataField="ExpandInStats" UIHint="Boolean_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycExpandInStats" runat="server" DataField="ExpandInStats" UIHint="Boolean_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNote" runat="server" Text="Annotazioni"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="MultilineText_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="MultilineText_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblTemplate" runat="server" Text="Modello"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycTemplate" runat="server" DataField="Template" UIHint="Integer" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DynamicControl ID="dycTemplate" runat="server" DataField="Template" UIHint="Integer_Edit" />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DynamicControl ID="dycTemplate" runat="server" DataField="Template" UIHint="Integer_Edit" />
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblItemManufacturing" runat="server" Text="Produzione"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DynamicControl ID="dycItemManufacturing" runat="server" DataField="ItemManufacturing"
                                    UIHint="Text" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DynamicControl ID="dycItemManufacturing" runat="server" DataField="ItemManufacturing"
                                    UIHint="Integer_Edit" />
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <asp:DynamicControl ID="dycItemManufacturing" runat="server" DataField="ItemManufacturing"
                                    UIHint="Integer_Edit" />
                            </InsertItemTemplate>
                        </asp:TemplateField>--%>
                                </Fields>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lbtDeactivate" runat="server" CssClass="selected" Font-Bold="False"
                                        Text="Disattiva voce" CommandName="Deactivate" Enabled="false"></asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="lbtActivate" runat="server" CssClass="selected" Font-Bold="False"
                                        Text="Riattiva voce" CommandName="Activate" Enabled="false"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:DetailsView>
                            <br />
                            <asp:LinkButton ID="lbtViewInputItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewInputItems_Click" Text="Visualizza gestione macrovoci attive" />
                            <br />
                            <asp:LinkButton ID="lbtViewCalculated" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewCalculated_Click" Text="Visualizza prospetto calcolato" />
                            <br />
                            <asp:LinkButton ID="lbtViewDeactivatedItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewDeactivatedItems_Click" Text="Visualizza gestione macrovoci disattivate" />
                            <br />
                            <asp:LinkButton ID="lbtPrintMacroItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtPrintMacroItems_Click" Text="Stampa tabella" />
                            <asp:LinqDataSource ID="ldsMacroItem" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="DetailsDataSource_Inserting"
                                TableName="MacroItems">
                                <WhereParameters>
                                    <asp:DynamicControlParameter ControlId="grdMacroItems" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                            <br>
                        </td>
                        <td valign="top">
                            <%--<asp:Panel runat="server" Height="670"  Width="100%" ScrollBars="Vertical" --%>
                            <asp:MultiView ID="mvwMain" runat="server" OnActiveViewChanged="mvwMain_ActiveViewChanged">
                                <asp:View runat="server" ID="vewMacroItemsDetails">
                                    <asp:LinqDataSource ID="ldsMacroItemDetails" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                        EnableDelete="True" TableName="MacroItemDetails" Where="ID_MacroItem = @ID" OnSelecting="ldsMacroItemDetails_Selecting">
                                        <WhereParameters>
                                            <asp:ControlParameter ControlID="dtvMacroItem" Name="ID" PropertyName="DataKey.Value"
                                                Type="Int32" />
                                        </WhereParameters>
                                    </asp:LinqDataSource>
                                    <h5>Dettaglio macrovoce selezionata (voci tabella base incluse)
                                    </h5>
                                    <asp:Button ID="btnUpdateMenu" runat="server" Enabled="true" OnClick="btnUpdateMenu_Click"
                                        Text="Aggiorna menu" Width="120px" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnRecalc" runat="server" Enabled="false" OnClick="btnRecalc_Click"
                                        Text="Ricalcola voci" Width="120px" />
                                    <br />
                                    <br />
                                    <asp:GridView ID="grdMacroItemDetails" runat="server" AutoGenerateColumns="False"
                                        AutoGenerateDeleteButton="True" AllowPaging="True" DataSourceID="ldsMacroItemDetails"
                                        DataKeyNames="ID" CssClass="gridview" OnPageIndexChanging="grdMacroItemDetails_PageIndexChanging"
                                        OnDataBound="grdMacroItemDetails_DataBound" OnRowDataBound="grdMacroItemDetails_RowDataBound"
                                        Width="98%" OnRowDeleted="grdMacroItemDetails_RowDeleted" OnPreRender="grdMacroItemDetails_PreRender">
                                        <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altRow" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycID" runat="server" DataField="ID" Mode="ReadOnly" UIHint="Text" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblType" runat="server" Text="Tipo"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycType" runat="server" DataField="PickingItem.Type" UIHint="ForeignKey" />--%>
                                                    <asp:Label ID="lblType1" runat="server" Text='<%# Eval("PickingItem.Type.Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycItemType" runat="server" DataField="PickingItem.ItemType"  UIHint="ForeignKey" />--%>
                                                    <asp:Label ID="lblItemType1" runat="server" Text='<%# Eval("PickingItem.ItemType.Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblItemDescription" runat="server" Text="Descrizione voce"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycItemDescription" runat="server" DataField="PickingItem.ItemDescription"
                                                UIHint="Text" />--%>
                                                    <asp:Label ID="lblItemDescription1" runat="server" Text='<%# Eval("PickingItem.ItemDescription") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblSupplier" runat="server" Text="Fornitore"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycSupplier" runat="server" DataField="PickingItem.Supplier"
                                                UIHint="ForeignKey" />--%>
                                                    <asp:Label ID="lblSupplier1" runat="server" Text='<%# Eval("PickingItem.Supplier.Name") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblUM" runat="server" Text="UM"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycUM" runat="server" DataField="PickingItem.Unit" UIHint="ForeignKey" />--%>
                                                    <asp:Label ID="lblUM1" runat="server" Text='<%# Eval("PickingItem.Unit.Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblQuantity" Text="Quantità" runat="server"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantity" runat="server" Width="50" MaxLength="6" CssClass="droplist"
                                                        ToolTip="Inserire un valore intero, decimale con virgola o frazionario"> </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCost" runat="server" Text="Costo"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycCost" runat="server" DataField="PickingItem.Cost" UIHint="Text" />--%>
                                                    <asp:Label ID="lblCost1" runat="server" Text='<%# Eval("PickingItem.Cost") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblMultiply" runat="server" Text="Moltiplica"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycMultiply" runat="server" DataField="PickingItem.Multiply"
                                                UIHint="Boolean" />--%>
                                                    <asp:CheckBox ID="chkMultiply1" runat="server" Checked='<%# Eval("PickingItem.Multiply") %>'
                                                        Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblStandardPercentage" runat="server" Text="Tipo ric std"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycPercentageAuto" runat="server" DataField="PickingItem.PercentageAuto"
                                                UIHint="Text" />--%>
                                                    <asp:Label ID="lblStandardPercentage1" runat="server" Text='<%# Eval("PickingItem.StandardPercentage.Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblPercentageAuto" runat="server" Text="% Ric std"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycPercentageAuto" runat="server" DataField="PickingItem.PercentageAuto"
                                                UIHint="ForeignKey" />--%>
                                                    <asp:Label ID="lblPercentageAuto1" runat="server" Text='<%# Eval("PickingItem.PercentageAuto") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text="Data"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycDate" runat="server" DataField="PickingItem.Date" DataFormatString="{0:d}"
                                                UIHint="DateTime" />--%>
                                                    <asp:Label ID="lblDate1" runat="server" Text='<%# Eval("PickingItem.Date", "{0:d}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblLink" runat="server" Text="Dipendenza"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycLink" runat="server" DataField="PickingItem.Link" UIHint="Text" />--%>
                                                    <asp:Label ID="lblLink1" runat="server" Text='<%# Eval("PickingItem.Link") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblOrder" runat="server" Text="Ordine"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycOrder" runat="server" DataField="PickingItem.Order" UIHint="Text" />--%>
                                                    <asp:Label ID="lblOrder1" runat="server" Text='<%# Eval("PickingItem.Order") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="footer" />
                                        <SelectedRowStyle CssClass="gridview" />
                                        <PagerTemplate>
                                            <asp:GridViewPager ID="Pager2" runat="server" />
                                        </PagerTemplate>
                                        <EmptyDataTemplate>
                                            Nessuna voce trovata.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:View>
                                <asp:View runat="server" ID="vewMacroItems">
                                    <h5>Lista macrovoci</h5>
                                    <asp:LinqDataSource ID="ldsMacroItems" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                        EnableDelete="True" EnableInsert="True" EnableUpdate="True" OnSelected="ldsMacroItems_Selected"
                                        OnSelecting="ldsMacroItems_Selecting" TableName="MacroItems" AutoGenerateOrderByClause="true">
                                        <WhereParameters>
                                            <%--<asp:Parameter DefaultValue="True" Name="Inserted" Type="Boolean" />
                                            <asp:ControlParameter ControlID="itbNo" Name="ID" Type="Int32" PropertyName="Text" />
                                            <asp:ControlParameter ControlID="ddlCompanies" Name="ID_Company" Type="Int32" PropertyName="SelectedValue" />
                                            <asp:ControlParameter ControlID="ddlTypes" DefaultValue="" Name="TypeCode" PropertyName="SelectedValue" Type="Int32" />
                                            <asp:ControlParameter ControlID="ddlItemTypes" DefaultValue="" Name="ItemTypeCode" PropertyName="SelectedValue" Type="Int32" />
                                            <%--<asp:ControlParameter ControlID="ddlSuppliers" DefaultValue="" Name="SupplierCode"
                                        PropertyName="SelectedValue" Type="Int32" />--%>
                                        </WhereParameters>
                                        <OrderByParameters>
                                            <asp:ControlParameter ControlID="ddlOrderBy" DefaultValue="" Name="OrderBy" PropertyName="SelectedValue"
                                                Type="String" />
                                        </OrderByParameters>
                                    </asp:LinqDataSource>
                                    <%--<WhereParameters>
                        <asp:QueryStringParameter Name="ID" QueryStringField="P0" Type="Int32" />
                    </WhereParameters>--%>
                                    <asp:GridView ID="grdMacroItems" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True"
                                        AutoGenerateDeleteButton="True" AllowPaging="True" DataSourceID="ldsMacroItems"
                                        DataKeyNames="ID" CssClass="gridview" OnPageIndexChanging="grdMacroItems_PageIndexChanging"
                                        OnDataBound="OnGridViewDataBound" OnRowDataBound="grdMacroItems_RowDataBound"
                                        Width="98%" OnRowDeleted="grdMacroItems_RowDeleted" OnPreRender="grdMacroItems_PreRender">
                                        <SelectedRowStyle BackColor="#FFCC99" />
                                        <Columns>
                                            <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" CausesValidation="false"
                                Text="Elimina" OnClientClick='return confirm("Confermi la cancellazione di questa voce?");' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                            <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="InsertLinkButton" runat="server" CommandName="Insert" CausesValidation="false"
                                Text="Nuovo" OnClientClick='return confirm("Confermi la creazione di una nuova voce?");' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycID" runat="server" DataField="ID" UIHint="Text" Mode="ReadOnly" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblInserted" runat="server" Text="Inserisci"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean_Edit"
                                Mode="Insert" />
                        </InsertItemTemplate>
                    </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblType" runat="server" Text="Tipo"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey" />--%>
                                                    <asp:Label ID="dycType" runat="server" Text='<%# Eval("Type.Description") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey_Edit"
                                                        Mode="Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey_Edit"
                                                        Mode="Insert" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblItemType" runat="server" Text="Tipo voce"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey" />--%>
                                                    <asp:Label ID="dycItemType" runat="server" Text='<%# Eval("ItemType.Description") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey_Edit"
                                                        Mode="Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey_Edit"
                                                        Mode="Insert" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblItemDescription" runat="server" Text="Descrizione macrovoce"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="MacroItemDescription"
                                                        UIHint="Text" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="MacroItemDescription"
                                                        UIHint="Text_Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="MacroItemDescription"
                                                        UIHint="Text_Edit" Mode="Insert" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblUM" runat="server" Text="UM"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:DynamicControl ID="dycUM" runat="server" DataField="Unit" UIHint="ForeignKey" />--%>
                                                    <asp:Label ID="dycUM" runat="server" Text='<%# Eval("Unit.Description") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycUM" runat="server" DataField="Unit" UIHint="ForeignKey_Edit"
                                                        Mode="Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycUM" runat="server" DataField="Unit" UIHint="ForeignKey_Edit"
                                                        Mode="Insert" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblCost" runat="server" Text="Costo"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycCost" runat="server" DataField="CostCalc" UIHint="Text"
                                                        DataFormatString="{0:f2}" />
                                                </ItemTemplate>
                                                <%--<EditItemTemplate>
                                                    <asp:DynamicControl ID="dycCost" runat="server" DataField="CostCalc" UIHint="Decimal_Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycCost" runat="server" DataField="CostCalc" UIHint="Decimal_Edit"
                                                        Mode="Insert" />
                                                </InsertItemTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblMultiply" runat="server" Text="Moltiplica"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean_Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean_Edit"
                                                        Mode="Insert" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text="Data"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime"
                                                        DataFormatString="{0:d}" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime_Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime_Edit"
                                                        Mode="Insert" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblLink" runat="server" Text="Dipendenza da macrovoce"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycLink" runat="server" DataField="Link" UIHint="Text" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycLink" runat="server" DataField="Link" UIHint="Text_Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycLink" runat="server" DataField="Link" UIHint="Text_Edit"
                                                        Mode="Insert" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblPILink" runat="server" Text="Dipendenza da voce di tabella base"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycPILink" runat="server" DataField="PILink" UIHint="Text" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycPILink" runat="server" DataField="PILink" UIHint="Text_Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycPILink" runat="server" DataField="PILink" UIHint="Text_Edit" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblOrder" runat="server" Text="Ordine"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycOrder" runat="server" DataField="Order" UIHint="Text" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycOrder" runat="server" DataField="Order" UIHint="Text_Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycOrder" runat="server" DataField="Order" UIHint="Text_Edit"
                                                        Mode="Insert" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblExpandInStats" runat="server" Text="Espandi"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:DynamicControl ID="dycExpandInStats" runat="server" DataField="ExpandInStats" UIHint="Boolean" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DynamicControl ID="dycExpandInStats" runat="server" DataField="ExpandInStats" UIHint="Boolean_Edit" />
                                                </EditItemTemplate>
                                                <InsertItemTemplate>
                                                    <asp:DynamicControl ID="dycExpandInStats" runat="server" DataField="ExpandInStats" UIHint="Boolean_Edit" />
                                                </InsertItemTemplate>
                                            </asp:TemplateField>
                                            <%--                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblTemplate" runat="server" Text="Modello"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycTemplate" runat="server" DataField="Template" UIHint="Integer" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycTemplate" runat="server" DataField="Template" UIHint="Integer_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycTemplate" runat="server" DataField="Template" UIHint="Integer_Edit"
                                Mode="Insert" />
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblItemManufacturing" runat="server" Text="Produzione"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycItemManufacturing" runat="server" DataField="ItemManufacturing"
                                UIHint="Text" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycItemManufacturing" runat="server" DataField="ItemManufacturing"
                                UIHint="Integer_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycItemManufacturing" runat="server" DataField="ItemManufacturing"
                                UIHint="Integer_Edit" Mode="Insert" />
                        </InsertItemTemplate>
                    </asp:TemplateField>--%>
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
                                </asp:View>
                            </asp:MultiView>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
