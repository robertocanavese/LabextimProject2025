<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True"
    CodeBehind="QuotationConsole2.aspx.cs" Inherits="LabExtim.QuotationConsole2" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var validNavigation = false;
        //var ajaxLoading0 = false;

        function ClosingEvent() {
            var dont_confirm_leave = 0;
            var leave_message = 'LabExtim: vuoi veramente uscire e perdere le modifiche al preventivo non salvate?';
            function goodbye(e) {

                var isSyncro = 'False';
                //                if (!ajaxLoading0) {
                //                    ajaxLoading0 = true;
                v_quotationParameter = '<%= QuotationParameter %>';
                v_sessionUserId = '<%= GetCurrentEmployee().ID.ToString() %>';
                $.ajax({
                    type: "POST",
                    async: false,
                    url: document.location.href.split("?")[0].split("#")[0] + "/GetIsSyncronized",
                    data: JSON.stringify({ quotationParameter: v_quotationParameter, sessionUserId: v_sessionUserId }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (msg) {
                        //ajaxLoading0 = false;
                        if (msg.d.length > 0) {
                            isSyncro = msg.d;

                        }
                    },
                    error: function () {
                        //ajaxLoading0 = false;
                    }
                });

                if (!validNavigation) {
                    if (isSyncro == 'False') {
                        if (dont_confirm_leave !== 1) {
                            if (!e) e = window.event;
                            e.cancelBubble = true;
                            e.returnValue = leave_message;
                            if (e.stopPropagation) {
                                e.stopPropagation();
                                e.preventDefault();
                            }
                            return leave_message;
                        }
                    }
                }
                validNavigation = false;

                //}

            }
            window.onbeforeunload = goodbye;
            $('document').bind('keypress', function (e) {
                if (e.keyCode == 116) {
                    validNavigation = true;
                }
                if (e.keyCode == 115) {
                    validNavigation = false;
                }
            });

            $("a.closers").bind('click', function () {
                validNavigation = false;
            });
            $("a:not(.closers)").bind("click", function () {
                validNavigation = true;
            });
            $("form").bind("submit", function () {
                validNavigation = true;
            });
            $("input[type=submit]").bind("click", function () {
                validNavigation = true;
            });
            $("input,img,div").bind('click', function () {
                validNavigation = true;
            });

            $("#ctl00_ContentPlaceHolder1_lbtNewQuotation").bind('click', function (e) {
                validNavigation = true;
                var isSyncro = 'False';
                //                if (!ajaxLoading0) {
                //                    ajaxLoading0 = true;
                v_quotationParameter = '<%= QuotationParameter %>';
                v_sessionUserId = '<%= GetCurrentEmployee().ID.ToString() %>';
                controlId = $(this).attr('id')
                $.ajax({
                    type: "POST",
                    async: false,
                    url: document.location.href.split("?")[0].split("#")[0] + "/GetIsSyncronized",
                    data: JSON.stringify({ quotationParameter: v_quotationParameter, sessionUserId: v_sessionUserId }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (msg) {
                        //ajaxLoading0 = false;
                        if (msg.d.length > 0) {
                            isSyncro = msg.d;

                        }
                    },
                    error: function () {
                        //ajaxLoading0 = false;
                    }
                });
                if (isSyncro == 'False') {
                    if (confirm('LabExtim: vuoi veramente uscire e perdere le modifiche al preventivo non salvate?')) {
                        WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(controlId, '', true, '', '', false, true));
                    }
                    else {
                        validNavigation = false;
                        return false;
                    }
                }
                else { WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(controlId, '', true, '', '', false, true)); }
                //}

            });






        }


        function SetAutoComplete() {

            $("#ctl00_ContentPlaceHolder1_dtvQuotation_txtSearchCli").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: document.location.href.split("?")[0].split("#")[0] + '/GetCustomers',
                        data: "{ 'q': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        success: function (result) {
                            if (result.hasOwnProperty("d")) { result = result.d; }
                            var data = jQuery.parseJSON(result);
                            response($.map(data, function (item) {
                                return {
                                    label: item.Name,
                                    value: item.Code,
                                    markUp: item.MarkUp
                                }
                            }))
                        },
                        error: function (response) {
                            //alert(response.responseText);
                        },
                        failure: function (response) {
                            //alert(response.responseText);
                        }
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    //log(ui.item ? ui.item.label : this.label);
                    $("#ctl00_ContentPlaceHolder1_dtvQuotation_txtSearchCli").val(ui.item.label);
                    $("#ctl00_ContentPlaceHolder1_dtvQuotation_hidSearchCli").val(ui.item.value);
                    $("#ctl00_ContentPlaceHolder1_dtvQuotation_txtMarkUp").val(ui.item.markUp);
                    document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;
                    return false;
                },
                open: function () {
                    //$(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    //$(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }
            });


        }


        $(document).ready(function () {


            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {

                SetAutoComplete();

            }

            SetAutoComplete();
            ClosingEvent();

        });



        //var prm = Sys.WebForms.PageRequestManager.getInstance();

        //prm.add_endRequest(function () {
        //    //$('#ctl00_tblHeader').children().find('*:not(.tabbed)').attr('tabindex', -1);
        //    //$('#ctl00_divMain').children().find('*:not(.tabbed)').attr('tabindex', -1);


        //});

        function SetSelectedQuantity() {
            re = new RegExp("dtvQuotation_rbtQ");
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i];
                if (elm.type == 'radio') {
                    if (re.test(elm.id)) {
                        if (elm.checked == true) {
                            quantity = document.getElementById(elm.id.replace("rbt", "txt")).value;
                            break;
                        }
                    }
                }
            }
            return quantity;
        }

        function SetParametersAndOpenPopUp() {

            OpenItem('ProductionOrderPopup.aspx?' +
                    'POquo=' + '<% = QuotationParameter %>' + '&' +
                    'POName=' + '<% = Server.UrlEncode(QuotationHeader.Value ?? " ").Replace("'","%27") %>' + '&' +
                    'POcid=0&' +
                    'POq=' + SetSelectedQuantity());
            return false;

        }




    </script>

    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <table style="width: 100%">
        <tr>
            <td class="h2" style="width: 85%">
                <asp:Label ID="lblQuotationHeader" runat="server"></asp:Label>
            </td>
            <td style="width: 15%; text-align: right">
                <asp:LinkButton ID="lbtNewQuotation" runat="server" Text="Nuovo preventivo" OnClick="lbtNewQuotation_Click"> 
                </asp:LinkButton>
            </td>
        </tr>
    </table>
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <table class="notTabbed" width="900">
        <tr>
            <td>
                <asp:Panel ID="DetailsPanel" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="3">
                                <asp:Menu ID="mnuOperations1" runat="server" Orientation="Horizontal" CssClass="menu"
                                    DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                    DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                                    <DynamicMenuStyle CssClass="menu" />
                                </asp:Menu>
                                <asp:Menu ID="mnuOperations2" runat="server" Orientation="Horizontal" CssClass="menuBlue"
                                    DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                    DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuOperations_MenuItemClick">
                                    <DynamicMenuStyle CssClass="menuBlue" />
                                </asp:Menu>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Menu ID="mnuQuotationTemplates" runat="server" Orientation="Horizontal" CssClass="menuYellow"
                                    DisappearAfter="3000" DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False"
                                    DynamicHorizontalOffset="0" DynamicVerticalOffset="0" OnMenuItemClick="mnuQuotationTemplates_MenuItemClick">
                                    <DynamicMenuStyle CssClass="menuYellow" />
                                </asp:Menu>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 41%">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                    HeaderText="Elenco degli errori di validazione" />
                                <asp:DynamicValidator runat="server" ID="DynamicValidator1" ControlToValidate="dtvQuotation"
                                    Display="None" />
                                <asp:DetailsView ID="dtvQuotation" runat="server" FieldHeaderStyle-CssClass="bold"
                                    AutoGenerateEditButton="true" DefaultMode="ReadOnly" CssClass="detailstable"
                                    AutoGenerateRows="false" DataSourceID="DetailsDataSource" OnItemCreated="dtvQuotation_ItemCreated"
                                    Width="270" OnItemUpdated="dtvQuotation_ItemUpdated" OnDataBound="dtvQuotation_DataBound"
                                    OnItemUpdating="dtvQuotation_ItemUpdating">
                                    <FieldHeaderStyle CssClass="bold" />
                                    <Fields>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblNumber" runat="server" Text="Numero" Width="120"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycNumber" runat="server" DataField="Number" UIHint="Text" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text="Data creazione" Width="120"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycDate" runat="server" DataField="Date" UIHint="DateTime"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblUpdateDate" runat="server" Text="Data aggiornamento" Width="120"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycUpdateDate" runat="server" DataField="UpdateDate" UIHint="DateTime"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField >
                                            <HeaderTemplate>
                                                <asp:Label ID="lblManager" runat="server" Text="Gestione"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%--<asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey" />--%>
                                                <asp:DropDownList ID="ddlManagers" runat="server"  CssClass="droplist" DataSourceID="ldsManagers" DataTextField="Description" DataValueField="ID" ></asp:DropDownList>
                                                <asp:LinqDataSource ID="ldsManagers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext" OrderBy="Id" TableName="Managers">
                                        </asp:LinqDataSource>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" AllowNullValue="false" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycManager" runat="server" DataField="Manager" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblSubject" runat="server" Text="Titolo"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycSubject" runat="server" DataField="Subject" UIHint="Text" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycSubject" runat="server" DataField="Subject" UIHint="Text250_Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycSubject" runat="server" DataField="Subject" UIHint="Text250_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblOwner" runat="server" Text="Utente"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DynamicControl ID="dycOwner" runat="server" DataField="Employee1" UIHint="ForeignKey" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycOwner" runat="server" DataField="Employee1" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycOwner" runat="server" DataField="Employee1" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQ1" runat="server" Text="Quantità 1"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQ1" runat="server" Width="45" MaxLength="7" CssClass="droplist"
                                                    Text='<%# Eval("Q1") %>'></asp:TextBox>
                                                <asp:RadioButton ID="rbtQ1" runat="server" Checked="true" GroupName="selectQuantity" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP1" runat="server" Checked='<%# Eval("P1") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycQ1" runat="server" DataField="Q1" UIHint="Integer_Edit" Mode="Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP1" runat="server" DataField="P1" UIHint="Boolean_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycQ1" runat="server" DataField="Q1" UIHint="Integer_Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP1" runat="server" DataField="P1" UIHint="Boolean_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQ2" runat="server" Text="Quantità 2"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQ2" runat="server" Width="45" MaxLength="7" CssClass="droplist"
                                                    Text='<%# Eval("Q2") %>'></asp:TextBox>
                                                <asp:RadioButton ID="rbtQ2" runat="server" GroupName="selectQuantity" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP2" runat="server" Checked='<%# Eval("P2") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycQ2" runat="server" DataField="Q2" UIHint="Integer_Edit"
                                                    Mode="Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP2" runat="server" DataField="P2" UIHint="Boolean_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycQ2" runat="server" DataField="Q2" UIHint="Integer_Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP2" runat="server" DataField="P2" UIHint="Boolean_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQ3" runat="server" Text="Quantità 3"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQ3" runat="server" Width="45" MaxLength="7" CssClass="droplist"
                                                    Text='<%# Eval("Q3") %>'></asp:TextBox>
                                                <asp:RadioButton ID="rbtQ3" runat="server" GroupName="selectQuantity" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP3" runat="server" Checked='<%# Eval("P3") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycQ3" runat="server" DataField="Q3" UIHint="Integer_Edit"
                                                    Mode="Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP3" runat="server" DataField="P3" UIHint="Boolean_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycQ3" runat="server" DataField="Q3" UIHint="Integer_Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP3" runat="server" DataField="P3" UIHint="Boolean_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQ4" runat="server" Text="Quantità 4"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQ4" runat="server" Width="45" MaxLength="7" CssClass="droplist"
                                                    Text='<%# Eval("Q4") %>'></asp:TextBox>
                                                <asp:RadioButton ID="rbtQ4" runat="server" GroupName="selectQuantity" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP4" runat="server" Checked='<%# Eval("P4") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycQ4" runat="server" DataField="Q4" UIHint="Integer_Edit"
                                                    Mode="Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP4" runat="server" DataField="P4" UIHint="Boolean_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycQ4" runat="server" DataField="Q4" UIHint="Integer_Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP4" runat="server" DataField="P4" UIHint="Boolean_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQ5" runat="server" Text="Quantità 5"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQ5" runat="server" Width="45" MaxLength="7" CssClass="droplist"
                                                    Text='<%# Eval("Q5") %>'></asp:TextBox>
                                                <asp:RadioButton ID="rbtQ5" runat="server" GroupName="selectQuantity" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:CheckBox ID="chkP5" runat="server" Checked='<%# Eval("P5") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycQ5" runat="server" DataField="Q5" UIHint="Integer_Edit"
                                                    Mode="Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP5" runat="server" DataField="P5" UIHint="Boolean_Edit" Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycQ5" runat="server" DataField="Q5" UIHint="Integer_Edit" />
                                                &nbsp;&nbsp;<asp:Image runat="server" ImageUrl="Images/printer.png" ToolTip="Stampa su preventivo" /><asp:DynamicControl ID="dycP5" runat="server" DataField="P5" UIHint="Boolean_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text="Cliente"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtSearchCli" MaxLength="255" Width="200" ToolTip="Digitare almeno 3 caratteri per avviare la ricerca" CssClass="droplist" />
                                                <asp:HiddenField ID="hidSearchCli" runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycCustomer" runat="server" DataField="Customer" UIHint="ForeignKey_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblMarkUp" runat="server" Text="% Ric com"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMarkUp" runat="server" Width="40" MaxLength="3" CssClass="droplist"
                                                    Text='<%# Eval("MarkUp") %>' CausesValidation="true"></asp:TextBox><%--AutoPostBack="True"--%>
                                                <asp:CustomValidator runat="server" ID="CustomValidator2" CssClass="droplist" ControlToValidate="txtMarkUp"
                                                    Text="Valore inferiore a 100" OnServerValidate="CustomValidator2_ServerValidate" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Integer_Edit"
                                                    Mode="Edit" />
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:DynamicControl ID="dycMarkUp" runat="server" DataField="MarkUp" UIHint="Integer_Edit" />
                                            </InsertItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                </asp:DetailsView>
                            </td>
                            <td style="width: 25%" valign="top" align="center">
                                <table cellpadding="5" class="center">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnEmpty" runat="server" OnClick="btnEmpty_Click" Width="130px" Text="Svuota preventivo" CssClass="myButton"
                                                OnClientClick="return confirm('Sei sicuro di voler cancellare tutte le voci del preventivo?')" />
                                        </td>
                                    </tr>
                                    <%--<tr>
                                                <td>

                                                    <asp:LinkButton ID="lbtLoadTemplate" runat="server" Text="Svuota preventivo e carica modello"
                                                        OnClick="lbtLoadTemplate_Click" />
                                                    <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="droplist" Width="250px"
                                                        OnDataBound="ddlTemplates_DataBound">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>--%>
                                    <tr>
                                        <td style="background-color: #f7f868">
                                            <asp:Label runat="server" Text="SALVA COME MODELLO" />
                                            <asp:DropDownList ID="ddlTypes" runat="server" CssClass="droplist" Width="250px" OnDataBound="ddlTypes_DataBound"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlItemTypes" runat="server" CssClass="droplist" Width="250px" OnDataBound="ddlItemTypes_DataBound"></asp:DropDownList>
                                            <asp:Label ID="lblTemplateDescription" runat="server" Text="Inserire una descrizione per il nuovo modello"></asp:Label>
                                            <asp:TextBox ID="txtTemplateDescription" runat="server" CssClass="gridview" Width="250px"></asp:TextBox>
                                            <asp:Label ID="lblOrder" runat="server" Text="Inserire stringa ordinamento"></asp:Label>
                                            <asp:TextBox ID="txtOrder" runat="server" CssClass="gridview" Width="250px" MaxLength="50"></asp:TextBox>
                                            <asp:Button ID="lbtSaveAsTemplate" runat="server" Text="Salva come modello" OnClick="lbtSaveAsTemplate_Click1" CssClass="myButton" />
                                        </td>
                                    </tr>
                                    <asp:CustomValidator runat="server" ID="CustomValidator1" CssClass="droplist" ControlToValidate="txtTemplateDescription"
                                        Text="Valore già esistente" OnServerValidate="CustomValidator1_ServerValidate" />
                                    <tr>
                                        <td style="background-color: #f2f2f2">
                                            <asp:Label ID="lblSaveAsMain" runat="server" Text="SALVA CON NOME" Width="250px"></asp:Label>
                                            <asp:Label ID="lblSaveAs" runat="server" Text="Inserire una descrizione" Width="250px"></asp:Label>
                                            <asp:TextBox ID="txtQuotationDescription" runat="server" CssClass="gridview" Width="250px"
                                                Height="40px" Rows="2" Font-Bold="True" TextMode="MultiLine"></asp:TextBox>
                                            <asp:Button ID="lbtSaveAs" runat="server" Text="Salva con nome" OnClick="lbtSaveAs_Click"
                                                CssClass="myButton" Width="140px" /><br />
                                            <asp:CheckBox ID="chkUpdateNewQuotation" runat="server" Checked="true" Text="Aggiorna dati" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="lbtSave" runat="server" Text="Salva preventivo" Width="140" CssClass="myButton"
                                                OnClick="lbtSave_Click" OnClientClick="javascript:return confirm('Sovrascrivo il preventivo esistente con questa nuova copia modificata?');" />
                                        </td>
                                    </tr>
                                    <asp:CustomValidator runat="server" ID="CustomValidator3" CssClass="droplist" ControlToValidate="txtQuotationDescription"
                                        Text="Valore già esistente per il Cliente" OnServerValidate="CustomValidator3_ServerValidate" />
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnRecalc" runat="server" Enabled="false" OnClick="btnRecalc_Click" CssClass="myButton"
                                                Text="Ricalcola voci" Width="140px" />
                                        </td>
                                    </tr>
                                    <asp:LinqDataSource ID="ldsToolsTotals" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                        TableName="" OnSelecting="ldsToolsTotals_Selecting">
                                    </asp:LinqDataSource>
                                    <asp:LinqDataSource ID="ldsTotals" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                        TableName="" OnSelecting="ldsTotals_Selecting">
                                    </asp:LinqDataSource>
                                    <%--<asp:LinqDataSource ID="ldsCustomers" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                                TableName="" OnSelecting="ldsCustomers_Selecting">
                                            </asp:LinqDataSource>--%>
                                </table>
                            </td>
                            <td style="width: 33%" valign="top" align="left">
                                <asp:GridView ID="grdToolsTotals" runat="server" AutoGenerateColumns="false" DataSourceID="ldsToolsTotals"
                                    CellPadding="5" CssClass="gridview"
                                    Width="340">
                                    <HeaderStyle Font-Bold="true" HorizontalAlign="Center" />
                                    <RowStyle HorizontalAlign="Right" />
                                    <Columns>
                                        <asp:BoundField DataField="CstToolsTot" DataFormatString="{0:N2}" ItemStyle-Width="170"
                                            HeaderText="Costo Impianti" />
                                        <asp:BoundField DataField="PrcToolsTot" DataFormatString="{0:N2}" ItemStyle-Width="170"
                                            HeaderText="Prezzo Impianti" ItemStyle-Font-Bold="true"
                                            ItemStyle-BackColor="Yellow" />
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <asp:GridView ID="grdTotals" runat="server" AutoGenerateColumns="false" DataSourceID="ldsTotals"
                                    CellPadding="5" CssClass="gridview"
                                    Width="510">
                                    <HeaderStyle Font-Bold="true" HorizontalAlign="Center" />
                                    <RowStyle HorizontalAlign="Right" />
                                    <Columns>
                                        <asp:BoundField DataField="Quantity" DataFormatString="{0:N0}" HeaderText="Quantità"
                                            ItemStyle-Font-Bold="true" ItemStyle-Width="80" />
                                        <asp:BoundField DataField="CstProdTot" DataFormatString="{0:N2}" ItemStyle-Width="90"
                                            HeaderText="Cst Prod Tot" />
                                        <asp:BoundField DataField="PrcProdUni" DataFormatString="{0:N3}" ItemStyle-Width="80"
                                            ItemStyle-Font-Bold="true" HeaderText="Prz Prod Uni"
                                            ItemStyle-BackColor="Yellow" />
                                        <asp:BoundField DataField="PrcProdTot" DataFormatString="{0:N2}" ItemStyle-Width="90"
                                            HeaderText="Prz Prod Tot" ItemStyle-BackColor="Yellow" />
                                        <asp:BoundField DataField="PrcUni" DataFormatString="{0:N3}" ItemStyle-Width="80"
                                            HeaderText="Prz Compl Uni" ItemStyle-Font-Bold="true" />
                                        <asp:BoundField DataField="PrcTot" DataFormatString="{0:N2}" ItemStyle-Width="90"
                                            HeaderText="Prz Compl Tot" />
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <table class="gridview">
                                    <tr>
                                        <th>
                                            <asp:Label ID="lblDescription" runat="server" Text="Descrizione lavorazione" Font-Bold="true"> </asp:Label>
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtDescription" runat="server" Width="400px" TextMode="MultiLine"
                                                Rows="5"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <asp:Label ID="lblPriceCom" runat="server" Text="Note tecniche da OdP precedenti" Font-Bold="true"> </asp:Label>
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtPriceCom" runat="server" Width="400px" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <asp:Label ID="lblPrintingMainText" runat="server" Text="Testo stampa preventivo" Font-Bold="true"> </asp:Label>
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtPrintingMainText" runat="server" Width="400px" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <asp:Label ID="lblAppunti1" runat="server" Text="Appunti" Font-Bold="true"> </asp:Label>
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtAppunti1" runat="server" Width="400px" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableDelete="false" EnableInsert="false"
                        EnableUpdate="true" TableName="TempQuotations" ContextTypeName="DLLabExtim.QuotationDataContext">
                        <WhereParameters>
                            <asp:QueryStringParameter Name="ID_Quotation" QueryStringField="P0" Type="Int32" />
                        </WhereParameters>
                    </asp:LinqDataSource>
                </asp:Panel>
                <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdQuotationDetails"
                    Display="None" />
                &nbsp;<asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                <h3>Dettaglio&nbsp;
                            <asp:LinkButton ID="lbtAddUnmanagedItem" runat="server" CssClass="gridview" Font-Bold="False"
                                ForeColor="Olive" Text="Aggiungi voce libera" OnClick="lbtAddUnmanagedItem_Click" />
                    &nbsp;
                            <asp:LinkButton ID="lbtCreateProductionOrder" runat="server" CssClass="gridview"
                                Font-Bold="False" Text="Crea Ordine di Produzione" OnClientClick="javascript:return SetParametersAndOpenPopUp();" />
                    &nbsp;
                            <asp:LinkButton ID="hypListProductionOrders" runat="server" CssClass="gridview" Font-Bold="False"
                                Text="Elenco Ordini di Produzione" />
                    &nbsp;
                            <asp:LinkButton ID="lbtPrintQuotation" runat="server" CssClass="gridview" Font-Bold="False"
                                OnClick="lbtPrintQuotation_Click" Text="Stampa preventivo Cliente" />
                    &nbsp;
                            <asp:LinkButton ID="lbtPrintQuotationTechnical" runat="server" CssClass="gridview"
                                Font-Bold="False" OnClick="lbtPrintQuotationTechnical_Click"
                                Text="Stampa preventivo tecnico" />
                    &nbsp;
                </h3>
                <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" EnableUpdate="true"
                    ContextTypeName="DLLabExtim.QuotationDataContext" TableName="TempQuotationDetails"
                    OnDeleted="GridDataSource_Deleted" OnSelecting="GridDataSource_Selecting">
                </asp:LinqDataSource>
                <asp:GridView ID="grdQuotationDetails" runat="server" AutoGenerateColumns="false"
                    AutoGenerateDeleteButton="true" AllowPaging="True" DataSourceID="GridDataSource"
                    AllowSorting="false" CssClass="gridview" OnPageIndexChanging="grdQuotationDetails_PageIndexChanging"
                    OnRowDataBound="grdQuotationDetails_RowDataBound" OnPreRender="grdQuotationDetails_PreRender"
                    OnRowCommand="grdQuotationDetails_RowCommand" OnRowCreated="grdQuotationDetails_RowCreated"
                    OnDataBound="grdQuotationDetails_DataBound"
                    DataKeyNames="ID,SessionUser,ID_Quotation">
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altRow" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="~/Images/pencil.png" Height="10"
                                    Width="10" ToolTip="Modifica voce libera" Visible="false" CommandName="Edit" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtEdit" runat="server" ImageUrl="~/Images/disk.png" Height="10"
                                    Width="10" ToolTip="Salva voce libera" CommandName="Update" CausesValidation="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:DynamicField DataField="ID" HeaderText="" Visible="false" />
                        <asp:DynamicField DataField="SessionUser" HeaderText="" Visible="false" />
                        <asp:DynamicField DataField="ID_Quotation" HeaderText="" Visible="false" />

                        <asp:DynamicField DataField="ID_QuotationDetail" HeaderText="ID_QuotationDetail"
                            UIHint="Text" Visible="false" />
                        <asp:DynamicField DataField="Company" HeaderText="Esecuzione" UIHint="ForeignKey" />
                        <asp:DynamicField DataField="Type" HeaderText="Tipo fase" UIHint="ForeignKey" />
                        <asp:DynamicField DataField="ItemType" HeaderText="Tipo prodotto" UIHint="ForeignKey" />
                        <asp:DynamicField DataField="ItemTypeDescription" HeaderText="Descrizione prodotto"
                            UIHint="Text" />
                        <asp:DynamicField DataField="Unit" HeaderText="UM" UIHint="ForeignKey" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblQuantity" Text="Quantità" runat="server"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server" Width="50" MaxLength="7" CssClass="droplist tabbed"
                                    ToolTip="Inserire un valore intero, decimale con virgola o frazionario" AutoCompleteType="None"> </asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server" Width="50" MaxLength="7" CssClass="droplist tabbed"
                                    Enabled="false" AutoCompleteType="None" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:DynamicField DataField="Multiply" HeaderText="Moltiplica" UIHint="Boolean" />
                        <asp:DynamicField DataField="Cost" HeaderText="Cst Uni" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                        <asp:DynamicField DataField="TotalCost" HeaderText="Costo Totale" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                        <asp:DynamicField DataField="Percentage" HeaderText="% Ric std" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                        <asp:DynamicField DataField="MarkUp" HeaderText="% Ric com" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                        <asp:DynamicField DataField="Price" HeaderText="Prz Uni" UIHint="Text" ItemStyle-HorizontalAlign="Right" />
                        <asp:DynamicField DataField="TotPrice" HeaderText="Prz Tot" UIHint="Text" ItemStyle-HorizontalAlign="Right"
                            DataFormatString="{0:N2}" />
                        <asp:DynamicField DataField="Supplier" HeaderText="Fornitore" UIHint="ForeignKey" />
                    </Columns>
                    <PagerStyle CssClass="footer" />
                    <PagerTemplate>
                        <asp:GridViewPager ID="Pager0" runat="server" />
                    </PagerTemplate>
                    <EmptyDataTemplate>
                        Nessuna voce trovata.
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
