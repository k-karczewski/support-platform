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
                IServiceResult registrationResult = await _authService.RegisterAsync(userToRegister);

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

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(AccountToConfirmDto accountToConfirm)
        {
            if(ModelState.IsValid)
            {
                IServiceResult confirmationResult = await _authService.ConfirmEmailAsync(accountToConfirm);

                if(confirmationResult.Result == ResultType.Correct)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(confirmationResult.Errors);
                }
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }
    }
}
