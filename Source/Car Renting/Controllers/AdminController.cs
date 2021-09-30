using Car_Renting.Models;
using Car_Renting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            var cars = _appDbContext.Cars.Select(car => new InventoryViewModel.Car(
                car.Id, car.Year, car.Manufacturer, car.Model, car.LicensePlate, car.PricePerDay));

            var inventoryViewModel = new InventoryViewModel(cars);
            return View(inventoryViewModel);
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
            var car = new Car(viewModel.Year, viewModel.Manufacturer, viewModel.Model, viewModel.PricePerDay, viewModel.LicensePlate);
            _appDbContext.Cars.Add(car);
            _appDbContext.SaveChanges();
            return RedirectToAction("Inventory");
        }

        [HttpGet]
        public IActionResult EditVehicle(int id)
        {
            var car = _appDbContext.Cars.SingleOrDefault(c => c.Id == id);
            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel("Car does not exist"));
            }

            var vmEditVehicle = new EditVehicleViewModel
            {
                Id = car.Id,
                PricePerDay = car.PricePerDay
            };

            return View(vmEditVehicle);
        }

        [HttpPost]
        public IActionResult EditVehicle(int id, EditVehicleViewModel editVehiclePriceViewModel)
        {
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
