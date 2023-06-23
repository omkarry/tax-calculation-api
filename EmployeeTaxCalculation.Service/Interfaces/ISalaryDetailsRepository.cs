using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface ISalaryDetailsRepository
    {
        public Task<List<SalaryDetailsDto>?> GetSalaryDetails(string empId);
        public Task<SalaryDetailsDto?> GetSalaryDetailsByYear(string empId, int yearId);
        public Task<SalaryDetailsDto?> GetCurrentYearSalaryDetails(string empId);
        public Task<bool> AddSalaryDetails(SalaryDetailsDto salaryDetails);
        public Task<bool> UpdateSalaryDetails(int salaryDetailsId, SalaryDetailsDto updatedSalaryDetails);
        public Task<bool> DeleteSalaryDetails(string empId, int yearId);

        public Task<List<EmployeeNames>> PendingSalaryDetails();
    }
}
