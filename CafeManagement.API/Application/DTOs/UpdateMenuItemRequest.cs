namespace CafeManagement.API.Application.DTOs
{
    public class UpdateMenuItemRequest
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
    }
}
