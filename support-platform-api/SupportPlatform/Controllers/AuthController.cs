using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
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
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserToLoginDto userToLogin)
        {
            if (ModelState.IsValid)
            {
                IServiceResult<string> loginResult = await _authService.LoginAsync(userToLogin);

                if (loginResult.Result == ResultType.Correct)
                {
                    return Ok( new { username=userToLogin.Username, token = loginResult.ReturnedObject });
                }
                else
                {
                    return Unauthorized(loginResult.Errors);
                }
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
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
