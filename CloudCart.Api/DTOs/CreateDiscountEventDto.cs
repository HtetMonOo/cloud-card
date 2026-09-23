using CloudCart.Api.Domain.Entities;

namespace CloudCart.Api.DTOs
{
    public class CreateDiscountEventDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}
