using EmployeeTaxCalculation.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class EmployeeInvestmentDto
    {
        public int Id { get; set; }
        public int SubSectionId { get; set; }
        public string EmployeeId { get; set; }
        public decimal? InvestedAmount { get; set; }
    }
}
