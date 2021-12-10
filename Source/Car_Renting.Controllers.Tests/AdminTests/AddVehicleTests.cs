using System.Linq;
using System.Threading.Tasks;
using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Car_Renting.Controllers.Tests.AdminTests
{
    [TestClass]
    public class AddVehicleTests
    {
        [TestMethod]
        public async Task ReturnViewAgainIfModelIsInvalid()
        {
            // Arrange
            var adminController = new AdminController(null);
            adminController.ModelState.AddModelError("Test", "Test error");

            var addVehicleVm = new AddVehicleViewModel();

            // Act
            var result = await adminController.AddVehicle(addVehicleVm);

            // Assert
            var viewResult = result.ShouldBeOfType<PartialViewResult>();
            viewResult.ViewName.ShouldBe("_AddCarModalForm");
            viewResult.Model.ShouldBe(addVehicleVm);
        }

        [TestMethod]
        public async Task CarWithSameLicensePlateReturnsErrorMessage()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var adminController = new AdminController(context);
            var cars = DbSetup.SeedCars(context);

            var addVehicleVm = new AddVehicleViewModel
            {
                LicensePlate = cars[0].LicensePlate,
                Manufacturer = cars[0].Manufacturer,
                Model = cars[0].Model,
                PricePerDay = cars[0].PricePerDay,
                Year = cars[0].Year
            };

            // Act
            var result = await adminController.AddVehicle(addVehicleVm);

            // Assert
            var viewResult = result.ShouldBeOfType<PartialViewResult>();
            viewResult.ViewName.ShouldBe("_AddCarModalForm");
            adminController.ModelState.ShouldContain(error => error.Key == "LicensePlate" && error.Value.Errors.Single().ErrorMessage == ErrorMessages.SameLicensePlate);
            viewResult.Model.ShouldBe(addVehicleVm);
        }

        [TestMethod]
        public async Task SuccessfullyAddCarToInventory()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var cars = DbSetup.SeedCars(context);


            var adminController = new AdminController(context);

            var addVehicleVm = new AddVehicleViewModel
            {
                LicensePlate = "99-99-AA",
                Manufacturer = cars[0].Manufacturer,
                Model = cars[0].Model,
                PricePerDay = cars[0].PricePerDay,
                Year = cars[0].Year
            };

            // Act
            var result = await adminController.AddVehicle(addVehicleVm);

            // Assert
            var redirectResult = result.ShouldBeOfType<RedirectToActionResult>();
            redirectResult.ControllerName.ShouldBe(null);
            redirectResult.ActionName.ShouldBe("Inventory");
            context.Cars.Count().ShouldBe(cars.Count + 1);
            context.Cars.ShouldContain(car => car.LicensePlate == addVehicleVm.LicensePlate
                                              && car.Manufacturer == addVehicleVm.Manufacturer
                                              && car.Model == addVehicleVm.Model
                                              && car.PricePerDay == addVehicleVm.PricePerDay
                                              && car.Year == addVehicleVm.Year);
        }
    }
}
