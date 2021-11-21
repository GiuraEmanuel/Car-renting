using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.Models
{
    public class BookingCancelPostModel
    {
        public int Id { get; set; }
        public decimal RefundAmount { get; set; }
    }
}
