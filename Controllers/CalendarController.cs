using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace WebClinic.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IStringLocalizer<CalendarController> _localizer;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ILogger<CalendarController> logger, IStringLocalizer<CalendarController> localizer)
        {
            _localizer = localizer;
            _logger = logger;
        }

        public IActionResult Index()
        {
            string cookieViewType = Request.Cookies["ViewType"];

            if (cookieViewType == "Day")
            {
                return RedirectToAction("Day");
            }
            else if (cookieViewType == "Week")
            {
                return RedirectToAction("Week");
            }
            else if (cookieViewType == "Month")
            {
                return RedirectToAction("Month");
            }

            return View();
        }

        public IActionResult Day()
        {
            SetViewType("Day");
            return View();
        }

        public IActionResult Week()
        {
            SetViewType("Week");
            return View();
        }

        public IActionResult Month()
        {
            SetViewType("Month");
            return View();
        }

        public void SetViewType(string viewType)
        {
            Response.Cookies.Append("ViewType", viewType,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                }
            );
        }
    }
}