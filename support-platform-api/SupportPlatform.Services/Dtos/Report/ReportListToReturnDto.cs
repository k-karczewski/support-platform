using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportListToReturnDto
    {
        public int TotalPages { get; set; }
        public List<ReportListItemToReturnDto> ReportListItems { get; set; }
    }
}
