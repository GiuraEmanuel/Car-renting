using System.Collections.Generic;

namespace Car_Renting.Models
{
    public class CarRepository : ICarRepository
    {
        public IEnumerable<Car> GetAllCards => throw new System.NotImplementedException();

        public Car GetCarById(int carId)
        {
            throw new System.NotImplementedException();
        }
    }
}
