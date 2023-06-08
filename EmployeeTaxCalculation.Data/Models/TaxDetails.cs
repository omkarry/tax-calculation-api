using EmployeeTaxCalculation.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Data.Models
{
    public class TaxDetails
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [ForeignKey("FinancialYear")]
        public int FinancialYearId { get; set; }
        public FinancialYear FinancialYear { get; set;}
        public int RegimeType { get; set; }
        public decimal TaxPaid { get; set; }
    }
}
