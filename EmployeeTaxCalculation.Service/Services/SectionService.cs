using AutoMapper;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxCalculation.Service.Services
{
    public class SectionService : ISectionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public SectionService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<SectionDto>?> GetSections()
        {
            return await _dbContext.Sections.Include(s => s.SubSections).Select(e => SectionMapper.Map(e)).ToListAsync();
        }

        public async Task<bool> UpdateSubSectionLimit(int subSectionId, decimal limit)
        {
            SubSections? subSection = await _dbContext.SubSections.FirstOrDefaultAsync(e => e.Id == subSectionId);
            if (subSection != null)
            {
                subSection.MaxLimit = limit;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
    }
}
