using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IAdminRepository
    {
        public Task<List<AdminDto>> GetAdmins();
        public Task<AdminDto> GetAdmin(string id);
        public Task<bool> RegisterAdmin(string userId, RegisterDto model);
        public Task<bool> UpdateAdmin(string userId, string adminId, UpdateEmployeeDto model);
        public Task<bool> DeleteAdmin(string userId, string adminId);
    }
}
