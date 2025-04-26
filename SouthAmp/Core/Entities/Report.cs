namespace SouthAmp.Core.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUserProfile User { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public ReportStatus Status { get; set; }
    }
    public enum ReportStatus { Open, Closed, InProgress }
}