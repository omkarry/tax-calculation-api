namespace EmployeeTaxCalculation.Service.DTOs
{
    public class EmployeeDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? Gender { get; set; }
        public byte[]? ProfileImageBytes { get; set; }
    }
}
