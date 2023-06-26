using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxCalculation.Service.Services
{
    public class EmployeeInvestmentService : IEmployeeInvestmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeInvestmentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddEmployeeInvestment(string empId, List<EmployeeInvestmentDto> EmployeeInvestment)
        {
            EmployeeInvestment? empExist = await _dbContext.EmployeeInvestments.FirstOrDefaultAsync(s => s.EmployeeId == empId);
            if (empExist != null)
            {
                foreach (EmployeeInvestmentDto employeeInvestmentDto in EmployeeInvestment)
                {
                    EmployeeInvestment? newEmployeeInvestment = new()
                    {
                        Id = employeeInvestmentDto.Id,
                        SubSectionId = employeeInvestmentDto.SubSectionId,
                        EmployeeId = employeeInvestmentDto.EmployeeId,
                        InvestedAmount = employeeInvestmentDto.InvestedAmount
                    };
                    _dbContext.EmployeeInvestments.Add(newEmployeeInvestment);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteEmployeeInvestment(string empId, int yearId, int investmentId)
        {
            EmployeeInvestment? employeeInvestment = await _dbContext.EmployeeInvestments
                                                            .FirstOrDefaultAsync(e => e.Id == investmentId && e.YearId == yearId && e.EmployeeId == empId);
            if (employeeInvestment != null)
            {
                _dbContext.EmployeeInvestments.Remove(employeeInvestment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteEmployeeInvestmentsByYear(string empId, int yearId)
        {
            List<EmployeeInvestment> employeeInvestments = await _dbContext.EmployeeInvestments
                                                            .Where(e => e.YearId == yearId && e.EmployeeId == empId)
                                                            .ToListAsync();
            if (employeeInvestments.Count != 0)
            {
                _dbContext.EmployeeInvestments.RemoveRange(employeeInvestments);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<List<IGrouping<string, EmployeeInvestmentDto>>> GetAllInvestments()
        {
            List<IGrouping<string, EmployeeInvestmentDto>> employeeInvestments = await _dbContext.EmployeeInvestments
                                                                                .Select(e => EmployeeInvestmentMapper.Map(e))
                                                                                .GroupBy(e => e.EmployeeId).ToListAsync();
            return employeeInvestments;
        }

        public async Task<List<EmployeeInvestmentDto>> GetEmployeeInvestmentsForYear(string empId, int yearId)
        {
            List<EmployeeInvestmentDto> employeeInvestments = await _dbContext.EmployeeInvestments
                                                                                .Where(e => e.EmployeeId == empId && e.YearId == yearId)
                                                                                .Select(e => EmployeeInvestmentMapper.Map(e))
                                                                                .ToListAsync();
            return employeeInvestments;
        }

        public async Task<List<IGrouping<string, EmployeeInvestmentDto>>> GetAllInvestmentsByYear(int yearId)
        {
            List<IGrouping<string, EmployeeInvestmentDto>> employeeInvestments = await _dbContext.EmployeeInvestments
                                                                                .Where(e => e.YearId == yearId)
                                                                                .Select(e => EmployeeInvestmentMapper.Map(e))
                                                                                .GroupBy(e => e.EmployeeId).ToListAsync();
            return employeeInvestments;
        }

        public async Task<List<IGrouping<int, EmployeeInvestmentDto>>> GetEmployeeInvestmentById(string empId)
        {
            List<IGrouping<int, EmployeeInvestmentDto>> employeeInvestments = await _dbContext.EmployeeInvestments
                                                                                .Where(e => e.EmployeeId == empId)
                                                                                .Select(e => EmployeeInvestmentMapper.Map(e))
                                                                                .GroupBy(e => e.YearId).ToListAsync();
            return employeeInvestments;
        }

        public async Task<bool> UpdateEmployeeInvestment(string empId, List<EmployeeInvestmentDto> updatedEmployeeInvestment)
        {
            try
            {
                List<EmployeeInvestment>? empExist = await _dbContext.EmployeeInvestments.Where(s => s.EmployeeId == empId).ToListAsync();
                foreach (EmployeeInvestmentDto employeeInvestmentDto in updatedEmployeeInvestment)
                {
                    EmployeeInvestment? emp = empExist.FirstOrDefault(s => s.Id == employeeInvestmentDto.Id);
                    if (emp != null)
                    {
                        emp.InvestedAmount = employeeInvestmentDto.InvestedAmount;
                        _dbContext.EmployeeInvestments.Update(emp);
                    }
                    else
                    {
                        EmployeeInvestment? newEmployeeInvestment = new()
                        {
                            Id = employeeInvestmentDto.Id,
                            SubSectionId = employeeInvestmentDto.SubSectionId,
                            EmployeeId = empId,
                            InvestedAmount = employeeInvestmentDto.InvestedAmount
                        };
                        _dbContext.EmployeeInvestments.Add(newEmployeeInvestment);
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
