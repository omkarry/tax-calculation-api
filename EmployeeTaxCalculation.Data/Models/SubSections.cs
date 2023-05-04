using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Data.Models
{
    public class SubSections
    {
        [Key]
        public int Id { get; set; }
        public string SubSectionName { get; set; }
        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public decimal? MaxLimit { get; set; }

        public ICollection<EmployeeInvestment>? Investments { get; set; }
    }
}
