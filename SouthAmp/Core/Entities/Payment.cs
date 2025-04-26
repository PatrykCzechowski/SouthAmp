namespace SouthAmp.Core.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string PaymentProvider { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? RefundedAt { get; set; }
    }
    public enum PaymentStatus { Pending, Confirmed, Refunded, Failed }
}