using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SectionController : ControllerBase
    {
        public readonly ISectionRepository _subSectionRepository;
        public SectionController(ISectionRepository subSectionRepository)
        {
            _subSectionRepository = subSectionRepository;
        }

        [HttpPut("UpdateSubSctionLimit/{subSectionId}")]
        public async Task<IActionResult> UpdateSubSectionLimit(int subSectionId, [FromQuery] decimal limit)
        {
            bool result = await _subSectionRepository.UpdateSubSectionLimit(subSectionId, limit);
            if (result)
                return Ok(new ApiResponse<object> { Message = "SubSection limit updated successfully", Result = result });
            else
                return Ok(new ApiResponse<object> { Message = "unable to update limit" });
        }

        [HttpGet("Sections")]
        public async Task<IActionResult> GetAllSections()
        {
            try
            {
                List<SectionDto> result = await _subSectionRepository.GetSections();
                return Ok(new ApiResponse<List<SectionDto>> { Message = "List of SubSections", Result = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
