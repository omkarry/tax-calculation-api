namespace EmployeeTaxCalculation.Service.DTOs
{
    public class UpdateEmployeeDto
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
    }
}
