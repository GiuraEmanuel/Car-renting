using Car_Renting.Models;
using Car_Renting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.Controllers
{
    [Authorize]
    [Route("Bookings")]
    public class BookingController : Controller
    {

        private readonly AppDbContext _appDbContext;

        private readonly UserManager<User> _userManager;

        public BookingController(AppDbContext appDbContext, UserManager<User> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            return Ok("Good.");
        }

        [HttpGet("Start")]
        public async Task<IActionResult> Start(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate == null)
            {
                return View(new BookingStartViewModel());
            }
            else
            {
                string? error = null;
                if (startDate == null)
                {
                    error = "Start date can't be empty.";
                }
                else if (endDate == null)
                {
                    error = "End date can't be empty.";
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
                    endDate = startDate.Value.AddDays(1);
                }

                if (error != null)
                {
                    var errorModel = new BookingStartViewModel(startDate, endDate, error);
                    return View(errorModel);
                }
            }

            var cars = await _appDbContext.Cars
                .Where(car => car.Status == CarStatus.Active)
                .Select(car => new BookingStartViewModel.Car(car.Id, car.Year, car.Manufacturer, car.Model, car.LicensePlate,
                 car.PricePerDay)).ToListAsync();
            var startViewModel = new BookingStartViewModel(startDate!.Value, endDate!.Value, cars);
            return View(startViewModel);
        }

        [HttpGet("Confirm")]
        public async Task<IActionResult> Confirm(int carId, DateTime startDate, DateTime endDate)
        {
            var car = await _appDbContext.Cars.Where(c => c.Id == carId).SingleOrDefaultAsync();

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel("Car does not exist"));
            }

            var bookingConfirmVM = new BookingConfirmViewModel(car.Id, car.Manufacturer, car.Model, car.PricePerDay, startDate, endDate);
            return View(bookingConfirmVM);
        }

        [HttpPost("Confirm")]
        public async Task<IActionResult> Confirm(BookingConfirmPostModel bookingConfirmPostModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var booking = new Booking(userId, bookingConfirmPostModel.CarId,
                 bookingConfirmPostModel.StartDate, bookingConfirmPostModel.EndDate, bookingConfirmPostModel.TotalPrice);
            _appDbContext.Bookings.Add(booking);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index", new { id = booking.Id });
        }

    }
}
