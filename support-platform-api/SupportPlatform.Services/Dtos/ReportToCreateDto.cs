using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportToCreateDto
    {
        [Required]
        public string Heading { get; set; }
        [Required]
        public string Message { get; set; }
        public FileToUploadDto File { get; set; }
    }
}
