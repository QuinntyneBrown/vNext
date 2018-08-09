namespace IntegrationTests
{
    public class WarehouseScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Warehouses = "api/warehouses";

            public static string GetById(int id)
            {
                return $"api/warehouses/{id}";
            }
        }

        public static class Post
        {
            public static string Warehouses = "api/warehouses";
        }

        public static class Delete
        {
            public static string Warehouse(int id)
                => $"api/warehouses/{id}";
        }
    }
}
