namespace MenuAndOrdering.API.Application.DTOs
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
