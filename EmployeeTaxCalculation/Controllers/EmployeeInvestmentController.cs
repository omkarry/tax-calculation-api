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
    public class EmployeeInvestmentController : ControllerBase
    {
        private readonly IEmployeeInvestmentRepository _employee;
        public EmployeeInvestmentController(IEmployeeInvestmentRepository employee)
        {
            _employee = employee;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                List<EmployeeInvestmentDto>? result = await _employee.GetEmployeeInvestmentById(id);
                if (result != null)
                {
                    return Ok(new ApiResponse<List<EmployeeInvestmentDto>> { StatusCode = 200, Message = "Employee's Investment Details", Result = result });
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Employee with investment details not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery]string empId, [FromBody] List<EmployeeInvestmentDto> investmentDetails)
        {
            try
            {
                bool result = await _employee.AddEmployeeInvestment(empId, investmentDetails);

                if (!result)
                {
                    return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Employee with investment details already exist" });
                }
                else
                {
                    return Ok(new ApiResponse<bool> { StatusCode = 200, Message = "Investment details added succesfully", Result = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] List<EmployeeInvestmentDto> updatedEmployeeInvestment)
        {
            try
            {
                string? result = await _employee.UpdateEmployeeInvestment(id, updatedEmployeeInvestment);
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
                bool result = await _employee.DeleteEmployeeInvestment(id);
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
