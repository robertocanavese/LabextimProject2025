<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveRequestPopup.aspx.cs"
    Inherits="LabExtim.LeaveRequestPopup" %>

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
        window.onclose = opener.LeaveType.reload();
    </script>--%>
</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 10px; background-color: #FBFBFB; color: navy">

    <script type="text/javascript">
        window.onunload = refreshParent;
        function refreshParent() {
            window.opener.LeaveType.reload();
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
                                <h4>Richiesta permesso</h4>
                                <asp:DetailsView ID="dtvLeaveRequest" runat="server" DataSourceID="DetailsDataSource"
                                    AutoGenerateEditButton="true" AutoGenerateInsertButton="true" OnModeChanging="OnDetailsViewModeChanging"
                                    OnPreRender="OnDetailsViewPreRender" OnItemUpdated="OnDetailsViewItemUpdated"
                                    OnItemInserted="OnDetailsViewItemInserted" AutoGenerateRows="false" OnItemCommand="dtvLeaveRequest_ItemCommand"
                                    DataKeyNames="ID" OnItemCreated="dtvLeaveRequest_ItemCreated" OnItemUpdating="dtvLeaveRequest_ItemUpdating"
                                    OnItemInserting="dtvLeaveRequest_ItemInserting">
                                    <FieldHeaderStyle Font-Bold="true" />
                                    <RowStyle CssClass="selected" />
                                    <FooterStyle CssClass="selected" />
                                    <Fields>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblID" runat="server" Text="ID richiesta"></asp:Label>
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
                                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCompany" runat="server" DataField="Company" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblRequestDate" runat="server" Text="Data richiesta"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycRequestDate" runat="server" DataField="RequestDate" UIHint="DateTime"
                                                    DataFormatString="{0:d}" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycRequestDate" runat="server" DataField="RequestDate" UIHint="DateTime_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycRequestDate" runat="server" DataField="RequestDate" UIHint="DateTime_Edit"  Mode="Edit"/>
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                       <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblApplicant" runat="server" Text="Richiedente"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycApplicant" runat="server" DataField="Employee" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycApplicant" runat="server" DataField="Employee" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycApplicant" runat="server" DataField="Employee" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblLeaveType" runat="server" Text="Tipo permesso"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycLeaveType" runat="server" DataField="LeaveType1" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycLeaveType" runat="server" DataField="LeaveType1" UIHint="ForeignKey_Edit" Mode="Edit"  />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycLeaveType" runat="server" DataField="LeaveType1" UIHint="ForeignKey_Edit" Mode="Edit"  />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStartDate" runat="server" Text="Data inizio assenza"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime"
                                                    DataFormatString="{0:d}" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycStartDate" runat="server" DataField="StartDate" UIHint="DateTime_Edit"  Mode="Edit"/>
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblEndDate" runat="server" Text="Data fine assenza"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycEndDate" runat="server" DataField="EndDate" UIHint="DateTime"
                                                    DataFormatString="{0:d}" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycEndDate" runat="server" DataField="EndDate" UIHint="DateTime_Edit"  Mode="Edit"/>
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycEndDate" runat="server" DataField="EndDate" UIHint="DateTime_Edit"  Mode="Edit"/>
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDayFraction" runat="server" Text="Orario"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycDayFraction" runat="server" DataField="DayFraction1" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycDayFraction" runat="server" DataField="DayFraction1" UIHint="ForeignKey_Edit" Mode="Edit"  />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycDayFraction" runat="server" DataField="DayFraction1" UIHint="ForeignKey_Edit" Mode="Edit"  />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblVacationDays" runat="server" Text="Giorni assenza"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycVacationDays" runat="server" DataField="VacationDays" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycVacationDays" runat="server" DataField="VacationDays" UIHint="Integer_Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycVacationDays" runat="server" DataField="VacationDays" UIHint="Integer_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>


                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStatuse" runat="server" Text="Stato richiesta"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey_Edit" Mode="Edit"  />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycStatuse" runat="server" DataField="Statuse" UIHint="ForeignKey_Edit" Mode="Edit"  />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStatusDate" runat="server" Text="Data stato"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycStatusDate" runat="server" DataField="StatusDate" UIHint="DateTime"
                                                    DataFormatString="{0:d}" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycStatusDate" runat="server" DataField="StatusDate" UIHint="DateTime_Edit"  Mode="Edit"/>
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycStatusDate" runat="server" DataField="StatusDate" UIHint="DateTime_Edit"  Mode="Edit"/>
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblManager" runat="server" Text="Inviata a"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycManager" runat="server" DataField="Employee1" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycManager" runat="server" DataField="Employee1" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycManager" runat="server" DataField="Employee1" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblMessageToManager" runat="server" Text="MessageToManager"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycMessageToManager" runat="server" DataField="MessageToManager" UIHint="Text250Multiline"
                                                    HtmlEncode="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycMessageToManager" runat="server" DataField="MessageToManager" UIHint="Text250Multiline"
                                                    HtmlEncode="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycMessageToManager" runat="server" DataField="MessageToManager" UIHint="Text250Multiline"
                                                    HtmlEncode="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblMessageToApplicant" runat="server" Text="MessageToApplicant"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycMessageToApplicant" runat="server" DataField="MessageToApplicant" UIHint="Text250Multiline"
                                                    HtmlEncode="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycMessageToApplicant" runat="server" DataField="MessageToApplicant" UIHint="Text250Multiline_Edit"
                                                    HtmlEncode="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycMessageToApplicant" runat="server" DataField="MessageToApplicant" UIHint="Text250Multiline_Edit"
                                                    HtmlEncode="false" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                </asp:DetailsView>
                                <asp:LinqDataSource ID="DetailsDataSource" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                    EnableDelete="false" EnableInsert="true" EnableUpdate="true" OnInserting="DetailsDataSource_Inserting"
                                    TableName="LeaveRequests" OnSelecting="DetailsDataSource_Selecting" OnInserted="DetailsDataSource_Inserted"
                                    OnUpdating="DetailsDataSource_Updating" OnUpdated="DetailsDataSource_Updated">
                                </asp:LinqDataSource>
                            </td>

                        </tr>
                    </table>
                </td>


            </tr>
        </table>
    </form>
</body>
</html>
