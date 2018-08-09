namespace IntegrationTests
{
    public class StatusScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Statuses = "api/statuses";

            public static string GetById(int id)
            {
                return $"api/statuses/{id}";
            }
        }

        public static class Post
        {
            public static string Statuses = "api/statuses";
        }

        public static class Delete
        {
            public static string Status(int id)
                => $"api/statuses/{id}";
        }
    }
}
