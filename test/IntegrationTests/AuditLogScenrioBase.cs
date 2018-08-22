namespace IntegrationTests
{
    public class AuditLogScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string AuditLogs = "api/auditlogs";

            public static string GetById(int id)
            {
                return $"api/auditlogs/{id}";
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
