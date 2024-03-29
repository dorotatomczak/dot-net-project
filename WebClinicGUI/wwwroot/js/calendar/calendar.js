﻿let dp = null;

function _configureEventsAction() {
    dp.eventMoveHandling = "Disabled";
    dp.eventResizeHandling = "Disabled";
    dp.onEventClicked = function (args) {
        appointment = args.e.data.appointment;
        $('#time').text(appointment.time.replace("T", " "));
        $('#patient').text(appointment.patient.fullName);
        $('#physician').text(appointment.physician.fullName);

        var appointmentTypes = {
            CONSULTATION: 0,
            CURE: 1,
            CONTROL_CHECK: 2,
        };

        if (appointment.type == appointmentTypes.CONSULTATION) {
            $('#consultation').removeAttr("hidden");
        }
        else if (appointment.type == appointmentTypes.CURE) {
            $('#cure').removeAttr("hidden");
        }
        else if (appointment.type == appointmentTypes.CONTROL_CHECK) {
            $('#control-check').removeAttr("hidden");
        }
        else {
            throw new Error("Appointment Type does not have valid value.");
        }

        $('#modal-event-show').modal('show');
        $("#cancel-btn").click(function (e) {
            // Stop the normal navigation
            e.preventDefault();

            //Build the new URL
            var url = $(this).attr("href");
            url = url.replace("dummyId", appointment.id);

            //Navigate to the new URL
            window.location.href = url;

        });
    };
}

function createCalendarWeek() {
    dp = new DayPilot.Calendar("dp");
    dp.viewType = "Week";
    _configureEventsAction();
    dp.init();
}

function createCalendarDay() {
    dp = new DayPilot.Calendar("dp");
    dp.viewType = "Day";
    _configureEventsAction();
    dp.init();
}

function createCalendarMonth() {
    dp = new DayPilot.Month("dp");
    _configureEventsAction();
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