namespace IntegrationTests
{
    public class AuditLogScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string AuditLogs(string domain, int id, int userId,string fromAuditDate,string toAuditDate)
            {
                return $"api/auditlogs/{domain}/{id}/{userId}/{fromAuditDate}/{toAuditDate}";
            }
        }

        public static class Post
        {
            public static string AuditLogs = "api/auditlogs";
        }

        public static class Delete
        {
            public static string AuditLog(int id)
                => $"api/auditlogs/{id}";
        }
    }
}
