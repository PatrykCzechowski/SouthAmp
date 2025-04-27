using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            // TODO: Integrate with real email provider (e.g. SMTP, SendGrid)
            await Task.CompletedTask;
        }
    }
}