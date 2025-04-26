namespace SouthAmp.Core.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<RoomPhoto> Photos { get; set; }
    }
}