using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface ISectionRepository
    {
        public Task<bool> UpdateSubSectionLimit(int subSectionId, decimal limit);
        public Task<List<SectionDto>?> GetSections();
    }
}
