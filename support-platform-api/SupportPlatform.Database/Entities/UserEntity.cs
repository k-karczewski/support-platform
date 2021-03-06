﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SupportPlatform.Database
{
    public class UserEntity : IdentityUser<int>
    {
        public ICollection<UserRoleEntity> UserRoles { get; set; }
        public ICollection<ReportEntity> Reports { get; set; }
        public ICollection<ResponseEntity> Responses { get; set; }
    }
}
