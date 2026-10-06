namespace MenuAndOrdering.API.Application.DTOs
{
    public class MenuItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public string? ThumbnailUrl { get; set; }
        public List<string>? PicturesUrl { get; set; }
    }
}
