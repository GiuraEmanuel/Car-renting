using System;

namespace Car_Renting.Models
{
    public class BookingConfirmViewModel
    {
        public int CarId { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public decimal PricePerDay { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public int TotalNumberOfDays => (EndDate - StartDate).Days;
        public decimal TotalPrice => TotalNumberOfDays * PricePerDay;


        public BookingConfirmViewModel(int carId, string manufacturer, string model, decimal pricePerDay,
            DateTime startDate, DateTime endDate)
        {
            CarId = carId;
            Manufacturer = manufacturer;
            Model = model;
            PricePerDay = pricePerDay;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
