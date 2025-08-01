<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="HomeTablet.aspx.cs" Inherits="LabExtim._HomeTablet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />--%>
    <h2>Menu principale</h2>
    <br />

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <table style="width: 100%;text-align:center">
                    <tr>
                        <td align="center">
                            <asp:TreeView ID="tvwMainMenu" runat="server" 
                                ImageSet="Arrows" Font-Size="Large">
                            </asp:TreeView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="Images/foto_sede4.jpg" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
