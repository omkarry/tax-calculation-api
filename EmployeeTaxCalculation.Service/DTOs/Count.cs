using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class CountDto
    {
        public int NumberOfEmployeesWorking { get; set; }
        public int NumberOfDeclarationPending { get; set; }
        public int NumberOfSalaryDetailsPending { get; set; }
    }
}
