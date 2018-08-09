using System;

namespace vNext.Core.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string Operation { get; set; }
        public string Domain { get; set; }
        public int UserId { get; set; }
        public DateTime AuditDateTime { get; set; }
        public DateTime AuditDate { get; set; }
        public int Status { get; set; }
        public string Info { get; set; }
        public string Id { get; set; }
        public int NoteId { get; set; }
        public Note Note { get; set; }
        public User User { get; set; }
    }
}
