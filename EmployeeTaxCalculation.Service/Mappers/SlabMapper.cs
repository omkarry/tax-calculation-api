using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class SlabMapper
    {
        public static SlabDto Map(Slab entity)
        {
            return new SlabDto
            {
                Id = entity.Id,
                SlabNumber = entity.SlabNumber,
                Limit = entity.MaxLimit,
                PercentOfTax = entity.PercentOfTax,
                FinantialYearId = entity.FinancialYearId
            };
        }
    }
}
