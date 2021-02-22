using SupportPlatform.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportDetailsToReturnDto
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public StatusEnum Status { get; set; }
        public string Date { get; set; }
        public List<ResponseToReturnDto> Responses { get; set; }
        public List<ModificationEntryToReturnDto> ModificationEntries { get; set; }
        public AttachmentToReturnDto Attachment { get; set; }
    }
}
