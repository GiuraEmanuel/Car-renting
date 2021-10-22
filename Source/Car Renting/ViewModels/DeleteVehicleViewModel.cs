namespace Car_Renting.ViewModels
{
    public class DeleteVehicleViewModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
    }
}
