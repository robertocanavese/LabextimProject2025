<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="QuotationTemplateConsole.aspx.cs" Inherits="LabExtim.QuotationTemplateConsole" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <h2>
        <%--<asp:Label ID="lblQuotationHeader" runat="server" Text='Gestione Preventivo No. <% QuotationHeader.Key %> (<%QuotationHeader.Value %>)'></asp:Label>--%>
        <asp:Label ID="lblQuotationHeader" runat="server"></asp:Label>
    </h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="DetailsPanel" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td colspan="3">
                            <asp:Menu ID="mnuOperations1" runat="server" Orientation="Horizontal" CssClass="menu"
                                DisappearAfter="1000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                DynamicHorizontalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                                <DynamicMenuStyle CssClass="menu" />
                            </asp:Menu>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Menu ID="mnuOperations2" runat="server" Orientation="Horizontal" CssClass="menuBlue"
                                DisappearAfter="1000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                DynamicHorizontalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick" >
                                <DynamicMenuStyle CssClass="menu" />
                            </asp:Menu>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Menu ID="mnuQuotationTemplatesAdd" runat="server" Orientation="Horizontal" CssClass="menuYellow"
                                DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuQuotationTemplatesAdd_MenuItemClick">
                                <DynamicMenuStyle CssClass="menuYellow" />
                            </asp:Menu>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                HeaderText="Elenco degli errori di validazione" />
                            <asp:DynamicValidator runat="server" ID="DynamicValidator1" ControlToValidate="dtvQuotation"
                                Display="None" />
                            <asp:DetailsView ID="dtvQuotation" runat="server" FieldHeaderStyle-CssClass="bold"
                                AutoGenerateEditButton="true" DefaultMode="ReadOnly" CssClass="detailstable"
                                AutoGenerateRows="false" DataSourceID="DetailsDataSource" OnItemCreated="dtvQuotation_ItemCreated"
                                OnItemUpdated="dtvQuotation_ItemUpdated">
                                <FieldHeaderStyle CssClass="bold" />
                                <Fields>
                                    <%--<asp:DynamicField DataField="ID" UIHint="Text" />
                                    
                                    <asp:DynamicField DataField="Subject" />
                                    --%>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text="Descrizione"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text250_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycDescription" runat="server" DataField="Description" UIHint="Text250_Edit" />
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
                                            <asp:Label ID="lblQ1" runat="server" Text="Quantità 1"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycQ1" runat="server" DataField="Q1" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycQ1" runat="server" DataField="Q1" UIHint="Integer_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycQ1" runat="server" DataField="Q1" UIHint="Integer_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQ2" runat="server" Text="Quantità 2"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycQ2" runat="server" DataField="Q2" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycQ2" runat="server" DataField="Q2" UIHint="Integer_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycQ2" runat="server" DataField="Q2" UIHint="Integer_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQ3" runat="server" Text="Quantità 3"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycQ3" runat="server" DataField="Q3" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycQ3" runat="server" DataField="Q3" UIHint="Integer_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycQ3" runat="server" DataField="Q3" UIHint="Integer_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQ4" runat="server" Text="Quantità 4"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycQ4" runat="server" DataField="Q4" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycQ4" runat="server" DataField="Q4" UIHint="Integer_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycQ4" runat="server" DataField="Q4" UIHint="Integer_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblQ5" runat="server" Text="Quantità 5"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycQ5" runat="server" DataField="Q5" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycQ5" runat="server" DataField="Q5" UIHint="Integer_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycQ5" runat="server" DataField="Q5" UIHint="Integer_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField>
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
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text="Data creazione"></asp:Label>
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
                                </Fields>
                            </asp:DetailsView>
                        </td>
                    </tr>
                </table>
                <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableDelete="false" EnableInsert="false"
                    EnableUpdate="true" TableName="QuotationTemplates" ContextTypeName="DLLabExtim.QuotationDataContext">
                    <WhereParameters>
                        <asp:QueryStringParameter Name="ID" QueryStringField="P2" Type="Int32" />
                    </WhereParameters>
                </asp:LinqDataSource>
            </asp:Panel>
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdQuotationDetails"
                Display="None" />
            &nbsp;<asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label><h2>Dettaglio&nbsp;
                <asp:LinkButton ID="lbtViewInputItems" runat="server" CssClass="gridview" Font-Bold="False"
                    OnClick="lbtViewInputItems_Click" Text="Visualizza voci di input" Visible="false" />
                &nbsp;
                <%--<asp:LinkButton ID="lbtViewCalculation" runat="server" CssClass="gridview" Font-Bold="False"
                    Font-Size="Small" OnClick="lbtViewCalculation_Click" Text="Visualizza calcolo" />
                &nbsp;
                <asp:LinkButton ID="lbtViewResults" runat="server" CssClass="gridview" Font-Bold="False"
                    Font-Size="Small" OnClick="lbtResultsCalculation_Click" Text="Visualizza risultati" />
                &nbsp;--%>

                <asp:LinkButton ID="lbtPrintQuotationTechnical" runat="server" CssClass="gridview"
                    Font-Bold="False" OnClick="lbtPrintQuotationTechnical_Click"
                    Text="Stampa modello di preventivo tecnico" Visible="false" />
                &nbsp;
            </h2>
            <table>
                <tr>

                    <td>
                         <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" EnableUpdate="true"
                ContextTypeName="DLLabExtim.QuotationDataContext" TableName="QuotationTemplateDetails"
                OnDeleted="GridDataSource_Deleted" OnSelecting="GridDataSource_Selecting">
            </asp:LinqDataSource>
            <%--<WhereParameters>
                        <asp:QueryStringParameter Name="ID" QueryStringField="P0" Type="Int32" />
                    </WhereParameters>--%>
            <asp:GridView ID="grdQuotationDetails" runat="server" AutoGenerateColumns="false"
                AutoGenerateDeleteButton="true" AllowPaging="True" DataSourceID="GridDataSource"
                AllowSorting="false" CssClass="gridview" OnPageIndexChanging="grdQuotationDetails_PageIndexChanging"
                OnRowDataBound="grdQuotationDetails_RowDataBound" OnPreRender="grdQuotationDetails_PreRender"
                OnRowCommand="grdQuotationDetails_RowCommand" OnRowCreated="grdQuotationDetails_RowCreated">
                <%--AutoGenerateSelectButton="True" AutoGenerateEditButton="true"--%>
                <RowStyle CssClass="row" />
                <AlternatingRowStyle CssClass="altRow" />
                <Columns>
                    <asp:DynamicField DataField="ID" HeaderText="ID" UIHint="Text" Visible="false" />
                    <%--<asp:DynamicField DataField="Inserted" HeaderText="Incluso" UIHint="Boolean" />
                    <asp:DynamicField DataField="SelectPhase" HeaderText="Fase" UIHint="Boolean" />--%>
                    <%--<asp:DynamicField DataField="Position" HeaderText="Posizione" UIHint="Integer" />--%>
                    <asp:DynamicField DataField="Company" HeaderText="Azienda" UIHint="ForeignKey" ItemStyle-Font-Bold="true"  />
                    <asp:DynamicField DataField="Type" HeaderText="Tipo fase" UIHint="ForeignKey" />
                    <asp:DynamicField DataField="ItemType" HeaderText="Tipo prodotto" UIHint="ForeignKey" />
                    <asp:DynamicField DataField="ItemTypeDescription" HeaderText="Descrizione prodotto"
                        UIHint="Text" />
                    <asp:DynamicField DataField="Unit" HeaderText="UM" UIHint="ForeignKey" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblQuantity" Text="Quantità" runat="server" ToolTip="Inserire un valore intero, decimale con virgola o frazionario"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtQuantity" runat="server" Width="50" MaxLength="6" CssClass="droplist"
                                AutoPostBack="True"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:DynamicField DataField="Quantity" HeaderText="Quantità" UIHint="Integer_Edit" />--%>
                    <asp:DynamicField DataField="Cost" HeaderText="Cst Uni" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                    <asp:DynamicField DataField="Multiply" HeaderText="Moltiplica" UIHint="Boolean" />
                    <asp:DynamicField DataField="Percentage" HeaderText="% Ric std" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                    <asp:DynamicField DataField="MarkUp" HeaderText="% Ric com" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                    <asp:DynamicField DataField="Price" HeaderText="Prz Uni" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                    <asp:DynamicField DataField="Supplier" HeaderText="Fornitore" UIHint="ForeignKey" />
                    <%--<asp:DynamicField DataField="Save" HeaderText="Salva" UIHint="Boolean" />
                    <asp:DynamicField DataField="CommonKey" HeaderText="Chiave" UIHint="Text" />--%>
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
                    </td>
                    <td>
                       
                <asp:Label runat="server" Text="Abbandona e carica altro modello" />
                <br />
                <asp:Menu ID="mnuQuotationTemplates" runat="server" Orientation="Vertical" CssClass="menu"
                    DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                    DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuQuotationTemplates_MenuItemClick">
                    <DynamicMenuStyle CssClass="menu" />
                </asp:Menu>
                                </td>
                </tr>
            </table>
            <%--<br />
                            <asp:LinkButton ID="lbtLoadTemplate" runat="server" Text="Svuota e carica altro modello"
                                OnClick="lbtLoadTemplate_Click" />
                            <br />
                            <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="droplist" Width="250px"
                                OnDataBound="ddlTemplates_DataBound">
                            </asp:DropDownList>--%>

           
            <%--<asp:LinqDataSource ID="CalculatedDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                TableName="SPQDetailCalculations" OnSelecting="CalculatedDataSource_Selecting">
            </asp:LinqDataSource>
            <asp:GridView ID="grdQuotationCalculatedDetails" runat="server" AllowPaging="True"
                CssClass="gridview" DataSourceID="CalculatedDataSource" OnPageIndexChanging="grdQuotationCalculatedDetails_PageIndexChanging"
                AutoGenerateColumns="false" OnPreRender="grdQuotationCalculatedDetails_PreRender">
                <Columns>
                    <asp:BoundField DataField="Position" HeaderText="Pos" />
                    <asp:BoundField DataField="TypeDescription" HeaderText="Categoria" />
                    <asp:BoundField DataField="ItemTypeDescription" HeaderText="Tipo prodotto" />
                    <asp:BoundField DataField="UnitDescription" HeaderText="UM" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantità" />
                    <asp:BoundField DataField="Cost" HeaderText="Cst Uni" />
                    <asp:BoundField DataField="Percentage" HeaderText="Ric std" />
                    <asp:BoundField DataField="MarkUp" HeaderText="Ric com" />
                    <asp:BoundField DataField="Price" HeaderText="Prz Uni" />
                    <asp:BoundField DataField="TotalC" HeaderText="Cst Tot" ItemStyle-Font-Bold="true"/>
                    <asp:BoundField DataField="TotalP" HeaderText="Prz Tot" ItemStyle-Font-Bold="true"/>
                    <asp:DynamicField DataField="Multiply" HeaderText="Mult" UIHint="Boolean" />
                    <asp:DynamicField DataField="SelectPhase" HeaderText="Sel Fase" UIHint="Boolean" />
                    <asp:BoundField DataField="PhaseCost" HeaderText="Costo Fase" ItemStyle-Font-Bold="true"/>
                    <asp:BoundField DataField="PhasePrice" HeaderText="Prezzo Fase" ItemStyle-Font-Bold="true"/>
                    <asp:BoundField DataField="SupplierCode" HeaderText=" Cod Fornitore" />
            <asp:BoundField DataField="Q1" HeaderText="Qtà Rif" />
            <asp:BoundField DataField="Cost1" HeaderText="Cst Tot Rif" />
                    <asp:BoundField DataField="Price1" HeaderText="Prz Tot Rif" />
            <asp:BoundField DataField="ProdC1" HeaderText="Cst Prod Rif" ItemStyle-Font-Bold="true" />
            <asp:BoundField DataField="ProdV1" HeaderText="Prz Prod Rif" ItemStyle-Font-Bold="true" />
            <asp:BoundField DataField="PricePCT1" HeaderText="% ric Prz 1" />
            <asp:BoundField DataField="Q2" HeaderText="Qtà 2" />
                    <asp:BoundField DataField="Cost1" HeaderText="Cst Tot 2" />
                    <asp:BoundField DataField="Price1" HeaderText="Prz Tot 2" />
                    <asp:BoundField DataField="Batt2" HeaderText="Batt 2" />
                    <asp:BoundField DataField="ProdC2" HeaderText="Cst Prod 2" />
                    <asp:BoundField DataField="ProdV2" HeaderText="Prz Prod 2" />
                    <asp:BoundField DataField="PricePCT2" HeaderText="% ric Prz 2" />
                    <asp:BoundField DataField="Q3" HeaderText="Qtà 3" />
                    <asp:BoundField DataField="Cost3" HeaderText="Cst Tot 3" />
                    <asp:BoundField DataField="Price3" HeaderText="Prz Tot 3" />
                    <asp:BoundField DataField="Batt3" HeaderText="Batt 3" />
                    <asp:BoundField DataField="ProdC3" HeaderText="Cst Prod 3" />
                    <asp:BoundField DataField="ProdV3" HeaderText="Prz Prod 3" />
                    <asp:BoundField DataField="PricePCT3" HeaderText="% ric Prz 3" />
                    <asp:BoundField DataField="Q4" HeaderText="Qtà 4" />
                    <asp:BoundField DataField="Cost4" HeaderText="Cst Tot 4" />
                    <asp:BoundField DataField="Price4" HeaderText="Prz Tot 4" />
                    <asp:BoundField DataField="Batt4" HeaderText="Batt 4" />
                    <asp:BoundField DataField="ProdC4" HeaderText="Cst Prod 4" />
                    <asp:BoundField DataField="ProdV4" HeaderText="Prz Prod 4" />
                    <asp:BoundField DataField="PricePCT4" HeaderText="% ric Prz 4" />
                    <asp:BoundField DataField="Q5" HeaderText="Qtà 5" />
                    <asp:BoundField DataField="Cost5" HeaderText="Cst Tot 5" />
                    <asp:BoundField DataField="Price5" HeaderText="Prz Tot 5" />
                    <asp:BoundField DataField="Batt5" HeaderText="Batt 5" />
                    <asp:BoundField DataField="ProdC5" HeaderText="Cst Prod 5" />
                    <asp:BoundField DataField="ProdV5" HeaderText="Prz Prod 5" />
                    <asp:BoundField DataField="PricePCT5" HeaderText="% ric Prz 5" />
                </Columns>
                <PagerStyle CssClass="footer" />
                <SelectedRowStyle CssClass="selected" />
                <PagerTemplate>
                    <asp:GridViewPager ID="Pager1" runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    Nessuna voce trovata.
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:LinqDataSource ID="ResultDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                TableName="SPQDetailResults" OnSelecting="ResultDataSource_Selecting">
            </asp:LinqDataSource>
            <asp:GridView ID="grdQuotationResults" runat="server" AllowPaging="True" DataSourceID="ResultDataSource"
                CssClass="gridview" OnPageIndexChanging="grdQuotationResults_PageIndexChanging"
                AutoGenerateColumns="false" OnPreRender="grdQuotationResults_PreRender">
                <Columns>
                    <asp:BoundField DataField="TypeDescription" HeaderText="Categoria" />
                    <asp:BoundField DataField="ItemTypeDescription" HeaderText="Tipo prodotto" />
                    <asp:BoundField DataField="Q1" HeaderText="Qtà Rif" ItemStyle-Font-Bold="true"/>
                    <asp:BoundField DataField="ProdC1" HeaderText="Cst Prod Rif" />
                    <asp:BoundField DataField="ProdV1" HeaderText="Prz Prod Rif" />
                    
                    <asp:BoundField DataField="Cost1" HeaderText="Cst Tot Rif" />
                    <asp:BoundField DataField="Price1" HeaderText="Prz Tot Rif" ItemStyle-Font-Bold="true"/>
                    
                    <asp:BoundField DataField="PricePCT1" HeaderText="% ric Prz Rif" />
                    <asp:BoundField DataField="Q2" HeaderText="Qtà 2" ItemStyle-Font-Bold="true"/>
                    <asp:BoundField DataField="ProdC2" HeaderText="Cst Prod 2" />
                    <asp:BoundField DataField="ProdV2" HeaderText="Prz Prod 2" />
                    
                    <asp:BoundField DataField="Cost2" HeaderText="Cst Tot 2" />
                    <asp:BoundField DataField="Price2" HeaderText="Prz Tot 2" ItemStyle-Font-Bold="true"/>
                    
                    <asp:BoundField DataField="PricePCT2" HeaderText="% ric Prz 2" />
                    <asp:BoundField DataField="Q3" HeaderText="Qtà 3" ItemStyle-Font-Bold="true"/>
                    <asp:BoundField DataField="ProdC3" HeaderText="Cst Prod 3" />
                    <asp:BoundField DataField="ProdV3" HeaderText="Prz Prod 3" />
                    
                    <asp:BoundField DataField="Cost3" HeaderText="Cst Tot 3" />
                    <asp:BoundField DataField="Price3" HeaderText="Prz Tot 3" ItemStyle-Font-Bold="true"/>
                    
                    <asp:BoundField DataField="PricePCT3" HeaderText="% ric Prz 3" />
                    <asp:BoundField DataField="Q4" HeaderText="Qtà 4" ItemStyle-Font-Bold="true"/>
                    <asp:BoundField DataField="ProdC4" HeaderText="Cst Prod 4" />
                    <asp:BoundField DataField="ProdV4" HeaderText="Prz Prod 4" />
                    
                    <asp:BoundField DataField="Cost4" HeaderText="Cst Tot 4" />
                    <asp:BoundField DataField="Price4" HeaderText="Prz Tot 4" ItemStyle-Font-Bold="true"/>
                    
                    <asp:BoundField DataField="PricePCT4" HeaderText="% ric Prz 4" />
                    <asp:BoundField DataField="Q5" HeaderText="Qtà 5" ItemStyle-Font-Bold="true"/>
                    <asp:BoundField DataField="ProdC5" HeaderText="Cst Prod 5" />
                    <asp:BoundField DataField="ProdV5" HeaderText="Prz Prod 5" />
                    
                    <asp:BoundField DataField="Cost5" HeaderText="Cst Tot 5" />
                    <asp:BoundField DataField="Price5" HeaderText="Prz Tot 5" ItemStyle-Font-Bold="true"/>
                    
                    <asp:BoundField DataField="PricePCT5" HeaderText="% ric Prz 5" />
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager ID="Pager2" runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    Nessuna voce trovata.
                </EmptyDataTemplate>
            </asp:GridView>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
