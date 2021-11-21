using Car_Renting.Data;
using System;
namespace Car_Renting.Models
{
    public class BookingDetailsViewModel
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public int TotalNumberOfDays => (EndDate - StartDate).Days;
        public decimal TotalCost { get; }
        public string Name { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public int BookingId { get; }
        /// <summary>
        /// The amount that that will be refunded if the user cancels the booking
        /// </summary>
        public decimal RefundAmountIfCancelling { get; }
        public DateTime? CancelDateTimeLocal { get; }
        /// <summary>
        /// The amount that was already refunded
        /// </summary>
        public decimal? CancelRefundAmount { get; }

        public BookingDetailsViewModel(int bookingId, DateTime startDate, DateTime endDate,
            string manufacturer, string model, decimal totalCost,
            string firstName, string lastName, string email, string phoneNumber, DateTime? cancelDateTimeUtc, decimal? cancelRefundAmount)
        {
            BookingId = bookingId;
            StartDate = startDate;
            EndDate = endDate;
            Manufacturer = manufacturer;
            Model = model;
            TotalCost = totalCost;
            Name = firstName + " " + lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            CancelDateTimeLocal = cancelDateTimeUtc?.ToLocalTime();
            CancelRefundAmount = cancelRefundAmount;

            if (CancelRefundAmount != null)
            {
                RefundAmountIfCancelling = 0;
            }
            else
            {
                RefundAmountIfCancelling = Booking.CalculateRefund(startDate, endDate, totalCost);
            }
        }


    }
}
