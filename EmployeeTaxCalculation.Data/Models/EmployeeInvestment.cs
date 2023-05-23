using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Data.Models
{
    public class EmployeeInvestment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("SubSections")]
        public int SubSectionId { get; set; }
        public SubSections SubSections { get; set; }

        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public Employee Employee { get; set;}

        [ForeignKey("Year")]
        public int YearId { get; set; }
        public FinancialYear FinantialYear{ get; set; }
        public decimal? InvestedAmount { get; set; }
    }
}
