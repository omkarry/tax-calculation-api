using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Data.Models
{
    public class SalaryDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal BasicPay { get; set; }
        [Required]
        public decimal HRA { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal OtherAllowance { get; set; }
        public decimal EPF { get; set; }
        public decimal ProfessionalTax { get; set; }
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
