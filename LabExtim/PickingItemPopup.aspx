<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickingItemPopup.aspx.cs"
    Inherits="LabExtim.PickingItemPopup" %>

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
    <form id="form1" runat="server">
        <div>
            <asp:DynamicDataManager ID="DynamicDataManager1" runat="server"  AutoLoadForeignKeys="true" />
            <%--<asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"
            EnablePartialRendering="true" />--%>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true"></asp:ToolkitScriptManager>
            <asp:Panel ID="DetailsPanel" runat="server">
                <h2 style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 100%;">Dettaglio Voce tabella base No
                <%= Request.QueryString["ID"] %>
                </h2>
                <table>
                    <tr>
                        <td valign="top">
                            <asp:DetailsView ID="dtvPickingItem" runat="server" DataSourceID="DetailsDataSource"
                                AutoGenerateEditButton="true" AutoGenerateInsertButton="true" OnModeChanging="OnDetailsViewModeChanging"
                                OnPreRender="OnDetailsViewPreRender" OnItemUpdated="OnDetailsViewItemUpdated"
                                OnItemInserted="OnDetailsViewItemInserted" AutoGenerateRows="false" OnItemCommand="dtvPickingItem_ItemCommand"
                                DataKeyNames="ID" OnItemCreated="dtvPickingItem_ItemCreated"
                                OnItemUpdating="dtvPickingItem_ItemUpdating">
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
                                            <asp:Label ID="lblBelongsToMacroItem" runat="server" Text="Macrovoci"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Image ID="dycBelongsToMacroItem" runat="server" ImageUrl="~/Images/exclamation.png"
                                                ToolTip="Voce presente in una o più macrovoci, non modificarne la natura!" Visible='<%# Eval("BelongsToMacroItem") == null ? false : Eval("BelongsToMacroItem") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Image ID="dycBelongsToMacroItem" runat="server" ImageUrl="~/Images/exclamation.png"
                                                ToolTip="Voce presente in una o più macrovoci, non modificarne la natura!" Visible='<%# Eval("BelongsToMacroItem") == null ? false : Eval("BelongsToMacroItem") %>' />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:Image ID="dycBelongsToMacroItem" runat="server" ImageUrl="~/Images/exclamation.png"
                                                ToolTip="Voce presente in una o più macrovoci, non modificarne la natura!" Visible='<%# Eval("BelongsToMacroItem") == null ? false : Eval("BelongsToMacroItem") %>' />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCompany" runat="server" Text="Azienda"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey"  />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit"
                                                Mode="Edit"  AllowNullValue="false" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit"  AllowNullValue="false" />
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
                                            <asp:Label ID="lblItemDescription" runat="server" Text="Descrizione voce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="ItemDescription"
                                                UIHint="Text250" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="ItemDescription"
                                                UIHint="Text250_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycItemDescription" runat="server" DataField="ItemDescription"
                                                UIHint="Text250_Edit" />
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
                                            <asp:DynamicControl ID="dycSupplier" runat="server" DataField="Supplier" UIHint="ForeignKey_Edit" />
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
                                            <asp:DynamicControl ID="dycCost" runat="server" DataField="Cost" UIHint="Decimal_Edit" />
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
                                            <asp:DynamicControl ID="dycMultiply" runat="server" DataField="Multiply" UIHint="Boolean_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
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
                                            <asp:DynamicControl ID="dycPercentage" runat="server" DataField="Percentage" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
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
                                                UIHint="ForeignKey_Edit" Mode="Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycStandardPercentage" runat="server" DataField="StandardPercentage"
                                                UIHint="ForeignKey_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text="Data aggiornamento costo"></asp:Label>
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
                                            <asp:Label ID="lblLink" runat="server" Text="Dipendenza da altra voce"></asp:Label>
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
                                            <asp:Label ID="lblMILink" runat="server" Text="Dipendenza da macrovoce"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycMILink" runat="server" DataField="MILink" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycMILink" runat="server" DataField="MILink" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycMILink" runat="server" DataField="MILink" UIHint="Text_Edit" />
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
                                    <%--                        <asp:TemplateField>
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
                                                UIHint="ForeignKey_Edit" Mode="Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycItemDisplayMode" runat="server" DataField="ItemDisplayMode"
                                                UIHint="ForeignKey_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                </Fields>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lbtDeactivate" runat="server" CssClass="selected" Font-Bold="False"
                                        Font-Size="X-Small" Text="Disattiva voce" CommandName="Deactivate" Enabled="false"></asp:LinkButton>
                                    <asp:LinkButton ID="lbtActivate" runat="server" CssClass="selected" Font-Bold="False"
                                        Font-Size="X-Small" Text="Riattiva voce" CommandName="Activate" Enabled="false"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:DetailsView>
                            <br />
                            <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="DetailsDataSource_Inserting"
                                TableName="PickingItems">
                                <WhereParameters>
                                    <asp:DynamicQueryStringParameter Name="ID" />
                                </WhereParameters>
                            </asp:LinqDataSource>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
