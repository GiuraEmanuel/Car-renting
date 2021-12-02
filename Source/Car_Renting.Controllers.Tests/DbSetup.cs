using Car_Renting.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Car_Renting.Controllers.Tests
{
    public static class DbSetup
    {
        /// <summary>
        /// Initializes the database by deleting the existing database and recreating it.
        /// </summary>
        public static AppDbContext Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\CarRentingTest;Database=Car-Renting-Test;Trusted_Connection=True;MultipleActiveResultSets=true");
            var dbContext = new AppDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        /// <summary>
        /// Adds and admin and normal user to the database.
        /// </summary>
        public static (User Admin, User User) SeedUsers(MockUserManager userManager)
        {
            var admin = userManager.AddUser("giura.emanuel@gmail.com", "Giura", "Emanuel", "222555666", true); // admin
            var user = userManager.AddUser("jason.bourne@gmail.com", "Jason", "Bourne", "555777999", false); // normal user;

            userManager.Context.SaveChanges();
            return (admin, user);
        }

        /// <summary>
        /// Adds a list of 4 cars to the database. All the cars have unique models.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <returns> Returns a list of 4 cars containing data that will have tests performed on. </returns>
        public static List<Car> SeedCars(AppDbContext context)
        {
            var cars = new List<Car>
            {
                new Car(2021,"Audi","X7",25,"22-SB-AZ"),
                new Car(2021,"Audi","A6",30,"99-XX-89"),
                new Car(2021,"BMW","A7",45,"AA-20-XX"),
                new Car(2021,"Honda","A8",50,"15-BS-44")
            };

            foreach (var car in cars)
            {
                context.Cars.Add(car);
            }

            context.SaveChanges();

            return cars;
        }

        /// <summary>
        /// <para>Adds bookings to the provided user and returns them in a list in the following order:</para>
        /// <para>Past booking: StartDate: 2021-10-10 -- EndDate: 2021-10-29 -- Total Cost: 40</para>
        /// <para>Current booking: StartDate: today -- EndDate: 3 days from today -- Total Cost: 60</para>
        /// <para>Future booking: StartDate: 15 days from today -- EndDate: 20 days from today -- Total Cost: 20</para>
        /// </summary>
        /// <param name="context">The db context.</param>
        /// <param name="userManager">The user to add bookings for.</param>
        /// <param name="car">The car to book.</param>
        public static List<Booking> SeedBookings(AppDbContext context, User user, Car car)
        {
            var bookings = new List<Booking>
            {
                new Booking(user.Id, car.Id, new DateTime(2021,10,10), new DateTime(2021,10,29),40),
                new Booking(user.Id, car.Id, DateTime.Today, DateTime.Today.AddDays(3),60),
                new Booking(user.Id, car.Id, DateTime.Today.AddDays(15), DateTime.Today.AddDays(20),20),
            };

            foreach (var booking in bookings)
            {
                context.Bookings.Add(booking);
            }

            context.SaveChanges();
            return bookings;
        }
    }
}
