using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class EmployeeNamesMapper
    {
        public static EmployeeNames Map(Employee entity)
        {
            return new EmployeeNames
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.User.Email
            };
        }
    }
}
