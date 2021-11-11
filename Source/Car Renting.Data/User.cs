using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Car_Renting.Data
{
    public class User : IdentityUser
    {
        public List<Booking> Bookings { get; } = new();
    }
}