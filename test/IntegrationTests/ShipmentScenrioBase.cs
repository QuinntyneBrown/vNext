namespace IntegrationTests
{
    public class ShipmentScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Shipments = "api/shipments";

            public static string GetById(int id)
            {
                return $"api/shipments/{id}";
            }
        }

        public static class Post
        {
            public static string Shipments = "api/shipments";
        }

        public static class Delete
        {
            public static string Shipment(int id)
                => $"api/shipments/{id}";
        }
    }
}
