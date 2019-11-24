using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebClinic.Data;
using WebClinic.Models;
using WebClinic.Models.Users;

namespace WebClinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager userManager;
        private readonly IStringLocalizer<AccountController> localizer;

        public AccountController(IUserManager userManager, IStringLocalizer<AccountController> localizer)
        {
            this.userManager = userManager;
            this.localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Sex = model.Sex.GetValueOrDefault(),
                    DateOfBirth = model.DateOfBirth.GetValueOrDefault(),
                    Role = Role.Patient
                };
                var result = userManager.CreatePatient(patient, model.Password);
                if (result != null)
                {
                    userManager.SignIn(HttpContext, result.Email, model.Password);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                } else
                {
                    ModelState.AddModelError("EmailExist", localizer["EmailExists"]);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (userManager.SignIn(HttpContext, model.Email, model.Password)) {
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                } else
                {
                    ModelState.AddModelError("WrongCredentials", localizer["WrongCredentials"]);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            userManager.SignOut(HttpContext);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}