namespace IntegrationTests
{
    public class RegionScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string All = "api/regions";

            public static string GetById(int id)
            {
                return $"api/regions/{id}";
            }
        }

        public static class Post
        {
            public static string Regions = "api/regions";
        }

        public static class Delete
        {
            public static string Region(int id, int concurrencyVersion)
                => $"api/regions/{id}/{concurrencyVersion}";
        }
    }
}
