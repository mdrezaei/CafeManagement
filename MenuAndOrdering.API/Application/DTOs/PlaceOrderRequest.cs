namespace MenuAndOrdering.API.Application.DTOs
{
    public class PlaceOrderRequest
    {
        public Guid CustomerId { get; set; }
        public int TableId { get; set; }
        public List<OrderItemRequest> Items { get; set; }
        public string? CustomerNote { get; set; }
    }
}
