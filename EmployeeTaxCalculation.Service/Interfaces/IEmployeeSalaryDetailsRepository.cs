using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IEmployeeSalaryDetailsRepository
    {
        public Task<List<SalaryDetailsDto>?> GetSalaryDetails(string id);
        public Task<SalaryDetailsDto?> GetSalaryDetailsByYear(string id, int yearId);
        public Task<int> AddSalaryDetails(SalaryDetailsDto salaryDetails);
        public Task<int?> UpdateSalaryDetails(int salaryDetailsId, SalaryDetailsDto updatedSalaryDetails);
        public Task<bool> DeleteSalaryDetails(int id);
    }
}
