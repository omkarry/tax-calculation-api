using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxCalculationController : ControllerBase
    {
        public readonly ITaxCalculationRepository _employee;
        public TaxCalculationController(ITaxCalculationRepository employee)
        {
            _employee = employee;
        }

        [HttpGet]
        [Route("oldRegime")]
        public async Task<IActionResult> GetTaxByOldRegime(string empId)
        {
            try
            {
                decimal? result = await _employee.TaxByOldRegime(empId);
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
        public async Task<IActionResult> GetTaxByNewRegime(string empId)
        {
            try
            {
                decimal? result = await _employee.TaxByNewRegime(empId);
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
    }
}
