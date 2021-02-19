using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Database
{
    public class RoleEntity : IdentityRole<int>
    {
        public ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
