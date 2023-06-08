using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class FinancialYearMapper
    {
        public static FinancialYearDto Map(FinancialYear entity)
        {
            return new FinancialYearDto
            {
                Id = entity.Id,
                FinancialYearStart = entity.FinancialYearStart.Year,
                FinancialYearEnd = entity.FinancialYearEnd.Year
            };
        }
    }
}
