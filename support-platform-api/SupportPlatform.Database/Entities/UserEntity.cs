using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SupportPlatform.Database
{
    public class UserEntity : IdentityUser<int>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public List<ReportEntity> Reports { get; set; }
        public List<ResponseEntity> Responses { get; set; }
    }
}
