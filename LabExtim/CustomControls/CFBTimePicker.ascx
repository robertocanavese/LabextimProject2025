<%@ Control Language="C#" AutoEventWireup="true" Inherits="LabExtim.CustomControls.CFBTimePicker" CodeBehind="CFBTimePicker.ascx.cs" %>
<style type="text/css">
    .Dynamic-lblAnno {
        font-size: 10px;
        font-weight: bold;
        width: 100px;
    }

    .Dynamic-MNBS {
        font-size: 10px;
        font-weight: bold;
    }

    .Dynamic-SelAnno, .Dynamic-SelPeriodo, .Dynamic-SelGiorno, .Dynamic-SelMese, .Dynamic-SelSettimana, Dynamic-Ddl {
        margin-right: 2px;
        display: inline;
    }

    .panelDataChooser { display: inline; }
</style>
<asp:UpdatePanel ID="_UpdatePanel" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Panel ID="_BasicPanel" runat="server">
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>