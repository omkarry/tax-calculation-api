using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Service.DTOs;
using Microsoft.AspNetCore.Http;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<List<EmployeeDto>> GetEmployees();
        public Task<List<EmployeeDto>> GetEmployeesForPending();
        public Task<EmployeeDto?> GetEmployeeById(string id);
        public Task<string> RegisterEmployee(string userId, EmployeeSalaryDto inputModel);
        public Task<string?> UpdateEmployee(string id, EmployeeDto updatedEmployee);
        public Task<string> DeleteEmployee(string id);
        public Task<bool?> UploadProfile(string username, string userId, IFormFile photo);
    }
}