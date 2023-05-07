using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class SectionMapper
    {
        public static SectionDto Map(Section entity)
        {
            return new SectionDto
            {
                Id = entity.Id,
                SectionName = entity.SectionName,
                SubSections = entity.SubSections?.Select(e => SubSectionMapper.Map(e)).ToList()
            };
        }
    }
}
