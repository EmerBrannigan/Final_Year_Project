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
    public class StockController : BaseController
    {
        private ITullymurryDataService service;

        public StockController()
        {
            this.service = new TullymurryDataService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var stocks = service.SelectAllStock();
            return View(stocks);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var existing = service.SelectStockById(id);

            if (existing == null)
            {
                Alert("Stock does not exist", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            return View(existing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var suppliers = service.SelectAllSupplier();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "SupplierName");

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreate(Stock obj)
        {
            if (ModelState.IsValid)
            {
                // view is valid so create attendance
                service.InsertStock(obj);
                Alert("New Product Added", AlertType.success);

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Stock existing = service.SelectStockById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            service.DeleteStock(id);
            Alert("Product Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var suppliers = service.SelectAllSupplier();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "SupplierName");

            Stock existing = service.SelectStockById(id);

            return View(existing);
        }

        [HttpPost]
        public ActionResult ConfirmEdit(Stock obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateStock(obj))
                {
                    Alert("Changes to Product Saved", AlertType.success);
                    return RedirectToAction(nameof(Details), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Stock", AlertType.danger);
            }

            // re-display the edit page
            return View(nameof(Edit), obj);
        }
    }
}