using System;
using Xunit;

using WebClinicAPI.Data;
using Moq;
using WebClinicAPI.Controllers;
using System.Collections.Generic;
using WebClinicAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebClinicAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Test
{
    public class AppointmentsControllerTest
    {

        
        private DbContextOptions<ApplicationDbContext> CreateOptions(string name)
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;
        }

        private void FakeDatabaseSeed(string dbName)
        {
            var options = CreateOptions(dbName);

            // Insert seed data into the database using one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                context.Patients.Add(new Patient
                {
                    Id = 1,
                    Email = "patient1@gmail.com",
                    FirstName = "Geralt",
                    LastName = "Z Rivii",
                    DateOfBirth = new DateTime(1979, 10, 22),
                    Sex = Sex.Male,
                    Password = "pass",
                    Role = Role.Patient
                });
                context.Physicians.Add(new Physician
                {
                    Id = 2,
                    Email = "physician1@gmail.com",
                    FirstName = "Nathan",
                    LastName = "Drake",
                    DateOfBirth = new DateTime(1970, 11, 19),
                    Sex = Sex.Male,
                    Specialization = PhysicianSpecialization.Cardiologist,
                    Password = "pass",
                    Role = Role.Physician
                });
                context.Appointments.Add(new Appointment
                {
                    PatientId = 1,
                    PhysicianId = 2,
                    Time = new DateTime(2019, 12, 4, 12, 30, 0),
                    Type = AppointmentType.Consultation
                });
                context.Appointments.Add(new Appointment
                {
                    PatientId = 1,
                    PhysicianId = 2,
                    Time = new DateTime(2019, 12, 5, 15, 0, 0),
                    Type = AppointmentType.Consultation
                });
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetAppointments_WhenCalled_ReturnsUnauthorizedWhenUserNotAuthorized()
        {
            using (var context = new ApplicationDbContext(CreateOptions(Guid.NewGuid().ToString())))
            {
                // Arrange
                var controller = new AppointmentsController(context);

                var httpContextMock = new Mock<HttpContext>();
                httpContextMock.Setup(h => h.User.IsInRole(It.IsAny<string>()))
                    .Returns(false);

                var controllerContextMock = new Mock<ControllerContext>();
                controllerContextMock.Object.HttpContext = httpContextMock.Object;

                controller.ControllerContext = controllerContextMock.Object;

                // Act
                var result = await controller.GetAppointments();
                
                // Assert
                Assert.IsType<UnauthorizedResult>(result.Result);
            }
        }

        [Fact]
        public async Task GetAppointments_WhenCalled_ReturnsAllItemsForReceptionist()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            FakeDatabaseSeed(dbName);

            using (var context = new ApplicationDbContext(CreateOptions(dbName)))
            {
                // Arrange
                var controller = new AppointmentsController(context);

                var httpContextMock = new Mock<HttpContext>();
                httpContextMock.Setup(h => h.User.IsInRole("Receptionist"))
                    .Returns(true);

                var controllerContextMock = new Mock<ControllerContext>();
                controllerContextMock.Object.HttpContext = httpContextMock.Object;

                controller.ControllerContext = controllerContextMock.Object;

                // Act
                var result = await controller.GetAppointments();

                // Assert
                var appointments = Assert.IsType<List<Appointment>>(result.Value);
                Assert.Equal(2, appointments.Count);
            }
        }

        [Fact]
        public void GetFreeTerms_ReturnsFreeTerms()
        {
        }
    }
}
