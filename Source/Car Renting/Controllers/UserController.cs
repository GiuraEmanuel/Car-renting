using Car_Renting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.Controllers
{
    [Authorize(Roles = "User")]
    [Route("User")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("DisplayCarsAvailableForBooking")]
        public IActionResult DisplayCarsAvailableForBooking()
        {
            var model = new AvailableCarsForBookingViewModel();
            return View(model);
        }
    }
}
