using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Car_Renting.Data;
using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Car_Renting.Controllers.Tests.AdminTests
{
    [TestClass]
    public class AddVehicleGetTests
    {
        [TestMethod]
        public async Task ShowAddVehicleForm()
        {
            // Arrange
            await using var context = DbSetup.Initialize();

            var adminController = new AdminController(context);

            await context.SaveChangesAsync();

            // Act
            var result = adminController.AddVehicle();

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<AddVehicleViewModel>();
            model.LicensePlate.ShouldBe(String.Empty);
            model.Manufacturer.ShouldBe(String.Empty);
            model.Manufacturer.ShouldBe(String.Empty);
            model.PricePerDay.ShouldBe(null);
            model.Year.ShouldBe(null);
        }
    }
}
