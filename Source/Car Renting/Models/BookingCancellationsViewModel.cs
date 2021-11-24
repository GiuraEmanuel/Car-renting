using System;
using System.Collections.Generic;
using static Car_Renting.Models.BookingIndexViewModel;

namespace Car_Renting.Models
{
    public class BookingCancellationsViewModel
    {

        public List<BookingInfo> Bookings { get; }

        public bool IsAdmin { get;}

        public BookingCancellationsViewModel(List<BookingInfo> bookings, bool isAdmin)
        {
            Bookings = bookings;
            IsAdmin = isAdmin;
        }
    }
}
