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
    public class AdminService : IAdminRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AdminService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
                                IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<bool?> DeleteAdmin(string id)
        {
            Employee? employee = await _dbContext.Employees.Include(e => e.User)
                                                    .FirstOrDefaultAsync(e => e.Id.Equals(id) && e.IsActive == true);

            if (employee != null)
            {
                try
                {
                    employee.IsActive = false;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
            else
                return null;
        }

        public async Task<AdminDto?> GetAdmin(string id)
        {
            Employee? employee = await _dbContext.Employees.Include(e => e.User)
                                                    .FirstOrDefaultAsync(e => e.Id.Equals(id) && e.IsActive == true);
            if(employee != null)
            {
                return AdminMapper.Map(employee);
            }
            else
                return null;
        }

        public async Task<List<AdminDto>?> GetAdmins()
        {
            List<Employee> admins = await _dbContext.Employees.Include(e => e.User)
                                                    .Where(e => e.IsActive == true).ToListAsync();
            if (admins.Count != 0)
            {
                return admins.Select(e => AdminMapper.Map(e)).ToList();
            }
            else
                return null;
        }

        public async Task<bool?> RegisterAdmin(string userId, RegisterDto model)
        {
            using (var dbcxtransaction = _dbContext.Database.BeginTransaction())
            {
                User userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                    return null;
                try
                {
                    User user = new()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.Username
                    };
                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                        return false;
                    else
                    {
                        bool roleExist = await _roleManager.RoleExistsAsync(UserRoles.Admin);

                        if (!roleExist)
                        {
                            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                        }
                        Employee newAdmin = new()
                        {
                            Id = user.Id,
                            Name = model.Name!,
                            CreatedAt = DateTime.Now,
                            CreatedById = userId,
                            IsActive = true
                        };

                        _dbContext.Employees.Add(newAdmin);
                        await _dbContext.SaveChangesAsync();
                        await dbcxtransaction.CommitAsync();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    await dbcxtransaction.RollbackAsync();
                    Console.WriteLine(ex);
                    return false;

                }
            }
        }

        public async Task<bool?> UpdateAdmin(string userId, string adminId, UpdateEmployeeDto model)
        {
            try
            {
                Employee? employee = await _dbContext.Employees.Include(e => e.User)
                                                   .FirstOrDefaultAsync(e => e.Id.Equals(adminId) && e.IsActive == true);

                if (employee != null)
                {
                    if (!string.IsNullOrEmpty(model.Name))
                        employee.Name = model.Name;

                    if (!string.IsNullOrEmpty(model.Gender))
                        employee.Gender = model.Gender;

                    if (!string.IsNullOrEmpty(model.DOB.ToString()))
                        employee.DOB = model.DOB;

                    employee.UpdatedAt = DateTime.Now;
                    employee.UpdatedById = userId;

                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                    return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
