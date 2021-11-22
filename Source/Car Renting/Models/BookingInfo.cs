using System;

namespace Car_Renting.Models
{
    public partial class BookingIndexViewModel
    {
        public class BookingInfo
        {
            public int Id { get; }
            public string Email { get; }
            public DateTime StartDate { get; }
            public DateTime EndDate { get; set; }
            public int NumberOfDays => (EndDate - StartDate).Days;
            public string Model { get; }
            public string Manufacturer { get; }
            public decimal TotalCost { get; }

            public BookingInfo(int id, string email, DateTime startDate, DateTime endDate, string model, string manufacturer, decimal totalCost)
            {
                Id = id;
                Email = email;
                StartDate = startDate;
                EndDate = endDate;
                Model = model;
                Manufacturer = manufacturer;
                TotalCost = totalCost;
            }
        }
    }
}
