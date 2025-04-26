namespace SouthAmp.Core.Entities
{
    public class AppUserProfile
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        // ...inne właściwości zgodnie z wymaganiami...
    }
}