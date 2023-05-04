using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IEmployeeInvestmentRepository
    {
        public Task<List<EmployeeInvestmentDto>?> GetEmployeeInvestmentById(string id);
        public Task<bool> AddEmployeeInvestment(string empId, List<EmployeeInvestmentDto> EmployeeInvestment);
        public Task<string?> UpdateEmployeeInvestment(string empId, List<EmployeeInvestmentDto> updatedEmployeeInvestment);
        public Task<bool> DeleteEmployeeInvestment(int id);
    }
}
