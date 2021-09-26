using Car_Renting.Models;
using Car_Renting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Car_Renting.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AdminController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Inventory()
        {
            return View(_appDbContext.Cars);
        }

        [HttpGet]
        public IActionResult AddVehicle()
        {
            AddVehicleViewModel model = new AddVehicleViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddVehicle(AddVehicleViewModel viewModel)
        {
            var car = new Car
            {
                Model = viewModel.Model,
                Manufacturer = viewModel.Manufacturer,
                Year = viewModel.Year,
                PricePerDay = viewModel.PricePerDay
            };
            _appDbContext.Cars.Add(car);
            _appDbContext.SaveChanges();
            return RedirectToAction("Inventory");
        }

        [HttpGet]
        public IActionResult EditVehicle(int id)
        {
            var car = _appDbContext.Cars.Single(c => c.Id == id);

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel { Message = "Car does not exist" });
            }
            return View();
        }

        [HttpPost]
        public IActionResult EditVehicle(int id ,EditVehiclePriceViewModel editVehiclePriceViewModel)
        {
            editVehiclePriceViewModel = new EditVehiclePriceViewModel();
            var car = _appDbContext.Cars.Single(c => c.Id == id);
            car.PricePerDay = editVehiclePriceViewModel.PricePerDay;
            _appDbContext.SaveChanges();

            return RedirectToAction("Inventory");
        }

        //[HttpGet]
        //public IActionResult DeleteVehicle(int carId)
        //{
        //    return View("EditVehiclePrice");
        //}

        //[HttpPost]
        //public IActionResult DeleteVehicle(int carId)
        //{
        //    return View("EditVehiclePrice");
        //}
    }
}
