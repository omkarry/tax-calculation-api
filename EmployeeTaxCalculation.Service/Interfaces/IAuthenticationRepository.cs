using EmployeeTaxCalculation.Data.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<object?> Login(LoginDto model);
        public Task<bool> IsEmailExist(string email);
        public Task<bool> IsUsernameExist(string username);
    }
}
