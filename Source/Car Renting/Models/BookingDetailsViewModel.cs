using System;
namespace Car_Renting.Models
{
    public class BookingDetailsViewModel
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public int TotalNumberOfDays => (EndDate - StartDate).Days;
        public decimal TotalCost { get; }
        public string Name { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string BookingNumber { get; }


        public BookingDetailsViewModel(DateTime startDate, DateTime endDate,
            string manufacturer, string model, decimal totalCost, string name, string email, string phoneNumber, string bookingNumber)
        {
            StartDate = startDate;
            EndDate = endDate;
            Manufacturer = manufacturer;
            Model = model;
            TotalCost = totalCost;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            BookingNumber = bookingNumber;
        }
    }
}
