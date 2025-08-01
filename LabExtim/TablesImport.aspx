<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="TablesImport.aspx.cs"
         Inherits="LabExtim._TablesImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />--%>
    <h2>
        Importazione Tabelle da motore database esterno</h2>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td>
                <asp:GridView ID="Menu1" runat="server" AutoGenerateColumns="false" CssClass="gridview"
                              AlternatingRowStyle-CssClass="even" OnRowCommand="Menu1_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Gruppo Tabelle">
                            <ItemTemplate>
                                <asp:LinkButton ID="HyperLink1" runat="server" CommandArgument='<%#Eval("Key") %>'><%#Eval("Value") %></asp:LinkButton>
                                <%--<asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%#Eval("Value.ListActionPath") %>'><%#Eval("Key") %></asp:HyperLink>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>