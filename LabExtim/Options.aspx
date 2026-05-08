<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Options.aspx.cs" Inherits="LabExtim.Options" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td colspan="6" align="center">
                <h4>Ricalcolo massivo denunce di produzione</h4>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Label ID="lblDateFrom" runat="server" Text="Data inizio elaborazione"></asp:Label>
            </td>
            <td colspan="3" align="center">
                <asp:Label ID="lblDateTo" runat="server" Text="Data fine elaborazione"></asp:Label>
            </td>
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
                <h4>Ricalcolo massivo Master Production Schedule</h4>
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
                <h4>Aggiornamento generale dei menu</h4>
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
                <h4>Ricalcolo dati redditività da data</h4>
            </td>
        </tr>
        <tr>
            <td colspan="6" align="center">
                <asp:Label ID="Label1" runat="server" Text="Data inizio elaborazione"></asp:Label>
            </td>

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
                <h4>Variazione massiva costo voci di tabella base</h4>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="Label2" runat="server" Text="Ultima data aggiornamento voce da"></asp:Label>
            </td>

            <td align="center">
                <asp:Label ID="Label3" runat="server" Text="Ultima data aggiornamento voce a"></asp:Label>
            </td>

            <td align="center">
                <asp:Label ID="Label4" runat="server" Text="Fornitore"></asp:Label>
            </td>

            <td align="center">
                <asp:Label ID="Label5" runat="server" Text="Tipo"></asp:Label>
            </td>

            <td align="center">
                <asp:Label ID="Label6" runat="server" Text="Tipo voce"></asp:Label>
            </td>

            <td align="center">
                <asp:Label ID="Label7" runat="server" Text="Variazione (+/-) %"></asp:Label>
            </td>

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
            <td align="center"></td>
            <td align="center"></td>
            <td align="center"></td>
            <td align="center"></td>
        </tr>
        <tr>
            <td colspan="6" align="center">
                <asp:Button ID="btnCostChange" runat="server" Text="Esegui" OnClientClick="javascript:return confirm('Confermi il ricalcolo del costo delle voci di tabella base incluse nella selezione corrente?');"
                    CssClass="myButton" />
                <hr />
            </td>
        </tr>


        <tr>
            <td colspan="6" align="center">
                <asp:Button ID="btnTest" runat="server" Text="Test"
                    CssClass="myButton" OnClick="btnTest_Click" />
                <hr />
            </td>
        </tr>


    </table>
</asp:Content>
