using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class EmployeeMapper
    {
        public static EmployeeDto Map(Employee entity)
        {
            return new EmployeeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.User.Email,
                Username = entity.User.UserName,
                DOB = entity.DOB,
                Gender = entity.Gender,
                ProfileImageBytes = File.ReadAllBytes(entity.ProfileImagePath),
                IsActive = entity.IsActive
            };
        }
    }
}
