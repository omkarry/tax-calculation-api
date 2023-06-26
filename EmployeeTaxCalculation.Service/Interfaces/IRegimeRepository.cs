using EmployeeTaxCalculation.Service.DTOs;

namespace TaxCalculation.Service.Interfaces
{
    public interface IRegimeRepository
    {
        public Task<List<IGrouping<int, SlabDto>>> GetAllRegimes();
        public Task<List<SlabDto>> GetAllRegimesByYear(int yearId);
        public Task<bool> AddRegime(int yearId, int oldRegime, List<SlabDto> newRegime);
        public Task<bool> UpdateRegime(int yearId, List<SlabDto> updatedRegime);
        public Task<bool> DeleteRegime(int yearId);
    }
}
