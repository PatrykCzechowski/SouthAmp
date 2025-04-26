using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SouthAmp.Core.Entities;
using SouthAmp.Infrastructure.Identity;

namespace SouthAmp.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // Uwaga: IdentityDbContext już posiada DbSet<AppUser> Users
        // Jeśli chcesz korzystać z własnej encji User, zmień jej nazwę na np. AppUserProfile
        public DbSet<AppUserProfile> AppUserProfiles { get; set; }
        // ...inne DbSety...
    }
}