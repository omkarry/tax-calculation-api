using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeTaxCalculation.Data.Models
{
    public class InvestmentDeclaration
    {
        [Key]
        public int Id { get; set; }
        public decimal Section80G { get; set; }
        public decimal Section80DDB { get; set; }
        public decimal Section80U { get; set; }
        public decimal Section80CCG { get; set; }
        public decimal Section80DD { get; set; }
        public decimal Section80CCD { get; set; }
        public decimal HealthInsurance { get; set; }
        public decimal HealthCheckup { get; set; }
        public decimal HealthInsuranceParent { get; set; }
        public decimal HealthCheckupParent { get; set; }
        public decimal ProvidentFund { get; set; }
        public decimal LifeInsurance { get; set; }
        public decimal PPF { get; set; }
        public decimal NSC { get; set; }
        public decimal HousingLoan { get; set; }
        public decimal ChildrenEducation { get; set; }
        public decimal InfraBondsOrMFs { get; set; }
        public decimal OtherInvestments { get; set; }
        public decimal PensionScheme { get; set; }
        public decimal NationalPensionScheme { get; set; }
        public decimal HouseRent { get; set; }
        public decimal InterestOnSavings { get; set; }
        public decimal InterestOnDeposit { get; set; }
        public decimal OtherIncome { get; set; }
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
