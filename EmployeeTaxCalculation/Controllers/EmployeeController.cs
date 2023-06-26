using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Constants;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeController(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Employees")]
        public async Task<IActionResult> GetEmployeesDetails()
        {
            try
            {
                List<EmployeeDetailsDto> result = await _employeeRepository.GetEmployeesDetails();
                if (result.Count != 0)
                {
                    return Ok(new ApiResponse<List<EmployeeDetailsDto>> { Message = ResponseMessages.EmployeeList, Result = result });
                }
                return Ok(new ApiResponse<object> { Message = ResponseMessages.NoEmployees, Result = null });
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
        [HttpGet("EmployeeNames")]
        public async Task<IActionResult> GetEmployeeNames()
        {
            try
            {
                List<EmployeeNames> result = await _employeeRepository.GetEmployeeNames();
                if (result.Count != 0)
                {
                    return Ok(new ApiResponse<List<EmployeeNames>> { Message = "List of Employees", Result = result });
                }
                return Ok(new ApiResponse<object> { Message = "No employees", Result = null });
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
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            try
            {
                EmployeeDto? result = await _employeeRepository.GetEmployeeById(id);
                if (result != null)
                {
                    return Ok(new ApiResponse<EmployeeDto> { Message = "Employee Details", Result = result });
                }
                return NotFound(new ApiResponse<object> { Message = "Employee Not found", Result = null });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterDto inputModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<object> { Message = ResponseMessages.DataFormat });
                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
                bool result = await _employeeRepository.RegisterEmployee(userId, inputModel);

                if (result)
                {
                    return Ok(new ApiResponse<bool> { Message = "User Created succesfully", Result = result });
                }
                else
                {
                    return Ok(new ApiResponse<object> { Message = "User Already exist", Result = null });
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
        /// <param name="updatedEmployee"></param>
        /// <returns></returns>
        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] UpdateEmployeeDto updatedEmployee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<object> { Message = ResponseMessages.DataFormat });

                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
                bool result = await _employeeRepository.UpdateEmployee(id, userId, updatedEmployee);
                if (result)
                {
                    return Ok(new ApiResponse<string> { Message = "User updated succesfully" });
                }
                else
                {
                    return Ok(new ApiResponse<string> { Message = "Unable to update employee" });
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
        /// <returns></returns>
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            try
            {
                bool result = await _employeeRepository.DeleteEmployee(id);
                if (!result)
                {
                    return Ok(new ApiResponse<object> { Message = "Unable to delete employee" });
                }
                else
                {
                    return Ok(new ApiResponse<object> { Message = "User Deleted succesfully" });
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
        /// <param name="photo"></param>
        /// <returns></returns>
        [HttpPost("AddProfilephoto")]
        [Authorize]
        public async Task<IActionResult> UpdateProfilePhoto([FromForm] IFormFile photo)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string username = User.FindFirstValue(ClaimTypes.Name);
            List<string> fileTypes = new List<string>() { "jpg", "png", "jpeg" };

            if (photo == null || photo.Length == 0)
            {
                return BadRequest("Please provide a valid photo");
            }
            else if (!fileTypes.Contains(Path.GetExtension(photo.FileName)))
            {
                return BadRequest("Incorrect file type. File should be jpg/jpeg/png");
            }
            else
            {

                bool upload = await _employeeRepository.UploadProfilePhoto(username, userId, photo);
                if (upload)
                {
                    return Ok("Photo uploaded successfully");
                }
                else
                {
                    return Ok("Unable to update photo");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpDelete("RemoveProfilePhoto/{empId}")]
        public async Task<IActionResult> RemoveProfilePhoto(string empId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool result = await _employeeRepository.RemoveProfilePhoto(empId, userId);
            if (result)
            {
                return Ok("Photo removed successfully");
            }
            else
            {
                return Ok("Unable to remove photo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCount")]
        public async Task<IActionResult> GetCount()
        {
            CountDto count = await _employeeRepository.GetCount();
            return Ok(new ApiResponse<CountDto> { Message = "Count of employees, pending declaration and pending salary details", Result = count });
        }
    }
}