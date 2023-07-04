using EmployeeTaxCalculation.Constants;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxDetailsController : ControllerBase
    {
        private readonly ITaxDetailsRepository _taxDetailsRepository;
        public TaxDetailsController(ITaxDetailsRepository taxDetailsRepository)
        {
            _taxDetailsRepository = taxDetailsRepository;
        }

        /// <summary>
        /// Get all tax details
        /// </summary>
        /// <returns>Returns all tax details</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET /api/TaxDetails/AllTaxDetails
        /// 
        /// </remarks>
        /// <response code="200">Returns list of tax details</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("AllTaxDetails")]
        [ProducesResponseType(typeof(List<IGrouping<int, TaxDetailsDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTaxDetails()
        {
            try
            {
                List<IGrouping<int, TaxDetailsDto>> taxDetails = await _taxDetailsRepository.GetAllTaxDetails();
                return Ok(new ApiResponse<List<IGrouping<int, TaxDetailsDto>>> { Message = ResponseMessages.TaxDetailsList, Result = taxDetails });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get Tax details by employee id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns>List of tax details of employee</returns>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     GET /api/TaxDetails/{empId}
        /// 
        /// </remarks>
        /// <response code="200">Returns list of tax details</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{empId}")]
        [ProducesResponseType(typeof(List<TaxDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTaxDetails(string empId)
        {
            try
            {
                List<TaxDetailsDto> taxDetails = await _taxDetailsRepository.GetTaxDetails(empId);
                return Ok(new ApiResponse<List<TaxDetailsDto>> { Message = ResponseMessages.TaxDetailsList, Result = taxDetails });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get tax details by employee id and year
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="yearId"></param>
        /// <returns>Tax details of employee for year</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/TaxDetails/TaxDetailsByYear/{empId}/{yearId}
        ///     
        /// </remarks>
        /// <response code="200">Returns employee's tax details by year</response>
        /// <response code="404">Tax details not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("TaxDetailsByYear/{empId}/{yearId}")]
        [ProducesResponseType(typeof(TaxDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTaxDetailsByYear(string empId, int yearId)
        {
            try
            {
                TaxDetailsDto? taxDetails = await _taxDetailsRepository.GetTaxDetailsByYear(empId, yearId);

                if (taxDetails == null)
                {
                    return NotFound(new ApiResponse<object> { Message = ResponseMessages.UnableToGetTaxDetails });
                }
                else
                    return Ok(new ApiResponse<TaxDetailsDto> { Message = ResponseMessages.EmployeeTaxDetails, Result = taxDetails });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get tax details by year
        /// </summary>
        /// <param name="yearId"></param>
        /// <returns>Returns list of tax details for year</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/TaxDetails/AllTaxDetailsByYear/{yearId}
        ///     
        /// </remarks>
        /// <response code="200">Returns list of tax details for a year</response>
        /// <response code="500">Internal Server error</response>
        [HttpGet("AllTaxDetailsByYear/{yearId}")]
        [ProducesResponseType(typeof(List<TaxDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTaxDetailsByYear(int yearId)
        {
            try
            {
                List<TaxDetailsDto> taxDetails = await _taxDetailsRepository.GetAllTaxDetailsByYear(yearId);
                return Ok(new ApiResponse<List<TaxDetailsDto>> { Message = ResponseMessages.TaxDetailsList, Result = taxDetails });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Add tax details
        /// </summary>
        /// <param name="taxDetails"></param>
        /// <returns></returns>
        [HttpPost("AddTaxDetails")]
        public async Task<IActionResult> AddTaxDetails(TaxDetailsDto taxDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseMessages.DataFormat);
            bool result = await _taxDetailsRepository.AddTaxDetails(taxDetails);
            if (result)
                return Ok(new ApiResponse<object> { Message = "Tax details added successfully" });
            else
                return Ok(new ApiResponse<object> { Message = "Unable to add tax details" });
        }

        [HttpPut("UpdateTaxDetails")]
        public async Task<IActionResult> UpdateTaxDetails(TaxDetailsDto taxDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseMessages.DataFormat);
            bool result = await _taxDetailsRepository.UpdateTaxDetails(taxDetails);
            if (result)
                return Ok(new ApiResponse<object> { Message = "Tax details updated successfully" });
            else
                return Ok(new ApiResponse<object> { Message = "Unable to update tax details" });
        }

        [HttpDelete("DeleteTaxDetails/{empId}/{yearId}")]
        public async Task<IActionResult> DeleteTaxDetails(string empId, int yearId)
        {
            bool result = await _taxDetailsRepository.DeleteTaxDetails(empId, yearId);
            if (result)
                return Ok(new ApiResponse<object> { Message = "Tax details deleted successfully" });
            else
                return Ok(new ApiResponse<object> { Message = "Unable to delete tax details" });
        }

        [HttpGet("PendingTaxDeclaration")]
        public async Task<IActionResult> PendingTaxDeclaration()
        {
            List<EmployeeNames> employees = await _taxDetailsRepository.PendingTaxDeclaration();
            return Ok(new ApiResponse<List<EmployeeNames>> { Message = "List of employees with pending declaration" });
        }
    }
}
