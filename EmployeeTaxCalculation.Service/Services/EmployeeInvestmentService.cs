using AutoMapper;
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
        private readonly IMapper _mapper;

        public EmployeeInvestmentService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> AddEmployeeInvestment(string empId, List<EmployeeInvestmentDto> EmployeeInvestment)
        {
            EmployeeInvestment? empExist = await _dbContext.EmployeeInvestments.FirstOrDefaultAsync(s => s.EmployeeId == empId);
            if (empExist == null)
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

        public async Task<bool> DeleteEmployeeInvestment(int id)
        {
            EmployeeInvestment? empExist = await _dbContext.EmployeeInvestments.FirstOrDefaultAsync(s => s.Id == id);
            if (empExist != null)
            {
                _dbContext.EmployeeInvestments.Remove(empExist);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<EmployeeInvestmentDto>?> GetEmployeeInvestmentById(string id)
        {
            List<EmployeeInvestment>? empExist = await _dbContext.EmployeeInvestments.Where(s => s.EmployeeId == id).ToListAsync();
            if (empExist != null)
            {
                return empExist.Select(s => EmployeeInvestmentMapper.Map(s)).ToList();
            }
            return null;
        }

        public async Task<string?> UpdateEmployeeInvestment(string empId, List<EmployeeInvestmentDto> updatedEmployeeInvestment)
        {
            List<EmployeeInvestment>? empExist = await _dbContext.EmployeeInvestments.Where(s => s.EmployeeId == empId).ToListAsync();
            if (empExist != null)
            {
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
                            EmployeeId = employeeInvestmentDto.EmployeeId,
                            InvestedAmount = employeeInvestmentDto.InvestedAmount
                        };
                        _dbContext.EmployeeInvestments.Add(newEmployeeInvestment);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return empId;
            }
            return null;
        }
    }
}
