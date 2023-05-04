using EmployeeTaxCalculation.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmplyeeTaxCalculation.Data.Auth
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<SalaryDetails> SalaryDetails { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SubSections> SubSections { get; set; }
        public DbSet<EmployeeInvestment> EmployeeInvestments{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfiguration(new AdminData());

            builder.Entity<Employee>()
                .HasKey(e => e.Id);

            builder.Entity<SalaryDetails>()
                .HasKey(s  => s.Id);

            builder.Entity<Section>()
                .HasKey(s => s.Id);

            builder.Entity<SubSections>()
                .HasKey(s => s.Id);

            builder.Entity<EmployeeInvestment>()
                .HasKey(i  => i.Id);

            builder.Entity<Employee>()
                .HasOne(e => e.SalaryDetails)
                .WithOne(e => e.Employee);

            builder.Entity<Employee>()
                .HasMany(e => e.EmployeeInvestments)
                .WithOne(e => e.Employee);

            builder.Entity<Section>()
                .HasMany(e => e.SubSections)
                .WithOne(e => e.Section);

            builder.Entity<SubSections>()
                .HasMany(e => e.Investments)
                .WithOne(e => e.SubSections);
        }
    }
}