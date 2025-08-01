<%@ Page Language="C#" MasterPageFile="~/DynamicData/PageTemplates/PageTemplates.master" CodeBehind="List.aspx.cs" Inherits="LabExtim.DynamicData.PageTemplates.List" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter"
    TagPrefix="asp" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
   

     <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <asp:HyperLink ID="hypHome" runat="server" CssClass="template" Font-Bold="False"
        Font-Size="Small" NavigateUrl="~/Home.aspx" Text="Home" />
    <h2>
        <%= table.DisplayName %></h2>
    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="Elenco degli errori di validazione" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1" 
                Display="None" />
            <asp:FilterRepeater ID="FilterRepeater" runat="server" >
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("DisplayName") %>' AssociatedControlID="DynamicFilter$DropDownList1" />
                    <asp:DynamicFilter runat="server" ID="DynamicFilter" OnSelectedIndexChanged="OnFilterSelectedIndexChanged"  />
                </ItemTemplate>
                <FooterTemplate>
                    <br />
                    <br />
                </FooterTemplate>
            </asp:FilterRepeater>
            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource" AllowPaging="True"
                AllowSorting="True" CssClass="gridview" OnRowDeleted="GridView1_RowDeleted">
                <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altRow" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="EditHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'
                                Text="Modifica" />&nbsp;<asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                                    CausesValidation="false" Text="Cancella" OnClientClick='return confirm("Confermi la cancellazione di questa voce?");' />&nbsp;<asp:HyperLink
                                        ID="DetailsHyperLink" runat="server" NavigateUrl='<%# table.GetActionPath(PageAction.Details, GetDataItem()) %>'
                                        Text="Dettaglio" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="footer" />
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    <%--There are currently no items in this table.--%>
                    Nessuna voce trovata.
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true">
                <WhereParameters>
                    <asp:DynamicControlParameter ControlId="FilterRepeater" />
                </WhereParameters>
            </asp:LinqDataSource>
            <br />
            <div class="bottomhyperlink">
                <asp:HyperLink ID="InsertHyperLink" runat="server"><img runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Aggiungi nuova voce" />Aggiungi nuova voce</asp:HyperLink>
            </div>
            <br />
            <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:content>
