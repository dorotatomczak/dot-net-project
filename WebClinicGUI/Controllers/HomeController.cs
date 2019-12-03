using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClinicGUI.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;

namespace WebClinicGUI.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {

        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
            _logger = logger;
        }

        [HttpGet("/")]
        [HttpGet("[controller]/[action]")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ServerConnectionError()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult SetLanguage([FromForm] string culture, [FromQuery]string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
