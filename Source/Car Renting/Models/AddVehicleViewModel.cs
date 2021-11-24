using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Car_Renting.Models
{
    public class AddVehicleViewModel
    {
        [Required]
        [NotNull]
        [Range(2015,2100)]
        public int? Year { get; set; }

        [Required]
        [NotNull]
        [StringLength(50, MinimumLength = 3)]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [NotNull]
        [StringLength(50, MinimumLength = 2)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [NotNull]
        [StringLength(15, MinimumLength =2)]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [NotNull]
        [Range(0.1,1000.0)]
        public decimal? PricePerDay { get; set; }
    }
}
