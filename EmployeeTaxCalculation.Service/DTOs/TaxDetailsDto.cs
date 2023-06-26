using EmployeeTaxCalculation.Data.Enums;

namespace EmployeeTaxCalculation.Service.DTOs
{
    public class TaxDetailsDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public int FinancialYearId { get; set; }
        public RegimeTypeEnum RegimeType { get; set; }
        public decimal TaxPaid { get; set; }
    }
}
