let dp = null;

function _disableEventsActions() {
    dp.eventMoveHandling = "Disabled";
    dp.eventResizeHandling = "Disabled";
}

function createCalendarWeek() {
    dp = new DayPilot.Calendar("dp");
    dp.viewType = "Week";
    _disableEventsActions();
    dp.init();
}

function createCalendarDay() {
    dp = new DayPilot.Calendar("dp");
    dp.viewType = "Day";
    _disableEventsActions();
    dp.init();
}

function createCalendarMonth() {
    dp = new DayPilot.Month("dp");
    _disableEventsActions();
    dp.init();
}

function loadEvents(eventsData) {
    if (eventsData != undefined) {
        dp.events.list = [];
        for (var i = 0; i < eventsData.length; i++) {
            var e = new DayPilot.Event(eventsData[i]);
            dp.events.add(e);
        }
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
        throw new Error("Calendar object does not have proper type.");
    }
}

function setLocale(locale) {
    if ((locale.localeCompare("pl-pl", undefined, { sensitivity: 'base' }) === 0) ||
        (locale.localeCompare("en-us", undefined, { sensitivity: 'base' }) === 0) ) {
        dp.locale = locale;
    }
    else {
        console.error("Unknown locale. Setting to en-us.");
        dp.locale = "en-us";
    }
    dp.update();
}