using Car_Renting.Data;
using Car_Renting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Car_Renting.Models.BookingIndexViewModel;

namespace Car_Renting.Controllers
{
    [Authorize]
    [Route("Bookings")]
    public class BookingController : Controller
    {
        const string StartBookingAgainMessage = " Please go back and start your booking again. We apologize for the inconvenience.";

        private readonly AppDbContext _appDbContext;

        private readonly UserManager<User> _userManager;

        public BookingController(AppDbContext appDbContext, UserManager<User> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            IQueryable<Booking> bookingsQuery = _appDbContext.Bookings;

            if (!await CheckIfAdmin(user))
            {
                bookingsQuery = bookingsQuery.Where(b => b.UserId == user.Id);
            }

            List<BookingInfo> bookings = await bookingsQuery
                .Select(b => new BookingInfo(b.Id, b.User.Email, b.StartDate, b.EndDate, b.Car.Model, b.Car.Manufacturer, b.TotalCost))
                .ToListAsync();

            var model = new BookingIndexViewModel(bookings, await CheckIfAdmin(user));
            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var user = await GetCurrentUserAsync();

            var bookingInfo = await _appDbContext.Bookings
                .Where(b => b.Id == id)
                .Select(booking => new {
                    UserId = booking.UserId,
                    VM = new BookingDetailsViewModel(booking.StartDate, booking.EndDate, booking.Car.Manufacturer, booking.Car.Model,
                     booking.TotalCost, booking.User.FirstName, booking.User.LastName, booking.User.Email, booking.User.PhoneNumber, booking.Id)
                }).SingleOrDefaultAsync();

            if (user.Id == bookingInfo.UserId || await CheckIfAdmin(user))
            {
                return View(bookingInfo.VM);
            }
            return View("ErrorMessage", new ErrorMessageViewModel("You are not allowed to see the details of this booking."));
        }

        [HttpGet("Start")]
        public async Task<IActionResult> Start(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate == null)
            {
                return View(new BookingStartViewModel());
            }
            else if (!TryValidateDateRange(startDate, endDate, out string? error))
            {
                if (startDate == endDate)
                {
                    endDate = startDate!.Value.AddDays(1);
                }

                var errorModel = new BookingStartViewModel(startDate, endDate, error);
                return View(errorModel);
            }

            var cars = await _appDbContext.Cars
                .Where(car => car.Status == CarStatus.Active && car.Bookings.All(b => startDate >= b.EndDate || endDate <= b.StartDate))
                .Select(car => new BookingStartViewModel.Car(car.Id, car.Year, car.Manufacturer, car.Model, car.LicensePlate,
                 car.PricePerDay)).ToListAsync();
            var startViewModel = new BookingStartViewModel(startDate.Value, endDate.Value, cars);
            return View(startViewModel);
        }

        [HttpGet("Confirm")]
        public async Task<IActionResult> Confirm(int carId, DateTime startDate, DateTime endDate)
        {
            if (!TryValidateDateRange(startDate, endDate, out string? error))
            {
                var errorModel = new ErrorMessageViewModel(error + StartBookingAgainMessage);
                return View("ErrorMessage", errorModel);
            }

            Car car = await GetActiveAndAvailableCar(carId, startDate, endDate);

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel("Car is no longer available." + StartBookingAgainMessage));
            }

            var bookingConfirmVM = new BookingConfirmViewModel(car.Id, car.Manufacturer, car.Model, car.PricePerDay, startDate, endDate);
            return View(bookingConfirmVM);
        }

        [HttpPost("Confirm")]
        public async Task<IActionResult> Confirm(BookingConfirmPostModel bookingConfirmPostModel)
        {
            if (!TryValidateDateRange(bookingConfirmPostModel.StartDate, bookingConfirmPostModel.EndDate, out string? error))
            {
                var errorModel = new ErrorMessageViewModel(error + StartBookingAgainMessage);
                return View("ErrorMessage", errorModel);
            }

            var car = await GetActiveAndAvailableCar(bookingConfirmPostModel.CarId, bookingConfirmPostModel.StartDate, bookingConfirmPostModel.EndDate);

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel("Car is no longer available." + StartBookingAgainMessage));
            }

            if (bookingConfirmPostModel.TotalCost != bookingConfirmPostModel.TotalNumberOfDays * car.PricePerDay)
            {
                return View("ErrorMessage", new ErrorMessageViewModel("The price of the vehicle you are booking has changed." +
                    StartBookingAgainMessage));
            }
            var userId = _userManager.GetUserId(HttpContext.User);
            // [Credit card would be processed here in a real application]
            var booking = new Booking(userId, bookingConfirmPostModel.CarId,
                    bookingConfirmPostModel.StartDate, bookingConfirmPostModel.EndDate, bookingConfirmPostModel.TotalCost);

            await _appDbContext.Bookings.AddAsync(booking);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = booking.Id });
        }

        private async Task<Car> GetActiveAndAvailableCar(int carId, DateTime startDate, DateTime endDate)
        {
            return await _appDbContext.Cars
                .SingleOrDefaultAsync(c => c.Id == carId &&
                    c.Status == CarStatus.Active &&
                    c.Bookings.All(b => startDate >= b.EndDate || endDate <= b.StartDate));
        }

        private static bool TryValidateDateRange([NotNullWhen(true)] DateTime? startDate, [NotNullWhen(true)] DateTime? endDate, [NotNullWhen(false)] out string? error)
        {
            if (startDate == null)
            {
                error = "Start date can't be empty.";
            }
            else if (endDate == null)
            {
                error = "End date can't be empty.";
            }
            else if (startDate.Value.TimeOfDay != TimeSpan.Zero || endDate.Value.TimeOfDay != TimeSpan.Zero)
            {
                throw new InvalidDataException("Unexpected time component in date.");
            }
            else if (startDate < DateTime.Today)
            {
                error = "Start date can't preceed the current day.";
            }
            else if (endDate < startDate)
            {
                error = "End date must follow start date.";
            }
            else if (startDate == endDate)
            {
                error = "Minimum booking length is 1 day.";
            }
            else
            {
                error = null;
                return true;
            }

            return false;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private Task<bool> CheckIfAdmin(User user) => _userManager.IsInRoleAsync(user, "Admin");
    }
}
