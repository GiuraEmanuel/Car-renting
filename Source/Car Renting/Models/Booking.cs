﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Car_Renting.Models
{
    public class Booking
    {
        public int BookingId { get; private set; }
        public string UserId { get; private set; }
        public User User { get; private set; } = null!;
        public int CarId { get; private set; }
        public Car Car { get; private set; } = null!;
        public DateTime BookingStart { get; private set; }
        public DateTime BookingEnd { get; private set; }
        public decimal TotalCost { get; private set; }
        public BookingStatus Status { get; set; }

        public Booking(int bookingId, string userId, int carId, DateTime bookingStart, DateTime bookingEnd, decimal totalCost)
        {
            BookingId = bookingId;
            UserId = userId;
            CarId = carId;
            BookingStart = bookingStart;
            BookingEnd = bookingEnd;
            TotalCost = totalCost;
            Status = BookingStatus.Active;
        }
    }
}
