using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IAuthService
    {
        Task<IServiceResult> RegisterAsync(UserToRegisterDto userToRegister, IUrlHelper urlHelper);
    }
}
