using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.Services
{
    public class RegimeYear : IRegimeYearRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RegimeYear(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddRegimeDetails(int yearId, List<SlabDto> slabs)
        {
            Slab? regimeDetailsExist = await _dbContext.Slab.FirstOrDefaultAsync(s => s.Id == yearId);
            if (regimeDetailsExist == null)
            {
                List<Slab> slabsDetails = slabs.Select(e => new Slab
                {
                    Id = 0,
                    SlabNumber = e.SlabNumber,
                    MaxLimit = e.SlabNumber,
                    PercentOfTax = e.PercentOfTax,
                    FinantialYearId = yearId
                }).ToList();
                _dbContext.Slab.AddRange(slabsDetails);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<List<FinancialYearDto>> GetFinancialYears()
        {
            List<FinancialYear> years = await _dbContext.FinancialYear
                            .Include(y => y.FinancialYearStart)
                            .Include(y => y.FinancialYearEnd)
                            .ToListAsync();
            return years.Select(e => FinancialYearMapper.Map(e)).ToList();
        }
    }
}
