namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface ITaxCalculationRepository
    {
        public Task<decimal?> CalculateTaxableAmount(string EmpId);
        public Task<decimal?> TaxByOldRegime(string EmpId);
        public Task<decimal?> TaxByNewRegime(string EmpId);
    }
}
