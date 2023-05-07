using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using EmployeeTaxCalculation.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaxCalculation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        public readonly ISectionRepository _subSectionRepository;
        public SectionController(ISectionRepository subSectionRepository)
        {
            _subSectionRepository = subSectionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<SectionDto>? result = await _subSectionRepository.GetSections();
                if (result != null)
                {
                    return Ok(new ApiResponse<List<SectionDto>> { StatusCode = 200, Message = "List of SubSections", Result = result });
                }
                return Ok(new ApiResponse<object> { StatusCode = 200, Message = "No data available" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
