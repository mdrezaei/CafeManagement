namespace MenuAndOrdering.API.Application.DTOs
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Guid MenuItemId { get; set; }
        public string OrderedItemName { get; set; }
        public decimal OrderedUnitPrice { get; set; }
    }
}
