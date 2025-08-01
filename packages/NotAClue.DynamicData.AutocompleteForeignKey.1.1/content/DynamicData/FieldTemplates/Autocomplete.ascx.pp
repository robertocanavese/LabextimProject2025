<%@ Control Language="C#" Inherits="$rootnamespace$.AutocompleteField" Codebehind="Autocomplete.ascx.cs" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />

