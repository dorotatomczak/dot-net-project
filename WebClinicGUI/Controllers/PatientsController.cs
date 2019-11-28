﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data;
using WebClinic.Models;
using WebClinic.Models.Users;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebClinic.Controllers
{
    [Route("[controller]/[action]")]
    //[Authorize(Roles = "Receptionist,Physician")]
    public class PatientsController : Controller
    {
       // private ApplicationDbContext _context;

        public PatientsController()//ApplicationDbContext _context)
        {
          //  this._context = _context;
        }

        [HttpGet]
        public IActionResult AllPatients()
        {
            //var model = new PatientsViewModel();
            //model.Patients = _context.Patients.ToList();
            //return View(model);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            //var patient = _context.Patients.Find(id);
            //if (patient == null)
            //{
            //    return NotFound();
            //}
            //var model = PatientViewModel.GetModel(patient);
            //return View(model);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            //var patient = _context.Patients.Find(id);
            //if (patient == null)
            //{
            //    return NotFound();
            //}

            //var model = PatientViewModel.GetModel(patient);
            //return View(model);
            return NotFound();
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [FromForm] PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var patient = _context.Patients.Find(id);
                //if (id != model.Id)
                //{
                //    return BadRequest();
                //}
                //model.UpdatePatient(ref patient);

                //_context.Entry(patient).State = EntityState.Modified;

                //try
                //{
                //    _context.SaveChanges();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!PatientExists(id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction(nameof(PatientsController.Details), new { id });
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Delete(int id)
        {
            //var patient = _context.Patients.Find(id);
            //if (patient == null)
            //{
            //    return NotFound();
            //}

            //_context.Patients.Remove(patient);
            //_context.SaveChanges();

            return RedirectToAction(nameof(PatientsController.AllPatients));
        }

        private bool PatientExists(int id)
        {
            //return _context.Patients.Any(e => e.Id == id);
            return false;
        }
    }
}
