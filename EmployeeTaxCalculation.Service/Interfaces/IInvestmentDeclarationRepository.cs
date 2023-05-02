using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IInvestmentDeclarationRepository
    {
        public Task<InvestmentDeclarationDto?> GetInvestmentDeclarationById(string id);
        public Task<int> AddInvestmentDeclaration(InvestmentDeclarationDto salaryDetails);
        public Task<int?> UpdateInvestmentDeclaration(int salaryDetailsId, InvestmentDeclarationDto updatedSalaryDetails);
        public Task<bool> DeleteInvestmentDeclaration(int id);
    }
}
