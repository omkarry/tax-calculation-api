using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaxCalculationController : ControllerBase
    {
        public readonly ITaxCalculationRepository _employee;
        public TaxCalculationController(ITaxCalculationRepository employee)
        {
            _employee = employee;
        }

        [HttpGet]
        [Route("oldRegime")]
        public async Task<IActionResult> GetTaxByOldRegime([FromQuery]string empId, int yearId)
        {
            try
            {
                decimal? result = await _employee.TaxByOldRegime(empId, yearId);
                if (result != null)
                {
                    return Ok(new ApiResponse<decimal?> { StatusCode = 200, Message = "Tax by old regime", Result = result }) ;
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Investment declaration not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("newRegime")]
        public async Task<IActionResult> GetTaxByNewRegime([FromQuery]string empId, int yearId)
        {
            try
            {
                decimal? result = await _employee.TaxByNewRegime(empId, yearId);
                if (result != null)
                {
                    return Ok(new ApiResponse<decimal?> { StatusCode = 200, Message = "Tax by New regime", Result = result });
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Investment declaration not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("TaxDetails")]
        public async Task<IActionResult> GetTaxDetails(string empId)
        {
            try
            {
                List<TaxDetailsDTO>? result = await _employee.GetTaxDetails(empId);
                if (result != null)
                {
                    return Ok(new ApiResponse<List<TaxDetailsDTO>?> { StatusCode = 200, Message = "Tax by New regime", Result = result });
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Investment declaration not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
