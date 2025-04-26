namespace SouthAmp.Core.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        // Jeśli chcesz relację do profilu użytkownika:
        // public AppUserProfile User { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}