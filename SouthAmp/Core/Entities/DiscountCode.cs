namespace SouthAmp.Core.Entities
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
        public int? ProviderId { get; set; }
        public AppUserProfile Provider { get; set; }
    }
}