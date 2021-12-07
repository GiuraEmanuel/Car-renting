using Car_Renting.Data;
using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests.BookingTests
{
    [TestClass]
    public class ConfirmPostTests
    {
        [TestMethod]
        public async Task InvalidDateReturnsErrorMessage()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var cars = DbSetup.SeedCars(context);

            var bookingController = new BookingController(null, null, null);

            var postModel = new BookingConfirmPostModel()
            {
                StartDate = new DateTime(2021, 10, 10),
                EndDate = new DateTime(2021, 10, 12)
            };
            // Act
            var result = await bookingController.Confirm(postModel);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");
            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.StartDateLessThanToday + ErrorMessages.StartBookingAgainSuffix);
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
            var postModel = new BookingConfirmPostModel
            {
                CarId = cars[1].Id,
                StartDate = bookings[1].StartDate,
                EndDate = bookings[1].EndDate
            };

            // Act
            var result = await bookingController.Confirm(postModel);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.CarUnavailable + ErrorMessages.StartBookingAgainSuffix);
        }

        [TestMethod]
        public async Task ShowErrorMessageIfCarPriceChanged()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            var signInManager = MockSignInManager.Create(userManager);
            userManager.CurrentUser = user;
            var cars = DbSetup.SeedCars(context);

            var bookingController = new BookingController(context, userManager, signInManager);

            // Purposely set the total cost to an incorrect value to simulate the car's price per day changing.
            var postModel = new BookingConfirmPostModel
            {
                CarId = cars[1].Id,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5),
                TotalCost = cars[1].PricePerDay * 5 + 10 // add 10 to make this value incorrect
            };

            // Act
            var result = await bookingController.Confirm(postModel);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.CarPriceChanged + ErrorMessages.StartBookingAgainSuffix);
        }

        [TestMethod]
        public async Task BookingSuccessfullyConfirmed()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;
            var signInManager = MockSignInManager.Create(userManager);
            var cars = DbSetup.SeedCars(context);
            var bookingController = new BookingController(context, userManager, signInManager);

            var postModel = new BookingConfirmPostModel
            {
                CarId = cars[0].Id,
                StartDate = DateTime.Today.AddDays(25),
                EndDate = DateTime.Today.AddDays(30),
                TotalCost = cars[0].PricePerDay * 5,
            };

            // Act
            var result = await bookingController.Confirm(postModel);

            // Assert
            var redirectResult = result.ShouldBeOfType<RedirectToActionResult>();
            var newBooking = context.Bookings.Single();
            newBooking.UserId.ShouldBe(user.Id);
            newBooking.CarId.ShouldBe(postModel.CarId);
            newBooking.StartDate.ShouldBe(postModel.StartDate);
            newBooking.EndDate.ShouldBe(postModel.EndDate);
            newBooking.TotalCost.ShouldBe(postModel.TotalCost);
            redirectResult.ActionName.ShouldBe("Detail");
            redirectResult.RouteValues.ShouldBe(new[]
            {
                new KeyValuePair<string,object>("id", 1)
            });
        }
    }
}
