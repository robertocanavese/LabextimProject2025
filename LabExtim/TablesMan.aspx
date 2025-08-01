<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="TablesMan.aspx.cs" Inherits="LabExtim._TablesMan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />--%>

    <h2>Gestione standard Tabelle</h2>

    <br /><br />

    <asp:GridView ID="Menu1" runat="server" AutoGenerateColumns="false"
                  CssClass="gridview" AlternatingRowStyle-CssClass="even">
        <Columns>
            <asp:TemplateField HeaderText="Nome Tabella" SortExpression="TableName">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("ListActionPath") %>'><%#Eval("DisplayName") %></asp:HyperLink>
                    <%--<asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%#Eval("Value.ListActionPath") %>'><%#Eval("Key") %></asp:HyperLink>--%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>