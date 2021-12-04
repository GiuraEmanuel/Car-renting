using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests.BookingTests
{
    [TestClass]
    public class IndexTests
    {
        [TestMethod]
        public async Task AdminSeesAllBookings()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = admin;

            var cars = DbSetup.SeedCars(context);
            var adminBookings = DbSetup.SeedBookings(context, admin, cars[1]);
            var userBookings = DbSetup.SeedBookings(context, user, cars[0]);

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Index();


            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingIndexViewModel>();

            model.PastBookings
                 .Select(b => b.Id)
                 .ShouldBe(new[] { userBookings[0].Id, adminBookings[0].Id }, ignoreOrder: true);

            model.CurrentBookings
                 .Select(b => b.Id)
                 .ShouldBe(new[] { userBookings[1].Id, adminBookings[1].Id }, ignoreOrder: true);

            model.UpcomingBookings
                 .Select(b => b.Id)
                 .ShouldBe(new[] { userBookings[2].Id, adminBookings[2].Id }, ignoreOrder: true);

        }

        [TestMethod]
        public async Task UserSeesHisOwnBookings()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;

            var cars = DbSetup.SeedCars(context);
            var adminBookings = DbSetup.SeedBookings(context, admin, cars[1]);
            var userBookings = DbSetup.SeedBookings(context, user, cars[0]);

            var bookingController = new BookingController(context, userManager, null);

            // Act
            var result = await bookingController.Index();

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingIndexViewModel>();

            model.PastBookings
                 .Select(b => b.Id)
                 .ShouldBe(new[] { userBookings[0].Id });

            model.CurrentBookings
                 .Select(b => b.Id)
                 .ShouldBe(new[] { userBookings[1].Id });

            model.UpcomingBookings
                 .Select(b => b.Id)
                 .ShouldBe(new[] { userBookings[2].Id });
        }
    }
}
