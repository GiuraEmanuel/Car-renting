using Car_Renting.Data;
using Microsoft.AspNetCore.Identity;
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

        public static (MockUserManager UserManager, User AdminUser, User User) SeedUsers(this AppDbContext context)
        {
            var userManager = MockUserManager.Create(context);
            var admin = userManager.AddUser("giura.emanuel@gmail.com", "Giura", "Emanuel", "222555666", true); // admin
            var user = userManager.AddUser("jason.bourne@gmail.com", "Jason", "Bourne", "555777999", false); // normal user;

            return (userManager, admin, user);
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

        //public static List<Booking> SeedBookings(this AppDbContext appDbContext)
        //{
        //    var bookings = new List<Booking> {
        //        new Booking("xa1",1, new DateTime(2021,11,27),new DateTime(2021,11,30),50),
        //        new Booking("2ac",2, new DateTime(2021,12,10),new DateTime(2021,12,15),20),
        //        new Booking("4ac",3, new DateTime(2021,12,16),new DateTime(2021,12,25),25),
        //        new Booking("66a",4, new DateTime(2021,12,20),new DateTime(2021,12,28),30)
        //    };

        //    foreach (var booking in bookings)
        //    {
        //        appDbContext.Bookings.Add(booking);
        //    }

        //    return bookings;
        //}
    }
}
