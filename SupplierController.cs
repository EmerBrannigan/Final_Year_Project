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
    public class SupplierController : BaseController
    {
        private ITullymurryDataService service;

        public SupplierController()
        {
            this.service = new TullymurryDataService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var suppliers = service.SelectAllSupplier();
            return View(suppliers);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var existing = service.SelectSupplierById(id);

            if (existing == null)
            {
                Alert("Supplier does not exist", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            return View(existing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var suppliers = service.SelectAllSupplier();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreate(Supplier obj)
        {
            if (ModelState.IsValid)
            {
                service.InsertSupplier(obj);
                Alert("New Supplier Added", AlertType.success);

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Supplier existing = service.SelectSupplierById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            service.DeleteSupplier(id);
            Alert("Supplier Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Suppliers = service.SelectAllSupplier();
            ViewBag.Stocks = new SelectList(Suppliers, "Id", "FulName");

            // use better naming for service methods
            var model = service.SelectSupplierById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult ConfirmEdit(Supplier obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateSupplier(obj))
                {
                    Alert("Changes to Supplier Saved", AlertType.success);
                    return RedirectToAction(nameof(Details), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Supplier", AlertType.danger);
            }

            // re-display the edit page
            return View(nameof(Edit), obj);
        }
    }
}