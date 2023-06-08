using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public virtual User User { get; set; }

        public virtual ICollection<SalaryDetails> SalaryDetails { get; set; }

        public virtual ICollection<EmployeeInvestment> EmployeeInvestments { get; set; }
        public virtual ICollection<TaxDetails> TaxDetails { get; set; }

        [ForeignKey("CreatedBy")]
        public string? CreatedById { get; set; }

        [ForeignKey("UpdatedBy")]
        public string? UpdatedById { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User? CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User? UpdatedByUser { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
