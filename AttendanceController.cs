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
    public class AttendanceController : BaseController
    {

        private ITullymurryDataService service;

        public AttendanceController()
        {
            this.service = new TullymurryDataService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var Attendances = service.SelectAllAttendance();
            return View(Attendances);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var existing = service.SelectAttendanceById(id);

            if (existing == null)
            {
                Alert("Client Attendance Record does not exist", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            return View(existing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var clients = service.SelectAllClients();
            ViewBag.clients = new SelectList(clients, "Id", "FullName");

            var horses = service.SelectAllHorses();
            ViewBag.Horses = new SelectList(horses, "Id", "HorseName");

            var lessons = service.SelectAllLessons();
            ViewBag.Lessons = new SelectList(lessons, "Id", "DateAndTime");

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreate(Attendance obj)
        {
            if (ModelState.IsValid)
            {
                // view is valid so create attendance
                service.InsertAttendance(obj);
                Alert("New Client Attendance Saved", AlertType.success);
            }
            return RedirectToAction("Details", "Lesson");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Attendance existing = service.SelectAttendanceById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            service.DeleteAttendance(id);
            Alert("Attendance Record Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var clients = service.SelectAllClients();
            ViewBag.clients = new SelectList(clients, "Id", "FullName");

            var horses = service.SelectAllHorses();
            ViewBag.Horses = new SelectList(horses, "Id", "HorseName");

            var lessons = service.SelectAllLessons();
            ViewBag.Lessons = new SelectList(lessons, "Id", "DateAndTime");

            // use better naming for service methods
            var model = service.SelectAttendanceById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult ConfirmEdit(Attendance obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateAttendance(obj))
                {
                    Alert("Changes to Client's Attendance Saved", AlertType.success);
                    return RedirectToAction(nameof(Details), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Attendance", AlertType.danger);
            }

            // re-display the edit page
            return View("Index", "LessonController");

        }
    }
}