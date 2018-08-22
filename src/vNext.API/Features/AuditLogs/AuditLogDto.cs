using System;
using vNext.API.Features.Notes;
using vNext.Core.Models;

namespace vNext.API.Features.AuditLogs
{
    public class AuditLogDto
    {        
        public int AuditLogId { get; set; }
        public string Operation { get; set; }
        public string Domain { get; set; }
        public int UserId { get; set; }
        public DateTime AuditDateTime { get; set; }
        public short Status { get; set; }
        public string Info { get; set; }
        public int NoteId { get; set; }
        public NoteDto Note { get; set; }
        public int Id { get; set; }

        public static AuditLogDto FromAuditLog(dynamic auditLog)
            => new AuditLogDto
            {
                AuditLogId = auditLog.AuditLogId,
                Operation = auditLog.Operation,
                Domain = auditLog.Domain,
                UserId = auditLog.UserId,
                AuditDateTime = auditLog.AuditDateTime,
                Status = auditLog.Status,
                Info = auditLog.Info,
                NoteId = auditLog.NoteId,
                Id = auditLog.Id,
                Note =new NoteDto() {  Note = auditLog.Note }
            };
    }
}
