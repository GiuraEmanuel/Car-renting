using System;

namespace Car_Renting.Models
{
    public class BookingInfo
    {
        public int Id { get; }
        public string Email { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays => (EndDate - StartDate).Days;
        public int Year { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public decimal TotalCost { get; }

        public BookingInfo(int id, string email, DateTime startDate, DateTime endDate,
            int year, string manufacturer, string model, decimal totalCost)
        {
            Id = id;
            Email = email;
            StartDate = startDate;
            EndDate = endDate;
            Year = year;
            Manufacturer = manufacturer;
            Model = model;
            TotalCost = totalCost;
        }
    }
}
