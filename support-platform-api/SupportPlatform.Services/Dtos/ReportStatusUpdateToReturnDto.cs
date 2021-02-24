using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportStatusUpdateToReturnDto
    {
        public int Status { get; set; }
        public List<ModificationEntryToReturnDto> ModificationEntries { get; set; }
    }
}
