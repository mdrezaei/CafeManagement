namespace CafeManagement.API.Application.DTOs
{
    public class MCustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
