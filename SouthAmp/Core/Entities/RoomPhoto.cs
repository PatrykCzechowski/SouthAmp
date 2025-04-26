namespace SouthAmp.Core.Entities
{
    public class RoomPhoto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}