using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class EmployeeDetailsMapper
    {
        public static EmployeeDetailsDto Map(Employee entity)
        {
            return new EmployeeDetailsDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.User.Email,
                UserName = entity.User.UserName,
                Gender = entity.Gender,
                ProfileImageBytes = entity.ProfileImagePath != null ? File.ReadAllBytes(entity.ProfileImagePath) : null
            };
        }
    }
}
