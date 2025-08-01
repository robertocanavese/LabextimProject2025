<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PODetailsNotes.aspx.cs"
    Inherits="LabExtim.PODetailsNotes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="~/Site.css" rel="stylesheet" type="text/css" />
  
</head>
<body style="font-family: Verdana, Geneva, Tahoma, sans-serif; background-color: #FBFBFB">
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true"
            EnableScriptGlobalization="true">
        </asp:ToolkitScriptManager>
        <asp:Panel ID="DetailsPanel" runat="server" >
            <h5 style="text-align: center">
                Ordine di Produzione
                <asp:Label ID="lblItemNo" runat="server" />
            </h5>
        </asp:Panel>
        <h5>
            <asp:Label ID="lblDetails" runat="server" Text="Dettaglio annotazioni operazioni" />
        </h5>
        <asp:GridView ID="grdProductionOrderDetails" runat="server" CssClass="gridview" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ItemDescription" HeaderText="Fase"  />
                <asp:BoundField DataField="UniqueName" HeaderText="Operatore" DataFormatString="" />
                <asp:BoundField DataField="ProductionDate" HeaderText="Data operazione" dataformatstring="{0:dd/MM/yyyy}" HtmlEncode="false" />
                <asp:BoundField DataField="Note" HeaderText="Annotazioni"  />
            </Columns>
            <EmptyDataTemplate>
                Nessuna voce trovata.
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
