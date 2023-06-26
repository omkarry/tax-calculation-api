using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Interfaces
{
    public interface IEmployeeInvestmentRepository
    {
        public Task<List<IGrouping<string, EmployeeInvestmentDto>>> GetAllInvestments();
        public Task<List<IGrouping<string, EmployeeInvestmentDto>>> GetAllInvestmentsByYear(int yearId);
        public Task<List<EmployeeInvestmentDto>> GetEmployeeInvestmentsForYear(string empId, int yearId);
        public Task<List<IGrouping<int, EmployeeInvestmentDto>>> GetEmployeeInvestmentById(string empId);
        public Task<bool> AddEmployeeInvestment(string empId, List<EmployeeInvestmentDto> EmployeeInvestment);
        public Task<bool> UpdateEmployeeInvestment(string empId, List<EmployeeInvestmentDto> updatedEmployeeInvestment);
        public Task<bool> DeleteEmployeeInvestmentsByYear(string empId, int yearId);
        public Task<bool> DeleteEmployeeInvestment(string empId, int yearId, int investmentId);
    }
}
