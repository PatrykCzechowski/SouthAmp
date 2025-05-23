namespace SouthAmp.Application.DTOs
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int OwnerId { get; set; }
        public bool IsActive { get; set; }
        public double AverageRating { get; set; }
    }
}