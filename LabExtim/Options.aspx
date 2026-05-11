<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Options.aspx.cs" Inherits="LabExtim.Options" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="verticalLines">
        <tr>
            <td colspan="6" align="center">
                <h3>RICALCOLO MASSIVO DENUNCE DI PRODUZIONE</h3>
            </td>
        </tr>
        <tr>
            <th colspan="3" align="center">
                <asp:Label ID="lblDateFrom" runat="server" Text="Data INIZIO elaborazione"></asp:Label>
            </th>
            <th colspan="3" align="center">
                <asp:Label ID="lblDateTo" runat="server" Text="Data FINE elaborazione"></asp:Label>
            </th>
        </tr>
        <tr align="center">
            <td colspan="3" align="center">
                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="droplist" Columns="10"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                    ImageAlign="Middle" />
                <cc1:CalendarExtender ID="txtDateFrom_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                    CssClass="MyCalendar" PopupButtonID="ImageButton1" TargetControlID="txtDateFrom"
                    PopupPosition="Right" SelectedDate='<%# DateTime.Today %>'>
                </cc1:CalendarExtender>
            </td>
            <td colspan="3" align="center">
                <asp:TextBox ID="txtDateTo" runat="server" CssClass="droplist" Columns="10"></asp:TextBox>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                    ImageAlign="Middle" />
                <cc1:CalendarExtender ID="txtDateTo_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                    CssClass="MyCalendar" PopupButtonID="ImageButton2" TargetControlID="txtDateTo"
                    PopupPosition="Right" SelectedDate='<%# DateTime.Today %>'>
                </cc1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td colspan="6" align="center">
                <asp:Button ID="btnExecute" runat="server" Text="Esegui" OnClientClick="javascript:return confirm('Confermi il ricalcolo delle denunce di produzione con data compresa nel periodo indicato (il costo storico sarà sovrascritto con quello corrente)?');"
                    OnClick="btnExecute_Click" CssClass="myButton" />
                <hr />
            </td>
        </tr>

        <tr>
            <td colspan="6" align="center">
                <h3>RICALCOLO MASSIVO MASTER PRODUCTION SCHEDULE</h3>
            </td>
        </tr>
        <tr>
            <td colspan="6" align="center">Tutte le fasi schedulate ed in lavorazione saranno riprogrammate sulla base della data di consegna del relativo OdP.
            </td>
        </tr>
        <tr>
            <td colspan="6" align="center">
                <asp:Button ID="btnMPSRecalc" runat="server" Text="Esegui"
                    OnClientClick="javascript:return confirm('Confermi il ricalcolo del Master Production Schedule?');" OnClick="btnMPSRecalc_Click" CssClass="myButton" />
                <hr />
            </td>
        </tr>

        <tr>
            <td colspan="6" align="center">
                <h3>AGGIORNAMENTO GENERALE DEI MENU</h3>
            </td>
        </tr>
        <tr>
            <td colspan="6" align="center">Tutti i menu del programma saranno aggiornati alle ultime modifiche effettuate ai relativi modelli, voci, e macorovoci.
            </td>
        </tr>
        <tr>
            <td colspan="6" align="center">
                <asp:Button ID="btnMenuUpdate" runat="server" Text="Aggiorna" OnClick="btnMenuUpdate_Click" CssClass="myButton" />
                <hr />
            </td>
        </tr>

        <tr>
            <td colspan="6" align="center">
                <h3>RICALCOLO DATI REDDITIVITÀ DA DATA</h3>
            </td>
        </tr>
        <tr>
            <th colspan="6" align="center">
                <asp:Label ID="Label1" runat="server" Text="Data inizio elaborazione"></asp:Label>
            </th>

        </tr>
        <tr align="center">
            <td colspan="6" align="center">
                <asp:TextBox ID="txtDate1From" runat="server" CssClass="droplist" Columns="10"></asp:TextBox>
                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                    ImageAlign="Middle" />
                <cc1:CalendarExtender ID="txtDate1From_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                    CssClass="MyCalendar" PopupButtonID="ImageButton3" TargetControlID="txtDate1From"
                    PopupPosition="Right" SelectedDate='<%# DateTime.Today.AddYears(-1) %>'>
                </cc1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td colspan="6" align="center">
                <asp:Button ID="btnStatsRecalc" runat="server" Text="Esegui" OnClientClick="javascript:return confirm('Confermi il ricalcolo dei dati di redditività dalla data indicata?');"
                    CssClass="myButton" OnClick="btnStatsRecalc_Click" />
                <hr />
            </td>
        </tr>

        <tr>
            <td colspan="6" align="center">
                <h3>AGGIORNAMENTO MASSIVO COSTO VOCI DI TABELLA BASE</h3>
            </td>
        </tr>
        <tr>
            <th align="center">
                <asp:Label ID="Label2" runat="server" Text="Data aggiornamento voce DA"></asp:Label>
            </th>

            <th align="center">
                <asp:Label ID="Label3" runat="server" Text="Data aggiornamento voce A"></asp:Label>
            </th>

            <th align="center">
                <asp:Label ID="Label4" runat="server" Text="Fornitore"></asp:Label>
            </th>

            <th align="center">
                <asp:Label ID="Label5" runat="server" Text="Tipo"></asp:Label>
            </th>

            <th align="center">
                <asp:Label ID="Label6" runat="server" Text="Tipo voce"></asp:Label>
            </th>

            <th align="center">
                <asp:Label ID="Label7" runat="server" Text="Variazione (+/-) Es.: +15%, -10.3%"></asp:Label>
            </th>

        </tr>
        <tr align="center">
            <td align="center">
                <asp:TextBox ID="txtPIDateFrom" runat="server" CssClass="droplist" Columns="10"></asp:TextBox>
                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                    ImageAlign="Middle" />
                <cc1:CalendarExtender ID="txtPIDateFrom_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                    CssClass="MyCalendar" PopupButtonID="ImageButton4" TargetControlID="txtPIDateFrom"
                    PopupPosition="Right" SelectedDate='<%# DateTime.Today.AddYears(-1) %>'>
                </cc1:CalendarExtender>
            </td>
            <td align="center">
                <asp:TextBox ID="txtPIDateTo" runat="server" CssClass="droplist" Columns="10"></asp:TextBox>
                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/DynamicData/Content/Images/Calendar.png"
                    ImageAlign="Middle" />
                <cc1:CalendarExtender ID="txtPIDateTo_CalendarExtender" runat="server" FirstDayOfWeek="Monday"
                    CssClass="MyCalendar" PopupButtonID="ImageButton5" TargetControlID="txtPIDateTo"
                    PopupPosition="Right" SelectedDate='<%# DateTime.Today.AddYears(-1) %>'>
                </cc1:CalendarExtender>
            </td>
            <td align="center">
                <asp:DropDownList ID="ddlSuppliers" runat="server" AutoPostBack="false" DataSourceID="ldsSuppliers"
                    DataTextField="Name" DataValueField="Code" CssClass="droplist" AppendDataBoundItems="true">
                    <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:LinqDataSource ID="ldsSuppliers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                    TableName="Suppliers" OrderBy="Name">
                </asp:LinqDataSource>
            </td>
            <td align="center">
                <asp:DropDownList ID="ddlTypes" runat="server" AutoPostBack="false" DataSourceID="ldsTypes"
                    DataTextField="Description" DataValueField="Code" CssClass="droplist" AppendDataBoundItems="true">
                    <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:LinqDataSource ID="ldsTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                    TableName="Types" Where='Category="I"' OrderBy="Order">
                </asp:LinqDataSource>
            </td>
            <td align="center">
                <asp:DropDownList ID="ddlItemTypes" runat="server" AutoPostBack="false" DataSourceID="ldsItemTypes"
                    DataTextField="Description" DataValueField="Code" CssClass="droplist" AppendDataBoundItems="true">
                    <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:LinqDataSource ID="ldsItemTypes" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                    TableName="ItemTypes" Where='Category="I"' OrderBy="Order">
                </asp:LinqDataSource>
            </td>
            <td align="center">
                <cfb:FloatTextBox ID="txtPercIncrement" runat="server" CssClass="droplist" />
                %
            </td>
        </tr>
        <tr>
            <td colspan="6" align="center">
                <asp:Button ID="btnRestoreLast" runat="server" Text="Ripristina da ultimo aggiornamento" OnClientClick="javascript:return confirm('Confermi il ripristino dei dati esistenti prima dell\'ultimo aggiornamento? (N.B.: la selezione corrente (date, fornitore, etc:..) in questo caso è ininfluente)';"
                    CssClass="myButton" OnClick="btnRestoreLast_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnCostChange" runat="server" Text="Esegui" OnClientClick="javascript:return confirm('Confermi il ricalcolo del costo delle voci di tabella base incluse nella selezione corrente?');"
                    CssClass="myButton" OnClick="btnCostChange_Click" /><br />
                <asp:Label runat="server" ID="lblCostChange_Error" ></asp:Label>
                <hr />
            </td>
        </tr>


        <%--<tr>
            <td colspan="6" align="center">
                <asp:Button ID="btnTest" runat="server" Text="Test"
                    CssClass="myButton" OnClick="btnTest_Click" />
                <hr />
            </td>
        </tr>--%>
    </table>
</asp:Content>
