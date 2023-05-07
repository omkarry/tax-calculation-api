using EmployeeTaxCalculation.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
                .HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<Employee>(e => e.Id)
            .IsRequired();

            builder.Entity<Employee>()
                .HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
            .IsRequired();

            builder.Entity<Employee>()
                .HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedById);

            builder.Entity<Employee>()
                .HasOne(e => e.SalaryDetails)
                .WithOne(e => e.Employee);

            builder.Entity<Section>()
                .HasMany(e => e.SubSections)
                .WithOne(e => e.Section);

            builder.Entity<EmployeeInvestment>()
                .HasOne<SubSections>(e => e.SubSections)
                .WithMany(e => e.Investments)
                .HasForeignKey(e => e.SubSectionId);

            builder.Entity<EmployeeInvestment>()
                .HasOne<Employee>(e => e.Employee)
                .WithMany(e => e.EmployeeInvestments)
                .HasForeignKey(e => e.EmployeeId);

        }
    }
}