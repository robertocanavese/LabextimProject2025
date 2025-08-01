<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Home.aspx.cs" Inherits="LabExtim._Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h2>Menu principale</h2>
    <br />

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>

                <table style="width: 100%">
                    <tr>
                        <td style="vertical-align:top;width:33%">
                            <asp:TreeView ID="tvwMenu1" runat="server" 
                                ImageSet="Arrows" Font-Size="Larger">
                            </asp:TreeView>
                        </td>
                        <td style="vertical-align:top;width:33%">
                            <asp:TreeView ID="tvwMenu2" runat="server" 
                                ImageSet="Arrows" Font-Size="Larger" >
                            </asp:TreeView>
                        </td>
                        <td style="vertical-align:top;width:33%">
                            <asp:TreeView ID="tvwMenu3" runat="server" OnSelectedNodeChanged="tvwMenu3_SelectedNodeChanged"
                                ImageSet="Arrows" Font-Size="Larger">
                            </asp:TreeView>
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align:center">
                            <asp:Image ID="Image1" runat="server" ImageUrl="Images/foto_sede4.jpg"  />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
