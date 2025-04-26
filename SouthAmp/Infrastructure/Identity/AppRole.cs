using Microsoft.AspNetCore.Identity;

namespace SouthAmp.Infrastructure.Identity
{
    public class AppRole : IdentityRole<int>
    {
    }

    public enum UserRole
    {
        guest,
        provider,
        admin
    }
}