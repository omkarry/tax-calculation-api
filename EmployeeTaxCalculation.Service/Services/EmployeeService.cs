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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<string> RegisterEmployee(RegisterModel inputModel)
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

            if (!await _roleManager.RoleExistsAsync(UserRoles.Employee))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Employee))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Employee);
            }

            Employee newEmployee = new Employee()
            {
                Id = user.Id,
                Name = inputModel?.Name,
                IsActive = true
            };
            _dbContext.Employees.Add(newEmployee);
            await _dbContext.SaveChangesAsync();
            return user.Id;
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
