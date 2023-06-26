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

        [HttpGet("AllTaxDetails")]
        public async Task<IActionResult> GetAllTaxDetails()
        {
            List<IGrouping<int, TaxDetailsDto>> taxDetails = await _taxDetailsRepository.GetAllTaxDetails();
            return Ok(new ApiResponse<List<IGrouping<int, TaxDetailsDto>>> { Message = "List of employees tax details"});
        }

        [HttpGet("TaxDetails/{empId}")]
        public async Task<IActionResult> GetTaxDetails(string empId)
        {
            List<TaxDetailsDto> taxDetails = await _taxDetailsRepository.GetTaxDetails(empId);
            return Ok(new ApiResponse<List<TaxDetailsDto>> { Message = "List of tax details of employee", Result = taxDetails });
        }

        [HttpGet("TaxDetailsByYear/{empId}/{yearId}")]
        public async Task<IActionResult> GetTaxDetailsByYear(string empId, int yearId)
        {
            TaxDetailsDto? taxDetails = await _taxDetailsRepository.GetTaxDetailsByYear(empId, yearId);

            if (taxDetails == null)
            {
                return NotFound(new ApiResponse<object> { Message = "Unable to get tax details" });
            }
            else
                return Ok(new ApiResponse<TaxDetailsDto> { Message = "EMployee tax details", Result = taxDetails });
        }

        [HttpGet("AllTaxDetailsByYear/{yearId}")]
        public async Task<IActionResult> GetAllTaxDetailsByYear(int yearId)
        {
            List<TaxDetailsDto> taxDetails = await _taxDetailsRepository.GetAllTaxDetailsByYear(yearId);
            return Ok(new ApiResponse<List<TaxDetailsDto>> { Message = "List of tax details", Result = taxDetails });
        }

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
