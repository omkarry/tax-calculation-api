using AutoMapper.Execution;
using EmployeeTaxCalculation.Data.Auth;
using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeTaxCalculation.Service.Services
{
    public class EmployeeService : IEmployeeRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public EmployeeService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
                                IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            List<Employee> employees = await _dbContext.Employees.Include(e => e.User).Where(e => e.IsActive == true).ToListAsync();
            return employees.Select(e => EmployeeMapper.Map(e)).ToList();
        }

        public async Task<EmployeeDto?> GetEmployeeById(string id)
        {
            Employee? result = await _dbContext.Employees.Include(e => e.User)
                                                  .FirstOrDefaultAsync(e => e.Id.Equals(id) && e.IsActive == true);
            if (result != null)
            {
                EmployeeDto employee = EmployeeMapper.Map(result);
                return employee;
            }
            return null;
        }

        public async Task<string> RegisterEmployee(string userId, EmployeeSalaryDto inputModel)
        {
            try
            {
                User userExists = await _userManager.FindByNameAsync(inputModel.Username);
                if (userExists != null)
                {
                    return "0";
                }

                User user = new User()
                {
                    Email = inputModel.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = inputModel.Username,
                };
                IdentityResult result = await _userManager.CreateAsync(user, inputModel.Password);

                if (!result.Succeeded)
                {
                    return "-1";
                }

                bool roleExist = await _roleManager.RoleExistsAsync(UserRoles.Employee);

                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));
                    await _userManager.AddToRoleAsync(user, UserRoles.Employee);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Employee);
                }

                Employee newEmployee = new()
                {
                    Id = user.Id,
                    Name = inputModel.Name,
                    CreatedAt = DateTime.Now,
                    CreatedById = userId,
                    IsActive = true
                };
                SalaryDetails empSalaryDetails = new()
                {
                    Id = 0,
                    BasicPay = inputModel.SalaryDetails.BasicPay,
                    HRA = inputModel.SalaryDetails.HRA,
                    ConveyanceAllowance = inputModel.SalaryDetails.ConveyanceAllowance,
                    MedicalAllowance = inputModel.SalaryDetails.MedicalAllowance,
                    OtherAllowance = inputModel.SalaryDetails.OtherAllowance,
                    EPF = inputModel.SalaryDetails.EPF,
                    ProfessionalTax = inputModel.SalaryDetails.ProfessionalTax,
                    EmployeeId = user.Id
                };
                _dbContext.Employees.Add(newEmployee);
                _dbContext.SalaryDetails.Add(empSalaryDetails);
                await _dbContext.SaveChangesAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string?> UpdateEmployee(string id, EmployeeDto updatedEmployee)
        {
            Employee? employee = await _dbContext.Employees.Include(e => e.User)
                                                    .FirstOrDefaultAsync(e => e.Id.Equals(id) && e.IsActive == true);

            if (employee != null)
            {
                if (!string.IsNullOrEmpty(updatedEmployee.Name))
                    employee.Name = updatedEmployee.Name;

                if (!string.IsNullOrEmpty(updatedEmployee.Gender))
                    employee.Gender = updatedEmployee.Gender;

                if (!string.IsNullOrEmpty(updatedEmployee.DOB.ToString()))
                    employee.DOB = updatedEmployee.DOB;

                if (!string.IsNullOrEmpty(updatedEmployee.ProfileImagePath))
                    employee.ProfileImagePath = updatedEmployee.ProfileImagePath;

                await _dbContext.SaveChangesAsync();
                return employee.Id;
            }
            else
                return "0";
        }

        public async Task<string> DeleteEmployee(string id)
        {
            Employee? employee = await _dbContext.Employees.Include(e => e.User)
                                                    .FirstOrDefaultAsync(e => e.Id.Equals(id) && e.IsActive == true);

            if (employee != null)
            {
                employee.IsActive = false;
                await _dbContext.SaveChangesAsync();
                return employee.User.Id;
            }
            else
                return "0";
        }
    }
}
