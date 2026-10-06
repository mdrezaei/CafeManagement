namespace MenuAndOrdering.API.Application.DTOs
{
    public class OrderItemRequest
    {
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
