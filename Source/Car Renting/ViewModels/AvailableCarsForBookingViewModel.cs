using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.ViewModels
{
    public class AvailableCarsForBookingViewModel
    {
        public DateTime BookingStart { get; private set; }
        public DateTime BookingEnd { get; private set; }
    }
}
