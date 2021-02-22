using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SupportPlatform.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }


        [HttpGet(Name = "GetReportForClient")]
        [Authorize(Policy = "RequireClientRole")]
        public async Task<IActionResult> GetReportForClientAsync(int id)
        {
            if(ModelState.IsValid)
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                IServiceResult<ReportDetailsToReturnDto> result = await _reportService.GetReportDetailsForClientAsync(id, userId);

                if(result.Result == ResultType.Correct)
                {
                    return Ok(result.ReturnedObject);
                }

                return BadRequest();
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }
        

        [HttpPost("create")]
        [Authorize(Policy = "RequireClientRole")]
        public async Task<IActionResult> CreateAsync(ReportToCreateDto reportToCreate)
        {
            if(ModelState.IsValid)
            {
                IServiceResult<ReportDetailsToReturnDto> result = await _reportService.CreateAsync(reportToCreate, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));

                if(result.Result == ResultType.Correct)
                {
                    return CreatedAtRoute("GetReportForClient", new { id = result.ReturnedObject.Id}, result.ReturnedObject);
                }
                else if(result.Result == ResultType.Unauthorized)
                {
                    return Unauthorized();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }
    }
}
