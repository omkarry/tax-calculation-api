using EmployeeTaxCalculation.Data.Models;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public ICollection<SubSectionDto>? SubSections { get; set; }
    }
}
