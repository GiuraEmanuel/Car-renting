using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Car_Renting.Data;
using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Car_Renting.Controllers.Tests.AdminTests
{
    [TestClass]
    public class DeleteVehiclePostTests
    {
        [TestMethod]
        public async Task ReturnErrorMessageIfCarDoesNotExist()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var adminController = new AdminController(context);

            // Act
            var result = await adminController.DeleteVehicle(123);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");
            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.CarDoesNotExist);
        }

        [TestMethod]
        public async Task SuccessfullyDeleteCar()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var cars = DbSetup.SeedCars(context);

            var adminController = new AdminController(context);

            // Act
            var result = await adminController.DeleteVehicle(cars[0].Id);

            // Assert
            var redirectResult = result.ShouldBeOfType<RedirectToActionResult>();
            cars[0].Status.ShouldBe(CarStatus.Deleted);
            redirectResult.ControllerName.ShouldBe(null);
            redirectResult.ActionName.ShouldBe("Inventory");
        }
    }
}
