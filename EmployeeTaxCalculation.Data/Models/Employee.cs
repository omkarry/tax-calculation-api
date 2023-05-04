using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public ICollection<EmployeeInvestment>? EmployeeInvestments{ get; set; }

        [ForeignKey("CreatedBy")]
        public int CreatedBy { get; set; }
        public User CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UpdatedBy")]
        public int? UpdatedBy { get; set; }
        public User? UpdatedByUser { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
