using Car_Renting.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Add()
        {
            return RedirectToAction("Add");
        }
    }
}
