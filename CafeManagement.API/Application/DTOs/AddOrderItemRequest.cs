namespace CafeManagement.API.Application.DTOs
{
    public class AddOrderItemRequest
    {
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
