using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClinic.Models;


namespace WebClinic.Controllers
{
    [Route("[controller]/[action]")]
    //[Authorize(Roles = "Receptionist,Physician")]
    public class PhysiciansController : Controller
    {
        //private ApplicationDbContext _context;

        public PhysiciansController()//ApplicationDbContext _context)
        {
          //  this._context = _context;
        }

        [HttpGet]
        public IActionResult AllPhysicians()
        {
            //var model = new PhysiciansViewModel();
            //model.Physicians = _context.Physicians.ToList();
            //return View(model);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            //var physician = _context.Physicians.Find(id);

            //if (physician == null)
            //{
            //    return NotFound();
            //}

            //var model = PhysicianViewModel.GetModel(physician);
            //return View(model);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            //var physician = _context.Physicians.Find(id);

            //if (physician == null)
            //{
            //    return NotFound();
            //}

            //var model = PhysicianViewModel.GetModel(physician);
            //return View(model);
            return NotFound();
        }

        [HttpPost("{id}")]
        public IActionResult Edit(int id, [FromForm] PhysicianViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var physician = _context.Physicians.Find(id);
                //if (id != model.Id)
                //{
                //    return BadRequest();
                //}
                //model.UpdatePhysician(ref physician);

                //_context.Entry(physician).State = EntityState.Modified;

                //try
                //{
                //    _context.SaveChanges();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!PhysicianExists(id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}

                return RedirectToAction(nameof(PhysiciansController.Details), new { id });
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Delete(int id)
        {
            //var Physician = _context.Physicians.Find(id);
            //if (Physician == null)
            //{
            //    return NotFound();
            //}

            //_context.Physicians.Remove(Physician);
            //_context.SaveChanges();

            return RedirectToAction(nameof(PhysiciansController.AllPhysicians));
        }

        private bool PhysicianExists(int id)
        {
            // return _context.Physicians.Any(e => e.Id == id);
            return false;
        }
    }
}
