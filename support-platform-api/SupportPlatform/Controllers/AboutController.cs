using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportPlatform.Controllers
{
    /// <summary>
    /// This controller is not part of the application
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AboutController : ControllerBase
    {
        [HttpGet]
        public IActionResult About()
        {
            return Ok(new 
            { 
                Firstname = "Konrad",
                LastName = "Karczewski",
                Repository = "https://github.com/k-karczewski/support-platform"
            });
        }

    }
}
