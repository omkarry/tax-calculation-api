using EmployeeTaxCalculation.Data.Auth;
using EmployeeTaxCalculation.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection.Emit;

namespace EmplyeeTaxCalculation.Data.Auth
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<SalaryDetails> SalaryDetails { get; set; }
        public DbSet<InvestmentDeclaration> InvestmentDeclarations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfiguration(new AdminData());

            builder.Entity<Employee>()
                .HasKey(e => e.Id);

            builder.Entity<SalaryDetails>()
                .HasKey(s  => s.Id);

            builder.Entity<InvestmentDeclaration>()
                .HasKey(i  => i.Id);

            builder.Entity<Employee>()
                .HasOne(e => e.SalaryDetails)
                .WithOne(e => e.Employee);

            builder.Entity<Employee>()
                .HasOne(e => e.InvestmentDeclaration)
                .WithOne(e => e.Employee);
        }
    }
}