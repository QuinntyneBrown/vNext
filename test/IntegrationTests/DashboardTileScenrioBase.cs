namespace IntegrationTests
{
    public class DashboardTileScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string DashboardTiles = "api/dashboardtiles";

            public static string GetById(int id)
            {
                return $"api/dashboardtiles/{id}";
            }
        }

        public static class Post
        {
            public static string DashboardTiles = "api/dashboardtiles";
        }

        public static class Delete
        {
            public static string DashboardTile(int id, int concurrencyVersion)
                => $"api/dashboardtiles/{id}/{concurrencyVersion}";
        }
    }
}
