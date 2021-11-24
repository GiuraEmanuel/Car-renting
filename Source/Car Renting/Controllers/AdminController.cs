using Car_Renting.Data;
using Car_Renting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Inventory()
        {
            var cars = await _appDbContext.Cars
                .Where(car => car.Status == CarStatus.Active)
                .Select(car => new InventoryViewModel.Car(car.Id, car.Year, car.Manufacturer, car.Model, car.LicensePlate,
                 car.PricePerDay)).ToListAsync();

            var inventoryViewModel = new InventoryViewModel(cars);
            return View(inventoryViewModel);
        }

        [HttpGet("AddVehicle")]
        public IActionResult AddVehicle()
        {
            var model = new AddVehicleViewModel();
            return View(model);
        }

        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle(AddVehicleViewModel addVehicleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addVehicleViewModel);
            }

            var car = new Car(addVehicleViewModel.Year.Value,
                addVehicleViewModel.Manufacturer,
                addVehicleViewModel.Model,
                addVehicleViewModel.PricePerDay.Value,
                addVehicleViewModel.LicensePlate);

            _appDbContext.Cars.Add(car);

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is SqlException { Number: 2601 } sqlEx && sqlEx.Message.Contains("'IX_Cars_LicensePlate'"))
            {
                ModelState.AddModelError("LicensePlate", "A car with the same license plate already exists.");
                return View(addVehicleViewModel);
            }

            return RedirectToAction(nameof(Inventory));
        }

        [HttpGet("EditVehicle/{id}")]
        public async Task<IActionResult> EditVehicle(int id)
        {
            var car = await _appDbContext.Cars.SingleOrDefaultAsync(c => c.Id == id);
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

        [HttpPost("EditVehicle/{id}")]
        public async Task<IActionResult> EditVehicle(int id, EditVehicleViewModel editVehiclePriceViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var car = await _appDbContext.Cars.Where(c => c.Id == id).SingleOrDefaultAsync();

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel("Car does not exist"));
            }

            car.PricePerDay = editVehiclePriceViewModel.PricePerDay;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Inventory));
        }

        [HttpGet("DeleteVehicle/{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var car = await _appDbContext.Cars.Where(car => car.Id == id).SingleOrDefaultAsync();

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel("Car does not exist."));
            }

            DeleteVehicleViewModel deleteVehicleViewModel = new()
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
        public async Task<IActionResult> DeleteVehicleConfirm(int id)
        {
            var car = await _appDbContext.Cars.Where(car => car.Id == id).SingleOrDefaultAsync();

            if (car == null)
            {
                return View("ErrorMessage", new ErrorMessageViewModel("Car does not exist"));
            }

            car.Status = CarStatus.Deleted;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Inventory));
        }
    }
}
