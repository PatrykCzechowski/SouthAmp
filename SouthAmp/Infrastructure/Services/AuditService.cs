using Serilog;

namespace SouthAmp.Infrastructure.Services
{
    public interface IAuditService
    {
        void LogUserAction(string userId, string action, string details = "");
    }

    public class AuditService : IAuditService
    {
        public void LogUserAction(string userId, string action, string details = "")
        {
            Log.Information("AUDIT | User: {UserId} | Action: {Action} | Details: {Details}", userId, action, details);
        }
    }
}