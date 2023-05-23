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
        public DbSet<EmployeeInvestment> EmployeeInvestments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new AdminData());

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<FinancialYear>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<OldRegime>()
                .HasKey(y => y.FinancialYearId);

            modelBuilder.Entity<SalaryDetails>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Section>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<SubSections>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<EmployeeInvestment>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<FinancialYear>()
                .HasIndex(e => new { e.FinancialYearStartId, e.FinancialYearEndId })
                .IsUnique();

            modelBuilder.Entity<SalaryDetails>()
                .HasIndex(s => new { s.EmployeeId, s.FinantialYearId })
                .IsUnique();

            modelBuilder.Entity<EmployeeInvestment>()
                .HasIndex(e => new { e.SubSectionId, e.EmployeeId, e.YearId})
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<Employee>(e => e.Id)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedById);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.SalaryDetails)
                .WithOne(e => e.Employee);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.SubSections)
                .WithOne(e => e.Section);

            modelBuilder.Entity<EmployeeInvestment>()
                .HasOne(e => e.SubSections)
                .WithMany(e => e.Investments)
                .HasForeignKey(e => e.SubSectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeInvestment>()
                .HasOne(e => e.Employee)
                .WithMany(e => e.EmployeeInvestments)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FinancialYear>()
                .HasMany(c => c.Slabs)
                .WithOne(c => c.FinantialYear)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinancialYear>()
               .HasMany(c => c.EmployeeInvestments)
               .WithOne(c => c.FinantialYear)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OldRegime>()
                .HasOne(e => e.OldRegimeYear)
                .WithMany(e => e.OldRegimeDetails)
                .HasForeignKey(fy => fy.OldRegimeYearId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OldRegime>()
                .HasOne(e => e.FinancialYear)
                .WithMany()
                .HasForeignKey(fy => fy.FinancialYearId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinancialYear>()
                .HasOne(fy => fy.FinancialYearStart)
                .WithMany(y => y.FinancialYears)
                .HasForeignKey(fy => fy.FinancialYearStartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinancialYear>()
                .HasOne(fy => fy.FinancialYearEnd)
                .WithMany()
                .HasForeignKey(fy => fy.FinancialYearEndId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}