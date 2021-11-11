using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.Models
{
    public class BookingConfirmPostModel
    {
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        // Not currently validated, just for demonstration purposes
        public string CCNumber { get; set; } = string.Empty;
        // Not currently validated, just for demonstration purposes
        public string ExpirationDate { get; set; } = string.Empty;
        // Not currently validated, just for demonstration purposes
        public string CCV { get; set; } = string.Empty;
    }
}
