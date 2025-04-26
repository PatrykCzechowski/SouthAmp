namespace SouthAmp.Core.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUserProfile User { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus Status { get; set; }
        public Payment Payment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
    }
    public enum ReservationStatus { Pending, Confirmed, Cancelled }
}