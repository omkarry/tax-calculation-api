using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface ITaxCalculationRepository
    {
        public Task<decimal?> TaxByOldRegime(string EmpId, int yearId);
        public Task<decimal?> TaxByNewRegime(string EmpId, int yearId);
    }
}
