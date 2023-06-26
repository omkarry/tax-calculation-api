using EmployeeTaxCalculation.Constants;
using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminController(IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor)
        {
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all admins
        /// </summary>
        /// <returns>Returns list of admins</returns>
        /// <remarks>
        /// Sample Request:
        ///     
        ///     GET /api/Admin/Admins
        /// 
        /// </remarks>
        /// <response code="200">List of admins</response>
        [HttpGet("Admins")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdmins()
        {
            List<AdminDto> adminList = await _adminRepository.GetAdmins();
            return Ok(new ApiResponse<List<AdminDto>> { Message = ResponseMessages.AdminList, Result = adminList });
        }

        /// <summary>
        /// Get admin details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return admin details</returns>
        /// <remarks>
        /// Sample Request:
        ///     
        ///     GET /api/Admin/{id}
        /// 
        /// </remarks>
        /// <response code="200">Admin details</response>
        /// <response code="404">Admin not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdminById(string id)
        {
            AdminDto result = await _adminRepository.GetAdmin(id);
            if(result == null)
            {
                return NotFound(new ApiResponse<AdminDto> { Message = ResponseMessages.AdminNotFound, Result = result });
            }
            return Ok(new ApiResponse<AdminDto> { Message = ResponseMessages.AdminDetails, Result = result});
        }

        /// <summary>
        /// Register new admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns true if admin created successfully</returns>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /api/Admin/RegisterAdmin
        ///     {
        ///         "name": "string",
        ///         "username": "string",
        ///         "email": "string",
        ///         "password": "string"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Returns message whether admin is created or not</response>
        /// <response code="400">Returns error if input data is not in correct format</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("RegisterAdmin")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAdmin(RegisterDto model)
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<object?> { Message = ResponseMessages.DataFormat });
                bool result = await _adminRepository.RegisterAdmin(userId, model);
                if (result)
                {
                    return Ok(new ApiResponse<object> { Message = ResponseMessages.AdminRegistered });
                }
                else
                    return Ok(new ApiResponse<object> { Message = ResponseMessages.AdminNotCreated });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update admin
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="model"></param>
        /// <returns>Returns true if admin updated successfully</returns>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     UPDATE /api/Admin/UpdateAdmin/{adminId}
        ///     {
        ///         "name": "string",
        ///         "username": "string",
        ///         "email": "string",
        ///         "gender": "string",
        ///         "dob": "String"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Returns message whether admin is updated or not</response>
        /// <response code="400">Returns error if input data is not in correct format</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("UpdateAdmin/{adminId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdmin(string adminId, UpdateEmployeeDto model)
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<object?> { Message = ResponseMessages.DataFormat });
                bool result = await _adminRepository.UpdateAdmin(userId, adminId, model);
                if (result)
                {
                    return Ok(new ApiResponse<object> { Message = ResponseMessages.AdminUpdated});
                }
                else
                    return Ok(new ApiResponse<object> { Message = ResponseMessages.AdminNotUpdated});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes admin
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns>Return true if admin is deleted successfully</returns>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     DELETE /api/Admin/DeleteAdmin/{adminId}
        ///      
        /// </remarks>
        /// <response code="200">Returns message whether admin is deleted or not</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("DeleteAdmin/{adminId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAdmin(string adminId)
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
                bool result = await _adminRepository.DeleteAdmin(userId, adminId);
                if (result)
                {
                    return Ok(new ApiResponse<object> { Message = ResponseMessages.AdminDeleted});
                }
                else
                    return Ok(new ApiResponse<object> { Message = ResponseMessages.AdminNotDeleted});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
