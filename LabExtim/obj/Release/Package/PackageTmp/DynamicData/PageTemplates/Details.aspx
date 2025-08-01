<%@ Page Language="C#" MasterPageFile="~/DynamicData/PageTemplates/PageTemplates.master" CodeBehind="Details.aspx.cs" Inherits="LabExtim.DynamicData.PageTemplates.Details" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <asp:HyperLink ID="hypHome" runat="server" CssClass="template" Font-Bold="False"
                   Font-Size="Small"  NavigateUrl="~/Home.aspx" Text="Home" />
    <h2>Dettaglio voce della tabella <%= table.DisplayName %></h2>

    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                   HeaderText="Elenco degli errori di validazione" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1" Display="None" />

            <asp:DetailsView ID="DetailsView1" runat="server" DataSourceID="DetailsDataSource" OnItemDeleted="DetailsView1_ItemDeleted"
                             CssClass="detailstable" FieldHeaderStyle-CssClass="bold" >
                <Fields>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="EditHyperLink" runat="server"
                                           NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'
                                           Text="Modifica" />
                            <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete" CausesValidation="false"
                                            OnClientClick=' return confirm("Confermi la cancellazione di questa voce?"); '
                                            Text="Cancella" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Fields>
            </asp:DetailsView>

            <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableDelete="true">
                <WhereParameters>
                    <asp:DynamicQueryStringParameter />
                </WhereParameters>
            </asp:LinqDataSource>

            <br />

            <div class="bottomhyperlink">
                <asp:HyperLink ID="ListHyperLink" runat="server">Mostra tutte le voci</asp:HyperLink>
            </div>        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>