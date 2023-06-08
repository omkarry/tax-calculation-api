using EmployeeTaxCalculation.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class SalaryDetailsDto
    {
        public int Id { get; set; }
        public decimal BasicPay { get; set; }
        public decimal HRA { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal OtherAllowance { get; set; }
        public decimal EPF { get; set; }
        public decimal ProfessionalTax { get; set; }
        public string EmployeeId { get; set; }
        public int FinancialYearId { get; set; }
    }
}
