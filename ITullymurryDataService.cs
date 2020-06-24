using System;
using System.Collections.Generic;
using TullymurrySystem.Data.Models;

namespace TullymurrySystem.Data.Services
{
    public interface ITullymurryDataService
    {
        void Initialise();
        IList<Instructor> SelectAllInstructors();
        IList<Client> SelectAllClients(string search = null);
        IList<Horse> SelectAllHorses();
        IList<Supplier> SelectAllSupplier();
        IList<Lesson> SelectAllLessons(string search = null);
        IList<Attendance> SelectAllAttendance(string search = null);
        IList<Stock> SelectAllStock();
        IList<Schedule> SelectAllSchedules(string search = null);
        Schedule GenerateSchedule(DateTime DateOfEvent);
        Stock GenerateStockReport();
        IList<Attendance> GenerateAttendanceReport(DateTime DateAndTime);

        Instructor SelectInstructorById(int Id);
        Client SelectClientById(int Id);
        Horse SelectHorseById(int Id);
        Supplier SelectSupplierById(int Id);
        Lesson SelectLessonById(int Id);
        Attendance SelectAttendanceById(int Id);
        Stock SelectStockById(int Id);
        Schedule SelectScheduleById(int Id);

        Instructor InsertInstructor(Instructor obj);
        Client InsertClient(Client obj);
        Horse InsertHorse(Horse obj);
        Supplier InsertSupplier(Supplier obj);
        Lesson InsertLesson(Lesson obj);
        Attendance InsertAttendance(Attendance obj);
        Stock InsertStock(Stock obj);
        Schedule InsertSchedule(Schedule obj);

        bool UpdateInstructor(Instructor obj);
        bool UpdateClient(Client obj);
        bool UpdateHorse(Horse obj);
        bool UpdateSupplier(Supplier obj);
        bool UpdateLesson(Lesson obj);
        bool UpdateAttendance(Attendance obj);
        bool UpdateStock(Stock obj);
        bool UpdateSchedule(Schedule obj);

        bool Delete(int id);
        bool DeleteClient(int id);
        bool DeleteHorse(int id);
        bool DeleteSupplier(int id);
        bool DeleteLesson(int id);
        bool DeleteAttendance(int id);
        bool DeleteStock(int id);
        bool DeleteSchedule(int id);
    }
}
