using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests.BookingTests
{
    [TestClass]
    public class CancelTests
    {
        [TestMethod]
        public async Task BookingDoesNotExist()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;
            var signInManager = MockSignInManager.Create(userManager);

            var bookingController = new BookingController(context, userManager, signInManager);

            var postModel = new BookingCancelPostModel
            {
                Id = 123,
                RefundAmount = 20
            };

            // Act
            var result = await bookingController.Cancel(postModel);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.BookingNotFound);
        }

        [TestMethod]
        public async Task BookingAlreadyCancelled()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;
            var signInManager = MockSignInManager.Create(userManager);

            var cars = DbSetup.SeedCars(context);
            var bookings = DbSetup.SeedBookings(context, user, cars[1]);

            var bookingController = new BookingController(context, userManager, signInManager);

            var postModel = new BookingCancelPostModel
            {
                Id = bookings[2].Id,
                RefundAmount = 20
            };

            bookings[2].Cancel(postModel.RefundAmount);
            context.SaveChanges();

            // Act
            var result = await bookingController.Cancel(postModel);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.BookingAlreadyCancelled);
        }

        [TestMethod]
        public async Task RefundAmountChanged()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;
            var signInManager = MockSignInManager.Create(userManager);

            var cars = DbSetup.SeedCars(context);
            var bookings = DbSetup.SeedBookings(context, user, cars[1]);

            var bookingController = new BookingController(context, userManager, signInManager);

            var postModel = new BookingCancelPostModel
            {
                Id = bookings[2].Id,
                RefundAmount = 15
            };

            // Act
            var result = await bookingController.Cancel(postModel);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.RefundAmountChanged);
        }

        [TestMethod]
        public async Task BookingSuccessfullyCancelled()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;
            var signInManager = MockSignInManager.Create(userManager);

            var cars = DbSetup.SeedCars(context);
            var bookings = DbSetup.SeedBookings(context, user, cars[1]);

            var bookingController = new BookingController(context, userManager, signInManager);

            var postModel = new BookingCancelPostModel
            {
                Id = bookings[2].Id,
                RefundAmount = 20
            };

            // Act
            var result = await bookingController.Cancel(postModel);

            // Assert
            var redirectResult = result.ShouldBeOfType<RedirectToActionResult>();
            redirectResult.ActionName.ShouldBe("Detail");
            redirectResult.RouteValues.ShouldBe(new[]
            {
                new KeyValuePair<string,object>("id", 1)
            });
        }
    }
}
