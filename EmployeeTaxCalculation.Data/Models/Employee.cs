using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EmployeeTaxCalculation.Data.Models
{
    public class Employee
    {
        [Key, ForeignKey("User")]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? ProfileImagePath { get; set; }
        public User User { get; set; }
        public SalaryDetails? SalaryDetails { get; set; }
        public InvestmentDeclaration? InvestmentDeclaration { get; set; }
        public bool IsActive { get; set; }
    }
}
