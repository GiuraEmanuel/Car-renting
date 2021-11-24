using System.ComponentModel.DataAnnotations;

namespace Car_Renting.Models
{
    public class EditVehicleViewModel
    {
        public int Id { get; set; }

        [Range(0.1, 1000.0)]
        public decimal PricePerDay { get; set; }
    }
}
