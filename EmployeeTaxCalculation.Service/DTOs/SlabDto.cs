using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class SlabDto
    {
        public int Id { get; set; }
        public int SlabNumber { get; set; }
        public decimal Limit { get; set; }
        public float PercentOfTax { get; set; }
        public int FinantialYearId { get; set; }
    }
}
