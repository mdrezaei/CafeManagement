using MenuAndOrdering.API.Domain.Entities;
using MenuAndOrdering.API.Domain.ValueObjects;

namespace MenuAndOrdering.API.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string? CustomerNote { get; set; }
        public Guid CustomerId { get; set; }
        public int TableId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
