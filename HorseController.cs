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
    public class HorseController : BaseController
    {
        private ITullymurryDataService service;

        public HorseController()
        {
            this.service = new TullymurryDataService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var horses = service.SelectAllHorses();
            return View(horses);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var existing = service.SelectHorseById(id);

            if (existing == null)
            {
                Alert("Horse does not exist", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            return View(existing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var horses = service.SelectAllHorses();
            ViewBag.Horses = new SelectList(horses, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreate(Horse obj)
        {
            if (ModelState.IsValid)
            {
                service.InsertHorse(obj);
                Alert("New Horse Added", AlertType.success);

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Horse existing = service.SelectHorseById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            service.DeleteHorse(id);
            Alert("Horse Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Horses = service.SelectAllHorses();
            ViewBag.Stocks = new SelectList(Horses, "Id", "FulName");

            // use better naming for service methods
            var model = service.SelectHorseById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult ConfirmEdit(Horse obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateHorse(obj))
                {
                    Alert("Changes to Horse Saved", AlertType.success);
                    return RedirectToAction(nameof(Details), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Horse", AlertType.danger);
            }

            // re-display the edit page
            return View(nameof(Edit), obj);
        }
    }
}