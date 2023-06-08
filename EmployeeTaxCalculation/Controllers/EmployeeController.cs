using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using EmployeeTaxCalculation.Data.DTOs;

namespace EmployeeTaxCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                List<EmployeeDto> result = await _employee.GetEmployees();
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

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployeesDetails()
        {
            try
            {
                List<EmployeeDetailsDto>? result = await _employee.GetEmpoyeesDetails();
                if (result != null)
                {
                    return Ok(new ApiResponse<Object> { StatusCode = 200, Message = "List of Employees", Result = result });
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "No employees" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllPending()
        //{
        //    try
        //    {
        //        List<EmployeeDto> result = await _employee.GetEmployees();
        //        if (result != null)
        //        {
        //            return Ok(new ApiResponse<List<EmployeeDto>> { StatusCode = 200, Message = "List of Employees", Result = result });
        //        }
        //        return Ok(new ApiResponse<object> { StatusCode = 200, Message = "No employees" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                EmployeeDto? result = await _employee.GetEmployeeById(id);
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

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> Post([FromBody] RegisterDto inputModel)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string result = await _employee.RegisterEmployee(userId, inputModel);

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
                    return Ok(new ApiResponse<string> { StatusCode = 200, Message = "User Created succesfully", Result = result });
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
                if (result == "0")
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

        // Profile Photo
        [HttpPost("profilephoto")]
        [Authorize]
        public async Task<IActionResult> UpdateProfilePhoto([FromForm] IFormFile photo)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string username = User.FindFirstValue(ClaimTypes.Name);

            if (photo == null || photo.Length == 0)
            {
                return BadRequest("Please provide a valid photo");
            }

            bool? upload = await _employee.UploadProfile(username, userId, photo);
            if (upload == true)
            {
                return Ok();
            }
            else if(upload == null)
            {
                return NotFound();
            }
            else
            {
                return Ok("Error");
            }
        }

        //[HttpGet("profilephoto/{userId}")]
        //public async Task<IActionResult> GetProfilePhoto(string userId)
        //{
        //    var profile = await _context.Profiles.SingleOrDefaultAsync(p => p.UserId == userId);

        //    if (profile == null)
        //    {
        //        return NotFound("User profile not found");
        //    }

        //    if (string.IsNullOrEmpty(profile.PhotoUrl))
        //    {
        //        return NotFound("Profile photo not found");
        //    }

        //    string photoPath = Path.Combine(Directory.GetCurrentDirectory(), profile.PhotoUrl);
        //    var photoBytes = await System.IO.File.ReadAllBytesAsync(photoPath);

        //    return File(photoBytes, "image/jpeg");
        //}
    }
}