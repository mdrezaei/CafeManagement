namespace CafeManagement.API.Application.DTOs
{
    public class MEmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Section { get; set; }
        public string Level { get; set; }
        public string? SecondarySection { get; set; }
        public string? SecondaryLevel { get; set; }
        public string? Description { get; set; }
    }
}
