<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryTripPopup.aspx.cs"
    Inherits="LabExtim.DeliveryTripPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%--<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="~/Site.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript">
        window.onclose = opener.location.reload();
    </script>--%>
</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 10px; background-color: #FBFBFB; color: navy">

    <script type="text/javascript">
        window.onunload = refreshParent;
        function refreshParent() {
            window.opener.location.reload();
        }
    </script>

    <form id="form1" runat="server">
        <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true"></asp:ToolkitScriptManager>
        <h4>Viaggio<asp:Label ID="lblItemNo" runat="server" />
        </h4>
        <table>

            <tr>
                <td>
                    <table id="tblTestata" runat="server">
                        <tr>
                            <td valign="top">
                                <h4>Dati Viaggio</h4>
                                <asp:DetailsView ID="dtvDeliveryTrip" runat="server" DataSourceID="DetailsDataSource"
                                    AutoGenerateEditButton="true" AutoGenerateInsertButton="true" OnModeChanging="OnDetailsViewModeChanging"
                                    OnPreRender="OnDetailsViewPreRender" OnItemUpdated="OnDetailsViewItemUpdated"
                                    OnItemInserted="OnDetailsViewItemInserted" AutoGenerateRows="false" OnItemCommand="dtvDeliveryTrip_ItemCommand"
                                    DataKeyNames="ID" OnItemCreated="dtvDeliveryTrip_ItemCreated" OnItemUpdating="dtvDeliveryTrip_ItemUpdating"
                                    OnItemInserting="dtvDeliveryTrip_ItemInserting">
                                    <FieldHeaderStyle Font-Bold="true" />
                                    <RowStyle CssClass="selected" />
                                    <FooterStyle CssClass="selected" />
                                    <Fields>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblID" runat="server" Text="ID Viaggio"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycID" runat="server" DataField="ID" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCompany" runat="server" Text="Azienda"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey" CssClass="bold red" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit" Mode="Edit" AllowNullValue="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit" Mode="Edit" AllowNullValue="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text="Cliente viaggio"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit" Mode="Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text="Altra destinazione"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycLocation" runat="server" DataField="Location" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycLocation" runat="server" DataField="Location" UIHint="ForeignKey_Edit" Mode="Edit"  />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycLocation" runat="server" DataField="Location" UIHint="ForeignKey_Edit" Mode="Edit"  />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDriver" runat="server" Text="Autista"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycDriver" runat="server" DataField="Employee" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycDriver" runat="server" DataField="Employee" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycDriver" runat="server" DataField="Employee" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStartDate" runat="server" Text="Partenza"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime"
                                                    DataFormatString="{0:d}" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime_EditRO" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime_EditRO" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNote" runat="server" Text="Note"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text250Multiline"
                                                    HtmlEncode="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text250Multiline_Edit"
                                                    HtmlEncode="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycNote" runat="server" DataField="Note" UIHint="Text250Multiline_Edit"
                                                    HtmlEncode="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                </asp:DetailsView>
                                <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="DetailsDataSource_Inserting"
                                    TableName="DeliveryTrips" OnSelecting="DetailsDataSource_Selecting" OnInserted="DetailsDataSource_Inserted"
                                    OnUpdating="DetailsDataSource_Updating" OnUpdated="DetailsDataSource_Updated">
                                </asp:LinqDataSource>
                            </td>

                        </tr>
                    </table>
                </td>


            </tr>
            <tr>
                <td>

                    <asp:LinkButton ID="lbtShowHideDet" runat="server" CssClass="gridview" Font-Bold="False"
                        Text="Nascondi/visualizza dettaglio" OnClick="lbtShowHideDet_Click" />
                    <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                        <h4>
                            <asp:Label ID="lblDetails" runat="server" Text="Elenco Odp del viaggio" />
                        </h4>
                        <asp:LinqDataSource ID="ldsDeliveryTripDetails" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                            TableName="DeliveryTripDetails" OnSelecting="ldsDeliveryTripDetails_Selecting"
                            AutoGenerateWhereClause="true" EnableUpdate="true" EnableDelete="true">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="dtvDeliveryTrip" Name="ID_DeliveryTrip" Type="Int32"
                                    PropertyName="SelectedValue" DefaultValue="" />
                            </WhereParameters>

                        </asp:LinqDataSource>
                        <asp:GridView ID="grdDeliveryTripDetails" runat="server" AllowPaging="True" CssClass="gridview"
                            DataSourceID="ldsDeliveryTripDetails" OnPageIndexChanging="grdDeliveryTripDetails_PageIndexChanging"
                            AutoGenerateColumns="false" AutoGenerateEditButton="False" OnRowDataBound="grdDeliveryTripDetails_RowDataBound"
                            OnRowUpdated="grdDeliveryTripDetails_RowUpdated">
                            <RowStyle CssClass="row" />
                            <AlternatingRowStyle CssClass="altRow" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" CausesValidation="false"
                                            Text="Elimina" OnClientClick='return confirm("Confermi la cancellazione di questo OdP dal viaggio corrente?");' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProductionOrder.Customer.Name" HeaderText="Cliente" />
                                <asp:BoundField DataField="ID_ProductionOrder" HeaderText="Id OdP" />
                                <asp:BoundField DataField="ProductionOrder.Description" HeaderText="OdP" />
                                <asp:BoundField DataField="Quota" HeaderText="Quota viaggio" DataFormatString="{0:N3}" />
                                <asp:BoundField DataField="Employee.UniqueName" HeaderText="Inserito da" />
                                <asp:BoundField DataField="InsertDate" HeaderText="Data inserimento" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            </Columns>
                            <EmptyDataTemplate>
                                Nessuna voce trovata.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>

        </table>
    </form>
</body>
</html>
