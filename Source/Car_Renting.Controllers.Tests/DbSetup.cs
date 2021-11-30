using Car_Renting.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Car_Renting.Controllers.Tests
{
    public static class DbSetup
    {
        public static AppDbContext Initialize()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\CarRentingTest;Database=Car-Renting-Test;Trusted_Connection=True;MultipleActiveResultSets=true");
            var dbContext = new AppDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        public static MockUserManager SeedUsers(this AppDbContext context)
        {
            var userManager = MockUserManager.Create(context);
            userManager.AddUser("giura.emanuel@gmail.com", "Giura", "Emanuel", "222555666", true); // admin
            userManager.AddUser("jason.bourne@gmail.com", "Jason", "Bourne", "555777999", false); // normal user;

            return userManager;
        }
        /// <summary>
        /// Adds a list of 4 cars to the database.
        /// </summary>
        /// <param name="appDbContext">Database context.</param>
        /// <returns> Returns a list of cars containing data that will have tests performed on. </returns>
        public static List<Car> SeedCars(this AppDbContext appDbContext)
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
                appDbContext.Cars.Add(car);
            }

            return cars;
        }
        
        /// <summary>
        /// <para>Adds bookings to both user and admin and returns them in a list in the following order:</para>
        /// <para>Admin booking: StartDate: 2021-11-24 -- EndDate: 2021-11-29 -- Cost: 50</para>
        /// <para>User past booking: StartDate: 2021-10-10 -- EndDate: 2021-10-29 -- Cost: 40</para>
        /// <para>User current booking: StartDate: today -- EndDate: tomorrow -- Cost: 35</para>
        /// <para>User future booking: StartDate: 15 days from today -- EndDate: 20 days from today -- Cost: 20</para>
        /// <para></para>
        /// </summary>
        /// <param name="userManager"> Gets users.</param>
        /// <param name="car"> Car details.</param>
        public static List<Booking> SeedBookings(this MockUserManager userManager, Car car)
        {
            var bookings = new List<Booking>
            {
                new Booking(userManager.AdminUser.Id,car.Id, new DateTime(2021,11,24), new DateTime(2021,11,29),50),
                new Booking(userManager.NormalUser.Id,car.Id, new DateTime(2021,10,10), new DateTime(2021,10,29),40),
                new Booking(userManager.NormalUser.Id,car.Id, DateTime.Today, DateTime.Today.AddDays(1),35),
                new Booking(userManager.NormalUser.Id,car.Id, DateTime.Today.AddDays(15), DateTime.Today.AddDays(20),20),
            };

            return bookings;
        }
    }
}
