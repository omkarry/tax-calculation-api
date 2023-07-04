using EmployeeTaxCalculation.Data.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<object?> Login(LoginDto model);
        public Task<bool> IsEmailExist(string email);
        public Task<bool> IsUsernameExist(string username);
        public Task<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string? token);
        public Task<JwtSecurityToken> GetToken(List<Claim> authClaims);
        public Task<string> GenerateRefreshToken();
    }
}
