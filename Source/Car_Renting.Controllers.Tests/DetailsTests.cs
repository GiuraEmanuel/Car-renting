using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests
{
    [TestClass]
    public class DetailsTests
    {
        [TestMethod]
        public async Task UserChecksHisOwnBookingDetails()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = context.SeedUsers();
            var cars = context.SeedCars();
            var bookings = DbSetup.SeedBookings(userManager, cars[1]);
            userManager.CurrentUser = userManager.NormalUser;

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Detail(bookings[1].Id);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingDetailsViewModel>();
            model.BookingId.ShouldBe(bookings[1].Id);
            model.TotalNumberOfDays.ShouldBe(19);
            model.CancelRefundAmount.ShouldBe(null);
            model.RefundAmountIfCancelling.ShouldBe(0);
            model.Email.ShouldBe("jason.bourne@gmail.com");
        }

        [TestMethod]
        public async Task UserIsNotAllowedToSeeSomeoneElsesBookingDetails()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = context.SeedUsers();
            var cars = context.SeedCars();
            var bookings = DbSetup.SeedBookings(userManager, cars[0]);
            userManager.CurrentUser = userManager.NormalUser;

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Detail(bookings[0].Id);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe("You are not allowed to see the details of this booking.");
        }
    }
}
