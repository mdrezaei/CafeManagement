namespace CafeManagement.API.Application.DTOs
{
    public class MOrderDto
    {
        public Guid Id { get; set; }
        public List<MOrderItemDto> OrderItems { get; set; } = new();
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerNote { get; set; }
        public int TableId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid SourceOrderId { get; set; }
        public Guid? AssignedWaiterId { get; set; }
        public Guid? AssignedBaristaId { get; set; }
        public Guid? AssignedChefId { get; set; }
        public Guid? AssignedCashierId { get; set; }
    }
}
