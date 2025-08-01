<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportCustomizer.ascx.cs"
            Inherits="LabExtim.CustomControls.ReportCustomizer" %>
<link href="../Site.css" rel="stylesheet" type="text/css" />
<div>
    <asp:DataList ID="dlsReportTexts" runat="server" RepeatLayout="Table" ShowHeader="true"
                  OnItemDataBound="dlsReportTexts_ItemDataBound" OnItemCommand="dlsReportTexts_ItemCommand">
        <HeaderTemplate>
            <table class="ReportCustomizer">
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="lblDescription" Text="Tipo testo" runat="server" Width="110" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblText" Text="Testo" runat="server" Width="300" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblDefault" Text="Default" runat="server" Width="50" />
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table class="ReportCustomizer">
                <tr>
                    <td style="width: 110px">
                        <asp:Label ID="lblReportTextType" runat="server" Width="110" Text='<%# DataBinder.Eval(Container.DataItem, "Value") %>'></asp:Label>
                    </td>
                    <td style="width: 300px">
                        <asp:TextBox ID="txtText" runat="server" Rows="5" Width="300" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td style="width: 50px; text-align: center">
                        <asp:ImageButton ID="ibtDefault" runat="server" ImageUrl="~/Images/pencil.png" CommandName="SaveAsDefault"
                                         ToolTip="Salva come default voce corrente" OnClientClick=" javascript:return confirm('Vuoi salvare questo testo come default per la voce corrente?'); ">
                        </asp:ImageButton>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:DataList>
    <table style="width: 480px; text-align: center">
        <tr>
            <td style="width: 100%; text-align: center">
                <asp:Button ID="btnUseCurrent" runat="server" CssClass="ReportCustomizer" Text="Usa testi correnti"
                            OnClick="btnUseCurrent_Click"></asp:Button>
                &nbsp;
                <asp:Button ID="btnSaveAsDefault" runat="server" CssClass="ReportCustomizer" Text="Salva come default generale"
                            OnClick="btnSaveAsMainDefault_Click" OnClientClick=" javascript:return confirm('Vuoi salvare questi testi come default per il tipo di report corrente?'); ">
                </asp:Button>
            </td>
        </tr>
    </table>
</div>