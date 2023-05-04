using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
