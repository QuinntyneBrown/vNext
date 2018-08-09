namespace IntegrationTests
{
    public class ConcurrencyScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Concurrencies = "api/concurrencies";

            public static string GetById(int id)
            {
                return $"api/concurrencies/{id}";
            }

            public static string GetVersionByDomainAndIdQuery(int version, string domain, int id)
            {
                return $"api/concurrencies/domain/{domain}/id/{id}/version/{version}";
            }
        }

        public static class Post
        {
            public static string Concurrencies = "api/concurrencies";
        }

        public static class Delete
        {
            public static string Concurrency(int id)
                => $"api/concurrencies/{id}";

            public static string Truncate = "api/concurrencies";
        }
    }
}
