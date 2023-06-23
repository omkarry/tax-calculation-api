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
    public class TaxDetailsService : ITaxDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFinancialYearRepository _financialYearRepository;
        public TaxDetailsService(ApplicationDbContext dbContext, IFinancialYearRepository financialYearRepository)
        {
            _dbContext = dbContext;
            _financialYearRepository = financialYearRepository; 
        }

        public async Task<bool> AddTaxDetails(TaxDetailsDTO taxDetailsDTO)
        {
            TaxDetails? taxDetails = await _dbContext.TaxDetails
                                        .FirstOrDefaultAsync(e => e.EmployeeId == taxDetailsDTO.EmployeeId && e.FinancialYearId == taxDetailsDTO.FinancialYearId);
            if (taxDetails == null)
            {
                TaxDetails newTaxDetails = new()
                {
                    Id = 0,
                    EmployeeId = taxDetailsDTO.EmployeeId,
                    FinancialYearId = taxDetailsDTO.FinancialYearId,
                    RegimeType = (int)taxDetailsDTO.RegimeType,
                    TaxPaid = taxDetailsDTO.TaxPaid
                };
                _dbContext.TaxDetails.Add(newTaxDetails);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteTaxDetails(string empId, int yearId)
        {
            TaxDetails? taxDetails = await _dbContext.TaxDetails
                                            .FirstOrDefaultAsync(e => e.FinancialYearId == yearId && e.EmployeeId == empId);
            if (taxDetails != null)
            {
                _dbContext.TaxDetails.Remove(taxDetails);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<List<IGrouping<int, TaxDetailsDTO>>> GetAllTaxDetails()
        {
            List<IGrouping<int, TaxDetailsDTO>> taxDetails = await _dbContext.TaxDetails
                                                                    .Select(e => TaxDetailsMapper.Map(e))
                                                                    .GroupBy(e => e.FinancialYearId)
                                                                    .ToListAsync();
            return taxDetails;
        }

        public async Task<List<TaxDetailsDTO>> GetAllTaxDetailsByYear(int yearId)
        {
            List<TaxDetailsDTO> taxDetails = await _dbContext.TaxDetails
                                                    .Where(e => e.FinancialYearId == yearId)
                                                    .Select(e => TaxDetailsMapper.Map(e))
                                                    .ToListAsync();
            return taxDetails;
        }

        public async Task<List<TaxDetailsDTO>> GetTaxDetails(string empId)
        {
            List<TaxDetailsDTO> taxDetails = await _dbContext.TaxDetails
                                                    .Where(e => e.EmployeeId == empId)
                                                    .Select(e => TaxDetailsMapper.Map(e))
                                                    .ToListAsync();
            return taxDetails;
        }

        public async Task<TaxDetailsDTO?> GetTaxDetailsByYear(string empId, int yearId)
        {
            TaxDetailsDTO? taxDetails = await _dbContext.TaxDetails
                                                    .Where(e => e.FinancialYearId == yearId && e.EmployeeId == empId)
                                                    .Select(e => TaxDetailsMapper.Map(e))
                                                    .FirstOrDefaultAsync();
            return taxDetails;
        }

        public async Task<List<EmployeeNames>> PendingTaxDeclaration()
        {
            FinancialYearDto currentYear = await _financialYearRepository.GetCurrentFinancialYear();
            List<EmployeeNames> employees = await _dbContext.Employees
                                                    .Include(e => e.TaxDetails)
                                                    .Where(e => e.TaxDetails.Select(e => e.FinancialYearId == currentYear.Id).Count() == 0)
                                                    .Select(e => EmployeeNamesMapper.Map(e))
                                                    .ToListAsync();
            return employees;
        }

        public async Task<bool> UpdateTaxDetails(TaxDetailsDTO taxDetailsDTO)
        {
            TaxDetails? taxDetails = await _dbContext.TaxDetails
                                            .FirstOrDefaultAsync(e => e.FinancialYearId == taxDetailsDTO.FinancialYearId && 
                                                                    e.EmployeeId == taxDetailsDTO.EmployeeId);
            if (taxDetails != null)
            {
                taxDetails.TaxPaid = taxDetailsDTO.TaxPaid;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
    }
}
