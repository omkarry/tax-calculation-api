using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IRegimeYearRepository
    {
        public Task<List<FinancialYearDto>> GetFinancialYears();
        public Task<bool> AddRegimeDetails(int yearId, List<SlabDto> slabs);
    }
}
