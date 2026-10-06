namespace CafeManagement.API.Application.DTOs
{
    public class CreateOrderRequest
    {
        public Guid SourceOrderId { get; set; }
        public int TableId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
