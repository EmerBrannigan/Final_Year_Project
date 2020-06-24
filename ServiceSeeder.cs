using System;
using TullymurrySystem.Data.Services;
using TullymurrySystem.Data.Models;


namespace TullymurrySystem.Data.Services
{
    public class ServiceSeeder
    {
        
        public static void Seed(TullymurryDataService service)
        {
            service.Initialise();

            var i1 = service.InsertInstructor(new Instructor
            {
                Surname = "Turley",
                FirstName = "Sarah",
                Address = "145 Ballydugan Road, Downpatrick, BT30 8HH",
                Email = "sarahturley@tullymurry.com",
                TelNum = "07761854345"
            });

            var i2 = service.InsertInstructor(new Instructor
            {
                Surname = "Bloggs",
                FirstName = "Joe",
                Address = "145 Main St, Portadown, BT31 5HT",
                Email = "joe@bloggs.com",
                TelNum = "0772345665"
            });

            var c1 = service.InsertClient(new Client
            {
                Surname = "Brannigan",
                FirstName = "Emer",
                DateOfBirth = new DateTime(1998,02,26),
                Address = "23 The Meadows, Downpatrick, BT30 6LN",
                Email = "emerbrannigan@btinternet.com",
                TelNum = "07549254269"
            });

            var c2 = service.InsertClient(new Client
            {
                Surname = "Smith",
                FirstName = "John",
                DateOfBirth = new DateTime(1978, 02, 26),
                Address = "23 Main St Downpatrick",
                Email = "jsmith@btinternet.com",
                TelNum = "0735656669"
            });

            var h1 = service.InsertHorse(new Horse
            {
                HorseName = "Finbar",
                DateOfBirth = new DateTime(2006, 05, 16),
                Breed = "Friesian",
                HorseLevel = "Beginner/Intermediate/Advanced"
            });

            var h2 = service.InsertHorse(new Horse
            {
                HorseName = "Jockey",
                DateOfBirth = new DateTime(2008, 05, 16),
                Breed = "Friesian",
                HorseLevel = "Beginner/Intermediate/Advanced"
            });

            var s1 = service.InsertSupplier(new Supplier
            {
                SupplierName = "Baileys Horse Feeds",
                TelNum = "02876498531"
            });


            var l1 = service.InsertLesson(new Lesson
            {

                DateAndTime = new DateTime(2019, 12, 14),
                LessonPlan = "Lesson Plan ........",
                LessonType = "Jumping",                
                Duration = 60,
                InstructorId = i1.Id
            });

            var l2 = service.InsertLesson(new Lesson
            {

                DateAndTime = new DateTime(2020, 2, 7),
                LessonPlan = "Lesson Plan ........",
                LessonType = "Trotting",
                Duration = 45,
                InstructorId = i2.Id
            });

            var a1 = service.InsertAttendance(new Attendance
            {
                AttendanceStatus = AttendanceStatus.Present,
                LessonId = l1.Id,
                ClientId = c1.Id,
                HorseId = h1.Id,
                Notes = "Did very well"
            });
            var a2 = service.InsertAttendance(new Attendance
            {
                AttendanceStatus = AttendanceStatus.Present,
                LessonId = l1.Id,
                ClientId = c2.Id,
                HorseId = h2.Id,
                Notes = "Struggled controlling the horsr"
            });

            var c8 = service.InsertStock(new Stock
            {
                ProductName = "No.8 Meadow Sweet with Turmeric",
                Quantity = 9,
                SupplierId = s1.Id
            });

            var c9 = service.InsertSchedule(new Schedule
            {
                ScheduleType = "Supply Order Arriving",
                DateOfEvent = new DateTime(2020,01,02),
                HorseId = h1.Id,
                SupplierId = s1.Id
            });

            var u1 = service.RegisterUser("admin@gmail.com", "admin", Role.Admin);

        }
    }
}
