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

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Car>().HasData(new Car
            {
                Id = 1,
                Name = "Audi A6",
                Model = "A6",
                Manufacturer = "Audi",
                Year = 1994
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 2,
                Name = "BMW X6",
                Model = "X6",
                Manufacturer = "BMW",
                Year = 2008
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 3,
                Name = "Citroen A",
                Model = "A",
                Manufacturer = "Citroen",
                Year = 2020
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 4,
                Name = "Chevrolet Volt",
                Model = "Volt",
                Manufacturer = "Chevrolet",
                Year = 2011
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 5,
                Name = "Chrysler Sedan",
                Model = "300C Sedan",
                Manufacturer = "Chrysler",
                Year = 2004
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 6,
                Name = "Lamborghini Diablo",
                Model = "Diablo Coupe",
                Manufacturer = "Lamborghini",
                Year = 2000
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 7,
                Name = "Kia Optima",
                Model = "Optima Sedan",
                Manufacturer = "Kia",
                Year = 2018
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 8,
                Name = "Jaguar XK Coupe",
                Model = "XK Coupe",
                Manufacturer = "Jaguar",
                Year = 2011
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 9,
                Name = "Porsche Taycan",
                Model = "Taycan Hatchback",
                Manufacturer = "Porsche",
                Year = 2019
            });

            builder.Entity<Car>().HasData(new Car
            {
                Id = 10,
                Name = "Mitsubishi ASX SUV",
                Model = "ASX SUV",
                Manufacturer = "Mitsubishi",
                Year = 2019
            });
        }
    }
}
