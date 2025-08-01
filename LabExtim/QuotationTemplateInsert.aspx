<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="QuotationTemplateInsert.aspx.cs"
         Inherits="LabExtim.QuotationTemplateInsert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <h2>
        Inserisci nuovo Modello di Preventivo</h2>
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                   HeaderText="Elenco degli errori di validazione" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1"
                                  Display="None" />
            <asp:DetailsView ID="DetailsView1" runat="server" DataSourceID="DetailsDataSource"
                             DefaultMode="Insert" AutoGenerateInsertButton="True" OnItemCommand="DetailsView1_ItemCommand"
                             OnItemInserted="DetailsView1_ItemInserted" CssClass="detailstable" FieldHeaderStyle-CssClass="bold"
                             OnItemInserting="DetailsView1_ItemInserting" AutoGenerateRows="false">
                <FieldHeaderStyle CssClass="bold" />
                <RowStyle CssClass="selected" />
                <Fields>
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
            <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableInsert="true" OnInserted="DetailsDataSource_Inserted"
                                ContextTypeName="DLLabExtim.QuotationDataContext" TableName="QuotationTemplates">
            </asp:LinqDataSource>
            <br />
            <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>