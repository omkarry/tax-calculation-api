using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Data.Models
{
    public class OldRegime
    {
        [Key, ForeignKey("YearOfDeclaration")]
        public int FinancialYearId { get; set; }
        public FinancialYear FinancialYear { get; set; }
        [ForeignKey("OldRegimeYear")]
        public int? OldRegimeYearId { get; set; }
        public FinancialYear? OldRegimeYear { get; set; }
    }
}
