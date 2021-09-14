using System;

namespace Car_Renting.Models
{
    public class Car : ICar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }

        public Car(int id, string name, string model, string manufacturer, int year)
        {
            Id = id;
            Name = name;
            Model = model;
            Manufacturer = manufacturer;
            Year = year;
        }

        public void RentCar(Car car, DateTime time)
        {

        }
    }
}
