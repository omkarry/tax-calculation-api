using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentDeclarationController : ControllerBase
    {
        private readonly IInvestmentDeclarationRepository _employee;
        public InvestmentDeclarationController(IInvestmentDeclarationRepository employee)
        {
            _employee = employee;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _employee.GetInvestmentDeclarationById(id);
                if (result != null)
                {
                    return Ok(new ApiResponse<InvestmentDeclarationDto> { StatusCode = 200, Message = "Employee's Investment Details", Result = result });
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Employee with investment details not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InvestmentDeclarationDto investmentDetails)
        {
            try
            {
                int result = await _employee.AddInvestmentDeclaration(investmentDetails);

                if (result == 1)
                {
                    return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Employee with investment details already exist" });
                }
                else
                {
                    return Ok(new ApiResponse<int> { StatusCode = 200, Message = "Investment details added succesfully", Result = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] InvestmentDeclarationDto updatedInvestmentDeclaration)
        {
            try
            {
                int? result = await _employee.UpdateInvestmentDeclaration(id, updatedInvestmentDeclaration);
                if (result == null)
                {
                    return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Investment details not found" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Investment details updated succesfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _employee.DeleteInvestmentDeclaration(id);
                if (!result)
                {
                    return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Investment details Not Found" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Investment details Deleted succesfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
