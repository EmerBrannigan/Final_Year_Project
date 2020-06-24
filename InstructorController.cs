using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TullymurrySystem.Data.Models;
using TullymurrySystem.Data.Services;

namespace TullymurrySystem.Web.Controllers
{
    public class InstructorController : BaseController
    {
        private ITullymurryDataService service;

        public InstructorController()
        {
            this.service = new TullymurryDataService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var instructors = service.SelectAllInstructors();
            return View(instructors);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var existing = service.SelectInstructorById(id);

            if (existing == null)
            {
                Alert("Instructor does not exist", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            return View(existing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var instructors = service.SelectAllInstructors();
            ViewBag.Instructors = new SelectList(instructors, "Id", "FulName");

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreate(Instructor obj)
        {
            if (ModelState.IsValid)
            {
                service.InsertInstructor(obj);
                Alert("New instructor Added", AlertType.success);

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Instructor existing = service.SelectInstructorById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            service.Delete(id);
            Alert("Instructor Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Instructors = service.SelectAllInstructors();
            ViewBag.Stocks = new SelectList(Instructors, "Id", "FulName");

            // use better naming for service methods
            var model = service.SelectInstructorById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult ConfirmEdit(Instructor obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateInstructor(obj))
                {
                    Alert("Changes to Instructor Saved", AlertType.success);
                    return RedirectToAction(nameof(Details), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Instructor", AlertType.danger);
            }

            // re-display the edit page
            return View(nameof(Edit), obj);
        }
    }
}