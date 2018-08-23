namespace IntegrationTests
{
    public class SalesOrderScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string SalesOrders = "api/salesorders";

            public static string GetById(int id)
            {
                return $"api/salesorders/{id}";
            }
        }

        public static class Post
        {
            public static string SalesOrders = "api/salesorders";
        }

        public static class Delete
        {
            public static string SalesOrder(int id)
                => $"api/salesorders/{id}";
        }
    }
}
