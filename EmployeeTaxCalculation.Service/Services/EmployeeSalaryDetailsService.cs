using AutoMapper;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxCalculation.Service.Services
{
    public class EmployeeSalaryDetailsService : IEmployeeSalaryDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeSalaryDetailsService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> AddSalaryDetails(SalaryDetailsDto salaryDetails)
        {
            try
            {
                SalaryDetails? empWithSalaryExist = await _dbContext.SalaryDetails
                                                        .FirstOrDefaultAsync(s => s.EmployeeId == salaryDetails.EmployeeId
                                                        && s.FinancialYearId == salaryDetails.FinancialYearId);
                if (empWithSalaryExist == null)
                {
                    SalaryDetails? newSalaryDetails = new()
                    {
                        BasicPay = salaryDetails.BasicPay,
                        HRA = salaryDetails.HRA,
                        ConveyanceAllowance = salaryDetails.ConveyanceAllowance,
                        MedicalAllowance = salaryDetails.MedicalAllowance,
                        OtherAllowance = salaryDetails.MedicalAllowance,
                        EPF = salaryDetails.EPF,
                        ProfessionalTax = salaryDetails.ProfessionalTax,
                        EmployeeId = salaryDetails.EmployeeId,
                        FinancialYearId = salaryDetails.FinancialYearId
                    };
                    _dbContext.SalaryDetails.Add(newSalaryDetails);
                    await _dbContext.SaveChangesAsync();
                    return newSalaryDetails.Id;
                }
                else
                    return 1;
            }
            catch (Exception)
            {
                return (-1);
            }
        }

        public async Task<bool> DeleteSalaryDetails(int id)
        {
            SalaryDetails? empWithSalaryExist = await _dbContext.SalaryDetails.FirstOrDefaultAsync(s => s.Id == id);
            if (empWithSalaryExist != null)
            {
                _dbContext.SalaryDetails.Remove(empWithSalaryExist);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<SalaryDetailsDto>?> GetSalaryDetails(string id)
        {
            List<SalaryDetails> employeeSalaryDetails= await _dbContext.SalaryDetails
                                                            .Where(s => s.EmployeeId == id).ToListAsync();
            if (employeeSalaryDetails.Count != 0)
            {
                return employeeSalaryDetails.Select(e => SalaryDetailsMapper.Map(e)).ToList();
            }
            return null;
        }

        public async Task<SalaryDetailsDto?> GetSalaryDetailsByYear(string id, int yearId)
        {
            SalaryDetails? employeeSalaryDetails = await _dbContext.SalaryDetails
                                                            .FirstOrDefaultAsync(s => s.EmployeeId == id && s.FinancialYearId == yearId);
            if (employeeSalaryDetails != null)
            {
                return SalaryDetailsMapper.Map(employeeSalaryDetails);
            }
            return null;
        }

        public async Task<int?> UpdateSalaryDetails(int salaryDetailsId, SalaryDetailsDto updatedSalaryDetails)
        {
            SalaryDetails? empWithSalaryExist = await _dbContext.SalaryDetails.FirstOrDefaultAsync(s => s.Id == salaryDetailsId);
            if (empWithSalaryExist != null)
            {
                empWithSalaryExist.BasicPay = updatedSalaryDetails.BasicPay;
                empWithSalaryExist.HRA = updatedSalaryDetails.HRA;
                empWithSalaryExist.ConveyanceAllowance = updatedSalaryDetails.ConveyanceAllowance;
                empWithSalaryExist.MedicalAllowance = updatedSalaryDetails.MedicalAllowance;
                empWithSalaryExist.OtherAllowance = updatedSalaryDetails.MedicalAllowance;
                empWithSalaryExist.EPF = updatedSalaryDetails.EPF;
                empWithSalaryExist.ProfessionalTax = updatedSalaryDetails.ProfessionalTax;
                empWithSalaryExist.EmployeeId = updatedSalaryDetails.EmployeeId;
                
                _dbContext.SalaryDetails.Update(empWithSalaryExist);
                await _dbContext.SaveChangesAsync();
                return empWithSalaryExist.Id;
            }
            return null;
        }
    }
}
