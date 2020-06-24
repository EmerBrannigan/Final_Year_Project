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
    public class ClientController : BaseController
    {
        private ITullymurryDataService service;

        public ClientController()
        {
            this.service = new TullymurryDataService();
        }

        [HttpGet]
        public IActionResult Index(string search = null)
        {
            var clients = service.SelectAllClients(search);
            return View(clients);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var existing = service.SelectClientById(id);

            if (existing == null)
            {
                Alert("Client does not exist", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            return View(existing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var clients = service.SelectAllClients();
            ViewBag.Clients = new SelectList(clients, "Id", "FulName");

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreate(Client obj)
        {
            if (ModelState.IsValid)
            {
                service.InsertClient(obj);
                Alert("New Client Added", AlertType.success);

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Client existing = service.SelectClientById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            service.DeleteClient(id);
            Alert("Client Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Clients = service.SelectAllClients();
            ViewBag.Stocks = new SelectList(Clients, "Id", "FulName");

            // use better naming for service methods
            var model = service.SelectClientById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult ConfirmEdit(Client obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateClient(obj))
                {
                    Alert("Changes to Client Saved", AlertType.success);
                    return RedirectToAction(nameof(Details), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Client", AlertType.danger);
            }

            // re-display the edit page
            return View(nameof(Edit), obj);

        }

        /*        [ProducesResponseType(typeof(Attendance))]
                public async Task<IHttpActionResult> GetClientAttendanceRecord(int id)
                {
                    var attendance = await db.Atendance.Include(a => a.Client).Select(a =>
                        new Attendance()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Year = a.Year,
                            Price = a.Price,
                            AuthorName = a.Author.Name,
                            Genre = a.Genre
                        }).SingleOrDefaultAsync(a => a.Id == id);
                    if (attendance == null)
                    {
                        return NotFound();
                    }

                    return Ok(attendance);
                }
            }*/
    }
}