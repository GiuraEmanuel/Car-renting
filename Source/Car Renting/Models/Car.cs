using System;
using System.Collections.Generic;

namespace Car_Renting.Models
{

    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Manufacturer { get; set; }
        public DateTime? BookedUntilDate { get; set; }
        public List<Booking> Bookings { get; set; }
        public decimal PricePerDay { get; set; }
        public CarStatus Status { get; set; }

        public Car()
        {
                
        }

        public Car(int id, string name, string model, int year, string manufacturer)
        {
            Id = id;
            Name = name;
            Model = model;
            Year = year;
            Manufacturer = manufacturer;
        }

        public void RentCar(Car car, DateTime time)
        {

        }
    }
}
