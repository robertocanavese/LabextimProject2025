<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProductionGraphicScheduler.aspx.cs" Inherits="LabExtim.ProductionGraphicScheduler"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/CustomControls/SearchEngine.ascx" TagName="SearchEngine" TagPrefix="cfb" %>

<asp:Content ID="Header1" ContentPlaceHolderID="Header1" runat="server">


    <title>Schedulatore grafico lavorazioni</title>

    <!-- head -->
    <!-- demo stylesheet -->
    <link type="text/css" rel="stylesheet" href="Script/daypilot/helpers/demo.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/helpers/media/layout.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/helpers/media/elements.css?v=2911" />

    <!-- helper libraries -->
    <script src="Script/daypilot/helpers/jquery-1.12.2.min.js" type="text/javascript"></script>

    <!-- daypilot libraries -->
    <script src="Script/daypilot/js/daypilot-all.min.js?v=2911" type="text/javascript"></script>

    <!-- daypilot themes -->
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/areas.css?v=2911" />

    <%--<link type="text/css" rel="stylesheet" href="Script/daypilot/themes/month_white.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/month_green.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/month_transparent.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/month_traditional.css?v=2911" />--%>

    <%--<link type="text/css" rel="stylesheet" href="Script/daypilot/themes/navigator_8.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/navigator_white.css?v=2911" />--%>

    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/calendar_transparent.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/calendar_white.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/calendar_green.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/calendar_traditional.css?v=2911" />

    <%--<link type="text/css" rel="stylesheet" href="Script/daypilot/themes/scheduler_8.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/scheduler_white.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/scheduler_green.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/scheduler_blue.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/scheduler_traditional.css?v=2911" />
    <link type="text/css" rel="stylesheet" href="Script/daypilot/themes/scheduler_transparent.css?v=2911" />--%>

    <script src="Script/jquery-ui.min.js" type="text/javascript"></script>
    <link href="Script/jquery-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="Script/jquery-ui.structure.min.css" rel="stylesheet" type="text/css" />

    <!-- /head -->

    <link href="AutocompleteStyle.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .calendar_default_event_inner {
            font-size: 10px !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <h2>Schedulatore grafico lavorazioni
                <asp:Label ID="lblModeDescription" runat="server"></asp:Label>
    </h2>
    <table width="100%">
        <tr>
            <td class="droplist">Reparto:
                <select id="ddlDepartments" class="droplist"></select>
                Macchina:
                <select id="ddlMachines" class="droplist"></select>
                Cliente:
                <input type="text" id="txtSearchCli" class="droplist" style="width: 300px" />
                <input type="hidden" id="hidSearchCli" />
                Odp:
                <input type="text" id="txtSearchPo" class="droplist" style="width: 300px" />
                <input type="hidden" id="hidSearchPo" />
                <button id="btnReset" class="droplist">Reset</button>
                <button id="btnSearch" class="droplist">Search</button>
            </td>
        </tr>
        <tr>
            <td>
                <div id="dp"></div>
            </td>
        </tr>
    </table>



    <script type="text/javascript">
        var dp = new DayPilot.Calendar("dp");


        dp.startDate = new DayPilot.Date.today().addDays(-2);
        dp.viewType = "Days";
        dp.days = 28,
        dp.columnWidthSpec = "Fixed",
        dp.columnWidth = 100,
        dp.locale = "it-it";
        dp.cellDuration = "15";
        dp.businessBeginsHour = 8;
        dp.businessEndsHour = 17;
        dp.showNonBusiness = true;

        //dp.cssClassPrefix = "calendar_white";

        // bubble, sync loading
        // see also DayPilot.Event.data.staticBubbleHTML property
        dp.bubble = new DayPilot.Bubble();

        dp.contextMenu = new DayPilot.Menu({
            items: [
                    {
                        text: "Annulla priorità impostata", onclick: function () {
                            debugger;
                            var event = this.source.data;
                            params = event.id.toString();
                            $.ajax({
                                type: 'POST',
                                url: document.location.href + '/CancelPriority',
                                data: JSON.stringify({ pipedParams: params }),
                                dataType: "json",
                                async: false,
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    dp.message("Priorità annullata: " + event.text());
                                },
                                error: function (data) {
                                    alert("Errore: " + event.text());
                                }

                            });
                        }
                    },
                    //{ text: "Show event text", onclick: function () { alert("Event text: " + this.source.text()); } },
                    //{ text: "Show event start", onclick: function () { alert("Event start: " + this.source.start().toStringSortable()); } },
                    //{ text: "Go to google.com", href: "http://www.google.com/?q={0}" },
                    //{ text: "CallBack: Delete this event", command: "delete" },
                    //    {
                    //        text: "submenu", items: [
                    //               { text: "Show event ID", onclick: function () { alert("Event value: " + this.source.value()); } },
                    //               { text: "Show event text", onclick: function () { alert("Event text: " + this.source.text()); } }
                    //        ]
                    //    }
            ]
        });

        var departments = null;
        $.ajax({
            type: 'POST',
            url: document.location.href + '/GetDepartments',
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                departments = data.d;
            },
            error: function (data) {
                departments = data.d;
            }

        });

        var machines = null;
        $.ajax({
            type: 'POST',
            url: document.location.href + '/GetProductionMachines',
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                machines = data.d;
            },
            error: function (data) {
                machines = data.d;
            }

        });
        dp.resources = machines;


        function LoadData() {

            var events = null;
            $.ajax({
                type: 'POST',
                url: document.location.href + '/GetEvents',
                data: JSON.stringify({ idDepartment: $('#ddlDepartments option:selected').val(), idMachine: $('#ddlMachines option:selected').val(), customerCode: $('#hidSearchCli').val(), productionOrderId: $('#hidSearchPo').val() }),
                dataType: "json",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    events = data.d;
                },
                error: function (data) {
                    events = data.d;
                }

            });

            dp.events.list = [];
            dp.update();

            var data = new Array();

            // generate and load events
            for (var i = 0; i < events.length; i++) {
                curEvent = events[i];
                //var duration = Math.floor(Math.random() * 6) + 1; // 1 to 6
                //var start = Math.floor(Math.random() * 6) - 3; // -3 to 3

                var e = new DayPilot.Event({
                    start: new DayPilot.Date(curEvent.start),
                    end: new DayPilot.Date(curEvent.end),
                    id: curEvent.id, //DayPilot.guid(),
                    resource: curEvent.resource,
                    text: curEvent.text,
                    tooltip: curEvent.tooltip,
                    backColor: (curEvent.status == 15 ? 'yellow' : ''),
                    barColor: curEvent.barColor
                    /*,
                bubbleHtml: "Testing bubble"*/
                });
                //dp.events.add(e);
                data.push(e.data);
            }
            dp.events.list = data;
            dp.update();
        }

        dp.eventHoverHandling = "Bubble";

        dp.onBeforeEventRender = function (args) {
            args.e.bubbleHtml = "Da:" + args.e.start.value.substr(11, 8) + " a:" + args.e.end.value.substr(11, 8) + " " + args.e.tooltip;
        };

        // event moving
        dp.onEventMove = function (args) {

            params = args.e.data.id.toString() + "|" + args.e.data.resource + "|" + args.e.cache.start.value.toString() + "|" + args.e.cache.end.value.toString() + "|" + args.newStart.value.toString() + "|" + args.newEnd.value.toString();
            $.ajax({
                type: 'POST',
                url: document.location.href + '/MovePhase',
                data: JSON.stringify({ pipedParams: params }),
                dataType: "json",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    dp.message("Priorità impostata: " + args.e.text());
                },
                error: function (data) {
                    args.preventDefault();
                }

            });
        };

        dp.onEventMoved = function (args) {
            dp.update();
        }

        // event resizing
        dp.onEventResized = function (args) {
            dp.message("Resized: " + args.e.text());
        };

        // event creating
        dp.onTimeRangeSelected = function (args) {
            var name = prompt("New event name:", "Event");
            dp.clearSelection();
            if (!name) return;
            var e = new DayPilot.Event({
                start: args.start,
                end: args.end,
                id: DayPilot.guid(),
                resource: args.resource,
                text: name
            });
            dp.events.add(e);
            dp.message("Created");
        };

        dp.onEventClicked = function (args) {
            //alert("clicked: " + args.e.id());
        };

        dp.onTimeHeaderClick = function (args) {
            alert("clicked: " + args.header.start);
        };

        dp.snapToGrid = false;
        dp.useEventBoxes = "Never";

        dp.onEventMoving = function (args) {
            var offset = args.start.getMinutes() % 5;
            if (offset) {
                args.start = args.start.addMinutes(-offset);
                args.end = args.end.addMinutes(-offset);
            }

            args.left.enabled = true;
            args.left.html = args.start.toString("h:mm tt");
        };

        //dp.cellWidth = 50;

        dp.onIncludeTimeCell = function (args) {
            //if (args.cell.start.getDayOfWeek() === 0) { // hide Sundays
            //    args.cell.visible = false;
            //}
            //if (args.cell.Hour === 17) { // hide Sundays
            //    args.cell.visible = false;
            //}
        };



        dp.init();

        //dp.scrollTo("2013-03-24T16:00:00");


        function SetAutoCompleteCli() {

            $("#txtSearchCli").autocomplete({
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
                    $("#txtSearchCli").val(ui.item.label);
                    $("#hidSearchCli").val(ui.item.value);
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

        function SetAutoCompletePo() {
            $("#txtSearchPo").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: document.location.href.split("?")[0].split("#")[0] + '/GetProductionOrders',
                        data: "{ 'q': '" + request.term + "', 'customerCode': '" + $("#hidSearchCli").val() + "'}",
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
                                    value: item.Code
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
                    $("#txtSearchPo").val(ui.item.label);
                    $("#hidSearchPo").val(ui.item.value);
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

        $("#btnSearch").click(function (e) {
            e.preventDefault();
            LoadData();

        });

        $("#btnReset").click(function (e) {
            e.preventDefault();
            $("#txtSearchCli").val("");
            $("#hidSearchCli").val("");
            $("#txtSearchPo").val("");
            $("#hidSearchPo").val("");

        });


    </script>

    <!-- bottom -->

    <script type="text/javascript">
        $(document).ready(function () {

            var options0 = $("#ddlDepartments");
            options0.append($("<option />").val("").text("Tutti"));
            $.each(departments, function () {
                options0.append($("<option />").val(this.id).text(this.name));
            });

            var options1 = $("#ddlMachines");
            options1.append($("<option />").val("").text("Tutte"));
            $.each(machines, function () {
                options1.append($("<option />").val(this.id).text(this.name));
            });

            SetAutoCompleteCli();
            SetAutoCompletePo();

            var url = window.location.href;
            var filename = url.substring(url.lastIndexOf('/') + 1);
            if (filename === "") filename = "index.html";
            $(".menu a[href='" + filename + "']").addClass("selected");



        });

    </script>
</asp:Content>
