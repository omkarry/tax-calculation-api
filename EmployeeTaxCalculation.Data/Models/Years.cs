using System.ComponentModel.DataAnnotations;

namespace EmployeeTaxCalculation.Data.Models
{
    public class Years
    {
        [Key]
        public int Id { get; set; }
        public int Year{ get; set; }
        public ICollection<FinancialYear> FinancialYears { get; set; }
    }
}
