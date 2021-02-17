using System;

namespace SupportPlatform.Database
{
    public class ResponseEntity : EntityBase
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public int ReportId { get; set; }
        public ReportEntity Report { get; set; }

        public int UserId { get; set; }
        public UserEntity User {get;set;}
    }
}
