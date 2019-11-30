using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace WebClinicGUI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CalendarController : Controller
    {
        private readonly IStringLocalizer<CalendarController> _localizer;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ILogger<CalendarController> logger, IStringLocalizer<CalendarController> localizer)
        {
            _localizer = localizer;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
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
            else
            {
                return RedirectToAction("Month");
            }
        }

        [HttpGet]
        public IActionResult Day()
        {
            SetViewType("Day");
            return View();
        }

        [HttpGet]
        public IActionResult Week()
        {
            SetViewType("Week");
            return View();
        }

        [HttpGet]
        public IActionResult Month()
        {
            SetViewType("Month");
            return View();
        }

        [HttpGet]
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