using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.Constants;
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

        public async Task<decimal?> TotalIncome(string empId)
        {
            SalaryDetails? EmpSalaryDetails = await _dbContext.SalaryDetails.FirstOrDefaultAsync(s => s.EmployeeId == empId);
            decimal? totalIncome = (EmpSalaryDetails?.BasicPay +
                            EmpSalaryDetails?.HRA +
                            EmpSalaryDetails?.ConveyanceAllowance +
                            EmpSalaryDetails?.MedicalAllowance +
                            EmpSalaryDetails?.OtherAllowance +
                            EmpSalaryDetails?.EPF +
                            EmpSalaryDetails?.ProfessionalTax) * 12;
            return totalIncome;
        }

        public async Task<decimal?> CalculateSection80CAmount(string empId)
        {
            List<EmployeeInvestment>? empInvestmentDetails = await _dbContext.EmployeeInvestments.Where(s => s.EmployeeId == empId && s.SubSections.Section.SectionName == "Section80").ToListAsync();

            decimal? section80C = empInvestmentDetails.Select(s => s.InvestedAmount).Sum();
            return section80C;
        }

        public async Task<decimal?> CalculateHRADeduction(string empId)
        {
            EmployeeInvestment? empInvestmentDetails = await _dbContext.EmployeeInvestments
                .FirstOrDefaultAsync(s => s.EmployeeId == empId && s.SubSections.SubSectionName == "HouseRentAllowance");
            SalaryDetails? salaryDetails = await _dbContext.SalaryDetails.FirstOrDefaultAsync(e => e.EmployeeId == empId);

            if (empInvestmentDetails?.InvestedAmount > salaryDetails?.HRA)
                return salaryDetails?.HRA;
            else
                return empInvestmentDetails?.InvestedAmount;
        }

        public async Task<decimal?> CalculateTaxableAmount(string empId)
        {
            SalaryDetails? EmpSalaryDetails = await _dbContext.SalaryDetails.FirstOrDefaultAsync(s => s.EmployeeId == empId);
            if (EmpSalaryDetails != null)
            {
                List<EmployeeInvestment>? empInvestmentDetails = await _dbContext.EmployeeInvestments.Where(s => s.EmployeeId == empId).ToListAsync();

                decimal? section80C = await CalculateSection80CAmount(empId);
                decimal? hra = await CalculateHRADeduction(empId);
                decimal? section80G = empInvestmentDetails
                    .Where(e => e.SubSections != null && e.SubSections.Section.SectionName == "Section80G")
                    .Sum(e => e.InvestedAmount);

                decimal? taxableAmount = await TotalIncome(empId);

                decimal? totalDeductableAmount = 0;

                foreach (EmployeeInvestment emp in empInvestmentDetails)
                {
                    if (emp.SubSections?.MaxLimit != null)
                    {
                        if (emp.InvestedAmount > emp.SubSections?.MaxLimit)
                        {
                            emp.InvestedAmount = emp.SubSections.MaxLimit;
                            totalDeductableAmount += emp.InvestedAmount;
                        }
                        else
                            totalDeductableAmount += emp.InvestedAmount;
                    }
                }

                totalDeductableAmount += section80C + section80G + hra;

                //if (EmpInvestmentDetails?.Section80DDB > EmployeeInvestmentLimits.Section80DDBLimit)
                //    EmpInvestmentDetails.Section80DDB = EmployeeInvestmentLimits.Section80DDBLimit;

                //if (EmpInvestmentDetails?.Section80U > EmployeeInvestmentLimits.Section80ULimit)
                //    EmpInvestmentDetails.Section80U = EmployeeInvestmentLimits.Section80ULimit;

                //if (EmpInvestmentDetails?.Section80CCG > EmployeeInvestmentLimits.Section80CCGLimit)
                //    EmpInvestmentDetails.Section80CCG = EmployeeInvestmentLimits.Section80CCGLimit;

                //if (EmpInvestmentDetails?.Section80DD > EmployeeInvestmentLimits.Section80DDLimit)
                //    EmpInvestmentDetails.Section80DD = EmployeeInvestmentLimits.Section80DDLimit;

                //if ((EmpInvestmentDetails?.HealthCheckup + EmpInvestmentDetails?.HealthInsurance) > EmployeeInvestmentLimits.Section80DLimit)
                //    section80D += EmployeeInvestmentLimits.Section80DLimit;
                //else
                //    section80D += EmpInvestmentDetails?.HealthCheckup + EmpInvestmentDetails?.HealthInsurance;
                //if ((EmpInvestmentDetails?.HealthCheckup + EmpInvestmentDetails?.HealthInsurance) > EmployeeInvestmentLimits.Section80DLimit)
                //    section80D += EmployeeInvestmentLimits.Section80DLimit;
                //else
                //    section80D += EmpInvestmentDetails?.HealthCheckupParent + EmpInvestmentDetails?.HealthInsuranceParent;

                //if ((section80C) > EmployeeInvestmentLimits.Section80CLimit)
                //    section80C = EmployeeInvestmentLimits.Section80CLimit;

                //if (EmpInvestmentDetails?.HouseRent > EmployeeInvestmentLimits.HouseRentLimit)
                //    EmpInvestmentDetails.HouseRent = EmployeeInvestmentLimits.HouseRentLimit;

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

                if (taxableAmount <= 500000)
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
            decimal? taxableAmount = await TotalIncome(EmpId) - 50000;
            if (taxableAmount == null)
                return null;
            else
            {
                decimal? taxToBePaid = 0;

                if (taxableAmount <= 700000)
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
