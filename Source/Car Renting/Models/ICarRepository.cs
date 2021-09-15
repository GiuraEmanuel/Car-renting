using System.Collections.Generic;

namespace Car_Renting.Models
{
    public interface ICarRepository
    {
        public IEnumerable<Car> GetAllCars { get;}
        Car GetCarById(int carId);
    }
}
