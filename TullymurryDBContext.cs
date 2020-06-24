using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TullymurrySystem.Data.Models;

namespace TullymurrySystem.Data.Repository
{
    public class TullymurryDBContext : DbContext
    {
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Horse> Horses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        /** for autentication **/
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocaldb;Database=TullymurryDB;Trusted_Connection=True;");
            optionsBuilder
                 .UseSqlite("Filename=TullymurryDB") /** using sqlite as its simpler when in develoment mode **/
                 .UseLoggerFactory(new ServiceCollection()//***//* logger to log the sql commands issued by entityframework **//*
                     .AddLogging(builder => builder.AddConsole())
                     .BuildServiceProvider()
                     .GetService<ILoggerFactory>()
                 );

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API property configurations here so you can leave the models as plain POCO
            //modelBuilder.Entity<Horse>()
            //       .Property(h => h.Breed)
            //        .HasMaxLength(30);

        }

        public void Initialise()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
