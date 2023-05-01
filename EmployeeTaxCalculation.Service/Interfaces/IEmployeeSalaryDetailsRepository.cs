using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IEmployeeSalaryDetailsRepository
    {
        public Task<SalaryDetailsDto?> GetSalaryDetailsById(string id);
        public Task<int> AddSalaryDetails(SalaryDetailsDto salaryDetails);
        public Task<int?> UpdateSalaryDetails(int salaryDetailsId, SalaryDetailsDto updatedSalaryDetails);
        public Task<bool> DeleteSalaryDetails(int id);
    }
}
