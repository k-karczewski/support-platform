using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportListParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }  = 10;
        public int? StatusFilter { get; set; }  = null;
    }
}
