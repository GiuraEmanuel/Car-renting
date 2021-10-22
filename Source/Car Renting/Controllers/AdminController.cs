using Car_Renting.Models;
using Car_Renting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Car_Renting.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AdminController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet("Inventory")]
        public IActionResult Inventory()
        {
            var cars = _appDbContext.Cars
                .Where(car => car.Status == CarStatus.Active)
                .Select(car => new InventoryViewModel.Car(car.Id, car.Year, car.Manufacturer, car.Model, car.LicensePlate,
                 car.PricePerDay));

            var inventoryViewModel = new InventoryViewModel(cars);
            return View(inventoryViewModel);
        }

        [HttpGet("AddVehicle")]
        public IActionResult AddVehicle()
        {
            AddVehicleViewModel model = new AddVehicleViewModel();
            return View(model);
        }

        [HttpPost("AddVehicle")]
        public IActionResult AddVehicle(AddVehicleViewModel addVehicleViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var car = new Car(addVehicleViewModel.Year,
                addVehicleViewModel.Manufacturer,
                addVehicleViewModel.Model,
                addVehicleViewModel.PricePerDay,
                addVehicleViewModel.LicensePlate);
            _appDbContext.Cars.Add(car);
            _appDbContext.SaveChanges();
            return RedirectToAction("Inventory");
        }

        [HttpGet("EditVehicle/{id}")]
        public IActionResult EditVehicle(int id)
        {
            var car = _appDbContext.Cars.SingleOrDefault(c => c.Id == id);
            if (car == null)
            {
                return View(new ErrorMessageViewModel("Car does not exist"));
            }

            var vmEditVehicle = new EditVehicleViewModel
            {
                Id = car.Id,
                PricePerDay = car.PricePerDay
            };

            return View(vmEditVehicle);
        }

        [HttpPost("EditVehicle/{id}")]
        public IActionResult EditVehicle(int id, EditVehicleViewModel editVehiclePriceViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var car = _appDbContext.Cars.Where(c => c.Id == id).SingleOrDefault();

            if (car == null)
            {
                return View(new ErrorMessageViewModel("Car does not exist"));
            }

            car.PricePerDay = editVehiclePriceViewModel.PricePerDay;
            _appDbContext.SaveChanges();

            return RedirectToAction("Inventory");
        }

        [HttpGet("DeleteVehicle/{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var car = _appDbContext.Cars.Where(car => car.Id == id).SingleOrDefault();

            if (car == null)
            {
                return View(new ErrorMessageViewModel("Car does not exist."));
            }

            DeleteVehicleViewModel deleteVehicleViewModel = new DeleteVehicleViewModel
            {
                Id = car.Id,
                Year = car.Year,
                Manufacturer = car.Manufacturer,
                Model = car.Model,
                LicensePlate = car.LicensePlate,
                PricePerDay = car.PricePerDay
            };

            return View(deleteVehicleViewModel);
        }

        [HttpPost("DeleteVehicleConfirm/{id}")]
        public IActionResult DeleteVehicleConfirm(int id)
        {
            var car = _appDbContext.Cars.Where(car => car.Id == id).SingleOrDefault();

            if (car == null)
            {
                return View(new ErrorMessageViewModel("Car does not exist"));
            }

            car.Status = CarStatus.Deleted;
            _appDbContext.SaveChanges();

            return RedirectToAction("Inventory");
        }
    }
}
