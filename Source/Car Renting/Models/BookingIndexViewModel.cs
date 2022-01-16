using System;
using System.Collections.Generic;

namespace Car_Renting.Models
{
    public class BookingIndexViewModel
    {
        public List<BookingInfo> CurrentBookings { get; } = new();
        public List<BookingInfo> PastBookings { get; } = new();
        public List<BookingInfo> UpcomingBookings { get; } = new();
        public bool HasCancelledBookings { get; }
        public bool IsAdmin { get; }

        public BookingIndexViewModel(IEnumerable<BookingInfo> bookings, bool isAdmin, bool hasCancelledBookings)
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
            HasCancelledBookings = hasCancelledBookings;
        }
    }
}
