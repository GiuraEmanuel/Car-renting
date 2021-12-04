using Car_Renting.Data;
using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests.BookingTests
{
    [TestClass]
    public class StartTests
    {
        [TestMethod]
        public async Task BothDatesAreValid()
        {
            // Arrange
            using var context = DbSetup.Initialize();

            var cars = DbSetup.SeedCars(context);

            var bookingController = new BookingController(context, null, null);

            // Act
            var result = await bookingController.Start(DateTime.Today, DateTime.Today.AddDays(5));

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(DateTime.Today);
            model.EndDate.ShouldBe(DateTime.Today.AddDays(5));
            model.Cars.ShouldNotBeNull();
            model.Cars.Count.ShouldBe(cars.Count);
        }

        [TestMethod]
        public async Task DoesNotShowDeletedCars()
        {
            // Arrange
            using var context = DbSetup.Initialize();

            var cars = DbSetup.SeedCars(context);

            foreach (var car in cars)
            {
                car.Status = CarStatus.Deleted;
            }

            context.SaveChanges();
            var bookingController = new BookingController(context, null, null);

            // Act
            var result = await bookingController.Start(DateTime.Today, DateTime.Today.AddDays(3));

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(DateTime.Today);
            model.EndDate.ShouldBe(DateTime.Today.AddDays(3));
            model.Cars.ShouldNotBeNull();
            model.Cars.Count.ShouldBe(0);
        }

        [TestMethod]
        public async Task DoesNotShowBookedCars()
        {
            // Arrange
            using var context = DbSetup.Initialize();
            var userManager = MockUserManager.Create(context);
            var (admin, user) = DbSetup.SeedUsers(userManager);
            userManager.CurrentUser = user;

            var cars = DbSetup.SeedCars(context);

            var searchStartDate = DateTime.Today.AddDays(20);
            var searchEndDate = DateTime.Today.AddDays(25);

            // Add a past booking to all the cars
            foreach (var car in cars)
            {
                context.Bookings.Add(new Booking(user.Id, car.Id, new DateTime(2021, 10, 10), new DateTime(2021, 10, 12), 50));
            }

            // Add a future booking to all the cars that are outside of the search range
            foreach (var car in cars)
            {
                context.Bookings.Add(new Booking(user.Id, car.Id, searchStartDate.AddDays(6), searchEndDate.AddDays(11), 60));
            }

            // Add a future booking to the last car that overlaps the search range
            context.Bookings.Add(new Booking(user.Id, cars[3].Id, searchStartDate.AddDays(1), searchEndDate.AddDays(3), 80));

            context.SaveChanges();
            var bookingController = new BookingController(context, null, null);

            // Act
            var result = await bookingController.Start(searchStartDate, searchEndDate);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(DateTime.Today.AddDays(20));
            model.EndDate.ShouldBe(DateTime.Today.AddDays(25));
            model.Cars.ShouldNotBeNull();
            model.Cars.Select(c => c.Model).ShouldBe(new[] { cars[0].Model, cars[1].Model, cars[2].Model }, ignoreOrder: true);
        }

        [TestMethod]
        public async Task NoDatesEnteredYet()
        {
            // Arrange
            var bookingController = new BookingController(null, null, null);

            // Act
            var result = await bookingController.Start(null, null);

            // Assert

            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(null);
            model.EndDate.ShouldBe(null);
        }

        [TestMethod]
        public async Task StartDateIsNullEndDateIsNot()
        {
            // Arrange
            var bookingController = new BookingController(null, null, null);

            // Act
            var result = await bookingController.Start(null, DateTime.Today);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(null);
            model.EndDate.ShouldBe(DateTime.Today);
            model.ErrorMessage.ShouldBe(ErrorMessages.EmptyStartDate);
        }

        [TestMethod]
        public async Task EndDateIsNullStartDateIsNot()
        {
            // Arrange
            var bookingController = new BookingController(null, null, null);

            // Act
            var result = await bookingController.Start(DateTime.Today, null);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(DateTime.Today);
            model.EndDate.ShouldBe(null);
            model.ErrorMessage.ShouldBe(ErrorMessages.EmptyEndDate);
        }

        [TestMethod]
        public async Task ThrowsIfDatesContainTime()
        {
            // Arrange
            var bookingController = new BookingController(null, null, null);
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(4);
            // Act/Assert
            await Should.ThrowAsync<InvalidDataException>(() => bookingController.Start(startDate.AddHours(1), endDate));
            await Should.ThrowAsync<InvalidDataException>(() => bookingController.Start(startDate, endDate.AddHours(1)));
        }

        [TestMethod]
        public async Task StartDateIsLessThanToday()
        {
            // Arrange
            var bookingController = new BookingController(null, null, null);

            // Act
            var result = await bookingController.Start(DateTime.Today.AddDays(-3), DateTime.Today);

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(DateTime.Today.AddDays(-3));
            model.EndDate.ShouldBe(DateTime.Today);
            model.ErrorMessage.ShouldBe(ErrorMessages.StartDateLessThanToday);
        }

        [TestMethod]
        public async Task EndDateIsLessThanStartDate()
        {
            // Arrange
            var bookingController = new BookingController(null, null, null);

            // Act
            var result = await bookingController.Start(DateTime.Today, DateTime.Today.AddDays(-1));

            // Assert
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(DateTime.Today);
            model.EndDate.ShouldBe(DateTime.Today.AddDays(-1));
            model.ErrorMessage.ShouldBe(ErrorMessages.EndDateLessThanStartDate);
        }

        [TestMethod]
        public async Task StartDateAndEndDateAreEqual()
        {
            // Arrange
            var bookingController = new BookingController(null, null, null);

            // Act
            var result = await bookingController.Start(DateTime.Today, DateTime.Today);

            // Assert

            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.ViewName.ShouldBe(null);

            var model = viewResult.Model.ShouldBeOfType<BookingStartViewModel>();
            model.StartDate.ShouldBe(DateTime.Today);
            model.EndDate.ShouldBe(DateTime.Today.AddDays(1));
            model.ErrorMessage.ShouldBe(ErrorMessages.StartDateAndEndDateAreEqual);
        }
    }
}
