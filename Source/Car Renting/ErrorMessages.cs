namespace Car_Renting
{
    public static class ErrorMessages
    {
        // Booking controller error messages
        public const string StartBookingAgainSuffix = " Please go back and start your booking again. We apologize for the inconvenience.";
        public const string BookingAccessDenied = "You are not allowed to see the details of this booking.";
        public const string EmptyStartDate = "Start date can't be empty.";
        public const string EmptyEndDate = "End date can't be empty.";
        public const string StartDateLessThanToday = "Start date can't preceed the current day.";
        public const string EndDateLessThanStartDate = "End date must follow start date.";
        public const string StartDateAndEndDateAreEqual = "Minimum booking length is 1 day.";
        public const string BookingNotFound = "Booking not found.";
        public const string BookingAlreadyCancelled = "Booking has already been canceled.";
        public const string RefundAmountChanged = "Refund amount has changed, please go back and cancel your booking again.";
        public const string CarUnavailable = "Car is no longer available.";
        public const string CarPriceChanged = "The price of the car you are booking has changed.";

        // Admin controller error messages
        public const string SameLicensePlate = "A car with the same license plate already exists.";
        public const string CarDoesNotExist = "Car does not exist.";
    }
}
