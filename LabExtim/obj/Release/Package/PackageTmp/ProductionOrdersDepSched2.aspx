<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProductionOrdersDepSched2.aspx.cs" Inherits="LabExtim.ProductionOrdersDepSched2"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<%--<%@ Register Src="~/CustomControls/FloatTextBox.ascx" TagName="FloatTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/IntTextBox.ascx" TagName="IntTextBox" TagPrefix="cfb" %>
<%@ Register Src="~/CustomControls/YearCounterTextBox.ascx" TagName="YearCounterTextBox"
    TagPrefix="cfb" %>--%>
<%@ Register Src="~/CustomControls/SearchEngine.ascx" TagName="SearchEngine" TagPrefix="cfb" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {

            let ajaxLoading = false;

            function clickSearch() {
                $('#ctl00_ContentPlaceHolder1_senMain_ibtSearch').trigger("click");
            }

            function EndRequestHandler() {

                $('.goToOperator').bind('click', goToOperator);
                $('.pause').bind('click', pauseRestartCurrentProdPhase);
                SetAutoCompleteInc();
                SetAutoCompleteEsc();

                SetStartDialog();
                SetStartDialogButtons();
                SetEndDialog();
                SetEndDialogButtons();
                SetForceEndDialog();
                SetForceEndDialogButtons();

                setInterval(clickSearch, 60000);

            }

            function SetStartDialogButtons() {
                $(".start").click(function (e) {
                    $('#hidSelPoId').val($(this).prev()[0].defaultValue.toString().split('|')[0]);
                    $('#hidSelQdId').val($(this).prev()[0].defaultValue.toString().split('|')[1]);
                    $(".startDialog").dialog("open");
                    return false;
                });
            }



            function SetStartDialog() {

                var objDetails = [];
                //var selectedMpId = 0;
                var selectedQdId = 0;

                $('#ddlPhase').change(function () {
                    // this.options[this.selectedIndex].text);
                    if (objDetails.length > 0) {
                        //selectedMpId = objDetails[this.selectedIndex].idProductionMp;
                        selectedQdId = objDetails[this.selectedIndex].idQuotationDetail;
                        $('#lblPhaseTime').text('Selezionata ' + objDetails[this.selectedIndex].pmDescription + "||Previsti " + objDetails[this.selectedIndex].prodTimeMin + ' minuti');
                        var selHtml = $('#lblPhaseTime').html().split('||');
                        $('#lblPhaseTime').html(selHtml[0] + '<br />' + selHtml[1]);
                    }
                });

                $(".startDialog").dialog({
                    modal: true,
                    autoOpen: false,
                    height: 'auto',
                    dialogClass: 'MyPopup',
                    width: 500,
                    title: "Avvia nuova fase di lavorazione",
                    open: function () {

                        $('#spnPoDesc').text('');
                        $('#lblPhaseTime').text('');
                        $("#ddlPhase").html('');

                        $(this).parent().children().children(".ui-dialog-titlebar-close").hide();
                        if (ajaxLoading == false) {
                            ajaxLoading = true;
                            params = $('#hidSelPoId').val();
                            $.ajax({
                                type: "POST",
                                url: document.location.href + "/GetProductionOrderOpenPhases",
                                data: JSON.stringify({ pipedParams: params }),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    ajaxLoading = false;
                                    if (result.hasOwnProperty("d")) { result = result.d; }
                                    var data = jQuery.parseJSON(result);
                                    $('#spnPoDesc').text(data.header);

                                    objDetails = [];
                                    var s = ''; //'<option value="-1">Please Select a Department</option>';
                                    for (var i = 0; i < data.details.length; i++) {
                                        objDetails.push(data.details[i]);
                                        s += '<option value="' + data.details[i].idQuotationDetail + '">' + data.details[i].pmDescription + '</option>';
                                    }
                                    $("#ddlPhase").html(s);
                                    $("#ddlPhase").trigger('change');
                                },
                                error: function (msg) {
                                    ajaxLoading = false;
                                    alert(msg.d)
                                }
                            });
                        }
                    },
                    buttons: [
                        {
                            text: "Annulla",
                            width: "100",
                            click: function () {
                                $(this).dialog("close");
                            }
                        },
                        {
                            text: "Conferma",
                            width: "100",
                            click: function () {

                                if (confirm('Sicuro di voler avviare questa fase? (l\'operazione non sarà più annullabile!)')) {

                                    $(this).dialog("close");
                                    if (ajaxLoading == false) {
                                        ajaxLoading = true;
                                        params = $('#hidSelPoId').val() + "|" + selectedQdId;
                                        $.ajax({
                                            type: "POST",
                                            url: document.location.href + "/SetCurrentProductionPhase",
                                            data: JSON.stringify({ userId: '<% =WebUser.Employee.ID%>', pipedParams: params }),
                                            dataType: "json",
                                            contentType: "application/json; charset=utf-8",
                                            success: function (msg) {
                                                ajaxLoading = false;
                                                //$(this).closest('.start')[0].visible = false;
                                                window.location.reload();
                                                //alert(msg.d)
                                            },
                                            error: function (msg) {
                                                ajaxLoading = false;
                                                alert(msg.d)
                                            }
                                        });
                                    }

                                }
                            }
                        }

                    ]
                });
                }


            function SetEndDialogButtons() {
                $(".end").click(function (e) {
                    $('#hidSelPoId').val($(this).prev()[0].defaultValue.toString().split('|')[0]);
                    $('#hidSelQdId').val($(this).prev()[0].defaultValue.toString().split('|')[1]);
                    $(".endDialog").dialog("close");
                    $(".endDialog").dialog("open");
                    return false;
                });
            }

            function SetEndDialog() {

                var objDetails = [];
                var selectedMpId = 0;
                var selectedQdId = 0;

                $(".endDialog").dialog({
                    modal: true,
                    autoOpen: false,
                    height: 'auto',
                    dialogClass: 'MyPopup',
                    width: 650,
                    title: "Chiusura fase di lavorazione",
                    open: function () {

                        $('#spnPoDescC').text('');
                        $('#lblPhaseTimeC').text('');

                        $(this).parent().children().children(".ui-dialog-titlebar-close").hide();
                        if (ajaxLoading == false) {
                            ajaxLoading = true;
                            params = $('#hidSelPoId').val() + "|" + $('#hidSelQdId').val();
                            $.ajax({
                                type: "POST",
                                url: document.location.href + "/GetCurrentPhaseTotalTime",
                                data: JSON.stringify({ pipedParams: params }),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    ajaxLoading = false;
                                    if (result.hasOwnProperty("d")) { result = result.d; }
                                    var data = jQuery.parseJSON(result);
                                    $('#spnPoDescC').text(data.header);
                                    $('#lblPhaseTimeC').text('Tempo lav. atteso: ' + data.detail.prodTimeMin.toString() + ' min - Tempo impiegato: ' + data.detail.prodEffMin.toString() + ' min - Diff: ' + (parseInt(data.detail.prodEffMin) - parseInt(data.detail.prodTimeMin)).toString() + ' min');
                                },
                                error: function (msg) {
                                    ajaxLoading = false;
                                    alert(msg.d)
                                }
                            });
                        }
                    },
                    buttons: [
                        {
                            text: "Annulla",
                            width: "100",
                            click: function () {
                                $(this).dialog("close");
                            }
                        },
                        {
                            text: "Chiudi fase e scarica materiali",
                            width: "250",
                            click: function () {

                                $(this).dialog("close");
                                goToOperator($('#hidSelPoId').val(), selectedQdId);

                            }
                        },
                        {
                            text: "Chiudi fase",
                            width: "100",
                            click: function () {

                                if (confirm('Sicuro di voler chiudere questa fase?')) {

                                    $(this).dialog("close");
                                    if (ajaxLoading == false) {
                                        ajaxLoading = true;
                                        params = $('#hidSelPoId').val() + "|" + $('#hidSelQdId').val();
                                        $.ajax({
                                            type: "POST",
                                            url: document.location.href + "/CloseCurrentProductionPhase",
                                            data: JSON.stringify({ userId: '<% =WebUser.Employee.ID%>', pipedParams: params }),
                                            dataType: "json",
                                            contentType: "application/json; charset=utf-8",
                                            success: function (msg) {
                                                ajaxLoading = false;
                                                window.location.reload();
                                            },
                                            error: function (msg) {
                                                ajaxLoading = false;
                                                alert(msg.d)
                                            }
                                        });
                                    }

                                }
                            }
                        }

                    ]
                });
                }





            ///////////////////////


            function SetForceEndDialogButtons() {
                $(".forceEnd").click(function (e) {
                    $('#hidSelPoId').val($(this).prev()[0].defaultValue.toString().split('|')[0]);
                    $('#hidSelQdId').val($(this).prev()[0].defaultValue.toString().split('|')[1]);
                    $(".forceEndDialog").dialog("open");
                    return false;
                });
            }



            function SetForceEndDialog() {

                var objDetails = [];
                //var selectedMpId = 0;
                var selectedQdId = 0;

                $('#ddlPhaseF').change(function () {
                    // this.options[this.selectedIndex].text);
                    if (objDetails.length > 0) {
                        //selectedMpId = objDetails[this.selectedIndex].idProductionMp;
                        selectedQdId = objDetails[this.selectedIndex].idQuotationDetail;
                        $('#lblPhaseTimeF').text('Selezionata ' + objDetails[this.selectedIndex].pmDescription + "||Previsti " + objDetails[this.selectedIndex].prodTimeMin + ' minuti');
                        var selHtml = $('#lblPhaseTimeF').html().split('||');
                        $('#lblPhaseTimeF').html(selHtml[0] + '<br />' + selHtml[1]);
                    }
                });

                $(".forceEndDialog").dialog({
                    modal: true,
                    autoOpen: false,
                    height: 'auto',
                    dialogClass: 'MyPopup',
                    width: 500,
                    title: "Forza chiusura fase di lavorazione",
                    open: function () {

                        $('#spnPoDescF').text('');
                        $('#lblPhaseTimeF').text('');
                        $("#ddlPhaseF").html('');

                        $(this).parent().children().children(".ui-dialog-titlebar-close").hide();
                        if (ajaxLoading == false) {
                            ajaxLoading = true;
                            params = $('#hidSelPoId').val();
                            $.ajax({
                                type: "POST",
                                url: document.location.href + "/GetProductionOrderOpenPhases",
                                data: JSON.stringify({ pipedParams: params }),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    ajaxLoading = false;
                                    if (result.hasOwnProperty("d")) { result = result.d; }
                                    var data = jQuery.parseJSON(result);
                                    $('#spnPoDescF').text(data.header);

                                    objDetails = [];
                                    var s = ''; //'<option value="-1">Please Select a Department</option>';
                                    for (var i = 0; i < data.details.length; i++) {
                                        objDetails.push(data.details[i]);
                                        s += '<option value="' + data.details[i].idQuotationDetail + '">' + data.details[i].pmDescription + '</option>';
                                    }
                                    $("#ddlPhaseF").html(s);
                                    $("#ddlPhaseF").trigger('change');
                                },
                                error: function (msg) {
                                    ajaxLoading = false;
                                    alert(msg.d)
                                }
                            });
                        }
                    },
                    buttons: [
                        {
                            text: "Annulla",
                            width: "100",
                            click: function () {
                                $(this).dialog("close");
                            }
                        },
                        {
                            text: "Forza chiusura fase e scarica materiali",
                            width: "250",
                            click: function () {

                                $(this).dialog("close");
                                goToOperator($('#hidSelPoId').val(), selectedQdId);

                            }
                        },
                        {
                            text: "Conferma",
                            width: "100",
                            click: function () {

                                if (confirm('Sicuro di voler forzare la chiusura questa fase? (l\'operazione non sarà più annullabile!)')) {

                                    $(this).dialog("close");
                                    if (ajaxLoading == false) {
                                        ajaxLoading = true;
                                        params = $('#hidSelPoId').val() + "|" + selectedQdId;
                                        $.ajax({
                                            type: "POST",
                                            url: document.location.href + "/ForceCloseCurrentProductionPhase",
                                            data: JSON.stringify({ userId: '<% =WebUser.Employee.ID%>', pipedParams: params }),
                                            dataType: "json",
                                            contentType: "application/json; charset=utf-8",
                                            success: function (msg) {
                                                ajaxLoading = false;
                                                //$(this).closest('.start')[0].visible = false;
                                                window.location.reload();
                                                //alert(msg.d)
                                            },
                                            error: function (msg) {
                                                ajaxLoading = false;
                                                alert(msg.d)
                                            }
                                        });
                                    }

                                }
                            }
                        }

                    ]
                });
                }



            ///////////////////////




            function pauseRestartCurrentProdPhase() {

                if (ajaxLoading == false) {
                    ajaxLoading = true;
                    params = ($(this).prev()[0].defaultValue.toString().split('|')[0]) + '|' + ($(this).prev()[0].defaultValue.toString().split('|')[1]);
                    $.ajax({
                        type: "POST",
                        url: document.location.href + "/PauseRestartCurrentProductionPhase",
                        data: JSON.stringify({ userId: '<% =WebUser.Employee.ID%>', pipedParams: params }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            ajaxLoading = false;
                            window.location.reload();
                            //alert(msg.d)
                        },
                        error: function (msg) {
                            ajaxLoading = false;
                            alert(msg.d)
                            //window.location.reload();
                        }
                    });
                }
                return false;
            };


            function SetAutoCompleteInc() {

                $("#ctl00_ContentPlaceHolder1_senMain_txtTextField1").autocomplete({
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
                        $("#ctl00_ContentPlaceHolder1_senMain_txtTextField1").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_senMain_hidTextField1").val(ui.item.value);
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

            function SetAutoCompleteEsc() {

                $("#ctl00_ContentPlaceHolder1_senMain_txtTextField2").autocomplete({
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
                        $("#ctl00_ContentPlaceHolder1_senMain_txtTextField2").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_senMain_hidTextField2").val(ui.item.value);
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
            function goToOperator(pidPo, pidQd) {

                if (confirm('Confermi il completamento della lavorazione corrente?')) {

                    if (pidPo) {
                        idProductionOrder = pidPo;
                        idQuotationDetail = pidQd;
                    }
                    else {
                        idProductionOrder = $(this).prev().prev()[0].defaultValue;
                        idQuotationDetail = $(this).prev()[0].defaultValue;
                    }
                    labextimOperatorUrl = '<%= System.Configuration.ConfigurationManager.AppSettings["LabextimOperatorUrl"] %>';
                    curUser = '<%=GetCurrentEmployee().ID.ToString()%>';
                    $(this).removeAttr('href');
                    $(this).unbind('click');
                    $(this).closest('td').prev().find('a').removeAttr('href');
                    $(this).closest('td').prev().find('a').removeAttr('onclick');
                    //$(this).closest('td').prev().find('a').unbind('click');
                    OpenBigItem2(labextimOperatorUrl + '/tempproductionorderdetail/getodp?a=' + curUser + '&idodp=' + idProductionOrder + '&qd=' + idQuotationDetail);
                }
                return false;
            };

            $('.goToOperator').bind('click', goToOperator);
            SetAutoCompleteInc();
            SetAutoCompleteEsc();

            SetStartDialog();
            SetStartDialogButtons();
            SetEndDialog();
            SetEndDialogButtons();
            SetForceEndDialog();
            SetForceEndDialogButtons();
            $('.pause').bind('click', pauseRestartCurrentProdPhase);
            setInterval(clickSearch, 60000);

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        });
    </script>

    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Calendario lavorazioni di reparto
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
            </h2>
            <table width="100%">
                <tr>
                    <td>

                        <cfb:SearchEngine ID="senMain" runat="server" UseBigButtons="true" CustomCssClass="searchEngineExtraLarge"></cfb:SearchEngine>
                        <asp:HiddenField ID="hidSenMainTextField1Text" runat="server" />
                        <asp:HiddenField ID="hidSenMainTextField2Text" runat="server" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnMPSRecalc" runat="server" Text="Ricalcola MPS"
                                        OnClientClick="javascript:return confirm('Confermi il ricalcolo del Master Production Schedule di tutti i reparti?');" CssClass="myButton" OnClick="btnMPSRecalc_Click" />
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rdlGreenOnly" runat="server" RepeatDirection="Horizontal" Font-Size="10" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="rdlGreenOnly_SelectedIndexChanged">
                                        <asp:ListItem Text="ODP PRESENTI IN REPARTO ED IN ARRIVO" Value="T" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="SOLO ODP PRESENTI IN REPARTO" Value="P"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataList ID="dlButtons" runat="server" OnItemCommand="dlButtons_ItemCommand" RepeatDirection="Horizontal">
                            <ItemTemplate>
                                <asp:Button ID="cmdSubmit" Height="40" Font-Bold="true" Font-Size="Large" ForeColor="Navy" Text='<%#   Eval( "Value" ) %>' runat="server" CommandArgument='<%# Eval("Key") %>' />&nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
            <%--<asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="grdProductionMPS"
                Display="None" />--%>
            <asp:Panel ID="DetailsPanel" runat="server">
                <table>
                    <%--<tr>
                        <td>
                            <asp:LinkButton ID="lbtNewItem" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Brown" Text="Nuova voce" />
                            &nbsp;
                            <asp:LinkButton ID="lbtUpdateGrid" runat="server" CssClass="gridview" Font-Bold="True"
                                ForeColor="Green" Text="Aggiorna lista" OnClick="lbtUpdateGrid_Click" />
                            &nbsp;
                            <asp:LinkButton ID="lbtPrintProductionMPS" runat="server" CssClass="gridview"
                                Font-Bold="True" OnClick="lbtPrintProductionMPS_Click" Text="Stampa tabella" />
                            <asp:Label ID="Label1" runat="server" Text="(a filtro vuoto sono visualizzati gli OdP degli ultimi 12 mesi)"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <asp:LinqDataSource ID="ldsProductionMPS" runat="server" ContextTypeName="DLLabExtim.QuotationDataContext"
                                TableName="VW_ProductionExtMPS_GroupedByPhases" OnSelected="ldsProductionMPS_Selected" EnableUpdate="True"
                                EnableDelete="True" EnableInsert="True" OnSelecting="ldsProductionMPS_Selecting"
                                AutoGenerateOrderByClause="true" EnableViewState="false">
                            </asp:LinqDataSource>
                            <asp:GridView ID="grdProductionMPS" runat="server" AutoGenerateColumns="False"
                                PagerSettings-Position="Top" AllowPaging="True" DataSourceID="ldsProductionMPS"
                                DataKeyNames="ID" CssClass="gridview" OnPageIndexChanging="grdProductionMPS_PageIndexChanging"
                                OnDataBound="grdProductionMPS_DataBound" OnRowDataBound="grdProductionMPS_RowDataBound"
                                OnRowDeleted="grdProductionMPS_RowDeleted" OnRowCommand="grdProductionMPS_RowCommand"
                                OnPreRender="grdProductionMPS_PreRender">
                                <HeaderStyle Font-Size="Large" />
                                <RowStyle CssClass="row" Font-Size="Large" />
                                <AlternatingRowStyle CssClass="altRowDarker" />
                                <Columns>

                                    <%--                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtSendToMachine" runat="server" ImageUrl="~/Images/RotCCRight.gif" Visible="false"
                                                ToolTip="Invia alla macchina (BOBST)" CommandName="SendToMachine" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IDProductionOrder")%>' />
                                            <asp:ImageButton ID="ibtStopBatch" runat="server" ImageUrl="~/Images/StopSign.gif" OnClick="ibtStopBatch_Click" Visible="false" ToolTip="Fine produzione (BOBST)" Width="30" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypEdit" runat="server" ImageUrl="~/Images/pencil.png" ToolTip="Gestione ordine di produzione" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%--<asp:ImageButton ID="ibtSendToMachine" runat="server" ImageUrl="~/Images/RotCCRight.gif" Visible="false"
                                                ToolTip="Invia alla macchina (BOBST)" CommandName="SendToMachine" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IDProductionOrder")%>' />
                                            <asp:ImageButton ID="ibtStopBatch" runat="server" ImageUrl="~/Images/StopSign.gif" OnClick="ibtStopBatch_Click" Visible="false" ToolTip="Fine produzione (BOBST)" Width="30" />--%>
                                            <%--<hr style="border: 1px dotted #f7f4f4;">--%>
                                            <%--<br />--%>
                                            <asp:HyperLink ID="hypEdit" runat="server" ToolTip="Gestione ordine di produzione">
                                                <asp:Image runat="server" ImageUrl="~/Images/pencil.png" style="height:40px" />
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <%--<asp:BoundField HeaderText="Cliente" DataField="cuName" ItemStyle-Font-Bold="true" />--%>


                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            ODP
                                            <hr />
                                            CLIENTE
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("IDProductionOrder")%>' Font-Size="Smaller" />&nbsp;&nbsp;<asp:Label runat="server" Text='<%# Eval("Number") %>' Font-Bold="true" />
                                            <%--<hr style="border: 1px dotted #f7f4f4;">--%>
                                            <br />
                                            <asp:Label runat="server" Text='<%# Eval("cuName") %>' Font-Bold="true" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>



                                    <asp:BoundField HeaderText="TITOLO" DataField="poDescription" ItemStyle-Width="17%" />

                                    <%-- <asp:TemplateField HeaderText="Descrizione OdP">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtqtNote" Text='<%# Bind("qtNote") %>' runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" Width="500px" ForeColor="Navy" BorderStyle="None"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <%--<asp:BoundField HeaderText="Quantità" DataField="Quantity" />--%>


                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            QUANTITA'
                                            <hr />
                                            DATA CONSEGNA
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Quantity") %>' />
                                            <%--<hr style="border: 1px dotted #f7f4f4;">--%>
                                            <br />
                                            <asp:Label runat="server" Text='<%# Eval("DeliveryDate","{0:dd/MM/yyyy}") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>


                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            ATTUALMENTE IN
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("curMachineDescription") %>' />
                                            <%--<hr style="border: 1px dotted #f7f4f4;">--%>
                                            <br />
                                            <asp:Image ID="imgSemaphore" runat="server"
                                                ImageUrl='<%# Eval("SemaphoreImage", "~/Images/{0}.gif") %>'
                                                AlternateText='<%# Bind("SemaphoreTitle") %>'
                                                ToolTip='<%# Bind("SemaphoreTitle") %>' />
                                            <asp:Image ID="imgPercExe" ToolTip="Percentuale lavorazione eseguita" runat="server" CssClass="MyIconSmall" ImageUrl="~/Images/under-construction-2.gif" />
                                            <asp:Label runat="server" ID="lblPercExe"><%# Eval("PercLavExe")%></asp:Label>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>


                                    <%--<asp:BoundField HeaderText="Attualmente in" DataField="curMachineDescription" />

                                    <asp:TemplateField HeaderText="" SortExpression="">
                                        <ItemTemplate>
                                            <asp:Image ID="imgSemaphore" runat="server"
                                                ImageUrl='<%# Eval("SemaphoreImage", "~/Images/{0}.gif") %>'
                                                AlternateText='<%# Bind("SemaphoreTitle") %>'
                                                ToolTip='<%# Bind("SemaphoreTitle") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>

                                    <%--<asp:TemplateField HeaderText="Fase attuale" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CloseCurPhaseLinkButton" runat="server" CommandName="CloseCurPhase" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IDProductionOrder") + "|" +  DataBinder.Eval(Container.DataItem, "curPhaseQuotationDetail") %>'
                                                CausesValidation="false" Text="Scarica fase attuale" OnClientClick="return confirm('Confermi il completamento della lavorazione corrente?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Fase attuale" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hidIdProductionOrder" />
                                            <asp:HiddenField runat="server" ID="hidQuotationDetail" />
                                            <asp:LinkButton ID="CloseCurPhaseAndMaterialLinkButton" runat="server" CausesValidation="false" Text="Scarica fase attuale e materiali" CssClass="goToOperator" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            SCARICA FASE
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CloseCurPhaseLinkButton" runat="server" CommandName="CloseCurPhase" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IDProductionOrder") + "|" +  DataBinder.Eval(Container.DataItem, "curPhaseQuotationDetail") %>'
                                                CausesValidation="false" Text="Scarica fase attuale" OnClientClick="return confirm('Confermi il completamento della lavorazione corrente?');" />
                                            <br />
                                            <asp:HiddenField runat="server" ID="hidIdProductionOrder" />
                                            <asp:HiddenField runat="server" ID="hidQuotationDetail" />
                                            <asp:LinkButton ID="CloseCurPhaseAndMaterialLinkButton" runat="server" CausesValidation="false" Text="Scarica fase attuale e materiali" CssClass="goToOperator" />

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            APRI/CHIUDI FASE
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 33%; text-align: center; border: none">
                                                        <input type="hidden" id="hidPoId" value='<%# Eval("IDProductionOrder").ToString()  + "|" + Eval("IDQuotationDetail").ToString()%>' />
                                                        <asp:Image ID="ibtStart" ToolTip="Avvia lavorazione" runat="server" CssClass="start MyIcon" ImageUrl="Images/project-launch-1487652-1260917.png" /></td>
                                                    <td style="width: 33%; text-align: center; border: none">
                                                        <input type="hidden" id="hidPoIdP" value='<%# Eval("IDProductionOrder").ToString() + "|" + Eval("IDQuotationDetail").ToString()%>' />
                                                        <asp:Image ID="ibtPause" ToolTip="Sospendi lavorazione" runat="server" CssClass="pause MyIcon" /></td>
                                                    <td style="width: 33%; text-align: center; border: none">
                                                        <input type="hidden" id="hidPoIdC" value='<%# Eval("IDProductionOrder").ToString() + "|" + Eval("IDQuotationDetail").ToString()%>' />
                                                        <asp:Image ID="ibtEnd" ToolTip="Chiudi lavorazione" runat="server" CssClass="end MyIcon" ImageUrl="~/Images/c40271fd53a764efd9977469270398af.png" />
                                                        <input type="hidden" id="hidPoIdCF" value='<%# Eval("IDProductionOrder").ToString() + "|" + Eval("IDQuotationDetail").ToString()%>' />
                                                        <asp:Image ID="ibtForceEnd" ToolTip="Forza chiusura lavorazione" runat="server" CssClass="forceEnd MyIcon" ImageUrl="~/Images/c40271fd53a764efd9977469270398af.png" /></td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <%--<asp:BoundField HeaderText="Data consegna" DataField="DeliveryDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />--%>
                                </Columns>
                                <PagerStyle CssClass="footer" />
                                <PagerTemplate>
                                    <asp:GridViewPager ID="Pager1" runat="server" />
                                </PagerTemplate>
                                <EmptyDataTemplate>
                                    Nessuna voce trovata.
                                </EmptyDataTemplate>
                            </asp:GridView>

                            <input type="hidden" id="hidSelPoId" />
                            <input type="hidden" id="hidSelQdId" />
                            <div id="mpnlStart" class="startDialog MyPopup">
                                <table cellpadding="5" style="width: 100%">
                                    <tr>
                                        <td colspan="2"><span id="spnPoDesc"></span></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Scegli la fase di lavorazione da iniziare</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <select id="ddlPhase" class="droplist"></select></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblPhaseTime"></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="mpnlEnd" class="endDialog MyPopup">
                                <table cellpadding="5" style="width: 100%">
                                    <tr>
                                        <td colspan="2"><span id="spnPoDescC"></span></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Chiusura fase di lavorazione</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblPhaseTimeC"></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="mpnlForceEnd" class="forceEndDialog MyPopup">
                                <table cellpadding="5" style="width: 100%">
                                    <tr>
                                        <td colspan="2"><span id="spnPoDescF"></span></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Scegli la fase di lavorazione di cui forzare la chiusura</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <select id="ddlPhaseF" class="droplist"></select></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblPhaseTimeF"></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <%--<asp:Timer ID="tmrScheduling" runat="server" Interval="15000" OnTick="tmrScheduling_Tick">
            </asp:Timer>--%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="senMain" />
            <asp:PostBackTrigger ControlID="dlButtons" />
            <asp:PostBackTrigger ControlID="rdlGreenOnly" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
