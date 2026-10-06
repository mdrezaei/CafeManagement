namespace CafeManagement.API.Application.DTOs
{
    public class CreateMenuItemRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string? Description { get; set; }
    }
}
