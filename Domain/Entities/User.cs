using System.Collections.Generic;

namespace CloudCart.Api.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<Order> Orders { get; set; } = new();
    }
}
