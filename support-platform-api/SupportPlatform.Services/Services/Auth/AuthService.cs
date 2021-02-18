﻿using Microsoft.AspNetCore.Identity;
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
        public async Task<IServiceResult> RegisterAsync(UserToRegisterDto userToRegister)
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
                    await SendConfirmationEmail(user);

                    return new ServiceResult(ResultType.Correct);
                }

                // user registration failed - no user has been added to database
                return new ServiceResult(ResultType.Failed, GetErrorsFromResult(signUpResult));
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

        public async Task<IServiceResult> ConfirmEmailAsync(AccountToConfirmDto accountToConfirm)
        {
            UserEntity user = await _signInManager.UserManager.FindByIdAsync(accountToConfirm.UserId);

            if(user.EmailConfirmed == false)
            {
                string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(accountToConfirm.Token));

                IdentityResult confirmationResult = await _signInManager.UserManager.ConfirmEmailAsync(user, decodedToken);

                if (confirmationResult.Succeeded)
                {
                    return new ServiceResult(ResultType.Correct);
                }

                return new ServiceResult(ResultType.Failed, GetErrorsFromResult(confirmationResult));
            }

            return new ServiceResult(ResultType.Failed, new List<string> { "Ten adres email został już potwierdzony" });
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
        private async Task SendConfirmationEmail(UserEntity user) 
        {
            // generate confitmation token for new user
            string confirmationToken = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            confirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

            // create confirmation url which will be sent by email
            // get link to client application
            string callbackUrl = GetCallbackUrl(user.Id, confirmationToken);

            // create email sender instance
            using (var emailSender = new EmailSender(_configuration))
            {
                // send email
                await emailSender.SendAccountConfirmation(user, callbackUrl);
            }
        }

        private List<string> GetErrorsFromResult(IdentityResult result)
        {
            List<string> errors = new List<string>();
            errors.AddRange(result.Errors.Select(x => x.Description));

            return errors;
        }

        private string GetCallbackUrl(int userId, string token)
        {
            string clientAppBaseUrl = _configuration.GetSection("ClientAppSettings:ClientAppUrl").Value;
            return $"{clientAppBaseUrl}confirmInProgress?userId={userId}&token={token}";
        }
    }
}
