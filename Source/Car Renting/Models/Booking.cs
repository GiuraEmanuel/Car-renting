﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Car_Renting.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string UserId { get; set; }
        public int CarId { get; set; }
        public DateTime BookingStart { get; set; }
        public DateTime BookingEnd { get; set; }
        public Car Car { get; set; }
        public User User { get; set; }
        public decimal TotalCost { get; set; }
    }
}
