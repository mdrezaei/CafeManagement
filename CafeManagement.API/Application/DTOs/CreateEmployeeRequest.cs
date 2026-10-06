namespace CafeManagement.API.Application.DTOs
{
    public class CreateEmployeeRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Section { get; set; }
        public string Level { get; set; }
    }
}
