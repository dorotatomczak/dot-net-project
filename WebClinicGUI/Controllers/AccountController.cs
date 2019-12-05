using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebClinicGUI.Helpers;
using WebClinicGUI.Models;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;
using WebClinicGUI.Utils;

namespace WebClinicGUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly INetworkClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<CalendarController> _logger;

        public AccountController(ILogger<CalendarController> logger, IStringLocalizer<AccountController> localizer, INetworkClient client, IHttpContextAccessor accessor)
        {
            _localizer = localizer;
            _logger = logger;
            _client = client;
            _contextAccessor = accessor;
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
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                    Role = Role.Patient,
                    Password = HashUtils.Hash(model.Password)
                };

                try
                {
                    _logger.Log(LogLevel.Debug, "User tries to register");
                    await _client.SendRequestWithBodyAsync<Patient>(HttpMethod.Post, "Account/RegisterPatient", patient);
                    _logger.Log(LogLevel.Debug, "User registered succesfully");
                    return RedirectToAction("Login");
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError("EmailExist", _localizer["EmailExists"]);
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
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var userDto = new AppUser
                {
                    Email = model.Email,
                    Password = HashUtils.Hash(model.Password),
                };

                try
                {
                    _logger.Log(LogLevel.Debug, "User tries to login");
                    await Login(userDto);
                    _logger.Log(LogLevel.Debug, "User loggged succesfully");

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError("WrongCredentials", _localizer["WrongCredentials"]);
                }
            }
            return View(model);
        }

        private async Task Login(AppUser userDto)
        {
            var user = await _client.SendRequestWithBodyAsync<AppUser>(HttpMethod.Post, "Account/Authenticate", userDto);
            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private IEnumerable<Claim> GetUserClaims(AppUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("access_token", user.Token)
            };
            return claims;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _logger.Log(LogLevel.Debug, "User logged out succesfully");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Receptionist")]
        public async Task<IActionResult> ReceptionistAccount()
        {
            _logger.Log(LogLevel.Debug, "AccountController::ReceptionistAccount()");
            if (TempData["message"] != null)
            {
                ModelState.AddModelError("CredentialChanged", _localizer[TempData["message"].ToString()]);
            }
            try
            {
                var id = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var receptionist = await _client.SendRequestAsync<Receptionist>(HttpMethod.Get, $"Account/{id}");
                var model = new ReceptionistAccountViewModel();

                model.Email = receptionist.Email;
                model.FirstName = receptionist.FirstName;
                model.LastName = receptionist.LastName;
                model.DateOfBirth = receptionist.DateOfBirth;
                model.Sex = receptionist.Sex;

                return View(model);
            }
            catch (HttpRequestException)
            {
                //show error
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> PatientAccount()
        {
            _logger.Log(LogLevel.Debug, "AccountController::PatientAccount()");
            if (TempData["message"] != null)
            {
                ModelState.AddModelError("CredentialChanged", _localizer[TempData["message"].ToString()]);
            }
            try
            {
                var id = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var patient = await _client.SendRequestAsync<Patient>(HttpMethod.Get, $"Account/{id}");
                var model = new PatientAccountViewModel();

                model.Email = patient.Email;
                model.FirstName = patient.FirstName;
                model.LastName = patient.LastName;
                model.DateOfBirth = patient.DateOfBirth;
                model.Sex = patient.Sex;
                model.RecommendedDrugs = patient.RecommendedDrugs;

                return View(model);
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Physician")]
        public async Task<IActionResult> PhysicianAccount()
        {
            _logger.Log(LogLevel.Debug, "AccountController::PhysicianAccount");
            if (TempData["message"] != null)
            {
                ModelState.AddModelError("CredentialChanged", _localizer[TempData["message"].ToString()]);
            }
            try
            {
                var id = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var physician = await _client.SendRequestAsync<Physician>(HttpMethod.Get, $"Account/{id}");
                var model = new PhysicianAccountViewModel();

                model.Email = physician.Email;
                model.FirstName = physician.FirstName;
                model.LastName = physician.LastName;
                model.DateOfBirth = physician.DateOfBirth;
                model.Sex = physician.Sex;
                model.Specialization = physician.Specialization;

                return View(model);
            }
            catch (HttpRequestException)
            {
                throw new ServerConnectionException();
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            _logger.Log(LogLevel.Debug, "AccountController::ChangePassword");
            if (ModelState.IsValid)
            {
                try
                {
                    var credentials = new ChangePasswordModel(
                        _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email),
                        HashUtils.Hash(model.OldPassword),
                        HashUtils.Hash(model.NewPassword));

                    await _client.SendRequestWithBodyAsync(HttpMethod.Put, "Account/UpdatePassword", credentials);

                    TempData["message"] = "Your password has been changed.";

                    if (_contextAccessor.HttpContext.User.IsInRole("Physician"))
                        return RedirectToAction("PhysicianAccount");
                    if (_contextAccessor.HttpContext.User.IsInRole("Receptionist"))
                        return RedirectToAction("ReceptionistAccount");

                    return RedirectToAction("PatientAccount");
                }
                catch (HttpRequestException)
                {
                    ModelState.AddModelError("WrongCredential", _localizer["Old password is incorrect."]);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            _logger.Log(LogLevel.Debug, "AccountController::ChangeEmail");
            if (ModelState.IsValid)
            {
                try
                {
                    var credentials = new ChangeEmailModel(
                        model.Email,
                        model.NewEmail,
                        HashUtils.Hash(model.Password));

                    await _client.SendRequestWithBodyAsync(HttpMethod.Put, "Account/UpdateEmail", credentials);

                    await HttpContext.SignOutAsync();

                    var userDto = new AppUser
                    {
                        Email = model.NewEmail,
                        Password = HashUtils.Hash(model.Password),
                    };
                    await Login(userDto);

                    TempData["message"] = "Your email has been changed.";

                    if (_contextAccessor.HttpContext.User.IsInRole("Physician"))
                        return RedirectToAction("PhysicianAccount");
                    if (_contextAccessor.HttpContext.User.IsInRole("Receptionist"))
                        return RedirectToAction("ReceptionistAccount");

                    return RedirectToAction("PatientAccount");
                }
                catch (HttpRequestException ex)
                {
                    if (ex.Message.Contains("Email \"" + model.NewEmail + "\" is already taken"))
                        ModelState.AddModelError("WrongCredential", _localizer["EmailExists"]);
                    else if (ex.Message.Contains("Password is incorrect"))
                        ModelState.AddModelError("WrongCredential", _localizer["WrongPassword"]);
                    else ModelState.AddModelError("WrongCredential", _localizer["WrongCredentials"]);
                }
            }
            return View(model);
        }
    }
}