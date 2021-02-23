using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportListOptionsDto
    {
        public int PageNumber { get; set; } = 0;
        public int ItemsPerPage { get; set; } = 10;
        public int ReportStatus { get; set; } = 0;
    }
}
