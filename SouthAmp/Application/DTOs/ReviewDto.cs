namespace SouthAmp.Application.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HotelId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}