<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoAuth.aspx.cs" Inherits="LabExtim.AutoAuth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel ID="pnlLeaveRequestsConsole" Visible="false" CssClass="droplist" runat="server">
                <table style="text-align:center">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblRequest" CssClass="droplist" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessageToApplicant" runat="server">Comunicazione a richiedente</asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtmessageToApplicant" runat="server" TextMode="MultiLine" Rows="3" Columns="60"></asp:TextBox></td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Button ID="btnReject" CssClass="droplist" runat="server" Text="Approva" OnClick="btnReject_Click" /></td>
                        <td>
                            <asp:Button ID="btnApprove" CssClass="droplist" runat="server" Text="Respingi" OnClick="btnApprove_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblMessage" CssClass="droplist" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
