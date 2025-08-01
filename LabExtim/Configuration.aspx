<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Configuration.aspx.cs" Inherits="LabExtim.Configuration" %>

<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td class="labelsForEditTable">
                <asp:Label ID="lblForPIMustUpdateAfter" runat="server" Text="Periodo di validità voci tabella base (mesi)"></asp:Label>
            </td>
            <td class="textBoxesForEditTable">
               
                <cfb:IntTextBox ID="itbPIMustUpdateAfter" runat="server" ShowFindButton="false" />
                <asp:Label ID="Label1" runat="server" Text="(0 per disattivare la voce)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnSave" runat="server" Text="Salva configurazione" 
                    CssClass="droplist" onclick="btnSave_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
