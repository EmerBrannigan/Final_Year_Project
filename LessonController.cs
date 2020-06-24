using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TullymurrySystem.Data.Models;
using TullymurrySystem.Data.Services;
using TullymurrySystem.Web.ViewModels;

namespace TullymurrySystem.Web.Controllers
{
    public class LessonController : BaseController
    {
        private ITullymurryDataService service;
        private TullymurryMessageService messageService;


        public LessonController()
        {
            this.service = new TullymurryDataService();
            this.messageService = new TullymurryMessageService();
        }

        // GET: /<controller>/
        [HttpGet]
        
        public IActionResult Index(string search = null)
        {
            var lessons = service.SelectAllLessons(search);
            return View(lessons);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            var existing = service.SelectLessonById(id);

            if (existing == null)
            {
                Alert("Lesson does not exist", AlertType.danger);
                return RedirectToAction(nameof(Index));
            }

            return View(existing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var instructors = service.SelectAllInstructors();  
            ViewBag.Instructors = new SelectList(instructors, "Id", "FullName");
              
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreate(Lesson obj)
        {
            if (ModelState.IsValid)
            {
                // view is valid so create attendance
                service.InsertLesson( obj );
                Alert("New Lesson Saved", AlertType.success);

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult ClientLessoMessageSend(LessonResultViewModel vm)
        {
            // validate the vm


            // retrieve data needed
            var lesson = service.SelectLessonById(vm.LessonId);
            var client = lesson.ContainsClient(vm.ClientId);

            // check lesson and client exist


            // extract values from vm and send appropriate message type
            var result = messageService.SendSMS(new Message { To = client.TelNum, Subject = "Unknown", Content = vm.Message });
            if (result == true)
            {
                Alert("Message Sent", AlertType.success);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            Lesson existing = service.SelectLessonById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            service.DeleteLesson(id);
            Alert("Lesson Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var instructors = service.SelectAllInstructors();
            ViewBag.Instructors = new SelectList(instructors, "Id", "FullName");

            // use better naming for service methods
            var model = service.SelectLessonById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult ConfirmEdit(Lesson obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateLesson(obj))                  
                {
                    Alert("Changes to Lesson Saved", AlertType.success);
                    return RedirectToAction(nameof(Details), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Lesson", AlertType.danger);               
            }

            // re-display the edit page
            return View(nameof(Edit),obj);
      
        }

        [HttpGet]
        public IActionResult SendMessage(int id)
        {
            var client = service.SelectClientById(id);
            if(client == null) {
                return NotFound();
            }
            var message = new Message
            {
                To = client.Email,
                From = "emerbrannigan@btniternet.com"
            };
            return View(message);
        }

        [HttpPost]
        public IActionResult SendMessage(Message message)
        {
            if (message.MessageType == MessageType.Email)
            {
                if (messageService.SendMail(message))
                {
                    return RedirectToAction(nameof(Index));//new {Id = message.LessonId}
                }
            }

            if(message.MessageType == MessageType.Text)
            {
                if(messageService.SendSMS(message))
                {
                    return RedirectToAction(nameof(Index)); 
                }
            }

            if(message.MessageType == MessageType.Both)
            {
                if (messageService.SendMail(message))
                {
                    return RedirectToAction(nameof(Index));//new {Id = message.LessonId}
                }
                if (messageService.SendSMS(message))
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(message);
        }

        //------------ Attendance Controller Methods -------------- //


        [HttpGet]
        public IActionResult AttendanceIndex(string search = null)
        {
            var Attendances = service.SelectAllAttendance(search);
            return View(Attendances);
        }

        [HttpGet]
        public ActionResult AttendanceDetails(int id)
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
        public IActionResult CreateAttendance()
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
        public IActionResult ConfirmCreateAttendance(Attendance obj)
        {
            if (ModelState.IsValid)
            {
                // view is valid so create attendance
                service.InsertAttendance(obj);
                Alert("New Client Attendance Saved", AlertType.success);

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult DeleteAttendance(int id)
        {
            Attendance existing = service.SelectAttendanceById(id);
            return View(existing);
        }

        [HttpPost]
        public IActionResult ConfirmDeleteAttendance(int id)
        {
            service.DeleteAttendance(id);
            Alert("Attendance Record Deleted", AlertType.success);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditAttendance(int id)
        {
            var clients = service.SelectAllClients();
            ViewBag.clients = new SelectList(clients, "Id", "FullName");

            var horses = service.SelectAllHorses();
            ViewBag.Horses = new SelectList(horses, "Id", "HorseName");

            var lessons = service.SelectAllLessons();
            ViewBag.Lessons = new SelectList(lessons, "Id", "DateAndTime");

            // use better naming for service methods
            Attendance existing = service.SelectAttendanceById(id);

            return View(existing);
        }

        [HttpPost]
        public ActionResult ConfirmEditAttendance(Attendance obj)
        {
            if (ModelState.IsValid)
            {
                // model is valid so update attendance 
                if (service.UpdateAttendance(obj))
                {
                    Alert("Changes to Client's Attendance Saved", AlertType.success);
                    return RedirectToAction(nameof(Index), new { Id = obj.Id });
                    //return RedirectToAction("Index");
                }
                Alert("Problem Updating Attendance", AlertType.danger);
            }

            // re-display the edit page
            return View(nameof(Edit), obj);

        }
    }
}
