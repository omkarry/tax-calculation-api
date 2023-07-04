using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxCalculation.Service.Services
{
    public class FinancialYearService : IFinancialYearRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FinancialYearService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddFinancialYear(NewFinancialYearDto financialYearDto)
        {
            FinancialYear? financialYearExist = await _dbContext.FinancialYear
                                    .FirstOrDefaultAsync(e => e.FinancialYearStartId == financialYearDto.FinancialYearStartId || e.FinancialYearEndId == financialYearDto.FinancialYearEndId);
            if(financialYearExist != null)
            {
                return false;
            }
            else
            {
                FinancialYear financialYear = new()
                {
                    Id = 0,
                    FinancialYearStartId = financialYearDto.FinancialYearStartId,
                    FinancialYearEndId = financialYearDto.FinancialYearEndId
                };
                _dbContext.FinancialYear.Add(financialYear);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<FinancialYearDto>> GetFinancialYears()
        {
            return await _dbContext.FinancialYear
                            .Include(e => e.FinancialYearStart)
                            .Include(e => e.FinancialYearEnd)
                            .Select(e => FinancialYearMapper.Map(e)).ToListAsync();
        }
        
        public async Task<FinancialYearDto> GetCurrentFinancialYear()
        {
            int currentMonth = DateTime.UtcNow.Month;
            int currentYear = DateTime.UtcNow.Year;
            FinancialYear financialYear;
            if (currentMonth >= 4)
            {
                financialYear = await _dbContext.FinancialYear
                                        .Include(e => e.FinancialYearStart)
                                        .Include(e => e.FinancialYearEnd)
                                        .FirstAsync(e => e.FinancialYearStart.Year == currentYear);
            }
            else
                financialYear = await _dbContext.FinancialYear
                                        .Include(e => e.FinancialYearStart)
                                        .Include(e => e.FinancialYearEnd).FirstAsync(e => e.FinancialYearEnd.Year == currentYear);
            return FinancialYearMapper.Map(financialYear);
        }
    }
}
