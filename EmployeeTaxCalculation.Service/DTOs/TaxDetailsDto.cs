using EmployeeTaxCalculation.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class TaxDetailsDTO
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public int FinancialYearId { get; set; }
        public RegimeTypeEnum RegimeType { get; set; }
        public decimal TaxPaid { get; set; }
    }
}
