namespace CafeManagement.API.Application.DTOs
{
    public class MMenuItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public string? ThumbNailUrl { get; set; }
        public List<string>? PicturesUrl { get; set; }
        public string Category { get; set; }
    }
}
