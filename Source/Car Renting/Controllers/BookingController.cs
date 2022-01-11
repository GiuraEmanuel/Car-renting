using Car_Renting.Data;
using Car_Renting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
        

        private readonly AppDbContext _appDbContext;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public BookingController(AppDbContext appDbContext, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            var isAdmin = await CheckIfAdmin(user);

            var bookings = await GetBookingInfosAsync(user.Id, isAdmin, BookingStatus.Active);

            var model = new BookingIndexViewModel(bookings, isAdmin);
            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var user = await GetCurrentUserAsync();

            var bookingInfo = await _appDbContext.Bookings
                .Where(b => b.Id == id)
                .Select(booking => new
                {
                    UserId = booking.UserId,
                    VM = new BookingDetailsViewModel(booking.Id, booking.StartDate, booking.EndDate, booking.Car.Year, booking.Car.Manufacturer, booking.Car.Model,
                     booking.TotalCost, booking.User.FirstName, booking.User.LastName, booking.User.Email, booking.User.PhoneNumber,
                     booking.CancelDateTimeUtc, booking.CancelRefundAmount)
                }).SingleOrDefaultAsync();

            if (bookingInfo == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel(ErrorMessages.BookingNotFound));
            }

            if (user.Id == bookingInfo.UserId || await CheckIfAdmin(user))
            {
                return View(bookingInfo.VM);
            }
            return View("ErrorMessage", new ErrorMessageViewModel(ErrorMessages.BookingAccessDenied));
        }

        [AllowAnonymous]
        [HttpGet("Start")]
        public async Task<IActionResult> Start(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate == null)
            {
                return View(new BookingStartViewModel(DateTime.Today, DateTime.Today.AddDays(1)));
            }
            else if (!TryValidateDateRange(startDate, endDate, out string? error))
            {
                if (startDate == endDate)
                {
                    endDate = startDate!.Value.AddDays(1);
                }

                var errorModel = new BookingStartViewModel(startDate ?? DateTime.Today, endDate ?? DateTime.Today.AddDays(1), error);
                return View(errorModel);
            }

            var cars = await _appDbContext.Cars
                .Where(car => car.Status == CarStatus.Active && car.Bookings.All(b => startDate >= b.EndDate || endDate <= b.StartDate))
                .Select(car => new BookingStartViewModel.Car(car.Id, car.Year, car.Manufacturer, car.Model, car.LicensePlate,
                 car.PricePerDay)).ToListAsync();
            var startViewModel = new BookingStartViewModel(startDate.Value, endDate.Value, cars);
            return View(startViewModel);
        }

        [AllowAnonymous]
        [HttpGet("Confirm")]
        public async Task<IActionResult> Confirm(int carId, DateTime startDate, DateTime endDate)
        {
            if (!TryValidateDateRange(startDate, endDate, out string? error))
            {
                var errorModel = new ErrorMessageViewModel(error + ErrorMessages.StartBookingAgainSuffix);
                return View("ErrorMessage", errorModel);
            }

            if (!_signInManager.IsSignedIn(User))
            {
                string url = Url.Action("Confirm", new { carId, startDate, endDate });
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = url });
            }

            Car car = await GetActiveAndAvailableCar(carId, startDate, endDate);

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel(ErrorMessages.CarUnavailable + ErrorMessages.StartBookingAgainSuffix));
            }

            var bookingConfirmVM = new BookingConfirmViewModel(car.Id, car.Year, car.Manufacturer, car.Model, car.PricePerDay, startDate, endDate);
            return View(bookingConfirmVM);
        }

        [HttpPost("Confirm")]
        public async Task<IActionResult> Confirm(BookingConfirmPostModel bookingConfirmPostModel)
        {
            if (!TryValidateDateRange(bookingConfirmPostModel.StartDate, bookingConfirmPostModel.EndDate, out string? error))
            {
                var errorModel = new ErrorMessageViewModel(error + ErrorMessages.StartBookingAgainSuffix);
                return View("ErrorMessage", errorModel);
            }

            var car = await GetActiveAndAvailableCar(bookingConfirmPostModel.CarId, bookingConfirmPostModel.StartDate, bookingConfirmPostModel.EndDate);

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel(ErrorMessages.CarUnavailable + ErrorMessages.StartBookingAgainSuffix));
            }

            if (bookingConfirmPostModel.TotalCost != bookingConfirmPostModel.TotalNumberOfDays * car.PricePerDay)
            {
                return View("ErrorMessage", new ErrorMessageViewModel(ErrorMessages.CarPriceChanged +
                    ErrorMessages.StartBookingAgainSuffix));
            }
            var userId = _userManager.GetUserId(User);
            // [Credit card would be processed here in a real application]
            var booking = new Booking(userId, bookingConfirmPostModel.CarId,
                    bookingConfirmPostModel.StartDate, bookingConfirmPostModel.EndDate, bookingConfirmPostModel.TotalCost);

            _appDbContext.Bookings.Add(booking);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Detail), new { id = booking.Id });
        }

        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel(BookingCancelPostModel cancelPostModel)
        {
            var booking = await _appDbContext.Bookings.SingleOrDefaultAsync(b => b.Id == cancelPostModel.Id);

            if (booking == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel(ErrorMessages.BookingNotFound));
            }

            if (booking.Status == BookingStatus.Cancelled)
            {
                return View("ErrorMessage", new ErrorMessageViewModel(ErrorMessages.BookingAlreadyCancelled));
            }

            var refundAmount = Booking.CalculateRefund(booking.StartDate, booking.EndDate, booking.TotalCost);

            if (refundAmount != cancelPostModel.RefundAmount)
            {
                return View("ErrorMessage", new ErrorMessageViewModel(ErrorMessages.RefundAmountChanged));
            }

            booking.Cancel(refundAmount);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Detail), new { id = booking.Id });
        }

        [HttpGet("Cancellations")]
        public async Task<IActionResult> Cancellations()
        {
            var user = await GetCurrentUserAsync();

            var isAdmin = await CheckIfAdmin(user);

            var bookings = await GetBookingInfosAsync(user.Id, isAdmin, BookingStatus.Cancelled);

            var model = new BookingCancellationsViewModel(bookings, isAdmin);
            return View(model);
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
                error = ErrorMessages.EmptyStartDate;
            }
            else if (endDate == null)
            {
                error = ErrorMessages.EmptyEndDate;
            }
            else if (startDate.Value.TimeOfDay != TimeSpan.Zero || endDate.Value.TimeOfDay != TimeSpan.Zero)
            {
                // this indicates a bug in our program so throw instead of returning a user error message
                throw new InvalidDataException("Unexpected time component in date.");
            }
            else if (startDate < DateTime.Today)
            {
                error = ErrorMessages.StartDateLessThanToday;
            }
            else if (endDate < startDate)
            {
                error = ErrorMessages.EndDateLessThanStartDate;
            }
            else if (startDate == endDate)
            {
                error = ErrorMessages.StartDateAndEndDateAreEqual;
            }
            else
            {
                error = null;
                return true;
            }
            return false;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(User);

        private Task<bool> CheckIfAdmin(User user) => _userManager.IsInRoleAsync(user, "Admin");

        private async Task<List<BookingInfo>> GetBookingInfosAsync(string userId, bool isAdmin, BookingStatus bookingStatus)
        {
            IQueryable<Booking> bookingsQuery = _appDbContext.Bookings;

            if (!isAdmin)
            {
                bookingsQuery = bookingsQuery.Where(b => b.UserId == userId);
            }

            return await bookingsQuery
                .Where(b => b.Status == bookingStatus)
                .Select(b => new BookingInfo(b.Id, b.User.Email, b.StartDate, b.EndDate, b.Car.Model, b.Car.Manufacturer, b.TotalCost))
                .ToListAsync();
        }
    }
}
