using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaxCalculationController : ControllerBase
    {
        public readonly ITaxCalculationRepository _taxCalculationRepository;
        public TaxCalculationController(ITaxCalculationRepository taxCalculationRepository)
        {
            _taxCalculationRepository= taxCalculationRepository;
        }

        [HttpGet]
        [Route("OldRegime/{empId}/{yearId}")]
        public async Task<IActionResult> GetTaxByOldRegime(string empId, int yearId)
        {
            try
            {
                decimal? result = await _taxCalculationRepository.TaxByOldRegime(empId, yearId);
                if (result != null)
                {
                    return Ok(new ApiResponse<decimal?> { Message = "Tax by old regime", Result = result }) ;
                }
                return Ok(new ApiResponse<object> { Message = "Unable to calculate tax" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("NewRegime/{empId}/{yearId}")]
        public async Task<IActionResult> GetTaxByNewRegime(string empId, int yearId)
        {
            try
            {
                decimal? result = await _taxCalculationRepository.TaxByNewRegime(empId, yearId);
                if (result != null)
                {
                    return Ok(new ApiResponse<decimal?> { Message = "Tax by New regime", Result = result });
                }
                return Ok(new ApiResponse<object> {Message = "Unable to calculate tax" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
