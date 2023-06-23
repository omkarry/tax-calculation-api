using AutoMapper;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxCalculation.Service.Services
{
    public class SalaryDetailsService : ISalaryDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFinancialYearRepository _financialYear;

        public SalaryDetailsService(ApplicationDbContext dbContext, IFinancialYearRepository financialYear)
        {
            _dbContext = dbContext;
            _financialYear = financialYear;
        }

        public async Task<bool> AddSalaryDetails(SalaryDetailsDto salaryDetails)
        {
            SalaryDetails? empWithSalaryExist = await _dbContext.SalaryDetails
                                        .FirstOrDefaultAsync(s => s.EmployeeId == salaryDetails.EmployeeId
                                        && s.FinancialYearId == salaryDetails.FinancialYearId);
            if (empWithSalaryExist == null)
            {
                SalaryDetails newSalaryDetails = new()
                {
                    BasicPay = salaryDetails.BasicPay,
                    HRA = salaryDetails.HRA,
                    ConveyanceAllowance = salaryDetails.ConveyanceAllowance,
                    MedicalAllowance = salaryDetails.MedicalAllowance,
                    OtherAllowance = salaryDetails.OtherAllowance,
                    EPF = salaryDetails.EPF,
                    ProfessionalTax = salaryDetails.ProfessionalTax,
                    EmployeeId = salaryDetails.EmployeeId,
                    FinancialYearId = salaryDetails.FinancialYearId
                };
                _dbContext.SalaryDetails.Add(newSalaryDetails);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteSalaryDetails(string empId, int yearId)
        {
            SalaryDetails? empWithSalary = await _dbContext.SalaryDetails.FirstOrDefaultAsync(s => s.EmployeeId == empId && s.FinancialYearId == yearId);
            if (empWithSalary != null)
            {
                _dbContext.SalaryDetails.Remove(empWithSalary);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<SalaryDetailsDto?> GetCurrentYearSalaryDetails(string empId)
        {
            FinancialYearDto currentYear = await _financialYear.GetCurrentFinancialYear();
            return await _dbContext.SalaryDetails
                        .Where(e => e.FinancialYearId == currentYear.Id && e.EmployeeId == empId)
                        .Select(e => SalaryDetailsMapper.Map(e))
                        .FirstOrDefaultAsync();
        }

        public async Task<List<SalaryDetailsDto>?> GetSalaryDetails(string empId)
        {
            return await _dbContext.SalaryDetails
                        .Where(e => e.EmployeeId == empId)
                        .Select(e => SalaryDetailsMapper.Map(e))
                        .ToListAsync();
        }

        public async Task<SalaryDetailsDto?> GetSalaryDetailsByYear(string empId, int yearId)
        {
            return await _dbContext.SalaryDetails
                        .Where(e => e.FinancialYearId == yearId && e.EmployeeId == empId)
                        .Select(e => SalaryDetailsMapper.Map(e))
                        .FirstOrDefaultAsync();
        }

        public async Task<List<EmployeeNames>> PendingSalaryDetails()
        {
            FinancialYearDto currentYear = await _financialYear.GetCurrentFinancialYear();
            return await _dbContext.Employees
                        .Include(e => e.SalaryDetails)
                        .Where(e => e.SalaryDetails.Select(e => e.FinancialYearId == currentYear.Id).Count() != 0)
                        .Select(e => EmployeeNamesMapper.Map(e))
                        .ToListAsync();
        }

        public async Task<bool> UpdateSalaryDetails(int salaryDetailsId, SalaryDetailsDto updatedSalaryDetails)
        {
            SalaryDetails? salaryDetails = await _dbContext.SalaryDetails
                                                .FirstOrDefaultAsync(e => e.Id == salaryDetailsId);
            if (salaryDetails != null)
            {
                salaryDetails.BasicPay = updatedSalaryDetails.BasicPay;
                salaryDetails.HRA = updatedSalaryDetails.HRA;
                salaryDetails.ConveyanceAllowance = updatedSalaryDetails.ConveyanceAllowance;
                salaryDetails.MedicalAllowance = updatedSalaryDetails.MedicalAllowance;
                salaryDetails.OtherAllowance = updatedSalaryDetails.OtherAllowance;
                salaryDetails.EPF = updatedSalaryDetails.EPF;
                salaryDetails.ProfessionalTax = updatedSalaryDetails.ProfessionalTax;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
    }
}
