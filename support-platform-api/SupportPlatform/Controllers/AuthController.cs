using Microsoft.AspNetCore.Mvc;
using SupportPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportPlatform.Controllers
{
    [ApiController]
    [Route("api/auth/")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(UserToRegisterDto userToRegister)
        {
            if(ModelState.IsValid)
            {
                IServiceResult registrationResult = await _authService.RegisterAsync(userToRegister, Url);

                if(registrationResult.Result == ResultType.Correct)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(registrationResult.Errors);
                }
            }

            return BadRequest();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(int userId, string token)
        {
            return Ok();
        }
    }
}
