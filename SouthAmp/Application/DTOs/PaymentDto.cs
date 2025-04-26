namespace SouthAmp.Application.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PaymentProvider { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}