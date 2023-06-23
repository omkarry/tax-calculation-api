using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Service.DTOs;
using Microsoft.AspNetCore.Http;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<List<EmployeeNames>> GetEmployeeNames();
        public Task<List<EmployeeDetailsDto>> GetEmployeesDetails();
        public Task<EmployeeDto?> GetEmployeeById(string id);
        public Task<bool> RegisterEmployee(string userId, RegisterDto inputModel);
        public Task<bool> UpdateEmployee(string userId, string empId, UpdateEmployeeDto updatedEmployee);
        public Task<bool> DeleteEmployee(string id);
        public Task<bool> UploadProfilePhoto(string username, string userId, IFormFile photo);
        public Task<bool> RemoveProfilePhoto(string id);

        public Task<CountDto> GetCount();
    }
}