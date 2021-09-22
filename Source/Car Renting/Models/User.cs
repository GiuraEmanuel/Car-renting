using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Car_Renting.Models
{
    public class User : IdentityUser
    {
        public List<Booking> Bookings { get; set; }
    }
}