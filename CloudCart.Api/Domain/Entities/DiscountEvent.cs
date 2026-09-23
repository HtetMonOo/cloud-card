namespace CloudCart.Api.Domain.Entities
{
    public class DiscountEvent
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsActive =>
    DateTime.UtcNow >= StartDate &&
    DateTime.UtcNow <= EndDate;
        public List<Product> Products { get; set; } = new();

    }

    public enum DiscountType
    {
        Percentage,
        FixedAmount
    }
}
