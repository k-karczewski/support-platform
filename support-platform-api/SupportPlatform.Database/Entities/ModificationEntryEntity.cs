using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Database
{
    public class ModificationEntryEntity : EntityBase
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public int ReportId { get; set; }
        public ReportEntity Report { get; set; }
    }
}
