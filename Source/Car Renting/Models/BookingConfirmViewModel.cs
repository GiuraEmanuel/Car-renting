using System;

namespace Car_Renting.Models
{
    public class BookingConfirmViewModel
    {
        public int CarId { get; }
        public int Year { get;}
        public string Manufacturer { get; }
        public string Model { get; }
        public decimal PricePerDay { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public int TotalNumberOfDays => (EndDate - StartDate).Days;
        public decimal TotalCost => TotalNumberOfDays * PricePerDay;


        public BookingConfirmViewModel(int carId, int year, string manufacturer, string model, decimal pricePerDay,
            DateTime startDate, DateTime endDate)
        {
            CarId = carId;
            Year = year;
            Manufacturer = manufacturer;
            Model = model;
            PricePerDay = pricePerDay;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
