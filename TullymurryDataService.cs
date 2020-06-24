using System;
using System.Collections.Generic;
using System.Linq;
using TullymurrySystem.Data.Models;
using TullymurrySystem.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace TullymurrySystem.Data.Services
{
    public class TullymurryDataService : ITullymurryDataService
    {
        private TullymurryDBContext db;

        public TullymurryDataService()
        {
            this.db = new TullymurryDBContext();
        }

        public TullymurryDataService(TullymurryDBContext db)
        {
            this.db = db;
        }

        public void Initialise()
        {
            db.Initialise();
        }

        // ===============   Reports ====================================

        public IList<Schedule> GenerateSchedule(DateTime DateOfEvent)
        {
            return db.Schedules.OrderBy(a => a.DateOfEvent == DateOfEvent).ToList();
        }

        Schedule ITullymurryDataService.GenerateSchedule(DateTime DateOfEvent)
        {
            throw new NotImplementedException();
        }

        public Stock GenerateStockReport()
        {
            throw new NotImplementedException();
        }


        public IList<Attendance> GenerateAttendanceReport(DateTime DateAndTime)
        {
            throw new NotImplementedException();
        }

        // ==================== CRUD Operations =============================

        public IList<Instructor> SelectAllInstructors()
        {
            return db.Instructors
                .OrderBy(a => a.Surname)
                .ToList();
        }

        public IList<Client> SelectAllClients(string search = null)
        {
            if (search != null)
            {
                return db.Clients
                    .Where(a => a.Surname.Contains(search))
                    .ToList();
            }
            else
            {
                return db.Clients
                    .OrderBy(l => l.Surname).ToList();
            }
        }

        public IList<Horse> SelectAllHorses()
        {
            return db.Horses
                .OrderBy(a => a.HorseName)
                .ToList();
        }

        public IList<Supplier> SelectAllSupplier()
        {
            return db.Suppliers
                .OrderBy(a => a.SupplierName)
                .ToList();
        }

        // return all lessons or optionally lessons taken instructor with specified surname
        public IList<Lesson> SelectAllLessons(string search = null)
        {
            if (search != null)
            {
                return db.Lessons
                    .Where(l => l.Instructor.Surname.Contains(search))
                    .Include(l =>l.Instructor)
                    .ToList();
            }
            else
            {
                return db.Lessons
                    .Include(l => l.Instructor)
                    .OrderBy(l => l.DateAndTime).ToList();
            }
        }


        public IList<Attendance> SelectAllAttendance(string search = null)
        {
            if (search != null)
            {
                return db.Attendances
                    .Where(a => a.Client.Surname.Contains(search))
                    .Include(l => l.Client)
                    .Include(h => h.Horse)
                    .ToList();
            }
            else
            {
                return db.Attendances
                    .Include(a => a.Client) // eager load the client
                    .Include(a => a.Horse)  // eager load the horse
                    .OrderBy(a => a.Client).ToList();
            }
        }

        public IList<Stock> SelectAllStock()
        {
            return db.Stocks
                .OrderBy(a => a.ProductName)
                .Include(a => a.Supplier)
                .ToList();
        }

        public IList<Schedule> SelectAllSchedules(string search = null)
        {
            if (search != null)
            {
                return db.Schedules
                    .Where(s => s.ScheduleType.Contains(search))
                    .ToList();
            }
            else
            {
                return db.Schedules
                .OrderBy(a => a.DateOfEvent)
                .Include(s => s.Horse)
                .Include(s => s.Supplier)
                .ToList();
            }
        }

        public Instructor SelectInstructorById(int id)
        { 
            return db.Instructors
                .OrderBy(i => i.Surname)
                .FirstOrDefault(a => a.Id == id);
        }

        public Client SelectClientById(int id)
        {
            return db.Clients
                .OrderBy(c => c.Surname)
                .FirstOrDefault(a => a.Id == id);
        }

        public Horse SelectHorseById(int id)
        {
            return db.Horses
                .OrderBy(h => h.HorseName)
                .FirstOrDefault(a => a.Id == id);
        }

        public Supplier SelectSupplierById(int id)
        {
            return db.Suppliers
                .OrderBy(s => s.SupplierName)
                .FirstOrDefault(a => a.Id == id);
        }
 
        // retrieve lesson and associated instructor
        public Lesson SelectLessonById(int id)
        {
            return db.Lessons
                     .Include(l => l.Instructor)
                     .Include("Attendances.Client")
                     .Include("Attendances.Horse")
                     .OrderBy(l => l.DateAndTime)
                     .FirstOrDefault(a => a.Id == id);
        }

        // rename to SelectAttendanceById
        public Attendance SelectAttendanceById(int id)
        {
            return db.Attendances
                .Include( a => a.Client) // eager load the client
                .Include(a => a.Horse)  // eager load the horse
                .Include( a => a.Lesson) // eager load the lesson
                .ThenInclude( l => l.Instructor) // eager load the lesson instructor
                .FirstOrDefault(a => a.Id == id);
        }

        public Stock SelectStockById(int id)
        {
            return db.Stocks.FirstOrDefault(a => a.Id == id);
        }

        public Schedule SelectScheduleById(int id)
        {
            return db.Schedules.FirstOrDefault(a => a.Id == id);
        }

      
        public Instructor InsertInstructor(Instructor obj)
        {
            db.Instructors.Add(obj);
            db.SaveChanges(); 
            return obj;
        }

        public Client InsertClient(Client obj)
        {
            db.Clients.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public Horse InsertHorse(Horse obj)
        {
            db.Horses.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public Supplier InsertSupplier(Supplier obj)
        {
            db.Suppliers.Add(obj);
            db.SaveChanges();
            return obj;
        }


        public Lesson InsertLesson(Lesson obj)
        {
            db.Lessons.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public Attendance InsertAttendance(Attendance obj)
        {
            db.Attendances.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public Stock InsertStock(Stock obj)
        {
            db.Stocks.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public Schedule InsertSchedule(Schedule obj)
        {
            db.Schedules.Add(obj);
            db.SaveChanges();
            return obj;
        }

        public bool UpdateInstructor(Instructor obj)
        {
            // ** verify that the lesson record exists first **/
            var instructor = SelectInstructorById(obj.Id);
            if (instructor == null)
            {
                return false;
            }
            // ** we now need to detatch the attendance as it will confict with obj when updating **/
            db.Entry(instructor).State = EntityState.Detached;
            db.Update(obj);
            db.SaveChanges();
            return true;
        }

        public bool UpdateClient(Client obj)
        {
            // ** verify that the lesson record exists first **/
            var client = SelectClientById(obj.Id);
            if (client == null)
            {
                return false;
            }
            // ** we now need to detatch the attendance as it will confict with obj when updating **/
            db.Entry(client).State = EntityState.Detached;
            db.Update(obj);
            db.SaveChanges();
            return true;
        }

        public bool UpdateHorse(Horse obj)
        {
            // ** verify that the lesson record exists first **/
            var horse = SelectHorseById(obj.Id);
            if (horse == null)
            {
                return false;
            }
            // ** we now need to detatch the attendance as it will confict with obj when updating **/
            db.Entry(horse).State = EntityState.Detached;
            db.Update(obj);
            db.SaveChanges();
            return true;
        }

        public bool UpdateSupplier(Supplier obj)
        {
            // ** verify that the lesson record exists first **/
            var supplier = SelectSupplierById(obj.Id);
            if (supplier == null)
            {
                return false;
            }
            // ** we now need to detatch the attendance as it will confict with obj when updating **/
            db.Entry(supplier).State = EntityState.Detached;
            db.Update(obj);
            db.SaveChanges();
            return true;
        }


        public bool UpdateLesson(Lesson obj)
        {
            // ** verify that the lesson record exists first **/
            var lesson = SelectLessonById(obj.Id);
            if (lesson == null)
            {
                return false;
            }
            // ** we now need to detatch the attendance as it will confict with obj when updating **/
            db.Entry(lesson).State = EntityState.Detached;
            db.Update(obj);
            db.SaveChanges();
            return true;
        }

        public bool UpdateAttendance(Attendance obj)
        {
            // ** verify that the lesson record exists first **/
            var attendance = SelectAttendanceById(obj.Id);
            if (attendance == null)
            {
                return false;
            }
            // ** we now need to detatch the attendance as it will confict with obj when updating **/
            db.Entry(attendance).State = EntityState.Detached;
            db.Update(obj);
            db.SaveChanges();
            return true;
        }

        public bool UpdateStock(Stock obj)
        {
            // ** verify that the lesson record exists first **/
            var stock = SelectStockById(obj.Id);
            if (stock == null)
            {
                return false;
            }
            // ** we now need to detatch the attendance as it will confict with obj when updating **/
            db.Entry(stock).State = EntityState.Detached;
            db.Update(obj);
            db.SaveChanges();
            return true;
        }

        public bool UpdateSchedule(Schedule obj)
        {
            var schedule = SelectScheduleById(obj.Id);
            if (schedule == null)
            {
                return false;
            }
            // ** we now need to detatch the attendance as it will confict with obj when updating **/
            db.Entry(schedule).State = EntityState.Detached;
            db.Update(obj);
            db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var existing = db.Instructors.FirstOrDefault(a => a.Id == id);
            if (existing == null)
            {
                return false;
            }
            db.Remove(existing);
            db.SaveChanges();
            return true;
        }

        public bool DeleteClient(int id)
        {
            var existing = db.Clients.FirstOrDefault(a => a.Id == id);
            if (existing == null)
            {
                return false;
            }
            db.Remove(existing);
            db.SaveChanges();
            return true;
        }

        public bool DeleteHorse(int id)
        {
            var existing = db.Horses.FirstOrDefault(a => a.Id == id);
            if (existing == null)
            {
                return false;
            }
            db.Remove(existing);
            db.SaveChanges();
            return true;
        }

        public bool DeleteSupplier(int id)
        {
            var existing = db.Suppliers.FirstOrDefault(a => a.Id == id);
            if (existing == null)
            {
                return false;
            }
            db.Remove(existing);
            db.SaveChanges();
            return true;
        }


        public bool DeleteLesson(int id)
        {
            var existing = db.Lessons.FirstOrDefault(a => a.Id == id);
            if (existing == null)
            {
                return false;
            }
            db.Remove(existing);
            db.SaveChanges();
            return true;
        }

        public bool DeleteAttendance(int id)
        {
            var existing = db.Attendances.FirstOrDefault(a => a.Id == id);
            if (existing == null)
            {
                return false;
            }
            db.Remove(existing);
            db.SaveChanges();
            return true;
        }

        public bool DeleteStock(int id)
        {
            var existing = db.Stocks.FirstOrDefault(a => a.Id == id);
            if (existing == null)
            {
                return false;
            }
            db.Remove(existing);
            db.SaveChanges();
            return true;
        }

        public bool DeleteSchedule(int id)
        {
            var existing = db.Schedules.FirstOrDefault(a => a.Id == id);
            if (existing == null)
            {
                return false;
            }
            db.Remove(existing);
            db.SaveChanges();
            return true;
        }

        public IQueryable<Attendance> GetClientAttendanceRecord()
        {
            var attendance = from a in db.Attendances
                        select new Attendance()
                        {
                            Id = a.Id,
                            Client = a.Client,
                            AttendanceStatus = a.AttendanceStatus
                        };

            return attendance;
        }

        // ================= Authentication ===============================

        public User GetUserByCredentials(string username, string password)
        {
            return db.Users.FirstOrDefault(s => s.Username == username && s.Password == password);
        }

        public User GetUserByName(string username)
        {
            return db.Users.FirstOrDefault(s => s.Username == username);
        }
        // Register new user
        public User RegisterUser(string username, string password, Role role)
        {
            var o = GetUserByName(username);
            if (o != null)
            {
                return null;
            }
            // user is unique so store in database
            User user = new User { Username = username, Password = password, Role = role };

            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }
    }
}
