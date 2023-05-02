using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class InvestmentDeclarationMapper
    {
        public static InvestmentDeclarationDto Map(InvestmentDeclaration entity)
        {
            return new InvestmentDeclarationDto
            {
                Id = entity.Id,
                Section80G = entity.Section80G,
                Section80DDB = entity.Section80DDB,
                Section80U = entity.Section80U,
                Section80CCG = entity.Section80CCG,
                Section80DD = entity.Section80DD,
                Section80CCD = entity.Section80CCD,
                HealthInsurance = entity.HealthInsurance,
                HealthCheckup = entity.HealthCheckup,
                HealthInsuranceParent = entity.HealthCheckupParent,
                HealthCheckupParent = entity.HealthCheckupParent,
                ProvidentFund = entity.ProvidentFund,
                LifeInsurance = entity.LifeInsurance,
                PPF = entity.PPF,
                NSC = entity.NSC,
                HousingLoan = entity.HousingLoan,
                ChildrenEducation = entity.ChildrenEducation,
                InfraBondsOrMFs = entity.InfraBondsOrMFs,
                OtherInvestments = entity.OtherInvestments,
                PensionScheme = entity.PensionScheme,
                NationalPensionScheme = entity.NationalPensionScheme,
                HouseRent = entity.HouseRent,
                InterestOnSavings = entity.InterestOnSavings,
                InterestOnDeposit = entity.InterestOnDeposit,
                OtherIncome = entity.OtherIncome,
                EmployeeId = entity.EmployeeId
            };
        }
    }
}
