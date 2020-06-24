using System;
using Xunit;
using TullymurrySystem.Data.Services;
using System.Linq;

namespace TullymurrySystem.Test
{
    public class TullymurryServiceTests
    {
        private TullymurryDataService service;
        private TullymurryMessageService MessageService;

        public TullymurryServiceTests()
        {
            service = new TullymurryDataService();
            MessageService = new TullymurryMessageService();
            ServiceSeeder.Seed(service);
        }

        [Fact]
        public void TestSelectAllInstructors()
        {
            var instructors = service.SelectAllInstructors();
            var instructorCount = instructors.Count();

            Assert.Equal(1, instructorCount);
        }

        [Fact]
        public void TestClientSelectById()
        {
            var client = service.SelectClientById(1);
            string clientName = client.Surname;
            Assert.Equal("Brannigan", clientName);
        }

        [Fact]
        public void TestInsertClient()
        {
            var client = service.InsertClient(new TullymurrySystem.Data.Models.Client
            {
                FirstName = "Diane",
                Surname = "Vance",
                DateOfBirth = DateTime.Parse("18/06/66"),
                Address = "100 Stream Street",
                TelNum = "077356892",
                Email = "diane.vance@gmail.com"
            });

            Assert.Equal("Diane", client.FirstName);
        }

        [Fact]
        public void TestDeleteClient()
        {
            var client = service.SelectClientById(1);
            var result = service.DeleteClient(1);

            Assert.True(result);
            client = service.SelectClientById(1);
            Assert.Null(client);
        }

        [Fact]
        public void TestUpdateClient()
        {
            var client = service.SelectClientById(1);
            client.FirstName = "Elaine";
            service.UpdateClient(client);

            client = service.SelectClientById(1);
            Assert.Equal("Elaine", client.FirstName);
        }
    }
}

