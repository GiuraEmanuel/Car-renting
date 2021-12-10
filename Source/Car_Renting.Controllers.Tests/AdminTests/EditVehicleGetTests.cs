using System.Threading.Tasks;
using Car_Renting.Data;
using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Car_Renting.Controllers.Tests.AdminTests
{
    [TestClass]
    public class EditVehicleGetTests
    {
        [TestMethod]
        public async Task CarsDoesNotExistReturnsErrorMessage()
        {
            // Arrange
            using var context = DbSetup.Initialize();

            var adminController = new AdminController(context);

            // Act
            var result = await adminController.EditVehicle(123);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");
            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.CarDoesNotExist);
        }

        [TestMethod]
        public async Task SuccessfullyReturnCarWithTheRightData()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var cars = DbSetup.SeedCars(context);

            var adminController = new AdminController(context);

            var editVehicleVm = new EditVehicleViewModel
            {
                Id = cars[0].Id,
                PricePerDay = cars[0].PricePerDay
            };

            // Act
            var result = await adminController.EditVehicle(editVehicleVm.Id);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);
            var model = viewResult.Model.ShouldBeOfType<EditVehicleViewModel>();
            model.Id.ShouldBe(cars[0].Id);
            model.PricePerDay.ShouldBe(cars[0].PricePerDay);
        }
    }
}
