using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebClinicGUI.Controllers;
using WebClinicGUI.Models;
using WebClinicGUI.Models.Users;
using WebClinicGUI.Services;
using Xunit;

namespace Test
{
    public class AddPatientViewTest
    {
        [Fact]
        public void AddPatient_ReturnsViewResult()
        {
            // Arrange
            var clientMock = new Mock<INetworkClient>();
            var cacheServiceMock = new Mock<ICacheService>();
            var localizerMock = new Mock <IStringLocalizer<PatientsController>> ();
            var controller = new PatientsController(clientMock.Object, cacheServiceMock.Object, localizerMock.Object);

            // Act
            var result = controller.AddPatient();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddPatient_TestForm_ValidEmail()
        {
            // Arrange
            PatientViewModel patientVM = new PatientViewModel()
            {
                FirstName = "Bob",
                LastName = "Smith",
                Sex = Sex.Male,
                DateOfBirth = new DateTime(1999, 2, 2),
                Email = "validemail@gmail.com"
            };

            var result = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(patientVM, new ValidationContext(patientVM), result);
            Assert.True(isValid);
        }

        [Fact]
        public void AddPatient_TestForm_NoEmail()
        {
            // Arrange
            PatientViewModel patientVM = new PatientViewModel()
            {
                FirstName = "Bob",
                LastName = "Smith",
                Sex = Sex.Male,
                DateOfBirth = new DateTime(1999, 2, 2)
            };

            var result = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(patientVM, new ValidationContext(patientVM), result);
            Assert.False(isValid);
            Assert.Single(result);
            Assert.Contains("'Email' field is required.", result[0].ErrorMessage);
        }
    }
}
