using EmployeeTaxCalculation.Constants;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaxCalculation.Service.Interfaces;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegimeController : ControllerBase
    {
        private readonly IRegimeRepository _regimeRepository;
        public RegimeController(IRegimeRepository regimeRepository)
        {
            _regimeRepository = regimeRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllRegimes")]
        public async Task<IActionResult> GetAllRegimes()
        {
            List<IGrouping<int, SlabDto>> regimes = await _regimeRepository.GetAllRegimes();
            return Ok(new ApiResponse<List<IGrouping<int, SlabDto>>> { Message = "List of all regimes by year", Result = regimes});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearId"></param>
        /// <returns></returns>
        [HttpGet("Regime/{yearId}")]
        public async Task<IActionResult> GetRegimeByYear(int yearId)
        {
            List<SlabDto> slabs = await _regimeRepository.GetAllRegimesByYear(yearId);
            return Ok(new ApiResponse<List<SlabDto>> { Message = "Regime details", Result = slabs });
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearId"></param>
        /// <param name="oldRegime"></param>
        /// <param name="slabs"></param>
        /// <returns></returns>
        [HttpPost("AddRegime/{yearId}")]
        public async Task<IActionResult> AddRegime(int yearId, [FromQuery] int oldRegime, List<SlabDto> slabs)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ResponseMessages.DataFormat);
                bool result = await _regimeRepository.AddRegime(yearId, oldRegime, slabs);
                if (result)
                {
                    return Ok(new ApiResponse<object> { Message = "Regime details added successfully" });
                }
                return Ok(new ApiResponse<object> { Message = "Unable to add regime details" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearId"></param>
        /// <param name="updatedRegime"></param>
        /// <returns></returns>
        [HttpPut("UpdateRegime/{yearId}")]
        public async Task<IActionResult> UpdateRegime(int yearId, List<SlabDto> updatedRegime)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseMessages.DataFormat);

                bool result = await _regimeRepository.UpdateRegime(yearId, updatedRegime);
                if (result)
                {
                    return Ok(new ApiResponse<object> { Message = "Regime details updated successfully" });
                }
                return Ok(new ApiResponse<object> { Message = "Unable to update" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRegime/{yearId}")]
        public async Task<IActionResult> DeleteRegime(int yearId)
        {
            try
            {
                bool result = await _regimeRepository.DeleteRegime(yearId);
                if (result)
                {
                    return Ok(new ApiResponse<object> { Message = "Regime details deleted successfully" });
                }
                return Ok(new ApiResponse<object> { Message = "Unable to delete" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
