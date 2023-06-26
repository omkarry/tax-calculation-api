using EmployeeTaxCalculation.Constants;
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
    public class EmployeeInvestmentController : ControllerBase
    {
        private readonly IEmployeeInvestmentRepository _employee;
        public EmployeeInvestmentController(IEmployeeInvestmentRepository employee)
        {
            _employee = employee;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllInvestments")]
        public async Task<IActionResult> GetAllInvestments()
        {
            List<IGrouping<string, EmployeeInvestmentDto>> investments = await _employee.GetAllInvestments();
            return Ok(new ApiResponse<List<IGrouping<string, EmployeeInvestmentDto>>> { Message = "All investment details", Result = investments });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearId"></param>
        /// <returns></returns>
        [HttpGet("AllInvestmentsByYear/{yearId}")]
        public async Task<IActionResult> GetAllInvestmentsByYear(int yearId)
        {
            List<IGrouping<string, EmployeeInvestmentDto>> investments = await _employee.GetAllInvestmentsByYear(yearId);
            return Ok(new ApiResponse<List<IGrouping<string, EmployeeInvestmentDto>>> { Message = "All investments by year" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        [HttpGet("EmployeeInvestmentsForYear/{empId}/{yearId}")]
        public async Task<IActionResult> GetEmployeeInvestmentsForYear(string empId, int yearId)
        {
            List<EmployeeInvestmentDto> investments = await _employee.GetEmployeeInvestmentsForYear(empId: empId, yearId: yearId);
            return Ok(new ApiResponse<List<EmployeeInvestmentDto>> { Message = "Employee's investments in a year", Result = investments });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet("EmployeeInvestments/{empId}")]
        public async Task<IActionResult> GetEmployeeInvestmentsById(string empId)
        {
            try
            {
                List<IGrouping<int, EmployeeInvestmentDto>> result = await _employee.GetEmployeeInvestmentById(empId);
                return Ok(new ApiResponse<List<IGrouping<int, EmployeeInvestmentDto>>> { Message = "Employee's investments", Result = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="investmentDetails"></param>
        /// <returns></returns>
        [HttpPost("AddEmployeeInvestment/{empId}")]
        public async Task<IActionResult> Post(string empId, [FromBody] List<EmployeeInvestmentDto> investmentDetails)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseMessages.DataFormat);

                bool result = await _employee.AddEmployeeInvestment(empId, investmentDetails);

                if (!result)
                {
                    return Ok(new ApiResponse<object> { Message = "Unable to add employee investment" });
                }
                else
                {
                    return Ok(new ApiResponse<bool> { Message = "Investment details added succesfully", Result = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="updatedEmployeeInvestment"></param>
        /// <returns></returns>
        [HttpPut("UpdateInvestmentDetails/{empId}")]
        public async Task<IActionResult> Put(string empId, [FromBody] List<EmployeeInvestmentDto> updatedEmployeeInvestment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseMessages.DataFormat);

                bool result = await _employee.UpdateEmployeeInvestment(empId, updatedEmployeeInvestment);
                if (!result)
                {
                    return Ok(new ApiResponse<object> { Message = "Unable to update investment details" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { Message = "Investment details updated succesfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteInvestmentsByYear/{empId}/{yearId}")]
        public async Task<IActionResult> Delete(string empId, int yearId)
        {
            try
            {
                bool result = await _employee.DeleteEmployeeInvestmentsByYear(empId, yearId);
                if (!result)
                {
                    return Ok(new ApiResponse<object> { Message = "Unable to delete investment details" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { Message = "Investment details Deleted succesfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="yearId"></param>
        /// <param name="investmentId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteInvestment/{empId}/{yearId}/{investmentId}")]
        public async Task<IActionResult> DeleteEmployeeInvestment(string empId, int yearId, int investmentId)
        {
            try
            {
                bool result = await _employee.DeleteEmployeeInvestment(empId, yearId, investmentId);
                if (!result)
                {
                    return Ok(new ApiResponse<object> { Message = "Unable to delete investment details" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { Message = "Investment details Deleted succesfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}