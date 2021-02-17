using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using SupportPlatform.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserEntityMapper _userMapper;
        private readonly IConfiguration _configuration;

        public AuthService(SignInManager<UserEntity> signInManager, RoleManager<IdentityRole<int>> roleManager, UserEntityMapper userMapper, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userMapper = userMapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Registers new user to the application
        /// </summary>
        /// <param name="userToRegister">Data of new user</param>
        /// <param name="password">Password of new user</param>
        /// <returns>Result state</returns>
        public async Task<IServiceResult> RegisterAsync(UserToRegisterDto userToRegister, IUrlHelper urlHelper)
        {
            try
            {
                userToRegister.Username = userToRegister.Username.ToLower();
                UserEntity user = _userMapper.Map(userToRegister);

                IdentityResult signUpResult = await _signInManager.UserManager.CreateAsync(user, userToRegister.Password);

                if(signUpResult.Succeeded)
                {
                    // assign role to newly registered user
                    await AssignClientRole(user);

                    // send confirmation email
                    await SendConfirmationEmail(user, urlHelper);

                    return new ServiceResult(ResultType.Correct);
                }

                // user registration failed - no user has been added to database
                List<string> errors = new List<string>();
                errors.AddRange(signUpResult.Errors.Select(x => x.Description));

                return new ServiceResult(ResultType.Failed, errors);
            }
            catch(Exception e)
            {
                // in case of role assignment or confirmation email sending failure
                // remove new user from database - procedure has not been completed
                UserEntity user = await _signInManager.UserManager.FindByNameAsync(userToRegister.Username);

                if(user != null)
                {
                    await _signInManager.UserManager.DeleteAsync(user);
                }

                return new ServiceResult(ResultType.Error, new List<string> { e.Message });
            }
        }

        private async Task AssignClientRole(UserEntity user)
        {
            string roleName = "Client";

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>
                {
                    Name = roleName
                });
            }

            await _signInManager.UserManager.AddToRoleAsync(user, roleName);
        }

        /// <summary>
        /// Sends email confirmation message to newly registered user using EmailSender class
        /// </summary>
        /// <param name="user">Newly registered user</param>
        /// <param name="urlHelper">Url helper used for generating of confirmation link</param>
        private async Task SendConfirmationEmail(UserEntity user, IUrlHelper urlHelper) 
        {
            // generate confitmation token for new user
            string confirmationToken = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            confirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

            // create confirmation url which will be sent by email
            // temporary link to client application
            string callbackUrl = $"http://localhost:3000/?{user.Id}&{confirmationToken}";
            //string callbackUrl2 = urlHelper.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = confirmationToken }, "https");

            // create email sender instance
            using(var emailSender = new EmailSender(_configuration))
            {
                // send email
                await emailSender.SendAccountConfirmation(user, callbackUrl);
            }
        }
    }
}
