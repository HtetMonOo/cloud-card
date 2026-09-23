using CloudCart.Api.Domain.Entities;

namespace CloudCart.Api.DTOs
{
    public class DiscountEventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsActive { get; set; }
        public List<ProductDto> Products { get; set; } = new();
    }
}
