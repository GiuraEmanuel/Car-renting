using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.Models
{
    public class BookingStartViewModel
    {
        /// <summary>
        /// If the value is null it means the user didn't enter the startDate and endDate yet therefore there is nothing to show.
        /// If the value is non-null but has 0 elements it means there are no available vehicles for the selected range and send the appropriate message to the user.
        /// </summary>
        public IList<Car>? Cars { get; }

        public DateTime? StartDate { get;}

        public DateTime? EndDate { get; }

        public string? ErrorMessage { get;}

        public BookingStartViewModel()
        {

        }

        public BookingStartViewModel(DateTime? startDate, DateTime? endDate, string errorMessage)
        {
            StartDate = startDate;
            EndDate = endDate;
            ErrorMessage = errorMessage;
        }

        /// <param name="cars">
        /// If the value is null it means the user didn't enter the startDate and endDate yet therefore there is nothing to show.
        /// If the value is non-null but has 0 elements it means there are no available vehicles for the selected range and send the appropriate message to the user.
        ///</param>
        public BookingStartViewModel(DateTime startDate, DateTime endDate,IEnumerable<Car> cars)
        {
            StartDate = startDate;
            EndDate = endDate;
            Cars = cars.ToList();
            if (Cars.Count == 0)
            {
                ErrorMessage = "There are no available cars in the specified date range.";
            }
        }


        public class Car
        {
            public int Id { get; }
            public int Year { get; }
            public string Manufacturer { get; }
            public string Model { get; }
            public string LicensePlate { get; }
            public decimal PricePerDay { get; }


            public Car(int id, int year, string manufacturer, string model, string licensePlate, decimal pricePerDay)
            {
                Id = id;
                Year = year;
                Manufacturer = manufacturer;
                Model = model;
                LicensePlate = licensePlate;
                PricePerDay = pricePerDay;
            }
        }
    }
}

