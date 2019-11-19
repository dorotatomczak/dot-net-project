const eventsUri = '/api/appointments/events'
let dp = null;
let url = "";

function createUrl() {
    url = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '') + eventsUri;
}

function createCalendarWeek() {
    dp = new DayPilot.Calendar("dp");
    dp.viewType = "Week";
    dp.init();
}

function createCalendarDay() {
    dp = new DayPilot.Calendar("dp");
    dp.viewType = "Day";
    dp.init();
}

function createCalendarMonth() {
    dp = new DayPilot.Month("dp");
    dp.init();
}

function loadEvents() {
    if (url == "") {
        console.error("Url not valid.");
    }

    if (dp instanceof DayPilot.Calendar) {
        dp.events.load(url);
    }
    else if (dp instanceof DayPilot.Month) {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        firstDay.setUTCHours(0, 0, 0, 0);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        lastDay.setUTCHours(23, 59, 59, 999);
        var urlWithTimeRange = url + "?start=" + firstDay.toISOString() + "&end=" + lastDay.toISOString();
        $.get(urlWithTimeRange,
            function (data) {
                for (var i = 0; i < data.length; i++) {
                    var e = new DayPilot.Event(data[i]);
                    dp.events.add(e);
                }
            }
        );
    }
    else {
        console.error("Calendar object does not have proper type.");
    }
}

function previous() {
    _modifyStartDate(-1);
    dp.update();
}

function next() {
    _modifyStartDate(1);
    dp.update();
}

function _modifyStartDate(forward) {
    var sign = -1;
    if (forward == 1) {
        sign = 1;
    }
    if (dp instanceof DayPilot.Calendar && dp.viewType == "Day") {
        dp.startDate = dp.startDate.addDays(sign);
    }
    else if (dp instanceof DayPilot.Calendar && dp.viewType == "Week") {
        dp.startDate = dp.startDate.addDays(sign * 7);
    }
    else if (dp instanceof DayPilot.Month) {
        dp.startDate = dp.startDate.addMonths(sign);
    }
    else {
        console.error("Calendar object does not have proper type.");
    }
}