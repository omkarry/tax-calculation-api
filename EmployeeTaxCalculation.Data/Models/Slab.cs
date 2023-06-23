using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Data.Models
{
    public class Slab
    {
        [Key]
        public int Id { get; set; }
        public int SlabNumber { get; set; }
        public decimal MaxLimit { get; set; }
        public float PercentOfTax { get; set; }
        [ForeignKey("FinancialYear")]
        public int FinancialYearId { get; set; }
        public FinancialYear FinancialYear { get; set; }
    }
}
