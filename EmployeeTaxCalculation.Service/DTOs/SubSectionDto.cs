using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class SubSectionDto
    {
        public int Id { get; set; }
        public string? SubSectionName { get; set; }
        public decimal? MaxLimit { get; set; }
    }
}
