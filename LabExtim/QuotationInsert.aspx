<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="QuotationInsert.aspx.cs"
         Inherits="LabExtim.QuotationInsert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <h2>
        Inserisci nuovo Preventivo</h2>
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
                            <asp:Label ID="lblDate" runat="server" Text="Data creazione" Width="120"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime_Edit"/>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblSubject" runat="server" Text="Descrizione"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycSubject" runat="server" DataField="Subject" UIHint="Text" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycSubject" runat="server" DataField="Subject" UIHint="Text250_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycSubject" runat="server" DataField="Subject" UIHint="Text250_Edit" />
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
                            <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycCustomere" runat="server" DataField="Customer" UIHint="ForeignKey" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycCustomere" runat="server" DataField="Customer" UIHint="ForeignKey_Edit" />
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblMarkUp" runat="server" Text="% Ric com"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Text" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Integer_Edit" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Integer_Edit" />
                        </InsertItemTemplate>
                    </asp:TemplateField>
                </Fields>
            </asp:DetailsView>
            <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableInsert="true" OnInserted="DetailsDataSource_Inserted"
                                ContextTypeName="DLLabExtim.QuotationDataContext" TableName="Quotations">
            </asp:LinqDataSource>
            <br />
            <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>