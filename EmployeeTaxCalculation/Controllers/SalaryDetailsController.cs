using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryDetailsController : ControllerBase
    {
        private readonly IEmployeeSalaryDetailsRepository _employee;
        public SalaryDetailsController(IEmployeeSalaryDetailsRepository employee)
        {
            _employee = employee;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _employee.GetSalaryDetailsById(id);
                if (result != null)
                {
                    return Ok(new ApiResponse<SalaryDetailsDto> { StatusCode = 200, Message = "Employee's Salary Details", Result = result });
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Employee with salary Not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SalaryDetailsDto salaryDetails)
        {
            try
            {
                int result = await _employee.AddSalaryDetails(salaryDetails);

                if (result == 1)
                {
                    return Ok(new ApiResponse<object> { StatusCode = 200, Message = "Employee with salary details Already exist" });
                }
                else
                {
                    return Ok(new ApiResponse<int> { StatusCode = 200, Message = "Salary details added succesfully", Result = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SalaryDetailsDto updatedSalary)
        {
            try
            {
                int? result = await _employee.UpdateSalaryDetails(id, updatedSalary);
                if (result == null)
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "Salary details Not found" });
                }
                else
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "Salary details Updated succesfully" });
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
                bool result = await _employee.DeleteSalaryDetails(id);
                if (!result)
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "Salary details Not Found" });
                }
                else
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "SalaryDetails Deleted succesfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
