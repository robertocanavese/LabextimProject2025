<%@ Page Language="C#" MasterPageFile="~/DynamicData/PageTemplates/PageTemplates.master" CodeBehind="Find.aspx.cs" Inherits="LabExtim.DynamicData.PageTemplates.Find" %>


    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <asp:HyperLink ID="hypHome" runat="server" CssClass="template" Font-Bold="False"
                   Font-Size="Small"  NavigateUrl="~/Home.aspx" Text="Home" />
    <h2>Trova voce nella tabella <%= table.DisplayName %></h2>

    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                   HeaderText="Elenco degli errori di validazione" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1" Display="None" />

            <asp:DetailsView ID="DetailsView1" runat="server" 
                             DataSourceID="DetailsDataSource" DefaultMode="Insert"
                             AutoGenerateInsertButton="False" OnItemCommand="DetailsView1_ItemCommand" OnItemInserted="DetailsView1_ItemInserted"
                             CssClass="detailstable" FieldHeaderStyle-CssClass="bold" 
                             oniteminserting="DetailsView1_ItemInserting">
            </asp:DetailsView>

            <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableInsert="true">
            </asp:LinqDataSource>
            
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>