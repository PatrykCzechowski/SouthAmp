namespace SouthAmp.Application.DTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public string Status { get; set; }
    }
}