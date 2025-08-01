<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="QuotationPrint.aspx.cs" Inherits="LabExtim.QuotationPrint" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $('#ctl00_ContentPlaceHolder1_crvQuotation').contents().find('iframe').contents().find('head').append("<style>.CRReverseText{color: white!important;background-color: gray;}</style>");
        });
    </script>

    <br />
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <h2>Stampa Preventivo No.
        <%= QuotationHeader.Key %>
        (<%= QuotationHeader.Value %>) &nbsp;/&nbsp; Ordine di produzione No.
        <%= ProductionOrderHeader.Key %>
        (<%= ProductionOrderHeader.Value %>)
    </h2>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <asp:Panel ID="DetailsPanel" runat="server">
        <table>
            <tr>
                <td valign="top">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                        HeaderText="Elenco degli errori di validazione" />
                    <asp:DynamicValidator runat="server" ID="DynamicValidator1" ControlToValidate="dtvQuotation"
                        Display="None" />
                    <asp:DetailsView ID="dtvQuotation" runat="server" FieldHeaderStyle-CssClass="bold"
                        AutoGenerateEditButton="true" DefaultMode="ReadOnly" CssClass="detailstable"
                        AutoGenerateRows="false" DataSourceID="DetailsDataSource" OnItemCreated="dtvQuotation_ItemCreated" OnItemUpdated="dtvQuotation_ItemUpdated">
                        <FieldHeaderStyle CssClass="bold" />
                        <Fields>
                            <asp:DynamicField DataField="ID" HeaderText="No Preventivo" />
                            <asp:DynamicField DataField="Customer" HeaderText="Cliente originario" UIHint="ForeignKey_RO" />
                            <asp:BoundField DataField="Subject" HeaderText="Descrizione" ReadOnly="true" />
                            <asp:BoundField DataField="Date" HeaderText="Data creazione" ReadOnly="true" />



                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQ1" runat="server" Text="Quantità 1"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtQ1" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                        Text='<%# Eval("Q1") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP1" runat="server" Checked='<%# Eval("P1") ?? false %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txtQ1" runat="server" Width="40" MaxLength="6" CssClass="droplist" Text='<%# Eval("Q1") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP1" runat="server" DataField="P1" UIHint="Boolean_Edit" Mode="Edit" />
                                </EditItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQ2" runat="server" Text="Quantità 2"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtQ2" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                        Text='<%# Eval("Q2") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP2" runat="server" Checked='<%# Eval("P2") ?? false %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txtQ2" runat="server" Width="40" MaxLength="6" CssClass="droplist" Text='<%# Eval("Q2") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP2" runat="server" DataField="P2" UIHint="Boolean_Edit" Mode="Edit" />
                                </EditItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQ3" runat="server" Text="Quantità 3"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtQ3" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                        Text='<%# Eval("Q3") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP3" runat="server" Checked='<%# Eval("P3") ?? false %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txtQ3" runat="server" Width="40" MaxLength="6" CssClass="droplist" Text='<%# Eval("Q3") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP3" runat="server" DataField="P3" UIHint="Boolean_Edit" Mode="Edit" />
                                </EditItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQ4" runat="server" Text="Quantità 4"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtQ4" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                        Text='<%# Eval("Q4") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP4" runat="server" Checked='<%# Eval("P4") ?? false %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txtQ4" runat="server" Width="40" MaxLength="6" CssClass="droplist" Text='<%# Eval("Q4") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP4" runat="server" DataField="P4" UIHint="Boolean_Edit" Mode="Edit" />
                                </EditItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblQ5" runat="server" Text="Quantità 5"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtQ5" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                        Text='<%# Eval("Q5") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP5" runat="server" Checked='<%# Eval("P5") ?? false %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="txtQ5" runat="server" Width="40" MaxLength="6" CssClass="droplist" Text='<%# Eval("Q5") %>'></asp:Label>
                                    &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP5" runat="server" DataField="P5" UIHint="Boolean_Edit" Mode="Edit" />
                                </EditItemTemplate>

                            </asp:TemplateField>




                        </Fields>
                    </asp:DetailsView>
                    <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableDelete="false" EnableInsert="false"
                        EnableUpdate="true" TableName="Quotations" ContextTypeName="DLLabExtim.QuotationDataContext">
                        <WhereParameters>
                            <asp:QueryStringParameter Name="ID" QueryStringField="P0" Type="Int32" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                    <br />
                    <asp:LinkButton ID="lbtReportTextsConfigure" runat="server" CssClass="gridview" Font-Bold="False"
                        Text="Configura testi report" />&nbsp;
                    <asp:ImageButton ID="ibtUpdate" runat="server" ImageUrl="~/Images/arrow_refresh.png"
                        ToolTip="Aggiorna testi" OnClick="ibtUpdate_Click" />
                    <br />
                    <asp:LinkButton ID="lbtExportPDF" runat="server" CssClass="gridview" Font-Bold="True"
                        OnClick="lbtExportPDF_Click" Text="Stampa in PDF" Font-Size="Medium" />
                    <br />
                    <asp:LinkButton ID="lbtExportExcel" runat="server" CssClass="gridview" Font-Bold="False"
                        OnClick="lbtExportExcel_Click" Text="Esporta in Excel" />
                    <br />
                    <asp:LinkButton ID="lbtExportWord" runat="server" CssClass="gridview" Font-Bold="False"
                        OnClick="lbtExportWord_Click" Text="Esporta in Word" />
                    <br />
                    <asp:HyperLink ID="hypQuotationConsole" runat="server" CssClass="template" Font-Bold="False"
                        Text="Torna a Console Preventivo" />
                    <br />
                    <br />
                    <asp:Repeater ID="PrintersList" runat="server" OnItemCommand="PrintersList_ItemCommand" OnItemDataBound="PrintersList_ItemDataBound">
                        <HeaderTemplate>
                            <h2>Seleziona la stampante (lato server)</h2>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="Printer" runat="server"></asp:LinkButton><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td valign="top" align="left">
                    <CR:CrystalReportViewer ID="crvQuotation" runat="server" AutoDataBind="True" EnableDatabaseLogonPrompt="False"
                        EnableParameterPrompt="False" HasCrystalLogo="False" HasExportButton="False"
                        HasPrintButton="False" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
