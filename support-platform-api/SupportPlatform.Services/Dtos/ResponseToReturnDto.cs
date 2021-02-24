using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ResponseToReturnDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public string CreatedBy { get; set; }
        public ModificationEntryToReturnDto ModificationEntry { get;  set; }
    }
}
