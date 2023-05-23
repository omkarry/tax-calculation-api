using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Data.Models
{
    public class FinancialYear
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("FinancialYearStart")]
        public int FinancialYearStartId { get; set; }
        public Years FinancialYearStart { get; set; }

        [ForeignKey("FinancialYearEnd")]
        public int FinancialYearEndId { get; set; }
        public Years FinancialYearEnd { get; set; }
        public ICollection<EmployeeInvestment> EmployeeInvestments { get; set; }
        public ICollection<OldRegime> OldRegimeDetails { get; set; }
        public List<Slab> Slabs { get; set; }
    }
}
