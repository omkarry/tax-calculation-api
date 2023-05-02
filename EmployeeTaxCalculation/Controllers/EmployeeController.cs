using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using EmployeeTaxCalculation.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employee;
        public EmployeeController(IEmployeeRepository employee)
        {
            _employee = employee;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _employee.GetEmployees();
                if (result != null)
                {
                    return Ok(new ApiResponse<List<EmployeeDto>> { StatusCode = 200, Message = "List of Employees", Result = result });
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "No employees" });
            }
            catch ( Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _employee.GetEmployeeById(id);
                if (result != null)
                {
                    return Ok(new ApiResponse<EmployeeDto> { StatusCode = 200, Message = "Employee Details", Result = result });
                }
                return Ok(new ApiResponse<EmployeeDto> { StatusCode = 200, Message = "Employee Not found" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel inputModel)
        {
            try
            {
                string result = await _employee.RegisterEmployee(inputModel);

                if (result.Equals("0"))
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "User Already exist" });
                }
                else if (result.Equals("-1"))
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "User Not Created" });
                }
                else
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "User Created succesfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] EmployeeDto updatedEmployee)
        {
            try
            {
                string? result = await _employee.UpdateEmployee(id, updatedEmployee);
                if (result.Equals("0"))
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "User Not found" });
                }
                else
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "User Updated succesfully" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                string? result = await _employee.DeleteEmployee(id);
                if (result.Equals("0"))
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "User Not Found" });
                }
                else
                {
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "User Deleted succesfully" });
                }
            }
            catch( Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
