using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SouthAmp.Core.Entities;
using SouthAmp.Infrastructure.Identity;

namespace SouthAmp.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<AppUser, AppRole, int>(options)
    {
        public DbSet<AppUserProfile> AppUserProfiles { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RoomPhoto> RoomPhotos { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}