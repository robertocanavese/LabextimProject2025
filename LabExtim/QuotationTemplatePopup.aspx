<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuotationTemplatePopup.aspx.cs"
    Inherits="LabExtim.QuotationTemplatePopup" %>

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
            <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
            <%--<asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"
            EnablePartialRendering="true" />--%>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true"></asp:ToolkitScriptManager>
            <asp:Panel ID="DetailsPanel" runat="server">
                <h2 style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 100%;">Creazione nuovo modello da preventivo No
                <%= Request.QueryString["ID"] %>
                </h2>
                <table>
                    <tr>
                        <td valign="top">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                HeaderText="Elenco degli errori di validazione" />
                            <asp:DynamicValidator runat="server" ID="DynamicValidator1" ControlToValidate="dtvQuotation"
                                Display="None" />
                            <asp:DetailsView ID="dtvQuotationTemplate" runat="server" DataSourceID="DetailsDataSource" DefaultMode="Insert"
                                OnModeChanging="OnDetailsViewModeChanging"
                                OnPreRender="OnDetailsViewPreRender" OnItemUpdated="OnDetailsViewItemUpdated"
                                OnItemInserted="OnDetailsViewItemInserted" AutoGenerateRows="false" OnItemCommand="dtvQuotationTemplate_ItemCommand"
                                DataKeyNames="ID" OnItemCreated="dtvQuotationTemplate_ItemCreated"
                                OnItemUpdating="dtvQuotationTemplate_ItemUpdating">
                                <FieldHeaderStyle Font-Bold="true" />
                                <RowStyle CssClass="selected" />
                                <FooterStyle CssClass="selected" />
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
                        </td>
                    </tr>
                </table>
                <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableDelete="false" EnableInsert="true"
                    EnableUpdate="false" TableName="QuotationTemplates" ContextTypeName="DLLabExtim.QuotationDataContext">
                    <WhereParameters>
                        <asp:QueryStringParameter Name="ID" QueryStringField="P2" Type="Int32" />
                    </WhereParameters>
                </asp:LinqDataSource>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
