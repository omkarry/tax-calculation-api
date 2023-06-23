using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.Mappers
{
    internal class EmployeeInvestmentMapper
    {
        public static EmployeeInvestmentDto Map(EmployeeInvestment entity)
        {
            return new EmployeeInvestmentDto
            {
                Id = entity.Id,
                SubSectionId = entity.SubSectionId,
                InvestedAmount = entity.InvestedAmount,
                EmployeeId = entity.EmployeeId,
                YearId = entity.YearId
            };
        }
    }
}
