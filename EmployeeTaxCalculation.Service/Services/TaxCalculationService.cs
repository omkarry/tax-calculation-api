using EmployeeTaxCalculation.Data.Enums;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeTaxCalculation.Service.Services
{
    public class TaxCalculationService : ITaxCalculationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public TaxCalculationService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Calculate total income 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        public async Task<decimal?> TotalIncome(string empId, int yearId)
        {
            SalaryDetails? EmpSalaryDetails = await _dbContext.SalaryDetails.FirstOrDefaultAsync(s => s.EmployeeId == empId && s.FinancialYearId == yearId);
            decimal? totalIncome = (EmpSalaryDetails?.BasicPay +
                            EmpSalaryDetails?.HRA +
                            EmpSalaryDetails?.ConveyanceAllowance +
                            EmpSalaryDetails?.MedicalAllowance +
                            EmpSalaryDetails?.OtherAllowance +
                            EmpSalaryDetails?.EPF +
                            EmpSalaryDetails?.ProfessionalTax) * 12;
            return totalIncome;
        }

        /// <summary>
        /// Calculate investment under section80C
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        public async Task<decimal?> CalculateSection80CAmount(string empId, int yearId)
        {
            List<EmployeeInvestment>? empInvestmentDetails = await _dbContext.EmployeeInvestments
                .Where(s => s.EmployeeId == empId && s.SubSections.Section.SectionName == "Section80C" && s.YearId == yearId)
                .ToListAsync();

            decimal? section80C = empInvestmentDetails.Select(s => s.InvestedAmount).Sum();
            return section80C;
        }

        /// <summary>
        /// Calculate house rent allowance
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<decimal?> CalculateHRADeduction(string empId, int yearId)
        {
            EmployeeInvestment? empInvestmentDetails = await _dbContext.EmployeeInvestments
                .FirstOrDefaultAsync(s => s.EmployeeId == empId && s.SubSections.SubSectionName == "HouseRentAllowance");

            SalaryDetails? salaryDetails = await _dbContext.SalaryDetails.FirstOrDefaultAsync(e => e.EmployeeId == empId && e.FinancialYearId == yearId);

            if (empInvestmentDetails?.InvestedAmount > salaryDetails?.HRA)
                return salaryDetails?.HRA;
            else
                return empInvestmentDetails?.InvestedAmount;
        }

        /// <summary>
        /// Calculate taxable amount
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        public async Task<decimal?> CalculateTaxableAmount(string empId, int yearId)
        {
            SalaryDetails? EmpSalaryDetails = await _dbContext.SalaryDetails.FirstOrDefaultAsync(s => s.EmployeeId == empId && s.FinancialYearId == yearId);
            if (EmpSalaryDetails != null)
            {
                List<EmployeeInvestment>? empInvestmentDetails = await _dbContext.EmployeeInvestments.Where(s => s.EmployeeId == empId).ToListAsync();

                decimal? section80C = await CalculateSection80CAmount(empId, yearId);
                decimal? section80CLimit = decimal.Parse(_configuration.GetSection("Tax:Section80CLimit").Value);
                if (section80C > section80CLimit)
                {
                    section80C = section80CLimit;
                }
                decimal? hra = await CalculateHRADeduction(empId, yearId);
                decimal? section80G = empInvestmentDetails
                    .Where(e => e.SubSections != null && e.SubSections.Section.SectionName == "Section80G")
                    .Sum(e => e.InvestedAmount);

                decimal? taxableAmount = await TotalIncome(empId, yearId);

                decimal? totalDeductableAmount = 0;

                foreach (EmployeeInvestment emp in empInvestmentDetails)
                {
                    if (emp.SubSections?.MaxLimit != null)
                    {
                        if (emp.InvestedAmount > emp.SubSections?.MaxLimit)
                        {
                            emp.InvestedAmount = emp.SubSections.MaxLimit;
                            totalDeductableAmount += emp.InvestedAmount;
                        }
                        else
                            totalDeductableAmount += emp.InvestedAmount;
                    }
                }

                totalDeductableAmount += section80C + section80G + hra;
                taxableAmount -= totalDeductableAmount;
                return taxableAmount;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Calculate Tax by year
        /// </summary>
        /// <param name="taxableAmount"></param>
        /// <param name="yearId"></param>
        /// <param name="EmpId"></param>
        /// <param name="regimeType"></param>
        /// <returns></returns>
        public async Task<decimal?> CalculateTaxByYear(decimal? taxableAmount, int yearId)
        {
            decimal? taxToBePaid = 0;
            decimal? remainingAmount = taxableAmount;

            List<Slab> slabDetails = await _dbContext.Slab
                            .Where(s => s.FinantialYearId == yearId)
                            .ToListAsync();

            foreach (Slab slab in slabDetails)
            {
                if (remainingAmount <= 0)
                    break;

                if (remainingAmount <= slab.Limit)
                {
                    taxToBePaid += remainingAmount *
                    (decimal)(slab.PercentOfTax / 100);
                    break;
                }
                else
                {
                    if (slab.Limit == 0)
                    {
                        taxToBePaid += remainingAmount * (decimal)(slab.PercentOfTax / 100);
                        break;
                    }
                    else
                    {
                        taxToBePaid += slab.Limit * (decimal)(slab.PercentOfTax / 100);
                        remainingAmount -= slab.Limit;
                    }
                }
            }

            string CessValue = _configuration.GetSection("Tax:cess").Value;
            taxToBePaid += taxToBePaid * decimal.Parse(CessValue);
            return taxToBePaid;
        }

        /// <summary>
        /// Calculate tax bynew regime
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        public async Task<decimal?> TaxByNewRegime(string EmpId, int yearId)
        {
            string exemptionForNewRegime = _configuration.GetSection("Tax:NewRegimeExemption").Value;
            decimal? taxableAmount = await TotalIncome(EmpId, yearId) - decimal.Parse(exemptionForNewRegime);
            if (taxableAmount == null)
                return null;
            else
            {
                decimal? taxToBePaid = await CalculateTaxByYear(taxableAmount, yearId);
                return taxToBePaid;

            }
        }


        /// <summary>
        /// TAx calculation by new regime
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        public async Task<decimal?> TaxByOldRegime(string EmpId, int yearId)
        {
            decimal? taxableAmount = await CalculateTaxableAmount(EmpId, yearId);
            if (taxableAmount == null)
                return null;
            else
            {
                OldRegime? oldRegime = await _dbContext.OldRegime
                                    .FirstOrDefaultAsync(s => s.FinancialYearId == yearId);
                if (oldRegime == null)
                {
                    return null;
                }
                else
                {
                    decimal? taxToBePaid = await CalculateTaxByYear(taxableAmount, yearId);
                    return taxToBePaid;
                }
            }
        }

        public async Task<bool> AddTaxDetails(string EmpId, int yearId, int regimeType, decimal taxToBePaid)
        {
            _dbContext.TaxDetails.Add(new TaxDetails { Id = 0, EmployeeId = EmpId, FinancialYearId = yearId, RegimeType = regimeType, TaxPaid = taxToBePaid });
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaxDetailsDTO>> GetTaxDetails(string empId)
        {
            List<TaxDetailsDTO> taxDetails = await _dbContext.TaxDetails
                                .Include(e => e.FinancialYear.FinancialYearStart)
                                .Include(e => e.FinancialYear.FinancialYearEnd)
                                .Where(e => e.EmployeeId == empId)
                                .OrderBy(e => e.FinancialYear.FinancialYearStart)
                                .Select(e => TaxDetailsMapper.Map(e))
                                .ToListAsync();
            return taxDetails;
        }

        public async Task<List<EmployeeInvestmentDto>> GetInvestmentDetails(string empId, int yearId)
        {
            List<EmployeeInvestmentDto> investmentDetails = await _dbContext.EmployeeInvestments
                                .Include(e => e.FinantialYear.FinancialYearStart)
                                .Include(e => e.FinantialYear.FinancialYearEnd)
                                .Where(e => e.EmployeeId == empId && e.YearId == yearId)
                                .Select(e => EmployeeInvestmentMapper.Map(e))
                                .ToListAsync();
            return investmentDetails;
        }
    }
}
