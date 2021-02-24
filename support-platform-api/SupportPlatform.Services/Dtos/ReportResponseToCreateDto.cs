using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportResponseToCreateDto
    {
        [Required]
        public int ReportId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
