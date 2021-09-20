using System.Collections.Generic;
using System.Linq;

namespace Car_Renting.Models
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _appDbContext;

        public CarRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Car> GetAllCars => _appDbContext.Cars.Select(c => c);

        public Car GetCarById(int carId) => _appDbContext.Cars.FirstOrDefault(c => c.Id == carId);
    }
}
