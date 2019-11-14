using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClinic.Data;
using WebClinic.Models;
using WebClinic.Models.Users;

namespace WebClinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager userManager;

        public AccountController(IUserManager userManager)
        {
            this.userManager = userManager;
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
                    LastName =  model.LastName,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth
                };
                var result = userManager.CreatePatient(patient, model.Password);
                if (result != null)
                {
                    userManager.SignIn(HttpContext, result.Email, model.Password);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
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
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                userManager.SignIn(HttpContext, model.Email, model.Password);
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