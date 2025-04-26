namespace SouthAmp.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUserProfile User { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsReported { get; set; }
    }
}