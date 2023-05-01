using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<List<EmployeeDto>> GetEmployees();
        public Task<EmployeeDto?> GetEmployeeById(string id);
        public Task<string> RegisterEmployee(RegisterModel inputModel);
        public Task<string?> UpdateEmployee(string id, EmployeeDto updatedEmployee);
        public Task<string> DeleteEmployee(string id);
    }
}