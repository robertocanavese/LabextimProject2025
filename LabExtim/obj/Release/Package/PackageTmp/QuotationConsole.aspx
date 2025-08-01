<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True"
    CodeBehind="QuotationConsole.aspx.cs" Inherits="LabExtim.QuotationConsole" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        function SetSelectedQuantity() {
            re = new RegExp("dtvQuotation_rbtQ");
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i];
                if (elm.type == 'radio') {
                    if (re.test(elm.id)) {
                        if (elm.checked == true) {
                            quantity = document.getElementById(elm.id.replace("rbt", "txt")).value;
                            break;
                        }
                    }
                }
            }
            return quantity;
        }

        function SetParametersAndOpenPopUp() {
            OpenItem('ProductionOrderPopup.aspx?' +
                    'POquo=' + '<% = QuotationParameter %>' + '&' +
                    'POName=' + '<% = QuotationHeader.Value %>' + '&' +
                    'POcid=0&' +
                    'POq=' + SetSelectedQuantity());
            return false;
        }
        //   'POcid=' + '<% = GetQuotationCustomerID().ToString() %>' + '&' +
    </script>

    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <%--<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />--%>
    <%--<h2>--%>
    <table style="width: 100%">
        <tr>
            <td class="h2" style="width: 85%">
                <asp:Label ID="lblQuotationHeader" runat="server"></asp:Label>
            </td>
            <td style="width: 15%; text-align: right">
                <asp:LinkButton ID="lbtNewQuotation" runat="server" Text="Nuovo preventivo" OnClick="lbtNewQuotation_Click"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <%--</h2>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <table width="900">
                <tr>
                    <td>
                        <asp:Panel ID="DetailsPanel" runat="server">
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="3">
                                        <asp:Menu ID="mnuOperations1" runat="server" Orientation="Horizontal" CssClass="menu"
                                            DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                            DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                                            <DynamicMenuStyle CssClass="menu" />
                                        </asp:Menu>
                                        <asp:Menu ID="mnuOperations2" runat="server" Orientation="Horizontal" CssClass="menuBlue"
                                            DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                            DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                                            <DynamicMenuStyle CssClass="menu" />
                                        </asp:Menu>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 41%">
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                            HeaderText="Elenco degli errori di validazione" />
                                        <asp:DynamicValidator runat="server" ID="DynamicValidator1" ControlToValidate="dtvQuotation"
                                            Display="None" />
                                        <asp:DetailsView ID="dtvQuotation" runat="server" FieldHeaderStyle-CssClass="bold"
                                            AutoGenerateEditButton="true" DefaultMode="ReadOnly" CssClass="detailstable"
                                            AutoGenerateRows="false" DataSourceID="DetailsDataSource" OnItemCreated="dtvQuotation_ItemCreated"
                                            Width="270" OnItemUpdated="dtvQuotation_ItemUpdated" OnDataBound="dtvQuotation_DataBound"
                                            OnItemUpdating="dtvQuotation_ItemUpdating">
                                            <FieldHeaderStyle CssClass="bold" />
                                            <Fields>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text="Data creazione" Width="120"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                                    </ItemTemplate>
                                                    <%--<EditItemTemplate>
                                                        <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime_Edit"
                                                            Mode="Edit" />
                                                    </EditItemTemplate>
                                                    <InsertItemTemplate>
                                                        <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime_Edit" />
                                                    </InsertItemTemplate>--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSubject" runat="server" Text="Titolo"></asp:Label>
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
                                                        <asp:Label ID="lblOwner" runat="server" Text="Utente"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DynamicControl ID="dycOwner" runat="server" DataField="Employee1" UIHint="ForeignKey" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DynamicControl ID="dycOwner" runat="server" DataField="Employee1" UIHint="ForeignKey_Edit"
                                                            Mode="Edit" />
                                                    </EditItemTemplate>
                                                    <InsertItemTemplate>
                                                        <asp:DynamicControl ID="dycOwner" runat="server" DataField="Employee1" UIHint="ForeignKey_Edit" />
                                                    </InsertItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblQ1" runat="server" Text="Quantità 1"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:DynamicControl ID="dycQ1" runat="server" DataField="Q1" UIHint="Text"  Mode="Edit"/>--%>
                                                        <asp:TextBox ID="txtQ1" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                                            Text='<%# Eval("Q1") %>'></asp:TextBox><%--AutoPostBack="True"--%>
                                                        <asp:RadioButton ID="rbtQ1" runat="server" Checked="true" GroupName="selectQuantity" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DynamicControl ID="dycQ1" runat="server" DataField="Q1" UIHint="Integer_Edit"
                                                            Mode="Edit" />
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
                                                        <%--<asp:DynamicControl ID="dycQ2" runat="server" DataField="Q2" UIHint="Text" />--%>
                                                        <asp:TextBox ID="txtQ2" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                                            Text='<%# Eval("Q2") %>'></asp:TextBox><%--AutoPostBack="True"--%>
                                                        <asp:RadioButton ID="rbtQ2" runat="server" GroupName="selectQuantity" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DynamicControl ID="dycQ2" runat="server" DataField="Q2" UIHint="Integer_Edit"
                                                            Mode="Edit" />
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
                                                        <%--<asp:DynamicControl ID="dycQ3" runat="server" DataField="Q3" UIHint="Text" />--%>
                                                        <asp:TextBox ID="txtQ3" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                                            Text='<%# Eval("Q3") %>'></asp:TextBox><%--AutoPostBack="True"--%>
                                                        <asp:RadioButton ID="rbtQ3" runat="server" GroupName="selectQuantity" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DynamicControl ID="dycQ3" runat="server" DataField="Q3" UIHint="Integer_Edit"
                                                            Mode="Edit" />
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
                                                        <%--<asp:DynamicControl ID="dycQ4" runat="server" DataField="Q4" UIHint="Text" />--%>
                                                        <asp:TextBox ID="txtQ4" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                                            Text='<%# Eval("Q4") %>'></asp:TextBox><%--AutoPostBack="True"--%>
                                                        <asp:RadioButton ID="rbtQ4" runat="server" GroupName="selectQuantity" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DynamicControl ID="dycQ4" runat="server" DataField="Q4" UIHint="Integer_Edit"
                                                            Mode="Edit" />
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
                                                        <%--<asp:DynamicControl ID="dycQ5" runat="server" DataField="Q5" UIHint="Text" />--%>
                                                        <asp:TextBox ID="txtQ5" runat="server" Width="40" MaxLength="6" CssClass="droplist"
                                                            Text='<%# Eval("Q5") %>'></asp:TextBox><%--AutoPostBack="True"--%>
                                                        <asp:RadioButton ID="rbtQ5" runat="server" GroupName="selectQuantity" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DynamicControl ID="dycQ5" runat="server" DataField="Q5" UIHint="Integer_Edit"
                                                            Mode="Edit" />
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
                                                        <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit"
                                                            Mode="Edit" />
                                                    </EditItemTemplate>
                                                    <InsertItemTemplate>
                                                        <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit" />
                                                    </InsertItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblMarkUp" runat="server" Text="% Ric com"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Text" />--%>
                                                        <asp:TextBox ID="txtMarkUp" runat="server" Width="40" MaxLength="3" CssClass="droplist"
                                                            Text='<%# Eval("MarkUp") %>' CausesValidation="true"></asp:TextBox><%--AutoPostBack="True"--%>
                                                        <asp:CustomValidator runat="server" ID="CustomValidator2" CssClass="droplist" ControlToValidate="txtMarkUp"
                                                            Text="Valore inferiore a 100" OnServerValidate="CustomValidator2_ServerValidate" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Integer_Edit"
                                                            Mode="Edit" />
                                                    </EditItemTemplate>
                                                    <InsertItemTemplate>
                                                        <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Integer_Edit" />
                                                    </InsertItemTemplate>
                                                </asp:TemplateField>
                                            </Fields>
                                        </asp:DetailsView>
                                    </td>
                                    <td style="width: 25%" valign="top" align="center">
                                        <table cellpadding="5">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnEmpty" runat="server" OnClick="btnEmpty_Click" Width="130px" Text="Svuota preventivo"
                                                        OnClientClick="return confirm('Sei sicuro di voler cancellare tutte le voci del preventivo?')" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lbtLoadTemplate" runat="server" Text="Svuota preventivo e carica modello"
                                                        OnClick="lbtLoadTemplate_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="droplist" Width="250px"
                                                        OnDataBound="ddlTemplates_DataBound">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lbtSaveAsTemplate" runat="server" Text="Salva preventivo come modello"
                                                        OnClick="lbtSaveAsTemplate_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTemplateDescription" runat="server" Text="Inserire una descrizione per il nuovo modello"></asp:Label>
                                                    <asp:TextBox ID="txtTemplateDescription" runat="server" CssClass="gridview" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <asp:CustomValidator runat="server" ID="CustomValidator1" CssClass="droplist" ControlToValidate="txtTemplateDescription"
                                                Text="Valore già esistente" OnServerValidate="CustomValidator1_ServerValidate" />
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSaveAs" runat="server" Text="Inserire la descrizione (salva con nome)"></asp:Label>
                                                    <asp:TextBox ID="txtQuotationDescription" runat="server" CssClass="gridview" Width="250px"
                                                        Height="40px" Rows="2" Font-Bold="True" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="lbtSave" runat="server" Text="Salva con nome" OnClick="lbtSaveAs_Click"
                                                        CssClass="contextmenu" />&nbsp;<asp:CheckBox ID="chkUpdateNewQuotation" runat="server"
                                                            Checked="true" Text="Aggiorna dati" />
                                                </td>
                                            </tr>
                                            <asp:CustomValidator runat="server" ID="CustomValidator3" CssClass="droplist" ControlToValidate="txtQuotationDescription"
                                                Text="Valore già esistente per il Cliente" OnServerValidate="CustomValidator3_ServerValidate" />
                                            <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist"
                                ControlToValidate="txtTemplateDescription" Display="Dynamic" 
                                Enabled="False" />
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="droplist"
                                ControlToValidate="txtTemplateDescription" Display="Dynamic" 
                                Enabled="False" />--%>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnRecalc" runat="server" Enabled="false" OnClick="btnRecalc_Click"
                                                        Text="Ricalcola voci" Width="130px" />
                                                </td>
                                            </tr>
                                            <asp:LinqDataSource ID="ldsToolsTotals" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                                TableName="" OnSelecting="ldsToolsTotals_Selecting">
                                            </asp:LinqDataSource>
                                            <asp:LinqDataSource ID="ldsTotals" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                                TableName="" OnSelecting="ldsTotals_Selecting">
                                            </asp:LinqDataSource>
                                        </table>
                                    </td>
                                    <td style="width: 33%" valign="top" align="left">
                                        <asp:GridView ID="grdToolsTotals" runat="server" AutoGenerateColumns="false" DataSourceID="ldsToolsTotals"
                                            Font-Size="Medium" CellPadding="5" BorderColor="ActiveBorder" BorderStyle="Solid"
                                            Width="340">
                                            <HeaderStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" BackColor="WhiteSmoke" />
                                            <RowStyle HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField DataField="CstToolsTot" DataFormatString="{0:N2}" ItemStyle-Width="170"
                                                    HeaderText="Costo Impianti" />
                                                <asp:BoundField DataField="PrcToolsTot" DataFormatString="{0:N2}" ItemStyle-Width="170"
                                                    HeaderText="Prezzo Impianti" ItemStyle-Font-Bold="true" HeaderStyle-BackColor="Yellow"
                                                    ItemStyle-BackColor="Yellow" />
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                        <asp:GridView ID="grdTotals" runat="server" AutoGenerateColumns="false" DataSourceID="ldsTotals"
                                            Font-Size="Small" CellPadding="5" BorderColor="ActiveBorder" BorderStyle="Solid"
                                            Width="510">
                                            <HeaderStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" BackColor="WhiteSmoke" />
                                            <RowStyle HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField DataField="Quantity" DataFormatString="{0:N0}" HeaderText="Quantità"
                                                    ItemStyle-Font-Bold="true" ItemStyle-Width="80" />
                                                <asp:BoundField DataField="CstProdTot" DataFormatString="{0:N2}" ItemStyle-Width="90"
                                                    HeaderText="Cst Prod Tot" />
                                                <asp:BoundField DataField="PrcProdUni" DataFormatString="{0:N3}" ItemStyle-Width="80"
                                                    ItemStyle-Font-Bold="true" HeaderText="Prz Prod Uni" HeaderStyle-BackColor="Yellow"
                                                    ItemStyle-BackColor="Yellow" />
                                                <asp:BoundField DataField="PrcProdTot" DataFormatString="{0:N2}" ItemStyle-Width="90"
                                                    HeaderText="Prz Prod Tot" HeaderStyle-BackColor="Yellow" ItemStyle-BackColor="Yellow" />
                                                <%--<asp:BoundField DataField="CstTot" DataFormatString="{0:N2}" HeaderText="Costo Tot" />--%>
                                                <asp:BoundField DataField="PrcUni" DataFormatString="{0:N3}" ItemStyle-Width="80"
                                                    HeaderText="Prz Compl Uni" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="PrcTot" DataFormatString="{0:N2}" ItemStyle-Width="90"
                                                    HeaderText="Prz Compl Tot" />
                                            </Columns>
                                            <%--<itemtemplate>
                                    Quantità:
                                    <asp:Label ID="Label13" runat="server" Text='<%# Bind("Q1") %>' Font-Bold="true" />
                                    C.U.:
                                    <asp:Label ID="Label14" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CstTot1", "€ {0:N4}")  %>'  Font-Bold="true"/>
                                    P.U.:
                                    <asp:Label ID="Label15" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PrcTot1", "€ {0:N4}") %>'  Font-Bold="true"/>
                                    <br />
                                    Quantità:
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Q2")  %>' Font-Bold="true" />
                                    C.U.:
                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CstTot2", "€ {0:N4}") %>' Font-Bold="true" />
                                    P.U.:
                                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PrcTot2", "€ {0:N4}") %>' Font-Bold="true" />
                                    <br />
                                    Quantità:
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("Q3") %>' Font-Bold="true" />
                                    C.U.:
                                    <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CstTot3", "€ {0:N4}") %>' Font-Bold="true" />
                                    P.U.:
                                    <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PrcTot3", "€ {0:N4}") %>' Font-Bold="true" />
                                    <br />
                                    Quantità:
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("Q4") %>' Font-Bold="true" />
                                    C.U.:
                                    <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CstTot4", "€ {0:N4}") %>' Font-Bold="true" />
                                    P.U.:
                                    <asp:Label ID="Label9" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PrcTot4", "€ {0:N4}") %>' Font-Bold="true" />
                                    <br />
                                    Quantità:
                                    <asp:Label ID="Label10" runat="server" Text='<%# Bind("Q5")  %>' Font-Bold="true" />
                                    C.U.:
                                    <asp:Label ID="Label11" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CstTot5", "€ {0:N4}") %>' Font-Bold="true" />
                                    P.U.:
                                    <asp:Label ID="Label12" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PrcTot5", "€ {0:N4}") %>' Font-Bold="true" />
                                    <br />
                                </itemtemplate>--%>
                                        </asp:GridView>
                                        <br />
                                        <table class="gridview">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDescription" runat="server" Text="Descrizione lavorazione" Font-Bold="true"> </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" runat="server" Width="400px" TextMode="MultiLine"
                                                      Rows="3"  ></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableDelete="false" EnableInsert="false"
                                EnableUpdate="true" TableName="Quotations" ContextTypeName="DLLabExtim.QuotationDataContext">
                                <WhereParameters>
                                    <asp:QueryStringParameter Name="ID" QueryStringField="P0" Type="Int32" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                        </asp:Panel>
                        <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdQuotationDetails"
                            Display="None" />
                        &nbsp;<asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                        <h2>
                            Dettaglio&nbsp;
                            <asp:LinkButton ID="lbtAddUnmanagedItem" runat="server" CssClass="gridview" Font-Bold="False"
                                Font-Size="Small" ForeColor="Olive" Text="Aggiungi voce libera" OnClick="lbtAddUnmanagedItem_Click" />
                            &nbsp;
                            <asp:LinkButton ID="lbtCreateProductionOrder" runat="server" CssClass="gridview"
                                Font-Bold="False" Font-Size="Small" Text="Crea Ordine di Produzione" OnClientClick="javascript:SetParametersAndOpenPopUp();" />
                            &nbsp;
                            <%--<asp:LinkButton ID="lbtViewInputItems" runat="server" CssClass="gridview" Font-Bold="False"
                                Font-Size="Small" OnClick="lbtViewInputItems_Click" Text="Visualizza voci di input" />
                            &nbsp;--%>
                            <asp:HyperLink ID="hypListProductionOrders" runat="server" CssClass="gridview" Font-Bold="False"
                                Font-Size="Small" Text="Elenco Ordini di Produzione" />
                            &nbsp;
                            <%--<asp:LinkButton ID="lbtViewCalculation" runat="server" CssClass="gridview" Font-Bold="False"
                    Font-Size="Small" OnClick="lbtViewCalculation_Click" Text="Visualizza calcolo" />
                &nbsp;
                <asp:LinkButton ID="lbtViewResults" runat="server" CssClass="gridview" Font-Bold="False"
                    Font-Size="Small" OnClick="lbtResultsCalculation_Click" Text="Visualizza risultati" />
                &nbsp;--%>
                            <asp:LinkButton ID="lbtPrintQuotation" runat="server" CssClass="gridview" Font-Bold="False"
                                Font-Size="Small" OnClick="lbtPrintQuotation_Click" Text="Stampa preventivo Cliente" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintQuotationTechnical" runat="server" CssClass="gridview"
                                Font-Bold="False" Font-Size="Small" OnClick="lbtPrintQuotationTechnical_Click"
                                Text="Stampa preventivo tecnico" />
                            &nbsp;
                        </h2>
                        <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" EnableUpdate="true"
                            ContextTypeName="DLLabExtim.QuotationDataContext" TableName="QuotationDetails"
                            OnDeleted="GridDataSource_Deleted" OnSelecting="GridDataSource_Selecting">
                        </asp:LinqDataSource>
                        <%--<WhereParameters>
                        <asp:QueryStringParameter Name="ID" QueryStringField="P0" Type="Int32" />
                    </WhereParameters>--%>
                        <asp:GridView ID="grdQuotationDetails" runat="server" AutoGenerateColumns="false"
                            AutoGenerateDeleteButton="true" AllowPaging="True" DataSourceID="GridDataSource"
                            AllowSorting="false" CssClass="gridview" OnPageIndexChanging="grdQuotationDetails_PageIndexChanging"
                            OnRowDataBound="grdQuotationDetails_RowDataBound" OnPreRender="grdQuotationDetails_PreRender"
                            OnRowCommand="grdQuotationDetails_RowCommand" OnRowCreated="grdQuotationDetails_RowCreated"
                            OnDataBound="grdQuotationDetails_DataBound">
                            <%--AutoGenerateSelectButton="True" AutoGenerateEditButton="true" TabIndex="0"--%>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="~/Images/pencil.png" Height="10"
                                            Width="10" ToolTip="Modifica voce libera" Visible="false" CommandName="Edit" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="~/Images/disk.png" Height="10"
                                            Width="10" ToolTip="Salva voce libera" CommandName="Update" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:DynamicField DataField="ID" HeaderText="ID" UIHint="Text" Visible="false" />
                                <%--<asp:DynamicField DataField="Inserted" HeaderText="Incluso" UIHint="Boolean" />
                    <asp:DynamicField DataField="SelectPhase" HeaderText="Fase" UIHint="Boolean" />--%>
                                <%--<asp:DynamicField DataField="Position" HeaderText="Posizione" UIHint="Integer" />--%>
                                <asp:DynamicField DataField="Type" HeaderText="Tipo fase" UIHint="ForeignKey" />
                                <asp:DynamicField DataField="ItemType" HeaderText="Tipo prodotto" UIHint="ForeignKey" />
                                <asp:DynamicField DataField="ItemTypeDescription" HeaderText="Descrizione prodotto"
                                    UIHint="Text" />
                                <asp:DynamicField DataField="Unit" HeaderText="UM" UIHint="ForeignKey" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuantity" Text="Quantità" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" Width="50" MaxLength="6" CssClass="droplist"
                                            ToolTip="Inserire un valore intero, decimale con virgola o frazionario"> </asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" Width="50" MaxLength="6" CssClass="droplist"
                                            Enabled="false" />
                                    </EditItemTemplate>
                                    <%--AutoPostBack="True"--%>
                                </asp:TemplateField>
                                <%--<asp:DynamicField DataField="Quantity" HeaderText="Quantità" UIHint="Integer_Edit" />--%>
                                <asp:DynamicField DataField="Cost" HeaderText="Cst Uni" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                                <asp:DynamicField DataField="Multiply" HeaderText="Moltiplica" UIHint="Boolean" />
                                <asp:DynamicField DataField="Percentage" HeaderText="% Ric std" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                                <asp:DynamicField DataField="MarkUp" HeaderText="% Ric com" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                                <asp:DynamicField DataField="Price" HeaderText="Prz Uni" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                                <asp:DynamicField DataField="TotPrice" HeaderText="Prz Tot" UIHint="Text" ItemStyle-HorizontalAlign="Right"
                                    DataFormatString="{0:N2}" />
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
