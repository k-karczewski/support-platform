using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportToCreateDto
    {
        public string Heading { get; set; }
        public string Message { get; set; }
        public byte[] FileInBytes { get; set; }
    }
}
