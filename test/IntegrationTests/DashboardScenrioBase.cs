namespace IntegrationTests
{
    public class DashboardScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Dashboards = "api/dashboards";

            public static string GetById(int id)
            {
                return $"api/dashboards/{id}";
            }

            public static string GetDashboardsByUserId(int id)
            {
                return $"api/dashboards/user/{id}";
            }
        }

        public static class Post
        {
            public static string Dashboards = "api/dashboards";
        }

        public static class Delete
        {
            public static string Dashboard(int id)
                => $"api/dashboards/{id}";
        }
    }
}
