namespace IntegrationTests
{
    public class DivisionScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Divisions = "api/divisions";

            public static string GetById(int id)
            {
                return $"api/divisions/{id}";
            }
        }

        public static class Post
        {
            public static string Divisions = "api/divisions";
        }

        public static class Delete
        {
            public static string Division(int id)
                => $"api/divisions/{id}";
        }
    }
}
