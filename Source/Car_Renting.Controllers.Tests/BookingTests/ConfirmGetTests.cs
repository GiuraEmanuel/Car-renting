using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests.BookingTests
{
    [TestClass]
    public class ConfirmGetTests
    {
        [TestMethod]
        public async Task UserNotLoggedInRedirectToLoginPage()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var signInManager = MockSignInManager.Create(userManager);

            var bookingController = new BookingController(context, userManager, signInManager)
            {
                Url = new MockUrlHelper()
            };
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(5);

            // Act
            var result = await bookingController.Confirm(123, startDate, endDate);

            // Assert
            var redirectResult = result.ShouldBeOfType<RedirectToPageResult>();
            redirectResult.PageName.ShouldBe("/Account/Login");
            redirectResult.RouteValues.ShouldBe(new[]
            {
                new KeyValuePair<string,object>("area","Identity"),
                new KeyValuePair<string,object>("returnUrl", $"/Confirm#?carId=123&startDate={startDate}&endDate={endDate}")
            }, ignoreOrder: true);

        }

        [TestMethod]
        public async Task InvalidDateReturnsErrorMessage()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var cars = DbSetup.SeedCars(context);

            var bookingController = new BookingController(null, null, null);

            var startDate = new DateTime(2021, 10, 10);
            var endDate = new DateTime(2021, 10, 12);
            // Act
            var result = await bookingController.Confirm(cars[1].Id, startDate, endDate);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");
            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.StartDateLessThanToday + ErrorMessages.StartBookingAgainSuffix);
        }

        [TestMethod]
        public async Task PickAvailableCarResultsSuccess()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            var signInManager = MockSignInManager.Create(userManager);
            userManager.CurrentUser = user;
            var cars = DbSetup.SeedCars(context);

            var bookingController = new BookingController(context, userManager, signInManager);

            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(5);

            // Act
            var result = await bookingController.Confirm(cars[0].Id, startDate, endDate);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);
            var model = viewResult.Model.ShouldBeOfType<BookingConfirmViewModel>();
            cars[0].Status.ShouldBe(Data.CarStatus.Active);
            model.CarId.ShouldBe(cars[0].Id);
            model.Manufacturer.ShouldBe(cars[0].Manufacturer);
            model.Model.ShouldBe(cars[0].Model);
            model.PricePerDay.ShouldBe(cars[0].PricePerDay);
            model.TotalNumberOfDays.ShouldBe(5);
            model.TotalCost.ShouldBe(cars[0].PricePerDay * 5);
        }

        [TestMethod]
        public async Task PickUnavailableCarShowsErrorMessage()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            var signInManager = MockSignInManager.Create(userManager);
            userManager.CurrentUser = user;
            var cars = DbSetup.SeedCars(context);
            var bookings = DbSetup.SeedBookings(context, user, cars[1]);

            var bookingController = new BookingController(context, userManager, signInManager);

            // Act
            var result = await bookingController.Confirm(cars[1].Id, bookings[2].StartDate, bookings[2].EndDate);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.CarUnavailable + ErrorMessages.StartBookingAgainSuffix);
        }

        [TestMethod]
        public async Task PickDeletedCarShowsErrorMessage()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            var signInManager = MockSignInManager.Create(userManager);
            userManager.CurrentUser = user;
            var cars = DbSetup.SeedCars(context);

            var bookingController = new BookingController(context, userManager, signInManager);

            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(5);

            cars[1].Status = Data.CarStatus.Deleted;
            context.SaveChanges();

            // Act
            var result = await bookingController.Confirm(cars[1].Id, startDate, endDate);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.CarUnavailable + ErrorMessages.StartBookingAgainSuffix);
        }
    }
}
