using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Car_Renting.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 1,
            //    Model = "A6",
            //    Manufacturer = "Audi",
            //    Year = 1994
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 2,
            //    Model = "X6",
            //    Manufacturer = "BMW",
            //    Year = 2008
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 3,
            //    Model = "A",
            //    Manufacturer = "Citroen",
            //    Year = 2020
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 4,
            //    Model = "Volt",
            //    Manufacturer = "Chevrolet",
            //    Year = 2011
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 5,
            //    Model = "300C Sedan",
            //    Manufacturer = "Chrysler",
            //    Year = 2004
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 6,
            //    Model = "Diablo Coupe",
            //    Manufacturer = "Lamborghini",
            //    Year = 2000
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 7,
            //    Model = "Optima Sedan",
            //    Manufacturer = "Kia",
            //    Year = 2018
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 8,
            //    Model = "XK Coupe",
            //    Manufacturer = "Jaguar",
            //    Year = 2011
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 9,
            //    Model = "Taycan Hatchback",
            //    Manufacturer = "Porsche",
            //    Year = 2019
            //});

            //builder.Entity<Car>().HasData(new Car
            //{
            //    Id = 10,
            //    Model = "ASX SUV",
            //    Manufacturer = "Mitsubishi",
            //    Year = 2019
            //});
        }
    }
}
