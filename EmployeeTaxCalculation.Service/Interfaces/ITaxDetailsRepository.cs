using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface ITaxDetailsRepository
    {
        public Task<List<IGrouping<int, TaxDetailsDto>>> GetAllTaxDetails();
        public Task<List<TaxDetailsDto>> GetTaxDetails(string empId);
        public Task<TaxDetailsDto?> GetTaxDetailsByYear(string empId, int yearId);
        public Task<List<TaxDetailsDto>> GetAllTaxDetailsByYear(int yearId);
        public Task<bool> AddTaxDetails(TaxDetailsDto taxDetailsDTO);
        public Task<bool> UpdateTaxDetails(TaxDetailsDto taxDetailsDTO);
        public Task<bool> DeleteTaxDetails(string empId, int yearId);

        public Task<List<EmployeeNames>> PendingTaxDeclaration();
    }
}
