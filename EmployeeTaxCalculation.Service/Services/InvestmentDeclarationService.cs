using AutoMapper;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Mappers;
using EmplyeeTaxCalculation.Data.Auth;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxCalculation.Service.Services
{
    public class InvestmentDeclarationService : IInvestmentDeclarationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public InvestmentDeclarationService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> AddInvestmentDeclaration(InvestmentDeclarationDto investmentDeclaration)
        {
            var empExist = await _dbContext.InvestmentDeclarations.FirstOrDefaultAsync(s => s.EmployeeId == investmentDeclaration.EmployeeId);
            if (empExist == null)
            {
                InvestmentDeclaration? newInvestmentDeclaration = new()
                {
                    Section80G = investmentDeclaration.Section80G,
                    Section80DDB = investmentDeclaration.Section80DDB,
                    Section80U = investmentDeclaration.Section80U,
                    Section80CCG = investmentDeclaration.Section80CCG,
                    Section80DD = investmentDeclaration.Section80DD,
                    Section80CCD = investmentDeclaration.Section80CCD,
                    HealthInsurance = investmentDeclaration.HealthInsurance,
                    HealthCheckup = investmentDeclaration.HealthCheckup,
                    HealthInsuranceParent = investmentDeclaration.HealthInsuranceParent,
                    HealthCheckupParent = investmentDeclaration.HealthCheckupParent,
                    ProvidentFund = investmentDeclaration.ProvidentFund,
                    LifeInsurance = investmentDeclaration.LifeInsurance,
                    PPF = investmentDeclaration.PPF,
                    NSC = investmentDeclaration.NSC,
                    HousingLoan = investmentDeclaration.HousingLoan,
                    ChildrenEducation = investmentDeclaration.ChildrenEducation,
                    InfraBondsOrMFs = investmentDeclaration.InfraBondsOrMFs,
                    OtherInvestments = investmentDeclaration.OtherInvestments,
                    PensionScheme = investmentDeclaration.PensionScheme,
                    NationalPensionScheme = investmentDeclaration.NationalPensionScheme,
                    HouseRent = investmentDeclaration.HouseRent,
                    InterestOnSavings = investmentDeclaration.InterestOnSavings,
                    InterestOnDeposit = investmentDeclaration.InterestOnDeposit,
                    OtherIncome = investmentDeclaration.OtherIncome,
                    EmployeeId = investmentDeclaration.EmployeeId
                };       
                _dbContext.InvestmentDeclarations.Add(newInvestmentDeclaration);
                await _dbContext.SaveChangesAsync();
                return newInvestmentDeclaration.Id;
            }
            return 1;
        }

        public async Task<bool> DeleteInvestmentDeclaration(int id)
        {
            var empExist = await _dbContext.InvestmentDeclarations.FirstOrDefaultAsync(s => s.Id == id);
            if (empExist != null)
            {
                _dbContext.InvestmentDeclarations.Remove(empExist);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<InvestmentDeclarationDto?> GetInvestmentDeclarationById(string id)
        {
            var empExist = await _dbContext.InvestmentDeclarations.FirstOrDefaultAsync(s => s.EmployeeId == id);
            if (empExist != null)
            {
                return InvestmentDeclarationMapper.Map(empExist);
            }
            return null;
        }

        public async Task<int?> UpdateInvestmentDeclaration(int investmentDeclarationId, InvestmentDeclarationDto updatedInvestmentDeclaration)
        {
            var empExist = await _dbContext.InvestmentDeclarations.FirstOrDefaultAsync(s => s.Id == investmentDeclarationId);
            if (empExist != null)
            {
                empExist.Section80G = updatedInvestmentDeclaration.Section80G;
                empExist.Section80DDB = updatedInvestmentDeclaration.Section80DDB;
                empExist.Section80U = updatedInvestmentDeclaration.Section80U;
                empExist.Section80CCG = updatedInvestmentDeclaration.Section80CCG;
                empExist.Section80DD = updatedInvestmentDeclaration.Section80DD;
                empExist.Section80CCD = updatedInvestmentDeclaration.Section80CCD;
                empExist.HealthInsurance = updatedInvestmentDeclaration.HealthInsurance;
                empExist.HealthCheckup = updatedInvestmentDeclaration.HealthCheckup;
                empExist.HealthInsuranceParent = updatedInvestmentDeclaration.HealthInsuranceParent;
                empExist.HealthCheckupParent = updatedInvestmentDeclaration.HealthCheckupParent;
                empExist.ProvidentFund = updatedInvestmentDeclaration.ProvidentFund;
                empExist.LifeInsurance = updatedInvestmentDeclaration.LifeInsurance;
                empExist.PPF = updatedInvestmentDeclaration.PPF;
                empExist.NSC = updatedInvestmentDeclaration.NSC;
                empExist.HousingLoan = updatedInvestmentDeclaration.HousingLoan;
                empExist.ChildrenEducation = updatedInvestmentDeclaration.ChildrenEducation;
                empExist.InfraBondsOrMFs = updatedInvestmentDeclaration.InfraBondsOrMFs;
                empExist.OtherInvestments = updatedInvestmentDeclaration.OtherInvestments; 
                empExist.PensionScheme = updatedInvestmentDeclaration.PensionScheme;
                empExist.NationalPensionScheme = updatedInvestmentDeclaration.NationalPensionScheme;
                empExist.HouseRent = updatedInvestmentDeclaration.HouseRent;
                empExist.InterestOnDeposit = updatedInvestmentDeclaration.InterestOnDeposit;
                empExist.InterestOnSavings = updatedInvestmentDeclaration.InterestOnSavings;
                empExist.OtherIncome = updatedInvestmentDeclaration.OtherIncome;
                _dbContext.InvestmentDeclarations.Update(empExist);
                await _dbContext.SaveChangesAsync();
                return empExist.Id;
            }
            return null;
        }
    }
}
