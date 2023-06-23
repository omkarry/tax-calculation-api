using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class NewFinancialYearDto
    {
        public int Id { get; set; }
        public int FinancialYearStartId { get; set; }
        public int FinancialYearEndId { get; set; }
    }
}
