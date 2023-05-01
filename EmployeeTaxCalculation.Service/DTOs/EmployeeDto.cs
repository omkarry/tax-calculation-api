namespace EmployeeTaxCalculation.Service.DTOs
{
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? ProfileImagePath { get; set; }
        public bool IsActive { get; set; }
    }
}
