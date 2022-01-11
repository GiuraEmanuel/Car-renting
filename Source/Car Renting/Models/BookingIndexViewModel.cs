using Car_Renting.Data;
using System;
using System.Collections.Generic;

namespace Car_Renting.Models
{
    public class BookingIndexViewModel
    {
        public List<BookingInfo> CurrentBookings { get; } = new();
        public List<BookingInfo> PastBookings { get; } = new();
        public List<BookingInfo> UpcomingBookings { get; } = new();
        public bool IsAdmin { get; }

        public BookingIndexViewModel(IEnumerable<BookingInfo> bookings, bool isAdmin)
        {
            foreach (var booking in bookings)
            {
                if (booking.StartDate > DateTime.Today)
                {
                    UpcomingBookings.Add(booking);
                }
                else if (booking.EndDate < DateTime.Today)
                {
                    PastBookings.Add(booking);
                }
                else
                {
                    CurrentBookings.Add(booking);
                }
            }

            IsAdmin = isAdmin;
        }

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
}
