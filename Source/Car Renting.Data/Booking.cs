using System;

namespace Car_Renting.Data
{
    public class Booking
    {
        public int Id { get; private set; }
        public string UserId { get; private set; }
        public User User { get; private set; } = null!;
        public int CarId { get; private set; }
        public Car Car { get; private set; } = null!;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public decimal TotalCost { get; private set; }
        public BookingStatus Status { get; set; }

        public Booking(string userId, int carId,
                DateTime startDate, DateTime endDate, decimal totalCost)
        {
            UserId = userId;
            CarId = carId;
            StartDate = startDate;
            EndDate = endDate;
            TotalCost = totalCost;
            Status = BookingStatus.Active;
        }
    }
}
