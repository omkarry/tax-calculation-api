using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class AdminMapper
    {
        public static AdminDto Map(Employee entity)
        {
            return new AdminDto
            {
                Id = entity.Id,
                Username = entity.User.UserName,
                Email = entity.User.Email
            };
        }
    }
}
