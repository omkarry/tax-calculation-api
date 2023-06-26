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
using TaxCalculation.Service.Interfaces;
using static System.Reflection.Metadata.BlobBuilder;

namespace EmployeeTaxCalculation.Service.Services
{
    public class RegimeYearService : IRegimeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RegimeYearService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddRegime(int yearId, int oldRegime, List<SlabDto> newRegime)
        {
            Slab? regimeDetailsExist = await _dbContext.Slab.FirstOrDefaultAsync(s => s.FinancialYearId == yearId);
            if (regimeDetailsExist == null)
            {
                _dbContext.OldRegime.Add(new OldRegime { FinancialYearId = yearId, OldRegimeYearId = oldRegime });
                _dbContext.Slab.AddRange(newRegime.Select(e => new Slab
                {
                    Id = 0,
                    SlabNumber = e.SlabNumber,
                    MaxLimit = e.SlabNumber,
                    PercentOfTax = e.PercentOfTax,
                    FinancialYearId = yearId
                }).ToList());
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteRegime(int yearId)
        {
            List<Slab> slabs = await _dbContext.Slab.Where(e => e.FinancialYearId == yearId).ToListAsync();
            if (slabs.Count == 0)
                return false;
            else
            {
                _dbContext.RemoveRange(slabs);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<IGrouping<int,SlabDto>>> GetAllRegimes()
        {
            return await _dbContext.Slab.Select(e => SlabMapper.Map(e)).GroupBy(e => e.FinantialYearId).ToListAsync();
        }

        public async Task<List<SlabDto>> GetAllRegimesByYear(int yearId)
        {
            return await _dbContext.Slab.Where(e => e.FinancialYearId == yearId).Select(e => SlabMapper.Map(e)).ToListAsync();
        }

        public async Task<bool> UpdateRegime(int yearId, List<SlabDto> updatedRegime)
        {
            try
            {
                List<Slab>? slabs = await _dbContext.Slab.Where(s => s.FinancialYearId == yearId).ToListAsync();
                foreach (SlabDto slabDto in updatedRegime)
                {
                    Slab? slab = slabs.FirstOrDefault(s => s.Id == slabDto.Id);
                    if (slab != null)
                    {
                        slab.MaxLimit = slabDto.Limit;
                        slab.PercentOfTax = slabDto.PercentOfTax;
                        _dbContext.Slab.Update(slab);
                    }
                    else
                    {
                        Slab newSlab = new()
                        {
                            Id = slabDto.Id,
                            SlabNumber = slabDto.SlabNumber,
                            MaxLimit = slabDto.Limit,
                            PercentOfTax = slabDto.PercentOfTax,
                            FinancialYearId = yearId
                        };
                        _dbContext.Slab.Add(newSlab);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
