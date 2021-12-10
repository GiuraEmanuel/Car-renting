using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Renting.Data;

namespace Car_Renting.Controllers.Tests.AdminTests
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public async Task ReturnAllActiveCarsFromInventory()
        {
            // Arrange
            await using var context = DbSetup.Initialize();

            var cars = DbSetup.SeedCars(context);

            cars[0].Status = CarStatus.Deleted;

            await context.SaveChangesAsync();

            var adminController = new AdminController(context);

            // Act
            var result = await adminController.Inventory();

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);
            viewResult.Model.ShouldBeOfType<InventoryViewModel>();
            context.Cars.ShouldContain(car => car.Status == CarStatus.Active);
        }
    }
}
