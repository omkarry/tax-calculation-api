using EmployeeTaxCalculation.Constants;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalaryDetailsController : ControllerBase
    {
        private readonly ISalaryDetailsRepository _salaryDetailsRepository;
        public SalaryDetailsController(ISalaryDetailsRepository salaryDetailsRepository)
        {
            _salaryDetailsRepository = salaryDetailsRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet("SalaryDetails/{empId}")]
        public async Task<IActionResult> GetSalaryDetails(string empId)
        {
            List<SalaryDetailsDto> salaryDetails = await _salaryDetailsRepository.GetSalaryDetails(empId);
            return Ok(new ApiResponse<List<SalaryDetailsDto>> { Message = "Salary details of employee", Result = salaryDetails });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        [HttpGet("SalaryDetailsByYear/{empId}/{yearId}")]
        public async Task<IActionResult> Get(string empId, int yearId)
        {
            try
            {
                SalaryDetailsDto? result = await _salaryDetailsRepository.GetSalaryDetailsByYear(empId, yearId);
                if (result != null)
                {
                    return Ok(new ApiResponse<SalaryDetailsDto> { Message = "Employee's Salary Details", Result = result });
                }
                return Ok(new ApiResponse<object> { Message = "Employee with salary Not found" });
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
        /// <returns></returns>
        [HttpGet("CurrentYearSalaryDetails/{empId}")]
        public async Task<IActionResult> CurrentYearSalaryDetails(string empId)
        {
            SalaryDetailsDto? salaryDetails = await _salaryDetailsRepository.GetCurrentYearSalaryDetails(empId);
            if (salaryDetails != null)
            {
                return Ok(new ApiResponse<SalaryDetailsDto> { Message = "Current year salary details", Result = salaryDetails });
            }
            else
                return NotFound(new ApiResponse<object> { Message = "Salary details not found for current year" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salaryDetails"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("AddSalaryDetails")]
        public async Task<IActionResult> AddSalaryDetails([FromBody] SalaryDetailsDto salaryDetails)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseMessages.DataFormat);
                bool result = await _salaryDetailsRepository.AddSalaryDetails(salaryDetails);

                if (!result)
                {
                    return Ok(new ApiResponse<object> { Message = "Unable to add salary details" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { Message = "Salary details added succesfully"});
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
        /// <param name="id"></param>
        /// <param name="updatedSalary"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateSalaryDetails/{id}")]
        public async Task<IActionResult> UpdateSalaryDetails(int id, [FromBody] SalaryDetailsDto updatedSalary)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ResponseMessages.DataFormat);
                bool result = await _salaryDetailsRepository.UpdateSalaryDetails(id, updatedSalary);
                if (!result)
                {
                    return Ok(new ApiResponse<string> { Message = "Unable to update salary details" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { Message = "Salary details updated succesfully" });
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteSalaryDetails/{empId}/{yearId}")]
        public async Task<IActionResult> DeleteSalaryDetails(string empId, int yearId)
        {
            try
            {
                bool result = await _salaryDetailsRepository.DeleteSalaryDetails(empId, yearId);
                if (!result)
                {
                    return Ok(new ApiResponse<object> { Message = "Unable to delete salary details" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { Message = "Salary details deleted succesfully" });
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
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("PendingSalaryDetails")]
        public async Task<IActionResult> GetPendingSalaryDetails()
        {
            List<EmployeeNames> employees = await _salaryDetailsRepository.PendingSalaryDetails();
            return Ok(new ApiResponse<List<EmployeeNames>> { Message = "List of employees with pending salary details" });
        }
    }
}
