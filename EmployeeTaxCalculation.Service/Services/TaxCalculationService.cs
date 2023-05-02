using EmployeeTaxCalculation.Service.Interfaces;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxCalculation.Service.Services
{
    public class TaxCalculationService : ITaxCalculationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TaxCalculationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<decimal?> CalculateTaxableAmount(string EmpId)
        {
            var EmpSalaryDetails = await _dbContext.SalaryDetails.FirstOrDefaultAsync(s => s.EmployeeId == EmpId);
            if (EmpSalaryDetails != null)
            {
                var EmpInvestmentDetails = await _dbContext.InvestmentDeclarations.FirstOrDefaultAsync(s => s.EmployeeId == EmpId);

                decimal? section80D = 0;
                decimal? section80C = EmpInvestmentDetails?.ProvidentFund + EmpInvestmentDetails?.LifeInsurance + EmpInvestmentDetails?.PPF +
                    EmpInvestmentDetails?.NSC + EmpInvestmentDetails?.HousingLoan + +EmpInvestmentDetails?.ChildrenEducation +
                    EmpInvestmentDetails?.InfraBondsOrMFs + EmpInvestmentDetails?.OtherInvestments + EmpInvestmentDetails?.PensionScheme;

                var taxableAmount = (EmpSalaryDetails?.BasicPay +
                            EmpSalaryDetails?.HRA +
                            EmpSalaryDetails?.ConveyanceAllowance +
                            EmpSalaryDetails?.MedicalAllowance +
                            EmpSalaryDetails?.OtherAllowance +
                            EmpSalaryDetails?.EPF +
                            EmpSalaryDetails?.ProfessionalTax) * 12 +
                            EmpInvestmentDetails?.InterestOnSavings +
                            EmpInvestmentDetails?.InterestOnDeposit +
                            EmpInvestmentDetails?.OtherIncome;

                if (EmpInvestmentDetails?.Section80DDB > 100000)
                    EmpInvestmentDetails.Section80DDB = 100000;

                if (EmpInvestmentDetails?.Section80U > 125000)
                    EmpInvestmentDetails.Section80U = 125000;

                if (EmpInvestmentDetails?.Section80CCG > 25000)
                    EmpInvestmentDetails.Section80CCG = 25000;

                if (EmpInvestmentDetails?.Section80DD > 125000)
                    EmpInvestmentDetails.Section80DD = 125000;

                if ((EmpInvestmentDetails?.HealthCheckup + EmpInvestmentDetails?.HealthInsurance) > 25000)
                    section80D += 25000;
                else
                    section80D += EmpInvestmentDetails?.HealthCheckup + EmpInvestmentDetails?.HealthInsurance;
                if ((EmpInvestmentDetails?.HealthCheckup + EmpInvestmentDetails?.HealthInsurance) > 25000)
                    section80D += 25000;
                else
                    section80D += EmpInvestmentDetails?.HealthCheckupParent + EmpInvestmentDetails?.HealthInsuranceParent;

                if ((section80C) > 1500000)
                    section80C = 150000;

                if (EmpInvestmentDetails?.HouseRent > 60000)
                    EmpInvestmentDetails.HouseRent = 60000;

                var totalDeductableAmount = EmpInvestmentDetails?.Section80G +
                                            EmpInvestmentDetails?.Section80DDB +
                                            EmpInvestmentDetails?.Section80U +
                                            EmpInvestmentDetails?.Section80CCG +
                                            EmpInvestmentDetails?.Section80DD +
                                            EmpInvestmentDetails?.Section80CCD +
                                            section80D + section80C +
                                            EmpInvestmentDetails?.HouseRent;

                taxableAmount -= totalDeductableAmount;
                return taxableAmount;
            }
            else
            {
                return null;
            }
        }

        public async Task<decimal?> TaxByOldRegime(string EmpId)
        {
            decimal? taxableAmount = await CalculateTaxableAmount(EmpId);
            if (taxableAmount == null)
                return null;
            else
            {
                decimal? taxToBePaid = 0;

                if (taxableAmount <= 250000)
                {
                    return taxToBePaid;
                }
                else
                {
                    taxableAmount -= 250000;
                    if (taxableAmount > 250000)
                    {
                        taxToBePaid += 250000 * 0.05m;
                        taxableAmount -= 250000;
                        if (taxableAmount > 500000)
                        {
                            taxToBePaid += 500000 * 0.2m;
                            taxableAmount -= 500000;
                            if (taxableAmount > 0)
                            {
                                taxToBePaid += taxableAmount * 0.3m;
                                return taxToBePaid + 112500;
                            }
                            return taxToBePaid + 12500;
                        }
                        else
                        {
                            taxToBePaid += taxableAmount * 0.2m;
                            return taxToBePaid + 12500m;
                        }
                    }
                    else
                    {
                        taxToBePaid += taxableAmount * 0.05m;
                        return taxToBePaid;
                    }
                }
            }
        }

        public async Task<decimal?> TaxByNewRegime(string EmpId)
        {
            decimal? taxableAmount = await CalculateTaxableAmount(EmpId) - 50000;
            if (taxableAmount == null)
                return null;
            else
            {
                decimal? taxToBePaid = 0;

                if (taxableAmount <= 300000)
                {
                    return taxToBePaid;
                }
                else
                {
                    taxableAmount -= 300000;
                    if (taxableAmount > 300000)
                    {
                        taxToBePaid += 300000 * 0.05m;
                        taxableAmount -= 300000;
                        if (taxableAmount > 300000)
                        {
                            taxToBePaid += 300000 * 0.1m;
                            taxableAmount -= 300000;
                            if (taxableAmount > 300000)
                            {
                                taxToBePaid += 300000 * 0.15m;
                                taxableAmount -= 300000;
                                if (taxableAmount > 300000)
                                {
                                    taxToBePaid += 300000 * 0.2m;
                                    taxableAmount -= 300000;
                                    if (taxableAmount > 0)
                                    {
                                        taxToBePaid += taxableAmount * 0.3m;
                                        return taxToBePaid;
                                    }
                                    else
                                    {
                                        return taxToBePaid;
                                    }
                                }
                                else
                                {
                                    taxToBePaid += taxableAmount * 0.2m;
                                    return taxToBePaid;
                                }
                            }
                            else
                            {
                                taxToBePaid += taxableAmount * 0.15m;
                                return taxToBePaid + 12500;
                            }
                        }
                        else
                        {
                            taxToBePaid += taxableAmount * 0.1m;
                            return taxToBePaid + 12500m;
                        }
                    }
                    else
                    {
                        taxToBePaid += taxableAmount * 0.05m;
                        return taxToBePaid;
                    }
                }
            }
        }
    }
}
