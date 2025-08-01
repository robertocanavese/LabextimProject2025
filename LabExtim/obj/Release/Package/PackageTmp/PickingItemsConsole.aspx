<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True"
    CodeBehind="PickingItemsConsole.aspx.cs" Inherits="LabExtim.PickingItemsConsole"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/SearchEngine.ascx" TagName="SearchEngine" TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Gestione Tabella base
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>
            <%--<table width="100%">
                <tr>
                    <td>
                        <cfb:SearchEngine ID="senMain" runat="server"></cfb:SearchEngine>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>--%>

            <table class="searchEngine">
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
                        <asp:Label ID="lblddlSuppliers" runat="server" Text="Fornitore"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblddlOrderBy" runat="server" Text="Ordina voci per... "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblObsolete" runat="server" Text="Solo voci con costo obsoleto"></asp:Label>
                    </td>
                </tr>
                <tr>

                    <td>
                        <cfb:IntTextBox ID="itbNo" runat="server" ShowFindButton="true"></cfb:IntTextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTitleContains" runat="server" CssClass="droplist" AutoPostBack="true" OnTextChanged="PersistSelection">
                        </asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCompanies" runat="server" AutoPostBack="true" DataSourceID="ldsCompanies"
                            DataTextField="Description" DataValueField="ID" CssClass="droplist" OnDataBound="ddlCompanies_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsCompanies" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Companies" OrderBy="ID">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTypes" runat="server" AutoPostBack="true" DataSourceID="ldsTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlTypes_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Types" Where='Category="I"' OrderBy="Order">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlItemTypes" runat="server" AutoPostBack="true" DataSourceID="ldsItemTypes"
                            DataTextField="Description" DataValueField="Code" CssClass="droplist" OnDataBound="ddlItemTypes_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsItemTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="ItemTypes" Where='Category="I"' OrderBy="Order">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSuppliers" runat="server" AutoPostBack="true" DataSourceID="ldsSuppliers"
                            DataTextField="Name" DataValueField="Code" CssClass="droplist" OnDataBound="ddlSuppliers_DataBound"
                            OnSelectedIndexChanged="PersistSelection">
                        </asp:DropDownList>
                        <asp:LinqDataSource ID="ldsSuppliers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="Suppliers" OrderBy="Name">
                        </asp:LinqDataSource>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOrderBy" runat="server" AutoPostBack="true" DataTextField="Key"
                            DataValueField="Value" CssClass="droplist" OnSelectedIndexChanged="PersistSelection"
                            OnDataBound="ddlOrderBy_DataBound">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkObsolete" runat="server" AutoPostBack="true" CssClass="droplist"
                            OnCheckedChanged="PersistSelection"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
            </table>
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdPickingItems"
                Display="None" />
            <asp:Panel ID="DetailsPanel" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:HyperLink ID="lbtNew" runat="server" CssClass="red cursor" Font-Bold="True"
                                Text="Nuova voce" />
                            &nbsp;
                            <asp:LinkButton ID="lbtViewInputItems" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewInputItems_Click" Text="Visualizza gestione voci attive" />
                            &nbsp;
                            <asp:LinkButton ID="lbtViewCalculated" runat="server" CssClass="gridview" Font-Bold="True"
                                OnClick="lbtViewCalculated_Click" Text="Visualizza prospetto calcolato" />
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
                                TableName="PickingItems" OnSelected="GridDataSource_Selected" EnableUpdate="True"
                                EnableDelete="True" EnableInsert="True" OnSelecting="GridDataSource_Selecting" >
                                <%--AutoGenerateOrderByClause="true"--%>
                                <WhereParameters>
                                    <%--<asp:Parameter Name="Inserted" Type="Boolean" DefaultValue="True" />
                                    <asp:ControlParameter ControlID="itbNo" Name="ID" Type="Int32" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="ddlCompanies" Name="ID_Company" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                    <asp:ControlParameter ControlID="ddlTypes" Name="TypeCode" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                    <asp:ControlParameter ControlID="ddlItemTypes" Name="ItemTypeCode" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                    <asp:ControlParameter ControlID="ddlSuppliers" Name="SupplierCode" Type="Int32" PropertyName="SelectedValue"
                                        DefaultValue="" />--%>
                                </WhereParameters>
                                <%--<OrderByParameters>
                                    <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                                        DefaultValue="" />
                                </OrderByParameters>--%>
                            </asp:LinqDataSource>
                            <%--<WhereParameters>
                        <asp:QueryStringParameter Name="ID" QueryStringField="P0" Type="Int32" />
                    </WhereParameters>--%>
                            <asp:GridView ID="grdPickingItems" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                DataSourceID="GridDataSource" DataKeyNames="ID" CssClass="gridview" OnPageIndexChanging="grdPickingItems_PageIndexChanging"
                                OnDataBound="OnGridViewDataBound" OnRowDataBound="grdPickingItems_RowDataBound"
                                OnRowDeleted="grdPickingItems_RowDeleted" OnRowCommand="grdPickingItems_RowCommand">
                                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
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
                                            <asp:Label ID="lblBelongsToMacroItem" runat="server" Text="Macrovoci"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Image ID="dycBelongsToMacroItem" runat="server" ImageUrl="~/Images/exclamation.png"
                                                ToolTip='<%# Eval("DependantMacroItems") %>' Visible='<%# Eval("BelongsToMacroItem") %>' />
                                            <%--ToolTip="Voce presente in una o più macrovoci, non modificarne la natura!" Visible='<%# Eval("BelongsToMacroItem") %>' />--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <EditItemTemplate>
                                            <asp:Image ID="dycBelongsToMacroItem" runat="server" ImageUrl="~/Images/exclamation.png"
                                                ToolTip="Voce presente in una o più macrovoci, non modificarne la natura!" Visible='<%# Eval("BelongsToMacroItem") %>' />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:Image ID="dycBelongsToMacroItem" runat="server" ImageUrl="~/Images/exclamation.png"
                                                ToolTip="Voce presente in una o più macrovoci, non modificarne la natura!" Visible='<%# Eval("BelongsToMacroItem") %>' />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCompany" runat="server" Text="Azienda"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey" CssClass="bold" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit"
                                                Mode="Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit"
                                                Mode="Insert" />
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
                                            <asp:DynamicControl ID="dycType" runat="server" DataField="Type" UIHint="ForeignKey_Edit"
                                                Mode="Insert" />
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
                                            <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemType" UIHint="ForeignKey_Edit"
                                                Mode="Insert" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblItemDescription" runat="server" Text="Descrizione voce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="ItemDescription"
                                                UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="ItemDescription"
                                                UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="ItemDescription"
                                                UIHint="Text_Edit" Mode="Insert" />
                                        </InsertItemTemplate>
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
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycSupplier" runat="server" DataField="Supplier" UIHint="ForeignKey_Edit"
                                                Mode="Insert" />
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
                                            <asp:DynamicControl ID="dycUM" runat="server" DataField="Unit" UIHint="ForeignKey_Edit"
                                                Mode="Insert" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCost" runat="server" Text="Costo €"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit"
                                                Mode="Insert" />
                                        </InsertItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
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
                                    <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPercentage" runat="server" Text="% Ric std"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Text_Edit"
                                                Mode="Insert" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblStandardPercentage" runat="server" Text="Tipo ric std"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycStandardPercentage" runat="server" DataField="StandardPercentage"
                                                UIHint="ForeignKey" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycStandardPercentage" runat="server" DataField="StandardPercentage"
                                                UIHint="ForeignKey_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycStandardPercentage" runat="server" DataField="StandardPercentage"
                                                UIHint="ForeignKey_Edit" Mode="Insert" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPercentageAuto" runat="server" Text="% Ric std"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPercentageAuto" runat="server" DataField="PercentageAuto"
                                                UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycPercentageAuto" runat="server" DataField="PercentageAuto"
                                                UIHint="Text" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycPercentageAuto" runat="server" DataField="PercentageAuto"
                                                UIHint="Text" Mode="Insert" />
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
                                            <asp:Label ID="lblLink" runat="server" Text="Dipendenza da altra voce"></asp:Label>
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
                                            <asp:Label ID="lblMILink" runat="server" Text="Dipendenza da macrovoce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycMILink" runat="server" DataField="MILink" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycMILink" runat="server" DataField="MILink" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycMILink" runat="server" DataField="MILink" UIHint="Text_Edit"
                                                Mode="Insert" />
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
                            <asp:DynamicControl ID="dycTemplate" runat="server" DataField="Template" UIHint="Integer_Edit"
                                Mode="Insert" />
                        </InsertItemTemplate>
                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblItemDisplayMode" runat="server" Text="Visualizza"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycItemDisplayMode" runat="server" DataField="ItemDisplayMode"
                                                UIHint="ForeignKey" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycItemDisplayMode" runat="server" DataField="ItemDisplayMode"
                                                UIHint="ForeignKey_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycItemDisplayMode" runat="server" DataField="ItemDisplayMode"
                                                UIHint="ForeignKey_Edit" Mode="Insert" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
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
            <asp:LinqDataSource ID="CalculatedDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                TableName="SPPickingItemsCalculated" OnSelecting="CalculatedDataSource_Selecting"
                AutoGenerateWhereClause="true">
                <WhereParameters>
                    <asp:Parameter Name="Inserted" DefaultValue="true" Type="Boolean" />
                    <asp:ControlParameter ControlID="ddlCompanies" Name="ID_Company" Type="Int32" PropertyName="SelectedValue"
                        DefaultValue="" />
                    <asp:ControlParameter ControlID="ddlTypes" Name="TypeCode" Type="Int32" PropertyName="SelectedValue"
                        DefaultValue="" />
                    <asp:ControlParameter ControlID="ddlItemTypes" Name="ItemTypeCode" Type="Int32" PropertyName="SelectedValue"
                        DefaultValue="" />
                    <asp:ControlParameter ControlID="ddlSuppliers" Name="SupplierCode" Type="Int32" PropertyName="SelectedValue"
                        DefaultValue="" />
                </WhereParameters>
                <OrderByParameters>
                    <asp:ControlParameter ControlID="ddlOrderBy" Name="OrderBy" Type="String" PropertyName="SelectedValue"
                        DefaultValue="" />
                </OrderByParameters>
            </asp:LinqDataSource>
            <asp:GridView ID="grdCalculated" runat="server" AllowPaging="True" CssClass="gridview"
                DataSourceID="CalculatedDataSource" OnPageIndexChanging="grdCalculated_PageIndexChanging"
                AutoGenerateColumns="false">
                <RowStyle CssClass="row" />
                <AlternatingRowStyle CssClass="altRow" />
                <Columns>
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
                            <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dynID" runat="server" DataField="ID" UIHint="Text" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblInserted" runat="server" Text="Inserisci"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycInserted" runat="server" DataField="Inserted" UIHint="Boolean" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblCompany" runat="server" Text="Azienda"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="CompanyDescription" UIHint="Text" />
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
                            <asp:DynamicControl ID="dycItemType" runat="server" DataField="ItemTypeDescription"
                                UIHint="Text" />
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
                            <asp:Label ID="lblCost" runat="server" Text="Costo"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblMultiply" runat="server" Text="Moltiplica"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblPercentage" runat="server" Text="% Ric std"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Text"
                                ItemStyle-HorizontalAlign="Right" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblPercentageAuto" runat="server" Text="Tipo ric std"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycPercentageAuto" runat="server" DataField="PercentageAuto"
                                UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblPrice" runat="server" Text="Prezzo"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycPrice" runat="server" DataField="Price" UIHint="Text" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblDate" runat="server" Text="Data"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime"
                                DataFormatString="{0:d}" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblLink" runat="server" Text="Dipendenza"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycLink" runat="server" DataField="Link" UIHint="Text" />
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
                            <asp:Label ID="lblTemplate" runat="server" Text="Modello"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycTemplate" runat="server" DataField="Template" UIHint="Integer" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblItemManufacturing" runat="server" Text="Produzione"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycItemManufacturing" runat="server" DataField="ItemManufacturing"
                                UIHint="Integer" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
