using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests.BookingTests
{
    [TestClass]
    public class DetailTests
    {
        [TestMethod]
        public async Task UserChecksHisOwnBookingDetails()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;

            var cars = DbSetup.SeedCars(context);
            var bookings = DbSetup.SeedBookings(context, user, cars[1]);

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Detail(bookings[0].Id);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingDetailsViewModel>();
            model.BookingId.ShouldBe(bookings[0].Id);
            model.TotalNumberOfDays.ShouldBe(19);
            model.CancelRefundAmount.ShouldBe(null);
            // past booking so refund should be 0
            model.RefundAmountIfCancelling.ShouldBe(0);
            model.Email.ShouldBe("jason.bourne@gmail.com");
            model.Model.ShouldBe(cars[1].Model);
        }

        [TestMethod]
        public async Task UserIsNotAllowedToSeeSomeoneElsesBookingDetails()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;

            var cars = DbSetup.SeedCars(context);
            var bookings = DbSetup.SeedBookings(context, admin, cars[0]);

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Detail(bookings[0].Id);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.BookingAccessDenied);
        }

        [TestMethod]
        public async Task AdminChecksUsersBookings()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = admin;

            var cars = DbSetup.SeedCars(context);
            var bookings = DbSetup.SeedBookings(context, user, cars[2]);

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Detail(bookings[1].Id);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingDetailsViewModel>();
            model.BookingId.ShouldBe(bookings[1].Id);
            model.TotalNumberOfDays.ShouldBe(3);
            model.CancelRefundAmount.ShouldBe(null);
            // partial refund for 2 days since booking started today
            model.RefundAmountIfCancelling.ShouldBe(40);
            model.Email.ShouldBe("jason.bourne@gmail.com");
            model.Model.ShouldBe(cars[2].Model);
        }

        [TestMethod]
        public async Task UserChecksHisOwnCancelledBookingDetails()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;

            var cars = DbSetup.SeedCars(context);
            var bookings = DbSetup.SeedBookings(context, user, cars[1]);

            bookings[2].Cancel(20);
            context.SaveChanges();

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Detail(bookings[2].Id);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingDetailsViewModel>();
            model.BookingId.ShouldBe(bookings[2].Id);
            model.TotalNumberOfDays.ShouldBe(5);
            model.CancelRefundAmount.ShouldBe(20);
            // future booking already cancelled so refund should be 0
            model.RefundAmountIfCancelling.ShouldBe(0);
            model.Email.ShouldBe("jason.bourne@gmail.com");
            model.Model.ShouldBe(cars[1].Model);
        }

        [TestMethod]
        public async Task BookingNotFound()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Detail(-1);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe("ErrorMessage");

            var model = viewResult.Model.ShouldBeOfType<ErrorMessageViewModel>();
            model.Message.ShouldBe(ErrorMessages.BookingNotFound);
        }
    }
}
