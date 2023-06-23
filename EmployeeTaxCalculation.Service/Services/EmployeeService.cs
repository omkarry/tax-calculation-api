using EmployeeTaxCalculation.Data.Auth;
using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.AspNetCore.Http;
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
        private readonly IFinancialYearRepository _financialYear;
        public EmployeeService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
                                IConfiguration configuration, ApplicationDbContext dbContext, IFinancialYearRepository financialYear)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _financialYear = financialYear;
        }

        public async Task<bool> DeleteEmployee(string id)
        {
            Employee? employee = await _dbContext.Employees.Include(e => e.User)
                                                    .FirstOrDefaultAsync(e => e.Id.Equals(id) && e.IsActive == true);

            if (employee != null)
            {
                employee.IsActive = false;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<CountDto> GetCount()
        {
            FinancialYearDto currentFinancialYear = await _financialYear.GetCurrentFinancialYear();
            CountDto count = new()
            {
                NumberOfEmployeesWorking = await _dbContext.Employees.CountAsync(e => e.IsActive),
                NumberOfDeclarationPending = await _dbContext.Employees
                                                    .Include(e => e.TaxDetails)
                                                    .CountAsync(e => e.TaxDetails.All(e => e.FinancialYearId == currentFinancialYear.Id)),
                NumberOfSalaryDetailsPending = await _dbContext.Employees
                                                    .Include(e => e.SalaryDetails)
                                                    .CountAsync(e => e.SalaryDetails.All(e => e.FinancialYearId == currentFinancialYear.Id))
            };
            return count;
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

        public async Task<List<EmployeeNames>> GetEmployeeNames()
        {
            List<Employee> employees = await _dbContext.Employees
                                                .Include(e => e.User)
                                                .Where(e => e.IsActive)
                                                .ToListAsync();
            return employees.Select(e => EmployeeNamesMapper.Map(e)).ToList();
        }

        public async Task<List<EmployeeDetailsDto>> GetEmployeesDetails()
        {
            List<Employee> employees = await _dbContext.Employees
                                                .Include(e => e.User)
                                                .Where(e => e.IsActive).ToListAsync();
            return employees.Select(e => EmployeeDetailsMapper.Map(e)).ToList();
        }

        public async Task<bool> RegisterEmployee(string userId, RegisterDto inputModel)
        {
            using (var dbcxtransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    User userExists = await _userManager.FindByNameAsync(inputModel.Username);
                    if (userExists != null)
                    {
                        return false;
                    }

                    User user = new()
                    {
                        Email = inputModel.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = inputModel.Username,
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, inputModel.Password);

                    if (!result.Succeeded)
                    {
                        return false;
                    }
                    else
                    {

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
                            Name = inputModel.Name!,
                            CreatedAt = DateTime.Now,
                            CreatedById = userId,
                            IsActive = true
                        };

                        _dbContext.Employees.Add(newEmployee);
                        await _dbContext.SaveChangesAsync();
                        await dbcxtransaction.CommitAsync();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    await dbcxtransaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> RemoveProfilePhoto(string id)
        {
            Employee? employee = await _dbContext.Employees
                                        .FirstOrDefaultAsync(e => e.Id == id);
            string? filePath = employee?.ProfileImagePath;
            if (employee == null || filePath == null || !File.Exists(filePath))
            {
                return false;
            }
            else
            {
                File.Delete(filePath);
                employee.ProfileImagePath = null;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            //if (employee != null)
            //{
            //    if (filePath != null)
            //    {
            //        if (!File.Exists(filePath))
            //        {
            //            return false;
            //        }
            //        else
            //        {
            //            File.Delete(filePath);
            //            employee.ProfileImagePath = null;
            //            return true;
            //        }
            //    }
            //    else
            //        return false;
            //}
            //else
            //    return false;
        }

        public async Task<bool> UpdateEmployee(string userId, string empId, UpdateEmployeeDto updatedEmployee)
        {
            Employee? employee = await _dbContext.Employees.Include(e => e.User)
                                               .FirstOrDefaultAsync(e => e.Id.Equals(empId) && e.IsActive == true);

            if (employee != null)
            {
                if (!string.IsNullOrEmpty(updatedEmployee.Name))
                    employee.Name = updatedEmployee.Name;

                if (!string.IsNullOrEmpty(updatedEmployee.Gender))
                    employee.Gender = updatedEmployee.Gender;

                if (!string.IsNullOrEmpty(updatedEmployee.DOB.ToString()))
                    employee.DOB = updatedEmployee.DOB;

                employee.UpdatedAt = DateTime.Now;
                employee.UpdatedById = userId;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<bool> UploadProfilePhoto(string username, string userId, IFormFile photo)
        {
            string folderName = Path.Combine("uploads", username);
            string filePath = Path.Combine(folderName, photo.FileName);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            Employee? profile = await _dbContext.Employees.FirstOrDefaultAsync(p => p.Id == userId);

            if (profile == null)
            {
                return false;
            }

            profile.ProfileImagePath = filePath;
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
