using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Database
{
    public class ReportEntity : EntityBase
    {
        public string Heading { get; set; }
        public string Message { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime Date => DateTime.Now;

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public List<ResponseEntity> Responses { get; set; }
        public List<ModificationEntryEntity> ModificationEntries { get; set; }
    }
}
