using EmployeeTaxCalculation.Constants;
using EmployeeTaxCalculation.Data.DTOs;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticateController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IAuthenticationRepository authenticationRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _authenticationRepository = authenticationRepository;
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns object with token, expiration, role and user id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Authentication/Login
        ///     {
        ///        "username": "string",
        ///        "password": "string"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns object with token, expiration, role and user id</response>
        /// <response code="400">Entered data is not in correct format</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object> { Message = ResponseMessages.DataFormat });
            try
            {
                object? result = await _authenticationRepository.Login(model);
                if (result != null)
                {
                    return Ok(new ApiResponse<object> { Message = ResponseMessages.LoginSuccessful, Result = result });
                }
                else
                    return Unauthorized(new ApiResponse<object> { Message = ResponseMessages.WrongCredentials });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Check whether user with this email exist or not 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns true if user with email present</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Authentication/IsEmailExist/{email}
        ///     
        /// </remarks>
        /// <response code="200">Returns true if user with email present</response>
        /// <response code="404">Returns false if user with email is not present</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("IsEmailExist/{email}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsEmailExist(string email)
        {
            try
            {
                bool result = await _authenticationRepository.IsEmailExist(email);
                if (result)
                {
                    return Ok(new ApiResponse<bool> { Message = ResponseMessages.UserExistWithEmail, Result = result });
                }
                else
                {
                    return NotFound(new ApiResponse<bool> { Message = ResponseMessages.UserNotExistWithEmail, Result = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Check whether user with this username exist or not 
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns true if user with username present</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Authentication/IsUsernameExist/{username}
        ///     
        /// </remarks>
        /// <response code="200">Returns true if user with username present</response>
        /// <response code="404">Returns false if user with username is not present</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("IsUsernameExist/{username}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IsUsernameExist(string username)
        {
            try
            {
                bool result = await _authenticationRepository.IsEmailExist(username);
                if (result)
                {
                    return Ok(new ApiResponse<bool> { Message = ResponseMessages.UserExistWithUsername, Result = result });
                }
                else
                {
                    return NotFound(new ApiResponse<bool> { Message = ResponseMessages.UserNotExistWithUsername, Result = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}