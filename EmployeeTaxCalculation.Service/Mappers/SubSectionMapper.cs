using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class SubSectionMapper
    {
        public static SubSectionDto Map(SubSections entity)
        {
            return new SubSectionDto
            {
                Id = entity.Id,
                SubSectionName = entity.SubSectionName,
                MaxLimit = entity.MaxLimit
            };
        }
    }
}
