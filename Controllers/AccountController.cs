using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClinic.Data;
using WebClinic.Models;
using WebClinic.Models.Users;

namespace WebClinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountManagement accountManagement;

        public AccountController(IAccountManagement accountManagement)
        {
            this.accountManagement = accountManagement;
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
                    DateOfBirth = model.DateOfBirth,
                    Password = model.Password
                };
                var result = accountManagement.CreatePatient(patient, model.Password);
                if (result != null)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            return View(model);
        }
    }
}