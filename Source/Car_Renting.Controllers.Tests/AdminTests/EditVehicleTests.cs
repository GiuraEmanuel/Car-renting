using System.Threading.Tasks;
using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Car_Renting.Controllers.Tests.AdminTests
{
    [TestClass]
    public class EditVehicleTests
    {
        [TestMethod]
        public async Task ReturnViewIfModelIsInvalid()
        {
            // Arrange
            var adminController = new AdminController(null);

            adminController.ModelState.AddModelError("Test", "Test error");

            var editVehicleVm = new EditVehicleViewModel();
            

            // Act
            var result = await adminController.EditVehicle(0, editVehicleVm);

            // Assert
            var partialViewResult = result.ShouldBeOfType<PartialViewResult>();
            partialViewResult.ViewName.ShouldBe("_EditVehicleModalForm");
            partialViewResult.Model.ShouldBe(editVehicleVm);
        }

        [TestMethod]
        public async Task ReturnErrorMessageIfCarDoesNotExist()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var adminController = new AdminController(context);


            // Act
            var result = await adminController.EditVehicle(123, null);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");
            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.CarDoesNotExist);
        }

        [TestMethod]
        public async Task SuccessfullyChangeCarPrice()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var cars = DbSetup.SeedCars(context);

            var adminController = new AdminController(context);

            var editVehicleVm = new EditVehicleViewModel
            {
                Id = cars[0].Id,
                PricePerDay = cars[0].PricePerDay + 10
            };

            await context.SaveChangesAsync();

            // Act
            var result = await adminController.EditVehicle(editVehicleVm.Id, editVehicleVm);

            // Assert
            var partialViewResult = result.ShouldBeOfType<PartialViewResult>();
            partialViewResult.ViewName.ShouldBe("_SuccessModalContent");
            partialViewResult.Model.ShouldBeOfType<SuccessModalContentViewModel>();
            cars[0].PricePerDay.ShouldBe(editVehicleVm.PricePerDay);
        }
    }
}
