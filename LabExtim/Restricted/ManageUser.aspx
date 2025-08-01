<%@ Page Language="C#" AutoEventWireup="true" Inherits="LabExtim.Restricted.ManageUser" CodeBehind="ManageUser.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Site.css" rel="stylesheet" type="text/css" />

    <title>Extimator 3.0 - Gestione utente</title>
</head>
<body>
    <form id="form1" runat="server">

        <table id="tblHeader" runat="server" width="100%" class="companyHeader" cellspacing="0" cellpadding="5" border="0">
            <tr style="background-color: black">
                <td style="width: 1%">
                    <div class="logoint">

                        <img src="../images/labe.png" alt="logo" />
                    </div>
                </td>
                <td>
                    <div class="logoint">
                        <a style="color: White">S.r.l. - Via Perù,
                                9/3 - 35127 Padova - tel. 049.7800500 - fax 049 7803891 - P.IVA 01698090287</a>
                    </div>
                </td>

                <td>
                    <div class="extimator">
                        <asp:Label ID="lblApplication" runat="server" Text="Extimator 3.0" ForeColor="LightGray"></asp:Label>&nbsp;
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:SiteMapPath ID="smpSite" runat="server" Font-Size="Small" RenderCurrentNodeAsLink="true"
                        PathSeparator=" > " NodeStyle-CssClass="closers" SiteMapProvider="Workstation">
                    </asp:SiteMapPath>
                </td>
            </tr>
        </table>

        <table class="loginStyle">
            <tr>
                <td rowspan="2" valign="top">
                    <asp:LinqDataSource ID="ldsVW_Employees" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext" TableName="VW_Employees" OrderBy="UniqueName"></asp:LinqDataSource>
                    <asp:Panel runat="server" Height="800" ScrollBars="Vertical">
                        <asp:GridView runat="server" ID="grdEmployees" AutoGenerateColumns="false" CssClass="gridview" Font-Size="X-Small" DataSourceID="ldsVW_Employees" AllowSorting="true" OnSorting="grdEmployees_Sorting" OnRowDataBound="grdEmployees_RowDataBound">
                            <RowStyle CssClass="row" />
                            <AlternatingRowStyle CssClass="altRow" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="Codice dipendente" SortExpression="ID" />
                                <asp:BoundField DataField="UserName" HeaderText="Username utente" SortExpression="UserName"  />
<%--                                <asp:BoundField DataField="CompanyCode" HeaderText="Codice interno" SortExpression="CompanyCode"  />--%>
                                <asp:BoundField DataField="Name" HeaderText="Nome" SortExpression="Name"  />
                                <asp:BoundField DataField="Surname" HeaderText="Cognome" SortExpression="Surname"  />
                                <asp:BoundField DataField="UniqueName" HeaderText="Nome univoco" SortExpression="UniqueName"  />
                                <%--<asp:BoundField DataField="UserGUID" HeaderText="Codice utente" />--%>
                                <asp:BoundField DataField="LeavingDate" HeaderText="Data cessazione" SortExpression="LeavingDate"  DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="roleDesc" HeaderText="Privilegi" SortExpression="roleDesc"  />
                                <asp:BoundField DataField="companyDesc" HeaderText="Azienda" SortExpression="companyDesc"  />
                            </Columns>
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>

                <td align="center" valign="top" class="grayPanel">
                    <table>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:Label ID="lblMessage" runat="server" CssClass="droplist" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center"><b>Nuovo utente / dipendente</b>
                        </tr>
                        <tr>
                            <td style="text-align: right">Nome:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="droplist"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Cognome:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="droplist"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Codice aziendale:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="droplist"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Azienda:</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlCompanies" runat="server" CssClass="droplist" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Seleziona azienda" Value="" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Ruolo:</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlRoles" runat="server" CssClass="droplist" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Seleziona ruolo" Value="select" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">Email:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtMail" runat="server" CssClass="droplist"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Username:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="droplist"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">Password:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="droplist"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: right">Conferma password:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="droplist"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:Button ID="btnCreateUser" CssClass="myButton" runat="server" Text="Crea utente" OnClick="btnCreateUser_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td colspan="2" align="center"><b>Modifica utente</b>
                        </tr>
                        <tr>
                            <td style="text-align: right">Username utente:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtEditName" runat="server" CssClass="droplist"></asp:TextBox>&nbsp;
                                    <asp:Button ID="btnSearchName" runat="server" Text="Cerca" CssClass="myButton" OnClick="btnSearchName_Click" />
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Codice, nome e cognome:</td>
                            <td style="text-align: left">
                                <asp:Label ID="lblUniqueName" runat="server" CssClass="droplist"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Codice aziendale:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtEditCompanyCode" runat="server" CssClass="droplist"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Azienda:</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlEditCompanies" runat="server" CssClass="droplist" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Seleziona azienda" Value="" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right">Ruolo:</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlEditRoles" runat="server" CssClass="droplist">
                                    <asp:ListItem Text="Seleziona ruolo" Value="select" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:Button ID="btnEditUser" CssClass="myButton" runat="server" Text="Salva modifiche" OnClick="btnEditUser_Click" />&nbsp;
                                    <asp:Button ID="btnDeleteUser" Visible="false" CssClass="myButton" runat="server" Text="Elimina utente" OnClick="btnDeleteUser_Click" OnClientClick="return confirm('Sei sicuro di voler eliminare questo username? (il relativo operatore Labextim non sarà eliminato!)')" />&nbsp;
                                <asp:Button ID="btnDeactivateUser" CssClass="myButton" runat="server" Text="Disattiva utente" OnClick="btnDeactivateUser_Click" />&nbsp;
                                    <asp:Button ID="btnActivateUser" CssClass="myButton" runat="server" Text="Riattiva utente" OnClick="btnActivateUser_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                                ATTENZIONE: <b>gli utenti che possiedono uno Username</b> sono abilitati ad accedere ed operare sui siti Labextim e LabextimOperator utlizzando detto Username e la password ad essi fornita.
                                <br />
                                La navigazione avviene in base ai priviliegi assegnati all'utente.
                                    <br />
                                <b>Gli utenti che non possiedono uno Username</b> sono di fatto ex-utenti non più abilitati all'accesso ma presenti sui siti in quanto responsabili della creazione di documenti
                                <br />
                                (preventivi o ordini di produzione) o di denunce produzione.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
    </form>
</body>
</html>
