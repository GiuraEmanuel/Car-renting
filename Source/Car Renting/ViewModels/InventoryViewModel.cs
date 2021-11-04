using System.Collections.Generic;

namespace Car_Renting.ViewModels
{
    public class InventoryViewModel
    {

        public IEnumerable<Car> Cars { get; }

        public InventoryViewModel(IEnumerable<Car> cars)
        {
            Cars = cars;
        }

        public class Car
        {
            public int Id { get; }
            public int Year { get; }
            public string Manufacturer { get; }
            public string Model { get; }
            public string LicensePlate { get; }
            public decimal PricePerDay { get; }

            public Car(int id, int year, string manufacturer, string model, string licensePlate, decimal pricePerDay)
            {
                Id = id;
                Year = year;
                Manufacturer = manufacturer;
                Model = model;
                LicensePlate = licensePlate;
                PricePerDay = pricePerDay;
            }
        }
    }
}
