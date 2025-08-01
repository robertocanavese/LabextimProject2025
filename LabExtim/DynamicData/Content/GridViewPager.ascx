<%@ Control Language="C#" CodeBehind="GridViewPager.ascx.cs" Inherits="LabExtim.DynamicData.Content.GridViewPager" %>

<div class="pager">
    <span class="results1">
        <asp:ImageButton AlternateText="Prima pagina" ToolTip="Prima pagina" ID="ImageButtonFirst" runat="server" ImageUrl="Images/PgFirst.gif" Width="8" Height="9" CommandName="Page" CommandArgument="First" />
        &nbsp;
        <asp:ImageButton AlternateText="Pagina precedente" ToolTip="Pagina precedente" ID="ImageButtonPrev" runat="server" ImageUrl="Images/PgPrev.gif" Width="5" Height="9" CommandName="Page" CommandArgument="Prev" />
        &nbsp;
        <asp:Label ID="LabelPage" runat="server" Text="Pagina " AssociatedControlID="TextBoxPage" />
        <asp:TextBox ID="TextBoxPage" runat="server" Columns="5" AutoPostBack="true" ontextchanged="TextBoxPage_TextChanged" Width="25px" CssClass="droplist" />
        di
        <asp:Label ID="LabelNumberOfPages" runat="server" />
        &nbsp;
        <asp:ImageButton AlternateText="Pagina successiva" ToolTip="Pagina successiva" ID="ImageButtonNext" runat="server" ImageUrl="Images/PgNext.gif" Width="5" Height="9" CommandName="Page" CommandArgument="Next" />
        &nbsp;
        <asp:ImageButton AlternateText="Ultima pagina" ToolTip="Ultima pagina" ID="ImageButtonLast" runat="server" ImageUrl="Images/PgLast.gif" Width="8" Height="9" CommandName="Page" CommandArgument="Last" />
    </span>
    <span class="results2">
        <asp:Label ID="LabelRows" runat="server" Text="Voci per pagina:" AssociatedControlID="DropDownListPageSize" />
        <asp:DropDownList ID="DropDownListPageSize" runat="server" AutoPostBack="true" CssClass="droplist" onselectedindexchanged="DropDownListPageSize_SelectedIndexChanged">
            <asp:ListItem Value="10" />
            <asp:ListItem Value="20" />
            <asp:ListItem Value="30" />
            <asp:ListItem Value="50" />
            <asp:ListItem Value="100" />
            <asp:ListItem Value="Tutte" />
        </asp:DropDownList>
    </span>
</div>