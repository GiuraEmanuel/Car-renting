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
        public BookingStatus Status { get; private set; }
        public DateTime? CancelDateTimeUtc { get; private set; }
        public decimal? CancelRefundAmount { get; private set; }

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



        public static decimal CalculateRefund(DateTime startDate, DateTime endDate, decimal totalCost)
        {
            // Bookings for today cannot be cancelled, days can only be cancelled the day before
            // so only a partial refund is issued for the remaining days
            if (startDate > DateTime.Today)
            {
                return totalCost;
            }
            else if (endDate <= DateTime.Today)
            {
                return 0;
            }
            else
            {
                int penaltyDays = (DateTime.Today - startDate).Days + 1;
                int totalNumberOfDays = (endDate - startDate).Days;
                return totalCost * (totalNumberOfDays - penaltyDays) / totalNumberOfDays;
            }
        }

        public void Cancel(decimal refundAmount)
        {
            if (Status != BookingStatus.Active)
            {
                throw new InvalidOperationException("Selected booking is already canceled.");
            }
            else if (refundAmount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(refundAmount), "Refund amount must be greater than 0");
            }

            Status = BookingStatus.Cancelled;
            CancelDateTimeUtc = DateTime.UtcNow;
            CancelRefundAmount = refundAmount;
        }
    }
}
