namespace IntegrationTests
{
    public class TileScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Tiles = "api/tiles";

            public static string GetById(int id)
            {
                return $"api/tiles/{id}";
            }
        }

        public static class Post
        {
            public static string Tiles = "api/tiles";
        }

        public static class Delete
        {
            public static string Tile(int id)
                => $"api/tiles/{id}";
        }
    }
}
