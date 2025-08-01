<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerMarkUpPopup.aspx.cs"
    Inherits="LabExtim.CustomerMarkUpPopup" %>

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
    <%--background-color: #fdffb8"--%>
    <form id="form1" runat="server">
        <div>
            <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
            <%--<asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"
            EnablePartialRendering="true" />--%>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true"
                EnableScriptGlobalization="true">
            </asp:ToolkitScriptManager>
            <asp:Panel ID="DetailsPanel" runat="server" Font-Size="Smaller">
                <h2 style="text-align: center;">Dettaglio Cliente
                <asp:Label ID="lblItemNo" runat="server" />
                </h2>
                <center>
                    <table cellpadding="10">
                        <tr>
                            <td>
                                <asp:Label ID="lblSuccess" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-size: x-small" valign="top">
                                <asp:DetailsView ID="dtvCustomerMarkUp" runat="server" DataSourceID="DetailsDataSource"
                                    AutoGenerateEditButton="true" AutoGenerateInsertButton="true" OnModeChanging="OnDetailsViewModeChanging"
                                    OnPreRender="OnDetailsViewPreRender" OnItemUpdated="OnDetailsViewItemUpdated"
                                    OnItemInserted="OnDetailsViewItemInserted" AutoGenerateRows="false" OnItemCommand="dtvCustomerMarkUp_ItemCommand"
                                    DataKeyNames="ID" OnItemCreated="dtvCustomerMarkUp_ItemCreated"
                                    OnItemUpdating="dtvCustomerMarkUp_ItemUpdating"
                                    OnItemInserting="dtvCustomerMarkUp_ItemInserting">
                                    <FieldHeaderStyle Font-Bold="true" />
                                    <RowStyle CssClass="selected" />
                                    <FooterStyle CssClass="selected" />
                                    <Fields>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text="Cliente" Width="120"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycCode" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycCode" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCode" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblMarkUp" runat="server" Text="Ricarico commerciale"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Integer_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Integer_Edit"
                                                    Mode="Insert" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDistance" runat="server" Text="Distanza da sede (km)"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycDistance" runat="server" DataField="Distance" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycDistance" runat="server" DataField="Distance" UIHint="Integer_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycDistance" runat="server" DataField="Distance" UIHint="Integer_Edit"
                                                    Mode="Insert" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                    <FooterTemplate>
                                        <%--<asp:LinkButton ID="lbtDeactivate" runat="server" CssClass="selected" Font-Bold="False"
                                    Font-Size="X-Small" Text="Disattiva voce" CommandName="Deactivate" Enabled="false"></asp:LinkButton>
                                <asp:LinkButton ID="lbtActivate" runat="server" CssClass="selected" Font-Bold="False"
                                    Font-Size="X-Small" Text="Riattiva voce" CommandName="Activate" Enabled="false"></asp:LinkButton>--%>
                                    </FooterTemplate>
                                </asp:DetailsView>

                                <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="DetailsDataSource_Inserting"
                                    TableName="CustomersMarkUps" OnSelecting="DetailsDataSource_Selecting" OnInserted="DetailsDataSource_Inserted"
                                    OnUpdating="DetailsDataSource_Updating">
                                    <%--<WhereParameters>
                                <asp:DynamicQueryStringParameter Name="ID" />
                            </WhereParameters>--%>
                                </asp:LinqDataSource>
                            </td>
                        </tr>


                        <tr>
                            <td style="font-size: x-small" valign="top">
                                <asp:DetailsView ID="dtvCustomerNickname" runat="server" DataSourceID="ldsCustomerNicknames"
                                    AutoGenerateEditButton="true" AutoGenerateInsertButton="true" OnModeChanging="dtvCustomerNickname_ModeChanging"
                                    OnPreRender="dtvCustomerNickname_PreRender" OnItemUpdated="dtvCustomerNickname_ItemUpdated"
                                    OnItemInserted="dtvCustomerNickname_ItemInserted" AutoGenerateRows="false" OnItemCommand="dtvCustomerNickname_ItemCommand"
                                    DataKeyNames="ID" OnItemCreated="dtvCustomerNickname_ItemCreated"
                                    OnItemUpdating="dtvCustomerNickname_ItemUpdating"
                                    OnItemInserting="dtvCustomerNickname_ItemInserting">
                                    <FieldHeaderStyle Font-Bold="true" />
                                    <RowStyle CssClass="selected" />
                                    <FooterStyle CssClass="selected" />
                                    <Fields>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text="Cliente" Width="120"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycCode" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycCode" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCode" runat="server" DataField="Customer" UIHint="ForeignKey" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNickname" runat="server" Text="Nickname"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycNickname" runat="server" DataField="Nickname" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycNickname" runat="server" DataField="Nickname" UIHint="Text_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycNickname" runat="server" DataField="Nickname" UIHint="Text_Edit"
                                                    Mode="Insert" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                    <FooterTemplate>
                                        <%--<asp:LinkButton ID="lbtDeactivate" runat="server" CssClass="selected" Font-Bold="False"
                                    Font-Size="X-Small" Text="Disattiva voce" CommandName="Deactivate" Enabled="false"></asp:LinkButton>
                                <asp:LinkButton ID="lbtActivate" runat="server" CssClass="selected" Font-Bold="False"
                                    Font-Size="X-Small" Text="Riattiva voce" CommandName="Activate" Enabled="false"></asp:LinkButton>--%>
                                    </FooterTemplate>
                                </asp:DetailsView>
                                <asp:LinqDataSource ID="ldsCustomerNicknames" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="ldsCustomerNicknames_Inserting"
                                    TableName="CustomerNicknames" OnSelecting="ldsCustomerNicknames_Selecting" OnInserted="ldsCustomerNicknames_Inserted"
                                    OnUpdating="ldsCustomerNicknames_Updating">
                                </asp:LinqDataSource>
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <center>
                    <asp:LinkButton ID="lbtClose" runat="server" CssClass="gridview" Font-Bold="False"
                        Font-Size="X-Small" OnClientClick="javascript:window.close();"
                        Text="Chiudi" />
                </center>
                <br />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
