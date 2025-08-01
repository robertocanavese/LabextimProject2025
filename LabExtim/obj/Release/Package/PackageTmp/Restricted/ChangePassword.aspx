<%@ Page Language="C#" AutoEventWireup="true" Inherits="LabExtim.Restricted.ChangePassword" Codebehind="ChangePassword.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Untitled Page</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <div style="text-align: center">
                <asp:ChangePassword ID="ChangePwdCtrl" runat="server">
                    <MailDefinition From="pwd@apress.com" Subject="Changes in your profile" Priority="high" />
                    <TitleTextStyle />
                </asp:ChangePassword>
            </div>
        </form>
    </body>
</html>