using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportStatusToUpdateDto
    {
        [Required]
        public int ReportId { get; set; }
        [Required]
        public int NewStatus { get; set; }
    }
}
