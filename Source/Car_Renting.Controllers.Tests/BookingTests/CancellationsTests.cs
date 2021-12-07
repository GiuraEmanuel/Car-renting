using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests.BookingTests
{
    [TestClass]
    public class CancellationsTests
    {
        [TestMethod]
        public async Task AdminCanSeeAllCancelledBookings()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = admin;
            var signInManager = MockSignInManager.Create(userManager);

            var cars = DbSetup.SeedCars(context);
            var adminBookings = DbSetup.SeedBookings(context, admin, cars[1]);
            var userBookings = DbSetup.SeedBookings(context, user, cars[0]);

            var bookingController = new BookingController(context, userManager, signInManager);

            adminBookings[1].Cancel(60);
            userBookings[2].Cancel(20);
            context.SaveChanges();

            // Act
            var result = await bookingController.Cancellations();

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);
            var model = viewResult.Model.ShouldBeOfType<BookingCancellationsViewModel>();

            model.Bookings
                 .Select(b => b.Id)
                 .ShouldBe(new[] { userBookings[2].Id, adminBookings[1].Id }, ignoreOrder: true);
        }

        [TestMethod]
        public async Task UserCanOnlySeeHisOwnBookings()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;
            var signInManager = MockSignInManager.Create(userManager);

            var cars = DbSetup.SeedCars(context);
            var adminBookings = DbSetup.SeedBookings(context, admin, cars[1]);
            var userBookings = DbSetup.SeedBookings(context, user, cars[0]);

            var bookingController = new BookingController(context, userManager, signInManager);

            adminBookings[1].Cancel(60);
            userBookings[2].Cancel(20);
            context.SaveChanges();

            // Act
            var result = await bookingController.Cancellations();

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);
            var model = viewResult.Model.ShouldBeOfType<BookingCancellationsViewModel>();

            model.Bookings
                 .Select(b => b.Id)
                 .ShouldBe(new[] { userBookings[2].Id});
        }
    }
}
