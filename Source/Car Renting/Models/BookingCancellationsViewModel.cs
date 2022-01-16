using System.Collections.Generic;

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
