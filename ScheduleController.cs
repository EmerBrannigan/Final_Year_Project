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
    public class ScheduleController : BaseController
    {
        private ITullymurryDataService service;

        public ScheduleController()
        {
            this.service = new TullymurryDataService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var schedules = service.SelectAllSchedules();
            return View(schedules);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var existing = service.SelectScheduleById(id);

            if (existing == null)
            {
                Alert("Schedule does not exist", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            return View(existing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var suppliers = service.SelectAllSupplier();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "SupplierName");

            var horses = service.SelectAllHorses();
            ViewBag.Horses = new SelectList(horses, "Id", "HorseName");

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreate(Schedule obj)
        {
            if (ModelState.IsValid)
            {
                // view is valid so create attendance
                service.InsertSchedule(obj);
                Alert("New Event Scheduled", AlertType.success);

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Schedule existing = service.SelectScheduleById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            service.DeleteSchedule(id);
            Alert("Event Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var suppliers = service.SelectAllSupplier();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "SupplierName");

            var horses = service.SelectAllHorses();
            ViewBag.Horses = new SelectList(horses, "Id", "HorseName");

            // use better naming for service methods
            Schedule existing = service.SelectScheduleById(id);

            return View(existing);
        }

        [HttpPost]
        public ActionResult ConfirmEdit(Schedule obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateSchedule(obj))
                {
                    Alert("Changes to Event Saved", AlertType.success);
                    return RedirectToAction(nameof(Details), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Schedule", AlertType.danger);
            }

            // re-display the edit page
            return View(nameof(Edit), obj);
        }
    }
}