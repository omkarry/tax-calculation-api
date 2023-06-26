using EmployeeTaxCalculation.Constants;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialYearController : ControllerBase
    {
        private readonly IFinancialYearRepository _financialYearRepository;
        public FinancialYearController(IFinancialYearRepository financialYearRepository)
        {
            _financialYearRepository = financialYearRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("FinancialYears")]
        public async Task<IActionResult> GetFinancialYears()
        {
            List<FinancialYearDto> financialYears = await _financialYearRepository.GetFinancialYears();
            return Ok(new ApiResponse<List<FinancialYearDto>> { Message = "List of financial years", Result = financialYears });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentFinancialYear")]
        public async Task<IActionResult> GetCurrentFinancialYear()
        {
            FinancialYearDto financialYear = await _financialYearRepository.GetCurrentFinancialYear();
            return Ok(new ApiResponse<FinancialYearDto> { Message = "Current financial year", Result = financialYear });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="financialYear"></param>
        /// <returns></returns>
        [HttpPost("AddFinancialYear")]
        public async Task<IActionResult> AddFinancialYear(NewFinancialYearDto financialYear)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseMessages.DataFormat);
            bool result = await _financialYearRepository.AddFinancialYear(financialYear);
            if (result)
            {
                return Ok(new ApiResponse<object> { Message = "Financial year added successfully" });
            }
            else
                return Ok(new ApiResponse<object> { Message = "Unable to add financial year" });
        }
    }
}
