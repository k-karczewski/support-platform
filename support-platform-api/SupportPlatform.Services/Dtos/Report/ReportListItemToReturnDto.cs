using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ReportListItemToReturnDto
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string CreatedBy { get; set; }
        public string Date { get; set; }
        public int Status { get; set; }
    }
}
