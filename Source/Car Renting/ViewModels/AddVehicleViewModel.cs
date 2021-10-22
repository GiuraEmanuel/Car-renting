using System.ComponentModel.DataAnnotations;

namespace Car_Renting.ViewModels
{
    public class AddVehicleViewModel
    {
        public int Year { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Manufacturer { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 2)]
        public string Model { get; set; } = string.Empty;

        [StringLength(15, MinimumLength =2)]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; } = string.Empty;

        [Range(0.1,1000.0)]
        public decimal PricePerDay { get; set; }
    }
}
