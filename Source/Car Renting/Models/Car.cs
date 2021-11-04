using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Car_Renting.Models
{
    [Index(nameof(LicensePlate), IsUnique = true)]
    public class Car
    {
        public int Id { get; private set; }
        public int Year { get; private set; }
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public decimal PricePerDay { get; set; }
        public string LicensePlate { get; set; }
        public CarStatus Status { get; set; }
        public List<Booking> Bookings { get; } = new();

        public Car(int year, string manufacturer, string model, decimal pricePerDay, string licensePlate)
        {
            Year = year;
            Manufacturer = manufacturer;
            Model = model;
            PricePerDay = pricePerDay;
            LicensePlate = licensePlate;
            Status = CarStatus.Active;
        }
    }
}
