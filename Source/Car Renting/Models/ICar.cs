namespace Car_Renting.Models
{
    public interface ICar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
    }
}
