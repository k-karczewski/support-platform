using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Database
{
    public class UserRoleEntity : IdentityUserRole<int>
    {
        public UserEntity User { get; set; }
        public RoleEntity Role { get; set; }
    }
}
