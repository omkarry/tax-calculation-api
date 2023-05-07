using System.ComponentModel.DataAnnotations;

namespace EmployeeTaxCalculation.Data.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SectionName { get; set; }
        public ICollection<SubSections>? SubSections { get; set; }
    }
}
