using vNext.Core.Models;

namespace vNext.API.Features.AuditLogs
{
    public class AuditLogDto
    {        
        public int AuditLogId { get; set; }
        public string Code { get; set; }

        public static AuditLogDto FromAuditLog(dynamic auditLog)
            => new AuditLogDto
            {
                AuditLogId = auditLog.AuditLogId,
                Code = auditLog.Code
            };
    }
}
