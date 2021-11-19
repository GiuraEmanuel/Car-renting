using Car_Renting.Data;
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
        public int BookingId { get; }
        public decimal CancelRefundAmount { get;}

        public BookingDetailsViewModel(DateTime startDate, DateTime endDate,
            string manufacturer, string model, decimal totalCost,
            string firstName, string lastName, string email, string phoneNumber, int bookingId)
        {
            StartDate = startDate;
            EndDate = endDate;
            Manufacturer = manufacturer;
            Model = model;
            TotalCost = totalCost;
            Name = firstName + " " + lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            BookingId = bookingId;


            // Bookings for today cannot be cancelled, days can only be cancelled the day before, so only a partial refund is issued for the remaining days
            if (StartDate > DateTime.Today)
            {
                CancelRefundAmount = TotalCost;
            }
            else if (endDate <= DateTime.Today)
            {
                CancelRefundAmount = 0;
            }
            else
            {
                int penaltyDays = (DateTime.Today - StartDate).Days + 1;
                CancelRefundAmount = TotalCost * (TotalNumberOfDays - penaltyDays) / TotalNumberOfDays;
            }
        }
    }
}
