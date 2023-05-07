using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface ISectionRepository
    {
        public Task<List<SectionDto>?> GetSections();
    }
}
