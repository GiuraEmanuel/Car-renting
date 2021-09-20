using System;

namespace Car_Renting.Models
{
    public class Car : ICar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public bool IsAvailable { get; set; }
        public int Year { get; set; }
        public string Manufacturer { get; set; }

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
