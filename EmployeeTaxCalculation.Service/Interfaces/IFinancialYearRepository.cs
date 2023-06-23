using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IFinancialYearRepository
    {
        public Task<bool> AddFinancialYear(NewFinancialYearDto financialYear);
        public Task<List<FinancialYearDto>> GetFinancialYears();
        public Task<FinancialYearDto> GetCurrentFinancialYear();
    }
}
