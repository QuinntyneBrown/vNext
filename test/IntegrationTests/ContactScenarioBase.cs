namespace IntegrationTests
{
    public class ContactScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Contacts = "api/contacts";

            public static string ContactById(int id)
            {
                return $"api/contacts/{id}";
            }
        }

        public static class Post
        {
            public static string Contacts = "api/contacts";
        }

        public static class Delete
        {
            public static string Contact(int id,int concurrencyVersion)
                => $"api/contacts/{id}/{concurrencyVersion}";
        }
    }
}
