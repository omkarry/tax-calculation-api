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
                DOB = entity.DOB,
                Gender = entity.Gender,
                ProfileImagePath = entity.ProfileImagePath,
                IsActive = entity.IsActive
            };
        }
    }
}
