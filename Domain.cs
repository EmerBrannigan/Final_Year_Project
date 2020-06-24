using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TullymurrySystem.Data.Models
{

    public enum Role { Admin, Manager }

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }

    public class Instructor
    {
        public int Id { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Surname of the Instructor is Required")]
        public string Surname { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The First Name of the Instructor is Required")]
        public string FirstName { get; set; }

        [StringLength(500)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Address of the Instructor is required")]
        public string Address { get; set; }

        [StringLength(500)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "The Email Address of the Instructor is Required")]
        public string Email { get; set; }

        [StringLength(11)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Telephone Number of the Instructor is Required")]
        public string TelNum { get; set; }

        // read-only convenience property to return Fullname
        public string FullName => FirstName + " " + Surname;
    }

    public class Client
    {
        public Client()
        {
            Attendances = new List<Attendance>();
        }

        public int Id { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Surname of the Instructor is Required")]
        public string Surname { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The First Name of the Instructor is Required")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The Data of Birth of the Client is Required")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(500)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Address of the Instructor is required")]
        public string Address { get; set; }

        [StringLength(500)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "The Email Address of the Instructor is Required")]
        public string Email { get; set; }

        [StringLength(11)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Telephone Number of the Instructor is Required")]
        public string TelNum { get; set; }

        // 1-M relationship between client and attendance
        public IList<Attendance> Attendances { get; set; } //= new List<Attendance>();

        // read-only convenience property to return Fullname
        public string FullName => FirstName + " " + Surname;
    }

    public class Horse
    {
        public int Id { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Name of the Horse is Required")]
        public string HorseName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The Data of Birth of the Client is Required")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Breed of the Horse is Required")]
        public string Breed { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Level of the Horse is Required for Lessons")]
        public string HorseLevel { get; set; }
    }


    public class Lesson
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "The Date and Time of the Lesson is Required")]
        public DateTime DateAndTime { get; set; }

        [Required(ErrorMessage = "The Lesson Duration Required in minutes")]
        public int Duration { get; set; }

        [StringLength(500)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Lesson Notes are Required")]
        public string LessonType { get; set; }

        [StringLength(500)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Lesson Notes are Required")]
        public string LessonPlan { get; set; }

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        // List of attendance records associated with this lesson
        public IList<Attendance> Attendances { get; set; } = new List<Attendance>();

        public Client ContainsClient(int clientId) => Attendances
                        .Where(a => a.ClientId == clientId).Select(a => a.Client)
                        .FirstOrDefault();

    }


    public enum AttendanceStatus { Present, Absent, Excused }

    public class Attendance
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public int HorseId { get; set; }
        public Horse Horse { get; set; }

        // consider making this an enumeration?
        [EnumDataType(typeof(AttendanceStatus), ErrorMessage = "Invalid Attendance Status")]
        public AttendanceStatus AttendanceStatus { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

    }


    public class Schedule
    {
        public int Id { get; set; }

        // consider an enmeration
        public string ScheduleType { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The Date of Schedule is Required")]
        public DateTime DateOfEvent { get; set; }

        public int HorseId { get; set; }
        public Horse Horse { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }


    public class Supplier
    {
        public int Id { get; set; }

        [StringLength(60)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Name of the Supplier is Required")]
        public string SupplierName { get; set; }

        [StringLength(15)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "A Telephone Number is required for the Supplier")]
        public string TelNum { get; set; }
    }

    public class Stock
    {
        public int Id { get; set; }

        [StringLength(100)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The Name of the Product is Required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "The Quantity of the Product is Required")]
        public int Quantity { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
