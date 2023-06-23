using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface ITaxDetailsRepository
    {
        public Task<List<IGrouping<int, TaxDetailsDTO>>> GetAllTaxDetails();
        public Task<List<TaxDetailsDTO>> GetTaxDetails(string empId);
        public Task<TaxDetailsDTO?> GetTaxDetailsByYear(string empId, int yearId);
        public Task<List<TaxDetailsDTO>> GetAllTaxDetailsByYear(int yearId);
        public Task<bool> AddTaxDetails(TaxDetailsDTO taxDetailsDTO);
        public Task<bool> UpdateTaxDetails(TaxDetailsDTO taxDetailsDTO);
        public Task<bool> DeleteTaxDetails(string empId, int yearId);

        public Task<List<EmployeeNames>> PendingTaxDeclaration();
    }
}
