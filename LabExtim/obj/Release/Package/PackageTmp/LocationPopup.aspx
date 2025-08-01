<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocationPopup.aspx.cs"
    Inherits="LabExtim.LocationPopup" %>

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
</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 10px; background-color: #FBFBFB; color: navy">

    <script type="text/javascript">
        window.onunload = refreshParent;
        function refreshParent() {
            window.opener.location.reload();
        }
    </script>

    <form id="form1" runat="server"> 
        <div>
            <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
            <%--<asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"
            EnablePartialRendering="true" />--%>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true"
                EnableScriptGlobalization="true">
            </asp:ToolkitScriptManager>
            <asp:Panel ID="DetailsPanel" runat="server" >
                <h4 >Dettaglio Destinazione viaggio <asp:Label ID="lblItemNo" runat="server" />
                </h4>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblSuccess" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:DetailsView ID="dtvLocation" runat="server" DataSourceID="DetailsDataSource"
                                AutoGenerateEditButton="true" AutoGenerateInsertButton="true" OnModeChanging="OnDetailsViewModeChanging"
                                OnPreRender="OnDetailsViewPreRender" OnItemUpdated="OnDetailsViewItemUpdated"
                                OnItemInserted="OnDetailsViewItemInserted" AutoGenerateRows="false" OnItemCommand="dtvLocation_ItemCommand"
                                DataKeyNames="ID" OnItemCreated="dtvLocation_ItemCreated"
                                OnItemUpdating="dtvLocation_ItemUpdating"
                                OnItemInserting="dtvLocation_ItemInserting">
                                <FieldHeaderStyle Font-Bold="true" />
                                <RowStyle CssClass="selected" />
                                <FooterStyle CssClass="selected" />
                                <Fields>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text="Codice" Width="120"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCode" runat="server" DataField="Code" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblName" runat="server" Text="Ragione Sociale"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycName" runat="server" DataField="Name" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycName" runat="server" DataField="Name" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycName" runat="server" DataField="Name" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblContact" runat="server" Text="Contatto"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycContact" runat="server" DataField="Contact" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycContact" runat="server" DataField="Contact" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycContact" runat="server" DataField="Contact" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycEmail" runat="server" DataField="Email" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycEmail" runat="server" DataField="Email" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycEmail" runat="server" DataField="Email" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblPhone" runat="server" Text="Telefono"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycPhone" runat="server" DataField="Phone" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycPhone" runat="server" DataField="Phone" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycPhone" runat="server" DataField="Phone" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycFax" runat="server" DataField="Fax" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycFax" runat="server" DataField="Fax" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycFax" runat="server" DataField="Fax" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblStreet" runat="server" Text="Indirizzo"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycStreet" runat="server" DataField="Street" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycStreet" runat="server" DataField="Street" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycStreet" runat="server" DataField="Street" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCAP" runat="server" Text="CAP"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCAP" runat="server" DataField="CAP" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycCAP" runat="server" DataField="CAP" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycCAP" runat="server" DataField="CAP" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCity" runat="server" Text="Località"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycCity" runat="server" DataField="City" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycCity" runat="server" DataField="City" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycCity" runat="server" DataField="City" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblProvince" runat="server" Text="Provincia"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DynamicControl ID="dycProvince" runat="server" DataField="Province" UIHint="Text" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DynamicControl ID="dycProvince" runat="server" DataField="Province" UIHint="Text_Edit" />
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DynamicControl ID="dycProvince" runat="server" DataField="Province" UIHint="Text_Edit" />
                                        </InsertItemTemplate>
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
                                </FooterTemplate>
                            </asp:DetailsView>
                            <br />
                            <asp:LinkButton ID="lbtClose" runat="server" CssClass="gridview" Font-Bold="False"
                                Font-Size="Small" OnClientClick="javascript:window.close();"
                                Text="Chiudi" />
                            <br />
                            <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="DetailsDataSource_Inserting"
                                TableName="Locations" OnSelecting="DetailsDataSource_Selecting" OnInserted="DetailsDataSource_Inserted"
                                OnUpdating="DetailsDataSource_Updating">
                            </asp:LinqDataSource>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <h5>&nbsp;</h5>
        </div>
    </form>
</body>
</html>
