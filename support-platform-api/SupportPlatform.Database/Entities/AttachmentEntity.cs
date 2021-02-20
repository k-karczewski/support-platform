using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Database
{
    public class AttachmentEntity : EntityBase
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public int ReportId { get; set; }
        public ReportEntity Report { get; set; }
    }
}
