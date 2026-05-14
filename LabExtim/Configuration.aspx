<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Configuration.aspx.cs" Inherits="LabExtim.Configuration" %>

<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="verticalLines">
        <tr>
            <th align="left">
                <asp:Label ID="lblForPIMustUpdateAfter" runat="server" Text="Periodo di validità costi voci tabella base (mesi) per segnalazione obsolescenza"></asp:Label>
            </th>
            <td align="left">
               
                <cfb:IntTextBox ID="itbPIMustUpdateAfter" runat="server" ShowFindButton="false" />
                <asp:Label ID="Label1" runat="server" Text="(0 per disattivare la voce)"></asp:Label>
            </td>
        </tr>
        <tr>
            <th align="left">
                <asp:Label ID="Label2" runat="server" Text="Periodo di validità voci tabella base (mesi) per disattivazione automatica"></asp:Label>
            </th>
            <td align="left">
                <cfb:IntTextBox ID="itbPIMIDeactivateAfter" runat="server" ShowFindButton="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnSave" runat="server" Text="Salva configurazione" 
                    CssClass="droplist" onclick="btnSave_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="Button1" runat="server" Text="Salva configurazione" 
                    CssClass="droplist" onclick="btnSave_Click" /><br />
                 <asp:Label runat="server" ID="lblError" ></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
