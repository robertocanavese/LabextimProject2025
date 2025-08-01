<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True"
    CodeBehind="FreeTypePODetails2.aspx.cs" Inherits="LabExtim.FreeTypePODetails2"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/CustomControls/SearchEngine.ascx" TagName="SearchEngine" TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Report voci libere (denuncia produzione)
        <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
    </h2>
    <table>
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
    </table>
    <asp:Panel ID="DetailsPanel" runat="server">
        <table>
            <tr>
                <td>
                    <span style="font-weight: bold">Costi lavorazioni esterne da OdP
                    </span>
                    <asp:GridView ID="grdProductionOrders" runat="server" AutoGenerateColumns="False"
                        AllowPaging="false" PagerSettings-Position="Bottom" DataKeyNames="ID" CssClass="gridview"
                        OnPageIndexChanging="grdProductionOrders_PageIndexChanging" OnRowDataBound="grdProductionOrders_RowDataBound"
                        OnRowCommand="grdProductionOrders_RowCommand" ShowFooter="true" OnRowEditing="grdProductionOrders_RowEditing" OnRowUpdating="grdProductionOrders_RowUpdating" OnRowCancelingEdit="grdProductionOrders_RowCancelingEdit"
                        AutoGenerateEditButton="true" >
                        <RowStyle CssClass="row" />
                        <AlternatingRowStyle CssClass="altRow" />
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate >
                                    <asp:HiddenField ID="hid_ID_ProductionOrderDetail" runat="server" Value='<%#Eval("ID_ProductionOrderDetail") %>'></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField runat="server" DataField="ProductionDate" HeaderText="Data produzione"
                                DataFormatString="{0:d}" ReadOnly="true"  />
                            <asp:BoundField runat="server" DataField="SupplierName" HeaderText="Fornitore" ReadOnly="true" />
                            <asp:BoundField runat="server" DataField="ID" HeaderText="ID OdP"  ReadOnly="true" />
                            <asp:BoundField runat="server" DataField="Number" HeaderText="Numero OdP"  ReadOnly="true" />
                            <asp:BoundField runat="server" DataField="CustomerName" HeaderText="Cliente" ReadOnly="true"  />
                            <asp:BoundField runat="server" DataField="FreeItemDescription" HeaderText="Descrizione voce libera" ItemStyle-Wrap="true"  ReadOnly="true" />
                            <asp:BoundField runat="server" DataField="UMDescription" HeaderText="UM" ItemStyle-HorizontalAlign="Right" ReadOnly="true"  />
                            <%--<asp:BoundField runat="server" DataField="RawMaterialQuantity" HeaderText="Quantità"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField runat="server" DataField="Cost" HeaderText="Costo totale" ItemStyle-HorizontalAlign="Right" />--%>
                            <%-- <asp:BoundField runat="server" DataField="Note" HeaderText="Note" />--%>
                            <asp:TemplateField HeaderText="Quantità" ItemStyle-HorizontalAlign="Right"  ControlStyle-CssClass="droplist">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_RawMaterialQuantity" runat="server" Text='<%#Eval("RawMaterialQuantity") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_RawMaterialQuantity" runat="server" Text='<%#Eval("RawMaterialQuantity") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo totale" ItemStyle-HorizontalAlign="Right" ControlStyle-CssClass="droplist">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Cost" runat="server" Text='<%#Eval("Cost") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_Cost" runat="server" Text='<%#Eval("Cost") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle Font-Bold="true" CssClass="footer" HorizontalAlign="Right" />
                        <PagerStyle CssClass="footer" />
                        <PagerTemplate>
                            <asp:GridViewPager ID="Pager1" runat="server" />
                        </PagerTemplate>
                        <EmptyDataTemplate>
                            Nessuna voce trovata.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <%--<tr>
                <td>
                    <span style="font-weight: bold">Costi lavorazioni esterne da DDT
                    </span>
                    <asp:GridView ID="grdDDTs" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                        PagerSettings-Position="Bottom" CssClass="gridview" OnPageIndexChanging="grdDDTs_PageIndexChanging"
                        OnRowDataBound="grdDDTs_RowDataBound" OnRowCommand="grdDDTs_RowCommand" ShowFooter="true">
                        <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                        <Columns>
                            <asp:BoundField runat="server" DataField="mm_lotto" HeaderText="ID OdP" />
                            <asp:BoundField runat="server" DataField="NumDDT" HeaderText="No DDT" />
                            <asp:BoundField runat="server" DataField="DataDDT" HeaderText="Data DDT" DataFormatString="{0:d}" />
                            <asp:BoundField runat="server" DataField="mm_descr" HeaderText="Descrizione DDT" />
                            <asp:BoundField runat="server" DataField="mm_unmis" HeaderText="UM DDT" />
                            <asp:BoundField runat="server" DataField="mm_quant" HeaderText="Quantità DDT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField runat="server" DataField="mm_prezzo" HeaderText="P/Uni DDT" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField runat="server" DataField="mm_valore" HeaderText="Totale DDT" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                        <FooterStyle Font-Bold="true" CssClass="footer" HorizontalAlign="Right" />
                        <PagerStyle CssClass="footer" />
                        <PagerTemplate>
                            <asp:GridViewPager ID="Pager1" runat="server"  />
                        </PagerTemplate>
                        <EmptyDataTemplate>
                            Nessuna voce trovata.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>--%>
        </table>
        <br />
    </asp:Panel>
</asp:Content>
